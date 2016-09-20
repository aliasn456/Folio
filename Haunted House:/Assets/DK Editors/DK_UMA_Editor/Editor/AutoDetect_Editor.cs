using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;



public class AutoDetect_Editor : EditorWindow {
	#region Variables
	public static string Action = "";
	public DK_UMACrowd _DK_UMACrowd;

	string SearchString = "";
	bool InResults = true;
//	string Done = "Not done";
//	string Done2 = "Not done";
//	string Done3 = "Not done";
//	string Done4 = "Not done";
	bool ShowUMA = true;
	bool ShowDK = true;
	bool ShowDefault = false;
	bool ShowNew = false;

	bool ShowDKslot = true;
	bool ShowDKoverlay = true;

	bool Showslot = true;
	bool Showoverlay = true;

	bool ShowLibs = true;

	UMAContext _UMAContext;

	SlotLibrary _SlotLibrary;
	OverlayLibrary _OverlayLibrary;
	DKSlotLibrary _DKSlotLibrary;
	DKOverlayLibrary _DKOverlayLibrary;

	DKUMA_Variables _DKUMA_Variables;
	GameObject DK_UMA;

	
	Vector2 scroll;
	Vector2 scroll2;
	Color Green = new Color (0.8f, 1f, 0.8f, 1);
	Color Red = new Color (0.9f, 0.5f, 0.5f);

	bool Helper = false;

	DK_RPG_UMA _DK_RPG_UMA;

	#endregion Variables

	public static void OpenChooseLibWin()
	{
		GetWindow(typeof(ChangeLibrary), false, "UMA Libs");
	}

	void OnEnable (){
		DK_UMA = GameObject.Find("DK_UMA");

		if ( DK_UMA == null ) {
			DK_UMA = (GameObject)PrefabUtility.InstantiatePrefab (Resources.Load ("DK_UMA"));
			DK_UMA.name = "DK_UMA";
			DK_UMA = GameObject.Find("DK_UMA");
		}
		if ( _DKUMA_Variables == null )
			_DKUMA_Variables = DK_UMA.GetComponent<DKUMA_Variables>();
		_DKUMA_Variables.CleanLibraries ();
		_DKUMA_Variables.SearchAll ();
	}

	void OnGUI () {
		try{

		this.minSize = new Vector2(350, 500);

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

		DK_UMA = GameObject.Find("DK_UMA");
		if ( DK_UMA == null ) {
			DK_UMA = (GameObject)PrefabUtility.InstantiatePrefab (Resources.Load ("DK_UMA"));
			DK_UMA.name = "DK_UMA";
			DK_UMA = GameObject.Find("DK_UMA");
		}
		if ( _DK_UMACrowd == null )
			try{
			_DK_UMACrowd = GameObject.Find ("DKUMACrowd").GetComponent<DK_UMACrowd>();
			}catch(NullReferenceException){ Debug.LogError ( "DK UMA is not installed in your scene, please install DK UMA." ); this.Close(); }
		if ( _DKUMA_Variables == null )
			_DKUMA_Variables = DK_UMA.GetComponent<DKUMA_Variables>();


		#region Menu
		using (new Horizontal()) {
			GUILayout.Label("DK Elements Manager", "toolbarbutton", GUILayout.ExpandWidth (true));
			if ( Helper ) GUI.color = Green;
			else GUI.color = Color.yellow;
			if ( GUILayout.Button ( "?", "toolbarbutton", GUILayout.ExpandWidth (false))) {
				if ( Helper ) Helper = false;
				else Helper = true;
			}
		}
	//	using (new ScrollView(ref scroll2)) 	{

			if ( Helper ) GUILayout.TextField("You must convert a UMA Element for DK to be able to use it. To do so you have to click on the buttons below." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			if ( Helper ) GUILayout.TextField(" the Converter will search all your elements for UMA or DK, then it will be possible to Convert all the UMA elements to DK." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

			// Libraries
			GUILayout.Space(5);
		if ( !Helper )using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("Libraries", "toolbarbutton", GUILayout.ExpandWidth (true));
				if ( ShowLibs ) GUI.color = Green;
				else GUI.color = Color.gray;
				if ( GUILayout.Button ( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))) {
					if ( ShowLibs ) ShowLibs = false;
					else ShowLibs = true;
				}
			}

		if ( !Helper && ShowLibs ){
				// libraries variables
				_DKSlotLibrary = _DK_UMACrowd.slotLibrary;
				_DKOverlayLibrary = _DK_UMACrowd.overlayLibrary;

				_SlotLibrary = DKUMA_Variables._SlotLibrary;
				if ( DKUMA_Variables._SlotLibrary == null ){
					try{
					DKUMA_Variables._SlotLibrary = GameObject.Find ("SlotLibrary").GetComponent<SlotLibrary>();
					}catch(NullReferenceException){ /*Debug.LogError ( "UMA is not installed in your scene, please install UMA." );*/ }
				}

				_OverlayLibrary = DKUMA_Variables._OverlayLibrary;
				_DKUMA_Variables.ActiveOverlayLibrary = EditorVariables._OverlayLibrary;
				if ( DKUMA_Variables._OverlayLibrary == null ){
					try{
					DKUMA_Variables._OverlayLibrary = GameObject.Find ("OverlayLibrary").GetComponent<OverlayLibrary>();
					}catch(NullReferenceException){/* Debug.LogError ( "UMA is not installed in your scene, please install UMA." );*/ }
				}

			if (!Helper ) using (new Horizontal()) {
				_DKUMA_Variables.ActiveSlotLibrary = EditorVariables._DKSlotLibrary;

				if ( _DKSlotLibrary == null ){
					_DKSlotLibrary = EditorVariables._DKSlotLibrary;
				}
				if ( _DKSlotLibrary ){
				if ( ShowDK ) using (new Horizontal()) {
						GUI.color = Color.white;
						GUILayout.TextField( _DKSlotLibrary.name , 256, style, GUILayout.Width (110));
						if ( GUILayout.Button ( "Change", GUILayout.ExpandWidth (false))) {
							OpenChooseLibWin();
							ChangeLibrary.CurrentLibrary = _DKSlotLibrary.gameObject;

						}
					}
				}
				else {
				if ( ShowDK ) using (new Horizontal()) {
						GUILayout.Label("No DK Slots Library detected", GUILayout.ExpandWidth (true));
						if ( GUILayout.Button ( "Install Default", GUILayout.ExpandWidth (false))) {
							
						}
					}
				}
				if ( _DKOverlayLibrary == null ){
					_DKOverlayLibrary = EditorVariables._OverlayLibrary;
				}
				if ( _DKOverlayLibrary ){
					if ( ShowDK ) using (new Horizontal()) {
						GUI.color = Color.white;
						GUILayout.TextField( _DKOverlayLibrary.name , 256, style, GUILayout.Width (110));
						if ( GUILayout.Button ( "Change", GUILayout.ExpandWidth (false))) {
							OpenChooseLibWin();
							ChangeLibrary.CurrentLibrary = _DKOverlayLibrary.gameObject;
						}
					}
				}
				else {
					if ( ShowDK ) using (new Horizontal()) {
						GUILayout.Label("No DK Overlay Library detected", GUILayout.ExpandWidth (true));
						if ( GUILayout.Button ( "Install Default", GUILayout.ExpandWidth (false))) {
							
						}
					}
				}
			}


			if (! Helper ) using (new Horizontal()) {
				if ( _SlotLibrary ){
					if ( ShowUMA ) using (new Horizontal()) {
						GUI.color = Color.white;
						GUILayout.TextField( _SlotLibrary.name , 256, style, GUILayout.Width (110));
						if ( GUILayout.Button ( "Change", GUILayout.ExpandWidth (false))) {
							OpenChooseLibWin();
							ChangeLibrary.CurrentLibrary = _SlotLibrary.gameObject;
							
						}
					}
				}
				if ( _OverlayLibrary ){
					try {
					if ( ShowUMA ) using (new Horizontal()) {
						GUI.color = Color.white;
						GUILayout.TextField( _OverlayLibrary.name , 256, style, GUILayout.Width (110));
						if ( GUILayout.Button ( "Change", GUILayout.ExpandWidth (false))) {
							OpenChooseLibWin();
							ChangeLibrary.CurrentLibrary = _OverlayLibrary.gameObject;
						}
					}
					}catch ( ArgumentException ) { Repaint (); }
				}

				if ( !_OverlayLibrary || !_SlotLibrary ) 
					using (new Horizontal()) {
					GUI.color = Color.white ;
						GUILayout.Label("No UMA Libraries detected", GUILayout.ExpandWidth (true));
					GUI.color = Green ;
						if ( GUILayout.Button ( "Optional : Install UMA", GUILayout.ExpandWidth (true))) {
						InstallUMA();
					}
					}
				}
			}

			// Options
		try {
			GUILayout.Space(5);
			GUI.color = Color.white ;
			GUILayout.Label("Options to Detect assets", "toolbarbutton", GUILayout.ExpandWidth (true));
		
			#region Actions
			if ( Helper ) GUILayout.TextField("1- Detect all the UMA and DK UMA assets using the Detect button." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			if ( Helper ) GUILayout.TextField("2- You can convert all the UMA assets for DK UMA to use them." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
		}catch (ArgumentException){}

		using (new Horizontal()) {
				GUI.color = Green;
				if ( GUILayout.Button ( "1-Detect all Elements", GUILayout.ExpandWidth (true))) {
					Action = "Detecting";
				/*	SearchUMASlots ();
					SearchDKSlots ();
					SearchUMAOverlays ();
					SearchDKOverlays ();
					*/
					_DKUMA_Variables.SearchAll ();
				}
				GUI.color = Color.white ;
				if ( GUILayout.Button ( "2-Convert all", GUILayout.ExpandWidth (true))) {
					ImportAll ();
				}
			}
			if ( Helper ) GUILayout.TextField("3- You Can add all the new DK UMA elements to you current libraries, all of them or just the search result ones." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			if ( Helper ) GUILayout.TextField("4- You Can add all of them or just the search result ones." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			if ( Helper ) GUILayout.TextField("5- Clean your Libraries of any 'null' reference." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

			using (new Horizontal()) {
				GUILayout.Label("Add to Libraries", GUILayout.ExpandWidth (false));
				if ( GUILayout.Button ( "DK", GUILayout.Width (50))) {
					AddToLib ();
				}
				if ( _SlotLibrary && _OverlayLibrary && GUILayout.Button ( "UMA", GUILayout.Width (50))) {
					AddToUMALib ();
				}
				if ( InResults ) GUI.color = Green;
				else GUI.color = Color.gray;
				if ( GUILayout.Button ( "Result Only", GUILayout.ExpandWidth (true))) {
					if ( InResults ) InResults = false;
					else InResults = true ;
				}
				GUI.color = Color.white ;
				if ( GUILayout.Button ( "Clean", GUILayout.ExpandWidth (true))) {
					_DK_UMACrowd.CleanLibraries ();
				}
			}

			if ( Helper ) GUILayout.TextField("6- The original UMA element need to be linked to the DK UMA element for the incoming new version of DK UMA to be able to directly generate the basic UMA Avatars. It is automated but i let the button, just in case." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			if ( Helper ) GUILayout.TextField("7- DK UMA is now working with a brand new generator to store and find the DK elements. It is designed to be the root of the DK UMA RPG asset. It is automated but i let the button, just in case." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			if ( Helper ) GUILayout.TextField("8- An Auto Fix to move the linked Overlays of your DK Slots to the new correct lists." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			
		using (new Horizontal()) {
				GUI.color = Color.white ;
				if ( GUILayout.Button ( "Link to UMA", GUILayout.ExpandWidth (true))) {
					LinkAll ();
				}
				if ( GUILayout.Button ( "Create RPG lists", GUILayout.ExpandWidth (true))) {
					DK_RPG_UMA_Generator _DK_RPG_UMA_Generator = DK_UMA.GetComponent<DK_RPG_UMA_Generator>();
					_DK_RPG_UMA_Generator.PopulateAllLists();
				}
				if ( GUILayout.Button ( "Fix Elements", GUILayout.ExpandWidth (true))) {
					CorrectElements ();
				}
			}
			if ( Helper ) GUILayout.TextField("Close the helper to access to the lists of elements." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUI.color = Green ;
			if ( Helper && GUILayout.Button ( "Close Helper", GUILayout.ExpandWidth (true))) {
				Helper = false;
			}
	//	}

		#endregion Actions

		#endregion Menu

		#region Lists
		GUILayout.Space(5);
		if ( !Helper ) using (new Horizontal()) {
			GUI.color = Color.white ;
			GUILayout.Label("List of Elements", "toolbarbutton", GUILayout.ExpandWidth (true));
			if (ShowDefault) GUI.color = Green ;
			else GUI.color = Color.gray ;
			if ( GUILayout.Button( "Default", "toolbarbutton", GUILayout.Width (60))){
				if (ShowDefault) ShowDefault = false;
				else ShowDefault = true;
			}
			if (ShowNew) GUI.color = Green ;
			else GUI.color = Color.gray ;
			if ( GUILayout.Button( "New Only", "toolbarbutton", GUILayout.Width (60))){
				if ( ShowNew ) ShowNew = false;
				else ShowNew = true;
			}
		/*	if (ShowDK) GUI.color = Green ;
			else GUI.color = Color.gray ;
			if ( GUILayout.Button( "DK", "toolbarbutton", GUILayout.Width (40))){
				if (ShowDK) ShowDK = false;
				else ShowDK = true;
			}
			if (ShowUMA) GUI.color = Green ;
			else GUI.color = Color.gray ;
			if ( GUILayout.Button( "UMA", "toolbarbutton", GUILayout.Width (40))){
				if (ShowUMA) ShowUMA = false;
				else ShowUMA = true;
			}*/
		}
		if (!Helper ) using (new Horizontal()) {
			if (ShowDKslot) GUI.color = Color.cyan ;
			else GUI.color = Color.gray ;
			if ( GUILayout.Button( "DK slots", "toolbarbutton", GUILayout.Width (85))){
				if (ShowDKslot){
				//	ShowDK = false;
					ShowDKslot = false;
				}
				else {
				//	ShowDK = true;
					ShowDKslot = true;
				}
			}
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
			if (ShowDKoverlay) GUI.color = Color.cyan ;
			else GUI.color = Color.gray ;
			if ( GUILayout.Button( "DK overlays", "toolbarbutton", GUILayout.Width (85))){
				if (ShowDKoverlay ){ ShowDKoverlay = false; }
				else { ShowDKoverlay = true; }
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
		}

		/*	#region Helper
		// Helper
		GUI.color = Color.white ;
		if ( Helper ) GUILayout.TextField("Click on the name to select an Element." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
		GUI.color = Green ;
		if ( Helper ) GUILayout.TextField("The Green elements are installed in DK UMA, click on the 'X' button at the end of the line to delete it." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

		GUI.color = Red ;
		if ( Helper ) GUILayout.TextField("The Red elements are not installed in DK UMA, click on the 'Add' button at the end of the line to correct it." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

		#endregion Helper
*/
		#region Search
		if ( !Helper ) using (new Horizontal()) {
			GUI.color = Color.white;
			GUILayout.Label("Search for :", GUILayout.ExpandWidth (false));
			SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));
			
		}
		#endregion Search

		if ( !Helper ) using (new Horizontal()) {
			using (new ScrollView(ref scroll)) 	{
				if (Showslot) 
				using (new Horizontal()) {
					// UMA Slots
					GUI.color = Color.cyan ;
					GUILayout.Label("UMA Slots ("+_DKUMA_Variables.UMASlotsList.Count.ToString()+")", "toolbarbutton", GUILayout.ExpandWidth (true));
					if (Showslot) GUI.color = Green ;
					else GUI.color = Color.gray ;
					if ( GUILayout.Button( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))){
						if (Showslot) Showslot = false;
						else Showslot = true;
					}
				}
				if ( ShowUMA && Showslot && _DKUMA_Variables.UMASlotsList.Count > 0 )
				{
					#region Helper
					// Helper
					GUI.color = Color.white ;
					if ( Helper ) GUILayout.TextField("Click on the name to select an Element." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					GUI.color = Green ;
					if ( Helper ) GUILayout.TextField("The Green elements are installed in DK UMA." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					
					GUI.color = Red ;
					if ( Helper ) GUILayout.TextField("The Red elements are not installed in DK UMA, click on the 'Add' button to install it." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					
					#endregion Helper
					for (int i = 0; i < _DKUMA_Variables.UMASlotsList.Count ; i++)
					{



						// preview image
						Texture2D Preview;
						if ( _DKUMA_Variables.UMASlotsList[i] != null 
						    && _DKUMA_Variables.UMASlotsList[i].name != "DefaultSlotType"
						    && ( SearchString == "" 
						    || _DKUMA_Variables.UMASlotsList[i].name.ToLower().Contains(SearchString.ToLower()) ))
						using (new Horizontal()) {
							using (new Vertical()) {
								string path = AssetDatabase.GetAssetPath (_DKUMA_Variables.UMASlotsList[i]);
								path = path.Replace (_DKUMA_Variables.UMASlotsList[i].name+".asset", "");
								Preview = AssetDatabase.LoadAssetAtPath(path+"Preview-"+_DKUMA_Variables.UMASlotsList[i].name+".asset", typeof(Texture2D) ) as Texture2D;
								// trying to modify hideflags
								if ( Preview )  Preview.hideFlags = HideFlags.None;
							//	if ( i == 1 ) Debug.Log (Preview.hideFlags.ToString());
								if (Preview == null ){
									path = AssetDatabase.GetAssetPath (_DKUMA_Variables.UMASlotsList[i]);
									path = path.Replace (_DKUMA_Variables.UMASlotsList[i].name+".asset", "");
									Preview = AssetDatabase.LoadAssetAtPath(path+"Preview-"+_DKUMA_Variables.UMASlotsList[i].name+".asset", typeof(Texture2D) ) as Texture2D;
								}
								if ( Preview != null ) GUI.color = Color.white ;
								else GUI.color = Red;
								if ( GUILayout.Button( Preview ,GUILayout.Width (75), GUILayout.Height (75))){
									Selection.activeObject = _DKUMA_Variables.UMASlotsList[i];
								}
							}

							using (new Vertical()) {
								GUILayout.Space(5);
								using (new Horizontal()) {
									if (_DKUMA_Variables.DKSlotsNamesList.Contains(_DKUMA_Variables.UMASlotsList[i].name)) GUI.color = Green ;
									else GUI.color = Red ;
									if ( Selection.activeObject == _DKUMA_Variables.UMASlotsList[i] ) GUI.color = Color.yellow ;
									if ( GUILayout.Button( _DKUMA_Variables.UMASlotsList[i].name, "toolbarbutton", GUILayout.Width (225))){
										Selection.activeObject = _DKUMA_Variables.UMASlotsList[i];
									}
									GUI.color = Red ;
									if ( GUILayout.Button( "X", "toolbarbutton", GUILayout.ExpandWidth (false))){
										
									}
								}

								if ( _DKUMA_Variables.UMASlotsList[i] != null 
								    && _DKUMA_Variables.UMASlotsList[i].name != "DefaultSlotType"
								    && ( SearchString == "" 
								    || _DKUMA_Variables.UMASlotsList[i].name.ToLower().Contains(SearchString.ToLower()) ))
								using (new Horizontal()) {
									GUI.color = Green ;
									if ( _DKUMA_Variables.DKSlotsNamesList.Contains(_DKUMA_Variables.UMASlotsList[i].name) == false
									    && GUILayout.Button( "Add", "toolbarbutton", GUILayout.Width (240))){
										AddToDK(_DKUMA_Variables.UMASlotsList[i].GetType(), _DKUMA_Variables.UMASlotsList[i] as UnityEngine.Object);
									}
									GUI.color = Green ;
									if ( _DKUMA_Variables.DKSlotsNamesList.Contains(_DKUMA_Variables.UMASlotsList[i].name) == true
									    && GUILayout.Button( "Select DK", "toolbarbutton", GUILayout.Width (240)))
									{
										for (int i1 = 0; i1 < _DKUMA_Variables.DKSlotsList.Count ; i1++)
										{
											if (_DKUMA_Variables.DKSlotsList[i1].name == _DKUMA_Variables.UMASlotsList[i].name ){
												Selection.activeObject = _DKUMA_Variables.DKSlotsList[i1];
												
											}
										}
									}
								}
								using (new Horizontal()) {
									// preview
									if ( Preview != null ) GUI.color = Green ;
									else GUI.color = Color.gray ;
									if ( GUILayout.Button( "preview (Double click)", "toolbarbutton", GUILayout.Width (240))){
										if ( Preview == null ) {
											if ( _DKUMA_Variables.UMASlotsList[i].meshRenderer.gameObject != null ){
												string path = AssetDatabase.GetAssetPath (_DKUMA_Variables.UMASlotsList[i]);
												path = path.Replace (_DKUMA_Variables.UMASlotsList[i].name+".asset", "");
												Preview = AssetPreview.GetAssetPreview( _DKUMA_Variables.UMASlotsList[i].meshRenderer.gameObject);
												//	Preview.name = "preview (Double click)";
												if ( Preview ){
													AssetDatabase.CreateAsset (Preview, path+"Preview-"+_DKUMA_Variables.UMASlotsList[i].name+".asset");
												}
											}
										}
									}
								}
								try{
									if ( _SlotLibrary ) using (new Horizontal()) {
										if ( _SlotLibrary.GetAllSlots().ToList().Contains (_DKUMA_Variables.UMASlotsList[i]) == true ) GUI.color = Green ;
									else GUI.color = Color.gray ;
									if ( GUILayout.Button( "In UMA Library", "toolbarbutton", GUILayout.Width (240)))
									{

											if (_SlotLibrary.GetAllSlots().ToList().Contains (_DKUMA_Variables.UMASlotsList[i]) == false ) {
												_SlotLibrary.AddSlot ( _DKUMA_Variables.UMASlotsList[i]);
											EditorUtility.SetDirty (_SlotLibrary);
												AssetDatabase.SaveAssets ();
										}
									}
								}
								}catch(ArgumentException){}
							}
						}
						if ( _DKUMA_Variables.UMASlotsList[i] == null ) _DKUMA_Variables.UMASlotsList.Remove (_DKUMA_Variables.UMASlotsList[i]);
						try {
						if ( _DKUMA_Variables.UMASlotsList[i] != null && ( SearchString == "" 
						                                 || _DKUMA_Variables.UMASlotsList[i].name.ToLower().Contains(SearchString.ToLower()) ))
							GUILayout.Space(10);
						}catch(ArgumentOutOfRangeException){}
					}

				}
				// DK Slots
				if (ShowDKslot)
				using (new Horizontal()) {
					GUI.color = Color.cyan ;
					GUILayout.Label("DK Slots ("+_DKUMA_Variables.DKSlotsList.Count.ToString()+")", "toolbarbutton", GUILayout.ExpandWidth (true));
					if (ShowDKslot) GUI.color = Green ;
					else GUI.color = Color.gray ;
					if ( GUILayout.Button( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))){
						if (ShowDKslot) ShowDKslot = false;
						else ShowDKslot = true;
					}
				}
				#region Helper
				// Helper
				GUI.color = Color.white ;
				if ( Helper ) GUILayout.TextField("Click on the name to select an Element and modify it using the Prepare tab of the DK Editor, or using the Auto Detect (in the Prepare tab)." , 500, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				GUI.color = Green ;
				if ( Helper ) GUILayout.TextField("The Green elements has been setup for DK UMA." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				GUI.color = Red ;
				if ( Helper ) GUILayout.TextField("The Red elements are not ready for DK UMA, click on the 'Setup' button at the end of the line to ..." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				
				#endregion Helper
				if ( ShowDK && ShowDKslot && _DKUMA_Variables.DKSlotsList.Count > 0 ){
					for (int i = 0; i < _DKUMA_Variables.DKSlotsList.Count ; i++)
					{
						DKSlotData slot = _DKUMA_Variables.DKSlotsList[i];
						if ( slot != null 
						    && ( SearchString == "" || slot.name.ToLower().Contains(SearchString.ToLower() ) )
						    && ( ( ShowDefault == false && slot.Default == false ) || ShowDefault == true )
						    && ( ( ShowNew && ( slot.Place == null && slot._LegacyData.IsLegacy == false )) || !ShowNew )
						    )
						using (new Horizontal()) {
							Texture2D Preview;
							string path = AssetDatabase.GetAssetPath (_DKUMA_Variables.DKSlotsList[i]);
							path = path.Replace (_DKUMA_Variables.DKSlotsList[i].name+".asset", "");
							Preview = AssetDatabase.LoadAssetAtPath(path+"Preview-"+_DKUMA_Variables.DKSlotsList[i].name+".asset", typeof(Texture2D) ) as Texture2D;

							using (new Vertical()) {

								if ( Preview != null ) GUI.color = Color.white ;
								else GUI.color = Red;
								if ( GUILayout.Button( Preview ,GUILayout.Width (75), GUILayout.Height (75))){
									Selection.activeObject = _DKUMA_Variables.DKSlotsList[i];
								}
//# endif
							}
							using (new Vertical()) {
								GUILayout.Space(3);
								using (new Horizontal()) {
									if ( _DKUMA_Variables.DKSlotsList[i].Place != null ) GUI.color = Green ;
									else GUI.color = Red ;
									if ( Selection.activeObject == _DKUMA_Variables.DKSlotsList[i] ) GUI.color = Color.yellow ;
									if ( _DKUMA_Variables.DKSlotsList[i] != null && GUILayout.Button( _DKUMA_Variables.DKSlotsList[i].name, "toolbarbutton", GUILayout.Width (225))){
										Selection.activeObject = _DKUMA_Variables.DKSlotsList[i];
									}
									GUI.color = Red ;
									if ( GUILayout.Button( "X", "toolbarbutton", GUILayout.ExpandWidth (false))){
										RemoveFromDK(_DKUMA_Variables.DKSlotsList[i].GetType(), _DKUMA_Variables.DKSlotsList[i] as UnityEngine.Object);
									}
								}
								using (new Horizontal()) {
									if ( _DKUMA_Variables.DKSlotsList[i].Active == true ) GUI.color = Green ;
									else GUI.color = Color.gray ;
									if ( GUILayout.Button( "Is Active", "toolbarbutton", GUILayout.Width (120))){
										if ( _DKUMA_Variables.DKSlotsList[i].Active == true ) _DKUMA_Variables.DKSlotsList[i].Active = false ;
										else _DKUMA_Variables.DKSlotsList[i].Active = true ;
										EditorUtility.SetDirty (_DKUMA_Variables.DKSlotsList[i]);
										AssetDatabase.SaveAssets ();
									}
									if (_DK_UMACrowd == null ) _DK_UMACrowd =  GameObject.Find("DKUMACrowd").GetComponent<DK_UMACrowd>();
									if ( _DK_UMACrowd.slotLibrary.slotElementList.Contains (_DKUMA_Variables.DKSlotsList[i]) == true ) GUI.color = Green ;
									else GUI.color = Color.gray ;
									if ( GUILayout.Button( "In Library", "toolbarbutton", GUILayout.Width (120))){
										if (_DK_UMACrowd.slotLibrary.slotElementList.Contains (_DKUMA_Variables.DKSlotsList[i]) == false ) {
											_DK_UMACrowd.slotLibrary.AddSlot (_DKUMA_Variables.DKSlotsList[i].name, _DKUMA_Variables.DKSlotsList[i]);
											EditorUtility.SetDirty (_DK_UMACrowd.slotLibrary);
											AssetDatabase.SaveAssets ();
										}
										else {

										}
									}
								}
								if ( _DKUMA_Variables.DKSlotsList[i].meshRenderer == null  ) {
									GUI.color = Red ;
									GUILayout.Label("The UMA Element is not in your project.", Slim, GUILayout.ExpandWidth (true));
								//	GUILayout.Label("The DK Element is skipped.", Slim, GUILayout.ExpandWidth (true));

								}

								if ( Preview == null ) using (new Horizontal()) {
									GUI.color = Color.gray ;
									if ( GUILayout.Button( "preview (Double click)", "toolbarbutton", GUILayout.Width (240))){
										if ( _DKUMA_Variables.DKSlotsList[i].meshRenderer.gameObject != null ){
											path = AssetDatabase.GetAssetPath (_DKUMA_Variables.DKSlotsList[i]);
											path = path.Replace (_DKUMA_Variables.DKSlotsList[i].name+".asset", "");
											Preview = AssetPreview.GetAssetPreview( _DKUMA_Variables.DKSlotsList[i].meshRenderer.gameObject);
											if ( Preview ){
												Preview.hideFlags = HideFlags.DontSaveInBuild;
												AssetDatabase.CreateAsset (Preview, path+"Preview-"+_DKUMA_Variables.DKSlotsList[i].name+".asset");
											}
										}
									}
								}
								// Add to Avatar
								else if ( _DKUMA_Variables.DKSlotsList[i].Place != null
								         && _DKUMA_Variables.DKSlotsList[i].OverlayType != null
								         && _DKUMA_Variables.DKSlotsList[i].Race.Count > 0
								         && _DKUMA_Variables.DKSlotsList[i].Gender != null ) 
								{
									// find RPG component

									if ( _DK_RPG_UMA == null && Selection.activeGameObject ) _DK_RPG_UMA = Selection.activeGameObject.GetComponent<DK_RPG_UMA>();
									if ( _DK_RPG_UMA == null && Selection.activeGameObject &&Selection.activeGameObject.transform && Selection.activeGameObject.transform.parent ) _DK_RPG_UMA = Selection.activeGameObject.transform.parent.GetComponent<DK_RPG_UMA>();
									
									// Verify the Race And gender
									if ( _DK_RPG_UMA 
									    &&  _DKUMA_Variables.DKSlotsList[i].Race.Contains(_DK_RPG_UMA.Race)
									    && ( _DKUMA_Variables.DKSlotsList[i].Gender == _DK_RPG_UMA.Gender ||_DKUMA_Variables.DKSlotsList[i].Gender == "Both" )
									    ){
										using (new Horizontal()) {
											GUI.color = Color.yellow;
											if ( GUILayout.Button( "Add Slot to selected avatar", "toolbarbutton", GUILayout.Width (240))){
												if ( _DKUMA_Variables.DKSlotsList[i].Place.name.Contains("Wear")
												    || _DKUMA_Variables.DKSlotsList[i].Place.name.Contains("Handled") ) 
													DK_UMA_RPG_Equip.PrepareEquipSlotElement ( _DKUMA_Variables.DKSlotsList[i], null, _DK_RPG_UMA );
												else {
													DK_UMA_RPG_ChangeBody.PrepareChangeSlotElement ( _DKUMA_Variables.DKSlotsList[i], null, _DK_RPG_UMA );
												}
											}
										}
									}
									else if ( _DK_RPG_UMA 
									         && ( _DKUMA_Variables.DKSlotsList[i].Race.Contains(_DK_RPG_UMA.Race) == false
									         || _DKUMA_Variables.DKSlotsList[i].Gender != _DK_RPG_UMA.Gender ) ) {
										GUILayout.Label("not same gender or race", Slim, GUILayout.ExpandWidth (true));

									//	GUI.color = Color.gray;
									//	if ( GUILayout.Button( "Selection is not of the same race or gender", "toolbarbutton", GUILayout.Width (240))){
									//	}
									}
								}
							}
						}
						if ( _DKUMA_Variables.DKSlotsList[i] == null ) {
							_DKUMA_Variables.DKSlotsList.Remove(_DKUMA_Variables.DKSlotsList[i]);
						}
					}
				}

			// UMA Overlays
				if (Showoverlay)
				using (new Horizontal()) {
					GUI.color = Color.cyan ;
					GUILayout.Label("UMA Overlays ("+_DKUMA_Variables.UMAOverlaysList.Count.ToString()+")", "toolbarbutton", GUILayout.ExpandWidth (true));
					if (Showoverlay) GUI.color = Green ;
					else GUI.color = Color.gray ;
					if ( GUILayout.Button( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))){
						if (Showoverlay) Showoverlay = false;
						else Showoverlay = true;
					}
				}
				if ( ShowUMA && Showoverlay && _DKUMA_Variables.UMAOverlaysList.Count > 0 ){
					#region Helper
					// Helper
					GUI.color = Color.white ;
					if ( Helper ) GUILayout.TextField("Click on the name to select an Element." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					GUI.color = Green ;
					if ( Helper ) GUILayout.TextField("The Green elements are installed in DK UMA, click on the 'X' button at the end of the line to delete it." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					
					GUI.color = Red ;
					if ( Helper ) GUILayout.TextField("The Red elements are not installed in DK UMA, click on the 'Add' button at the end of the line to correct it." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					
					#endregion Helper
					if ( ShowUMA && Showoverlay && _DKUMA_Variables.UMAOverlaysList.Count > 0 ){
						for (int i = 0; i < _DKUMA_Variables.UMAOverlaysList.Count ; i++)
						{
							if ( _DKUMA_Variables.UMAOverlaysList[i] != null 
							    && _DKUMA_Variables.UMAOverlaysList[i].name != "DefaultOverlayType"
							    && ( SearchString == "" 
							    || _DKUMA_Variables.UMAOverlaysList[i].name.ToLower().Contains(SearchString.ToLower()) )){
								using (new Horizontal()) {
									if (_DKUMA_Variables.DKOverlaysNamesList.Contains(_DKUMA_Variables.UMAOverlaysList[i].name)) GUI.color = Green ;
									else GUI.color = Red ;
									if ( Selection.activeObject == _DKUMA_Variables.UMAOverlaysList[i] ) GUI.color = Color.yellow ;
									if ( GUILayout.Button( _DKUMA_Variables.UMAOverlaysList[i].name, "toolbarbutton", GUILayout.Width (310))){
										Selection.activeObject = _DKUMA_Variables.UMAOverlaysList[i];
									}
									GUI.color = Red ;
									if ( GUILayout.Button( "X", "toolbarbutton", GUILayout.ExpandWidth (false))){
										
									}
								}
								if ( _DKUMA_Variables.UMAOverlaysList[i] != null && ( SearchString == "" 
								                                                     || _DKUMA_Variables.UMAOverlaysList[i].name.ToLower().Contains(SearchString.ToLower()) ))
								using (new Horizontal()) {
									foreach ( Texture2D _Texture2D in _DKUMA_Variables.UMAOverlaysList[i].textureList)
									using (new Vertical()) {
										if ( _Texture2D != null ) GUI.color = Color.white ;
										else GUI.color = Red;
										if ( GUILayout.Button( _Texture2D ,GUILayout.Width (75), GUILayout.Height (75))){
											Selection.activeObject = _DKUMA_Variables.UMAOverlaysList[i];
										}
									}
									using (new Vertical()) {
										using (new Horizontal()) {
											GUI.color = Green ;
											if ( _DKUMA_Variables.DKOverlaysNamesList.Contains(_DKUMA_Variables.UMAOverlaysList[i].name) == false
											    && GUILayout.Button( "Add", "toolbarbutton", GUILayout.Width (65))){
												AddToDK(_DKUMA_Variables.UMAOverlaysList[i].GetType(), _DKUMA_Variables.UMAOverlaysList[i] as UnityEngine.Object);
											}
										}
										using (new Horizontal()) {
										GUI.color = Green ;
											if ( _DKUMA_Variables.DKOverlaysNamesList.Contains(_DKUMA_Variables.UMAOverlaysList[i].name) == true
											    && GUILayout.Button( "Select DK", "toolbarbutton", GUILayout.Width (65)))
										{
												for (int i1 = 0; i1 < _DKUMA_Variables.DKOverlaysList.Count ; i1++)
											{
													if (_DKUMA_Variables.DKOverlaysList[i1].name == _DKUMA_Variables.UMAOverlaysList[i].name ){
														Selection.activeObject = _DKUMA_Variables.DKOverlaysList[i1];
														
													}
												}
											}
										}
										if ( _OverlayLibrary ) using (new Horizontal()) {
											if ( _OverlayLibrary.GetAllOverlays().ToList().Contains (_DKUMA_Variables.UMAOverlaysList[i]) == true ) GUI.color = Green ;
											else GUI.color = Color.gray ;
											if ( GUILayout.Button( "In Library", "toolbarbutton", GUILayout.Width (65))){
												if (_OverlayLibrary.GetAllOverlays().ToList().Contains (_DKUMA_Variables.UMAOverlaysList[i]) == false ) {
													_OverlayLibrary.AddOverlay (_DKUMA_Variables.UMAOverlaysList[i]);

												}
												else{

												}
												EditorUtility.SetDirty (_OverlayLibrary);
												AssetDatabase.SaveAssets ();
											}
										}
									}
									
									if ( _DKUMA_Variables.UMAOverlaysList[i] == null ) {
										_DKUMA_Variables.UMAOverlaysList.Remove(_DKUMA_Variables.UMAOverlaysList[i]);
									}
								}
								if ( _DKUMA_Variables.UMAOverlaysList[i] != null && ( SearchString == "" 
							     || _DKUMA_Variables.UMAOverlaysList[i].name.ToLower().Contains(SearchString.ToLower()) ))
								GUILayout.Space(5);
							}
						}
					}
				}
			
				// DK Overlays
				if (ShowDKoverlay)
				using (new Horizontal()) {
					GUI.color = Color.cyan ;
					GUILayout.Label("DK Overlays ("+_DKUMA_Variables.DKOverlaysList.Count.ToString()+")", "toolbarbutton", GUILayout.ExpandWidth (true));
					if (ShowDKoverlay) GUI.color = Green ;
					else GUI.color = Color.gray ;
					if ( GUILayout.Button( "Show", "toolbarbutton", GUILayout.ExpandWidth (false))){
						if (ShowDKoverlay) ShowDKoverlay = false;
						else ShowDKoverlay = true;
					}
				}
				#region Helper
				// Helper
				GUI.color = Color.white ;
				if ( Helper ) GUILayout.TextField("Click on the name to select an Element and modify it using the Prepare tab of the DK Editor, or using the Auto Detect (in the Prepare tab)." , 500, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				GUI.color = Green ;
				if ( Helper ) GUILayout.TextField("The Green elements has been setup for DK UMA." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				GUI.color = Red ;
				if ( Helper ) GUILayout.TextField("The Red elements are not ready for DK UMA, click on the 'Setup' button at the end of the line to ..." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				
				#endregion Helper
				if ( ShowDK && ShowDKoverlay && _DKUMA_Variables.DKOverlaysList.Count > 0 ){
					for (int i = 0; i < _DKUMA_Variables.DKOverlaysList.Count ; i++)
					{
						DKOverlayData overlay = _DKUMA_Variables.DKOverlaysList[i];
						if ( overlay != null && ( SearchString == "" || overlay.name.ToLower().Contains(SearchString.ToLower()) )
						    && (( ShowDefault == false && overlay.Default == false ) || ShowDefault )
						    && (( ShowNew && (overlay.Place == null && overlay.LinkedToSlot.Count == 0 )) || !ShowNew )
						    ){
							using (new Horizontal()) {
								if ( _DKUMA_Variables.DKOverlaysList[i].Place != null ) GUI.color = Green ;
								else GUI.color = Red ;
								if ( Selection.activeObject == _DKUMA_Variables.DKOverlaysList[i] ) GUI.color = Color.yellow ;
								if ( _DKUMA_Variables.DKOverlaysList[i] != null && GUILayout.Button( _DKUMA_Variables.DKOverlaysList[i].name, "toolbarbutton", GUILayout.Width (310))){
									Selection.activeObject = _DKUMA_Variables.DKOverlaysList[i];
								}
									GUI.color = Red ;
									if ( GUILayout.Button( "X", "toolbarbutton", GUILayout.ExpandWidth (false))){
									RemoveFromDK(_DKUMA_Variables.DKOverlaysList[i].GetType(), _DKUMA_Variables.DKOverlaysList[i] as UnityEngine.Object);
								}
							}
							if ( _DKUMA_Variables.DKOverlaysList[i] != null && ( SearchString == "" 
							                                                    || _DKUMA_Variables.DKOverlaysList[i].name.ToLower().Contains(SearchString.ToLower()) ))
							using (new Horizontal()) {
								foreach ( Texture2D _Texture2D in _DKUMA_Variables.DKOverlaysList[i].textureList)
								using (new Vertical()) {
									if ( _Texture2D != null ) GUI.color = Color.white ;
									else GUI.color = Red;
									if ( GUILayout.Button( _Texture2D ,GUILayout.Width (75), GUILayout.Height (75))){
										Selection.activeObject = _DKUMA_Variables.DKOverlaysList[i];
									}
								}
								using (new Vertical()) {
									using (new Horizontal()) {
										if ( _DKUMA_Variables.DKOverlaysList[i].Active == true ) GUI.color = Green ;
										else GUI.color = Color.gray ;
										if ( GUILayout.Button( "Is Active", "toolbarbutton", GUILayout.Width (65))){
											if (_DKUMA_Variables.DKOverlaysList[i].Active == true ) _DKUMA_Variables.DKOverlaysList[i].Active = false ;
											else _DKUMA_Variables.DKOverlaysList[i].Active = true ;
											EditorUtility.SetDirty (_DKUMA_Variables.DKOverlaysList[i]);
											AssetDatabase.SaveAssets ();
										}
										if (_DK_UMACrowd == null ) _DK_UMACrowd =  GameObject.Find("DKUMACrowd").GetComponent<DK_UMACrowd>();
										if ( _DK_UMACrowd.overlayLibrary.overlayElementList.Contains (_DKUMA_Variables.DKOverlaysList[i]) == true ) GUI.color = Green ;
										else GUI.color = Color.gray ;
										if ( GUILayout.Button( "In Library", "toolbarbutton", GUILayout.Width (65))){
											if (_DK_UMACrowd.overlayLibrary.overlayElementList.Contains (_DKUMA_Variables.DKOverlaysList[i]) == false ) {
												_DK_UMACrowd.overlayLibrary.AddOverlay (_DKUMA_Variables.DKOverlaysList[i].overlayName, _DKUMA_Variables.DKOverlaysList[i] as DKOverlayData);
												EditorUtility.SetDirty (_DK_UMACrowd.overlayLibrary);
												AssetDatabase.SaveAssets ();
											}
										}
									}
									// Add to Avatar
									if ( _DKUMA_Variables.DKOverlaysList[i].LinkedToSlot.Count > 0 ){
										GUILayout.Label("Is Linked to a slot", GUILayout.ExpandWidth (true));
									}
									else if ( _DKUMA_Variables.DKOverlaysList[i].Place != null
									    && _DKUMA_Variables.DKOverlaysList[i].OverlayType != null
									    && _DKUMA_Variables.DKOverlaysList[i].Race.Count > 0
									    && _DKUMA_Variables.DKOverlaysList[i].Gender != null
									    && _DKUMA_Variables.DKOverlaysList[i].LinkedToSlot.Count == 0 ) 
									{
										if ( _DK_RPG_UMA == null && Selection.activeGameObject ) _DK_RPG_UMA = Selection.activeGameObject.GetComponent<DK_RPG_UMA>();
										if ( _DK_RPG_UMA == null && Selection.activeGameObject 
										    && Selection.activeGameObject.transform 
										    && Selection.activeGameObject.transform.parent ) _DK_RPG_UMA = Selection.activeGameObject.transform.parent.GetComponent<DK_RPG_UMA>();
										
										// Verify the Race And gender
										if ( _DK_RPG_UMA 
										    &&  _DKUMA_Variables.DKOverlaysList[i].Race.Contains(_DK_RPG_UMA.Race)
										    && ( _DKUMA_Variables.DKOverlaysList[i].Gender == _DK_RPG_UMA.Gender ||_DKUMA_Variables.DKOverlaysList[i].Gender == "Both" ))
										{
											GUI.color = Color.yellow ;
											using (new Horizontal()) {
												if ( GUILayout.Button( "Add to the avatar", "toolbarbutton", GUILayout.Width (130))){
													if ( _DKUMA_Variables.DKOverlaysList[i].OverlayType.Contains("Wear") == true ) 
														DK_UMA_RPG_Equip.PrepareEquipSlotElement ( null, _DKUMA_Variables.DKOverlaysList[i], _DK_RPG_UMA );
													else {
														DK_UMA_RPG_ChangeBody.PrepareChangeSlotElement ( null, _DKUMA_Variables.DKOverlaysList[i], _DK_RPG_UMA );

													}
												}
											}
										}
										else if ( _DK_RPG_UMA ) {
											GUILayout.Label("not same gender or race", Slim, GUILayout.ExpandWidth (true));
										//	GUI.color = Color.gray ;
										//	if ( GUILayout.Button( "not same gender or race", "toolbarbutton", GUILayout.Width (130))){
										//	}
										}
									}
								}

								if ( _DKUMA_Variables.DKOverlaysList[i] == null ) {
									_DKUMA_Variables.DKOverlaysList.Remove(_DKUMA_Variables.DKOverlaysList[i]);
								}

							}
							if ( _DKUMA_Variables.DKOverlaysList[i] != null && ( SearchString == "" 
							                                                    || _DKUMA_Variables.DKOverlaysList[i].name.ToLower().Contains(SearchString.ToLower()) ))
								GUILayout.Space(5);
						}
					}
				}
			}
		}
		#endregion Lists
		}catch(InvalidOperationException){}
	}

	#region Search Voids


	void CorrectElements () {
		foreach ( DKSlotData slot in _DKUMA_Variables.DKSlotsList ){
			if ( slot.overlayList.Count > 0 ){
				//	foreach ( DKSlotData _slot in slot.overlayList ){
				slot.LinkedOverlayList.AddRange(slot.overlayList);
				slot.overlayList.Clear();
				if ( slot.materialSample == null ) slot.materialSample = _DKUMA_Variables.DKSlotsList[0].materialSample;
				EditorUtility.SetDirty (slot);
			}
			if ( slot.LinkedOverlayList.Count > 0 ){
				foreach ( DKOverlayData overlay in slot.LinkedOverlayList ){
					if ( overlay.LinkedToSlot.Contains(slot) == false ) {
						overlay.LinkedToSlot.Add (slot);
						EditorUtility.SetDirty (overlay);
					}
				}
			}
		}
		AssetDatabase.SaveAssets ();
	}

	void MakePreviews (){
	//	PreviewsList.Clear ();

		for (int i = 0; i < _DKUMA_Variables.DKSlotsList.Count; i ++) {
		//	# if Editor
		//	if ( _DKUMA_Variables.DKSlotsList[i].Preview == null ){
				string path = AssetDatabase.GetAssetPath (_DKUMA_Variables.DKSlotsList[i]);
				path = path.Replace (_DKUMA_Variables.DKSlotsList[i].name+".asset", "");
				if ( _DKUMA_Variables.DKSlotsList[i].meshRenderer.gameObject != null ){
					Texture2D Preview;
					Preview = AssetPreview.GetAssetPreview( _DKUMA_Variables.DKSlotsList[i].meshRenderer.gameObject);
				//	Preview.name = "preview (Double click)";
					if ( Preview ){
						AssetDatabase.CreateAsset (Preview, path+"Preview-"+_DKUMA_Variables.DKSlotsList[i].name+".asset");
					//	DKSlotsList[i].Preview = AssetDatabase.LoadAssetAtPath(path+"Preview-"+DKSlotsList[i].name+".asset", typeof(Texture2D) ) as Texture2D;
					//	PreviewsList.Add (Preview);
					}
				}
		//	}
		//	# endif
		}

	//	# if UNITY_EDITOR
		foreach ( DKSlotData _DKSlotData in _DKUMA_Variables.DKSlotsList ) {

			//	Texture2D Preview = AssetPreview.GetAssetPreview( DKSlotsList[i].meshRenderer.renderer);
		//	if ( _DKSlotData.Preview == null ){
				string path = AssetDatabase.GetAssetPath (_DKSlotData);
				path = path.Replace (_DKSlotData.name+".asset", "");
				if ( _DKSlotData.meshRenderer.gameObject != null ){
					Texture2D Preview;
					Preview = AssetPreview.GetAssetPreview( _DKSlotData.meshRenderer.gameObject);
					//	Preview.name = "preview (Double click)";
					if ( Preview ){
						AssetDatabase.CreateAsset (Preview, path+"Preview-"+_DKSlotData.name+".asset");
					//	_DKSlotData.Preview = AssetDatabase.LoadAssetAtPath(path+"Preview-"+_DKSlotData.name+".asset", typeof(Texture2D) ) as Texture2D;
					//	PreviewsList.Add (Preview);
					}
				}
			}
	//	}
	//	# endif
		// assign
	/*	for (int i = 0; i < DKSlotsList.Count; i ++) {
			string path = AssetDatabase.GetAssetPath (DKSlotsList[i]);
			path = path.Replace (DKSlotsList[i].name+".asset", "");
			DKSlotsList[i].Preview = AssetDatabase.LoadAssetAtPath(path+"Preview-"+DKSlotsList[i].name+".asset", typeof(Texture2D) ) as Texture2D;
		}*/
	}

	void ImportAll (){
		// Slots
		for (int i = 0; i < _DKUMA_Variables.UMASlotsList.Count; i++) {
			if (_DKUMA_Variables.DKSlotsNamesList.Contains(_DKUMA_Variables.UMASlotsList[i].name) == false ){

				AddToDK(_DKUMA_Variables.UMASlotsList[i].GetType(), _DKUMA_Variables.UMASlotsList[i] as UnityEngine.Object);
			}
		}
		// Overlays
		for (int i = 0; i < _DKUMA_Variables.UMAOverlaysList.Count; i++) {
			if (_DKUMA_Variables.DKOverlaysNamesList.Contains(_DKUMA_Variables.UMAOverlaysList[i].name) == false ){
				
				AddToDK(_DKUMA_Variables.UMAOverlaysList[i].GetType(), _DKUMA_Variables.UMAOverlaysList[i] as UnityEngine.Object);
			}
		}
		Debug.Log ("All the UMA elements have been converted to DK UMA. You can select and set them up by using the lists of the current 'UMA to DK' window.");
	}
	#endregion Import all Element

	#region Add Element
	void AddToDK (System.Type type, UnityEngine.Object Element){

		#region Slot
		// verify the type
		if ( type.ToString() == "UMA.SlotData" ){
		// create the new DK element
			DKSlotData newSlot = new DKSlotData() ;
		//copy the values from the UMA element
			newSlot.name = (Element as UMA.SlotData).name ;
			newSlot.meshRenderer = (Element as UMA.SlotData).meshRenderer ;
			newSlot.listID = (Element as UMA.SlotData).listID ;
			newSlot.materialSample = (Element as UMA.SlotData).materialSample ;
			newSlot.meshRenderer = (Element as UMA.SlotData).meshRenderer ;
			newSlot.overlayScale = (Element as UMA.SlotData).overlayScale ;
			newSlot.slotName = (Element as UMA.SlotData).slotName ;
			newSlot.umaBoneData = (Element as UMA.SlotData).umaBoneData ;

			// Add the correct slotDNA TO VERIFY IF IT'S NECESSARY
			
			// add the DK element to the DK...List
			_DKUMA_Variables.DKSlotsList.Add (newSlot);

		// add the name to the DK...NamesList
			_DKUMA_Variables.DKSlotsNamesList.Add (newSlot.name);

		// Create the prefab
			System.IO.Directory.CreateDirectory("Assets/DK Editors/DK_UMA_Editor/Elements/Slots/");
			// Gender select and create the asset
			// Male
			if ( newSlot.name.Contains("Male") == true && newSlot.name.Contains("Female") == false ){ 
				System.IO.Directory.CreateDirectory("Assets/DK Editors/DK_UMA_Editor/Elements/Slots/Male/");
				System.IO.Directory.CreateDirectory("Assets/DK Editors/DK_UMA_Editor/Elements/Slots/Male/"+newSlot.name+"/");
				string _path = ("Assets/DK Editors/DK_UMA_Editor/Elements/Slots/Male/"+newSlot.name+"/"+newSlot.name+".asset");
				newSlot._UMA = Element as  UMA.SlotData;
				AssetDatabase.CreateAsset(newSlot, _path);
				AssetDatabase.Refresh ();
				Selection.activeObject = newSlot;

			}
			// Female
			if ( newSlot.name.Contains("Female") && newSlot.name.Contains("Male") == false ){ 
				System.IO.Directory.CreateDirectory("Assets/DK Editors/DK_UMA_Editor/Elements/Slots/Female/");
				System.IO.Directory.CreateDirectory("Assets/DK Editors/DK_UMA_Editor/Elements/Slots/Female/"+newSlot.name+"/");
				string _path = ("Assets/DK Editors/DK_UMA_Editor/Elements/Slots/Female/"+newSlot.name+"/"+newSlot.name+".asset");
				newSlot._UMA = Element as  UMA.SlotData;
				AssetDatabase.CreateAsset(newSlot, _path);
				AssetDatabase.Refresh ();
				Selection.activeObject = newSlot;
			}
			// Shared
			if ( newSlot.name.Contains("Female") == false && newSlot.name.Contains("Male") == false ){ 
				System.IO.Directory.CreateDirectory("Assets/DK Editors/DK_UMA_Editor/Elements/Slots/Shared/");
				System.IO.Directory.CreateDirectory("Assets/DK Editors/DK_UMA_Editor/Elements/Slots/Shared/"+newSlot.name+"/");
				string _path = ("Assets/DK Editors/DK_UMA_Editor/Elements/Slots/Shared/"+newSlot.name+"/"+newSlot.name+".asset");
				newSlot._UMA = Element as  UMA.SlotData;
				AssetDatabase.CreateAsset(newSlot, _path);
				AssetDatabase.Refresh ();
				Selection.activeObject = newSlot;
			}

			// TODO
				// Optionnal :
			// Move textures and other datas to the DK Elements Folder

			// Delete the UMA element
		}
		#endregion Slot

		#region Overlay
		// verify the type
		if ( type.ToString() == "UMA.OverlayData" ){
			// test TODELETE
		//	Debug.Log ( "type : "+type.ToString()+" / "+Element.name );
			
			// create the new DK element
			DKOverlayData newOverlay = new DKOverlayData() ;
			//copy the values from the UMA element
			newOverlay.name = (Element as UMA.OverlayData).name ;
			newOverlay.channelAdditiveMask = (Element as UMA.OverlayData).channelAdditiveMask ;
			newOverlay.listID = (Element as UMA.OverlayData).listID ;
			newOverlay.channelMask = (Element as UMA.OverlayData).channelMask ;
			newOverlay.color = (Element as UMA.OverlayData).color ;
			newOverlay.textureList = (Element as UMA.OverlayData).textureList ;
			newOverlay.overlayName = (Element as UMA.OverlayData).overlayName ;
			newOverlay.rect = (Element as UMA.OverlayData).rect ;
			newOverlay.tags = (Element as UMA.OverlayData).tags ;

			// Add the correct slotDNA TO VERIFY IF IT'S NECESSARY
			
			// add the DK element to the DK...List
			_DKUMA_Variables.DKOverlaysList.Add (newOverlay);
			
			// add the name to the DK...NamesList
			_DKUMA_Variables.DKOverlaysNamesList.Add (newOverlay.name);
			
			// Create the prefab
			System.IO.Directory.CreateDirectory("Assets/DK Editors/DK_UMA_Editor/Elements/Overlays/");
			// Gender select and create the asset
			// Male
			if ( newOverlay.name.Contains("Male") == true && newOverlay.name.Contains("Female") == false ){ 
				System.IO.Directory.CreateDirectory("Assets/DK Editors/DK_UMA_Editor/Elements/Overlays/Male/");
				System.IO.Directory.CreateDirectory("Assets/DK Editors/DK_UMA_Editor/Elements/Overlays/Male/"+newOverlay.name+"/");
				string _path = ("Assets/DK Editors/DK_UMA_Editor/Elements/Overlays/Male/"+newOverlay.name+"/"+newOverlay.name+".asset");
				newOverlay._UMA = Element as  UMA.OverlayData;
				AssetDatabase.CreateAsset(newOverlay, _path);
				AssetDatabase.Refresh ();
				Selection.activeObject = newOverlay;
			}
			// Female
			if ( newOverlay.name.Contains("Female") && newOverlay.name.Contains("Male") == false ){ 
				System.IO.Directory.CreateDirectory("Assets/DK Editors/DK_UMA_Editor/Elements/Overlays/Female/");
				System.IO.Directory.CreateDirectory("Assets/DK Editors/DK_UMA_Editor/Elements/Overlays/Female/"+newOverlay.name+"/");
				string _path = ("Assets/DK Editors/DK_UMA_Editor/Elements/Overlays/Female/"+newOverlay.name+"/"+newOverlay.name+".asset");
				newOverlay._UMA = Element as  UMA.OverlayData;
				AssetDatabase.CreateAsset(newOverlay, _path);
				AssetDatabase.Refresh ();
				Selection.activeObject = newOverlay;
			}
			// Shared
			if ( newOverlay.name.Contains("Female") == false && newOverlay.name.Contains("Male") == false ){
				System.IO.Directory.CreateDirectory("Assets/DK Editors/DK_UMA_Editor/Elements/Overlays/Shared/");
				System.IO.Directory.CreateDirectory("Assets/DK Editors/DK_UMA_Editor/Elements/Overlays/Shared/"+newOverlay.name+"/");
				string _path = ("Assets/DK Editors/DK_UMA_Editor/Elements/Overlays/Shared/"+newOverlay.name+"/"+newOverlay.name+".asset");
				newOverlay._UMA = Element as  UMA.OverlayData;
				AssetDatabase.CreateAsset(newOverlay, _path);
				AssetDatabase.Refresh ();
				Selection.activeObject = newOverlay;

			}
			// TODO
			// Optionnal :
			// Move textures and other datas to the DK Elements Folder
			
			// Delete the UMA element
		}
		#endregion Slot
	}
	#endregion Add Element

	void LinkAll (){
		#region link slots
		foreach ( DKSlotData slot in _DKUMA_Variables.DKSlotsList ){
			if ( slot._UMA == null ) {
				foreach ( UMA.SlotData data in _DKUMA_Variables.UMASlotsList ){
					if ( data.slotName == slot.slotName ) slot._UMA = data;
				}
			} 
			if ( slot._UMA == null ) Debug.LogError ( slot.name+" has no link to the original UMA SlotData. To fixe it, select "
			                                         +slot.name+" and assign the UMA SlotData to the corresponding field." );
			EditorUtility.SetDirty (slot);
		}
	
		#endregion link slots

		#region link overlays
		foreach ( DKOverlayData overlay in _DKUMA_Variables.DKOverlaysList ){
			if ( overlay._UMA == null ) {
				foreach ( UMA.OverlayData data in _DKUMA_Variables.UMAOverlaysList ){
					if ( data.overlayName == overlay.overlayName ) overlay._UMA = data;
				}
			} 
			if ( overlay._UMA == null ) Debug.LogError ( overlay.name+" has no link to the original UMA OverlayData. To fixe it, select "
			                                         +overlay.name+" and assign the UMA OverlayData to the corresponding field called 'UMA'." );
			EditorUtility.SetDirty (overlay);
		}
		#endregion link overlays
		AssetDatabase.SaveAssets ();
	}

	#region Add To Lib
	void AddToLib (){
		_DK_UMACrowd = GameObject.Find("DKUMACrowd").GetComponent<DK_UMACrowd>(); 

		// Slots
		for (int i = 0; i < _DKUMA_Variables.DKSlotsList.Count; i++) {
			if ( InResults ) {
				if (( ((_DKUMA_Variables.DKSlotsList[i] as DKSlotData).slotName.ToLower().Contains(SearchString.ToLower())) 
					|| SearchString == "" )
				   && _DK_UMACrowd.slotLibrary.slotDictionary.ContainsValue(_DKUMA_Variables.DKSlotsList[i] as DKSlotData) == false
				 ){
					_DK_UMACrowd.slotLibrary.AddSlot( _DKUMA_Variables.DKSlotsList[i].name, _DKUMA_Variables.DKSlotsList[i] as DKSlotData);

				}
			}else if ( _DK_UMACrowd.slotLibrary.slotDictionary.ContainsValue(_DKUMA_Variables.DKSlotsList[i] as DKSlotData) == false
			          ){
				_DK_UMACrowd.slotLibrary.AddSlot( _DKUMA_Variables.DKSlotsList[i].name, _DKUMA_Variables.DKSlotsList[i] as DKSlotData);
			}
		}
		// Overlays
		for (int i = 0; i < _DKUMA_Variables.DKOverlaysList.Count; i++) {
			if ( InResults ) {
				if (((InResults && (_DKUMA_Variables.DKOverlaysList[i] as DKOverlayData).overlayName.ToLower().Contains(SearchString.ToLower())) 
				    || SearchString == "" )
					&& _DK_UMACrowd.overlayLibrary.overlayDictionary.ContainsValue(_DKUMA_Variables.DKOverlaysList[i] as DKOverlayData) == false){
					_DK_UMACrowd.overlayLibrary.AddOverlay( _DKUMA_Variables.DKOverlaysList[i].overlayName, _DKUMA_Variables.DKOverlaysList[i] as DKOverlayData);

				}
			}else if ( _DK_UMACrowd.overlayLibrary.overlayDictionary.ContainsValue(_DKUMA_Variables.DKOverlaysList[i] as DKOverlayData) == false){
				_DK_UMACrowd.overlayLibrary.AddOverlay( _DKUMA_Variables.DKOverlaysList[i].overlayName, _DKUMA_Variables.DKOverlaysList[i] as DKOverlayData);
				
			}
		}
		Debug.Log ("All the DK elements added to the slots and the overlays libraries.");
		Debug.Log ("DK Helper : You are now able to setup your new elements for DK to use them properly. If you are using a setting pack for a 3rd party content, you just need to create the RPG lists.");

		EditorUtility.SetDirty(_DK_UMACrowd.overlayLibrary);
		EditorUtility.SetDirty(_DK_UMACrowd.slotLibrary);
		AssetDatabase.SaveAssets();
	}

	void AddToUMALib (){
		// to be changed for next UMA version
	//	GameObject UMAContext = GameObject.Find("UMAContext");
		if ( _SlotLibrary == null ) _SlotLibrary = GameObject.Find("SlotLibrary").GetComponent<SlotLibrary>();
		if ( _OverlayLibrary == null ) _OverlayLibrary = GameObject.Find("OverlayLibrary").GetComponent<OverlayLibrary>();

		List<UMA.SlotData> LibSlotsList = new List<UMA.SlotData>();
		if ( _SlotLibrary != null ) LibSlotsList = _SlotLibrary.GetAllSlots ().ToList ();

		// Slots
		if ( _SlotLibrary != null ) for (int i = 0; i < _DKUMA_Variables.UMASlotsList.Count; i++) {
			if ( LibSlotsList.Contains(_DKUMA_Variables.UMASlotsList[i] as UMA.SlotData) == false)
				_SlotLibrary.AddSlot( _DKUMA_Variables.UMASlotsList[i] as UMA.SlotData);
		}

		List<UMA.OverlayData> LibOvsList = new List<UMA.OverlayData>();
		if ( _OverlayLibrary != null ) LibOvsList = _OverlayLibrary.GetAllOverlays ().ToList ();

		// Overlays
		if ( _OverlayLibrary != null )for (int i = 0; i < _DKUMA_Variables.UMAOverlaysList.Count; i++) {
			if ( LibOvsList.Contains(_DKUMA_Variables.UMAOverlaysList[i] as UMA.OverlayData) == false)
				_OverlayLibrary.AddOverlay( _DKUMA_Variables.UMAOverlaysList[i] as UMA.OverlayData);
		}
		Debug.Log ("All the UMA elements added to the slots and the overlays libraries.");
		Debug.Log ("DK Helper : You are now able to generate a UMA clone of every new DK avatar. To do so you have to enable 'To UMA' in the 'Create' Tab of the DK UMA Editor window.");
	}
	#endregion Add to Lib

	#region Remove Element
	void RemoveFromDK (System.Type type, UnityEngine.Object Element){
		// slot
		if ( type.ToString() == "DKSlotData" ){
			for (int i1 = 0; i1 < _DKUMA_Variables.DKSlotsNamesList.Count ; i1++)
			{
				if ( _DKUMA_Variables.DKSlotsNamesList[i1] == Element.name ){
					_DKUMA_Variables.DKSlotsNamesList.Remove (_DKUMA_Variables.DKSlotsNamesList[i1]);

				}
			}
			DestroyImmediate (Element, true);
		}
		// Overlay
		if ( type.ToString() == "DKOverlayData" ){
			for (int i1 = 0; i1 < _DKUMA_Variables.DKOverlaysNamesList.Count ; i1++)
			{
				if ( _DKUMA_Variables.DKOverlaysNamesList[i1] == Element.name ){
					_DKUMA_Variables.DKOverlaysNamesList.Remove (_DKUMA_Variables.DKOverlaysNamesList[i1]);
					
				}
			}
			DestroyImmediate (Element, true);
		}
	}
	#endregion Remove Element

	void InstallUMA (){
		GameObject go = Instantiate(Resources.Load("UMA+NaturalBe")) as GameObject;
		go.name = "UMA2";

		GameObject _UMA = GameObject.Find("UMA");
		List<Transform> _List =  go.transform.GetComponentsInChildren<Transform>().ToList();

		for (int i1 = 0; i1 < _List.Count ; i1++)
		{
			_List[i1].parent = _UMA.transform;
		}
		DestroyImmediate (go);
	}

	void OnProjectChange() {
		if ( Action == "Detecting" ){
		//	Debug.ClearDeveloperConsole();
		//	Debug.Log ("Console cleaned");
		}
	}

	void OnSelectionChange() {
		_DK_RPG_UMA = null;
		Repaint();
	}
}
