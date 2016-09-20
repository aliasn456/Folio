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

public class Import_Content_Details_Win : EditorWindow {
	Color Green = new Color (0.8f, 1f, 0.8f, 1);
	Color Red = new Color (0.9f, 0.5f, 0.5f);
	
	Vector2 scroll;

	public static DKRaceData _RaceData;
	public static DKOverlayData _DKOverlayData;
	public static DKSlotData _SlotData;

	public static string _Selection = "";
	public static string _Type = "";
	public static int i = 0;


	void OnGUI () {
		this.minSize = new Vector2 (330, 500);
		Repaint ();
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

		#region Variables

		#endregion Variables

		using (new Horizontal()) {
			GUILayout.Label ( "Importer Content Details", "toolbarbutton", GUILayout.ExpandWidth (true));
		}
		
		GUILayout.Space(5);	
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
			#endregion Infos
		// list
		GUILayout.Label ( "Content List", "toolbarbutton", GUILayout.ExpandWidth (true));
		#region Race
		// Race
		if ( _Type == "DKRaceData" ) using (new ScrollView(ref scroll)) {
			string Statut = "";
			if ( Statut == null ) Statut = "";
			if (DK_UMA_Import_Win._ExportData != null
				&& DK_UMA_Import_Win._ExportData.RacesList [i].RaceData != null) 
			{

				// Dependencies : Slots
				if ( DK_UMA_Import_Win._ExportData.RacesList[i]._SlotsList.Count > 0 ){
					using (new Horizontal()) {
						GUILayout.Space (25);
						GUILayout.Label ("Dependencies : Slots", "foldout", GUILayout.Width (170));
					}
					for (int i1 = 0; i1 < DK_UMA_Import_Win._ExportData.RacesList[i]._SlotsList.Count; i1 ++) {
						string Statut2 = "";
						if (Statut2 == null) Statut = "";
						if (EditorVariables.DK_UMACrowd.slotLibrary.slotElementList.ToList().Contains (DK_UMA_Import_Win._ExportData.RacesList [i]._SlotsList [i1].SlotData) == true) {
							Statut2 = "Ok";
						} 
						else Statut2 = "Add";
						using (new Horizontal()) {
							GUILayout.Space (35);
							if (Statut2 == "Ok")GUI.color = Green;
							else GUI.color = Color.yellow;
							if (GUILayout.Button (Statut2, "toolbarbutton", GUILayout.Width (40))) {
								
							}
							GUI.color = Color.white;
							if (GUILayout.Button (DK_UMA_Import_Win._ExportData.RacesList [i]._SlotsList [i1].Name, Slim, GUILayout.ExpandWidth (true))) {
			
							}
						}
					}
				}
				else GUILayout.Label ("- The Race has no Overlay Dependencies.", GUILayout.Width (300));
				// Dependencies : Overlays
					if ( DK_UMA_Import_Win._ExportData.RacesList[i]._OverlaysList.Count > 0 ){
					using (new Horizontal()) {
						GUILayout.Space (25);
						GUILayout.Label ("Dependencies : Overlays", "foldout", GUILayout.Width (170));
					}
						for (int i1 = 0; i1 < DK_UMA_Import_Win._ExportData.RacesList[i]._OverlaysList.Count; i1 ++) {
						string Statut2 = "";
						if (Statut2 == null) Statut = "";
						if (EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList.ToList().Contains (DK_UMA_Import_Win._ExportData.RacesList [i]._OverlaysList [i1].DKOverlayData) == true) {
							Statut2 = "Ok";
						} 
						else Statut2 = "Add";
						using (new Horizontal()) {
							GUILayout.Space (35);
							if (Statut2 == "Ok") GUI.color = Green;
							else GUI.color = Color.yellow;
							if (GUILayout.Button (Statut2, "toolbarbutton", GUILayout.Width (40))) {
								
							}
							GUI.color = Color.white;
							if (GUILayout.Button (DK_UMA_Import_Win._ExportData.RacesList [i]._OverlaysList [i1].Name, Slim, GUILayout.ExpandWidth (true))) {
								
							}
						}
					}
				}
				else GUILayout.Label ("- The Race has no slot Dependencies.", GUILayout.Width (300));
				// Dependencies : Color Presets
				if ( DK_UMA_Import_Win._ExportData.RacesList[i]._ColorsList.Count > 0 ){
					using (new Horizontal()) {
						GUILayout.Space (25);
						GUILayout.Label ("Dependencies : Color Presets", "foldout", GUILayout.Width (170));
					}
					for (int i1 = 0; i1 < DK_UMA_Import_Win._ExportData.RacesList[i]._ColorsList.Count; i1 ++) {

						using (new Horizontal()) {
							GUILayout.Space (35);
							GUI.color = Green;
							if (GUILayout.Button ("Ok", "toolbarbutton", GUILayout.Width (40))) {
								
							}
							GUI.color = Color.white;
							if (GUILayout.Button (DK_UMA_Import_Win._ExportData.RacesList [i]._ColorsList[i1].ColorPresetName, Slim, GUILayout.ExpandWidth (true))) {
								
							}
						}
					}
				}
				else GUILayout.Label ("- The Race has no Color Dependencies.", GUILayout.Width (300));
			}
		}
		#endregion Race
		#region Slot
		// Slot
		if ( _Type == "DKSlotData" ) using (new ScrollView(ref scroll)) {
			string Statut = "";
			if ( Statut == null ) Statut = "";
			if (DK_UMA_Import_Win._ExportData != null
			    && DK_UMA_Import_Win._ExportData.SlotsList [i].SlotData != null) 
			{
				// Dependencies : Overlays
				if ( DK_UMA_Import_Win._ExportData.SlotsList[i]._OverlaysList.Count > 0 ){
					using (new Horizontal()) {
						GUILayout.Space (25);
						GUILayout.Label ("Dependencies : Overlays", "foldout", GUILayout.Width (170));
					}
					for (int i1 = 0; i1 < DK_UMA_Import_Win._ExportData.SlotsList[i]._OverlaysList.Count; i1 ++) {
						string Statut2 = "";
						if (Statut2 == null) Statut = "";
						if (EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList.ToList().Contains(DK_UMA_Import_Win._ExportData.SlotsList[i]._OverlaysList[i1].DKOverlayData) == true) {
							Statut2 = "Ok";
						} 
						else Statut2 = "Add";
						using (new Horizontal()) {
							GUILayout.Space (35);
							if (Statut2 == "Ok") GUI.color = Green;
							else GUI.color = Color.yellow;
							if (GUILayout.Button (Statut2, "toolbarbutton", GUILayout.Width (40))) {
								
							}
							GUI.color = Color.white;
							if (GUILayout.Button (DK_UMA_Import_Win._ExportData.SlotsList[i]._OverlaysList[i1].Name, Slim, GUILayout.ExpandWidth (true))) {
								
							}
						}
					}
				}
				else GUILayout.Label ("- The slot has no Overlay Dependencies.", GUILayout.Width (300));
				// Dependencies : Color Presets
				if ( DK_UMA_Import_Win._ExportData.SlotsList[i]._ColorsList.Count > 0 ){
					using (new Horizontal()) {
						GUILayout.Space (25);
						GUILayout.Label ("Dependencies : Color Presets", "foldout", GUILayout.Width (170));
					}
					for (int i1 = 0; i1 < DK_UMA_Import_Win._ExportData.SlotsList[i]._ColorsList.Count; i1 ++) {
						
						using (new Horizontal()) {
							GUILayout.Space (35);
							GUI.color = Green;
							if (GUILayout.Button ("Ok", "toolbarbutton", GUILayout.Width (40))) {
								
							}
							GUI.color = Color.white;
							if (GUILayout.Button (DK_UMA_Import_Win._ExportData.SlotsList[i]._ColorsList[i1].ColorPresetName, Slim, GUILayout.ExpandWidth (true))) {
								
							}
						}
					}
				}
				else GUILayout.Label ("- The slot has no Color Element's Dependencies.", GUILayout.Width (300));
			}
		}
		#endregion Slot
		#region Overlay
		// Overlay
		if ( _Type == "DKOverlayData" ) using (new ScrollView(ref scroll)) {
			GUILayout.Label ("- The Overlay has no Dependencies.", GUILayout.Width (300));
		}
		#endregion Slot
	}
}
