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

public class DK_UMA_Import_Win : EditorWindow {
	Vector2 scroll;

	public static string Action = "Importing";
	string Selected = "None";

	bool ShowProjects = true;
	bool ShowContent = false;
	bool ShowRaces = false;
	bool ShowSlots = false;
	bool ShowOverlays = false;
	bool ShowModels = false;

	public static ExportData _ExportData;

	public static List<ExportData> ColorsList = new List<ExportData>();

	Color Green = new Color (0.8f, 1f, 0.8f, 1);
	Color Red = new Color (0.9f, 0.5f, 0.5f);

	void PrepareList (){


	}

	void OpenDetailsWin (){
		GetWindow(typeof(Import_Content_Details_Win), false, "Content");
	}
	public void OpenDeleteAsset(){
		GetWindow(typeof(DeleteAssetImport), false, "Deleting");
		DeleteAssetImport.Action = "Delete Package";
		DeleteAssetImport.AssetName = Selected;
	}

	void OnGUI () {
		this.minSize = new Vector2 (300, 500);
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


		GUILayout.Label ( Action, "toolbarbutton", GUILayout.ExpandWidth (true));
		using (new Horizontal()) {
			GUILayout.Label ( "Selected :", GUILayout.ExpandWidth (false));
			GUILayout.Label ( Selected, bold, GUILayout.ExpandWidth (true));
		}
		#region Importing Export Project
		if ( Action == "Importing" ){
			if ( Selected != "None" ){
				// search and Select the project
				UnityEngine.Object[] tmpObjs = AssetDatabase.LoadAllAssetsAtPath( "Assets/DK Editors/DK_UMA_Editor/Exporter/Incoming/"+Selected);
				for(int i = 0; i < tmpObjs.Length; i ++){
					if ( tmpObjs[i].name+".asset" == Selected ){
						Selection.activeObject = tmpObjs[i];
					}
				}
				if ( Selection.activeObject && Selection.activeObject.GetType().ToString() == "ExportData"  ){
					_ExportData = Selection.activeObject as ExportData;
					if ( _ExportData.Status == "" ) _ExportData.Status = "Not Installed";
					using (new Horizontal()) {
						GUILayout.Label ( "Statut :", GUILayout.Width (40));
						GUILayout.Label ( _ExportData.Status, GUILayout.Width (80));
						GUI.color = Green;
						if ( _ExportData.Status == "Not Installed" && GUILayout.Button ( "Install All", GUILayout.ExpandWidth (true))) {
							InstallAll ();
						}
						if ( _ExportData.Status == "Installed" && GUILayout.Button ( "Verify", GUILayout.ExpandWidth (true))) {
							
						}
						GUI.color = Color.yellow;
						if ( _ExportData.Status == "Installed" && GUILayout.Button ( "Remove", GUILayout.ExpandWidth (true))) {
							Remove();
						}
						GUI.color = Red;
						if ( _ExportData.Status == "Installed" && GUILayout.Button ( "Del", GUILayout.ExpandWidth (true))) {
							OpenDeleteAsset();
							DeleteAssetImport.ProcessName = "Delete Import Project";
						}
					}

					using (new Horizontal()) {
						GUILayout.Label ( "Type :", GUILayout.Width (80));
						GUILayout.Label ( _ExportData.PackageType, GUILayout.ExpandWidth (true));
					}

					// actions
					GUILayout.Space(5);
					using (new Horizontal()) {

						GUI.color = Color.white;
						if ( GUILayout.Button ( "View Content", GUILayout.ExpandWidth (true))) {
							if ( ShowContent == true )ShowContent = false;
							else ShowContent = true;
							ShowProjects = false;
						}
					}

					#region Content List
					GUILayout.Space(5);
					using (new Horizontal()) {
						GUI.color = Color.white;
						GUILayout.Label ( "Content List", "toolbarbutton", GUILayout.ExpandWidth (true));
						if ( ShowContent == true ) GUI.color = Color.white;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))) {
							if ( ShowContent == true )ShowContent = false;
							else ShowContent = true;
							ShowProjects = false;
						}
					}

					if ( ShowContent == true )using (new ScrollView(ref scroll)) {
						#region Races
						if ( _ExportData.RacesList.Count > 0 ) using (new Horizontal()) {
							GUI.color = Color.yellow;
							GUILayout.Label ( "Races :", "toolbarbutton", GUILayout.ExpandWidth (true));
							GUILayout.Label ( _ExportData.RacesList.Count.ToString(), "toolbarbutton", GUILayout.Width (30));
							if ( ShowRaces == true ) GUI.color = Color.white;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))) {
								if ( ShowRaces == true )ShowRaces = false;
								else ShowRaces = true;
							}
						}
						// Show Races
						if ( _ExportData.RacesList.Count > 0 ) if ( ShowRaces == true ){
							for (int i = 0; i < _ExportData.RacesList.Count ; i++){
								string Statut = "";
								if ( Statut == null ) Statut = "";
								if ( EditorVariables.DK_UMACrowd.raceLibrary.raceElementList.ToList()
								    .Contains(_ExportData.RacesList[i].RaceData) == true ){
									Statut = "Ready";
								} else Statut = "Install";
								using (new Horizontal()) {
									GUILayout.Space(15);
									GUILayout.Label ( _ExportData.RacesList[i].Name, "foldout", GUILayout.Width (170));
									if ( GUILayout.Button ( "Details", GUILayout.ExpandWidth (false))) {
										OpenDetailsWin ();
										Import_Content_Details_Win._Selection = _ExportData.RacesList[i].Name;
										Import_Content_Details_Win._Type = "DKRaceData";
										Import_Content_Details_Win.i = i;

									}
									if ( Statut == "Ready" ) GUI.color = Green;
									else GUI.color = Color.yellow;
									if ( GUILayout.Button ( Statut, GUILayout.ExpandWidth (false))) {

									}
								}
							}
						}
						#endregion Races
						#region Slots
						if ( _ExportData.SlotsList.Count > 0 ) using (new Horizontal()) {
							GUI.color = Color.yellow;
							GUILayout.Label ( "Slots :", "toolbarbutton", GUILayout.ExpandWidth (true));
							GUILayout.Label ( _ExportData.SlotsList.Count.ToString(), "toolbarbutton", GUILayout.Width (30));
							if ( ShowSlots == true ) GUI.color = Color.white;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))) {
								if ( ShowSlots == true )ShowSlots = false;
								else ShowSlots = true;
							}
						}
						// Show Slots
						if ( _ExportData.SlotsList.Count > 0 ) if ( ShowSlots == true ){
							for (int i = 0; i < _ExportData.SlotsList.Count ; i++){
								string Statut = "";
								if ( Statut == null ) Statut = "";
								if ( EditorVariables.DK_UMACrowd.slotLibrary.slotElementList.ToList()
								    .Contains(_ExportData.SlotsList[i].SlotData) == true ){
									Statut = "Ready";
								} else Statut = "Install";
								using (new Horizontal()) {
									GUILayout.Space(15);
									GUILayout.Label ( _ExportData.SlotsList[i].Name, "foldout", GUILayout.Width (170));
									if ( GUILayout.Button ( "Details", GUILayout.ExpandWidth (false))) {
										OpenDetailsWin ();
										Import_Content_Details_Win._Selection = _ExportData.SlotsList[i].Name;
										Import_Content_Details_Win._Type = "DKSlotData";
										Import_Content_Details_Win.i = i;
									}
									if ( Statut == "Ready" ) GUI.color = Green;
									else GUI.color = Color.yellow;
									if ( GUILayout.Button ( Statut, GUILayout.ExpandWidth (false))) {
										
									}
								}
							}
						}
						#endregion Slots
						#region Overlays
						if ( _ExportData.OverlaysList.Count > 0 )using (new Horizontal()) {
							GUI.color = Color.yellow;
							GUILayout.Label ( "Overlays :", "toolbarbutton", GUILayout.ExpandWidth (true));
							GUILayout.Label ( _ExportData.OverlaysList.Count.ToString(), "toolbarbutton", GUILayout.Width (30));
							if ( ShowOverlays == true ) GUI.color = Color.white;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))) {
									if ( ShowOverlays == true )ShowOverlays = false;
								else ShowOverlays = true;
							}
						}
						// Show Overlays
						if ( _ExportData.OverlaysList.Count > 0 ) if ( ShowOverlays == true ){
							for (int i = 0; i < _ExportData.OverlaysList.Count ; i++){
								string Statut = "";
								if ( Statut == null ) Statut = "";
								if ( EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList.ToList()
								    .Contains(_ExportData.OverlaysList[i].DKOverlayData) == true ){
									Statut = "Ready";
								} else Statut = "Install";
								using (new Horizontal()) {
									GUILayout.Space(15);
									GUILayout.Label ( _ExportData.OverlaysList[i].Name, "foldout", GUILayout.Width (170));
									if ( GUILayout.Button ( "Details", GUILayout.ExpandWidth (false))) {
										OpenDetailsWin ();
										Import_Content_Details_Win._Selection = _ExportData.OverlaysList[i].Name;
										Import_Content_Details_Win._Type = "DKOverlayData";
										Import_Content_Details_Win.i = i;

									}
									if ( Statut == "Ready" ) GUI.color = Green;
									else GUI.color = Color.yellow;
									if ( GUILayout.Button ( Statut, GUILayout.ExpandWidth (false))) {
										
									}
								}
							}
						}
						#endregion Overlays
						#region Models
						if ( _ExportData.ModelsList.Count > 0 ) using (new Horizontal()) {
							GUI.color = Color.yellow;
							GUILayout.Label ( "Models :", "toolbarbutton", GUILayout.ExpandWidth (true));
							GUILayout.Label ( _ExportData.ModelsList.Count.ToString(), "toolbarbutton", GUILayout.Width (30));
							if ( ShowModels == true ) GUI.color = Color.white;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))) {
								if ( ShowModels == true )ShowModels = false;
								else ShowModels = true;
							}
						}
						#endregion Models
					}
					#endregion Content List
				}
			}
			else {
				GUI.color = Color.yellow;
				GUILayout.Label ( "Select from the projects list.", GUILayout.ExpandWidth (true));
			}

			#region Import Projects List
			GUILayout.Space(5);
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label ( "Import Projects List", "toolbarbutton", GUILayout.ExpandWidth (true));
				if ( ShowProjects == true ) GUI.color = Color.white;
				else GUI.color = Color.gray;
				if ( GUILayout.Button ( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))) {
					if ( ShowProjects == true )ShowProjects = false;
					else ShowProjects = true;
					ShowContent = false;
				}
			}
			if ( ShowProjects == true )using (new ScrollView(ref scroll)) {
				GUI.color = Color.white;
				string _path = ("Assets/DK Editors/DK_UMA_Editor/Exporter/Incoming/");
				string[] aFilePaths = Directory.GetFiles(_path);
				foreach (string sFilePath in aFilePaths) {
					string FileName = sFilePath.Replace( "Assets/DK Editors/DK_UMA_Editor/Exporter/Incoming/", "" );
					if ( FileName == Selected ) GUI.color = Green;
					else GUI.color = Color.white;
					if( FileName.Contains(".asset.meta") == false ){
						using (new Horizontal()) {
							if ( GUILayout.Button (FileName, GUILayout.ExpandWidth (true))) {
								Selected = FileName;
								
							}
							UnityEngine.Object[] tmpObjs = AssetDatabase.LoadAllAssetsAtPath( "Assets/DK Editors/DK_UMA_Editor/Exporter/Incoming/"+FileName);
							for(int i = 0; i < tmpObjs.Length; i ++){
								if ( tmpObjs[i].name+".asset" == FileName ){
									_ExportData = tmpObjs[i] as ExportData;
								}
							}
							GUILayout.Label ( _ExportData.Status, GUILayout.ExpandWidth (false));
						}
					}
				}
			}
			#endregion Import Projects List
		}
		#endregion Importing Export Project

		#region Choose Export Project
		if (Action == "Choose Export Project") {
			GUI.color = Green;
			if (Selected != "None" && GUILayout.Button ("Assign", GUILayout.ExpandWidth (true))) {
				DK_UMA_Export_Win._ExportDataName = Selected;
				Repaint ();
				this.Close ();
			}
			GUI.color = Color.white;
			GUILayout.Space (5);
			GUILayout.Label ("Select from the list.", GUILayout.ExpandWidth (true));
			#endregion Choose Export Project

			#region Export Projects List
			GUILayout.Space (5);
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label ("Export Projects List", "toolbarbutton", GUILayout.ExpandWidth (true));
				if (ShowProjects == true) GUI.color = Color.white;
				else GUI.color = Color.gray;
				if (GUILayout.Button ("Show", "toolbarbutton", GUILayout.ExpandWidth (false))) {
					if (ShowProjects == true) ShowProjects = false;
					else ShowProjects = true;
				}
			}
			if (ShowProjects == true)
			using (new ScrollView(ref scroll)) {
				GUI.color = Color.white;
				string _path = ("Assets/DK Editors/DK_UMA_Editor/Exporter/Exporting/");
				string[] aFilePaths = Directory.GetFiles (_path);
				foreach (string sFilePath in aFilePaths) {
					string FileName = sFilePath.Replace ("Assets/DK Editors/DK_UMA_Editor/Exporter/Exporting/", "");
					if (FileName == Selected) GUI.color = Green;
					else GUI.color = Color.white;
					if (FileName.Contains (".asset.meta") == false) {
						using (new Horizontal()) {
							if (GUILayout.Button (FileName, GUILayout.ExpandWidth (true))) {
								Selected = FileName;

							}
							if ( _ExportData.Status == "Installed" ){
								GUI.color = Green;
								if (GUILayout.Button ("Ok", GUILayout.ExpandWidth (false))) {}
							}
						}
					}
				}
			}
			#endregion Export Projects List
		}
	}
	void InstallAll (){
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
			if (raceLibraryList.Contains(_ExportData.RacesList[i].RaceData) == false ){
				raceLibraryList.Add(_ExportData.RacesList[i].RaceData);
			}
			// Dependencies : Slots
			for (int i1 = 0; i1 < _ExportData.RacesList[i]._SlotsList.Count; i1++) {
				if (SlotLibraryList.Contains(_ExportData.RacesList[i]._SlotsList[i1].SlotData) == false ){
					SlotLibraryList.Add(_ExportData.RacesList[i]._SlotsList[i1].SlotData);
				}
				// Slot's Overlays
				for (int i2 = 0; i2 < _ExportData.RacesList[i]._SlotsList[i1]._OverlaysList.Count; i2++) {
					if (overlayLibraryList.Contains(_ExportData.RacesList[i]._SlotsList[i1]._OverlaysList[i2].DKOverlayData) == false ){
						overlayLibraryList.Add(_ExportData.RacesList[i]._SlotsList[i1]._OverlaysList[i2].DKOverlayData);
					}
				}
				// Slot's Place
				if (AnatoLibraryList.Contains(_ExportData.RacesList[i]._SlotsList[i1].Place) == false ){
					AnatoLibraryList.Add(_ExportData.RacesList[i]._SlotsList[i1].Place);
				}
			}
		}
		// For Slots
		for (int i = 0; i < _ExportData.SlotsList.Count; i++) {
			if (SlotLibraryList.Contains(_ExportData.SlotsList[i].SlotData) == false ){
				SlotLibraryList.Add(_ExportData.SlotsList[i].SlotData);
			}
			// Dependencies : Overlays
			for (int i2 = 0; i2 < _ExportData.SlotsList[i]._OverlaysList.Count; i2++) {
				if (overlayLibraryList.Contains(_ExportData.SlotsList[i]._OverlaysList[i2].DKOverlayData) == false ){
					overlayLibraryList.Add(_ExportData.SlotsList[i]._OverlaysList[i2].DKOverlayData);
				}
			}
			// Slot's Place
			if (AnatoLibraryList.Contains(_ExportData.SlotsList[i].Place) == false ){
				AnatoLibraryList.Add(_ExportData.SlotsList[i].Place);
			}
		}
		// For Overlays
		for (int i = 0; i < _ExportData.OverlaysList.Count; i++) {
			if (overlayLibraryList.Contains(_ExportData.OverlaysList[i].DKOverlayData) == false ){
				overlayLibraryList.Add(_ExportData.OverlaysList[i].DKOverlayData);
			}
			// Slot's Place
			if (AnatoLibraryList.Contains(_ExportData.OverlaysList[i].Place) == false ){
				AnatoLibraryList.Add(_ExportData.OverlaysList[i].Place);
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
		EditorUtility.SetDirty(_ExportData);

		_ExportData.Status = "Installed";
		Debug.Log ("The elements of "+_ExportData.PackageName+" have been installed to your project's libraries.");
	}
	void Remove(){
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
			}
			// Dependencies : Slots
			for (int i1 = 0; i1 < _ExportData.RacesList[i]._SlotsList.Count; i1++) {
				if (SlotLibraryList.Contains(_ExportData.RacesList[i]._SlotsList[i1].SlotData) == true ){
					SlotLibraryList.Remove(_ExportData.RacesList[i]._SlotsList[i1].SlotData);
				}
				// Slot's Overlays
				for (int i2 = 0; i2 < _ExportData.RacesList[i]._SlotsList[i1]._OverlaysList.Count; i2++) {
					if (overlayLibraryList.Contains(_ExportData.RacesList[i]._SlotsList[i1]._OverlaysList[i2].DKOverlayData) == false ){
						overlayLibraryList.Remove(_ExportData.RacesList[i]._SlotsList[i1]._OverlaysList[i2].DKOverlayData);
					}
				}
				// Slot's Place
				if (AnatoLibraryList.Contains(_ExportData.RacesList[i]._SlotsList[i1].Place) == true ){
					AnatoLibraryList.Remove(_ExportData.RacesList[i]._SlotsList[i1].Place);
				}
			}
		}
		// For Slots
		for (int i = 0; i < _ExportData.SlotsList.Count; i++) {
			if (SlotLibraryList.Contains(_ExportData.SlotsList[i].SlotData) == true ){
				SlotLibraryList.Remove(_ExportData.SlotsList[i].SlotData);
			}
			// Dependencies : Overlays
			for (int i2 = 0; i2 < _ExportData.SlotsList[i]._OverlaysList.Count; i2++) {
				if (overlayLibraryList.Contains(_ExportData.SlotsList[i]._OverlaysList[i2].DKOverlayData) == true ){
					overlayLibraryList.Remove(_ExportData.SlotsList[i]._OverlaysList[i2].DKOverlayData);
				}
			}
			// Slot's Place
			if (AnatoLibraryList.Contains(_ExportData.SlotsList[i].Place) == true ){
				AnatoLibraryList.Remove(_ExportData.SlotsList[i].Place);
			}
		}
		// For Overlays
		for (int i = 0; i < _ExportData.OverlaysList.Count; i++) {
			if (overlayLibraryList.Contains(_ExportData.OverlaysList[i].DKOverlayData) == true ){
				overlayLibraryList.Remove(_ExportData.OverlaysList[i].DKOverlayData);
			}
			// Slot's Place
			if (AnatoLibraryList.Contains(_ExportData.OverlaysList[i].Place) == true ){
				AnatoLibraryList.Remove(_ExportData.OverlaysList[i].Place);
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
		Debug.Log ("The elements of "+_ExportData.PackageName+" have been removed from your project's libraries.");
	}
}
