using UnityEngine;
using UnityEditor;
using System.Collections;
using LitJson;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Generic;

[CustomEditor(typeof(DKUMASaveTool))]
[CanEditMultipleObjects]
public class DKUMASaveToolEditor : Editor {
	
	public SerializedProperty avatarName;
	public SerializedProperty serializedAvatar;
	DKUMADnaHumanoid umaDna;
	float DNAValue;
	public string streamedUMA;


    void OnEnable () {
        avatarName = serializedObject.FindProperty ("avatarName");

		
    }
	
	
	public override void OnInspectorGUI(){	
		serializedObject.Update();
		
		GUILayout.Label ("Avatar Name", EditorStyles.boldLabel);
		avatarName.stringValue = EditorGUILayout.TextArea(avatarName.stringValue);   
		
		GUILayout.Space(20);
		
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Save Avatar")){
			DKUMASaveTool umaSaveTool = (DKUMASaveTool)target;    
			GameObject gameObject = (GameObject)umaSaveTool.gameObject;
			DKUMAData umaData = gameObject.GetComponent("DKUMAData") as DKUMAData;
			
			if(umaData){
				umaData.SaveToMemoryStream();
				var path = EditorUtility.SaveFilePanel("Save serialized Avatar","",avatarName.stringValue + ".txt","txt");
				if(path.Length != 0) {
					System.IO.File.WriteAllText(path, umaData.streamedUMA);
				}
			}
		}



		if(GUILayout.Button("Load Avatar")){
			LoadFromWin ();

		}
		GUILayout.EndHorizontal();
		GUILayout.Space(20);
	//	serializedObject.ApplyModifiedProperties();
	}

	public void LoadFromWin (){
		DKUMASaveTool umaSaveTool = (DKUMASaveTool)target;    
		GameObject gameObject = (GameObject)umaSaveTool.gameObject;
		DKUMAData umaData = gameObject.GetComponent("DKUMAData") as DKUMAData;
		umaData.Loading = true;

		if(umaData){
			var path = EditorUtility.OpenFilePanel("Load serialized Avatar","","txt");
			if (path.Length != 0) {
				DKUMAData.UMARecipe umaRecipe = new DKUMAData.UMARecipe();
				DKUMAData.UMAPackRecipe umaPackRecipe = new DKUMAData.UMAPackRecipe();
				
				streamedUMA = System.IO.File.ReadAllText(path);
				umaPackRecipe = JsonMapper.ToObject<DKUMAData.UMAPackRecipe>(streamedUMA);
				foreach ( DKRaceData _Race in umaData.raceLibrary.raceElementList ){
					if ( _Race.raceName == umaPackRecipe.race )
						umaRecipe.raceData =  _Race;
				}

				// New
				Transform tempUMA = (Instantiate(umaRecipe.raceData.racePrefab ,umaData.transform.position,umaData.transform.rotation) as GameObject).transform;
				Debug.Log ( "Creating ... " +tempUMA );
				DKUMAData newUMA = tempUMA.gameObject.GetComponentInChildren<DKUMAData>();
				newUMA.umaRecipe = umaRecipe;
				newUMA.streamedUMA = streamedUMA;
				DKUMADnaHumanoid _UMADnaHumanoid = new DKUMADnaHumanoid();
				Dictionary<Type,DKUMADna> umaDna = new Dictionary<Type,DKUMADna>();
				
				newUMA.umaPackRecipe = umaPackRecipe;
				
				// DNA
				// load DK_UMAdnaHumanoid
				newUMA.umaRecipe.umaDna.Clear();
				for(int dna = 0; dna < newUMA.umaPackRecipe.packedDna.Count; dna++){
					Type dnaType = DKUMADna.GetType(newUMA.umaPackRecipe.packedDna[dna].dnaType);
					newUMA.umaRecipe.umaDna.Add(dnaType, DKUMADna.LoadInstance(dnaType, umaPackRecipe.packedDna[dna].packedDna));
					umaDna = newUMA.umaRecipe.umaDna;
					
				}

				// Modifyers
				for (int i = 0; i <  umaRecipe.raceData.DNAConverterDataList.Count; i ++) {
					// create new DK DNA
					DKRaceData.DNAConverterData _newDNA = new DKRaceData.DNAConverterData();
					
					// add to DK_UMAdnaHumanoid
					DKUMADna temp = null;
					if ( umaDna.TryGetValue(_UMADnaHumanoid.GetType(), out temp) ){
						Debug.Log ( "success : " +umaDna[_UMADnaHumanoid.GetType()].Values.GetValue(i).ToString() );
						
						// there it is, apply the value to the modifyer
						DNAValue = float.Parse( umaDna[_UMADnaHumanoid.GetType()].Values.GetValue(i).ToString() );
					}
					
					_newDNA.Name = umaRecipe.raceData.DNAConverterDataList[i].Name;
					_newDNA.Value = DNAValue;
					_newDNA.Part = umaRecipe.raceData.DNAConverterDataList[i].Part;
					_newDNA.Part2 = umaRecipe.raceData.DNAConverterDataList[i].Part2;
					newUMA.DNAList2.Add(_newDNA);
					
				}
				newUMA.LoadFromMemoryStream();
				newUMA.Awaking();
				newUMA.atlasResolutionScale = umaData.atlasResolutionScale;
				newUMA.Dirty(true, true, true);
				newUMA.transform.parent.gameObject.name = avatarName.stringValue;
				newUMA.transform.parent = umaData.transform.parent ;
				
				serializedObject.ApplyModifiedProperties();
				
				// Finshing by destroying the previous model
				#if UNITY_EDITOR
				DestroyImmediate(umaData.transform.gameObject);
				#endif
				if (  Application.isPlaying ) Destroy(umaData.transform.gameObject);
				
			}
		}
	}

}