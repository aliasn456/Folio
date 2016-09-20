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

public class DK_UMA_Exp_Log : EditorWindow {

	Vector2 scroll1;

	Color Green = new Color (0.8f, 1f, 0.8f, 1);
	Color Red = new Color (0.9f, 0.5f, 0.5f);

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
	
		using (new Horizontal()) {
			GUILayout.Label ( "DK UMA Export Log", "toolbarbutton", GUILayout.ExpandWidth (true));
		}
		if (DK_UMA_Export_Win._ExportData != null
		    && DK_UMA_Export_Win._ExportData.ExportsPathList.Count > 0)
		{
			GUILayout.Space(5);	
			using (new Horizontal()) {
				GUILayout.Label ( "Export name :" , GUILayout.ExpandWidth (true));
				GUILayout.Label ( DK_UMA_Export_Win._ExportData.name , GUILayout.ExpandWidth (true));

			}
			GUILayout.Space(5);	
			using (new Horizontal()) {
				GUILayout.Label ( "Package Path :" , GUILayout.ExpandWidth (true));
				GUILayout.Label ( DK_UMA_Export_Win._SavePath , GUILayout.ExpandWidth (true));
				
			}
			GUILayout.Space(5);	
			using (new Horizontal()) {
				GUILayout.Label ( DK_UMA_Export_Win._ExportData.ExportsPathList.Count.ToString()+" files Exported successfully", GUILayout.ExpandWidth (true));
				if ( GUILayout.Button ( "Close", GUILayout.ExpandWidth (false))) {
					this.Close();
				}
			}

			GUILayout.Space(5);	
			using (new Horizontal()) {
				GUILayout.Label ( "Exports Path List", "toolbarbutton", GUILayout.ExpandWidth (true));
			}
			using (new ScrollView(ref scroll1)) {
				for(int i = 0; i < DK_UMA_Export_Win._ExportData.ExportsPathList.Count; i ++){
					using (new Horizontal()) {
						GUI.color = Color.white;
						GUILayout.Label ( DK_UMA_Export_Win._ExportData.ExportsPathList[i], GUILayout.ExpandWidth (true));
					}
				}
			}
		}
	}
}
