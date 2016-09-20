using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;



public class UMAElements_Editor : EditorWindow {
	#region Variables
	public static string Action = "";

	string SearchString = "";
//	string Done = "Not done";
//	string Done2 = "Not done";
//	string Done3 = "Not done";
//	string Done4 = "Not done";

	bool ShowUMA = true;
	bool Showslot = true;
	bool Showoverlay = true;
	bool ShowLibs = true;
	bool showAbout;
	bool InfoSlots = true;
	bool InfoOverlays = true;

	GameObject _UMA;

	UMAContext _UMAContext;

	UMA_Variables _UMA_Variables;

	SlotLibrary _SlotLibrary;
	OverlayLibrary _OverlayLibrary;

	Vector2 scroll;
	Color Green = new Color (0.8f, 1f, 0.8f, 1);
	Color Red = new Color (0.9f, 0.5f, 0.5f);

	bool Helper = false;

	#endregion Variables


	[MenuItem("UMA/DK Editor/Plug-Ins/UMA Elements")]
	[MenuItem("Window/DK Editors/DK UMA/Plug-Ins/UMA Elements")]
	public static void Init()
	{
		UMAElements_Editor window = EditorWindow.GetWindow<UMAElements_Editor> (false, "UMA Elements");
		window.autoRepaintOnSceneChange = true;
		window.Show ();
	}

	public static void OpenChooseLibWin()
	{
		GetWindow(typeof(UMAChangeLibrary), false, "UMA Libs");
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

		Action = "Detecting";
		if ( _UMA_Variables.UMASlotsList.Count == 0 ) SearchUMASlots ();
		if ( _UMA_Variables.UMAOverlaysList.Count == 0 ) SearchUMAOverlays ();
	//	Done = "Done";
		Repaint ();
		Action = "";
	}

	void OnGUI () {
		this.minSize = new Vector2(355, 500);

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
			_UMA.name = "DK_UMA";
			_UMA = GameObject.Find("UMA");
		}
		if ( _UMA_Variables == null )
			_UMA_Variables = _UMA.GetComponent<UMA_Variables>();
		if ( _UMA_Variables == null )
			_UMA_Variables = _UMA.AddComponent<UMA_Variables>();

		#region Menu
		using (new Horizontal()) {
			GUILayout.Label("UMA Elements", "toolbarbutton", GUILayout.ExpandWidth (true));
			if ( showAbout == true ) GUI.color = Green;
			else GUI.color = Color.white;
			if ( GUILayout.Button ("About", "toolbarbutton", GUILayout.ExpandWidth (false))) {
				if ( showAbout == true ) showAbout = false;
				else showAbout = true;
			}
			if ( Helper ) GUI.color = Green;
			else GUI.color = Color.yellow;
			if ( GUILayout.Button ( "?", "toolbarbutton", GUILayout.ExpandWidth (false))) {
				if ( Helper ) Helper = false;
				else Helper = true;
			}
		}
		if ( !showAbout ) {
			// Libraries
			GUILayout.Space(5);
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("Libraries", "toolbarbutton", GUILayout.ExpandWidth (true));
				if ( ShowLibs ) GUI.color = Green;
				else GUI.color = Color.gray;
				if ( GUILayout.Button ( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))) {
					if ( ShowLibs ) ShowLibs = false;
					else ShowLibs = true;
				}
			}

			// libraries variables
			_SlotLibrary = _UMA_Variables._SlotLibrary;
			if ( _SlotLibrary == null ){
				try{
				_UMA_Variables._SlotLibrary = GameObject.Find ("SlotLibrary").GetComponent<SlotLibrary>();
				}catch(NullReferenceException){ Debug.LogError ( "UMA is not installed in your scene, please install UMA." ); }
			}

			_OverlayLibrary = _UMA_Variables._OverlayLibrary;
			if ( _OverlayLibrary == null ){
				try{
				_UMA_Variables._OverlayLibrary = GameObject.Find ("OverlayLibrary").GetComponent<OverlayLibrary>();
				}catch(NullReferenceException){ Debug.LogError ( "UMA is not installed in your scene, please install UMA." ); }
			}
			if ( ShowLibs ){
				using (new Horizontal()) {
					if ( _SlotLibrary ){
						GUI.color = Color.white;
						GUILayout.TextField( _SlotLibrary.name , 256, style, GUILayout.Width (110));
						if ( GUILayout.Button ( "Change", GUILayout.ExpandWidth (false))) {
							OpenChooseLibWin();
							UMAChangeLibrary.CurrentLibrary = _SlotLibrary.gameObject;
						}
					}
					else {
						GUILayout.Label("No Slots Library", GUILayout.Width (120));
						if ( GUILayout.Button ( "Find", GUILayout.ExpandWidth (false))) {
							_UMA_Variables._SlotLibrary = GameObject.Find ("SlotLibrary").GetComponent<SlotLibrary>();
						}
					}
				
					if ( _OverlayLibrary ){
						GUI.color = Color.white;
						GUILayout.TextField( _OverlayLibrary.name , 256, style, GUILayout.Width (110));
						if ( GUILayout.Button ( "Change", GUILayout.ExpandWidth (false))) {
							OpenChooseLibWin();
							UMAChangeLibrary.CurrentLibrary = _OverlayLibrary.gameObject;
						}
					}
					else {
						GUILayout.Label("No Overlay Library", GUILayout.Width (120));
						if ( GUILayout.Button ( "Find", GUILayout.ExpandWidth (false))) {
							_UMA_Variables._OverlayLibrary = GameObject.Find ("OverlayLibrary").GetComponent<OverlayLibrary>();
						}
					}
				}
			}

			#region Actions
			GUILayout.Space(5);
			GUI.color = Color.white ;
			GUILayout.Label("Options to Detect assets", "toolbarbutton", GUILayout.ExpandWidth (true));
			using (new Horizontal()) {
				GUI.color = Green;
				if ( GUILayout.Button ( "Search all Elements in the project", GUILayout.ExpandWidth (true))) {
				//	Debug.Log ("Test");
					Action = "Detecting";
					SearchUMASlots ();
					SearchUMAOverlays ();
				//	Done = "Done";
				}

				if ( GUILayout.Button ( "Previews (Double clic)", GUILayout.ExpandWidth (true))) {
				//	PreviewsList.Clear();
					MakePreviews ();
				}
			}

			using (new Horizontal()) {
				try {
				if ( _SlotLibrary && _OverlayLibrary && GUILayout.Button ( "Add all UMA Elements to the Libraries", GUILayout.ExpandWidth (true))) {
				//	Done4 = "Done";
					AddToUMALib ();
				}
				}catch ( ArgumentException ){}
			}

			#endregion Actions
			#endregion Menu

			#region Lists
			GUILayout.Space(5);
			using (new Horizontal()) {
				GUI.color = Color.white ;
				GUILayout.Label("List of Elements", "toolbarbutton", GUILayout.ExpandWidth (true));

				if (Showslot) GUI.color = Color.cyan ;
				else GUI.color = Color.gray ;
				if ( GUILayout.Button( "UMA slots", "toolbarbutton", GUILayout.Width (85))){
					if (Showslot){ 
					//	ShowUMA = false; 
						Showslot = false;
					}
					else {
					//	ShowUMA = true;
						Showslot = true;
					}
				}
				if (InfoSlots) GUI.color = Green ;
				else GUI.color = Color.gray ;
				if ( GUILayout.Button ( "Info", "toolbarbutton", GUILayout.ExpandWidth (false))) {
					if ( InfoSlots ) InfoSlots = false;
					else InfoSlots = true;
				}
				if (Showoverlay) GUI.color = Color.cyan ;
				else GUI.color = Color.gray ;
				if ( GUILayout.Button( "UMA overlays", "toolbarbutton", GUILayout.Width (85))){
					if (Showoverlay){ 
					//	ShowUMA = false; 
						Showoverlay = false;
					}
					else {
					//	ShowUMA = true;
						Showoverlay = true;
					}
				}
				
				if (InfoOverlays) GUI.color = Green ;
				else GUI.color = Color.gray ;
				if ( GUILayout.Button ( "Info", "toolbarbutton", GUILayout.ExpandWidth (false))) {
					if ( InfoOverlays ) InfoOverlays = false;
					else InfoOverlays = true;
				}
			}
			#region Search
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("Search for :", GUILayout.ExpandWidth (false));
				SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));
				
			}
			#endregion Search

			using (new Horizontal()) {
				using (new ScrollView(ref scroll)) 	{
					if (Showslot) 
					using (new Horizontal()) {
						// UMA Slots
						GUI.color = Color.cyan ;
						GUILayout.Label("UMA Slots ("+_UMA_Variables.UMASlotsList.Count.ToString()+")", "toolbarbutton", GUILayout.ExpandWidth (true));
						if (InfoSlots) GUI.color = Green ;
						else GUI.color = Color.gray ;

						if (Showslot) GUI.color = Green ;
						else GUI.color = Color.gray ;
						if ( GUILayout.Button( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))){
							if (Showslot) Showslot = false;
							else Showslot = true;
						}
					}
					if ( ShowUMA && Showslot && _UMA_Variables.UMASlotsList.Count > 0 )
					{
						#region Helper
						// Helper
						GUI.color = Color.white ;
						if ( Helper ) GUILayout.TextField("Click on the name to select a Slot." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						#endregion Helper

						for (int i = 0; i < _UMA_Variables.UMASlotsList.Count ; i++)
						{
							Texture2D Preview;
							if ( _UMA_Variables.UMASlotsList[i] != null && ( SearchString == "" 
							                                                  || _UMA_Variables.UMASlotsList[i].name.ToLower().Contains(SearchString.ToLower()) ))
							using (new Horizontal()) {
								using (new Vertical()) {
									string path = AssetDatabase.GetAssetPath (_UMA_Variables.UMASlotsList[i]);
									path = path.Replace (_UMA_Variables.UMASlotsList[i].name+".asset", "");
									Preview = AssetDatabase.LoadAssetAtPath(path+"Preview-"+_UMA_Variables.UMASlotsList[i].name+".asset", typeof(Texture2D) ) as Texture2D;
									if (Preview == null ){
										path = AssetDatabase.GetAssetPath (_UMA_Variables.UMASlotsList[i]);
										path = path.Replace (_UMA_Variables.UMASlotsList[i].name+".asset", "");
										Preview = AssetDatabase.LoadAssetAtPath(path+"Preview-"+_UMA_Variables.UMASlotsList[i].name+".asset", typeof(Texture2D) ) as Texture2D;
									}
									if ( Preview != null ) GUI.color = Color.white ;
									else GUI.color = Red;
									if ( InfoSlots == true || Selection.activeObject == _UMA_Variables.UMASlotsList[i]  )	if ( GUILayout.Button( Preview ,GUILayout.Width (75), GUILayout.Height (75))){
										Selection.activeObject = _UMA_Variables.UMASlotsList[i];
									}
								}

								using (new Vertical()) {
									if ( _UMA_Variables.UMASlotsList[i] != null && ( SearchString == "" 
									                                                  || _UMA_Variables.UMASlotsList[i].name.ToLower().Contains(SearchString.ToLower()) ))

									using (new Horizontal()) {
										GUILayout.Space(5);
										if ( Selection.activeObject == _UMA_Variables.UMASlotsList[i] ) GUI.color = Color.yellow ;
										else GUI.color = Color.white ;
										if ( _UMA_Variables.UMASlotsList[i].meshRenderer == null || _UMA_Variables.UMASlotsList[i].materialSample == null )GUI.color = Red ;
										if ( GUILayout.Button( _UMA_Variables.UMASlotsList[i].name, "toolbarbutton", GUILayout.Width (225))){
											Selection.activeObject = _UMA_Variables.UMASlotsList[i];
										}
										GUI.color = Red ;
										if ( GUILayout.Button( "X", "toolbarbutton", GUILayout.ExpandWidth (false))){
											
										}
									}
									if ( InfoSlots == true || Selection.activeObject == _UMA_Variables.UMASlotsList[i] )																	
									using (new Horizontal()) {
										// preview
										if ( Preview != null ) GUI.color = Green ;
										else GUI.color = Color.gray ;
										if ( Preview == null && GUILayout.Button( "preview (Double click)", "toolbarbutton", GUILayout.Width (240))){
											if ( Preview == null ) {
												if ( _UMA_Variables.UMASlotsList[i].meshRenderer.gameObject != null ){
													string path = AssetDatabase.GetAssetPath (_UMA_Variables.UMASlotsList[i]);
													path = path.Replace (_UMA_Variables.UMASlotsList[i].name+".asset", "");
													Preview = AssetPreview.GetAssetPreview( _UMA_Variables.UMASlotsList[i].meshRenderer.gameObject);
													//	Preview.name = "preview (Double click)";
													if ( Preview ){
														AssetDatabase.CreateAsset (Preview, path+"Preview-"+_UMA_Variables.UMASlotsList[i].name+".asset");
													}
												}
											}
										}
									}
									try{if ( InfoSlots == true || Selection.activeObject == _UMA_Variables.UMASlotsList[i] )
										if ( _SlotLibrary ) using (new Horizontal()) {
											string InLibText = "";

											if ( _SlotLibrary.GetAllSlots().ToList().Contains (_UMA_Variables.UMASlotsList[i]) == true ){
												GUI.color = Green ;
												InLibText = "Remove from Library";

											}
											else if ( InfoSlots == true || Selection.activeObject == _UMA_Variables.UMASlotsList[i] ){
												GUI.color = Color.gray ;
												InLibText = "Add to Library";

											}
											if ( InfoSlots == true || Selection.activeObject == _UMA_Variables.UMASlotsList[i] )if ( GUILayout.Button( InLibText, "toolbarbutton", GUILayout.Width (240)))
										{
											if (_SlotLibrary.GetAllSlots().ToList().Contains (_UMA_Variables.UMASlotsList[i]) == false ) {
												_SlotLibrary.AddSlot ( _UMA_Variables.UMASlotsList[i]);
												EditorUtility.SetDirty (_SlotLibrary);
												AssetDatabase.SaveAssets ();
											}
												else if ( InfoSlots == true || Selection.activeObject == _UMA_Variables.UMASlotsList[i] ){
												_SlotLibrary.GetAllSlots().ToList().Remove(_UMA_Variables.UMASlotsList[i]);
											}
										}
									}
									}catch(ArgumentException){}
									if ( InfoSlots == true || Selection.activeObject == _UMA_Variables.UMASlotsList[i] )using (new Horizontal()) {
										if ( _UMA_Variables.UMASlotsList[i].meshRenderer != null ) GUI.color = Green ;
										else GUI.color = Red ;
										GUILayout.Label("Renderer OK", "toolbarbutton", GUILayout.Width (120));
										if ( _UMA_Variables.UMASlotsList[i].materialSample != null ) GUI.color = Green ;
										else GUI.color = Red ;
										GUILayout.Label("Material OK", "toolbarbutton", GUILayout.Width (120));
									/*	if ( _UMA_Variables.UMASlotsList[i].textureNameList.Length > 0 ) GUI.color = Green ;
										else GUI.color = Color.yellow ;
										GUILayout.Label("Texture(s)", "toolbarbutton", GUILayout.Width (70));*/
									}
								}
							}
							if ( _UMA_Variables.UMASlotsList[i] == null ) _UMA_Variables.UMASlotsList.Remove (_UMA_Variables.UMASlotsList[i]);
							if ( _UMA_Variables.UMASlotsList[i] != null && ( SearchString == "" 
							                                 || _UMA_Variables.UMASlotsList[i].name.ToLower().Contains(SearchString.ToLower()) ))
								GUILayout.Space(10);
						}
					}

					// UMA Overlays
					if (Showoverlay)
					using (new Horizontal()) {
						GUI.color = Color.cyan ;
						GUILayout.Label("UMA Overlays ("+_UMA_Variables.UMAOverlaysList.Count.ToString()+")", "toolbarbutton", GUILayout.ExpandWidth (true));

						if (Showoverlay) GUI.color = Green ;
						else GUI.color = Color.gray ;
						if ( GUILayout.Button( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))){
							if (Showoverlay) Showoverlay = false;
							else Showoverlay = true;
						}
					}
					if ( ShowUMA && Showoverlay && _UMA_Variables.UMAOverlaysList.Count > 0 ){
						#region Helper
						// Helper
						GUI.color = Color.white ;
						if ( Helper ) GUILayout.TextField("Click on the name to select an Overlay." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					
						#endregion Helper
						if ( ShowUMA && Showoverlay && _UMA_Variables.UMAOverlaysList.Count > 0 ){
							for (int i = 0; i < _UMA_Variables.UMAOverlaysList.Count ; i++)
							{
								if ( _UMA_Variables.UMAOverlaysList[i] != null && ( SearchString == "" 
								                                                     || _UMA_Variables.UMAOverlaysList[i].name.ToLower().Contains(SearchString.ToLower()) ))
								using (new Horizontal()) {
									GUI.color = Color.white ;
									if ( Selection.activeObject == _UMA_Variables.UMAOverlaysList[i] ) GUI.color = Color.yellow ;
									if ( GUILayout.Button( _UMA_Variables.UMAOverlaysList[i].name, "toolbarbutton", GUILayout.Width (310))){
										Selection.activeObject = _UMA_Variables.UMAOverlaysList[i];
									}
									GUI.color = Red ;
									if ( GUILayout.Button( "X", "toolbarbutton", GUILayout.ExpandWidth (false))){
										
									}
								}
								if ( ( InfoOverlays == true || Selection.activeObject == _UMA_Variables.UMAOverlaysList[i] ) &&
								    _UMA_Variables.UMAOverlaysList[i] != null && ( SearchString == "" 
								                                                     || _UMA_Variables.UMAOverlaysList[i].name.ToLower().Contains(SearchString.ToLower()) ))
								using (new Horizontal()) {
									foreach ( Texture2D _Texture2D in _UMA_Variables.UMAOverlaysList[i].textureList)
									using (new Vertical()) {
										if ( _Texture2D != null ) GUI.color = Color.white ;
										else GUI.color = Red;
										if ( GUILayout.Button( _Texture2D ,GUILayout.Width (75), GUILayout.Height (75))){
											Selection.activeObject = _UMA_Variables.UMAOverlaysList[i];
										}
										GUILayout.Space(5);
									}
									using (new Vertical()) {
										using (new Horizontal()) {
											string InLibText = "";
											GUI.color = Green ;
											if ( _OverlayLibrary ) using (new Horizontal()) {
												if ( _OverlayLibrary.GetAllOverlays().ToList().Contains (_UMA_Variables.UMAOverlaysList[i]) == true ) {
													InLibText = "Remove from Library";
													GUI.color = Green ;
												}
												else {
													GUI.color = Color.gray ;
													InLibText = "Add to Library";
												}
												if ( GUILayout.Button( InLibText, "toolbarbutton", GUILayout.Width (120))){

													if ( InLibText == "Add to Library" && _OverlayLibrary.GetAllOverlays().ToList().Contains (_UMA_Variables.UMAOverlaysList[i]) == false ) {
														_OverlayLibrary.AddOverlay ( _UMA_Variables.UMAOverlaysList[i]);

													}
													else if ( InLibText == "Remove from Library" ){
														_OverlayLibrary.GetAllOverlays().ToList().Remove(_UMA_Variables.UMAOverlaysList[i]);
													}
													EditorUtility.SetDirty (_OverlayLibrary);
													AssetDatabase.SaveAssets ();
												}
											}
										}
										using (new Horizontal()) {
											bool Ok = true;
											foreach ( Texture2D _Texture2D in _UMA_Variables.UMAOverlaysList[i].textureList){
												var importer = TextureImporter.GetAtPath (_Texture2D.name) as TextureImporter;
												if (importer && importer.isReadable != true){
													Ok =  false;
												}
											}
											if ( Ok == true ) GUI.color = Green ;
											else GUI.color = Red ;
											if ( GUILayout.Button("Read/Write OK", "toolbarbutton", GUILayout.Width (120))){

											}
										/*	if ( _UMA_Variables.UMASlotsList[i].materialSample != null ) GUI.color = Green ;
											else GUI.color = Red ;
											GUILayout.Label("Material OK", "toolbarbutton", GUILayout.Width (120));
										*/	/*	if ( _UMA_Variables.UMASlotsList[i].textureNameList.Length > 0 ) GUI.color = Green ;
										else GUI.color = Color.yellow ;
										GUILayout.Label("Texture(s)", "toolbarbutton", GUILayout.Width (70));*/
										}

										if ( _UMA_Variables.UMAOverlaysList[i] == null ) {
											_UMA_Variables.UMAOverlaysList.Remove(_UMA_Variables.UMAOverlaysList[i]);
										}
									}
									if ( _UMA_Variables.UMAOverlaysList[i] != null && ( SearchString == "" 
									                                                     || _UMA_Variables.UMAOverlaysList[i].name.ToLower().Contains(SearchString.ToLower()) ))
										GUILayout.Space(5);
								}
							}
						}
					}
				}
			}
			#endregion Lists
		}
		#region About
		if ( showAbout ){
			using (new HorizontalCentered()) 
			{
				GUI.color = Color.white;
				GUILayout.Label ( "U.M.A. Elements", bold);
			}
			using (new HorizontalCentered()) 
			{
				GUI.color = Color.white;
				GUILayout.Label ( "for U.M.A. & Dynamic Kit U.M.A. Editor");
			}
			using (new HorizontalCentered()) 
			{
				GUILayout.Label ( "Unity version 4.5 & higher");
			}
			GUILayout.Space(5);
			GUI.color = Color.yellow;
			GUILayout.TextField("Greetings to Fernando Ribeiro for the creation of U.M.A. and LaneFox for his models.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.Space(5);
			
			using (new HorizontalCentered()) 
			{
				GUI.color = Green;
				GUILayout.TextField("(c) 2014 Ricardo Luque Martos", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			}
			using (new Horizontal()) 
			{
				GUI.color = Color.cyan;
				if(GUILayout.Button("Website")){
					Application.OpenURL ("http://alteredreality.wix.com/dk-uma");
				}
			}
			GUI.color = Color.yellow;
			using (new Horizontal()){
				if(GUILayout.Button("Web Video Tutorials")){
					Application.OpenURL ("http://www.youtube.com/playlist?list=PLz3lDsmTMvxZpbSXp79gRm3XOiZs31g5t");
				}
			}
			
			using (new Horizontal()) 
			{
				GUI.color = Color.cyan;
				if(GUILayout.Button("Facebook Page")){
					Application.OpenURL ("https://www.facebook.com/DKeditorsUnity3D");
				}
			}
			// Previous Plug Ins
			GUILayout.Label ( "Previous Plug-In(s) :", GUILayout.ExpandWidth (false));
			GUI.color = Color.white;
			using (new Horizontal()) 
			{
				if(GUILayout.Button("UMA Natural Behaviour")){
					Application.OpenURL ("https://www.assetstore.unity3d.com/en/#!/content/20836");
				}
			}

			// Next to come
			GUILayout.Label ( "The next Plug-In(s) to come :", GUILayout.ExpandWidth (false));
			using (new Horizontal()) 
			{
				GUILayout.Label ( "'Final IK' Grounder for UMA", bold);
				GUILayout.Label ( " (December 2014)", GUILayout.ExpandWidth (false));

			}
			using (new Horizontal()) 
			{
				GUILayout.Label ( "UMA Moods", bold);
				GUILayout.Label ( " (January 2015)", GUILayout.ExpandWidth (false));
				
			}

		}
		#endregion About
	}

	#region Search Voids
	void SearchUMASlots (){
		_UMA_Variables.UMASlotsList.Clear ();
		// find default
		if ( _UMA_Variables.DefaultSlotType == null ) {
			_UMA_Variables.DefaultSlotType = Resources.Load("DefaultSlotType") as UMA.SlotData;
		}
		System.Type type = _UMA_Variables.DefaultSlotType.GetType() ;
		GetAssetsOfType( type , ".asset" );
	}

	void MakePreviews (){
		foreach ( UMA.SlotData _tmpSlotData in _UMA_Variables.UMASlotsList ) {
			string path = AssetDatabase.GetAssetPath (_tmpSlotData);
			path = path.Replace (_tmpSlotData.name+".asset", "");
			if ( _tmpSlotData.meshRenderer.gameObject != null ){
				Texture2D Preview;
				Preview = AssetPreview.GetAssetPreview( _tmpSlotData.meshRenderer.gameObject);
				if ( Preview ){
					AssetDatabase.CreateAsset (Preview, path+"Preview-"+_tmpSlotData.name+".asset");
					_UMA_Variables.PreviewsList.Add (Preview);
				}
			}
		}
	}

	void SearchUMAOverlays (){
		_UMA_Variables.UMAOverlaysList.Clear ();
		if ( _UMA_Variables.DefaultOverlayType == null ) {
			_UMA_Variables.DefaultOverlayType = Resources.Load("DefaultOverlayType") as UMA.OverlayData;
		}
		System.Type type = _UMA_Variables.DefaultOverlayType.GetType() ;
		GetAssetsOfType( type , ".asset" );
	}
	#endregion Search Voids

	#region Search Elements
	public UnityEngine.Object[] GetAssetsOfType(System.Type type, string fileExtension)
	{
		List<UnityEngine.Object> tempObjects = new List<UnityEngine.Object>();
		DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
		FileInfo[] goFileInfo = directory.GetFiles("*" + fileExtension, SearchOption.AllDirectories);
		
		int i = 0; int goFileInfoLength = goFileInfo.Length;
		FileInfo tempGoFileInfo; string tempFilePath;
		UnityEngine.Object tempGO;
		for (; i < goFileInfoLength; i++)
		{
			tempGoFileInfo = goFileInfo[i];
			if (tempGoFileInfo == null)
				continue;
			
			tempFilePath = tempGoFileInfo.FullName;
			tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
			try{
				tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof(UnityEngine.Object)) as UnityEngine.Object;
				if (tempGO == null)
				{
				//	Debug.LogWarning("Skipping Null");
					continue;
				}
				else if (tempGO.name != "DefaultOverlayType" && tempGO.name != "DefaultSlotType" && tempGO.GetType() != type)
				{
				//	Debug.LogWarning("Skipping " + tempGO.GetType().ToString());
					continue;
				}

				// finishing
				if ( tempGO.name != "DefaultOverlayType" && tempGO.name != "DefaultSlotType" &&tempGO.name.Contains("DefaultDK") == false && tempGO.name.Contains("DefaultUMA") == false ){
					tempObjects.Add(tempGO);
				//	Debug.Log ( "Type : "+type.ToString()+"Item : "+tempGO.name+" / "+tempGO.GetType().ToString());

					// UMA Slots
					if ( tempGO.GetType().ToString() == "UMA.SlotData" ){
						_UMA_Variables.UMASlotsList.Add (tempGO as UMA.SlotData);
					}

					// UMA Overlays
					if ( tempGO.GetType().ToString() == "UMA.OverlayData" ){
						_UMA_Variables.UMAOverlaysList.Add (tempGO as UMA.OverlayData);
					}

				}
			}catch(Exception e){Debug.Log(e.ToString());}
		}
	//	Debug.ClearDeveloperConsole();
		return tempObjects.ToArray();
	//	MakePreviews ();
	}
	#endregion Search Elements

	#region Add To Lib

	void AddToUMALib (){
		// to be changed for next UMA version
	//	GameObject UMAContext = GameObject.Find("UMAContext");
		if ( _SlotLibrary == null ) _SlotLibrary = GameObject.Find("SlotLibrary").GetComponent<SlotLibrary>();
		if ( _OverlayLibrary == null ) _OverlayLibrary = GameObject.Find("OverlayLibrary").GetComponent<OverlayLibrary>();

		List<UMA.SlotData> LibSlotsList = new List<UMA.SlotData>();
		if ( _SlotLibrary != null ) LibSlotsList = _SlotLibrary.GetAllSlots ().ToList ();

		// Slots
		if ( _SlotLibrary != null ) for (int i = 0; i < _UMA_Variables.UMASlotsList.Count; i++) {
			if ( LibSlotsList.Contains(_UMA_Variables.UMASlotsList[i] as UMA.SlotData) == false)
				_SlotLibrary.AddSlot( _UMA_Variables.UMASlotsList[i] as UMA.SlotData);
		}

		List<UMA.OverlayData> LibOvsList = new List<UMA.OverlayData>();
		if ( _OverlayLibrary != null ) LibOvsList = _OverlayLibrary.GetAllOverlays ().ToList ();

		// Overlays
		if ( _OverlayLibrary != null )for (int i = 0; i < _UMA_Variables.UMAOverlaysList.Count; i++) {
			if ( LibOvsList.Contains(_UMA_Variables.UMAOverlaysList[i] as UMA.OverlayData) == false)
				_OverlayLibrary.AddOverlay( _UMA_Variables.UMAOverlaysList[i] as UMA.OverlayData);
		}
	}

	#endregion Add to Lib

	void OnProjectChange() {
		if ( Action == "Detecting" ){
			Debug.ClearDeveloperConsole();
			Debug.Log ("Console cleaned");
		}
	}

	void OnSelectionChange() {
	
	}
}
