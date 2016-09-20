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

public class DK_UMA_ChooseExport_Win : EditorWindow {
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

		#region Choose Export Project
		if (Action == "Choose Export Project") {
			GUI.color = Green;
			if (Selected != "None" && GUILayout.Button ("Assign", GUILayout.ExpandWidth (true))) {
					DK_UMA_Export_Win._ExportDataName = Selected;
					Repaint ();
					this.Close ();
			}
		}
		if (Action == "Select Export Project") {
			GUI.color = Green;
			if (Selected != "None" && GUILayout.Button ("Export it !", GUILayout.ExpandWidth (true))) {
				UnityEngine.Object[] tmpObjs = AssetDatabase.LoadAllAssetsAtPath( "Assets/DK Editors/DK_UMA_Editor/Exporter/Exporting/"+Selected);
				for(int i = 0; i < tmpObjs.Length; i ++){
					if ( tmpObjs[i].name+".asset" == Selected ){
						Selection.activeObject = tmpObjs[i];
					}
				}
				Repaint ();
				this.Close ();
			}
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
		if (ShowProjects == true)using (new ScrollView(ref scroll)) {
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
					}
				}
			}
		}
			#endregion Export Projects List
	}
}
