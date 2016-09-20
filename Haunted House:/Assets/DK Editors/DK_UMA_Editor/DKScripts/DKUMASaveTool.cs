using UnityEngine;
using System.Collections;
using LitJson;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DKUMASaveTool : MonoBehaviour {
	public string avatarName = "AvatarName";
	public string path;
	public DKUMAGenerator _DKUMAGenerator;
	float DNAValue;
	public string streamedUMA;
	DK_UMACrowd _DK_UMACrowd;

	public void OnEnable (){
		AutoLoad ();
	}
	
	public void AutoLoad (){
		AutoLoadSelf();
	}

	public void AutoSave (){
		DKUMASaveTool umaSaveTool = this;    
		GameObject gameObject = (GameObject)umaSaveTool.gameObject;
		DKUMAData umaData = gameObject.GetComponent("DKUMAData") as DKUMAData;
		if(umaData){
			umaData.SaveToMemoryStream();
			var path = avatarName+".txt";
			if(path.Length != 0) {
			//	System.IO.File.WriteAllText(path, umaData.streamedUMA);
			}
		}
	}

/*	public void AutoLoadSelf (){
		DKUMASaveTool umaSaveTool = this;    
		GameObject gameObject = (GameObject)umaSaveTool.gameObject;
		DKUMAData umaData = gameObject.GetComponent("DKUMAData") as DKUMAData;
		umaData.Loading = true;

		DKUMAData.UMARecipe umaRecipe = new DKUMAData.UMARecipe();
		DKUMAData.UMAPackRecipe umaPackRecipe = new DKUMAData.UMAPackRecipe();
		
	//	streamedUMA = System.IO.File.ReadAllText(path);
		streamedUMA = umaData.streamedUMA;

		umaPackRecipe = JsonMapper.ToObject<DKUMAData.UMAPackRecipe>(streamedUMA);
	

	//	foreach ( DKRaceData _Race in umaData.raceLibrary.raceElementList ){
	//		if ( _Race.raceName == umaPackRecipe.race )
	//			umaRecipe.raceData =  _Race;
	//	}


		umaRecipe.raceData =  umaData.umaRecipe.raceData;

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
		newUMA.transform.parent.gameObject.name = umaData.transform.parent.name;
		newUMA.transform.parent = umaData.transform.parent ;
		
	//	serializedObject.ApplyModifiedProperties();
		
		// Finshing by destroying the previous model
		#if UNITY_EDITOR
	//	DestroyImmediate(umaData.transform.gameObject);
		#endif
	//	if (  Application.isPlaying ) Destroy(umaData.transform.gameObject);

	}
*/

	#region Old AutoLoadSelf ()
	public void AutoLoadSelf (){
		GameObject DKUMAGeneratorObj = GameObject.Find("DKUMAGenerator");	
		if ( DKUMAGeneratorObj != null ) _DKUMAGenerator =  DKUMAGeneratorObj.GetComponent<DKUMAGenerator>();
		
		DKUMAData umaData = gameObject.GetComponent("DKUMAData") as DKUMAData;
		if (umaData.transform.parent 
		    && umaData.transform.parent.name.Contains("(Clone)") == false )
		{
			umaData.Loading = true;
			if(umaData){
				umaData.LoadFromMemoryStream();

				for (int i = 0; i < _DKUMAGenerator.umaDirtyList.Count ; i++)
				{
					if ( _DKUMAGenerator.umaDirtyList[i] == null ) {
						_DKUMAGenerator.umaDirtyList.Remove(_DKUMAGenerator.umaDirtyList[i]);
					}
				}

				_DKUMAGenerator.Awake();
				umaData.dirty = false;
				umaData.Dirty(true, true, true);

				umaData.myRenderer.enabled = false;

		//		Debug.Log ( "AutoLoadSelf");
			}
		}
	}
	#endregion Old AutoLoadSelf ()

}