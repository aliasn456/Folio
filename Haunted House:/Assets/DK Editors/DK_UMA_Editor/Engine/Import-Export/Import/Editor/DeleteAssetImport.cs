using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public class DeleteAssetImport : EditorWindow {

	public static string ProcessName = "";
	public static float ProcessNumber = 0;
	public static float ProcessCurrent = 0;

	public static bool Processing = false;
	public static bool MultiUMAModel = false;
	public static bool UMAModel = false;
	public static string myPath;
	public static string Action;
	public static string AssetName = "";
	bool AdvDel = false;
	bool DelRaces;
	bool DelSlots;
	bool DelOverlays;
	bool DelPresets;
	Color Green = new Color (0.8f, 1f, 0.8f, 1);
	Color Red = new Color (0.9f, 0.5f, 0.5f);

	void CloseSelf(){
		try{
		Action = "";
		this.Close();
		}catch(NullReferenceException){}
	}
	public static void OpenProcessWindow()
	{
		GetWindow(typeof(DKUMAProcessWindow), false, "Processing...");
		DKUMAProcessWindow.ProcessName = "Create Prefabs";
		DKUMAProcessWindow.Processing = true;
		DKUMAProcessWindow.ProcessCurrent = 0;
		DKUMAProcessWindow.ProcessNumber = EditorVariables.MFSelectedList.childCount;
	}
	void OnGUI () {
		this.minSize = new Vector2(300, 150);
		this.maxSize = new Vector2(305, 155);
		Repaint();

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

		using (new HorizontalCentered()) {
			GUILayout.Label ( ProcessName, bold, GUILayout.ExpandWidth (true));

		}
		if ( Action != "DelByPath" && Selection.activeObject && ProcessName != "Delete all models" )using (new HorizontalCentered())GUILayout.Label ( Selection.activeObject.name, bold, GUILayout.ExpandWidth (false));
		if ( Action == "DelByPath" && ProcessName != "Delete all models" )using (new HorizontalCentered())GUILayout.Label ( AssetName, bold, GUILayout.ExpandWidth (false));
		GUI.color = Red;
		if ( !AdvDel ) {
			GUILayout.Space(20);
			if ( GUILayout.Button ( "Advanced Delete", GUILayout.ExpandWidth (true))){
				AdvDel = true;
			}
		}
		if ( AdvDel ){
			GUILayout.Space(2);
			using (new Horizontal()) {
				GUILayout.Label ( "Include dependencies ?", GUILayout.ExpandWidth (true));
			}
			using (new Horizontal()) {
			/*	if ( DelRaces ) GUI.color = Red;
				else GUI.color = Color.gray;
				if ( GUILayout.Button ( "Races", GUILayout.ExpandWidth (true))){
					if ( DelRaces ) DelRaces = false;
					else DelRaces = true;
				}*/
				if ( DelSlots ) GUI.color = Red;
				else GUI.color = Color.gray;
				if ( GUILayout.Button ( "Slots", GUILayout.ExpandWidth (true))){
					if ( DelSlots ) DelSlots = false;
					else DelSlots = true;
				}
				if ( DelOverlays ) GUI.color = Red;
				else GUI.color = Color.gray;
				if ( GUILayout.Button ( "Overlays", GUILayout.ExpandWidth (true))){
					if ( DelOverlays ) DelOverlays = false;
					else DelOverlays = true;
				}
				if ( DelPresets ) GUI.color = Green;
				else GUI.color = Color.gray;
				if ( GUILayout.Button ( "Colors", GUILayout.ExpandWidth (true))){
					if ( DelPresets ) DelPresets = false;
					else DelPresets = true;
				}
			}
		}
		GUILayout.Space(20);
		using (new HorizontalCentered())GUILayout.Label ( "Are you sure ?", GUILayout.ExpandWidth (false));
		using (new Horizontal()) {
			GUILayout.Space(20);
			GUI.color = new Color (0.9f, 0.5f, 0.5f);
			if ( GUILayout.Button ( "Yes, Delete.", GUILayout.ExpandWidth (true))){
				Delete();
			}
			GUILayout.Space(20);
			GUI.color = new Color (0.8f, 1f, 0.8f, 1);
			if ( GUILayout.Button ( "No, Cancel", GUILayout.ExpandWidth (true))){
				CloseSelf();
			}
			GUILayout.Space(20);
		}
	}
	void Clearing(){

	}
	void Delete(){
		if ( ProcessName == "Delete Import Project" ) {
			ExportData _ExportData = Selection.activeObject as ExportData;
			// libraries
			List<DKRaceData> raceLibraryList = new List<DKRaceData>();
			raceLibraryList = EditorVariables.DK_UMACrowd.raceLibrary.raceElementList.ToList ();
			List<DKSlotData> SlotLibraryList = new List<DKSlotData>();
			SlotLibraryList = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList.ToList ();
			List<DKOverlayData> overlayLibraryList = new List<DKOverlayData>();
			overlayLibraryList = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList.ToList ();
			List<DK_SlotsAnatomyElement> AnatoLibraryList = new List<DK_SlotsAnatomyElement>();
			AnatoLibraryList = EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.ToList ();
			// For Races
			for (int i = 0; i < _ExportData.RacesList.Count; i++) {
				if (raceLibraryList.Contains(_ExportData.RacesList[i].RaceData) == true ){
					raceLibraryList.Remove(_ExportData.RacesList[i].RaceData);
					string tmpPath = AssetDatabase.GetAssetPath(_ExportData.RacesList[i].RaceData);
					AssetDatabase.DeleteAsset(tmpPath );

					
				}
				// Dependencies : Slots
				if ( DelSlots ) for (int i1 = 0; i1 < _ExportData.RacesList[i]._SlotsList.Count; i1++) {
					if (SlotLibraryList.Contains(_ExportData.RacesList[i]._SlotsList[i1].SlotData) == true ){
						SlotLibraryList.Remove(_ExportData.RacesList[i]._SlotsList[i1].SlotData);
						string tmpPath = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._SlotsList[i1].SlotData);
						AssetDatabase.DeleteAsset(tmpPath );
					}
					// Slot's Overlays
					if ( DelOverlays ) for (int i2 = 0; i2 < _ExportData.RacesList[i]._SlotsList[i1]._OverlaysList.Count; i2++) {
						if (overlayLibraryList.Contains(_ExportData.RacesList[i]._SlotsList[i1]._OverlaysList[i2].DKOverlayData) == false ){
							overlayLibraryList.Remove(_ExportData.RacesList[i]._SlotsList[i1]._OverlaysList[i2].DKOverlayData);
							string tmpPath = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._SlotsList[i1]._OverlaysList[i2].DKOverlayData);
							AssetDatabase.DeleteAsset(tmpPath );
						}
					}
					// Slot's Place
					if (AnatoLibraryList.Contains(_ExportData.RacesList[i]._SlotsList[i1].Place) == true ){
					//	AnatoLibraryList.Remove(_ExportData.RacesList[i]._SlotsList[i1].Place);
					}
				}
				// Dependencies : Colors
				if ( DelPresets ) for (int i1 = 0; i1 < _ExportData.RacesList[i]._ColorsList.Count; i1++) {
					string tmpPath = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._ColorsList[i1]);
					AssetDatabase.DeleteAsset(tmpPath );
				}
				// Dependencies : Overlays
				if ( DelOverlays ) for (int i1 = 0; i1 < _ExportData.RacesList[i]._OverlaysList.Count; i1++) {
					overlayLibraryList.Remove(_ExportData.RacesList[i]._OverlaysList[i1].DKOverlayData);
					// textures
					for (int i2 = 0; i2 < _ExportData.RacesList[i]._OverlaysList[i1].textureList.Count; i2++) {
						string tmpPath2 = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._OverlaysList[i1].textureList[i2]);
						AssetDatabase.DeleteAsset(tmpPath2 );
					}
					string tmpPath = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._OverlaysList[i1].DKOverlayData);
					AssetDatabase.DeleteAsset(tmpPath );
				}
			}
			// For Slots
			for (int i = 0; i < _ExportData.SlotsList.Count; i++) {
				if (SlotLibraryList.Contains(_ExportData.SlotsList[i].SlotData) == true ){
					SlotLibraryList.Remove(_ExportData.SlotsList[i].SlotData);
					string tmpPath = AssetDatabase.GetAssetPath(_ExportData.SlotsList[i].SlotData);
					AssetDatabase.DeleteAsset(tmpPath );
				}
				// Dependencies : Overlays
				if ( DelOverlays ) for (int i2 = 0; i2 < _ExportData.SlotsList[i]._OverlaysList.Count; i2++) {
					if (overlayLibraryList.Contains(_ExportData.SlotsList[i]._OverlaysList[i2].DKOverlayData) == true ){
						overlayLibraryList.Remove(_ExportData.SlotsList[i]._OverlaysList[i2].DKOverlayData);
						// textures
						for (int i3 = 0; i3 < _ExportData.RacesList[i]._OverlaysList[i2].textureList.Count; i3++) {
							string tmpPath2 = AssetDatabase.GetAssetPath(_ExportData.RacesList[i]._OverlaysList[i2].textureList[i3]);
							AssetDatabase.DeleteAsset(tmpPath2 );
						}
						string tmpPath = AssetDatabase.GetAssetPath(_ExportData.SlotsList[i]._OverlaysList[i2].DKOverlayData);
						AssetDatabase.DeleteAsset(tmpPath );

					}
				}
				// Slot's Place
				if (AnatoLibraryList.Contains(_ExportData.SlotsList[i].Place) == true ){
				//	AnatoLibraryList.Remove(_ExportData.SlotsList[i].Place);
				}
			}
			// For Overlays
			for (int i = 0; i < _ExportData.OverlaysList.Count; i++) {
				if (overlayLibraryList.Contains(_ExportData.OverlaysList[i].DKOverlayData) == true ){
					overlayLibraryList.Remove(_ExportData.OverlaysList[i].DKOverlayData);
					string tmpPath = AssetDatabase.GetAssetPath(_ExportData.OverlaysList[i].DKOverlayData);
					AssetDatabase.DeleteAsset(tmpPath );
				}
				// Slot's Place
				if (AnatoLibraryList.Contains(_ExportData.OverlaysList[i].Place) == true ){
				//	AnatoLibraryList.Remove(_ExportData.OverlaysList[i].Place);
				}
			}
			EditorVariables.DK_UMACrowd.raceLibrary.raceElementList = raceLibraryList.ToArray();
			EditorUtility.SetDirty(EditorVariables.DK_UMACrowd.raceLibrary.transform);
			EditorVariables.DK_UMACrowd.slotLibrary.slotElementList = SlotLibraryList.ToArray();
			EditorUtility.SetDirty(EditorVariables.DK_UMACrowd.slotLibrary.transform);
			EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList = overlayLibraryList.ToArray();
			EditorUtility.SetDirty(EditorVariables.DK_UMACrowd.overlayLibrary.transform);
			EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList = AnatoLibraryList.ToArray();
			EditorUtility.SetDirty(EditorVariables._SlotsAnatomyLibrary.transform);
			
			AssetDatabase.SaveAssets();
			
			_ExportData.Status = "Not Installed";
			Debug.Log ("The elements of "+_ExportData.name+" have been deleted from your project.");
			AssetDatabase.DeleteAsset("Assets/DK Editors/DK_UMA_Editor/Exporter/Incoming/"+_ExportData.name+".asset" );
			CloseSelf();
		}
	}



	void OnSelectionChange() {
		CloseSelf();
	}
}
