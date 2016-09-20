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

public class DK_UMA_Export_Win : EditorWindow {

	Vector2 scroll1;
	
	public DKRaceData _RaceData;
	public DKOverlayData _DKOverlayData;
	public DKSlotData _SlotData;
	public Transform Model;
	public DKUMAData _DKUMAData;

	ExportData._raceData _tmpRaceData = new ExportData._raceData();
	ExportData._slotData _tmpSlotData = new ExportData._slotData();
	ExportData._overlayData _tmpDKOverlayData = new ExportData._overlayData();

	public static ExportData _ExportData;
	public static string _ExportDataName = "";
	string _Selection = "";
	string _Type = "";
	string _GenderType = "";
	public static string _Path = "";
	public static string _SavePath = "";
	string NewExpProj = "";
	string ExpName = "";

	bool AddToGrp;
	bool AddData;
	bool AddColors;
	bool AddSlots;
	bool AddOverlays;
	public static bool Ready = false;

	public List<string> ColorsList = new List<string>();
	public List<DKOverlayData> OverlaysList = new List<DKOverlayData>();
	public List<DKRaceData> RacesList = new List<DKRaceData>();
	public List<DKSlotData> SlotsList = new List<DKSlotData>();
	public List<string> DatasList = new List<string>();

	public List<ExportData._raceData> racesList = new List<ExportData._raceData>();
	public List<ExportData._overlayData> overlaysList = new List<ExportData._overlayData>();
	public List<ExportData._slotData> slotsList = new List<ExportData._slotData>();
	
	public static List<string> ExportList = new List<string>();

	Color Green = new Color (0.8f, 1f, 0.8f, 1);
	Color Red = new Color (0.9f, 0.5f, 0.5f);

	public static void OpenChooseExportWin(){
		GetWindow(typeof(DK_UMA_ChooseExport_Win), false, "Exports");
	}
	public static void OpenViewExportWin(){
		GetWindow(typeof(DK_UMA_ChooseExport_Win), false, "Exports");
	}
	public static void OpenExportJournalWin(){
		GetWindow(typeof(DK_UMA_Import_Win), false, "Exports");
	}
	void OnGUI () {
		Repaint();
		this.minSize = new Vector2(300, 500);
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

		#region Prepare the element
		_RaceData = Selection.activeObject as DKRaceData;
		_DKOverlayData = Selection.activeObject as DKOverlayData;
		_SlotData = Selection.activeObject as DKSlotData;
		_DKUMAData = Selection.activeObject as DKUMAData;

		if ( _RaceData ){
			_Selection = _RaceData.raceName;
			_Type = "DKRaceData";
		}
		if ( _SlotData ){
			_Selection = _SlotData.slotName;
			_Type = "DKSlotData";
		}
		if ( _DKOverlayData ){
			_Selection = _DKOverlayData.overlayName;
			_Type = "DKOverlayData";
		}
		if (Selection.activeObject && Selection.activeObject.GetType().ToString() == "ExportData" ) {
			_Selection = Selection.activeObject.name;
			_Type = "ExportData";
			ExpName = (Selection.activeObject as ExportData).PackageName;
			
		}

		#endregion Prepare the element

		using (new Horizontal()) {
			GUILayout.Label ( "DK UMA Exporter", "toolbarbutton", GUILayout.ExpandWidth (true));
		}

		GUILayout.Space(5);	
		if ( _Selection != null ) {
			#region Infos
			using (new Horizontal()) {
				GUI.color = Color.yellow;
				GUILayout.Label ( "Selection :", GUILayout.Width (70));
				GUI.color = Color.white;
				GUILayout.TextField( _Selection, 256, GUILayout.ExpandWidth (true));
			}
			using (new Horizontal()) {
				GUI.color = Color.yellow;
				GUILayout.Label ( "Type :", GUILayout.Width (70));
				GUI.color = Color.white;
				GUILayout.TextField( _Type, 256, GUILayout.ExpandWidth (true));
			}
			if ( _RaceData ){
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Race :", GUILayout.Width (70));
					GUI.color = Color.white;
					GUILayout.TextField( _RaceData.Race, 256, GUILayout.ExpandWidth (true));
				}
			}
	/*		GUILayout.Space(5);	
			if ( !AddToGrp ) GUI.color = Color.gray;
			else GUI.color = new Color (0.8f, 1f, 0.8f, 1);
			if ( GUILayout.Button ( "Add to Export group", GUILayout.ExpandWidth (true))){
				if ( !AddToGrp ) AddToGrp = true;
				else AddToGrp = false;
			}*/
			#endregion Infos

			#region Race Only
			if ( _RaceData ){
			
			}
			#endregion Race Only
			if ( GUILayout.Button ( "View Projects List", GUILayout.ExpandWidth (true))){
				OpenViewExportWin();
				DK_UMA_ChooseExport_Win.Action = "Select Export Project";
			}
			#region Export Project Only
			if (Selection.activeObject && Selection.activeObject.GetType().ToString() == "ExportData" ) {
				GUI.color = Color.yellow;
				GUILayout.Label ( "Export Project", "toolbarbutton", GUILayout.ExpandWidth (true));
				GUILayout.TextField("Create the Export Package to share your creation or to sell it ! " +
					"You just need to click for your package to be Ready to use for your users and byers." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				GUI.color = new Color (0.9f, 0.5f, 0.5f);
				GUILayout.TextField( "Be sure to own the right to share or to sell any content of your Package !" , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

				GUI.color = new Color (0.8f, 1f, 0.8f, 1);
				if ( GUILayout.Button ( "Create Package !", GUILayout.ExpandWidth (true))){
					_ExportData = (Selection.activeObject as ExportData);
					Export();
				}


			}
			#endregion Export Project Only
			else {
			#region Export Element(s) Only
				GUI.color = Color.yellow;
				GUILayout.Label ( "Export Dependencies", "toolbarbutton", GUILayout.ExpandWidth (true));
				using (new Horizontal()) {
					#region Data
					if ( !AddData ) GUI.color = Color.gray;
					else GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					if ( GUILayout.Button ( "Data", GUILayout.ExpandWidth (true))){
						if ( !AddData ) AddData = true;
						else AddData = false;
					}
					#endregion Data
					#region Colors Presets
					if ( _SlotData == null  && _DKOverlayData == null ){
						if ( !AddColors ) GUI.color = Color.gray;
						else GUI.color = new Color (0.8f, 1f, 0.8f, 1);
						if ( GUILayout.Button ( "Color Presets", GUILayout.ExpandWidth (true))){
							if ( !AddColors ) AddColors = true;
							else AddColors = false;
						}
					}
					#endregion Colors Presets
					#region Slots
					if ( _SlotData == null && _DKOverlayData == null ){
						if ( !AddSlots ) GUI.color = Color.gray;
						else GUI.color = new Color (0.8f, 1f, 0.8f, 1);
						if ( GUILayout.Button ( "Slots", GUILayout.ExpandWidth (true))){
							if ( !AddSlots ) AddSlots = true;
							else AddSlots = false;
						}
					}
					#endregion Slots
					#region Overlays
					if ( _DKOverlayData == null ){
						if ( !AddOverlays ) GUI.color = Color.gray;
						else GUI.color = new Color (0.8f, 1f, 0.8f, 1);
						if ( GUILayout.Button ( "Overlays", GUILayout.ExpandWidth (true))){
							if ( !AddOverlays ) AddOverlays = true;
							else AddOverlays = false;
						}
					}
					#endregion Overlays
				}
				#region Actions
				#region Export
				GUI.color = Color.yellow;
				GUILayout.Label ( "Export Process", "toolbarbutton", GUILayout.ExpandWidth (true));
				GUI.color = Color.white;
				GUILayout.Label ( "Step 1 : Detect all the necessary elements.", GUILayout.ExpandWidth (true));
				GUI.color = new Color (0.8f, 1f, 0.8f, 1);
				if ( GUILayout.Button ( "Prepare Export", GUILayout.ExpandWidth (true))){
					OverlaysList.Clear();
					RacesList.Clear();
					SlotsList.Clear();
					DatasList.Clear();
					ColorsList.Clear();

					PrepareExport();
				}
				if ( racesList.Count > 0 || slotsList.Count > 0 || overlaysList.Count > 0 ){
					GUI.color = Color.white;
					GUILayout.Label ( "Step 2 : Store the Export information.", GUILayout.ExpandWidth (true));
					using (new Horizontal()) {
						if ( NewExpProj == "New" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
						else GUI.color = Color.white;
						if ( GUILayout.Button ( "New Export Data", GUILayout.ExpandWidth (true))){
							NewExpProj = "New";
						}
						if ( NewExpProj == "Add" ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
						else GUI.color = Color.white;
						if ( GUILayout.Button ( "Add to Export Data", GUILayout.ExpandWidth (true))){
							NewExpProj = "Add";
						}
					}
					if ( NewExpProj == "New" ) {
						using (new Horizontal()) {
							if ( ExpName == "" ) GUI.color = Color.yellow;
							else GUI.color = Color.white;
							GUILayout.Label ( "Name", GUILayout.ExpandWidth (false));
							ExpName =  GUILayout.TextField ( ExpName, GUILayout.ExpandWidth (true));
						}
						using (new Horizontal()) {
							GUI.color = new Color (0.8f, 1f, 0.8f, 1);
							if ( ExpName != "" && GUILayout.Button ( "Create Export Data", GUILayout.ExpandWidth (true))){
								// Create Export Project
								ExportData newExportData = ScriptableObject.CreateInstance("ExportData") as ExportData;
								newExportData.PackageName = Selection.activeObject.name;
								string _path = ("Assets/DK Editors/DK_UMA_Editor/Exporter/Exporting/"+ExpName+".asset");
								AssetDatabase.CreateAsset(newExportData, _path);
								// Add the Elements to Export List
								if ( _RaceData ) newExportData.RacesList.Add(_tmpRaceData);
								if ( _SlotData ) newExportData.SlotsList.Add(_tmpSlotData);
								if ( _DKOverlayData ) newExportData.OverlaysList.Add(_tmpDKOverlayData);
								_ExportData = newExportData;
								EditorUtility.SetDirty(_ExportData);
								AssetDatabase.SaveAssets();
								Ready = true;
							}
						}
					}
					if ( NewExpProj == "Add" ){
						using (new Horizontal()) {
							if ( GUILayout.Button ( "Choose Export Project", GUILayout.ExpandWidth (true))){
								OpenChooseExportWin();
								DK_UMA_ChooseExport_Win.Action = "Choose Export Project";
							}
						}
						if ( _ExportDataName != "" ){
							using (new Horizontal()) {
								GUILayout.Label ( "Selected :", GUILayout.ExpandWidth (false));
								GUILayout.Label ( _ExportDataName, GUILayout.ExpandWidth (true));
							}
							if ( GUILayout.Button ( "Assign to the Project", GUILayout.ExpandWidth (true))){
								if ( _ExportDataName != "" ){
									string filePath = "Assets/DK Editors/DK_UMA_Editor/Exporter/Exporting/"+_ExportDataName  ;
									_ExportData = (ExportData)AssetDatabase.LoadAssetAtPath(filePath, typeof(ExportData));
									if ( _RaceData ) _ExportData.RacesList.Add(_tmpRaceData);
									if ( _SlotData ) _ExportData.SlotsList.Add(_tmpSlotData);
									if ( _DKOverlayData ) _ExportData.OverlaysList.Add(_tmpDKOverlayData);
									EditorUtility.SetDirty(_ExportData);
									AssetDatabase.SaveAssets();
								}
								Ready = true;
							}
						}
					}
					if ( Ready ) {
						GUI.color = Color.white;
						GUILayout.Label ( "Step 3 : Finishing the Export Project.", GUILayout.ExpandWidth (true));

							using (new Horizontal()) {
							if ( GUILayout.Button ( "Store the Project", GUILayout.ExpandWidth (true))){

								Selection.activeObject = _ExportData;
								Ready = false;
								this.Close();
							}
							GUI.color = new Color (0.8f, 1f, 0.8f, 1);
							if ( GUILayout.Button ( "Create the Package !", GUILayout.ExpandWidth (true))){

								Selection.activeObject = _ExportData;
								Ready = false;
								Export();

							}
						}
					}
				}

				#endregion Export

				#region Lists
				GUILayout.Space(5);
				GUI.color = Color.yellow;
				GUILayout.Label ( "Selected Elements", "toolbarbutton", GUILayout.ExpandWidth (true));
				#region Race List
				if ( _RaceData != null ) using (new ScrollView(ref scroll1)){

					GUI.color = Color.white;
					GUILayout.Label ( "Races", "toolbarbutton", GUILayout.ExpandWidth (true));
					if ( racesList.Count > 0 ) for(int i = 0; i < racesList.Count; i ++){
						using (new Horizontal()) {
							GUI.color = new Color (0.9f, 0.5f, 0.5f);
							DKRaceData tmpRaceData = racesList[i].RaceData;
							if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
								racesList.Remove(racesList[i]);
							}
							GUI.color = Color.white;
							if ( GUILayout.Button ( racesList[i].Name, Slim, GUILayout.ExpandWidth (true))) {
							}

						}
						if ( AddData ){
							using (new Horizontal()) {
								GUILayout.Space(15);
								GUILayout.Label ( "Datas", "toolbarbutton", GUILayout.ExpandWidth (true));
							}
							if ( racesList[i].racePrefab != null ) using (new Horizontal()) {
								GUILayout.Space(15);
								GUI.color = new Color (0.9f, 0.5f, 0.5f);
								if ( racesList[i].racePrefab != null && GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
									racesList[i].racePrefab = null;
								}
								GUI.color = Color.white;
								if ( racesList[i].racePrefab != null && GUILayout.Button ( racesList[i].racePrefab.ToString(), Slim, GUILayout.ExpandWidth (true))) {
								}
							}
							if ( racesList[i].TPose != null ) using (new Horizontal()) {
								GUILayout.Space(15);
								GUI.color = new Color (0.9f, 0.5f, 0.5f);
								if ( racesList[i].TPose != null && GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
									racesList[i].TPose = null;
								}
								GUI.color = Color.white;
								if ( racesList[i].TPose != null && GUILayout.Button ( racesList[i].TPose.ToString(), Slim, GUILayout.ExpandWidth (true))) {
								}
								
							}
						}
						if ( AddColors ){
							using (new Horizontal()) {
								GUILayout.Space(15);
								GUILayout.Label ( "Color Presets", "toolbarbutton", GUILayout.ExpandWidth (true));
							}
							if ( racesList[i]._ColorsList != null ) {
								if ( racesList[i]._ColorsList.Count > 0 ) 
								for(int i1 = 0; i1 < racesList[i]._ColorsList.Count; i1 ++){
									using (new Horizontal()) {
										GUILayout.Space(15);
										GUI.color = new Color (0.9f, 0.5f, 0.5f);
										if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
											racesList[i]._ColorsList.Remove(racesList[i]._ColorsList[i1]);
										}
										GUI.color = Color.white;
										if ( GUILayout.Button ( racesList[i]._ColorsList[i1].ColorPresetName, Slim, GUILayout.ExpandWidth (true))) {
										
										}
									}
								}
							}
						}
						if ( AddSlots ) {
							using (new Horizontal()) {
								GUILayout.Space(15);
								GUILayout.Label ( "Slots", "toolbarbutton", GUILayout.ExpandWidth (true));
							}
							if ( racesList[i]._SlotsList != null ) {
								if ( racesList[i]._SlotsList.Count > 0 ) 
								for(int i1 = 0; i1 < racesList[i]._SlotsList.Count; i1 ++){
									using (new Horizontal()) {
										GUILayout.Space(15);
										GUI.color = new Color (0.9f, 0.5f, 0.5f);
										if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
											racesList[i]._SlotsList.Remove(racesList[i]._SlotsList[i1]);
										}
										GUI.color = Color.white;
										if ( GUILayout.Button ( racesList[i]._SlotsList[i1].Name, Slim, GUILayout.ExpandWidth (true))) {
											
										}
									}
									if ( AddData ) using (new Horizontal()) {
										GUILayout.Space(30);
										GUI.color = new Color (0.9f, 0.5f, 0.5f);
										if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
											racesList[i]._SlotsList[i1].materialSample = null;									}
										GUI.color = Color.white;
										if ( GUILayout.Button ( racesList[i]._SlotsList[i1].materialSample.ToString(), Slim, GUILayout.ExpandWidth (true))) {
											
										}
									}
									if ( AddData ) using (new Horizontal()) {
										GUILayout.Space(30);
										GUI.color = new Color (0.9f, 0.5f, 0.5f);
										if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
											racesList[i]._SlotsList[i1].meshRenderer = null;
										}
				
										GUI.color = Color.white;
										if ( GUILayout.Button ( racesList[i]._SlotsList[i1].meshRenderer.ToString(), Slim, GUILayout.ExpandWidth (true))) {
											
										}
									}
									if ( AddData ) using (new Horizontal()) {
										GUILayout.Space(30);
										GUI.color = new Color (0.9f, 0.5f, 0.5f);
										if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
											racesList[i]._SlotsList[i1].Place = null;
										}
										
										GUI.color = Color.white;
										if ( GUILayout.Button ( racesList[i]._SlotsList[i1].Place.ToString(), Slim, GUILayout.ExpandWidth (true))) {
											
										}
									}
								}
							}
						}
						if ( AddOverlays ) {
							using (new Horizontal()) {
								GUILayout.Space(15);
								GUILayout.Label ( "Overlays", "toolbarbutton", GUILayout.ExpandWidth (true));
							}
							if ( racesList[i]._OverlaysList != null ) {
								if ( racesList[i]._OverlaysList.Count > 0 ) 
								for(int i1 = 0; i1 < racesList[i]._OverlaysList.Count; i1 ++){
									using (new Horizontal()) {
										GUILayout.Space(15);
										GUI.color = new Color (0.9f, 0.5f, 0.5f);
										if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
											racesList[i]._OverlaysList.Remove(racesList[i]._OverlaysList[i1]);
										}
										GUI.color = Color.white;
										if ( GUILayout.Button ( racesList[i]._OverlaysList[i1].Name, Slim, GUILayout.ExpandWidth (true))) {
											
										}
									}
									if ( AddData ) 
									for(int i2 = 0; i2 < racesList[i]._OverlaysList[i1].textureList.Count; i2 ++){
										using (new Horizontal()) {
											GUILayout.Space(30);
											GUI.color = new Color (0.9f, 0.5f, 0.5f);
											if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
												racesList[i]._OverlaysList[i1].textureList.Remove(racesList[i]._OverlaysList[i1].textureList[i2]);									
											}
											GUI.color = Color.white;
											if ( GUILayout.Button ( racesList[i]._OverlaysList[i1].textureList[i2].ToString(), Slim, GUILayout.ExpandWidth (true))) {
												
											
											}
										}
									}
									if ( AddData ) using (new Horizontal()) {
										GUILayout.Space(30);
										GUI.color = new Color (0.9f, 0.5f, 0.5f);
										if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
											racesList[i]._OverlaysList[i1].Place = null;
										}
										
										GUI.color = Color.white;
										if ( GUILayout.Button ( racesList[i]._OverlaysList[i1].Place.ToString(), Slim, GUILayout.ExpandWidth (true))) {
											
										}
									}
								}
							}
						}
					}
					if ( SlotsList.Count > 0 ) for(int i = 0; i < SlotsList.Count; i ++){
						
					}
					if ( OverlaysList.Count > 0 ) for(int i = 0; i < OverlaysList.Count; i ++){
						
					}
					if ( DatasList.Count > 0 ) for(int i = 0; i < DatasList.Count; i ++){


					}
				}
				#endregion Race List
				#region Slot List
				if ( _SlotData != null ) using (new ScrollView(ref scroll1)){
					GUILayout.Label ( "Slots", "toolbarbutton", GUILayout.ExpandWidth (true));
					if ( slotsList.Count > 0 ) for(int i = 0; i < slotsList.Count; i ++){
						using (new Horizontal()) {
							GUI.color = new Color (0.9f, 0.5f, 0.5f);
							DKSlotData tmpSlotData = slotsList[i].SlotData;
							if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
								slotsList.Remove(slotsList[i]);
							}
							GUI.color = Color.white;
							if ( GUILayout.Button ( slotsList[i].Name, Slim, GUILayout.ExpandWidth (true))) {
							}
							
						}
						if ( AddData ){
							using (new Horizontal()) {
								GUILayout.Space(15);
								GUILayout.Label ( "Datas", "toolbarbutton", GUILayout.ExpandWidth (true));
							}
							if ( slotsList[i].materialSample != null ) using (new Horizontal()) {
								GUILayout.Space(15);
								GUI.color = new Color (0.9f, 0.5f, 0.5f);
								if ( slotsList[i].materialSample != null && GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
									slotsList[i].materialSample = null;
								}
								GUI.color = Color.white;
								if ( slotsList[i].materialSample != null && GUILayout.Button ( slotsList[i].materialSample.ToString(), Slim, GUILayout.ExpandWidth (true))) {
								}
							}
							if ( slotsList[i].meshRenderer != null ) using (new Horizontal()) {
								GUILayout.Space(15);
								GUI.color = new Color (0.9f, 0.5f, 0.5f);
								if ( slotsList[i].meshRenderer != null && GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
									slotsList[i].meshRenderer = null;
								}
								GUI.color = Color.white;
								if ( slotsList[i].meshRenderer != null && GUILayout.Button ( slotsList[i].meshRenderer.ToString(), Slim, GUILayout.ExpandWidth (true))) {
								}
								
							}
							if ( slotsList[i].Place != null ) using (new Horizontal()) {
								GUILayout.Space(15);
								GUI.color = new Color (0.9f, 0.5f, 0.5f);
								if ( slotsList[i].Place != null && GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
									slotsList[i].Place = null;
								}
								GUI.color = Color.white;
								if ( slotsList[i].Place != null && GUILayout.Button ( slotsList[i].Place.ToString(), Slim, GUILayout.ExpandWidth (true))) {
								}
							}
						}
					
						if ( AddOverlays ) {
							using (new Horizontal()) {
							//	GUI.color = Color.yellow;
								GUILayout.Space(15);
								GUILayout.Label ( "Overlays", "toolbarbutton", GUILayout.ExpandWidth (true));
							}
							if ( slotsList[i]._OverlaysList != null ) {
								if ( slotsList[i]._OverlaysList.Count > 0 ) 
								for(int i1 = 0; i1 < slotsList[i]._OverlaysList.Count; i1 ++){
									using (new Horizontal()) {
										GUILayout.Space(15);
										GUI.color = new Color (0.9f, 0.5f, 0.5f);
										if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
											slotsList[i]._OverlaysList.Remove(slotsList[i]._OverlaysList[i1]);
										}
										GUI.color = Color.white;
										if ( GUILayout.Button ( slotsList[i]._OverlaysList[i1].Name, Slim, GUILayout.ExpandWidth (true))) {
											
										}
									}
									if ( AddData ) 
									for(int i2 = 0; i2 < slotsList[i]._OverlaysList[i1].textureList.Count; i2 ++){
										using (new Horizontal()) {
											GUILayout.Space(30);
											GUI.color = new Color (0.9f, 0.5f, 0.5f);
											if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
												slotsList[i]._OverlaysList[i1].textureList.Remove(slotsList[i]._OverlaysList[i1].textureList[i2]);									
											}
											GUI.color = Color.white;
											if ( GUILayout.Button ( slotsList[i]._OverlaysList[i1].textureList[i2].ToString(), Slim, GUILayout.ExpandWidth (true))) {
												
												
											}
										}
									}
									if ( AddData ) using (new Horizontal()) {
										GUILayout.Space(30);
										GUI.color = new Color (0.9f, 0.5f, 0.5f);
										if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
											slotsList[i]._OverlaysList[i1].Place = null;
										}
										
										GUI.color = Color.white;
										if ( GUILayout.Button ( slotsList[i]._OverlaysList[i1].Place.ToString(), Slim, GUILayout.ExpandWidth (true))) {
											
										}
									}
								}
							}
						}
					}
					if ( SlotsList.Count > 0 ) for(int i = 0; i < SlotsList.Count; i ++){
						
					}
					if ( OverlaysList.Count > 0 ) for(int i = 0; i < OverlaysList.Count; i ++){
						
					}
					if ( DatasList.Count > 0 ) for(int i = 0; i < DatasList.Count; i ++){
						
						
					}
				}
				#endregion Slot List
				#region Overlay List
				if ( _DKOverlayData != null ) using (new ScrollView(ref scroll1)){
					GUILayout.Label ( "Overlays", "toolbarbutton", GUILayout.ExpandWidth (true));
					if ( overlaysList.Count > 0 ) for(int i = 0; i < overlaysList.Count; i ++){
						using (new Horizontal()) {
							GUI.color = new Color (0.9f, 0.5f, 0.5f);
							DKOverlayData tmpDKOverlayData = overlaysList[i].DKOverlayData;
							if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
								overlaysList.Remove(overlaysList[i]);
							}
							GUI.color = Color.white;
							if ( GUILayout.Button ( overlaysList[i].Name, Slim, GUILayout.ExpandWidth (true))) {
							}
							
						}
						if ( AddData ){
							using (new Horizontal()) {
								GUILayout.Space(15);
								GUILayout.Label ( "Datas", "toolbarbutton", GUILayout.ExpandWidth (true));
							}
							for(int i2 = 0; i2 < overlaysList[i].textureList.Count; i2 ++){
								using (new Horizontal()) {
									GUILayout.Space(15);
									GUI.color = new Color (0.9f, 0.5f, 0.5f);
									if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
										overlaysList[i].textureList.Remove(overlaysList[i].textureList[i2]);									
									}
									GUI.color = Color.white;
									if ( GUILayout.Button ( overlaysList[i].textureList[i2].ToString(), Slim, GUILayout.ExpandWidth (true))) {
										
										
									}
								}
							}
							if ( overlaysList[i].Place != null ) using (new Horizontal()) {
								GUILayout.Space(15);
								GUI.color = new Color (0.9f, 0.5f, 0.5f);
								if ( overlaysList[i].Place != null && GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
									overlaysList[i].Place = null;
								}
								GUI.color = Color.white;
								if ( overlaysList[i].Place != null && GUILayout.Button ( overlaysList[i].Place.ToString(), Slim, GUILayout.ExpandWidth (true))) {
								}
							}
						}
					}
					if ( OverlaysList.Count > 0 ) for(int i = 0; i < SlotsList.Count; i ++){
						
					}
					if ( OverlaysList.Count > 0 ) for(int i = 0; i < OverlaysList.Count; i ++){
						
					}
					if ( DatasList.Count > 0 ) for(int i = 0; i < DatasList.Count; i ++){
						
						
					}
				}
				#endregion Overlay List
				#endregion List
				#endregion Export Element(s) Only
			}
			#endregion Actions
		}
	}



	void PrepareExport() {
		// Add selection to the right list
		racesList.Clear();
		overlaysList.Clear();
		slotsList.Clear();
		_tmpRaceData._SlotsList.Clear();
		_tmpRaceData._OverlaysList.Clear();
		_tmpSlotData._OverlaysList.Clear();
		_tmpSlotData._ColorsList.Clear();
		_tmpDKOverlayData._ColorsList.Clear();
		#region Race
		if ( _RaceData != null ){

			_tmpRaceData.Name = _RaceData.raceName;
			_tmpRaceData.RaceData = _RaceData;
			_tmpRaceData.Path = AssetDatabase.GetAssetPath(_RaceData);

			if ( AddColors ){
				_tmpRaceData._ColorsList = _RaceData.ColorPresetDataList;
			}
			if ( AddData ){
				_tmpRaceData.racePrefab = _RaceData.racePrefab;
				_tmpRaceData.TPose = _RaceData.TPose;
			}
			if ( AddSlots ){
				GameObject _DK_UMACrowdObj = GameObject.Find("DKUMACrowd");
				DK_UMACrowd _DK_UMACrowd = _DK_UMACrowdObj.GetComponent<DK_UMACrowd>();
				DKSlotLibrary _DKSlotLibrary = _DK_UMACrowd.slotLibrary;
				for(int i = 0; i < _DKSlotLibrary.slotElementList.Length; i ++){
					if ( _DKSlotLibrary.slotElementList[i].Race.Contains(_RaceData.Race)
					    && (_DKSlotLibrary.slotElementList[i].Gender ==  _RaceData.Gender
					    || _DKSlotLibrary.slotElementList[i].Gender == "Both" )){
						ExportData._slotData tmpData = new ExportData._slotData();
						tmpData.SlotData = _DKSlotLibrary.slotElementList[i];
						tmpData.Name = _DKSlotLibrary.slotElementList[i].slotName;
						tmpData.materialSample = _DKSlotLibrary.slotElementList[i].materialSample;
						tmpData.meshRenderer = _DKSlotLibrary.slotElementList[i].meshRenderer;
						tmpData.Path = AssetDatabase.GetAssetPath(_DKSlotLibrary.slotElementList[i]);
						tmpData.Place = _DKSlotLibrary.slotElementList[i].Place;
						_tmpRaceData._SlotsList.Add(tmpData);
					}
				}
			}
			if ( AddOverlays ){
				GameObject _DK_UMACrowdObj = GameObject.Find("DKUMACrowd");
				DK_UMACrowd _DK_UMACrowd = _DK_UMACrowdObj.GetComponent<DK_UMACrowd>();
				DKOverlayLibrary _OverlayLibrary = _DK_UMACrowd.overlayLibrary;
				for(int i = 0; i < _OverlayLibrary.overlayElementList.Length; i ++){
					if ( _OverlayLibrary.overlayElementList[i].Race.Contains(_RaceData.Race)
					    && (_OverlayLibrary.overlayElementList[i].Gender ==  _RaceData.Gender
					    || _OverlayLibrary.overlayElementList[i].Gender == "Both" )){
						ExportData._overlayData tmpData = new ExportData._overlayData();
						tmpData.DKOverlayData = _OverlayLibrary.overlayElementList[i];
						tmpData.Name = _OverlayLibrary.overlayElementList[i].overlayName;
						tmpData.textureList = _OverlayLibrary.overlayElementList[i].textureList.ToList();
						tmpData.Path = AssetDatabase.GetAssetPath(_OverlayLibrary.overlayElementList[i]);
						tmpData.Place = _OverlayLibrary.overlayElementList[i].Place;
						_tmpRaceData._OverlaysList.Add(tmpData);
					}
				}
			}
			racesList.Add( _tmpRaceData );
			// verify if add data is selected

		}
		#endregion Race
		#region Slot
		if ( _SlotData != null ){
			
			_tmpSlotData.Name = _SlotData.slotName;
			_tmpSlotData.SlotData = _SlotData;
			_tmpSlotData.Path = AssetDatabase.GetAssetPath(_SlotData);
			
			if ( AddColors ){
			}
			if ( AddData ){
				_tmpSlotData.materialSample = _SlotData.materialSample;
				_tmpSlotData.meshRenderer = _SlotData.meshRenderer;
				_tmpSlotData.Place = _SlotData.Place;
			}
		/*	if ( AddSlots ){
				GameObject _DK_UMACrowdObj = GameObject.Find("DKUMACrowd");
				DK_UMACrowd _DK_UMACrowd = _DK_UMACrowdObj.GetComponent<DK_UMACrowd>();
				DKSlotLibrary _DKSlotLibrary = _DK_UMACrowd.slotLibrary;
				for(int i = 0; i < _DKSlotLibrary.slotElementList.Length; i ++){
					if ( _DKSlotLibrary.slotElementList[i].Race.Contains(_RaceData.Race)
					    && (_DKSlotLibrary.slotElementList[i].Gender ==  _RaceData.Gender)){
						ExportData._slotData tmpData = new ExportData._slotData();
						tmpData.SlotData = _DKSlotLibrary.slotElementList[i];
						tmpData.Name = _DKSlotLibrary.slotElementList[i].slotName;
						tmpData.materialSample = _DKSlotLibrary.slotElementList[i].materialSample;
						tmpData.meshRenderer = _DKSlotLibrary.slotElementList[i].meshRenderer;
						tmpData.Path = AssetDatabase.GetAssetPath(_DKSlotLibrary.slotElementList[i]);
						tmpData.Place = _DKSlotLibrary.slotElementList[i].Place;
						//	tmpData._OverlaysList = _DKSlotLibrary.slotElementList[i].LinkedOverlayList;
						_tmpRaceData._SlotsList.Add(tmpData);
					}
				}
			}*/
			if ( AddOverlays ){
				GameObject _DK_UMACrowdObj = GameObject.Find("DKUMACrowd");
				DK_UMACrowd _DK_UMACrowd = _DK_UMACrowdObj.GetComponent<DK_UMACrowd>();
				DKOverlayLibrary _OverlayLibrary = _DK_UMACrowd.overlayLibrary;
				for(int i = 0; i < _OverlayLibrary.overlayElementList.Length; i ++){
					if ( _SlotData.overlayList.Contains(_OverlayLibrary.overlayElementList[i]) ){
						ExportData._overlayData tmpData = new ExportData._overlayData();
						tmpData.DKOverlayData = _OverlayLibrary.overlayElementList[i];
						tmpData.Name = _OverlayLibrary.overlayElementList[i].overlayName;
						tmpData.textureList = _OverlayLibrary.overlayElementList[i].textureList.ToList();
						tmpData.Path = AssetDatabase.GetAssetPath(_OverlayLibrary.overlayElementList[i]);
						tmpData.Place = _OverlayLibrary.overlayElementList[i].Place;
						//	tmpData._OverlaysList = _DKSlotLibrary.overlayElementList[i].LinkedOverlayList;
						_tmpSlotData._OverlaysList.Add(tmpData);
					}
				}
			}
			slotsList.Add( _tmpSlotData );
			// verify if add data is selected
			
		}
		#endregion Race
		#region Overlay
		if ( _DKOverlayData != null ){
			
			_tmpDKOverlayData.Name = _DKOverlayData.overlayName;
			_tmpDKOverlayData.DKOverlayData = _DKOverlayData;
			_tmpDKOverlayData.Path = AssetDatabase.GetAssetPath(_DKOverlayData);
			
			if ( AddColors ){
			//	_tmpDKOverlayData._ColorsList = _DKOverlayData.c;
			}
			if ( AddData ){
				_tmpDKOverlayData.Place = _DKOverlayData.Place;
				_tmpDKOverlayData.textureList = _DKOverlayData.textureList.ToList();
			}
			overlaysList.Add( _tmpDKOverlayData );
			// verify if add data is selected
			
		}
		#endregion Overlay

		// finishing
	//	Export();
	}
		
	void Export() {
		#region first, Add the Export Project
		_Path = AssetDatabase.GetAssetPath(_ExportData);
	//	ExportList.Add(_Path);
		#endregion first, Add the Export Project

		#region Races
		for (int i = 0; i < _ExportData.RacesList.Count; i++) {
			// race element
			_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i].RaceData);
			if ( ExportList.Contains(_Path)== false  ) {
				ExportList.Add(_Path);
				Debug.Log ("Adding "+_ExportData.RacesList[i].RaceData.raceName+" to ExportList");
			}
			// racePrefab
			_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i].racePrefab);
			if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
			// TPose
			_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i].TPose);
			if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);

			#region Races Overlays List 
			for (int i1 = 0; i1 < _ExportData.RacesList[i]._OverlaysList.Count; i1++) {

				// Overlay
				_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._OverlaysList[i1].DKOverlayData);
				if ( ExportList.Contains(_Path)== false  ){
					if ( ExportList.Contains(_Path)== false  ){
						ExportList.Add(_Path);
					//	Debug.Log( "Adding "+_ExportData.RacesList[i]._OverlaysList[i1].DKOverlayData.overlayName );
					}
					// Place
					_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._OverlaysList[i1].Place);
					if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
					// Texture List
					for (int i2 = 0; i2 < _ExportData.RacesList[i]._OverlaysList[i1].textureList.Count; i2++) {
						_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._OverlaysList[i1].textureList[i2]);
						if ( ExportList.Contains(_Path)== false  )ExportList.Add(_Path);
					}
					// Color Presets List
					for (int i2 = 0; i2 < _ExportData.RacesList[i]._OverlaysList[i1]._ColorsList.Count; i2++) {
						_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._OverlaysList[i1]._ColorsList[i2]);
						if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
					}
				}
			}
			#endregion Races Overlays List
			#region Races Slots List
			for (int i1 = 0; i1 < _ExportData.RacesList[i]._SlotsList.Count; i1++) {
				// Slot
				_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._SlotsList[i1].SlotData);
				if ( ExportList.Contains(_Path)== false  ){
					if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
					// Place
					_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._SlotsList[i1].Place);
					if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
					// MeshRenderer
					_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._SlotsList[i1].meshRenderer);
					if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
					// Mesh
					_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._SlotsList[i1].meshRenderer.GetComponent<SkinnedMeshRenderer>().sharedMesh);
					if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
					// materialSample
					_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._SlotsList[i1].materialSample);
					if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
					_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._SlotsList[i1].materialSample);
					if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
					// Overlays List
					for (int i2 = 0; i2 < _ExportData.RacesList[i]._SlotsList[i1]._OverlaysList.Count; i2++) {
						_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._SlotsList[i1]._OverlaysList[i2].DKOverlayData);
						if ( ExportList.Contains(_Path)== false  )ExportList.Add(_Path);
					}
					// Color Presets List
					for (int i2 = 0; i2 < _ExportData.RacesList[i]._SlotsList[i1]._ColorsList.Count; i2++) {
						_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._SlotsList[i1]._ColorsList[i2]);
						if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
					}
				}
			}
			#endregion Races Slots List
			#region Races Color Presets List
			for (int i1 = 0; i1 < _ExportData.RacesList[i]._ColorsList.Count; i1++) {
				_Path = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._ColorsList[i1]);
				if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
			}
			#endregion Races Color Presets List
		}
		#endregion Races

		#region Slots
		for (int i = 0; i < _ExportData.SlotsList.Count; i++) {
			// Place
			_Path = AssetDatabase.GetAssetPath(_ExportData.SlotsList[i].Place);
			if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
			// MeshRenderer
			_Path = AssetDatabase.GetAssetPath(_ExportData.SlotsList[i].meshRenderer);
			if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
			// Mesh
			_Path = AssetDatabase.GetAssetPath(_ExportData.SlotsList[i].meshRenderer.GetComponent<SkinnedMeshRenderer>().sharedMesh);
			if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
			// materialSample
			_Path = AssetDatabase.GetAssetPath(_ExportData.SlotsList[i].materialSample);
			if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
			_Path = AssetDatabase.GetAssetPath(_ExportData.SlotsList[i].materialSample);
			if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
			// Overlays List
			for (int i2 = 0; i2 < _ExportData.SlotsList[i]._OverlaysList.Count; i2++) {
				_Path = AssetDatabase.GetAssetPath(_ExportData.SlotsList[i]._OverlaysList[i2].DKOverlayData);
				// element
				if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
				// Place
				_Path = AssetDatabase.GetAssetPath(_ExportData.SlotsList[i]._OverlaysList[i2].Place);
				if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
				// Texture List
				for (int i3 = 0; i3 < _ExportData.SlotsList[i]._OverlaysList[i2].textureList.Count; i3++) {
					_Path = AssetDatabase.GetAssetPath(_ExportData.SlotsList[i]._OverlaysList[i2].textureList[i3]);
					if ( ExportList.Contains(_Path)== false  )ExportList.Add(_Path);
				}
				// Color Presets List
				for (int i3 = 0; i3 < _ExportData.SlotsList[i]._ColorsList.Count; i3++) {
					_Path = AssetDatabase.GetAssetPath(_ExportData.SlotsList[i]._OverlaysList[i2]._ColorsList[i3]);
					if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
				}
				// element
				_Path = AssetDatabase.GetAssetPath(_ExportData.SlotsList[i]._OverlaysList[i2].DKOverlayData);
				if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
			}
			// Color Presets List
			for (int i2 = 0; i2 < _ExportData.SlotsList[i]._ColorsList.Count; i2++) {
				_Path = AssetDatabase.GetAssetPath(_ExportData.SlotsList[i]._ColorsList[i2]);
				if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
			}

		}

		#endregion Slots

		#region Overlays
		for (int i = 0; i < _ExportData.OverlaysList.Count; i++) {
			// Place
			_Path = AssetDatabase.GetAssetPath(_ExportData.OverlaysList[i].Place);
			if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
			// Texture List
			for (int i2 = 0; i2 < _ExportData.OverlaysList[i].textureList.Count; i2++) {
				_Path = AssetDatabase.GetAssetPath(_ExportData.OverlaysList[i].textureList[i2]);
				if ( ExportList.Contains(_Path)== false  )ExportList.Add(_Path);
			}
			// Color Presets List
			for (int i2 = 0; i2 < _ExportData.OverlaysList[i]._ColorsList.Count; i2++) {
				_Path = AssetDatabase.GetAssetPath(_ExportData.OverlaysList[i]._ColorsList[i2]);
				if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
			}
			// element
			_Path = AssetDatabase.GetAssetPath(_ExportData.OverlaysList[i].DKOverlayData);
			if ( ExportList.Contains(_Path)== false  ) ExportList.Add(_Path);
		}
		#endregion Overlays
		// finishing
		_Path = AssetDatabase.GetAssetPath(_ExportData);
		string _NewPath = "Assets/DK Editors/DK_UMA_Editor/Exporter/Incoming/" + _ExportData.name+".asset";
		AssetDatabase.MoveAsset(_Path, _NewPath);
		EditorUtility.SetDirty(_ExportData);
		AssetDatabase.SaveAssets();
		ExportList.Add(_NewPath);
		_ExportData.ExportsPathList = ExportList;
		// Creating the Package
		if (ExportList.Count > 0) {
			string[] _Array = ExportList.ToArray ();
			_SavePath = EditorUtility.SaveFilePanel ("Export Package", "", "DK UMA-" + _Selection + ".unitypackage", "unitypackage");
			AssetDatabase.ExportPackage (_Array, _SavePath, ExportPackageOptions.Interactive);
		}

		this.Close();
	}
	void PrepareExportToGrp() {

	}


	void OnSelectionChange() {
		_RaceData = null;
		_DKOverlayData = null;
		_SlotData = null;
		racesList.Clear();
		slotsList.Clear();
		overlaysList.Clear();
		ExpName = "";
		_RaceData = Selection.activeObject as DKRaceData;
		_DKOverlayData = Selection.activeObject as DKOverlayData;
		_SlotData = Selection.activeObject as DKSlotData;
		_DKUMAData = Selection.activeObject as DKUMAData;
		ExportList.Clear();
		if (Selection.activeObject && Selection.activeObject.GetType().ToString() == "ExportData" ) {
			ExpName = (Selection.activeObject as ExportData).PackageName;

		}
		else
		if ( _RaceData != null ){
			_GenderType = _RaceData.Gender;

		}

		Repaint();
	}

}
