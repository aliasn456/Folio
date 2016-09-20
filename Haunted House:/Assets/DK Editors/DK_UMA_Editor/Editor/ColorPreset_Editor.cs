using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0472 // Null is true.
#pragma warning disable 0649 // Never Assigned.

public class ColorPreset_Editor : EditorWindow {
	
	public static string ColorPresetName = "";
	public static GameObject ColorPresetObj;
	public static Color CurrentElementColor;
	public static Color SelectedElementColor;
	
	public static string SelectedElement;
	public static string SelectedPresetOverlayType;
	ColorPresetData SelectedColorPreset;
	ColorPreset SelectedColorPreset0;
	PresetRaceAssign _PresetRaceAssignToMod;
	public int Tone1;
	public int Tone2;
	public int Tone3;
	string myPath;
	string SearchString = "";

	public static string Statut;
	public ColorPreset _ColorPreset;
	GameObject ColorPresets;
	GameObject DK_UMA;
	Vector2 scroll;
	Vector2 scroll2;

	GameObject LibraryObj;
	public static DKRaceData _RaceData;
	public static DKOverlayData _OverlayData;

	bool ShowPresets = false;

	GUIContent _Prefab = new GUIContent("P", "Create or Delete an Asset.");
	GUIContent _AutoDelMiss = new GUIContent("Auto Delete Missings", "Verify the Library and delete the missing fields, multiple clicks.");
	GUIContent _Delete = new GUIContent("X", "Delete.");
	GUIContent _Duplic = new GUIContent("C", "Create a copy.");
	GUIContent _Group = new GUIContent("G", "Add the Model to the selected Group. If the Model is already in a Group, the model is removed and placed at the Root.");
	GUIContent _Inst = new GUIContent("Instantiate", "Create an Instance of the Asset.");
	
	Color Green = new Color (0.8f, 1f, 0.8f, 1);
	Color Red = new Color (0.9f, 0.5f, 0.5f);


	private static Index<string, Index<string, List<object>>> _assetStoreCP = new Index<string, Index<string, List<object>>>();
	private Dictionary<string, bool> openCP = new Dictionary<string, bool> ();

	// Use this for initialization
	void OnEnable () {

		ColorPresets = GameObject.Find("Color Presets");
		if ( ColorPresets == null ) 
		{
			DK_UMA = GameObject.Find("DK_UMA");
			
			ColorPresets = (GameObject) Instantiate(Resources.Load("Color Presets"), Vector3.zero, Quaternion.identity);
			ColorPresets.name = "Color Presets";
			ColorPresets = GameObject.Find("Color Presets");
			try{
				ColorPresets.transform.parent = DK_UMA.transform;
			}catch(NullReferenceException){}
		}
		BuildLocalAssetStoreCP();
	}
	
	void OnGUI () {
		this.minSize = new Vector2(420, 400);

		if ( LibraryObj == null ) LibraryObj = GameObject.Find("Color Presets");

		#region fonts variables
		var bold = new GUIStyle ("label");
		var boldFold = new GUIStyle ("foldout");
		bold.fontStyle = FontStyle.Bold;
		bold.fontSize = 14;
		boldFold.fontStyle = FontStyle.Bold;
		var Slim = new GUIStyle ("label");
		Slim.fontStyle = FontStyle.Normal;
		Slim.fontSize = 10;	
		var style = new GUIStyle ("label");
		style.wordWrap = true;
		#endregion fonts variables
		
		Repaint();

		using (new Horizontal()) {
			GUILayout.Label ( "Color Preset Editor", "toolbarbutton", GUILayout.ExpandWidth (true));
		}
		if ( Statut == "ToOverlay" && _OverlayData ) using (new Horizontal()) {
			GUILayout.Label ( "Overlay :");
			GUILayout.Label ( _OverlayData.overlayName, bold);	
		}

		using (new Horizontal()) {
			GUI.color = Color.white;
			GUILayout.Label ( "Color Presets", "toolbarbutton", GUILayout.ExpandWidth (true));
			if ( ShowPresets ) GUI.color = Green;
			else GUI.color = Color.gray;
			if ( Statut == "ToOverlay" && _OverlayData && _OverlayData.ColorPresets.Count > 0 ){
				if ( ShowPresets ) GUI.color = Green;
				else GUI.color = Color.gray;
			    if ( GUILayout.Button ( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))) {
					if ( ShowPresets ) ShowPresets = false;
					else ShowPresets = true;
				}
			}
		}

		if ( Statut == "ToOverlay" && _OverlayData && _OverlayData.ColorPresets.Count > 0 && ShowPresets ) {
			GUI.color = Color.white;
			using (new ScrollView(ref scroll2)) 
			{
				for (int i = 0; i < _OverlayData.ColorPresets.Count ; i++)
				{
					ColorPresetData preset = _OverlayData.ColorPresets[i];
					using (new Horizontal()) {
						GUI.color = Green;
						GUILayout.Label ( preset.ColorPresetName, "toolbarbutton", GUILayout.Width (200));
						EditorGUILayout.ColorField("", preset.PresetColor, GUILayout.Width (100));
						GUI.color = Red;
						if ( GUILayout.Button ( _Delete, "toolbarbutton", GUILayout.ExpandWidth (false))) {
							_OverlayData.ColorPresets.Remove (preset);
							EditorUtility.SetDirty(_OverlayData);
							AssetDatabase.SaveAssets();
						}
					}
				}
			}
		}

		#region Color presets
		GUI.color = Color.white;
		BuildLocalAssetStoreCP();
		if (LibraryObj.GetComponent<ColorPresetLibrary>() == null ){
			LibraryObj.AddComponent<ColorPresetLibrary>();
		}

		List<ColorPresetData> Library = LibraryObj.GetComponent<ColorPresetLibrary>().ColorPresetList.ToList();
		List<string> NamesList = new List<string>();
		#region Apply to Color Element Only
		if ( Statut == "ApplyTo" ) {
			using (new Horizontal()) {	
				GUILayout.Label ( "Color Element to modify :");
				GUILayout.Label ( SelectedElement, bold);
			
			}
			using (new Horizontal()) {
				GUILayout.Label ( "Current Color :", GUILayout.ExpandWidth (false));
				CurrentElementColor = EditorGUILayout.ColorField("", CurrentElementColor, GUILayout.ExpandWidth (true));
			
			}
		}
		#endregion Apply to Color Element Only
		
		#region Edit Color Presets Only
			
		#region Edit Race Only
			
		if ( Statut == "ApplyToRace" ) {
			string RaceName = _RaceData.raceName;
			string Race = _RaceData.Race;
			using (new Horizontal()) {
				GUILayout.Label ( "Name :");
				GUILayout.Label ( RaceName, bold);
			}
			using (new Horizontal()) {
				GUILayout.Label ( "Race :");
				GUILayout.Label ( Race, bold);	
			}
			if ( GUILayout.Button ( "Apply to Race", GUILayout.ExpandWidth (true))) {
				bool AlreadyIn = false;
				if ( _RaceData.ColorPresetDataList.Contains(SelectedColorPreset) == false ) {
					_RaceData.ColorPresetDataList.Add(SelectedColorPreset);
					if ( SelectedColorPreset.RacesList.Contains(_RaceData) == false ) {
						SelectedColorPreset.RacesList.Add(_RaceData);
					}
					EditorUtility.SetDirty(SelectedColorPreset);
					EditorUtility.SetDirty(_RaceData);
					AssetDatabase.SaveAssets();
					this.Close();Repaint();
				}
				else{
					Debug.Log ( SelectedColorPreset.ColorPresetName+" already in "+_RaceData.raceName);

				}
			}
		}
		#endregion Edit Race Only

		#region Edit Overlay Only
		if ( Selection.activeObject && ColorPresetName != "" && Statut == "ToOverlay" ) {
		//	string RaceName = _OverlayData.overlayName;
			GUILayout.Space(10);
			if (SelectedColorPreset && GUILayout.Button ( "Apply to Overlay", GUILayout.ExpandWidth (true))) {
				bool AlreadyIn = false;
				if ( _OverlayData.ColorPresets.Contains(SelectedColorPreset) == false ) {
					_OverlayData.ColorPresets.Add(SelectedColorPreset);
				
					EditorUtility.SetDirty(_OverlayData);
					AssetDatabase.SaveAssets();
					Selection.activeObject = _OverlayData;
					this.Close();
					Repaint();
				}
				else{
					Debug.Log ( SelectedColorPreset.ColorPresetName+" already in "+_OverlayData.overlayName);
				}
			}
		}
		try{
			#endregion Edit Overlay Only
			if ( Selection.activeObject && ColorPresetName != "" ) {
				if ( Statut == "ApplyTo" ) {
					if (GUILayout.Button ( "Apply Preset", GUILayout.ExpandWidth (true))) {
						try{
						if ( SelectedElement == "HeadWear" ) {
								EditorVariables.HeadWColorPresetName = ColorPresetName;

								EditorVariables.HeadWearColor = SelectedColorPreset.PresetColor;
								
							this.Close();
						}
						if ( SelectedElement == "TorsoWear" ) {
								EditorVariables.TorsoWColorPresetName = ColorPresetName;
							EditorVariables.TorsoWearColor = SelectedColorPreset.PresetColor;
							this.Close();
						}
						if ( SelectedElement == "LegsWear" ) {
								EditorVariables.LegsWColorPresetName = ColorPresetName;
							EditorVariables.LegsWearColor = SelectedColorPreset.PresetColor;
							this.Close();
						}
						if ( SelectedElement == "HandWear" ) {
								EditorVariables.HandWColorPresetName = ColorPresetName;
							EditorVariables.HandWearColor = SelectedColorPreset.PresetColor;
							this.Close();
						}
						if ( SelectedElement == "BeltWear" ) {
								EditorVariables.BeltWColorPresetName = ColorPresetName;
							EditorVariables.BeltWearColor = SelectedColorPreset.PresetColor;
							this.Close();
						}
						if ( SelectedElement == "FeetWear" ) {
								EditorVariables.FeetWColorPresetName = ColorPresetName;
							EditorVariables.FeetWearColor = SelectedColorPreset.PresetColor;
							this.Close();
						}
						if ( SelectedElement == "Skin" ) {
								EditorVariables.SkinColorPresetName = ColorPresetName;
							EditorVariables.SkinColor = SelectedColorPreset.PresetColor;
							this.Close();
						}
						if ( SelectedElement == "Hair" ) {
								EditorVariables.HairColorPresetName = ColorPresetName;
							EditorVariables.HairColor = SelectedColorPreset.PresetColor;
							this.Close();
						}
						if ( SelectedElement == "Eyes" ) {
								EditorVariables.EyesColorPresetName = ColorPresetName;
							EditorVariables.EyesColor = SelectedColorPreset.PresetColor;
							this.Close();
						}
							}catch(NullReferenceException){Debug.Log("Select a color preset to apply.");}
					/*	if ( SelectedElement == "InnerMouth" ) {
							EditorVariables.InnerMouthColorPresetName = ColorPresetName;
							EditorVariables.InnerMouthColor = SelectedColorPreset.PresetColor;
							this.Close();
						}
					*/
						
					}
				}
				if ( SelectedColorPreset ) using (new Horizontal()) {	
					GUILayout.Label ( "Selected Color Preset :");
					ColorPresetName= GUILayout.TextField(ColorPresetName, 50, bold, GUILayout.ExpandWidth (true));
					if (GUILayout.Button ( "Rename", GUILayout.ExpandWidth (false))) {
						SelectedColorPreset.name = ColorPresetName;
						SelectedColorPreset.ColorPresetName = ColorPresetName;
							string path = AssetDatabase.GetAssetPath(SelectedColorPreset);
							AssetDatabase.RenameAsset(path , ColorPresetName );
					}
				}
				if ( SelectedColorPreset && SelectedColorPreset.PresetColor != null ) using (new Horizontal()) {
					GUILayout.Label ( "Preset Color :", GUILayout.ExpandWidth (false));
					SelectedColorPreset.PresetColor = EditorGUILayout.ColorField("", SelectedColorPreset.PresetColor, GUILayout.ExpandWidth (true));
				}
				#region Edit Color Preset Overlay
				using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label ( "Body :", GUILayout.ExpandWidth (false));
					if ( SelectedPresetOverlayType == "Flesh" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Flesh", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "Flesh";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}
					if ( SelectedPresetOverlayType == "Lips" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Lips", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "Lips";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
						
					}
					if ( SelectedPresetOverlayType == "InnerMouth" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Inner Mouth", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "InnerMouth";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}
					if ( SelectedPresetOverlayType == "Eyes" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Eyes", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "Eyes";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}	
					if ( SelectedPresetOverlayType == "Hair" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Hair", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "Hair";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}
				}
				using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label ( "Wears :", GUILayout.ExpandWidth (false));
					if ( SelectedPresetOverlayType == "TorsoWear" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Torso", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "TorsoWear";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}
					if ( SelectedPresetOverlayType == "LegsWear" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Legs", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "LegsWear";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}
					if ( SelectedPresetOverlayType == "FeetWear" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Feet", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "FeetWear";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}
					if ( SelectedPresetOverlayType == "HandsWear" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Hands", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "HandsWear";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}
					if ( SelectedPresetOverlayType == "HeadWear" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Head", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "HeadWear";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}
					if ( SelectedPresetOverlayType == "Underwear" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Underwear", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "Underwear";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}
					if ( SelectedPresetOverlayType == "" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "None", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}
				}
				using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label ( "Material :", GUILayout.ExpandWidth (false));
					if ( SelectedPresetOverlayType == "Metal" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Metal", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "Metal";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}
					if ( SelectedPresetOverlayType == "Cloth" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Cloth", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "Cloth";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}
					if ( SelectedPresetOverlayType == "Leather" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Leather", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "Leather";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}
					if ( SelectedPresetOverlayType == "Wood" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Wood", GUILayout.ExpandWidth (true))) {
						SelectedPresetOverlayType = "Wood";
						SelectedColorPreset =   Selection.activeObject as ColorPresetData;
						if ( SelectedColorPreset != null ) 
						{
							SelectedColorPreset.OverlayType = SelectedPresetOverlayType;
							EditorUtility.SetDirty(SelectedColorPreset);
							AssetDatabase.SaveAssets();
						}
					}
				}
				#endregion Edit Color Preset Overlay
			}
			else {

				using (new HorizontalCentered()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Select a Color Preset from the list bellow.", GUILayout.ExpandWidth (false));
				}	
				GUILayout.Space(10);
			}
			using (new Horizontal()) {
				GUI.color = Color.white;
				if ( GUILayout.Button ( "New Preset", GUILayout.ExpandWidth (true))) {
					ColorPresetData _NewColorPreset;
					_NewColorPreset = ScriptableObject.CreateInstance("ColorPresetData") as ColorPresetData;
					Selection.activeObject = _NewColorPreset;
					_NewColorPreset.name = "New Color Preset";
					_NewColorPreset.ColorPresetName = "New Color Preset";
					AssetDatabase.CreateAsset(_NewColorPreset as UnityEngine.Object, "Assets/DK Editors/DK_UMA_Editor/Prefabs/Color Presets/" + _NewColorPreset.name + ".asset");
					Debug.Log ("creating "+_NewColorPreset.name );
				}
				GUI.color = Color.yellow;
			/*	if ( GUILayout.Button ( "Import", GUILayout.ExpandWidth (true))) {
					GameObject ColorPresetsObj = GameObject.Find("Color Presets");	
					PresetRaceAssignList PresetRaceAssignList = ColorPresetsObj.GetComponent<PresetRaceAssignList>();
					ColorPresetLibrary ColorPresetLibrary = LibraryObj.GetComponent<ColorPresetLibrary>();

					foreach (Transform Child in ColorPresetsObj.transform) {
						ColorPreset ColorPreset = Child.GetComponent<ColorPreset>();
						if ( ColorPreset != null ){

							// To Assets
							ColorPresetData _NewColorPreset;
							_NewColorPreset = ScriptableObject.CreateInstance("ColorPresetData") as ColorPresetData;
							Selection.activeObject = _NewColorPreset;
							_NewColorPreset.name = ColorPreset.transform.name;
							_NewColorPreset.ColorPresetName = ColorPreset.transform.name;
							_NewColorPreset.PresetColor = ColorPreset.PresetColor;
							_NewColorPreset.OverlayType = ColorPreset.OverlayType;
							AssetDatabase.CreateAsset(_NewColorPreset as UnityEngine.Object, "Assets/DK Editors/DK_UMA_Editor/Prefabs/Color Presets/" + _NewColorPreset.name + ".asset");
						
							// To Library
							List<ColorPresetData> tmpList = new List<ColorPresetData>();
							tmpList = ColorPresetLibrary.ColorPresetList.ToList();
							tmpList.Add(_NewColorPreset);
							ColorPresetLibrary.ColorPresetList = tmpList.ToArray();
							if (EditorVariables.DK_UMACrowd.raceLibrary == null ) EditorVariables.DetectAll();
							List<DKRaceData> tmpRaceList = new List<DKRaceData>();
							tmpRaceList = EditorVariables.DK_UMACrowd.raceLibrary.raceElementList.ToList();
						
							// to races
							for(int i = 0; i < PresetRaceAssignList.RacesPresetsList.Count; i ++){
								for(int i2 = 0; i2 < tmpRaceList.Count; i2 ++){
									if ( tmpRaceList[i2] == PresetRaceAssignList.RacesPresetsList[i].RacePreset 
										&& tmpRaceList[i2].ColorPresetDataList.Contains(_NewColorPreset) == false )
									{
										for(int i3 = 0; i3 < PresetRaceAssignList.RacesPresetsList[i].RacePresetList.Count; i3 ++){
											if (PresetRaceAssignList.RacesPresetsList[i].RacePresetList[i3].name==_NewColorPreset.name){
											//	if ( 
												tmpRaceList[i2].ColorPresetDataList.Add(_NewColorPreset);
												EditorUtility.SetDirty(ColorPresetLibrary);
												EditorUtility.SetDirty(tmpRaceList[i2]);
												AssetDatabase.SaveAssets();
											}
										}
									}
								}
							}
						}
					}
				}
				if ( GUILayout.Button ( "Clear Races", GUILayout.ExpandWidth (true))) {
					List<DKRaceData> tmpRaceList = new List<DKRaceData>();
					tmpRaceList = EditorVariables.DK_UMACrowd.raceLibrary.raceElementList.ToList();
					for(int i2 = 0; i2 < tmpRaceList.Count; i2 ++){
						tmpRaceList[i2].ColorPresetDataList.Clear();
					}
				}
				if ( GUILayout.Button ( "Clear Assets", GUILayout.ExpandWidth (true))) {
					foreach (var tp in _assetStoreCP) {
						if ( !openCP.ContainsKey (tp.Key))
							openCP [tp.Key] = false;
						foreach (var n in tp.Value.OrderByDescending(q=>q.Value.Count).ThenBy(q=>q.Key)) {
							foreach (var i in n.Value.Cast<UnityEngine.Object>()) {
								if ( i.name.ToString ().ToLower().Contains(SearchString) ) {
									var addOn="";
									if(!AssetDatabase.IsMainAsset(i) && !AssetDatabase.IsSubAsset(i))
									{
										GUI.color = Color.red;
										addOn = " (internal to Unity)";
									}
									string path =  AssetDatabase.GetAssetPath(i);
									AssetDatabase.DeleteAsset (path);
								}
							}
						}
					}
				}
				if ( GUILayout.Button ( "Clear Lib", GUILayout.ExpandWidth (true))) {
					ColorPresetLibrary ColorPresetLibrary = LibraryObj.GetComponent<ColorPresetLibrary>();
					List<ColorPresetData> tmpList = new List<ColorPresetData>();
					tmpList = ColorPresetLibrary.ColorPresetList.ToList();
					tmpList.Clear();
					ColorPresetLibrary.ColorPresetList = tmpList.ToArray();
				}*/
			}
			GUILayout.Space(5);
			#region Search
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("Search for :", GUILayout.ExpandWidth (false));
				SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));
			}
			#endregion Search
			GUILayout.Space(5);
			#region Color Presets List
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label ( "Presets List", "toolbarbutton", GUILayout.ExpandWidth (true));
			}

			DirectoryInfo dir = new DirectoryInfo("Assets/DK Editors/DK_UMA_Editor/Prefabs/Color Presets/");
			FileInfo[] info = dir.GetFiles("*.asset");
			using (new ScrollView(ref scroll)) 
			{
				try{
					foreach (var tp in _assetStoreCP) {
						if ( !openCP.ContainsKey (tp.Key))
							openCP [tp.Key] = false;
						foreach (var n in tp.Value.OrderByDescending(q=>q.Value.Count).ThenBy(q=>q.Key)) {
							foreach (var i in n.Value.Cast<UnityEngine.Object>()) {
								try{
									if ( i.name.ToString ().ToLower().Contains(SearchString) ) {
										var addOn="";
										if(!AssetDatabase.IsMainAsset(i) && !AssetDatabase.IsSubAsset(i))
										{
											GUI.color = Color.red;
											addOn = " (internal to Unity)";
										}
										else if ( i.name.ToString ().ToLower().Contains(SearchString) ) {
											using (new Horizontal()){
											/*	GUI.color = new Color (0.8f, 1f, 0.8f, 1);
												if ( Library.Contains(i as ColorPresetData) == false  && GUILayout.Button ( "Add", "toolbarbutton", GUILayout.ExpandWidth (false))){
													BuildLocalAssetStoreCP();
													List<ColorPresetData> _Library = LibraryObj.GetComponent<ColorPresetLibrary>().ColorPresetList.ToList();
													Library.Add(i as ColorPresetData);
													LibraryObj.GetComponent<ColorPresetLibrary>().ColorPresetList = Library.ToArray();
												}*/
											//	if ( Library.Contains(i as ColorPresetData) == true ) { 
													GUILayout.Space(10);
													GUI.color = new Color (0.8f, 1f, 0.8f, 1);
													if ( Library.Contains(i as ColorPresetData) == true 
													    && GUILayout.Button ( new GUIContent(_Duplic), "toolbarbutton", GUILayout.ExpandWidth (false))){
														Library.Remove(i as ColorPresetData);
														LibraryObj.GetComponent<ColorPresetLibrary>().ColorPresetList = Library.ToArray();
														
													}
													GUILayout.Space(10);
											//	}
												if ( Library.Contains(i as ColorPresetData) == true
												    && Selection.activeObject != i ) GUI.color = Color.white;
												else if ( Library.Contains(i as ColorPresetData) == false 
												         && Selection.activeObject != i) GUI.color = Color.white;
												else if ( Selection.activeObject == i ) GUI.color = Color.yellow;
												if (GUILayout.Button (i.name.ToString () + addOn, "toolbarbutton",GUILayout.Width (230))) {
													SelectedColorPreset = i as ColorPresetData;
													Selection.activeObject = i;
													EditorGUIUtility.PingObject(i);
												}
												GUI.color = Color.white;
												(i as ColorPresetData).PresetColor = EditorGUILayout.ColorField("", (i as ColorPresetData).PresetColor, GUILayout.Width (70));
												GUI.color = Color.white;
												GUILayout.Label ( (i as ColorPresetData).OverlayType, "toolbarbutton", GUILayout.Width (75));
												GUI.color = new Color (0.9f, 0.5f, 0.5f);
												if ( Library.Contains(i as ColorPresetData) == true && GUILayout.Button ( new GUIContent(_Delete), "toolbarbutton", GUILayout.ExpandWidth (false))){
													Library.Remove(i as ColorPresetData);
													LibraryObj.GetComponent<ColorPresetLibrary>().ColorPresetList = Library.ToArray();
												}
												if ( Library.Contains(i as ColorPresetData) == false && GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))){
													Selection.activeObject = i;
													DeleteAsset.Action = "";
													DeleteAsset.ProcessName = "Delete Asset";
													DK_UMA_Editor.OpenDeleteAsset();
												}
											}
										}
									}
								}
								catch (MissingReferenceException){}
							}
						}
					}
				}catch(InvalidOperationException){}
			}
			}catch(ArgumentException){}
			#endregion
		#endregion Edit Color Presets Only
		
		#endregion HeadWear Color
	}
 
	void BuildLocalAssetStoreCP()
	{
		var tmp = Resources.FindObjectsOfTypeAll(typeof(ColorPresetData))
			.Distinct()
				.ToList();
		var assets = tmp
			.Where(g=>g!=null && !string.IsNullOrEmpty(g.name) )
				.Where(a=>AssetDatabase.IsMainAsset(a) || AssetDatabase.IsSubAsset(a))
				.Distinct()
				.ToList();
		_assetStoreCP.Clear();
		foreach(var a in assets)
		{
			_assetStoreCP[a.GetType().Name][a.name].Add(a);
			
		}
		foreach(var a in tmp)
		{
			if(_assetStoreCP.ContainsKey(a.GetType().Name) && _assetStoreCP[a.GetType().Name].ContainsKey(a.name) && !_assetStoreCP[a.GetType().Name][a.name].Contains(a))
			{
				_assetStoreCP[a.GetType().Name][a.name].Add(a);
			}
		}
		
	}
	void OnSelectionChange() {
		if ( Selection.activeObject ){
			if ( Selection.activeObject.GetType().ToString() == "DKRaceData" && Statut == "ApplyToRace" ) 
				_RaceData = ( Selection.activeObject as DKRaceData );

			if ( Selection.activeObject.GetType().ToString() == "ColorPresetData" ){
				SelectedColorPreset = Selection.activeObject as ColorPresetData;
				ColorPresetName = SelectedColorPreset.name;
				SelectedPresetOverlayType = SelectedColorPreset.OverlayType;
				CurrentElementColor = SelectedColorPreset.PresetColor;
			}
		}
	}
}
