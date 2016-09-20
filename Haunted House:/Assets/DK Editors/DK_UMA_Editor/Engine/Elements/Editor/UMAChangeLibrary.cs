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

public class UMAChangeLibrary : EditorWindow {
	public static string LibType = "";
	public static string Action = "";
	public static string CurrentLibN = "";
	public static string SelectedLibN = "";
	public static GameObject CurrentLibrary;
	public static GameObject SelectedLibrary;
	public static bool EditorMode;
	public static bool ShowSlots;
	public static bool ShowOverlays;
	public static bool ShowRaces;

	string New_Name;
	string _Type;
	string SearchString;

	GameObject _UMA;
	UMA_Variables _UMA_Variables;

	Vector2 scroll;

	GUIContent _Prefab = new GUIContent("P", "Create or Delete an Asset.");
	GUIContent _AutoDelMiss = new GUIContent("Auto Delete Missings", "Verify the Library and delete the missing fields, multiple clicks.");
	GUIContent _Delete = new GUIContent("X", "Delete.");
	GUIContent _Duplic = new GUIContent("C", "Create a copy.");
	GUIContent _Group = new GUIContent("G", "Add the Model to the selected Group. If the Model is already in a Group, the model is removed and placed at the Root.");
	GUIContent _Inst = new GUIContent("Instantiate", "Create an Instance of the Asset.");

	[MenuItem("UMA/DK Editor/Plug-Ins/UMA Libraries")]
	[MenuItem("Window/DK Editors/DK UMA/Plug-Ins/UMA Libraries")]

	public static void Init()
	{
		UMAChangeLibrary window = EditorWindow.GetWindow<UMAChangeLibrary> (false, "UMA Libraries");
		window.autoRepaintOnSceneChange = true;
		window.Show ();
	}

	void OnEnable (){
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
	//	EditorMode = true;
	}

	void OnGUI () {
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
			GUI.color = Color.yellow;
			GUILayout.Label ( "Open the list and change the library.", GUILayout.ExpandWidth (true));
		}
		GUI.color = Color.white;
		using (new Horizontal()) {
			GUILayout.Label ( "Current :", GUILayout.Width (75));
			GUILayout.Label ( CurrentLibN, GUILayout.ExpandWidth (true));
			if ( !_UMA ) if ( GUILayout.Button ( "Open List", GUILayout.ExpandWidth (true))) {
				_UMA = GameObject.Find("UMA");
			}
			if ( EditorMode ) GUI.color = Color.yellow;
			else GUI.color = Color.white;
			if ( GUILayout.Button ( "Editor Mode", GUILayout.ExpandWidth (false))) 
			{
				if ( EditorMode ) EditorMode = false;
				else EditorMode = true;
			}
		}
		// Editor
		if ( EditorMode ) {
			GUI.color = Color.white;
			GUILayout.Label ( "Editor", "toolbarbutton", GUILayout.ExpandWidth (true));

			using (new Horizontal()) {
				string _Name;
				GUILayout.Label ( "Selected :", GUILayout.ExpandWidth (false));
				if ( Selection.activeGameObject
				   && ( Selection.activeGameObject.gameObject.GetComponent<SlotLibrary>() == true
				    || Selection.activeGameObject.gameObject.GetComponent<OverlayLibrary>() == true
				    || Selection.activeGameObject.gameObject.GetComponent<RaceLibrary>() == true ))
				{
					_Name = Selection.activeGameObject.name;
					GUILayout.Label ( _Name, GUILayout.ExpandWidth (true));
					if ( Selection.activeGameObject.gameObject.GetComponent<SlotLibrary>() == true ) _Type = "SlotLibrary";
					if ( Selection.activeGameObject.gameObject.GetComponent<OverlayLibrary>() == true ) _Type = "OverlayLibrary";
					if ( Selection.activeGameObject.gameObject.GetComponent<RaceLibrary>() == true ) _Type = "RaceLibrary";
					GUILayout.TextField ( _Type, GUILayout.ExpandWidth (false));
				}
				else {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Selection is not a Library !", GUILayout.ExpandWidth (true));
				}

			}
			if ( Selection.activeGameObject
			    && ( Selection.activeGameObject.gameObject.GetComponent<SlotLibrary>() == true
			    || Selection.activeGameObject.gameObject.GetComponent<OverlayLibrary>() == true
			    || Selection.activeGameObject.gameObject.GetComponent<RaceLibrary>() == true ))
			{
				using (new Horizontal()) {
					if ( New_Name == null ) New_Name = "";
					GUILayout.Label ( "New Name :", GUILayout.ExpandWidth (false));
					New_Name = GUILayout.TextField ( New_Name, GUILayout.ExpandWidth (true));
					if ( New_Name != "" && GUILayout.Button ( "Rename", GUILayout.ExpandWidth (false))) 
					{
						if ( Selection.activeGameObject
						    && ( Selection.activeGameObject.gameObject.GetComponent<SlotLibrary>() == true
						    || Selection.activeGameObject.gameObject.GetComponent<OverlayLibrary>() == true
						    || Selection.activeGameObject.gameObject.GetComponent<RaceLibrary>() == true ))
						{
							Selection.activeGameObject.name = New_Name;
							EditorUtility.SetDirty(Selection.activeGameObject);
							AssetDatabase.SaveAssets();
						}
					}
				}
			
				using (new Horizontal()) {
					if ( GUILayout.Button ( new GUIContent(_AutoDelMiss), GUILayout.ExpandWidth (true))) 
					{
						AutoDel();
					}
					if ( Selection.activeGameObject.gameObject.GetComponent<SlotLibrary>() == true
					    || Selection.activeGameObject.gameObject.GetComponent<OverlayLibrary>() == true
					    || Selection.activeGameObject.gameObject.GetComponent<RaceLibrary>() == true )
					{
						GUI.color = new Color (0.9f, 0.5f, 0.5f);
						if ( GUILayout.Button ( new GUIContent("Clear", "Clear the Library to make it umpty."), GUILayout.ExpandWidth (false))) 
						{
							UMADeleteAsset.ProcessName = "Clearing Library";
							UMADeleteAsset.UMAModel = false;
							UMADeleteAsset.MultiUMAModel = false;
							UMADeleteAsset.Action = "";
							OpenDeleteAsset();
						
						}
					}
				}
			}
			// editor List
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label ( "List", "toolbarbutton", GUILayout.ExpandWidth (true));
				if ( ShowRaces ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
				else GUI.color = Color.gray;
				if ( GUILayout.Button ( new GUIContent("Races", "Display Races Libraries"), "toolbarbutton", GUILayout.Width (80))) 
				{
					if ( ShowRaces ) ShowRaces = false;
					else ShowRaces = true;
				}
				if ( ShowSlots ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
				else GUI.color = Color.gray;
				if ( GUILayout.Button (new GUIContent("Slot", "Display Slot Libraries"), "toolbarbutton", GUILayout.Width (80))) 
				{
					if ( ShowSlots ) ShowSlots = false;
					else ShowSlots = true;
				}
				if ( ShowOverlays ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
				else GUI.color = Color.gray;
				if ( GUILayout.Button ( new GUIContent("Overlay", "Display Overlay Libraries"), "toolbarbutton", GUILayout.Width (80))) 
				{
					if ( ShowOverlays ) ShowOverlays = false;
					else ShowOverlays = true;
				}
			}
			#region Search
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("Search for :", GUILayout.ExpandWidth (false));
				if ( SearchString == null ) SearchString = "";
				SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));
			}
			#endregion Search
		}
	
		if ( _UMA && EditorMode ) {
			using (new ScrollView(ref scroll)){
				if ( ShowRaces ){
					GUI.color = Color.yellow;
					GUILayout.Label ( "Race Libraries :", "toolbarbutton", GUILayout.ExpandWidth (true));
					RaceLibrary[]  LibraryList = FindObjectsOfType(typeof(RaceLibrary)) as RaceLibrary[];
					List<string> NamesList = new List<string>();
					for(int i = 0; i < LibraryList.Length; i ++){
						if ( PrefabUtility.GetPrefabParent( LibraryList[i].gameObject ) != null )
							NamesList.Add(PrefabUtility.GetPrefabParent( LibraryList[i].gameObject ).name+".prefab");
						if ( PrefabUtility.GetPrefabType(LibraryList[i]) != null 
						    && PrefabUtility.GetPrefabObject(LibraryList[i]) == null){
						}
					}
				/*	DirectoryInfo dir = new DirectoryInfo("Assets/DK Editors/DK_UMA_Editor/Prefabs/Libraries/Race Libraries/");
					FileInfo[] info = dir.GetFiles("*.prefab");
					foreach (FileInfo f in info)
					{
						if ( NamesList.Contains(f.Name) == false ) 
						using (new Horizontal()) {
							GUI.color = Color.cyan;
							if ( GUILayout.Button ( new GUIContent(_Inst), GUILayout.ExpandWidth (false))){
								string myPath = ("Assets/DK Editors/DK_UMA_Editor/Prefabs/Libraries/Race Libraries/"+f.Name.ToString());
								GameObject New =  PrefabUtility.InstantiatePrefab(Resources.LoadAssetAtPath(myPath , typeof(GameObject))  ) as GameObject;
							//	New.transform.parent = DK_UMA_Editor.UMAObj.transform;
								PrefabUtility.ReconnectToLastPrefab(New);
							}
							if ( GUILayout.Button ( f.Name, Slim, GUILayout.ExpandWidth (false))) {
							
							}
						}
					}*/
				

					foreach (Transform Lib in _UMA.transform)
					{
						if ( Lib && ( Lib.gameObject.GetComponent<RaceLibrary>() == true
						             && ( Lib.name.ToLower().Contains(SearchString.ToLower()) || SearchString == "" ))){
							using (new Horizontal()) {
								// Prefab
								// Prefab Verification
								string myPath;
								if ( PrefabUtility.GetPrefabParent( Lib.gameObject ) != null )
									myPath = PrefabUtility.GetPrefabParent( Lib.gameObject ).ToString() ;
								else myPath = "null";
								if ( myPath == "null" ) GUI.color = Color.gray;
								else GUI.color = Color.cyan;
								if ( GUILayout.Button ( new GUIContent(_Prefab), "toolbarbutton", GUILayout.ExpandWidth (false))) {
									
									// Create Prefab
									if ( myPath == "null" ) 
									{
										PrefabUtility.CreatePrefab("Assets/DK Editors/DK_UMA_Editor/Prefabs/Libraries/Race Libraries/" + Lib.name + ".prefab", Lib.gameObject,ReplacePrefabOptions.ConnectToPrefab  );
										
									}
									else {
										Selection.activeGameObject = Lib.gameObject;
										DelAsset();
									}
								}
								GUI.color = Color.yellow;
								if ( EditorMode && GUILayout.Button ( new GUIContent(_Duplic), "toolbarbutton", GUILayout.ExpandWidth (false))) 
								{
									Selection.activeGameObject = Lib.gameObject;
									Duplicating();
								}
								try {
									if ( Selection.activeObject == Lib.gameObject )GUI.color = Color.yellow;
									else GUI.color = Color.white; 
									if ( Lib && Lib.gameObject && GUILayout.Button ( Lib.name+"( UMA "+Lib.gameObject.GetComponent<RaceLibrary>().GetAllRaces().Length+")", Slim, GUILayout.ExpandWidth (true))){
										if ( EditorMode ){
											Selection.activeObject = Lib.gameObject;
										//	DK_UMA_Editor.RaceLibraryObj = Lib.gameObject;
										//	Editor_Global.Variables.RaceLibraryObj = Lib.gameObject;
										//	DK_UMA_Editor._RaceLibrary = Lib.gameObject.GetComponent<RaceLibrary>();
										//	UMA_Variables._RaceLibrary = DK_UMA_Editor._RaceLibrary;
										}
									}
									GUI.color = Color.white;
									if ( CurrentLibrary && CurrentLibrary.gameObject.GetComponent<RaceLibrary>() == true ) 
									{
										if (Action == "" && GUILayout.Button ( "Assign", GUILayout.ExpandWidth (false))) 
										{
											if ( Action == "" ){
												_UMA_Variables._RaceLibrary = Lib.gameObject.GetComponent<RaceLibrary>();
												this.Close ();
											}
											else if ( EditorMode ){
												Selection.activeGameObject = Lib.gameObject ;
												this.Close ();
											}
										}
									}

									GUI.color = new Color (0.9f, 0.5f, 0.5f);
									if ( EditorMode && GUILayout.Button( new GUIContent(_Delete), Slim, GUILayout.ExpandWidth (false)))
									{
										if ( Lib && Lib.gameObject ) DestroyImmediate( Lib.gameObject );
									}
								}catch(MissingReferenceException){}
							}
						}
					}
				}

				if ( ShowSlots ){
					GUI.color = Color.yellow;
					GUILayout.Label ( "Slot Libraries :", "toolbarbutton", GUILayout.ExpandWidth (true));
					SlotLibrary[]  LibraryList = FindObjectsOfType(typeof(SlotLibrary)) as SlotLibrary[];
					List<string> NamesList = new List<string>();
					for(int i = 0; i < LibraryList.Length; i ++){
						if ( PrefabUtility.GetPrefabParent( LibraryList[i].gameObject ) != null )
							NamesList.Add(PrefabUtility.GetPrefabParent( LibraryList[i].gameObject ).name+".prefab");
						if ( PrefabUtility.GetPrefabType(LibraryList[i]) != null 
						    && PrefabUtility.GetPrefabObject(LibraryList[i]) == null){
						}
					}
				/*	DirectoryInfo dir = new DirectoryInfo("Assets/DK Editors/DK_UMA_Editor/Prefabs/Libraries/Slot Libraries/");
					FileInfo[] info = dir.GetFiles("*.prefab");
					foreach (FileInfo f in info)
					{
						if ( NamesList.Contains(f.Name) == false ) 
						using (new Horizontal()) {
							GUI.color = Color.cyan;
							if ( GUILayout.Button ( new GUIContent(_Inst), GUILayout.ExpandWidth (false))){
								string myPath = ("Assets/DK Editors/DK_UMA_Editor/Prefabs/Libraries/Slot Libraries/"+f.Name.ToString());
								GameObject New =  PrefabUtility.InstantiatePrefab(Resources.LoadAssetAtPath(myPath , typeof(GameObject))  ) as GameObject;
							//	New.transform.parent = DK_UMA_Editor.UMAObj.transform;
								PrefabUtility.ReconnectToLastPrefab(New);
							}
							if ( GUILayout.Button ( f.Name, Slim, GUILayout.ExpandWidth (false))) {
								
							}
						}
					}*/

					foreach (Transform Lib in _UMA.transform)
					{
						if ( Lib && ( Lib.gameObject.GetComponent<SlotLibrary>() == true
						&& ( Lib.name.ToLower().Contains(SearchString.ToLower()) || SearchString == "" ))){
							using (new Horizontal()) {
								// Prefab
								// Prefab Verification
								string myPath;
								if ( PrefabUtility.GetPrefabParent( Lib.gameObject ) != null )
									myPath = PrefabUtility.GetPrefabParent( Lib.gameObject ).ToString() ;
								else myPath = "null";
								if ( myPath == "null" ) GUI.color = Color.gray;
								else GUI.color = Color.cyan;
								if ( GUILayout.Button ( new GUIContent(_Prefab), "toolbarbutton", GUILayout.ExpandWidth (false))) {
									
									// Create Prefab
									if ( myPath == "null" ) 
									{
										PrefabUtility.CreatePrefab("Assets/DK Editors/DK_UMA_Editor/Prefabs/Libraries/Slot Libraries/" + Lib.name + ".prefab", Lib.gameObject,ReplacePrefabOptions.ConnectToPrefab  );
										
									}
									else {
										Selection.activeGameObject = Lib.gameObject;
										DelAsset();
									}
								}
								GUI.color = Color.yellow;
								if ( EditorMode && GUILayout.Button ( new GUIContent(_Duplic), "toolbarbutton", GUILayout.ExpandWidth (false))) 
								{
									Selection.activeGameObject = Lib.gameObject;
									Duplicating();
								}
								try {
									if ( Selection.activeObject == Lib.gameObject )GUI.color = Color.yellow;
									else GUI.color = Color.white; 
									if ( Lib && Lib.gameObject && GUILayout.Button ( Lib.name+"( UMA "+Lib.gameObject.GetComponent<SlotLibrary>().GetAllSlots().Length+")", Slim, GUILayout.ExpandWidth (true))){
										if ( EditorMode ){
											Selection.activeObject = Lib.gameObject;
										
										}
									}
									GUI.color = Color.white;
									if ( CurrentLibrary && CurrentLibrary.gameObject.GetComponent<SlotLibrary>() == true ) 
									{
										if (Action == "" && GUILayout.Button ( "Assign", GUILayout.ExpandWidth (false))) 
										{
											if ( Action == "" ){
												_UMA_Variables._SlotLibrary = Lib.gameObject.GetComponent<SlotLibrary>();
												this.Close ();
											}
											else if ( EditorMode ){
												Selection.activeGameObject = Lib.gameObject ;
												this.Close ();
											}
										}
									}

									GUI.color = new Color (0.9f, 0.5f, 0.5f);
									if ( EditorMode && GUILayout.Button( new GUIContent(_Delete), Slim, GUILayout.ExpandWidth (false)))
									{
										if ( Lib && Lib.gameObject ) DestroyImmediate( Lib.gameObject );
									}
								}catch(MissingReferenceException){}
							}
						}
					}
				}
			
				if ( ShowOverlays ) {
					GUI.color = Color.yellow; 
					GUILayout.Label ( "Overlay Libraries :", "toolbarbutton", GUILayout.ExpandWidth (true));
					OverlayLibrary[]  LibraryList = FindObjectsOfType(typeof(OverlayLibrary)) as OverlayLibrary[];
					List<string> NamesList = new List<string>();
					for(int i = 0; i < LibraryList.Length; i ++){
						if ( PrefabUtility.GetPrefabParent( LibraryList[i].gameObject ) != null )
							NamesList.Add(PrefabUtility.GetPrefabParent( LibraryList[i].gameObject ).name+".prefab");
						if ( PrefabUtility.GetPrefabType(LibraryList[i]) != null 
						    && PrefabUtility.GetPrefabObject(LibraryList[i]) == null){
						}
					}
				/*	DirectoryInfo dir = new DirectoryInfo("Assets/DK Editors/DK_UMA_Editor/Prefabs/Libraries/Overlay Libraries/");
					FileInfo[] info = dir.GetFiles("*.prefab");
					foreach (FileInfo f in info)
					{
						if ( NamesList.Contains(f.Name) == false ) 
						using (new Horizontal()) {
							GUI.color = Color.cyan;
							if ( GUILayout.Button ( new GUIContent(_Inst), GUILayout.ExpandWidth (false))){
								string myPath = ("Assets/DK Editors/DK_UMA_Editor/Prefabs/Libraries/Overlay Libraries/"+f.Name.ToString());
								GameObject New =  PrefabUtility.InstantiatePrefab(Resources.LoadAssetAtPath(myPath , typeof(GameObject))  ) as GameObject;
							//	New.transform.parent = DK_UMA_Editor.UMAObj.transform;
								PrefabUtility.ReconnectToLastPrefab(New);
							}
							if ( GUILayout.Button ( f.Name, Slim, GUILayout.ExpandWidth (false))) {
								
							}
						}
					}*/

					foreach (Transform Lib in _UMA.transform)
					{
						if ( Lib && ( Lib.gameObject.GetComponent<OverlayLibrary>() == true
						&& ( Lib.name.ToLower().Contains(SearchString.ToLower()) || SearchString == "" ))){
							using (new Horizontal()) {
								// Prefab
								// Prefab Verification
								string myPath;
								if ( PrefabUtility.GetPrefabParent( Lib.gameObject ) != null )
									myPath = PrefabUtility.GetPrefabParent( Lib.gameObject ).ToString() ;
								else myPath = "null";
								if ( myPath == "null" ) GUI.color = Color.gray;
								else GUI.color = Color.cyan;
								if ( GUILayout.Button ( new GUIContent(_Prefab), "toolbarbutton", GUILayout.ExpandWidth (false))) {
									
									// Create Prefab
									if ( myPath == "null" ) 
									{
										PrefabUtility.CreatePrefab("Assets/DK Editors/DK_UMA_Editor/Prefabs/Libraries/Overlay Libraries/" + Lib.name + ".prefab", Lib.gameObject,ReplacePrefabOptions.ConnectToPrefab  );
									}
									else {
										Selection.activeGameObject = Lib.gameObject;
										DelAsset();
									}
								}
								GUI.color = Color.yellow;
								if ( EditorMode && GUILayout.Button ( new GUIContent(_Duplic), "toolbarbutton", GUILayout.ExpandWidth (false))) 
								{
									Selection.activeGameObject = Lib.gameObject;
									Duplicating();
								}
								try {
									if ( Selection.activeObject == Lib.gameObject )GUI.color = Color.yellow;
									else GUI.color = Color.white; 
									if (GUILayout.Button ( Lib.name+"( UMA "+Lib.gameObject.GetComponent<OverlayLibrary>().GetAllOverlays().Length+")", Slim, GUILayout.ExpandWidth (true))){
										if ( EditorMode ){
											Selection.activeObject = Lib.gameObject;
										//	DK_UMA_Editor.OverlayLibraryObj = Lib.gameObject;
										//	Editor_Global.Variables.OverlayLibraryObj = Lib.gameObject;
										//	DK_UMA_Editor._OverlayLibrary = Lib.gameObject.GetComponent<OverlayLibrary>();
										//	UMA_Variables._OverlayLibrary = DK_UMA_Editor._OverlayLibrary;
										}
									}

									if ( CurrentLibrary && CurrentLibrary.gameObject.GetComponent<OverlayLibrary>() == true ){
										GUI.color = Color.white;
										if ( Action == "" && GUILayout.Button ( "Assign", GUILayout.ExpandWidth (false))) 
										{
											if ( Action == "" ){
												_UMA_Variables._OverlayLibrary = Lib.gameObject.GetComponent<OverlayLibrary>();
												this.Close ();
											}
										}
									}

									GUI.color = new Color (0.9f, 0.5f, 0.5f);
									if ( EditorMode && GUILayout.Button( new GUIContent(_Delete), Slim, GUILayout.ExpandWidth (false)))
									{
										DestroyImmediate( Lib.gameObject );
									}
								}catch(MissingReferenceException){}
							}
						}
					}
				}
			}
		}

		// list
		if ( _UMA && !EditorMode ) {
			GUI.color = Color.white;
			GUILayout.Label ( "List", "toolbarbutton", GUILayout.ExpandWidth (true));
			using (new ScrollView(ref scroll)){
				if ( CurrentLibrary ){

					foreach (Transform Lib in _UMA.transform)
					{
						if ( CurrentLibrary.gameObject.GetComponent<RaceLibrary>() == true 
						    && Lib.gameObject.GetComponent<RaceLibrary>() == true ) 
						using (new Horizontal()) {
							GUI.color = new Color (0.9f, 0.5f, 0.5f);
							if ( EditorMode && GUILayout.Button( new GUIContent(_Delete), Slim, GUILayout.ExpandWidth (false)))
							{
								DestroyImmediate( Lib.gameObject );
							}
							GUI.color = Color.yellow;
							if ( EditorMode && GUILayout.Button ( new GUIContent(_Duplic), "toolbarbutton", GUILayout.ExpandWidth (false))) 
							{
								Selection.activeGameObject = Lib.gameObject;
								Duplicating();
							}
							try {
								if ( Selection.activeObject == Lib.gameObject )GUI.color = Color.yellow;
								else GUI.color = Color.white; 
								if ( GUILayout.Button ( Lib.name+"( UMA "+Lib.gameObject.GetComponent<RaceLibrary>().GetAllRaces().Length+")", Slim, GUILayout.ExpandWidth (true))){
									Selection.activeObject = Lib.gameObject;
								}
								GUI.color = Color.white; 
								if ( Lib.gameObject.GetComponent<RaceLibrary>().GetAllRaces().Length > 0 ){
									if (Action == "" && GUILayout.Button ( "Assign", GUILayout.ExpandWidth (false))) 
									{
										if ( Action == "" ){
											_UMA_Variables._RaceLibrary = Lib.gameObject.GetComponent<RaceLibrary>();
											this.Close ();
										}
										else if ( EditorMode ){
											Selection.activeGameObject = Lib.gameObject ;
											this.Close ();
										}
									}
								}
								else {
									GUI.color = Color.yellow;
									GUILayout.Label ( "Is umpty", GUILayout.ExpandWidth (false));
								}
							}catch(MissingReferenceException){}
						}
						if ( CurrentLibrary.gameObject.GetComponent<SlotLibrary>() == true 
						    && Lib.gameObject.GetComponent<SlotLibrary>() == true ) 
						using (new Horizontal()) {
							GUI.color = new Color (0.9f, 0.5f, 0.5f);
							if ( EditorMode && GUILayout.Button( new GUIContent(_Delete), Slim, GUILayout.ExpandWidth (false)))
							{
								DestroyImmediate( Lib.gameObject );
							}
							try {
								if ( Selection.activeObject == Lib.gameObject )GUI.color = Color.yellow;
								else GUI.color = Color.white; 
								if ( GUILayout.Button ( Lib.name+"("+Lib.gameObject.GetComponent<SlotLibrary>().GetAllSlots().Length+")", Slim, GUILayout.ExpandWidth (true))){
									Selection.activeObject = Lib.gameObject;
								}
								GUI.color = Color.white; 
								if ( Lib.gameObject.GetComponent<SlotLibrary>().GetAllSlots().Length > 0 ){
									if (Action == "" && GUILayout.Button ( "Assign", GUILayout.ExpandWidth (false))) 
									{
										if ( Action == "" ){
											_UMA_Variables._SlotLibrary = Lib.gameObject.GetComponent<SlotLibrary>();
											this.Close ();
										}
										else if ( EditorMode ){
											Selection.activeGameObject = Lib.gameObject ;
											this.Close ();
										}
									}
									GUI.color = Color.yellow;
									if ( EditorMode && GUILayout.Button ( new GUIContent(_Duplic), "toolbarbutton", GUILayout.ExpandWidth (false))) 
									{
										Selection.activeGameObject = Lib.gameObject;
										Duplicating();
									}
								}
								else {
									GUI.color = Color.yellow;
									GUILayout.Label ( "Is umpty", GUILayout.ExpandWidth (false));
								}
							}catch(MissingReferenceException){}
						}
						if ( CurrentLibrary.gameObject.GetComponent<OverlayLibrary>() == true 
						    && Lib.gameObject.GetComponent<OverlayLibrary>() == true ) 
						using (new Horizontal()) {
							GUI.color = new Color (0.9f, 0.5f, 0.5f);
							if ( EditorMode && GUILayout.Button( new GUIContent(_Delete), Slim, GUILayout.ExpandWidth (false)))
							{
								DestroyImmediate( Lib.gameObject );
							}
							try {
								if ( Selection.activeObject == Lib.gameObject )GUI.color = Color.yellow;
								else GUI.color = Color.white; 
								if (GUILayout.Button ( Lib.name+"("+Lib.gameObject.GetComponent<OverlayLibrary>().GetAllOverlays().Length+")", Slim, GUILayout.ExpandWidth (true))){
									Selection.activeObject = Lib.gameObject;
								}
								if ( Lib.gameObject.GetComponent<OverlayLibrary>().GetAllOverlays().Length > 0 ){
									GUI.color = Color.white;
									if ( Action == "" && GUILayout.Button ( "Assign", GUILayout.ExpandWidth (false))) 
									{
										if ( Action == "" ){
											_UMA_Variables._OverlayLibrary = Lib.gameObject.GetComponent<OverlayLibrary>();
											this.Close ();
										}
									}

									GUI.color = Color.yellow;
									if ( EditorMode && GUILayout.Button ( new GUIContent(_Duplic), "toolbarbutton", GUILayout.ExpandWidth (false))) 
									{
										Selection.activeGameObject = Lib.gameObject;
										Duplicating();
									}
								}
								else {
									GUI.color = Color.yellow;
									GUILayout.Label ( "Is umpty", GUILayout.ExpandWidth (false));
								}
						
							}catch(MissingReferenceException){}
						}
					}
				}
			}
		}
	}

	public static void OpenDeleteAsset(){
		GetWindow(typeof(UMADeleteAsset), false, "Deleting");
	}


	void DelAsset(){
		UMADeleteAsset.Action = "";
		UMADeleteAsset.ProcessName = "Delete Asset";
		UMADeleteAsset.MultiUMAModel = false;
		UMADeleteAsset.UMAModel = true;
		OpenDeleteAsset();
	}

	void Duplicating (){
		GameObject _Copy = Instantiate(Selection.activeGameObject) as GameObject;
		_Copy.name = Selection.activeGameObject.name + " (Copy)";
		_Copy.transform.parent = Selection.activeGameObject.transform.parent;
		_Copy.transform.position = new Vector3(0, 0, 0 );
		Selection.activeGameObject = _Copy.gameObject;
	}

	void AutoDel () {
/*		List<UMA.RaceData> tmpList0 = new List<UMA.RaceData>();
		tmpList0 = _UMA_Variables._RaceLibrary.GetAllRaces().ToList();
		for(int i = 0; i < tmpList0.Count; i ++){
			if ( tmpList0[i] == null ) tmpList0.Remove(tmpList0[i]);
		}
		_UMA_Variables._RaceLibrary.raceElementList = tmpList0.ToArray();
		List<UMA.SlotData> tmpList = new List<UMA.SlotData>();
		tmpList = _UMA_Variables._SlotLibrary.GetAllSlots().ToList();
		for(int i = 0; i < tmpList.Count; i ++){
			if ( tmpList[i] == null ) tmpList.Remove(tmpList[i]);
		}
		_UMA_Variables._SlotLibrary.slotElementList = tmpList.ToArray();
		
		List<UMA.OverlayData> tmpList2 = new List<UMA.OverlayData>();
		tmpList2 = _UMA_Variables._OverlayLibrary.GetAllOverlays().ToList();
		for(int i = 0; i < tmpList2.Count; i ++){
			if ( tmpList2[i] == null ) tmpList2.Remove(tmpList2[i]);
		}
		_UMA_Variables._OverlayLibrary.overlayElementList = tmpList2.ToArray();
		*/
	}

}
