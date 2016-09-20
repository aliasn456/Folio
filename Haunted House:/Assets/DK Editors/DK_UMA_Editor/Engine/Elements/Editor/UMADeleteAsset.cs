using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

public class UMADeleteAsset : EditorWindow {

	public static string ProcessName = "";
	public static float ProcessNumber = 0;
	public static float ProcessCurrent = 0;
	public static bool Processing = false;
	public static bool MultiUMAModel = false;
	public static bool UMAModel = false;
	public static string myPath;
	public static string Action;
	public static string AssetName = "";

	UMA_Variables _UMA_Variables;
	GameObject _UMA;

	void CloseSelf(){
		try{
		Action = "";
		this.Close();
		}catch(NullReferenceException){}
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

		_UMA = GameObject.Find("UMA");
		if ( _UMA == null ) {
			_UMA = (GameObject)PrefabUtility.InstantiatePrefab (Resources.Load ("UMA"));
			_UMA.name = "UMA";
			_UMA = GameObject.Find("UMA");
		}
		if ( _UMA_Variables == null )
			_UMA_Variables = _UMA.GetComponent<UMA_Variables>();
		if ( _UMA_Variables == null )
			_UMA_Variables = _UMA.AddComponent<UMA_Variables>();

		using (new HorizontalCentered()) {
			GUILayout.Label ( ProcessName, bold, GUILayout.ExpandWidth (true));
		}
		using (new HorizontalCentered())GUILayout.Label ( "Are you sure ?", GUILayout.ExpandWidth (false));
		if ( Action != "DelByPath" && Selection.activeObject && ProcessName != "Delete all models" )using (new HorizontalCentered())GUILayout.Label ( Selection.activeObject.name, bold, GUILayout.ExpandWidth (false));
		if ( Action == "DelByPath" && ProcessName != "Delete all models" )using (new HorizontalCentered())GUILayout.Label ( AssetName, bold, GUILayout.ExpandWidth (false));
		GUILayout.Space(40);
		using (new Horizontal()) {
			GUILayout.Space(20);
			GUI.color = new Color (0.9f, 0.5f, 0.5f);
			if ( ProcessName != "Clearing Library"  && GUILayout.Button ( "Yes, Delete.", GUILayout.ExpandWidth (true))){
				Delete();
			}
			else
			if (ProcessName == "Clearing Library" && GUILayout.Button ( "Yes, Clear it.", GUILayout.ExpandWidth (true))){
				Clearing();
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
	/*	if ( Selection.activeGameObject.gameObject.GetComponent<RaceLibrary>() == true ){
			List<UMA.RaceData> tmpList = new List<UMA.RaceData>();
			tmpList = Selection.activeGameObject.gameObject.GetComponent<RaceLibrary>().GetAllRaces().ToList();
			tmpList.Clear();
			Selection.activeGameObject.gameObject.GetComponent<RaceLibrary>().raceElementList = tmpList.ToArray();
		}
		if ( Selection.activeGameObject.gameObject.GetComponent<SlotLibrary>() == true ){
			List<UMA.SlotData> tmpList = new List<UMA.SlotData>();
			tmpList = Selection.activeGameObject.gameObject.GetComponent<SlotLibrary>().GetAllSlots().ToList();
			tmpList.Clear();
			Selection.activeGameObject.gameObject.GetComponent<SlotLibrary>().slotElementList = tmpList.ToArray();
		}
		if ( Selection.activeGameObject.gameObject.GetComponent<OverlayLibrary>() == true ){
			List<UMA.OverlayData> tmpList = new List<UMA.OverlayData>();
			tmpList = Selection.activeGameObject.gameObject.GetComponent<OverlayLibrary>().GetAllOverlays().ToList();
			tmpList.Clear();
			Selection.activeGameObject.gameObject.GetComponent<OverlayLibrary>().overlayElementList = tmpList.ToArray();
			myPath = "";
		}
		*/
		CloseSelf();
	}
	void Delete(){
	/*	// Elements
		if ( Selection.activeObject && Selection.activeObject.GetType().ToString() == "RaceData" ) {
			List<UMA.RaceData> tmpList = new List<UMA.RaceData>();
			tmpList = _UMA_Variables._RaceLibrary.GetAllRaces().ToList();
			if ( tmpList.Contains( Selection.activeObject as UMA.RaceData ) ){
				tmpList.Remove(( Selection.activeObject as UMA.RaceData ));
				_UMA_Variables._RaceLibrary.raceElementList = tmpList.ToArray();
			}
		}
		else
		if ( Selection.activeObject && Selection.activeObject.GetType().ToString() == "SlotData" ) {
			List<UMA.SlotData> tmpList = new List<UMA.SlotData>();
			tmpList = _UMA_Variables._SlotLibrary.GetAllSlots().ToList();
			if ( tmpList.Contains( Selection.activeObject as UMA.SlotData ) ){
				tmpList.Remove(( Selection.activeObject as UMA.SlotData ));
				_UMA_Variables._SlotLibrary.slotElementList = tmpList.ToArray();
			}
		}
		else
		if ( Selection.activeObject && Selection.activeObject.GetType().ToString() == "OverlayData" ) {
			List<UMA.OverlayData> tmpList = new List<UMA.OverlayData>();
			tmpList = _UMA_Variables._OverlayLibrary.GetAllOverlays().ToList();
			if ( tmpList.Contains( Selection.activeObject as UMA.OverlayData ) ){
				tmpList.Remove(( Selection.activeObject as UMA.OverlayData ));
				_UMA_Variables._OverlayLibrary.overlayElementList = tmpList.ToArray();
			}
		}
		else
		if ( Action != "DelByPath" && ProcessName != "Clearing Library" ) {
			myPath = AssetDatabase.GetAssetPath( Selection.activeObject ); 
			AssetDatabase.DeleteAsset (myPath);
		}
		if ( Action == "DelByPath" ) Action = "";
		*/
		CloseSelf();
	}

	void OnSelectionChange() {
		CloseSelf();
	}
}
