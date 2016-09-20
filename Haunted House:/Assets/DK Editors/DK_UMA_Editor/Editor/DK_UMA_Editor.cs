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

public class DK_UMA_Editor : EditorWindow {
	
	#region Variables
	Color Green = new Color (0.8f, 1f, 0.8f, 1);
	Color Red = new Color (0.9f, 0.5f, 0.5f);
	
	GameObject ObjToFind;
	DKUMADnaHumanoid umaDna;

	bool CreatingUMA;
	bool Helper;
		
	public static bool Step0 = false;
	public static bool Step1 = false;
	public static bool Step2 = false;
	public static bool Step3 = false;
	public static bool Step4 = false;
	public static bool Step5 = false;
	public static bool Step6 = false;
	public static bool Step7 = false;
	public static bool MultipleUMASetup = false;
	
	int RanRace;
	DK_Race DK_Race;

	string Gender;
	string MultiRace;
	
	public static bool ShowPrepare ;
	public static bool showCreate ;
	public static bool showModify ;
	public static bool showMorph ;
	public static bool showColors ;
	public static bool showColors2 ;
	public static bool showComp ;
	public static bool showList ;
	public static bool showSetup ;
	public static bool showPlugIn ;
	public static bool showAbout ;


	public static GameObject SlotExpressions;

	// Prepare

	string UMAObjName ;
	public static bool ShowLibraries = false;
	public static bool ShowDKLibraries = false;
	public static bool ShowDKLib = false;
	bool DisplayLists;

	bool choosePlace = false;
	bool chooseOverlay = false;
	bool chooseSlot = false;
	bool LinkedOverlayList = false;
	bool LinkedSlotList = false;
	Vector2 LinkedOverlayListScroll = new Vector2();
	Vector2 LinkedSlotListScroll = new Vector2();

	int TmpSlotIndex;
	public static string _SpawnPerct;
	public static string SearchString = "";
	
	// Detect Element Types

	public bool DetAll;

	
	// DK Libraries
	string myPath;

	bool ElemAlreadyIn = false;
	bool ShowDKLibSE = false;
	bool ShowDKLibSA = false;
	string NewExpressionName = "";
	string AnatomyPart = "";
	GameObject SelectedExpression;
	DK_SlotExpressionsElement dk_SlotExpressionsElement;
	bool ShowSelectAnatomy = false;
	DK_SlotsAnatomyElement SelectedAnaPart;
	GameObject SelectedAnaPartObj;
	string NewAnatomyName = "";
	bool ChooseLink;
	DK_SlotExpressionsElement AnaToLink;
	
	string SelectedRace = "Both";
	string SelectedPlace = "";
	string OverlayType =  "";
	public static string SelectedPrefabName = "";
	
	// Generator Presets
	public static bool ShowGenPreset = false;
	bool NewGenPreset = false;
	bool EditGenPreset = false;
	public static string NewPresetName =  "";
	public static string NewPresetGender =  "Both";
		
	public static DK_GeneratorPresetLibrary _GeneratorPresetLibrary;
	public static DK_GeneratorPresetLibrary _GeneratorPresetLibrary2;
	float SpawnFrequencySlider = 0.005F;
	
	// modify
	bool UMAEdit;
	DKUMAData SelectedDKUMAData;
	public static Transform EditedModel;
	bool Arms;
	bool Legs;
	bool Torso;
	bool Head;
	bool Face;
	bool Chin;
	bool Cheeks;
	bool Mouth;
	bool Nose;
	bool Eyes;
	bool ChangeOverlay;
	bool AddSlot;

	// Colors
	// Presets

	
	bool ShowSkinTone = false;
	bool ShowEyesColor = false;
	bool ShowTorsoWColor = false;
	bool ShowHairColor = false;
	bool ShowLegWColor = false;

	
	string _Type;
	
	Vector2 scroll;
	Vector2 scroll2;
	Vector2 scroll3;
	Vector2 scroll4;
	
	string Limit ;
	public static string _Name = "";
	string tmpWearWeight = "";
	
	// Plug Ins
	string PlugInSelected;
	public List<PlugInData> PlugInDataList = new List<PlugInData>();
	PlugInData _PlugInData;
	string PlugInPath;
	string[] aFilePaths;
	string[] aWinPaths;
	string FileName;
	string SelectedFile;
	
	bool ApplyToLib = true;

	bool AddRaces;
	bool AddSlots;
	bool AddOverlays;
	
	public DKUMA_Variables _DKUMA_Variables;

	#endregion Variables
	

	[MenuItem("UMA/DK Editor/UMA Editor %,")]
	[MenuItem("Window/DK Editors/DK UMA/UMA Editor %,")]

	public static void Init()
    {
   		// Get existing open window or if none, make a new one:
		DK_UMA_Editor window = EditorWindow.GetWindow<DK_UMA_Editor> (false, "DK UMA Editor");
		window.autoRepaintOnSceneChange = true;
		window.Show ();
	}
	public static void OpenConvWin(){
	//	GetWindow(typeof(Select_Converter), false, "Limitations");
	}
	public static void OpenColorPresetWin()
	{
		GetWindow(typeof(ColorPreset_Editor), false, "Color Presets");
	}
	public static void OpenChooseOverlayWin()
	{
		GetWindow(typeof(ChooseLinked_Editor), false, "Elements");
	}
	public static void OpenChooseSlotWin()
	{
		GetWindow(typeof(ChooseSlot_Win), false, "Choose Slot");
	}
	public static void OpenPlaceWin()
	{
		GetWindow(typeof(ChooseAnatomy_Win), false, "Anatomy places");
		ChooseAnatomy_Win.Action = "ChoosePlace";
	}
	public static void OpenAnatomy_Editor()
	{
		GetWindow(typeof(Anatomy_Editor), false, "Anatomy Editor");
	//	ChooseAnatomy_Win.Action = "ChoosePlace";
	}
	public static void OpenRaceEditor()
    {
		GetWindow(typeof(DK_UMA_Race_Editor), false, "DK UMA Race");
		DK_UMA_Race_Editor._RaceData = Selection.activeObject as DKRaceData;
	}
	public static void OpenRaceSelectEditor()
    {
		GetWindow(typeof(DK_UMA_RaceSelect_Editor), false, "Races List");
	}
	public static void OpenProcessWindow()
	{
		GetWindow(typeof(DKUMAProcessWindow), false, "Processing...");
	}
	public static void OpenLibrariesWindow()
	{
		GetWindow(typeof(ChangeLibrary), false, "UMA Libraries");
	}

	public static void OpenDeleteAsset(){
		GetWindow(typeof(DeleteAsset), false, "Deleting");
	}

	public static void OpenPlugInCreator(){
		GetWindow(typeof(NewPlugInWin), false, "New Plug-In");
	}
	public static void OpenAutoDetectWin(){
		GetWindow(typeof(AutoDetect_Editor), false, "Manager");
	}
	public static void OpenExpressionsWin(){
		GetWindow(typeof(Expression_Editor), false, "Expressions");
	}
	public static void OpenBrowser(){
		GetWindow(typeof(DK_UMA_Browser), false, "DKUMA Browser");
	}
	public static void OpenDKConvWin(){
		GetWindow(typeof(UMAConvAvatarWin), false, "Convert 2 DK");
	}
	public static void OpenRPGCharacterWin(){
	//	GetWindow(typeof(DK_RPG_UMA_Avatar_Win), false, "RPG Avatar");
	}
	public static void OpenVersionWin(){
		GetWindow(typeof(DKUMAVersionWin), false, "DK UMA Vers");
	}
	public static void OpenVideoLink(){
		Application.OpenURL ("http://www.youtube.com/playlist?list=PLz3lDsmTMvxZpbSXp79gRm3XOiZs31g5t");
	}
	public static void OpenDocumentationLink(){
		Application.OpenURL ("http://alteredreality.wix.com/dk-uma#!add-the-uma-elements/cnab");
	}

	public static void OnEnable(){
		DetectAndAddDK.DetectAll();
	}
	public static void Awake(){
		DetectAndAddDK.DetectAll();
	}


	void OnGUI () {
		GameObject DK_UMA = GameObject.Find("DK_UMA");
		if ( DK_UMA == null ) {
			DK_UMA = (GameObject)PrefabUtility.InstantiatePrefab (Resources.Load ("DK_UMA"));
			DK_UMA.name = "DK_UMA";
			DK_UMA = GameObject.Find("DK_UMA");
		}

		_DKUMA_Variables = DK_UMA.GetComponent<DKUMA_Variables>();
		if ( _DKUMA_Variables == null ) _DKUMA_Variables =  DK_UMA.AddComponent<DKUMA_Variables>() as DKUMA_Variables;
		EditorVariables.UseDkUMA = _DKUMA_Variables.UseDkUMA;
		EditorVariables.UseUMA = _DKUMA_Variables.UseUMA;

		if (EditorVariables.UseDkUMA && EditorVariables.UMACrowdObj ){
			DK_UMACrowd _DK_UMACrowd = EditorVariables.UMACrowdObj.GetComponent<DK_UMACrowd>();
			_DKUMA_Variables = DK_UMA.GetComponent<DKUMA_Variables>();
			if ( _DKUMA_Variables == null ) _DKUMA_Variables =  DK_UMA.GetComponent("DKUMA_Variables") as DKUMA_Variables;
			if ( _DKUMA_Variables == null ) _DKUMA_Variables =  DK_UMA.AddComponent<DKUMA_Variables>() as DKUMA_Variables;
			if ( _DK_UMACrowd.raceLibrary == null ) _DK_UMACrowd.raceLibrary = GameObject.Find( "DKRaceLibrary").GetComponent<DKRaceLibrary>();
			if ( _DK_UMACrowd.slotLibrary == null ) _DK_UMACrowd.slotLibrary = GameObject.Find( "DKSlotLibrary").GetComponent<DKSlotLibrary>();
			if ( _DK_UMACrowd.overlayLibrary == null ) _DK_UMACrowd.overlayLibrary = GameObject.Find( "DKOverlayLibrary").GetComponent<DKOverlayLibrary>();
		}

		this.minSize = new Vector2(370, 500);

		if ( EditorVariables.UseDkUMA && !EditorVariables.DK_UMACrowd || !EditorVariables.DK_DKUMACustomization || !EditorVariables.DK_DKUMAGenerator ) DetectAndAddDK.AddAll();
	
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

		Repaint();

		#region Menu
		ObjToFind = GameObject.Find("NPC Models");
		if ( ObjToFind == null ) 
		{	var go = new GameObject();	go.name = "NPC Models";	ObjToFind = GameObject.Find("NPC Models");		}
		if ( ObjToFind != null )
		{
			EditorVariables.MFSelectedList = ObjToFind.transform;
			if ( EditorVariables.DKUMAGeneratorObj != null ) EditorVariables._DKUMAGenerator =  EditorVariables.DKUMAGeneratorObj.GetComponent<DKUMAGenerator>();
			if ( EditorVariables.DKUMACustomizationObj != null ) EditorVariables._DKUMACustomization =  EditorVariables.DKUMACustomizationObj.GetComponent<DKUMACustomization>();			
		}
		
		#region Title
		try{
			GUILayout.Label ( "Dynamic Kit U.M.A. Editor (v1.6)", "toolbarbutton", GUILayout.ExpandWidth (true));
		}catch(ArgumentException) {}
		#endregion Title
		
		#region Main Menu
		using (new Horizontal()) {
			GUI.color = Color.yellow;
			if (GUILayout.Button ( "Welcome", "toolbarbutton", GUILayout.ExpandWidth (false))) {
				EditorOptions.DisplayWelcome();
			}
			if ( ShowPrepare == true ) GUI.color = Green;
			else GUI.color = Color.white;
			if (EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd && EditorVariables.DK_DKUMACustomization 
			    && GUILayout.Button ( "Prepare", "toolbarbutton", GUILayout.ExpandWidth (false))) {
				EditorOptions.DisplayPrepare ();
			}
		//	if ( EditorVariables.DK_DKUMAGenerator || EditorVariables.DK_UMACrowd || EditorVariables.DK_DKUMACustomization )
			if ( showCreate == true ) GUI.color = Green;
			else GUI.color = Color.white;
			if ( EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd && EditorVariables.DK_DKUMACustomization ){
			   if ( EditorVariables.SlotsAnatomyLibraryObj && EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length != 0 ){
				   if ( GUILayout.Button ( "Create", "toolbarbutton", GUILayout.ExpandWidth (false))) 
					{
						EditorOptions.DisplayCreate ();
					}
				}
			}
			if ( showSetup == true ) GUI.color = Green;
			else GUI.color = Color.white;
			try {
				if (EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd && EditorVariables.DK_DKUMACustomization 
				    && GUILayout.Button ("Setup", "toolbarbutton", GUILayout.ExpandWidth (false))) {
					EditorOptions.DisplaySetup ();
				}
			/*	if ( showPlugIn == true ) GUI.color = Green;
				else GUI.color = Color.white;
				if (EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd && EditorVariables.DK_DKUMACustomization
				&& GUILayout.Button ("Plug In", "toolbarbutton", GUILayout.ExpandWidth (false))) {
				EditorOptions.DisplayPlugIn ();
				}*/
				if ( showAbout == true ) GUI.color = Green;
				else GUI.color = Color.white;
				if (GUILayout.Button ( "About", "toolbarbutton", GUILayout.ExpandWidth (false))) {
					EditorOptions.DisplayAbout ();

				}
				GUI.color = Color.white;
				GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
				if ( !Helper )  GUI.color = Color.yellow;
				else GUI.color = Green;
				if (GUILayout.Button ( "?", "toolbarbutton", GUILayout.ExpandWidth (false))) {
					if ( Helper ) Helper = false;
					else Helper = true;
				}
			}catch (System.ArgumentException ) {}
		}
		#endregion Main Menu
		
		#region Help
		GUILayout.Space(5);
		if ( EditorVariables.DK_DKUMAGenerator == null || EditorVariables.DK_UMACrowd == null || EditorVariables.DK_DKUMACustomization == null ) 
		{
			// help
			GUI.color = Color.yellow;
			GUILayout.TextField("Welcome to the DK UMA Editor. With this tool you can create and edit your DK UMA Avatars in Editor mode or Runtime mode." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.Space(5);	
			GUILayout.TextField("The DK UMA Editor imports any UMA element for DK UMA to be able to use it. At that point of the development, DK UMA can not edit the basic UMA Avatars, it is planned but not effective at the moment." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

			GUILayout.Space(5);	
			GUI.color = Color.white;
			GUILayout.TextField("To be able to use it, you have to install the components into your scene." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUILayout.Space(5);	

		}
		#endregion Help


		#region Add Scripts
		//	if ( EditorVariables.DK_DKUMAGenerator == null || EditorVariables.DK_UMACrowd == null || EditorVariables.DK_DKUMACustomization == null ) 
	/*	using (new Horizontal()) {
			if ( !EditorVariables.UseDkUMA ) GUI.color = Color.gray;
			else GUI.color = Green;
			if ( GUILayout.Button("Use DK UMA", GUILayout.ExpandWidth (true))) 
			{
				if ( !EditorVariables.UseDkUMA ){
					_DKUMA_Variables.UseDkUMA = true;
					DetectAndAddDK.AddAll();
				}
				else _DKUMA_Variables.UseDkUMA = false;
			}
			GUI.color = Color.white;
			if ( EditorVariables.UseDkUMA && GUILayout.Button("Save Variables", GUILayout.ExpandWidth (true))) 
			{
				EditorUtility.SetDirty (_DKUMA_Variables);
				AssetDatabase.SaveAssets ();
			}	
*/
		/*	if ( !UseUMA ) GUI.color = Color.gray;
			else GUI.color = Green;
			if ( GUILayout.Button("Use UMA", GUILayout.ExpandWidth (true))) 
			{
				if ( !UseUMA ){
					_DKUMA_Variables.UseUMA = true;
				//	DetectAndAddDK.AddAll();
				}
				else _DKUMA_Variables.UseUMA = false;
			}	*/
	//	}

		#endregion Add Scripts
		if (EditorVariables.UseDkUMA) {
						#region Starting welcome
			if (EditorVariables.DK_DKUMAGenerator != null && EditorVariables.DK_UMACrowd != null && EditorVariables.DK_DKUMACustomization != null && !showPlugIn && !showAbout && !ShowPrepare 
				&& !showCreate && !showModify && !showSetup && !ShowLibraries && !ShowDKLibraries && !ShowGenPreset) 
			{
			//	GUILayout.Space (5);
				GUI.color = Color.white;
				using (new HorizontalCentered()) {
						GUILayout.TextField ("Welcome to the DK UMA Editor", 256, bold, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				}
				using (new ScrollView(ref scroll)) {
					using (new Horizontal()) {
						GUI.color = Green;
						if ( GUILayout.Button("For idea, request or assistance, please contact us", GUILayout.ExpandWidth (true))) 
						{
							Application.OpenURL ( "http://alteredreality.wix.com/dk-uma#!contact/c24vq" );
						}
					}
					GUI.color = Green;
					using (new HorizontalCentered())
					GUILayout.TextField("Enjoy it, rate it ! And get more content." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

										
										#region detect Slots Libraries
				
										#region Anatomy Library detection
										EditorVariables.SlotsAnatomyLibraryObj = GameObject.Find ("DK_SlotsAnatomyLibrary");
										GUI.color = Red;
										if (EditorVariables.SlotsAnatomyLibraryObj == null)
												GUILayout.TextField ("You need to create an Anatomy Library. It contains all the Generic body parts used by the DK Editor to create your Avatars.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
										if (EditorVariables.SlotsAnatomyLibraryObj == null) {
												GUI.color = Green;
												if (GUILayout.Button ("Create the Anatomy Library", GUILayout.ExpandWidth (true))) {
							EditorVariables.SlotsAnatomyLibraryObj = (GameObject)PrefabUtility.InstantiatePrefab (Resources.Load ("DK_SlotsAnatomyLibrary"));
														EditorVariables.SlotsAnatomyLibraryObj.name = "DK_SlotsAnatomyLibrary";
														EditorVariables._SlotsAnatomyLibrary = EditorVariables.SlotsAnatomyLibraryObj.GetComponent<DK_SlotsAnatomyLibrary> ();
														DK_UMA = GameObject.Find ("DK_UMA");
														if (DK_UMA == null) {
																var goDK_UMA = new GameObject ();
																goDK_UMA.name = "DK_UMA";
																DK_UMA = GameObject.Find ("DK_UMA");
														}
														EditorVariables.SlotsAnatomyLibraryObj.transform.parent = DK_UMA.transform;
														DetectAndAddDK.AddAll ();
												}
										} else  if (EditorVariables._SlotsAnatomyLibrary == null)
												EditorVariables._SlotsAnatomyLibrary = EditorVariables.SlotsAnatomyLibraryObj.GetComponent<DK_SlotsAnatomyLibrary> ();
										#endregion Anatomy Library detection
				
										SlotExpressions = GameObject.Find ("Expressions");
										#region Is Expressions Library Empty ?
										if (SlotExpressions == null) {
												GUI.color = Red;
												GUILayout.TextField ("The Expressions Library is umpty. Add the expressions automatically.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
												GUI.color = Green;
												if (GUILayout.Button ("Populate with Prefabs", GUILayout.ExpandWidth (true))) {
														DK_UMACrowd _DK_UMACrowd = GameObject.Find ("DKUMACrowd").GetComponent<DK_UMACrowd> ();
														DK_UMA = GameObject.Find ("DK_UMA");
														SlotExpressions = (GameObject)Instantiate (Resources.LoadAssetAtPath ("Assets/DK Editors/DK_UMA_Editor/Prefabs/Resources/DKExpressions.prefab", typeof(GameObject)));
														SlotExpressions.name = "Expressions";
														SlotExpressions.transform.parent = DK_UMA.transform;
														PrefabUtility.ReconnectToLastPrefab (SlotExpressions);
							DetectAndAddDK.GetAssetsOfType (_DK_UMACrowd.Default.ResearchDefault.DK_ExpressionData.GetType (), ".asset");



												}
										}
										#endregion Is Expressions Library Empty ?
				
										#endregion detect Slots Libraries
										if (EditorVariables.SlotsAnatomyLibraryObj && EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length != 0
												&& SlotExpressions != null
					    && EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd && EditorVariables.DK_DKUMACustomization) {

						GUI.color = Color.white;
						using (new Horizontal()) {
							if ( GUILayout.Button ( "Version information", GUILayout.ExpandWidth (true))) {
								OpenVersionWin ();
							}
							if (  GUILayout.Button("Video examples", GUILayout.ExpandWidth (true))){
								OpenVideoLink ();
							}
							if (  GUILayout.Button("Web Documentation", GUILayout.ExpandWidth (true))){
								OpenDocumentationLink ();
							}
						}

						GUILayout.Space (10);
						GUI.color = Color.white;
						GUILayout.Label ("commonly used windows", "toolbarbutton", GUILayout.ExpandWidth (true));				

						if ( GUILayout.Button ( "Open the Elements Manager", GUILayout.ExpandWidth (true))) {
							OpenAutoDetectWin();
						}
					/*	if (  GUILayout.Button("Open RPG Avatar Editor", GUILayout.ExpandWidth (true))){
							OpenRPGCharacterWin ();
						}*/
						if (  GUILayout.Button("Open the Scene Browser", GUILayout.ExpandWidth (true))){
							OpenBrowser ();
						}

						GUILayout.Space (5);
						GUI.color = Color.yellow;
						GUILayout.TextField ("Helper : You can activate the helper comments by clicking on the '?' at the end of the ontop menu. "
						    , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						GUILayout.Space (5);

						GUILayout.TextField ("Navigation : You can navigate from any DK UMA Panel to another menu" +
							" by clicking on the tabs of the DK Editor window.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

						GUI.color = Color.white;

						GUILayout.Label ("Navigate", "toolbarbutton", GUILayout.ExpandWidth (true));				
						GUILayout.TextField ("In the Prepare panel you can setup the DK UMA contents. It is dynamic, just select a UMA or DK element and the corresponding options will be displayed.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						GUILayout.TextField ("Also from the Prepare panel can be set the DK engine and all the UMA content can be imported to DK UMA.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

						if (EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd && EditorVariables.DK_DKUMACustomization && GUILayout.Button ("Go to the Prepare menu", GUILayout.ExpandWidth (true))) {
							ShowPrepare = true;
							Step0 = true;
							ResetSteps ();
							
						}


												GUI.color = Color.white;
												GUILayout.TextField ("Use the Create Panel to create an Avatar or multiple ones, just follow the steps to create it as you want it," +
														" randomly or by setting up any aspect of the model.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
												GUILayout.Space (5);
												GUI.color = Color.white;
						if (EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd && EditorVariables.DK_DKUMACustomization 
														&& EditorVariables.SlotsAnatomyLibraryObj && EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length != 0
														&& GUILayout.Button ("Go to the Create menu", GUILayout.ExpandWidth (true))) {
														showCreate = true;
														Step0 = true;
														ResetSteps ();
														
												}
					
											//	GUI.color = Color.white;
											//	GUILayout.TextField ("Open the Plug In Panel to use a DK UMA plug-in.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
											//	GUILayout.Space (5);
											//	GUI.color = Color.white;
											//	if (EditorVariables.SlotsAnatomyLibraryObj && EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length != 0
						//			&& EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd && EditorVariables.DK_DKUMACustomization && GUILayout.Button ("Plug-Ins menu", GUILayout.ExpandWidth (true))) {

											//			showPlugIn = true;
											//	}
										}
								}
						}
						#endregion Starting welcome
		
						#region Prepare
						if (ShowPrepare) {
				OnSelectionChange();
								if (!ShowLibraries && !ShowDKLibraries && !ShowGenPreset) {
										if (Helper) {
												GUI.color = Color.white;
												GUILayout.TextField ("In the Prepare panel you can setup the UMA engine and its content !", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
										}
										#region Prepare Menu
										GUI.color = Color.yellow;
										if (Helper)
												GUILayout.TextField ("Configure the DK UMA Libraries to be able to use it. Open the Libraries Tool to verify if all is correctly configured.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
										using (new Horizontal()) {
												GUI.color = Color.white;
												if (GUILayout.Button ("DK Engine", GUILayout.ExpandWidth (true))) {
														ShowDKLibraries = true;
												}
												if (GUILayout.Button ("Libraries List", GUILayout.ExpandWidth (true))) {
														DK_UMA_Editor.OpenLibrariesWindow ();
														ChangeLibrary.EditorMode = true;
												}

												// auto detect
												GUI.color = Green;
												if (GUILayout.Button ("Auto Detect Tool", GUILayout.ExpandWidth (true))) {
														ShowLibraries = true;
														EditorVariables.AutoDetLib = true;
												}
										}
										GUI.color = Color.white;
										GUILayout.Label ("Content Wizard", "toolbarbutton", GUILayout.ExpandWidth (true));
										GUI.color = Color.yellow;

										if (Selection.activeObject && Selection.activeObject.GetType ().ToString ().Contains ("UMA.")) {
												GUI.color = Color.yellow;
												GUILayout.TextField ("Your selection is a UMA Element.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
												GUI.color = Color.white;
												GUILayout.TextField ("You can convert a UMA Element for DK to be able to use it.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
												GUILayout.TextField (" the Converter will search all your elements for UMA or DK, then it will be possible to Convert all the UMA elements to DK, using a click..", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
												GUILayout.Space (10);
												GUI.color = Green;
												if (GUILayout.Button ("UMA and DK UMA contents", GUILayout.ExpandWidth (true))) {
														OpenAutoDetectWin ();
												}
										}

										// Plug-In
										if (Selection.activeObject && Selection.activeObject.GetType ().ToString () == "PlugInData") {
												GUILayout.TextField ("Your selection is a Plug-In File.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
										}
										// Export project
										if (Selection.activeObject && Selection.activeObject.GetType ().ToString () == "ExportData") {
												string _Path = AssetDatabase.GetAssetPath (Selection.activeObject);
												if (_Path.Contains ("Assets/DK Editors/DK_UMA_Editor/Exporter/Exporting/") == true) { 
														GUI.color = Color.yellow;
														GUILayout.TextField ("Your selection is an Export project, select the Plug-In Tab of the Editor Window, " +
																"then click on the button to open the DK UMA Exporter. ", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
														GUILayout.TextField ("If you can't find the Importer in your Plug-Ins List, you will have to download and install. " +
																"then click on the button to open the DK UMA Importer. ", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
														GUI.color = Color.white;
														using (new Horizontal()) {
																if (GUILayout.Button ("Open the Plug-Ins Tab", GUILayout.ExpandWidth (true))) {
																		ShowPrepare = false;
																		showPlugIn = true;
																}
														}
												}
												if (_Path.Contains ("Assets/DK Editors/DK_UMA_Editor/Exporter/Incoming/") == true) {
														GUI.color = Color.yellow;
														GUILayout.TextField ("Your selection is an Incoming project, select the Plug-In Tab of the Editor Window, " +
																"then click on the button to open the DK UMA Importer. ", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
														GUILayout.TextField ("If you can't find the Importer in your Plug-Ins List, you will have to download and install. " +
																"then click on the button to open the DK UMA Importer. ", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

														GUI.color = Color.white;
														using (new Horizontal()) {
																if (GUILayout.Button ("Open the Plug-Ins Tab", GUILayout.ExpandWidth (true))) {
																		ShowPrepare = false;
																		showPlugIn = true;
																}
														}
												}
										}
					// create a RPG element
					if ((Selection.activeGameObject 
					     && Selection.activeGameObject.GetComponent<Transform> () == true
					     && Selection.activeGameObject.GetComponent<DK_Model> () == null
					     && Selection.activeGameObject.GetComponent<DKUMAData> () == null
					     && Selection.activeGameObject.GetComponentInChildren<DKUMAData> () == null
					     && Selection.activeGameObject.GetComponentInParent<DKUMAData> () == null
					     && Selection.activeGameObject.GetComponent<UMA.UMAData> () == null
					     && Selection.activeGameObject.GetComponentInParent<UMA.UMAData> () == null
					     && Selection.activeGameObject.GetComponentInChildren<UMA.UMAData> () == null
					     && Selection.activeGameObject.GetComponentInChildren<SkinnedMeshRenderer> () != null)) 
					{

						if ( Selection.activeGameObject.GetComponent<DK_RPG_Element> () == null 
						    && Selection.activeGameObject.GetComponentInChildren<DK_RPG_Element> () == null
						    && Selection.activeGameObject.GetComponentInParent<DK_RPG_Element> () == null ){
							GUILayout.TextField ("You can create a RPG element from your current selection. add the necessary scripts by clicking on the button.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
							if ( GUILayout.Button ("Add the RPG Element Script to the selection", GUILayout.ExpandWidth (true))) {
								DK_RPG_Element.CreateOnGameObject(Selection.activeGameObject);
							}
						}
						else {
							// Instructions
							GUILayout.TextField ("A DK RPG UMA Element is a simple definition of an ensemble of values. Use the Inspector to modify the values.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
							GUILayout.TextField ("The first on is the Slot you want to add to the avatar" +
								", the second  is the Overlay for the slot and the thrid one is the ColorPreset.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
							GUILayout.TextField ("Select the Slot for corresponding gender, or select for both of then for the Element to be available for both.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
							GUILayout.TextField ("The selected Slot must be ready to be used by the desired gender.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
							GUILayout.TextField ("If the Slot is null but the Overlay is not, that means for the generator that the overlay IS the element and it will be applied on the body of the avatar.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

							// RPG Collider
							if ( Selection.activeGameObject.GetComponent<DK_RPG_Element_Collider> () == null
							    && Selection.activeGameObject.GetComponentInChildren<DK_RPG_Element_Collider> () == null
							    && Selection.activeGameObject.GetComponentInParent<DK_RPG_Element_Collider> () == null ){
								GUILayout.TextField ("You can add a RPG UMA Collider to your element. add the necessary scripts by clicking on the button.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
								GUILayout.TextField ("This Collider will add the element to your avatar.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
								if ( GUILayout.Button ("Add the RPG Collider Script to the selection", GUILayout.ExpandWidth (true))) {
								DK_RPG_Element_Collider.CreateOnGameObject(Selection.activeGameObject);
								}
							}
						}
					}
										// Others
										if (Selection.activeObject == null && Selection.activeGameObject == null) {
												GUILayout.TextField ("select an Element or Avatar from your Assets or by using the DK Browser.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
										}
								/*		if ((Selection.activeGameObject 
												&& EditedModel
												&& EditedModel != Selection.activeGameObject.transform
												&& Selection.activeGameObject.GetComponent<Transform> () == true
												&& Selection.activeGameObject.GetComponent<DK_Model> () == null
											     && Selection.activeGameObject.GetComponent<DKUMAData> () == null
											     && Selection.activeGameObject.GetComponent<UMA.UMAData> () == null
											     && Selection.activeGameObject.GetComponent<SkinnedMeshRenderer> () == null)) {
												GUILayout.TextField ("You can had the Spawn function to any GameObject. The Spawn function is a spawn point to create a DK UMA Avatar, you can set it with all the DK UMA options.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
												if (GUILayout.Button ("Add the Spawn Script to the GameObject", GUILayout.ExpandWidth (true))) {

												}
										}*/


					else{
						if ((Selection.activeGameObject 
						     && (Selection.activeGameObject.GetComponentInParent<UMA.UMAData> () != null
						    || Selection.activeGameObject.GetComponentInParent<UMADynamicAvatar> () != null
						    || Selection.activeGameObject.GetComponent<UMADynamicAvatar> () != null )
						     && Selection.activeGameObject.GetComponentInParent<DKUMAData> () == null ))
						{
							GUI.color = Color.white;
							GUILayout.TextField ("Your selection is a UMA Avatar. You can import it into DK UMA to be able to handle it with all the Editor Options. ", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
							GUI.color = Green;
							if (GUILayout.Button ("Convert to DK UMA", GUILayout.ExpandWidth (true))) {
								OpenDKConvWin ();
							}
							
						}
					}

										// Selected model
										if (Selection.activeGameObject) {
					
												if ((Selection.activeGameObject.transform.parent
														&& Selection.activeGameObject.transform.parent.GetComponent<DKUMAData> () == true)
														|| Selection.activeGameObject.GetComponent<DKUMAData> () == true
														|| Selection.activeGameObject.GetComponent<DK_Model> () == true) {
														showModify = true;
												} else
														showModify = false;
										}

										// selected Slot
										if (Selection.activeObject && Selection.activeObject.GetType ().ToString () == "DKSlotData") {
												GUI.color = Color.white;
												using (new Horizontal()) {
														GUI.color = Color.yellow;
														GUILayout.Label ("Slot Library :", GUILayout.Width (75));
														GUI.color = Color.white;
														GUILayout.TextField (EditorVariables.DKSlotLibraryObj.name, GUILayout.ExpandWidth (true));
														if (GUILayout.Button ("Change", GUILayout.Width (60))) {
																OpenLibrariesWindow ();
																ChangeLibrary.CurrentLibN = EditorVariables.DKSlotLibraryObj.name;
																ChangeLibrary.CurrentLibrary = EditorVariables.DKSlotLibraryObj;
																ChangeLibrary.Action = "";
														}
												}
												GUILayout.Space (5);
												GUI.color = Green;
												DKSlotLibrary DKSlotLibrary = EditorVariables.DKSlotLibraryObj.GetComponent<DKSlotLibrary> ();
												if (DKSlotLibrary.slotElementList.Contains ((Selection.activeObject as DKSlotData)) == false) {
														if (GUILayout.Button ("Add to selected Library", GUILayout.ExpandWidth (true))) {
																List<DKSlotData> DKSlotLibraryL;
																DKSlotLibraryL = DKSlotLibrary.slotElementList.ToList ();
																DKSlotLibraryL.Add ((Selection.activeObject as DKSlotData));
																DKSlotLibrary.slotElementList = DKSlotLibraryL.ToArray ();
																EditorUtility.SetDirty (DKSlotLibrary);
																AssetDatabase.SaveAssets ();
																OnSelectionChange ();
														}
												}
										} else
				// selected Overlay
				if (Selection.activeObject && Selection.activeObject.GetType ().ToString () == "DKOverlayData") {
												GUI.color = Color.white;
												using (new Horizontal()) {
														GUI.color = Color.yellow;
														GUILayout.Label ("Overlay Library :", GUILayout.Width (75));
														GUI.color = Color.white;
														GUILayout.TextField (EditorVariables.OverlayLibraryObj.name, GUILayout.ExpandWidth (true));
														if (GUILayout.Button ("Change", GUILayout.Width (60))) {
																OpenLibrariesWindow ();
																ChangeLibrary.CurrentLibN = EditorVariables.OverlayLibraryObj.name;
																ChangeLibrary.CurrentLibrary = EditorVariables.OverlayLibraryObj;
																ChangeLibrary.Action = "";
							
														}
												}
												GUILayout.Space (5);
												GUI.color = Green;
												DKOverlayLibrary OverlayLibrary = EditorVariables.OverlayLibraryObj.GetComponent<DKOverlayLibrary> ();
												if (OverlayLibrary.overlayElementList.Contains ((Selection.activeObject as DKOverlayData)) == false) {
														if (GUILayout.Button ("Add to selected Library", GUILayout.ExpandWidth (true))) {
																List<DKOverlayData> OverlayLibraryL;
																OverlayLibraryL = OverlayLibrary.overlayElementList.ToList ();
																OverlayLibraryL.Add ((Selection.activeObject as DKOverlayData));
																OverlayLibrary.overlayElementList = OverlayLibraryL.ToArray ();
																EditorUtility.SetDirty (OverlayLibrary);
																AssetDatabase.SaveAssets ();
																OnSelectionChange ();
														}
												}
										} else
				// selected Race
				if (Selection.activeObject && Selection.activeObject.GetType ().ToString () == "DKRaceData") {
												DKRaceLibrary RaceLibrary = EditorVariables.RaceLibraryObj.GetComponent<DKRaceLibrary> ();
				
												GUI.color = Color.white;
												using (new Horizontal()) {
														GUI.color = Color.yellow;
														GUILayout.Label ("Race Library :", GUILayout.Width (75));
														GUI.color = Color.white;
														GUILayout.TextField (EditorVariables.RaceLibraryObj.name, GUILayout.ExpandWidth (true));
														if (GUILayout.Button ("Change", GUILayout.Width (60))) {
																OpenLibrariesWindow ();
																ChangeLibrary.CurrentLibN = EditorVariables.RaceLibraryObj.name;
																ChangeLibrary.CurrentLibrary = EditorVariables.RaceLibraryObj;
																ChangeLibrary.Action = "";
														}
												}

												GUILayout.Space (5);
												GUI.color = Green;
												if (RaceLibrary.raceElementList.Contains ((Selection.activeObject as DKRaceData)) == false) {
														if (GUILayout.Button ("Add to selected Library", GUILayout.ExpandWidth (true))) {
																List<DKRaceData> RaceLibraryL;
																RaceLibraryL = RaceLibrary.raceElementList.ToList ();
																RaceLibraryL.Add ((Selection.activeObject as DKRaceData));
																RaceLibrary.raceElementList = RaceLibraryL.ToArray ();
																EditorUtility.SetDirty (RaceLibrary);
																AssetDatabase.SaveAssets ();
																OnSelectionChange ();
														}
												}
										} else {
												if (!Selection.activeObject) {
														GUI.color = Green;
							if (EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd && EditorVariables.DK_DKUMACustomization 
																&& EditorVariables.SlotsAnatomyLibraryObj && EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length != 0
																&& GUILayout.Button ("Open the Browser", GUILayout.ExpandWidth (true))) {
																OpenBrowser ();
														}
														GUI.color = Color.white;
														GUILayout.TextField ("You can use the 'UMA to DK Converter' to select the UMA or DK elements from your assets. It will detect all of them in a click.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
														GUI.color = Green;
							if (EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd && EditorVariables.DK_DKUMACustomization 
																&& EditorVariables.SlotsAnatomyLibraryObj && EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length != 0
																&& GUILayout.Button ("UMA and DK elements", GUILayout.ExpandWidth (true))) {
																OpenAutoDetectWin ();
														}
														/*
						GUI.color = Color.white ;
						GUILayout.TextField("If an Element is selected from your Assets and this message is still displayed :" , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						GUILayout.TextField("Add the missing Script to your Selected Element." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					//	GUILayout.TextField("The scripts can be found in the Assets/DK_UMA_Editor/Scripts/ folder." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						GUI.color = Color.yellow ;	
						GUILayout.TextField("if your element is a slot, add the DKSlotData script." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						GUILayout.TextField("if your element is a Overlay, add the DKOverlayData script." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						GUILayout.TextField("if your element is a Race, add the DKRaceData script." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						GUI.color = Color.white ;
						GUILayout.TextField("Drag and drop the Script to the 'Missing ..' field." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					
						if ( GUILayout.Button ( "Video example", GUILayout.ExpandWidth (true))){
							Application.OpenURL ("http://www.youtube.com/playlist?list=PLz3lDsmTMvxZpbSXp79gRm3XOiZs31g5t");
						}
						*/
												}
										}
										#region Prepare
										if (EditorVariables.RaceLibraryObj == null || EditorVariables.DKSlotLibraryObj == null || EditorVariables.OverlayLibraryObj == null)
												DetectAndAddDK.AddAll ();
										else
				if (EditorVariables._RaceLibrary == null)
												DetectAndAddDK.DetectAll ();
										if (Selection.activeObject 
				    && 
										/*    (EditorVariables.RaceLibraryObj.GetComponent<DKRaceLibrary>().raceElementList.Contains((Selection.activeObject as DKRaceData)) == true 
				    || EditorVariables.DKSlotLibraryObj.GetComponent<DKSlotLibrary>().slotElementList.Contains((Selection.activeObject as DKSlotData)) == true 
				    || EditorVariables.OverlayLibraryObj.GetComponent<DKOverlayLibrary>().overlayElementList.Contains((Selection.activeObject as DKOverlayData)) == true )
*/
										//    && 
												(Selection.activeObject 
												&& (Selection.activeObject.GetType ().ToString () == "DKRaceData"
												|| Selection.activeObject.GetType ().ToString () == "DKSlotData"
												|| Selection.activeObject.GetType ().ToString () == "DKOverlayData")))
												using (new ScrollView(ref scroll)) {

														// Race Assign
														if (Selection.activeObject 
																&& Selection.activeObject.GetType ().ToString () == "DKRaceData") {
																for (int i = 0; i < EditorVariables._RaceLibrary.raceElementList.Length; i ++) {
																		if (EditorVariables._RaceLibrary.raceElementList [i] == Selection.activeObject) {
																				DKRaceData SelectedData = EditorVariables._RaceLibrary.raceElementList [i];
																				EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
																		}
																}
														}
														// Slot assign
														if (Selection.activeObject 
																&& Selection.activeObject.GetType ().ToString () == "DKSlotData") {
																for (int i = 0; i < EditorVariables._DKSlotLibrary.slotElementList.Length; i ++) {
																		if (EditorVariables._DKSlotLibrary.slotElementList [i] == Selection.activeObject) {
																				DKSlotData SelectedData = EditorVariables._DKSlotLibrary.slotElementList [i];
																				EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
									EditorVariables.SelectedElementOverlayType = SelectedData.OverlayType;
																		}
																}
														}
														// Overlay Assign
														if (Selection.activeObject 
																&& Selection.activeObject.GetType ().ToString () == "DKOverlayData") {
																for (int i = 0; i < EditorVariables._OverlayLibrary.overlayElementList.Length; i ++) {
																		if (EditorVariables._OverlayLibrary.overlayElementList [i] == Selection.activeObject) {
																				DKOverlayData SelectedData = EditorVariables._OverlayLibrary.overlayElementList [i];
																				EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
																		}
																}
														}
														#region Prepare Libraries
														#region Actions	
														GUILayout.Space (5);
														if (!choosePlace && !chooseOverlay && !chooseSlot)
																using (new Horizontal()) {	
																		GUI.color = Color.yellow;
																}
														if (Selection.activeObject 
																&& Selection.activeObject.GetType ().ToString () == "DKOverlayData") {
																GUI.color = Color.white;
																GUILayout.Label ("Textures", "toolbarbutton", GUILayout.ExpandWidth (true));

																if (Helper)
																		GUILayout.TextField ("The textures have to be 'Read/Write Enabled'.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

																for (int i1 = 0; i1 < (Selection.activeObject as DKOverlayData ).textureList.Length; i1 ++) {
																		var obj = (Selection.activeObject as DKOverlayData).textureList [i1];
																		string file = AssetDatabase.GetAssetPath (obj);
																		var importer = TextureImporter.GetAtPath (file) as TextureImporter;
																	if ( obj != null ){										
																		using (new Horizontal()) {
																				// launch readable
																			//	GUILayout.Label ("Texture :", GUILayout.Width (50));
																				GUI.color = Color.white;
																				GUILayout.TextField (obj.name, GUILayout.Width (150));
																	//	}
																	//	using (new Horizontal()) {
																			//	GUI.color = Color.yellow;
																			//	GUILayout.Label ("Path :", GUILayout.Width (35));
																				GUI.color = Color.white;
																				GUILayout.TextField (file, GUILayout.Width (150));
																				if (!importer.isReadable) {
																						GUI.color = Color.yellow;
																						if (GUILayout.Button ("Make readable", GUILayout.Width (80))) {
																								importer.isReadable = true;
																								AssetDatabase.ImportAsset (file);
																						}
																				} else
																				if (GUILayout.Button ("Go to", GUILayout.Width (50))) {
																						Selection.activeObject = obj;
																				}
																		}
																	}else
								{
									GUI.color = Red;
									GUILayout.Label ("The UMA element is not installed ! Install it to your project.", GUILayout.Width (350));

								}
																}
														}
														GUI.color = Color.white;
														using (new Horizontal()) {
																GUILayout.Label ("Configure the Element", "toolbarbutton", GUILayout.ExpandWidth (true));	
																GUILayout.Label (Selection.activeObject.GetType ().ToString (), "toolbarbutton", GUILayout.ExpandWidth (false));
														}
														using (new Horizontal()) {
																#region Element obj Name
																GUI.color = Color.white;
																//	GUILayout.Label("Prefab", GUILayout.Width (90));
							//	if ( EditorVariables.UMAObj == null ) 
																//	{
							//		UMAObjName = UMAObjDefault;
							//		EditorVariables.UMAObj =  GameObject.Find(UMAObjName);
																//	}
																//	SelectedPrefabName  = GUILayout.TextField(SelectedPrefabName, 50, GUILayout.Width (175));
																#endregion
																//	GUI.color = Red;
																//	if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
																//		OpenDeleteAsset();
																//	}
														}

														if (Selection.activeObject.GetType ().ToString () == "DKSlotData"
						   									|| Selection.activeObject.GetType ().ToString () == "DKOverlayData"
						    								|| Selection.activeObject.GetType ().ToString () == "DKRaceData")
													
						using (new Horizontal()) {
							if (Selection.activeObject.GetType ().ToString () == "DKSlotData" ) 
							using (new Vertical()) {
								Texture2D Preview = null;

								if (Preview == null ){
									string path = AssetDatabase.GetAssetPath ((Selection.activeObject as DKSlotData));
									path = path.Replace ((Selection.activeObject as DKSlotData).name+".asset", "");
									Preview = AssetDatabase.LoadAssetAtPath(path+"Preview-"+(Selection.activeObject as DKSlotData).name+".asset", typeof(Texture2D) ) as Texture2D;
								}
																

								if ( Preview != null ) GUI.color = Color.white ;
								else GUI.color = Red;
								if ( GUILayout.Button( Preview ,GUILayout.Width (70), GUILayout.Height (70))){
																
								
								}
							}
								using (new Vertical()) {
									using (new Horizontal()) {
										#region Element Name
										GUI.color = Color.white;
								//	GUILayout.Label ("Name", GUILayout.ExpandWidth (false));
									if (EditorVariables.UMAObj == null) {
										UMAObjName = EditorVariables.UMAObjDefault;
										EditorVariables.UMAObj = GameObject.Find (UMAObjName);
										}
									 EditorVariables.SelectedElementName = GUILayout.TextField ( EditorVariables.SelectedElementName, 50, GUILayout.Width (120));
									//	GUILayout.Label (Selection.activeObject.GetType ().ToString (), Slim, GUILayout.ExpandWidth (true));
									GUILayout.Label ("UMA", GUILayout.ExpandWidth (false));						
									 EditorVariables.SelectedElementName = GUILayout.TextField ( EditorVariables.SelectedElementName, 50, GUILayout.Width (120));

								}

																using (new Horizontal()) {
																	if (Selection.activeObject.GetType ().ToString () == "DKRaceData") {
																			GUILayout.Label ("Race :", GUILayout.Width (90));
																			_Name = GUILayout.TextField (_Name, 50, GUILayout.Width (150));
																	}
																	#endregion

																	#region Wear Mesh replace Skin Mesh ?

																}
																#endregion	
																#region Element Race
																
								GUI.color = Color.yellow;

								if (Selection.activeObject 
									&& !choosePlace && !chooseOverlay && !chooseSlot 
								    && (Selection.activeObject.GetType ().ToString () == "DKSlotData" 
								    || Selection.activeObject.GetType ().ToString () == "DKOverlayData" )
								    && !EditorVariables.AutoDetLib)
								{
									using (new Horizontal()) {
										GUILayout.TextField ("Don't forget to set the Races", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

															//	if (Selection.activeObject 
															//		&& !choosePlace && !chooseOverlay && !chooseSlot) 
																	
										GUI.color = Color.white;
										if (Selection.activeObject.GetType ().ToString () != "DKRaceData"){
																	//		GUILayout.Label ("Element's Races :", GUILayout.Width (100));
											if (EditorVariables.UMAObj == null) {
												UMAObjName = EditorVariables.UMAObjDefault;
												EditorVariables.UMAObj = GameObject.Find (UMAObjName);
											}
											if (EditorVariables.SelectedElementObj == Selection.activeGameObject) {
												if (Selection.activeObject.GetType ().ToString () != "DKRaceData" && GUILayout.Button ("Open Races List", GUILayout.ExpandWidth (true))) {
													OpenRaceSelectEditor ();
												}
											}
										}
									}
								}
																#endregion
																#region Gender
																GUI.color = Color.yellow;
																if (!EditorVariables.AutoDetLib && EditorVariables.SelectedElementObj == Selection.activeGameObject && Helper && !choosePlace && !chooseOverlay && !chooseSlot)
																		GUILayout.TextField ("You can specify a Gender or let it be usable by both.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
																if (!EditorVariables.AutoDetLib && EditorVariables.SelectedElementObj == Selection.activeGameObject && !choosePlace && !chooseOverlay && !chooseSlot)
																using (new Horizontal()) {
									GUILayout.Label ("Gender", GUILayout.Width (60));

																	if (EditorVariables.SelectedElementGender == "Both")
																	GUI.color = Green;
																	else
																	GUI.color = Color.gray;
																	if (GUILayout.Button ("Both", GUILayout.ExpandWidth (true))) {
																		EditorVariables.SelectedElementGender = "Both";
																		DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																		if (SelectedSlotElement != null) {
																			SelectedSlotElement.Gender = EditorVariables.SelectedElementGender;
																		}
																		DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																		if (SelectedOverlayElement != null) {
																			SelectedOverlayElement.Gender = EditorVariables.SelectedElementGender;
																		}
																		EditorUtility.SetDirty (Selection.activeObject);
																		AssetDatabase.SaveAssets ();
																	}
																	if (EditorVariables.SelectedElementGender == "Female")
																	GUI.color = Green;
																	else
																	GUI.color = Color.gray;
																	if (GUILayout.Button ("Female", GUILayout.ExpandWidth (true))) {
																		EditorVariables.SelectedElementGender = "Female";
																		DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																		if (SelectedSlotElement != null) {
																			SelectedSlotElement.Gender = EditorVariables.SelectedElementGender;
																		}
																		DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																		if (SelectedOverlayElement != null) {
																			SelectedOverlayElement.Gender = EditorVariables.SelectedElementGender;
																		}
																		EditorUtility.SetDirty (Selection.activeObject);
																		AssetDatabase.SaveAssets ();
																	}
																	if (EditorVariables.SelectedElementGender == "Male")
																	GUI.color = Green;
																	else
																	GUI.color = Color.gray;
																	if (GUILayout.Button ("Male", GUILayout.ExpandWidth (true))) {
																		EditorVariables.SelectedElementGender = "Male";
																		DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																		if (SelectedSlotElement != null) {
																			SelectedSlotElement.Gender = EditorVariables.SelectedElementGender;
																		}
																		DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																		if (SelectedOverlayElement != null) {
																			SelectedOverlayElement.Gender = EditorVariables.SelectedElementGender;
																		}
																		EditorUtility.SetDirty (Selection.activeObject);
																		AssetDatabase.SaveAssets ();
																	}
																}
															}
														}
														#endregion
														#region Race Only
														EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
														if (Selection.activeObject && !choosePlace && !chooseOverlay && !chooseSlot
																&& Selection.activeObject.GetType ().ToString () == "DKRaceData"
																&& !EditorVariables.AutoDetLib) {
																GUILayout.Space (10);
																GUI.color = Color.white;
																if (Helper)
																		GUILayout.TextField ("You can set any DNA aspect for your avatars, it is controlled by the DKRaceData.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
																if (Helper)
																		GUILayout.TextField ("It is composed by 2 parts, the Race limitations and the Bones, the bones are including the DNA behaviour for you to choose how it will manipulate the aspect of your avatar.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
																if (Helper)
																		GUILayout.TextField ("Tip : The race limitations can be set directly when you are modifying an avatar.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
																if (Helper)
																		GUILayout.TextField ("Also Color Presets can be created and be applied to the races.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

																if (GUILayout.Button ("Open Race Editor", GUILayout.ExpandWidth (true))) {
																		OpenRaceEditor ();
																}				
														}
														#endregion Race Only
														#region Overlay Type
														GUI.color = Color.white;
														if ( Selection.activeObject.GetType ().ToString () != "DKRaceData" ) 
															GUILayout.Label ("Overlay Type", "toolbarbutton", GUILayout.ExpandWidth (true));
														GUI.color = Color.yellow;
														EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;

						if (!LinkedOverlayList ){
							if ( Selection.activeObject && !choosePlace && !chooseOverlay && !chooseSlot
																&& (Selection.activeObject.GetType ().ToString () == "DKOverlayData"
																|| Selection.activeObject.GetType ().ToString () == "DKSlotData") 
							    && !EditorVariables.AutoDetLib && Helper ) { 
																GUILayout.TextField ("The Overlay Type is used by the Generator during the Model's creation. " +
																		"All the 'Naked body parts' must be of the 'Flesh' Type, the Head slot must be of the 'Face' type.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
																if (Selection.activeObject.GetType ().ToString () == "DKOverlayData")
																		GUILayout.TextField ("About Hair : You can use hair modules, it has to be of the 'Hair+Element' Overlay type and its place is 'Hair'. It can be usefull to assign it to the corresponding Slot.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
																if (Selection.activeObject.GetType ().ToString () == "DKOverlayData")
																		GUILayout.TextField ("About Beard : If your Overlay is a Beard, select the Beard Overlay type and the place Head. The pilosity is settable during the creation.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
																if (Selection.activeObject.GetType ().ToString () == "DKOverlayData")
																		GUILayout.TextField ("About Eyebrow : If your Overlay is an Eyebrow, select the Hair Overlay type and the place Head.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

														}
						if (!LinkedOverlayList && Selection.activeObject && !choosePlace && !chooseOverlay && !chooseSlot
																&& Selection.activeObject.GetType ().ToString () == "DKSlotData" 
																&& !EditorVariables.AutoDetLib) {
																if (Helper) {
																		GUILayout.TextField ("Please remember : The head is composed by a single 'Face' slot (of the Anatomy part 'Head') and multiple 'Face+Element' slots, such as the Mouth or the Eyelid (of Anatomy part eyelid, not Eye).", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
																		GUILayout.TextField ("About Hair : You can use hair modules, it has to be of the 'Hair+Element' Overlay type and its place is 'Hair_Module'. It can be usefull to assign it to the corresponding Overlay.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
																		GUILayout.TextField ("About Beard : If your slot is a Beard, you will have to create a new Anatomy part.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
																}
														}
														if (Selection.activeObject && !choosePlace && !chooseOverlay && !chooseSlot
																&& (Selection.activeObject.GetType ().ToString () == "DKOverlayData"
																|| Selection.activeObject.GetType ().ToString () == "DKSlotData")
																&& !EditorVariables.AutoDetLib) 
																using (new Horizontal()) {
																		GUI.color = Color.white;
																		GUILayout.Label ("Body", GUILayout.ExpandWidth (false));
																		if (EditorVariables.SelectedElementOverlayType == "Flesh")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Flesh", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementOverlayType = "Flesh";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
										SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
								EditorUtility.SetDirty (Selection.activeObject);
								AssetDatabase.SaveAssets ();
																		}
																		if (EditorVariables.SelectedElementOverlayType == "Face")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Face", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementOverlayType = "Face";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
										SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
								EditorUtility.SetDirty (Selection.activeObject);
								AssetDatabase.SaveAssets ();
																		}
																		if (EditorVariables.SelectedElementOverlayType == "Hair")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Hair", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementOverlayType = "Hair";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
										SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
								EditorUtility.SetDirty (Selection.activeObject);
								AssetDatabase.SaveAssets ();
																		}
								if ((Selection.activeObject.GetType ().ToString () == "DKSlotData" && (Selection.activeObject as DKSlotData).Elem == true) 
																				|| (Selection.activeObject.GetType ().ToString () == "DKOverlayData" && (Selection.activeObject as DKOverlayData).Elem == true))
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("+ Elem", GUILayout.ExpandWidth (true))) {
									if (Selection.activeObject.GetType ().ToString () == "DKSlotData" && (Selection.activeObject as DKSlotData).Elem == false)
										(Selection.activeObject as DKSlotData).Elem = true;
																				else 
								if (Selection.activeObject.GetType ().ToString () == "DKSlotData")
											(Selection.activeObject as DKSlotData).Elem = false;
																				else
							if (Selection.activeObject.GetType ().ToString () == "DKOverlayData" && (Selection.activeObject as DKOverlayData).Elem == false)
																						(Selection.activeObject as DKOverlayData).Elem = true;
																				else 
							if (Selection.activeObject.GetType ().ToString () == "DKOverlayData")
																						(Selection.activeObject as DKOverlayData).Elem = false;
								EditorUtility.SetDirty (Selection.activeObject);
								AssetDatabase.SaveAssets ();
																		}
																		if (EditorVariables.SelectedElementOverlayType == "Eyes")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Eyes", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementOverlayType = "Eyes";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
										SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
								EditorUtility.SetDirty (Selection.activeObject);
								AssetDatabase.SaveAssets ();
																		}

																}
														if (Selection.activeObject && !choosePlace && !chooseOverlay && !chooseSlot
																&& (Selection.activeObject.GetType ().ToString () == "DKOverlayData"
																|| Selection.activeObject.GetType ().ToString () == "DKSlotData")
																&& !EditorVariables.AutoDetLib) 
					


																using (new Horizontal()) {
																		if (EditorVariables.SelectedElementOverlayType == "Eyebrow")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Eyebrow", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementOverlayType = "Eyebrow";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
										SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
								EditorUtility.SetDirty (Selection.activeObject);
								AssetDatabase.SaveAssets ();
																		}
																		if (EditorVariables.SelectedElementOverlayType == "Lips")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Lip", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementOverlayType = "Lips";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
										SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
								EditorUtility.SetDirty (Selection.activeObject);
								AssetDatabase.SaveAssets ();
																		}

																		if (EditorVariables.SelectedElementOverlayType == "Makeup")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Makeup", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementOverlayType = "Makeup";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
										SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
								EditorUtility.SetDirty (Selection.activeObject);
								AssetDatabase.SaveAssets ();
																		}

																		if (EditorVariables.SelectedElementOverlayType == "Tatoo")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Tatoo", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementOverlayType = "Tatoo";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
										SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
								EditorUtility.SetDirty (Selection.activeObject);
								AssetDatabase.SaveAssets ();
																		}

																		if (EditorVariables.SelectedElementOverlayType == "Beard")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Beard", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementOverlayType = "Beard";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
										SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
								EditorUtility.SetDirty (Selection.activeObject);
								AssetDatabase.SaveAssets ();
																		}
																}

														if (Selection.activeObject && !choosePlace && !chooseOverlay && !chooseSlot
																&& (Selection.activeObject.GetType ().ToString () == "DKOverlayData" 
																|| Selection.activeObject.GetType ().ToString () == "DKSlotData")
																&& !EditorVariables.AutoDetLib) {
																if (Helper && Selection.activeObject.GetType ().ToString () == "DKSlotData") 
																		GUILayout.TextField ("About Wear : If your slot is a Wear, select the corresponding Overlay type and Anatomy part (ex.: Torso and TorsoWear for a T-shirt)." +
																				"Then assign the correct Overlay to it ( ex.: FemaleTshirt01 Overlay for the FemaleTshirt01 slot).", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
																if (Helper && Selection.activeObject.GetType ().ToString () == "DKOverlayData") { 
																		GUILayout.TextField ("About Wear : If your Overlay is associated to a slot, verify to set the overlay exaclty the same than the slot, with the same weight (ex.: Torso and TorsoWear for a T-shirt, weight Light)." +
																				"The name of the associated slot is displayed below.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
																		GUILayout.TextField ("Overlay only Wear : If your Overlay has been created to be applied without an associated slot, it is possible that the leg overlay has to be applied on the torso Anatomy part, depending on your overlay creation." +
																				"Try to tweek it if you encounter a bad overlay placement.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
																}

																using (new Horizontal()) {
																		GUI.color = Color.white;
																		GUILayout.Label ("Wears :", GUILayout.ExpandWidth (false));
																		if (EditorVariables.SelectedElementOverlayType == "TorsoWear")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Torso", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementOverlayType = "TorsoWear";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
											SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
									EditorUtility.SetDirty (Selection.activeObject);
									AssetDatabase.SaveAssets ();
																		}
																		if (EditorVariables.SelectedElementOverlayType == "ShoulderWear")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Shoulder", GUILayout.ExpandWidth (true))) {
										EditorVariables.SelectedElementOverlayType = "ShoulderWear";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
											SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
									EditorUtility.SetDirty (Selection.activeObject);
									AssetDatabase.SaveAssets ();
									}
									if (EditorVariables.SelectedElementOverlayType == "BeltWear") GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("Belt", GUILayout.ExpandWidth (true))) {
										EditorVariables.SelectedElementOverlayType = "BeltWear";
										DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
										if (SelectedSlotElement != null) {
											SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
										}
										DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
										if (SelectedOverlayElement != null) {
											SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
										}
										EditorUtility.SetDirty (Selection.activeObject);
										AssetDatabase.SaveAssets ();
									}
									if (EditorVariables.SelectedElementOverlayType == "CloakWear") GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("Cloak", GUILayout.ExpandWidth (true))) {
										EditorVariables.SelectedElementOverlayType = "CloakWear";
										DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
										if (SelectedSlotElement != null) {
											SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
										}
										DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
										if (SelectedOverlayElement != null) {
											SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
										}
										EditorUtility.SetDirty (Selection.activeObject);
										AssetDatabase.SaveAssets ();
									}
									if (EditorVariables.SelectedElementOverlayType == "LegsWear")GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("Leg", GUILayout.ExpandWidth (true))) {
											EditorVariables.SelectedElementOverlayType = "LegsWear";
											DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
											if (SelectedSlotElement != null) {
													SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
											}
											DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
											if (SelectedOverlayElement != null) {
													SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
											}
									EditorUtility.SetDirty (Selection.activeObject);
									AssetDatabase.SaveAssets ();
																		}
																		if (EditorVariables.SelectedElementOverlayType == "FeetWear")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Feet", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementOverlayType = "FeetWear";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
																						SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
									EditorUtility.SetDirty (Selection.activeObject);
									AssetDatabase.SaveAssets ();
																		}
																}
														}
														if (Selection.activeObject && !choosePlace && !chooseOverlay && !chooseSlot
																&& (Selection.activeObject.GetType ().ToString () == "DKOverlayData"
																|| Selection.activeObject.GetType ().ToString () == "DKSlotData")
																&& !EditorVariables.AutoDetLib) 
																using (new Horizontal()) {
																		if (EditorVariables.SelectedElementOverlayType == "HandsWear")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Hand", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementOverlayType = "HandsWear";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
																						SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
								EditorUtility.SetDirty (Selection.activeObject);
								AssetDatabase.SaveAssets ();
								}
								if (EditorVariables.SelectedElementOverlayType == "HeadWear") GUI.color = Green;
								else GUI.color = Color.gray;
								if (GUILayout.Button ("Head", GUILayout.ExpandWidth (true))) {
										EditorVariables.SelectedElementOverlayType = "HeadWear";
										DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
										if (SelectedSlotElement != null) {
												SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
										}
										DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
										if (SelectedOverlayElement != null) {
												SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
										}
									EditorUtility.SetDirty (Selection.activeObject);
									AssetDatabase.SaveAssets ();
								}
								if (EditorVariables.SelectedElementOverlayType == "ArmbandWear") GUI.color = Green;
								else GUI.color = Color.gray;
								if (GUILayout.Button ("Armband", GUILayout.ExpandWidth (true))) {
									EditorVariables.SelectedElementOverlayType = "ArmbandWear";
									DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
									if (SelectedSlotElement != null) {
										SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
									}
									DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
									if (SelectedOverlayElement != null) {
										SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
									}
									
									EditorUtility.SetDirty (Selection.activeObject);
									AssetDatabase.SaveAssets ();
								}
								if (EditorVariables.SelectedElementOverlayType == "WristWear") GUI.color = Green;
								else GUI.color = Color.gray;
								if (GUILayout.Button ("Wrist", GUILayout.ExpandWidth (true))) {
									EditorVariables.SelectedElementOverlayType = "WristWear";
									DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
									if (SelectedSlotElement != null) {
										SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
									}
									DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
									if (SelectedOverlayElement != null) {
										SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
									}
									
									EditorUtility.SetDirty (Selection.activeObject);
									AssetDatabase.SaveAssets ();
								}
								if (EditorVariables.SelectedElementOverlayType == "Underwear") GUI.color = Green;
								else GUI.color = Color.gray;
								if (GUILayout.Button ("Underwear", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementOverlayType = "Underwear";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
																						SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																			EditorUtility.SetDirty (Selection.activeObject);
																			AssetDatabase.SaveAssets ();
																		}
																		if (EditorVariables.SelectedElementOverlayType == "")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("None", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementOverlayType = "";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
																						SelectedSlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
																				}
																			EditorUtility.SetDirty (Selection.activeObject);
																			AssetDatabase.SaveAssets ();
																		}
																}
			

														GUI.color = Color.white;
														if (Selection.activeObject
																&& ((Selection.activeObject.GetType ().ToString () == "DKSlotData"
																&& (Selection.activeObject as DKSlotData).OverlayType != null && (Selection.activeObject as DKSlotData).OverlayType.ToLower ().Contains ("wear") == true)
																|| (Selection.activeObject.GetType ().ToString () == "DKOverlayData"
																&& (Selection.activeObject as DKOverlayData).OverlayType != null && (Selection.activeObject as DKOverlayData).OverlayType.ToLower ().Contains ("wear") == true))
																&& !EditorVariables.AutoDetLib)  
																using (new Horizontal()) {
																		GUILayout.Label ("Weight", GUILayout.ExpandWidth (false));
																		if (EditorVariables.SelectedElementWearWeight == "Light")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Light", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementWearWeight = "Light";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
										SelectedSlotElement.WearWeight = EditorVariables.SelectedElementWearWeight;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.WearWeight = EditorVariables.SelectedElementWearWeight;
																				}
																			EditorUtility.SetDirty (Selection.activeObject);
																			AssetDatabase.SaveAssets ();
																		}
																		if (EditorVariables.SelectedElementWearWeight == "Medium")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Medium", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementWearWeight = "Medium";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
										SelectedSlotElement.WearWeight = EditorVariables.SelectedElementWearWeight;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.WearWeight = EditorVariables.SelectedElementWearWeight;
																				}
																			EditorUtility.SetDirty (Selection.activeObject);
																			AssetDatabase.SaveAssets ();
																		}
																		if (EditorVariables.SelectedElementWearWeight == "High")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("High", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementWearWeight = "High";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
																						SelectedSlotElement.WearWeight = EditorVariables.SelectedElementWearWeight;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.WearWeight = EditorVariables.SelectedElementWearWeight;
																				}
																			EditorUtility.SetDirty (Selection.activeObject);
																			AssetDatabase.SaveAssets ();
																		}
																		if (EditorVariables.SelectedElementWearWeight == "Heavy")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("Heavy", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementWearWeight = "Heavy";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
																						SelectedSlotElement.WearWeight = EditorVariables.SelectedElementWearWeight;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.WearWeight = EditorVariables.SelectedElementWearWeight;
																				}
																			EditorUtility.SetDirty (Selection.activeObject);
																			AssetDatabase.SaveAssets ();
																		}
																		if (EditorVariables.SelectedElementWearWeight == "")
																				GUI.color = Green;
																		else
																				GUI.color = Color.gray;
																		if (GUILayout.Button ("No", GUILayout.ExpandWidth (true))) {
																				EditorVariables.SelectedElementWearWeight = "";
																				DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
																				if (SelectedSlotElement != null) {
																						SelectedSlotElement.WearWeight = EditorVariables.SelectedElementWearWeight;
																				}
																				DKOverlayData SelectedOverlayElement = Selection.activeObject as DKOverlayData;
																				if (SelectedOverlayElement != null) {
																						SelectedOverlayElement.WearWeight = EditorVariables.SelectedElementWearWeight;
																				}
																			EditorUtility.SetDirty (Selection.activeObject);
																			AssetDatabase.SaveAssets ();
																		}
																}
														#endregion


														
						if (Selection.activeObject.GetType ().ToString () != "DKRaceData" 
						    && Selection.activeObject.GetType ().ToString () != "DKOverlayData") 
						{ 
							if (Helper)
								GUILayout.TextField ("In some cases you will need to delete a flesh part of your avatar to replace it by a wear slot (ex.: to replace the feet when generating some boots).", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

							using (new Horizontal()) {
								if (EditorVariables.Replace == true) GUI.color = Color.cyan;
								else GUI.color = Color.gray;
									
								DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
								if ( SelectedSlotElement.OverlayType.Contains("Wear") && GUILayout.Button ("Replace the flesh anatomy part by this one", GUILayout.ExpandWidth (true))) {
								
									if (EditorVariables.Replace == true) EditorVariables.Replace = false;
									else EditorVariables.Replace = true;
									if (SelectedSlotElement != null) {
										SelectedSlotElement.Replace = EditorVariables.Replace;
									}

									EditorUtility.SetDirty (SelectedSlotElement);
									AssetDatabase.SaveAssets ();
								}

							}

							if (EditorVariables.SelectedElementOverlayType == "HeadWear")
							using (new Horizontal()) {
								GUILayout.Label ("Hide", GUILayout.ExpandWidth (false));

								DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
									if ( SelectedSlotElement._HideData.HideHair ) GUI.color = Green;
								else GUI.color = Color.gray;
								if (GUILayout.Button ("Hair", GUILayout.ExpandWidth (true))) {
										if (SelectedSlotElement._HideData.HideHair == true) SelectedSlotElement._HideData.HideHair = false;
										else SelectedSlotElement._HideData.HideHair = true;
									EditorUtility.SetDirty (SelectedSlotElement);
									AssetDatabase.SaveAssets ();
								}
									if ( SelectedSlotElement._HideData.HideHairModule ) GUI.color = Green;
								else GUI.color = Color.gray;
								if (GUILayout.Button ("Hair Module", GUILayout.ExpandWidth (true))) {
										if (SelectedSlotElement._HideData.HideHairModule == true) SelectedSlotElement._HideData.HideHairModule = false;
										else SelectedSlotElement._HideData.HideHairModule = true;
									EditorUtility.SetDirty (SelectedSlotElement);
									AssetDatabase.SaveAssets ();
								}
									if ( SelectedSlotElement._HideData.HideEars ) GUI.color = Green;
								else GUI.color = Color.gray;
								if (GUILayout.Button ("Ears", GUILayout.ExpandWidth (true))) {
										if (SelectedSlotElement._HideData.HideEars == true) SelectedSlotElement._HideData.HideEars = false;
										else SelectedSlotElement._HideData.HideEars = true;
									EditorUtility.SetDirty (SelectedSlotElement);
									AssetDatabase.SaveAssets ();
								}
									if ( SelectedSlotElement._HideData.HideMouth ) GUI.color = Green;
								else GUI.color = Color.gray;
								if (GUILayout.Button ("Mouth", GUILayout.ExpandWidth (true))) {
										if (SelectedSlotElement._HideData.HideMouth == true) SelectedSlotElement._HideData.HideMouth = false;
										else SelectedSlotElement._HideData.HideMouth = true;
									EditorUtility.SetDirty (SelectedSlotElement);
									AssetDatabase.SaveAssets ();
								}
								if ( SelectedSlotElement._HideData.HideBeard ) GUI.color = Green;
								else GUI.color = Color.gray;
								if (GUILayout.Button ("Beard", GUILayout.ExpandWidth (true))) {
										if (SelectedSlotElement._HideData.HideBeard == true) SelectedSlotElement._HideData.HideBeard = false;
										else SelectedSlotElement._HideData.HideBeard = true;
									EditorUtility.SetDirty (SelectedSlotElement);
									AssetDatabase.SaveAssets ();
								}
								if ( SelectedSlotElement._HideData.HideCollar ) GUI.color = Green;
								else GUI.color = Color.gray;
								if (GUILayout.Button ("Collar", GUILayout.ExpandWidth (true))) {
									if (SelectedSlotElement._HideData.HideCollar == true) SelectedSlotElement._HideData.HideCollar = false;
									else SelectedSlotElement._HideData.HideCollar = true;
									EditorUtility.SetDirty (SelectedSlotElement);
									AssetDatabase.SaveAssets ();
								}
							}
							
								if (EditorVariables.SelectedElementOverlayType == "TorsoWear")
								using (new Horizontal()) {
									GUILayout.Label ("Hide", GUILayout.ExpandWidth (false));
									DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
										if ( SelectedSlotElement._HideData.HideShoulders ) GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("Shoulders", GUILayout.ExpandWidth (true))) {
											if (SelectedSlotElement._HideData.HideShoulders == true) SelectedSlotElement._HideData.HideShoulders = false;
											else SelectedSlotElement._HideData.HideShoulders = true;
										EditorUtility.SetDirty (SelectedSlotElement);
										AssetDatabase.SaveAssets ();
									}
										if ( SelectedSlotElement._HideData.HideLegs ) GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("Legs", GUILayout.ExpandWidth (true))) {
											if (SelectedSlotElement._HideData.HideLegs == true) SelectedSlotElement._HideData.HideLegs = false;
											else SelectedSlotElement._HideData.HideLegs = true;
										EditorUtility.SetDirty (SelectedSlotElement);
										AssetDatabase.SaveAssets ();
									}
										if ( SelectedSlotElement._HideData.HideBelt ) GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("Belt", GUILayout.ExpandWidth (true))) {
											if (SelectedSlotElement._HideData.HideBelt == true) SelectedSlotElement._HideData.HideBelt = false;
											else SelectedSlotElement._HideData.HideBelt = true;
										EditorUtility.SetDirty (SelectedSlotElement);
										AssetDatabase.SaveAssets ();
									}
										if ( SelectedSlotElement._HideData.HideArmBand ) GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("ArmBand", GUILayout.ExpandWidth (true))) {
											if (SelectedSlotElement._HideData.HideArmBand == true) SelectedSlotElement._HideData.HideArmBand = false;
											else SelectedSlotElement._HideData.HideArmBand = true;
										EditorUtility.SetDirty (SelectedSlotElement);
										AssetDatabase.SaveAssets ();
									}
									if ( SelectedSlotElement._HideData.HideWrist ) GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("Wrist", GUILayout.ExpandWidth (true))) {
										if (SelectedSlotElement._HideData.HideWrist == true) SelectedSlotElement._HideData.HideWrist = false;
										else SelectedSlotElement._HideData.HideWrist = true;
										EditorUtility.SetDirty (SelectedSlotElement);
										AssetDatabase.SaveAssets ();
									}
									if ( SelectedSlotElement._HideData.HideCollar ) GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("Collar", GUILayout.ExpandWidth (true))) {
										if (SelectedSlotElement._HideData.HideCollar == true) SelectedSlotElement._HideData.HideCollar = false;
										else SelectedSlotElement._HideData.HideCollar = true;
										EditorUtility.SetDirty (SelectedSlotElement);
										AssetDatabase.SaveAssets ();
									}
								}
								if (EditorVariables.SelectedElementOverlayType == "LegsWear")
								using (new Horizontal()) {
									GUILayout.Label ("Hide", GUILayout.ExpandWidth (false));
									DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
									if ( SelectedSlotElement._HideData.HideBelt ) GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("Belt", GUILayout.ExpandWidth (true))) {
											if (SelectedSlotElement._HideData.HideBelt == true) SelectedSlotElement._HideData.HideBelt = false;
											else SelectedSlotElement._HideData.HideBelt = true;
										EditorUtility.SetDirty (SelectedSlotElement);
										AssetDatabase.SaveAssets ();
									}
									if ( SelectedSlotElement._HideData.HideLegBand ) GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("Leg band", GUILayout.ExpandWidth (true))) {
										if (SelectedSlotElement._HideData.HideLegBand == true) SelectedSlotElement._HideData.HideLegBand = false;
										else SelectedSlotElement._HideData.HideLegBand = true;
										EditorUtility.SetDirty (SelectedSlotElement);
										AssetDatabase.SaveAssets ();
									}
									if ( SelectedSlotElement._HideData.HideUnderwear ) GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("Underwear", GUILayout.ExpandWidth (true))) {
										if (SelectedSlotElement._HideData.HideUnderwear == true) SelectedSlotElement._HideData.HideUnderwear = false;
										else SelectedSlotElement._HideData.HideUnderwear = true;
										EditorUtility.SetDirty (SelectedSlotElement);
										AssetDatabase.SaveAssets ();
									}
								}
								if (EditorVariables.SelectedElementOverlayType == "HandsWear")
								using (new Horizontal()) {
									GUILayout.Label ("Hide", GUILayout.ExpandWidth (false));
									
									DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
									if ( SelectedSlotElement._HideData.HideRing ) GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("Rings", GUILayout.ExpandWidth (true))) {
										if (SelectedSlotElement._HideData.HideRing == true) SelectedSlotElement._HideData.HideRing = false;
										else SelectedSlotElement._HideData.HideRing = true;
										EditorUtility.SetDirty (SelectedSlotElement);
										AssetDatabase.SaveAssets ();
									}
									if ( SelectedSlotElement._HideData.HideWrist ) GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("Wrist", GUILayout.ExpandWidth (true))) {
										if (SelectedSlotElement._HideData.HideWrist == true) SelectedSlotElement._HideData.HideWrist = false;
										else SelectedSlotElement._HideData.HideWrist = true;
										EditorUtility.SetDirty (SelectedSlotElement);
										AssetDatabase.SaveAssets ();
									}
								}
							}
							#region Legacy Menu
							if ( Selection.activeObject && !EditorVariables.AutoDetLib 
							    && Selection.activeObject.GetType ().ToString () == "DKSlotData") {
								if (Helper) {
									GUILayout.Space (5);
									GUI.color = Color.yellow;
									GUILayout.TextField ("The Legacy slot(s) is generated automatically with this slot." +
										"It is usefull in case that the current slot is a EditorVariables.Replace slot, such as a Tshirt with Legacy arms and torso.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
								//	GUILayout.TextField ("The Legacy slot has no Place and can remove another slot." +
								//	                     "It needs an Overlay Type and a Linked Overlay.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

								}

								DKSlotData SelectedSlotElement = Selection.activeObject as DKSlotData;
								using (new Horizontal()) {
									if ( SelectedSlotElement._LegacyData.IsLegacy == false ){
										if ( SelectedSlotElement._LegacyData.HasLegacy == true ) GUI.color = Green;
										else GUI.color = Color.gray;
										if (GUILayout.Button ("Uses Legacy slot(s)", GUILayout.ExpandWidth (true))) {
											if ( SelectedSlotElement._LegacyData.HasLegacy == true ) SelectedSlotElement._LegacyData.HasLegacy = false;
											else SelectedSlotElement._LegacyData.HasLegacy = true;
										}
									}
									if ( SelectedSlotElement._LegacyData.HasLegacy == false ){
										if ( SelectedSlotElement._LegacyData.IsLegacy == true ) GUI.color = Green;
										else GUI.color = Color.gray;
										if (GUILayout.Button ("Is a Legacy slot", GUILayout.ExpandWidth (true))) {
											if ( SelectedSlotElement._LegacyData.IsLegacy == true ) SelectedSlotElement._LegacyData.IsLegacy = false;
											else SelectedSlotElement._LegacyData.IsLegacy = true;
										}
									}
								}

								if ( SelectedSlotElement._LegacyData.HasLegacy == true )
								using (new Horizontal()) {
									GUI.color = Color.white;
									GUILayout.Label ("Legacy", GUILayout.ExpandWidth (false));
									if (SelectedSlotElement._LegacyData.LegacyList.Count == 1)
										GUILayout.Label (SelectedSlotElement._LegacyData.LegacyList [0].name, GUILayout.ExpandWidth (true));
									else if (SelectedSlotElement._LegacyData.LegacyList.Count > 0) {
										GUI.color = Green;
										GUILayout.Label ("Multiple", GUILayout.ExpandWidth (true));
										if (SelectedSlotElement._LegacyData.LegacyList.Count < 2)
											GUI.color = Green;
										else
											GUI.color = Color.white;
									//	if (!chooseOverlay && GUILayout.Button ("List", GUILayout.ExpandWidth (false))) {
										//	if (SelectedSlotElement._LegacyData.LegacyList)
										//		SelectedSlotElement._LegacyData.LegacyList = false;
										//	else
										//		SelectedSlotElement._LegacyData.LegacyList = true;
									//	}
									} else {
										GUI.color = Color.cyan;
										GUILayout.Label ("Choose ---->", GUILayout.ExpandWidth (true));
									}
									GUI.color = Color.cyan;
									if (EditorVariables.SelectedElementObj == Selection.activeGameObject && !choosePlace && !chooseOverlay && !chooseSlot
									    && GUILayout.Button ("Choose", GUILayout.ExpandWidth (false))) {
										OpenChooseSlotWin ();
									}
								}
								if ( SelectedSlotElement._LegacyData.IsLegacy == true ) {
									using (new Horizontal()) {
										GUI.color = Color.white;
										GUILayout.Label ("Elder(s)", GUILayout.ExpandWidth (false));
										if (SelectedSlotElement._LegacyData.ElderList.Count == 1)
											GUILayout.Label (SelectedSlotElement._LegacyData.ElderList [0].name, GUILayout.ExpandWidth (true));
										else if (SelectedSlotElement._LegacyData.ElderList.Count > 0) {
											GUI.color = Green;
											GUILayout.Label ("Multiple", GUILayout.ExpandWidth (true));
											if (SelectedSlotElement._LegacyData.ElderList.Count < 2) GUI.color = Green;
											else GUI.color = Color.white;
										} 
										else {
											GUI.color = Color.cyan;
											GUILayout.Label ("Choose ---->", GUILayout.ExpandWidth (true));
										}
										GUI.color = Color.cyan;
										if (EditorVariables.SelectedElementObj == Selection.activeGameObject && !choosePlace && !chooseOverlay && !chooseSlot
										    && GUILayout.Button ("Choose", GUILayout.ExpandWidth (false))) {
											OpenChooseSlotWin ();
										}
									}
									if ( SelectedSlotElement._LegacyData.Replace == true ) GUI.color = Green;
									else GUI.color = Color.gray;
									if (GUILayout.Button ("Replace the Anatomy Part (Place)", GUILayout.ExpandWidth (true))) {
										if ( SelectedSlotElement._LegacyData.Replace == true ) SelectedSlotElement._LegacyData.Replace = false;
										else SelectedSlotElement._LegacyData.Replace = true;
									}
								}
							}
							#endregion Legacy Menu
						
							#region choose Place button
								if (Selection.activeObject && !EditorVariables.AutoDetLib 
									&& Selection.activeObject.GetType ().ToString () != "DKRaceData") {
									GUI.color = Color.yellow;
									if (EditorVariables.SelectedElementObj == Selection.activeGameObject && Helper && !choosePlace && !chooseOverlay && !chooseSlot)
										GUILayout.TextField ("You can change the Element's place used during the model's generation. The place is very important, an Element without a place will not be generated.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
									GUI.color = Green;
														
								if (EditorVariables.SelectedElementObj == Selection.activeGameObject)
															
								using (new Horizontal()) {

									EditorVariables.SelectedElemSlot = Selection.activeObject as DKSlotData;
									if ( EditorVariables.SelectedElemSlot ) EditorVariables.SelectedElemPlace = EditorVariables.SelectedElemSlot.Place;
									if ( EditorVariables.SelectedElemOvlay ) EditorVariables.SelectedElemPlace = EditorVariables.SelectedElemOvlay.Place;
									GUI.color = Color.white;
										GUILayout.Label ("Place :", GUILayout.ExpandWidth (false));
										if ( EditorVariables.SelectedElemPlace)
												GUI.color = Green;
										else
												GUI.color = Red;
										if ( EditorVariables.SelectedElemPlace)
												GUILayout.Label ( EditorVariables.SelectedElemPlace.name, GUILayout.ExpandWidth (true));
										else
												GUILayout.Label ("Choose ---->", GUILayout.ExpandWidth (true));
										if (EditorVariables.SelectedElementObj == Selection.activeGameObject && !choosePlace && !chooseOverlay && !chooseSlot
												&& GUILayout.Button ("Choose", GUILayout.ExpandWidth (false))) {
												OpenPlaceWin ();
										}
									}
								}
								#endregion

							#region choose ColorPresets button
							if (Selection.activeObject && !EditorVariables.AutoDetLib && Selection.activeObject.GetType ().ToString () == "DKOverlayData") {
								if (Helper) {
									GUILayout.Space (5);
									GUI.color = Color.yellow;
									GUILayout.TextField ("You can assign some Color Presets to your overlay.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
									GUILayout.Space (10);
								}
								using (new Horizontal()) {
									 EditorVariables.SelectedElemOvlay = Selection.activeObject as DKOverlayData;

									GUI.color = Color.white;
									GUILayout.Label ("Color(s) :", GUILayout.ExpandWidth (false));
									
									if ( EditorVariables.SelectedElemOvlay.ColorPresets.Count == 1)
										GUILayout.Label ( EditorVariables.SelectedElemOvlay.ColorPresets [0].name, GUILayout.ExpandWidth (true));
									else if ( EditorVariables.SelectedElemOvlay.ColorPresets.Count > 0) {
										GUI.color = Green;
										GUILayout.Label ("Multiple ("+ EditorVariables.SelectedElemOvlay.ColorPresets.Count.ToString()+")", GUILayout.ExpandWidth (true));
									//	if ( EditorVariables.SelectedElemOvlay.ColorPresets)
									//		GUI.color = Green;
									//	else
									//		GUI.color = Color.white;
									//	if (!chooseOverlay && GUILayout.Button ("List", GUILayout.ExpandWidth (false))) {
									//		if (LinkedOverlayList)
									//			LinkedOverlayList = false;
									//		else
									//			LinkedOverlayList = true;
									//	}
									} else {
										GUI.color = Color.yellow;
										GUILayout.Label ("Choose ---->", GUILayout.ExpandWidth (true));
									}
									GUI.color = Color.yellow;
									if (EditorVariables.SelectedElementObj == Selection.activeGameObject && !choosePlace && !chooseOverlay && !chooseSlot
									    && GUILayout.Button ("Edit", GUILayout.ExpandWidth (false))) {
										OpenColorPresetWin ();
										ColorPreset_Editor._OverlayData =  EditorVariables.SelectedElemOvlay;
										ColorPreset_Editor.Statut = "ToOverlay";
									}
								}
							
							}
							#endregion choose ColorPresets button

														
							#region choose Overlay button
							if (Selection.activeObject && !EditorVariables.AutoDetLib && Selection.activeObject.GetType ().ToString () == "DKSlotData") {
								if (Helper) {
									GUILayout.Space (5);
									GUI.color = Color.yellow;
									GUILayout.TextField ("To ease the creation process, you can link a slot to an Overlay or let the DK Editor to do it during the creation.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
									GUILayout.Space (10);
								}

								if (EditorVariables.SelectedElementObj == Selection.activeGameObject)
								using (new Horizontal()) {
									GUI.color = Color.white;
									GUILayout.Label ("Overlay(s) :", GUILayout.ExpandWidth (false));
									EditorVariables.overlayList = ( Selection.activeObject as DKSlotData ).LinkedOverlayList;
									if ( EditorVariables.overlayList.Count == 1) GUILayout.Label ( EditorVariables.overlayList [0].name, GUILayout.ExpandWidth (true));
										
									else if ( EditorVariables.overlayList.Count > 0) {
										GUI.color = Green;
										GUILayout.Label ("Multiple", GUILayout.ExpandWidth (true));
										if (LinkedOverlayList) GUI.color = Green;
										else GUI.color = Color.white;
									
									/*	if (!chooseOverlay && GUILayout.Button ("List", GUILayout.ExpandWidth (false))) {
											if (LinkedOverlayList) LinkedOverlayList = false;
											else LinkedOverlayList = true;
										}*/
																				
									} else {
										GUI.color = Color.yellow;
										GUILayout.Label ("Choose ---->", GUILayout.ExpandWidth (true));
																				
									}
									GUI.color = Color.yellow;
									if (EditorVariables.SelectedElementObj == Selection.activeGameObject && !choosePlace && !chooseOverlay && !chooseSlot
											&& GUILayout.Button ("Choose", GUILayout.ExpandWidth (false))) {
											OpenChooseOverlayWin ();
									}
								}
							}
								#region Linked Overlay List
																
								if (!chooseOverlay && LinkedOverlayList && Selection.activeObject.GetType ().ToString () == "DKSlotData") {
																		GUILayout.Space (5);
																		using (new Horizontal()) {	
																				GUI.color = Color.white;
																				GUILayout.Label ("Linked Overlay List", "toolbarbutton", GUILayout.ExpandWidth (true));
																				GUI.color = Red;
																				// actions
																				if (GUILayout.Button ("X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
																						LinkedOverlayList = false;
																				}
																		}
								if ( EditorVariables.overlayList.Count == 0) {
																				using (new HorizontalCentered()) {	
																						GUI.color = Color.yellow;
																						GUILayout.Label ("No Linked Overlay in the List.", GUILayout.ExpandWidth (true));
																				}
																		} else {
																				GUI.color = Color.white;
																				using (new Horizontal()) {
																						GUILayout.Label ("Linked Overlays", "toolbarbutton", GUILayout.Width (160));
																					/*	GUILayout.Label ("Race", "toolbarbutton", GUILayout.Width (70));
																						GUILayout.Label ("Gender", "toolbarbutton", GUILayout.Width (70));
																						GUILayout.Label ("Place", "toolbarbutton", GUILayout.Width (70));
																						GUILayout.Label ("Overlay Type", "toolbarbutton", GUILayout.Width (70));
																						GUILayout.Label ("WearWeight", "toolbarbutton", GUILayout.Width (70));*/
																						GUILayout.Label ("", "toolbarbutton", GUILayout.ExpandWidth (true));
																				}

																				GUILayout.BeginScrollView (LinkedOverlayListScroll, GUILayout.ExpandHeight (true));
																				#region Linked Overlays List List
									for (int i = 0; i <  EditorVariables.overlayList.Count; i ++) {
										if ( EditorVariables.overlayList [i] != null)
																								using (new Horizontal()) {
											DKOverlayData _DKOverlayData =  EditorVariables.overlayList [i];
																										if (_DKOverlayData.Active == true)
																												GUI.color = Green;
																										else
																												GUI.color = Color.gray;
																										if (GUILayout.Button ("U", GUILayout.Width (20))) {
																												if (_DKOverlayData.Active == true)
																														_DKOverlayData.Active = false;
																												else
																														_DKOverlayData.Active = true;
																												EditorUtility.SetDirty (_DKOverlayData);
																												AssetDatabase.SaveAssets ();
																										} 
																										GUI.color = Color.white;
											if (GUILayout.Button ( EditorVariables.overlayList [i].overlayName, Slim, GUILayout.Width (120))) {
											
																										}
																										GUI.color = Red;
																										if (GUILayout.Button ("X ", Slim, GUILayout.Width (10))) {
												DKOverlayData TmpOv = EditorVariables.overlayList [i];	
																												(Selection.activeObject as DKSlotData).overlayList.Remove (TmpOv);
																												TmpOv.LinkedToSlot.Remove ((Selection.activeObject as DKSlotData));
																												EditorUtility.SetDirty (TmpOv);
																												EditorUtility.SetDirty (Selection.activeObject);
																												AssetDatabase.SaveAssets ();
												EditorVariables.overlayList.Remove (TmpOv);
																										}
																										GUI.color = Color.white;
																										string _Race = "No Race";
											if ( EditorVariables.overlayList [i].Race.Count == 0) {
																												GUI.color = Red;
																												_Race = "No Race";
																										}
											if ( EditorVariables.overlayList [i].Race.Count > 1) {
																												GUI.color = Color.cyan;
																												_Race = "Multi";
																										}
											if ( EditorVariables.overlayList [i].Race.Count == 1) {
																												GUI.color = Color.white;
												_Race =  EditorVariables.overlayList [i].Race [0];
																										}
																										if (GUILayout.Button (_Race, Slim, GUILayout.Width (70))) {
																												OpenRaceSelectEditor ();
																										}
																										GUI.color = Color.white;
											if (GUILayout.Button ( EditorVariables.overlayList [i].Gender, Slim, GUILayout.Width (70))) {
											
																										}
											if ( EditorVariables.overlayList [i].Place && GUILayout.Button ( EditorVariables.overlayList [i].Place.name, Slim, GUILayout.Width (70))) {
											
																										}
											if (GUILayout.Button ( EditorVariables.overlayList [i].OverlayType, Slim, GUILayout.Width (70))) {
											
																										}
											if (GUILayout.Button ( EditorVariables.overlayList [i].WearWeight, Slim, GUILayout.Width (70))) {
											
																										}
																								}
																				}
																				#endregion
																				GUILayout.EndScrollView ();	
																		}
																}
																#endregion
														}
														#endregion
														#region choose Overlay Tab
						
														#endregion
							
														#region choose Slot button
														if (Selection.activeObject && !EditorVariables.AutoDetLib && Selection.activeObject.GetType ().ToString () == "DKOverlayData") {
																if (Helper) {
																		GUILayout.Space (5);
																		GUI.color = Color.yellow;
																		GUILayout.TextField ("To ease the creation process, you can link an Overlay to a Slot or let the DK Editor to do it during the creation.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
																		GUILayout.Space (10);
																}
																DKOverlayData _SelectedOv = (Selection.activeObject as DKOverlayData);
																using (new Horizontal()) {
																		GUI.color = Color.white;
																		GUILayout.Label ("Linked to Slot(s) :", GUILayout.ExpandWidth (false));
																		if (_SelectedOv.LinkedToSlot.Count == 0 )
									GUILayout.Label ("Not linked", GUILayout.ExpandWidth (true));
																		else if (_SelectedOv.LinkedToSlot.Count == 1)
									GUILayout.Label (_SelectedOv.LinkedToSlot [0].name, GUILayout.ExpandWidth (true));
																		else if (_SelectedOv.LinkedToSlot.Count > 0) {
																				GUI.color = Green;
																				GUILayout.Label ("Multiple", GUILayout.ExpandWidth (true));
																		//		if (LinkedSlotList)
																		//				GUI.color = Green;
																		//		else
																		//				GUI.color = Color.white;
																		//		if (!chooseSlot && GUILayout.Button ("List", GUILayout.ExpandWidth (false))) {
																		//				if (LinkedSlotList)
																		//						LinkedSlotList = false;
																		//				else
																		//						LinkedSlotList = true;
																		//		}
																	//	} else {
																		//		GUI.color = Color.yellow;
																		//		GUILayout.Label ("Choose ---->", GUILayout.ExpandWidth (true));
																		}
																	//	GUI.color = Color.yellow;
																	//	if (Selection.activeObject.GetType ().ToString () == "DKOverlayData" && !choosePlace && !chooseOverlay && !chooseSlot
																	//			&& GUILayout.Button ("Edit", GUILayout.ExpandWidth (false))) {
																	//			OpenChooseSlotWin ();
																	//	}
																}
																#region Linked Slots List
																if (!chooseSlot && LinkedSlotList && Selection.activeObject.GetType ().ToString () == "DKOverlayData") {
																		GUILayout.Space (5);
																		using (new Horizontal()) {	
																				GUI.color = Color.white;
																				GUILayout.Label ("Linked Slot List", "toolbarbutton", GUILayout.ExpandWidth (true));
																				GUI.color = Red;
																				// actions
																				if (GUILayout.Button ("X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
																						LinkedSlotList = false;
																				}
																		}
																		if ((Selection.activeObject as DKOverlayData).LinkedToSlot.Count == 0) {
																				using (new HorizontalCentered()) {	
																						GUI.color = Color.yellow;
																						GUILayout.Label ("No Linked Slot in the List.", GUILayout.ExpandWidth (true));
																				}
																		} else {
																				GUI.color = Color.white;
																				using (new Horizontal()) {
																						GUILayout.Label ("Linked Slots", "toolbarbutton", GUILayout.Width (160));
																						GUILayout.Label ("Race", "toolbarbutton", GUILayout.Width (70));
																						GUILayout.Label ("Gender", "toolbarbutton", GUILayout.Width (70));
																						GUILayout.Label ("Place", "toolbarbutton", GUILayout.Width (70));
																						GUILayout.Label ("Overlay Type", "toolbarbutton", GUILayout.Width (70));
																						GUILayout.Label ("WearWeight", "toolbarbutton", GUILayout.Width (70));
																						GUILayout.Label ("", "toolbarbutton", GUILayout.ExpandWidth (true));
																				}
																				GUILayout.BeginScrollView (LinkedSlotListScroll, GUILayout.ExpandHeight (true));
																				#region Linked Slots List List
																				for (int i = 0; i < _SelectedOv.LinkedToSlot.Count; i ++) {
																						if (_SelectedOv.LinkedToSlot [i] != null)
																								using (new Horizontal()) {
																										DKSlotData _SlotData = _SelectedOv.LinkedToSlot [i];
																										if (_SlotData.Active == true)
																												GUI.color = Green;
																										else
																												GUI.color = Color.gray;
																										if (GUILayout.Button ("U", GUILayout.Width (20))) {
																												if (_SlotData.Active == true)
																														_SlotData.Active = false;
																												else
																														_SlotData.Active = true;
																												EditorUtility.SetDirty (_SlotData);
																												AssetDatabase.SaveAssets ();
																										} 
																										GUI.color = Color.white;
																										if (GUILayout.Button (_SelectedOv.LinkedToSlot [i].slotName, Slim, GUILayout.Width (120))) {
											
																										}
																										GUI.color = Red;
																										if (GUILayout.Button ("X ", Slim, GUILayout.Width (10))) {
																												DKSlotData TmpOv = _SelectedOv.LinkedToSlot [i];	
																												(Selection.activeObject as DKOverlayData).LinkedToSlot.Remove (TmpOv);
																												TmpOv.overlayList.Remove ((Selection.activeObject as DKOverlayData));
																												EditorUtility.SetDirty (TmpOv);
																												EditorUtility.SetDirty (Selection.activeObject);
																												AssetDatabase.SaveAssets ();
																												_SelectedOv.LinkedToSlot.Remove (TmpOv);
																										}
																										GUI.color = Color.white;
																										string _Race = "No Race";
											if ( EditorVariables.overlayList [i].Race.Count == 0) {
																												GUI.color = Red;
																												_Race = "No Race";
																										}
											if ( EditorVariables.overlayList [i].Race.Count > 1) {
																												GUI.color = Color.cyan;
																												_Race = "Multi";
																										}
											if ( EditorVariables.overlayList [i].Race.Count == 1) {
																												GUI.color = Color.white;
												_Race =  EditorVariables.overlayList [i].Race [0];
																										}
																										if (GUILayout.Button (_Race, Slim, GUILayout.Width (70))) {
																												OpenRaceSelectEditor ();
																										}
																										GUI.color = Color.white;	
										
																										if (GUILayout.Button (_SelectedOv.LinkedToSlot [i].Gender, Slim, GUILayout.Width (70))) {
											
																										}
																										if (GUILayout.Button (_SelectedOv.LinkedToSlot [i].Place.name, Slim, GUILayout.Width (70))) {
											
																										}
											if (GUILayout.Button (_SelectedOv.LinkedToSlot [i].OverlayType, Slim, GUILayout.Width (70))) {
											
																										}
											if (GUILayout.Button (_SelectedOv.LinkedToSlot [i].WearWeight, Slim, GUILayout.Width (70))) {
											
																										}
																								}
																				}
																				#endregion
																				GUILayout.EndScrollView ();	
																		}
																}
																#endregion
																#endregion
																#region choose Slot Tab
																if (chooseSlot) {
																		if (Selection.activeObject.GetType ().ToString () != "DKOverlayData") {
																				using (new Horizontal()) {	
																						GUI.color = Color.yellow;
																						GUILayout.Label ("First you need to select a slot, close this Tab to return to the slots list.", GUILayout.ExpandWidth (false));
																						GUI.color = Green;
									
																				}
																				if (GUILayout.Button ("Return to the lists", "toolbarbutton", GUILayout.ExpandWidth (true)))
																						chooseSlot = false;
																		}
																		// title
																		if (Selection.activeObject.GetType ().ToString () == "DKOverlayData")
																				using (new Horizontal()) {	
																						GUI.color = Color.white;
																						GUILayout.Label ("Slot information", "toolbarbutton", GUILayout.ExpandWidth (true));
																						GUI.color = Red;
																						// actions
																						if (GUILayout.Button ("X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
																								chooseSlot = false;
																						}
																				}
																		if (Selection.activeObject.GetType ().ToString () == "DKOverlayData") {
																				using (new Horizontal()) {
																						DKOverlayData _SlotElement = Selection.activeObject as DKOverlayData;
																						GUI.color = Color.yellow;
																						GUILayout.Label ("Slot's Info :", GUILayout.ExpandWidth (false));
																						GUI.color = Color.white;
																						GUILayout.Label ("Gender :", Slim, GUILayout.ExpandWidth (false));
																						GUI.color = Green;
																						GUILayout.Label (_SlotElement.Gender, GUILayout.ExpandWidth (false));
																						GUI.color = Color.white;
																						GUILayout.Label ("Overlay Type :", Slim, GUILayout.ExpandWidth (false));
																						GUI.color = Green;
																						GUILayout.Label (_SlotElement.OverlayType, GUILayout.ExpandWidth (false));
																						if (LinkedSlotList)
																								GUI.color = Green;
																						else
																								GUI.color = Color.white;
																						if (GUILayout.Button ("Linked Overlay List", GUILayout.ExpandWidth (true))) {
																								if (LinkedSlotList)
																										LinkedSlotList = false;
																								else
																										LinkedSlotList = true;
																						}
																				}
								
																				#region Linked Slot List
																				if (LinkedSlotList && Selection.activeObject.GetType().ToString() == "DKOverlayData" ) {
									GUILayout.Space(5);
									using (new Horizontal()) {	
										GUI.color = Color.white ;
										GUILayout.Label("Linked Overlay List", "toolbarbutton", GUILayout.ExpandWidth (true));
										GUI.color = Red;
										// actions
										if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
											LinkedSlotList = false;
										}
									}
									if ( (Selection.activeObject as DKOverlayData).LinkedToSlot.Count == 0 ) {
										using (new HorizontalCentered()) {	
											GUI.color = Color.yellow ;
											GUILayout.Label("No Linked Slot in the List.", GUILayout.ExpandWidth (true));
										}
									}
									else {
										GUI.color = Color.white ;
										using (new Horizontal()) {
											GUILayout.Label("Linked Slots", "toolbarbutton", GUILayout.Width (160));
											GUILayout.Label("Race", "toolbarbutton", GUILayout.Width (70));
											GUILayout.Label("Gender", "toolbarbutton", GUILayout.Width (70));
											GUILayout.Label("Place", "toolbarbutton", GUILayout.Width (70));
											GUILayout.Label("Overlay Type", "toolbarbutton", GUILayout.Width (70));
											GUILayout.Label("WearWeight", "toolbarbutton", GUILayout.Width (70));
											GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
										}
										GUILayout.BeginScrollView (LinkedSlotListScroll, GUILayout.ExpandHeight (true));
										#region Linked Slots List List

										for(int i = 0; i < (Selection.activeObject as DKOverlayData).LinkedToSlot.Count; i ++){
											if ((Selection.activeObject as DKOverlayData).LinkedToSlot[i] != null ) using (new Horizontal()) {
												DKSlotData _SlotData = (Selection.activeObject as DKOverlayData).LinkedToSlot[i];
												if ( _SlotData.Active == true ) GUI.color = Green;
												else GUI.color = Color.gray ;
												if (GUILayout.Button ( "U",  GUILayout.Width (20))){
													if ( _SlotData.Active == true ) _SlotData.Active = false;
													else _SlotData.Active = true;
													EditorUtility.SetDirty(_SlotData);
													AssetDatabase.SaveAssets();
												} 
												GUI.color = Color.white ;
												if (GUILayout.Button ( (Selection.activeObject as DKOverlayData).LinkedToSlot[i].slotName , Slim, GUILayout.Width (120))) {
													
												}
												GUI.color = Red;
												if (  GUILayout.Button ( "X " , Slim, GUILayout.Width (10))) {
													DKSlotData TmpSlot = (Selection.activeObject as DKOverlayData).LinkedToSlot[i];	
													(Selection.activeObject as DKOverlayData).LinkedToSlot.Remove(TmpSlot);
													TmpSlot.overlayList.Remove((Selection.activeObject as DKOverlayData));
													EditorUtility.SetDirty(TmpSlot);
													EditorUtility.SetDirty(Selection.activeObject);
													(Selection.activeObject as DKOverlayData).LinkedToSlot.Remove(TmpSlot);
													AssetDatabase.SaveAssets();
													
												}
												GUI.color = Color.white ;
												string _Race = "No Race";
												if ( (Selection.activeObject as DKOverlayData).LinkedToSlot[i].Race.Count == 0 )  {
													GUI.color = Red;
													_Race = "No Race";
												}
												if ( (Selection.activeObject as DKOverlayData).LinkedToSlot[i].Race.Count > 1 )  {
													GUI.color = Color.cyan ;
													_Race = "Multi";
												}
												if ( (Selection.activeObject as DKOverlayData).LinkedToSlot[i].Race.Count == 1 )  {
													GUI.color = Color.white ;
													_Race = (Selection.activeObject as DKOverlayData).LinkedToSlot[i].Race[0];
												}
												if (GUILayout.Button ( _Race , Slim, GUILayout.Width (70))) {
													OpenRaceSelectEditor();
												}
												GUI.color = Color.white ;
												if (GUILayout.Button ( (Selection.activeObject as DKOverlayData).LinkedToSlot[i].Gender , Slim, GUILayout.Width (70))) {
													
												}
												if (GUILayout.Button ( (Selection.activeObject as DKOverlayData).LinkedToSlot[i].Place.name , Slim, GUILayout.Width (70))) {
													
												}
													if (GUILayout.Button ( (Selection.activeObject as DKOverlayData).LinkedToSlot[i].OverlayType , Slim, GUILayout.Width (70))) {
													
												}
													if (GUILayout.Button ( (Selection.activeObject as DKOverlayData).LinkedToSlot[i].WearWeight , Slim, GUILayout.Width (70))) {
													
												}
											}
										}
										#endregion
										GUILayout.EndScrollView ();	
									}
								}	
							}
							#endregion
							
							GUILayout.Space(5);
							#region Slots List
							if ( Selection.activeObject.GetType().ToString() == "DKOverlayData" ){
								using (new Horizontal()) {	
									GUI.color = Color.white ;
									GUILayout.Label("Select a Slot for the Overlay", "toolbarbutton", GUILayout.ExpandWidth (true));
								}
								using (new Horizontal()) {
								GUI.color = Color.white ;
									GUILayout.Label("Selected Slot :", GUILayout.ExpandWidth (false));
									if (  EditorVariables.SelectedElemSlot != null ) {
										GUI.color = Green;
										GUILayout.Label( EditorVariables.SelectedElemSlot.slotName, GUILayout.ExpandWidth (true));
										if ( GUILayout.Button ( "Link it", GUILayout.Width (90))) {
											 EditorVariables.SelectedElemSlot.overlayList.Add(Selection.activeObject as DKOverlayData);
											(Selection.activeObject as DKOverlayData).LinkedToSlot.Add( EditorVariables.SelectedElemSlot);
											EditorUtility.SetDirty(Selection.activeObject);
											EditorUtility.SetDirty( EditorVariables.SelectedElemSlot);
											AssetDatabase.SaveAssets();
										}
									}
									else {
										GUI.color = Color.yellow ;
										GUILayout.Label("Select a Slot in the following List.", GUILayout.ExpandWidth (true));
									}
								}
								GUI.color = Color.white ;
								using (new Horizontal()) {
									GUILayout.Label("Slots", "toolbarbutton", GUILayout.Width (160));
									GUILayout.Label("Race", "toolbarbutton", GUILayout.Width (70));
									GUILayout.Label("Gender", "toolbarbutton", GUILayout.Width (70));
									GUILayout.Label("Place", "toolbarbutton", GUILayout.Width (70));
									GUILayout.Label("Overlay Type", "toolbarbutton", GUILayout.Width (80));
									GUILayout.Label("WearWeight", "toolbarbutton", GUILayout.Width (90));
									GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
								}
								using (new ScrollView(ref scroll)) 
								{
									#region Slots
									using (new Horizontal()) {	
										GUI.color = Color.yellow ;
										GUILayout.Label ( "Slot Library", GUILayout.ExpandWidth (false));
									}
										for(int i = 0; i < EditorVariables.DK_UMACrowd.slotLibrary.slotElementList.Length; i ++){
											if (EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i] != null ) using (new Horizontal()) {
												DKSlotData _SlotData = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
											if ( _SlotData.Active == true ) GUI.color = Green;
											else GUI.color = Color.gray ;
											if (GUILayout.Button ( "U",  GUILayout.Width (20))){
												if ( _SlotData.Active == true ) _SlotData.Active = false;
												else _SlotData.Active = true;
												EditorUtility.SetDirty(_SlotData);
												AssetDatabase.SaveAssets();
											} 
											
												if ( (Selection.activeObject as DKOverlayData).LinkedToSlot.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
											else {
													if ( EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i] ==  EditorVariables.SelectedElemSlot ) GUI.color = Color.yellow;
												else GUI.color = Color.white;
											}
												if (GUILayout.Button ( EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i].slotName , Slim, GUILayout.Width (140))) {
													if ( (Selection.activeObject as DKOverlayData).LinkedToSlot.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) == false )
														EditorVariables.SelectedElemSlot = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
											}
											// Race
											DKSlotData DK_Race;
												DK_Race = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
												if ( (Selection.activeObject as DKOverlayData).LinkedToSlot.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
											else GUI.color = Color.white;
											if ( DK_Race.Race.Count == 0 && GUILayout.Button ( "No Race" , Slim, GUILayout.Width (70))) {
												OpenRaceSelectEditor();
											}
				
												if ( (Selection.activeObject as DKOverlayData).LinkedToSlot.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
											else GUI.color = Color.white;
											if ( DK_Race.Race.Count == 1 && GUILayout.Button ( DK_Race.Race[0] , Slim, GUILayout.Width (70))) {
												OpenRaceSelectEditor();
											}
											else
											if ( DK_Race.Race.Count == 1 && GUILayout.Button ( "Multi" , Slim, GUILayout.Width (70))) {
												OpenRaceSelectEditor();
											}	
											// Gender
												if ( (Selection.activeObject as DKOverlayData).LinkedToSlot.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
											else GUI.color = Color.white;
											if ( DK_Race.Gender == "" && GUILayout.Button ( "No Gender" , Slim, GUILayout.Width (70))) {
													EditorVariables.SelectedElemSlot = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
											}
				
												if ( (Selection.activeObject as DKOverlayData).LinkedToSlot.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
											else GUI.color = Color.white;
											if ( DK_Race.Gender != "" && GUILayout.Button ( DK_Race.Gender , Slim, GUILayout.Width (70))) {
													EditorVariables.SelectedElemSlot = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
											}
											// Place
												if ( (Selection.activeObject as DKOverlayData).LinkedToSlot.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
											else GUI.color = Color.white;
											if ( DK_Race.Place == null && GUILayout.Button ( "No Place" , Slim, GUILayout.Width (70))) {
												
											}
				
												if ( (Selection.activeObject as DKOverlayData).LinkedToSlot.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
											else GUI.color = Color.white;
											if ( DK_Race.Place != null && GUILayout.Button ( DK_Race.Place.name , Slim, GUILayout.Width (70))) {
													EditorVariables.SelectedElemSlot = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
											}
											// Overlay Type
												if ( (Selection.activeObject as DKOverlayData).LinkedToSlot.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
											else GUI.color = Color.white;
												if ( DK_Race.OverlayType == "" && GUILayout.Button ( "No Overlay Type" , Slim, GUILayout.Width (70))) {
													EditorVariables.SelectedElemSlot = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
											}
				
												if ( (Selection.activeObject as DKOverlayData).LinkedToSlot.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
											else GUI.color = Color.white;
												if ( DK_Race.OverlayType != "" && GUILayout.Button ( DK_Race.OverlayType , Slim, GUILayout.Width (90))) {
													EditorVariables.SelectedElemSlot = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
											}
											// WearWeight
											GUI.color =  Color.gray;
												if (  DK_Race.WearWeight == "" ) 
											{
											//	GUI.color = Color.gray ;
											//	GUILayout.Space(55);
											//	GUILayout.Label ( "Not Weight");
											}
											else
											{
													if ( DK_Race.WearWeight != "" ) GUI.color = Red;
													if ( DK_Race.WearWeight != "" && GUILayout.Button ( "X " , Slim, GUILayout.Width (10))) {
														DK_Race.WearWeight = "";
													EditorUtility.SetDirty(DK_Race);
													AssetDatabase.SaveAssets();
												}
												GUI.color = Color.white ;
													GUILayout.Label( DK_Race.WearWeight , Slim, GUILayout.Width (70));
											}
										}
									}
									#endregion
								}
							}
						}
					}
					#endregion
				
					#endregion
					GUILayout.Space(10);
					#region Apply changes
					if ( !EditorVariables.AutoDetLib && !choosePlace && !chooseOverlay && !chooseSlot )
					{
						GUI.color = Color.yellow;
						if ( EditorVariables.SelectedElementObj == Selection.activeGameObject && Helper ) GUILayout.TextField("Setup the Element and click on the Apply button." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						if ( EditorVariables.SelectedElementObj == Selection.activeGameObject ) GUI.color = Green;
						else GUI.color = Red;
						if ( EditorVariables.SelectedElementObj != Selection.activeGameObject ) GUILayout.TextField("You need to select an Element from the following List." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						

							using (new Horizontal()) {
							if (  EditorVariables.SelectedElementName != "" && EditorVariables.SelectedElementObj == Selection.activeGameObject 
								&& GUILayout.Button("Apply changes", GUILayout.ExpandWidth (true)))
							{
								// Edit Prefab
								EditorUtility.SetDirty(Selection.activeObject);
							/*	// Find component
								DKSlotData _SlotElement = Selection.activeObject as DKSlotData;
								DKOverlayData _OverlayElement = Selection.activeObject as DKOverlayData;
								DKRaceData _RaceElement = Selection.activeObject as DKRaceData;
								// verify component and apply
								if ( _SlotElement ) {
								//	string myPath;
								//	myPath = AssetDatabase.GetAssetPath( Selection.activeObject ); 
								//	AssetDatabase.RenameAsset(myPath, SelectedPrefabName);
									_SlotElement.slotName =  EditorVariables.SelectedElementName;
									//Change Slot Elem's name
									_SlotElement.name = _SlotElement.slotName;
									// apply to DK_Race component
								//	_SlotElement.Gender = EditorVariables.SelectedElementGender;
									//	_SlotElement.OverlayType = EditorVariables.SelectedElementOverlayType;
									//	_SlotElement.LinkedOverlayList =  EditorVariables.overlayList;
									//	_SlotElement.WearWeight = EditorVariables.SelectedElementWearWeight;
								//	_SlotElement.Replace = EditorVariables.Replace;
									_SlotElement.Active = true;
									
									if (  EditorVariables.SelectedElemPlace != null ) _SlotElement.Place =  EditorVariables.SelectedElemPlace;
								}
								else
								if ( _OverlayElement ) {
								//	string myPath;
								//	myPath = AssetDatabase.GetAssetPath( Selection.activeObject ); 
								//	AssetDatabase.RenameAsset(myPath, SelectedPrefabName);
									_OverlayElement.name = _OverlayElement.overlayName;
									_OverlayElement.overlayName =  EditorVariables.SelectedElementName;
									_OverlayElement.Gender = EditorVariables.SelectedElementGender;
									_OverlayElement.OverlayType = EditorVariables.SelectedElementOverlayType;
									_OverlayElement.WearWeight = EditorVariables.SelectedElementWearWeight;
									_OverlayElement.Active = true;

									if (  EditorVariables.SelectedElemPlace != null ) _OverlayElement.Place =  EditorVariables.SelectedElemPlace;
								}
								else	
								if ( _RaceElement ) {
									string myPath;
									myPath = AssetDatabase.GetAssetPath( Selection.activeObject ); 
									AssetDatabase.RenameAsset(myPath, SelectedPrefabName);
									_RaceElement.raceName =  EditorVariables.SelectedElementName;
									_RaceElement.Race = _Name;
									//Change Race Elem
									_RaceElement.name =  EditorVariables.SelectedElementName;
									// apply to DK_Race component
									_RaceElement.Gender = EditorVariables.SelectedElementGender;
								}*/
								AssetDatabase.SaveAssets();
							}
						}
						}
					#endregion

					#endregion Prepare Libraries
				}
				#endregion
				#endregion Prepare
				#endregion Prepare Menu
			}
			
			#region DK Libraries
			if ( ShowDKLibraries )
			{
				// title
				using (new Horizontal()) {
					GUI.color = Color.white ;
					GUILayout.Label("DK UMA Libraries", "toolbarbutton", GUILayout.ExpandWidth (true));
					GUI.color = Red;
					if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
						ShowDKLibraries = false;
						
					}
				}
				#region Helper DK Lib general
				if ( !ShowDKLibSE && !ShowDKLibSA )
				{
					if ( Helper )
					{
						GUI.color = Color.white;
						GUILayout.TextField("All the DK UMA Libraries need to be correctly populated and configured for the Editor to work properly." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					}
					#region Helper Messages

					#region Menu
					if ( !ShowGenPreset ) {
						GUI.color = Color.yellow;
						if ( Helper )  GUILayout.TextField("Adding or removing expression from the Slot Expressions Library will redefine the way that the Research of the Auto Detecting Slot Element is working." +
							"Every Expression is used to sort the Slot Elements." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						GUI.color = Color.white;
						if (  GUILayout.Button ( "Modify Expressions Library", GUILayout.ExpandWidth (true))) {
							OpenExpressionsWin();
						}
					}
					if ( !ShowGenPreset && EditorVariables.SlotsAnatomyLibraryObj != null && EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length != 0 ) 
					{
						GUI.color = Color.yellow;
						if ( Helper ) GUILayout.TextField("The Anatomy Slots are used to generate the Model, during the Generation, the engine will use them depending on the Active 'Generator Preset'. Every Anatomy Slot MUST be unique." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						if (  GUILayout.Button ( "Modify Anatomy Slots Library", GUILayout.ExpandWidth (true))) {
								OpenAnatomy_Editor();
						
							AnatomyPart = "";

						}
					}
					GUI.color = Color.white;
					if (!ShowGenPreset &&  Helper ) GUILayout.TextField("Create and modify the Color Presets." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					if ( !ShowGenPreset &&  GUILayout.Button ( "Color Presets", GUILayout.ExpandWidth (true))) {
						OpenColorPresetWin();

					}
					GUI.color = Color.yellow;
					#region V1.1
				/*	if ( Editor_Global.VersionW >= 1 && Editor_Global.VersionX >= 1 ) {
						if ( !ShowGenPreset &&  GUILayout.Button ( "Default DNA Limitations Editor", GUILayout.ExpandWidth (true))) {
							OpenConvWin();
						}
					}*/
					#endregion V1.1
					if (!ShowGenPreset &&  Helper )
					{
						GUI.color = Color.white;
						GUILayout.TextField("To create a Model, the DK UMA uses a Generator Preset, gathering the required Slots. You can create and use your own custom Presets." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						GUI.color = Color.yellow;
						GUILayout.TextField("All the presets can be modified to meet your requirement." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					}
					GUI.color = Color.white;
					if (!ShowGenPreset &&  GUILayout.Button ( "Modify Generator Presets", GUILayout.ExpandWidth (true))) {
						DK_UMA = GameObject.Find("DK_UMA");
						if ( DK_UMA == null ) {
							var goDK_UMA = new GameObject();	goDK_UMA.name = "DK_UMA";
							DK_UMA = GameObject.Find("DK_UMA");
						}
						EditorVariables.GeneratorPresets = GameObject.Find("Generator Presets");
						if ( EditorVariables.GeneratorPresets == null ) 
						{
							EditorVariables.GeneratorPresets = (GameObject) Instantiate(Resources.Load("Generator Presets"), Vector3.zero, Quaternion.identity);
							EditorVariables.GeneratorPresets.name = "Generator Presets";
							EditorVariables.GeneratorPresets = GameObject.Find("Generator Presets");
							EditorVariables.GeneratorPresets.transform.parent = DK_UMA.transform;
							EditorVariables.PresetToEdit = null;
						}
						ShowGenPreset = true;
						EditGenPreset = true;
					}	
					GUI.color = Color.yellow;
					if (!ShowGenPreset &&  Helper ) GUILayout.TextField("You can create your own presets, adding the Slots to a specific Library." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					GUI.color = Color.white;
					if ( !ShowGenPreset && GUILayout.Button ( "New Generator Preset", GUILayout.ExpandWidth (true))) {
						NewPresetName = "New Preset";
						ShowGenPreset = true;
						NewGenPreset = true;
						for(int i = 0; i < EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length; i ++){
							EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.Selected = false;
						}
					}
					#endregion Menu
					#endregion Helper Messages
				}
				#endregion Helper DK Lib general
				
				
				

				if ( ShowDKLibSE ){
					DK_UMA = GameObject.Find("DK_UMA");
					SlotExpressions = GameObject.Find("Slot Expressions");
					if ( DK_UMA == null ) {
						var goDK_UMA = new GameObject();	goDK_UMA.name = "DK_UMA";
						var goSlotExpressions = new GameObject();	goSlotExpressions.name = "Slot Expressions";
						goSlotExpressions.transform.parent = goDK_UMA.transform;
					}
					else if ( SlotExpressions == null ) {
						SlotExpressions = new GameObject();	SlotExpressions.name = "Slot Expressions";
						SlotExpressions.transform.parent = DK_UMA.transform;
					}
					SlotExpressions = GameObject.Find("Slot Expressions");
					
					if ( SelectedExpression != null ) 
					{
						dk_SlotExpressionsElement = SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
					}
					
					GUILayout.Space(5);
					using (new Horizontal()) {
						GUI.color = Color.white ;
						GUILayout.Label("Modify Slot Expressions", "toolbarbutton", GUILayout.ExpandWidth (true));
						GUI.color = Red;
						if (  GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
							ShowDKLib = false;
							ShowDKLibSE = false;
							ShowDKLibSA = false;
							
						}
					}
					GUI.color = Color.white ;
					if ( Helper ) GUILayout.TextField("The Expressions are used during the Auto Detect process to populate your libraries. You can add words to the Expressions, with an Anatomy link." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					
					GUI.color = Color.yellow ;
					if ( Helper ) GUILayout.TextField("Write the Expression for the research engine to find during the Auto Detect process." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					using (new Horizontal()) {
						GUI.color = Color.white ;
						GUILayout.Label("Expression :", GUILayout.ExpandWidth (false));
						if ( NewExpressionName != "" ) GUI.color = Green;
						else GUI.color = Red;
						NewExpressionName = GUILayout.TextField(NewExpressionName, 50, GUILayout.ExpandWidth (true));
						if (  GUILayout.Button ( "Change", GUILayout.ExpandWidth (false))) {
							Debug.Log ( "Expression " +SelectedExpression.name+ " changed to " +NewExpressionName+ ".");
							SelectedExpression.name = NewExpressionName;
							dk_SlotExpressionsElement.dk_SlotExpressionsElement.dk_SlotExpressionsName = NewExpressionName;
							EditorUtility.SetDirty(dk_SlotExpressionsElement.gameObject);
							AssetDatabase.SaveAssets();
						}
					}
					GUI.color = Color.yellow ;
					if ( Helper ) GUILayout.TextField("The Anatomy Part is really important, it is the place where the detected Slot Element will be generated. In most of the cases, a full Model counts only one Anatomy part of each type (Eyes, head...)." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					if ( !ShowSelectAnatomy )using (new Horizontal()) {
						GUI.color = Color.white ;
						GUILayout.Label("Anatomy Part :", GUILayout.ExpandWidth (false));
						if ( AnatomyPart != "" ) GUI.color = Green;
						else GUI.color = Red;
						if ( SelectedExpression == null) AnatomyPart = "";
						else if ( dk_SlotExpressionsElement.dk_SlotExpressionsElement.AnatomyPart == null ) AnatomyPart = "Select an Anatomy Part";
						else AnatomyPart = dk_SlotExpressionsElement.dk_SlotExpressionsElement.AnatomyPart.name;
						GUILayout.Label (AnatomyPart, GUILayout.ExpandWidth (true));
						GUI.color = Color.white ;
						if (  GUILayout.Button ( "Select", GUILayout.ExpandWidth (false))) {
							ShowSelectAnatomy = true;
						}
					}
				

					// Overlay Type
					GUI.color = Color.yellow;
					if ( Helper ) GUILayout.TextField("The Overlay Type is used by the Generator during the Model's creation. " +
						"All the 'Naked body parts' must be of the 'Flesh' Type, the Head slot must be of the 'Face' type." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

					using (new Horizontal()) {
						GUI.color = Color.white;
						GUILayout.Label ( "Body", GUILayout.ExpandWidth (false));
						if ( OverlayType == "Flesh" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Flesh", GUILayout.ExpandWidth (true))) {
							OverlayType = "Flesh";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
						if ( OverlayType == "Face" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Face", GUILayout.ExpandWidth (true))) {
							OverlayType = "Face";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
				
						if ( OverlayType == "Eyes" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Eyes", GUILayout.ExpandWidth (true))) {
							OverlayType = "Eyes";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
						if ( OverlayType == "Hair" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Hair", GUILayout.ExpandWidth (true))) {
							OverlayType = "Hair";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
					}

					using (new Horizontal()) {	
						if ( OverlayType == "Eyebrow" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Eyebrow", GUILayout.ExpandWidth (true))) {
							OverlayType = "Eyebrow";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
						if ( OverlayType == "Lips" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Lips", GUILayout.ExpandWidth (true))) {
							OverlayType = "Lips";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
						if ( OverlayType == "Makeup" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Makeup", GUILayout.ExpandWidth (true))) {
							OverlayType = "Makeup";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
						if ( OverlayType == "Tatoo" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Tatoo", GUILayout.ExpandWidth (true))) {
							OverlayType = "Tatoo";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
						if ( OverlayType == "Beard" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Beard", GUILayout.ExpandWidth (true))) {
							OverlayType = "Beard";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
					}

					using (new Horizontal()) {
						GUI.color = Color.white;
						GUILayout.Label ( "Wears :", GUILayout.ExpandWidth (false));
						if ( OverlayType == "TorsoWear" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Torso", GUILayout.ExpandWidth (true))) {
							OverlayType = "TorsoWear";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
						if ( GUILayout.Button ( "Shoulder", GUILayout.ExpandWidth (true))) {
							OverlayType = "ShoulderWear";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
						if ( GUILayout.Button ( "Belt", GUILayout.ExpandWidth (true))) {
								OverlayType = "BeltWear";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
						if ( OverlayType == "LegsWear" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Leg", GUILayout.ExpandWidth (true))) {
							OverlayType = "LegsWear";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
						if ( OverlayType == "FeetWear" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Feet", GUILayout.ExpandWidth (true))) {
							OverlayType = "FeetWear";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
						if ( OverlayType == "HandsWear" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Hand", GUILayout.ExpandWidth (true))) {
							OverlayType = "HandsWear";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
						if ( OverlayType == "HeadWear" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Head", GUILayout.ExpandWidth (true))) {
							OverlayType = "HeadWear";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
					}
					using (new Horizontal()) {	
						if ( OverlayType == "Underwear" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Underwear", GUILayout.ExpandWidth (true))) {
							OverlayType = "Underwear";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = OverlayType; 
						}
						if ( OverlayType == "" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "None", GUILayout.ExpandWidth (true))) {
							OverlayType = "";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType = ""; 
						}
					}
					// weights
					if ( OverlayType.Contains("Wear") == true && OverlayType != "Underwear" ) using (new Horizontal()) {
						GUI.color = Color.white;
						GUILayout.Label ( "Weight", GUILayout.ExpandWidth (false));
						if ( EditorVariables.SelectedElementWearWeight == "Light" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Light", GUILayout.ExpandWidth (true))) {
							EditorVariables.SelectedElementWearWeight = "Light";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.WearWeight = EditorVariables.SelectedElementWearWeight; 
						}
						if ( EditorVariables.SelectedElementWearWeight == "Medium" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Medium", GUILayout.ExpandWidth (true))) {
							EditorVariables.SelectedElementWearWeight = "Medium";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.WearWeight = EditorVariables.SelectedElementWearWeight; 

						}
						if ( EditorVariables.SelectedElementWearWeight == "High" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "High", GUILayout.ExpandWidth (true))) {
							EditorVariables.SelectedElementWearWeight = "High";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.WearWeight = EditorVariables.SelectedElementWearWeight; 

						}
						if ( EditorVariables.SelectedElementWearWeight == "Heavy" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Heavy", GUILayout.ExpandWidth (true))) {
							EditorVariables.SelectedElementWearWeight = "Heavy";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.WearWeight = EditorVariables.SelectedElementWearWeight; 

						}
						if ( EditorVariables.SelectedElementWearWeight == "" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "No", GUILayout.ExpandWidth (true))) {
							EditorVariables.SelectedElementWearWeight = "";
							DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
							Expression_SlotExpressionsElement.dk_SlotExpressionsElement.WearWeight = EditorVariables.SelectedElementWearWeight; 

						}
					}
					GUI.color = Color.yellow ;
					if ( !ShowSelectAnatomy && Helper ) GUILayout.TextField("Here follows a list of the installed Expressions. The 'P' letter is about Prefab, gray seams that the Expression has no Prefab, cyan seams that it is ok. Click on a gray 'P' to create a prefab. The Auto Detect Process will only use the 'Selected' Expression. " , 268, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					// List menu
					if ( !ShowSelectAnatomy )using (new Horizontal()) {
						GUI.color = Color.white ;
						if (  GUILayout.Button ( "Add New", GUILayout.ExpandWidth (true))) {
							GameObject NewExpressionObj =  (GameObject) Instantiate(Resources.Load("New_Expression"), Vector3.zero, Quaternion.identity);
							NewExpressionObj.name = "New Expression";
							Selection.activeGameObject = NewExpressionObj;
							SelectedExpression = NewExpressionObj;
							if ( SlotExpressions != null ) NewExpressionObj.transform.parent = SlotExpressions.transform;
							NewExpressionName = SelectedExpression.name;
							Debug.Log ("New Expression created and selected.");
						}
						if (  GUILayout.Button ( "Copy Selected", GUILayout.ExpandWidth (true))) {
							if ( SelectedExpression != null )
							{
								GameObject NewExpressionObj =  (GameObject) Instantiate(SelectedExpression, Vector3.zero, Quaternion.identity);
								NewExpressionObj.name = SelectedExpression.name + " (Copy)";
								NewExpressionObj.transform.parent = SlotExpressions.transform;
								Debug.Log (SelectedExpression+ " has been copied to " + NewExpressionObj.name+ ".");
							}
							else Debug.Log ("You have to select an Expression from the list to be able to copy it.");
							
						}
						if (  GUILayout.Button ( "Select All", GUILayout.ExpandWidth (true))) {
							foreach ( Transform Expression in SlotExpressions.transform )
							{
								DK_SlotExpressionsElement Expression_SlotExpressionsElement =  Expression.GetComponent<DK_SlotExpressionsElement>();
								Expression_SlotExpressionsElement.dk_SlotExpressionsElement.Selected = true; 
							}
						}
					}
					// Lists
					GUILayout.Space(5);
					// Choose Anatomy Part List
					
						if ( ShowSelectAnatomy ) 
						{
							using (new Horizontal()) {
								GUI.color = Color.white ;
								GUILayout.Label("Choose Anatomy Part", "toolbarbutton", GUILayout.ExpandWidth (true));
								GUI.color = Red;
								if (  GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
									ShowSelectAnatomy = false;
								}
							}
							GUILayout.Space(5);
							GUI.color = Color.white ;
							using (new Horizontal()) {
								GUILayout.Label("Selected Part :",  GUILayout.ExpandWidth (false));
								GUI.color = Color.yellow ;
								if ( SelectedAnaPart != null ) GUILayout.Label(SelectedAnaPart.name, GUILayout.ExpandWidth (true));
							}
							GUILayout.Space(5);
							GUI.color = Color.white ;
							if ( GUILayout.Button ( "Assign to Expression", GUILayout.ExpandWidth (true))) {
								DK_SlotExpressionsElement Expression_SlotExpressionsElement =  SelectedExpression.GetComponent<DK_SlotExpressionsElement>();
								Debug.Log ( "Anatomy Part " +SelectedAnaPart.name+ " assigned to " +Expression_SlotExpressionsElement.name+ ".");
								GameObject _Prefab = PrefabUtility.GetPrefabParent( SelectedAnaPart.gameObject) as GameObject;
								if ( _Prefab ) Expression_SlotExpressionsElement.dk_SlotExpressionsElement.AnatomyPart = _Prefab;
								else Expression_SlotExpressionsElement.dk_SlotExpressionsElement.AnatomyPart = SelectedAnaPart.gameObject;
									
								ShowSelectAnatomy = false;
								EditorUtility.SetDirty(Expression_SlotExpressionsElement.gameObject);
								AssetDatabase.SaveAssets();
							}
							GUILayout.Space(5);
							#region Search
							using (new Horizontal()) {
								GUI.color = Color.white;
								GUILayout.Label("Search for :", GUILayout.ExpandWidth (false));
								SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));
								
							}
							#endregion Search

							GUILayout.Space(5);
							using (new Horizontal()) {
								GUI.color = Color.white ;
								GUILayout.Label("Anatomy Part", "toolbarbutton", GUILayout.Width (200));
								GUILayout.Label("Race", "toolbarbutton", GUILayout.Width (60));
								GUILayout.Label("Gender", "toolbarbutton", GUILayout.Width (60));
								GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
							}
							using (new ScrollView(ref scroll2)) 
							{
								for(int i = 0; i < EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length; i ++){
									if ( SearchString == "" || EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].name.ToLower().Contains(SearchString.ToLower()) ) using (new Horizontal(GUILayout.Width (80))) {
										// Element
										GUI.color = Color.white ;
										if (GUILayout.Button ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.dk_SlotsAnatomyName , Slim, GUILayout.Width (200))) {
											SelectedAnaPart = EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i];
										}
										// Race
										if ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.GetComponent<DK_Race>() as DK_Race == null ) {
											EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.AddComponent<DK_Race>();	
										}
										DK_Race DK_Race;
										DK_Race = EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].GetComponent("DK_Race") as DK_Race;
										if ( DK_Race.Race.Count == 0 ) GUI.color = Red;
										if ( DK_Race.Race.Count == 0 && GUILayout.Button ( "No Race" , Slim, GUILayout.Width (60))) {
											
										}
										GUI.color = Green;
										if ( DK_Race.Race.Count == 1 && GUILayout.Button ( DK_Race.Race[0] , Slim, GUILayout.Width (60))) {
											
										}
										if ( DK_Race.Race.Count > 1 && GUILayout.Button ( "Multi" , Slim, GUILayout.Width (60))) {
											
										}
										// Gender
										if ( DK_Race.Gender == "" ) GUI.color = Red;
										if ( DK_Race.Gender == "" ) GUILayout.Label ( "N" , "Button") ;
										GUI.color = Green;
										if ( DK_Race.Gender != "" && DK_Race.Gender == "Female" ) GUILayout.Label ( "Female" , Slim, GUILayout.Width (50) );
										if ( DK_Race.Gender != "" && DK_Race.Gender == "Male" ) GUILayout.Label ( "Male" , Slim, GUILayout.Width (50) );
										if ( DK_Race.Gender != "" && DK_Race.Gender == "Both" ) GUILayout.Label ( "Both" , Slim, GUILayout.Width (50) );
										
										// OverlayType
										if ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.GetComponent<DK_Race>() as DK_Race == null ) {
											EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.AddComponent<DK_Race>();	
										}
										DK_Race = EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].GetComponent("DK_Race") as DK_Race;
										if ( DK_Race.OverlayType == "" ) GUI.color = Red;
										if ( DK_Race.OverlayType == "" && GUILayout.Button ( "No OverlayType" , Slim, GUILayout.Width (50))) {
											
										}
										GUI.color = Green;
										if ( DK_Race.OverlayType != "" && GUILayout.Button ( DK_Race.OverlayType , Slim, GUILayout.Width (50))) {
											
										}
									}
								}
							}
						}
						else
					
						#region Search
					
						if ( !ShowSelectAnatomy ){
							using (new Horizontal()) {
								GUI.color = Color.white;
								GUILayout.Label("Search for :", GUILayout.ExpandWidth (false));
								SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));
								
							}
							#endregion Search
							// Expressions List
						using (new Horizontal()) {
								GUI.color = Color.white ;
								GUILayout.Label("Expression", "toolbarbutton", GUILayout.Width (145));
								GUILayout.Label("Anatomy Part", "toolbarbutton", GUILayout.Width (120));
								GUILayout.Label("Overlay Type", "toolbarbutton", GUILayout.Width (90));
								GUILayout.Label("Weight", "toolbarbutton", GUILayout.Width (90));
								GUILayout.Label("Selected", "toolbarbutton", GUILayout.Width (70));
								GUILayout.Label("Delete", "toolbarbutton", GUILayout.ExpandWidth (true));
							}
							using (new ScrollView(ref scroll)) 
							{
								foreach ( Transform Expression in SlotExpressions.transform )
								{
									DK_SlotExpressionsElement Expression_SlotExpressionsElement =  Expression.GetComponent<DK_SlotExpressionsElement>();
									if ( SearchString == "" || Expression.name.ToLower().Contains(SearchString.ToLower()) )using (new Horizontal()) {
										// Prefab Verification
										string myPath = PrefabUtility.GetPrefabParent( Expression.gameObject ).ToString() ;
										if ( myPath == "null" ) GUI.color = Color.gray;
										else GUI.color = Color.cyan;
										if ( GUILayout.Button ( "P", "toolbarbutton", GUILayout.ExpandWidth (false))) {
											// Create Prefab
											if ( myPath == "null" ) 
											{
												PrefabUtility.CreatePrefab("Assets/DK Editors/DK_UMA_Editor/Prefabs/DK_SlotExpressions/" + Expression.name + ".prefab", Expression.gameObject );
												GameObject clone = PrefabUtility.InstantiateAttachedAsset( Resources.LoadAssetAtPath("Assets/DK Editors/DK_UMA_Editor/Prefabs/DK_SlotExpressions/"+ Expression.name + ".prefab", typeof(GameObject)) ) as GameObject;										
												clone.name = Expression.name;
												clone.transform.parent = Expression.parent;
												DestroyImmediate ( Expression.gameObject ) ;
											}
										}
										if ( Expression && SelectedExpression && Expression == SelectedExpression.transform ) GUI.color = Color.yellow ;
		  								else GUI.color = Color.white ;
										if ( Expression && GUILayout.Button (  Expression.name , Slim, GUILayout.Width (120))) {
											if ( Expression_SlotExpressionsElement.dk_SlotExpressionsElement.AnatomyPart != null ) AnatomyPart = Expression_SlotExpressionsElement.dk_SlotExpressionsElement.AnatomyPart.name;
											else AnatomyPart = "Select an Anatomy part";
											SelectedExpression = Expression.gameObject;
											Selection.activeGameObject = SelectedExpression;
											NewExpressionName = Expression.name;
											OverlayType = Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType;
											EditorVariables.SelectedElementWearWeight = Expression_SlotExpressionsElement.dk_SlotExpressionsElement.WearWeight;
										}
										// Anatomy part
										if ( Expression_SlotExpressionsElement.dk_SlotExpressionsElement.AnatomyPart != null )
											GUILayout.Label(Expression_SlotExpressionsElement.dk_SlotExpressionsElement.AnatomyPart.name, Slim, GUILayout.Width (120));
										else GUILayout.Label("Select an Anatomy part for the Expression to be linked.", Slim, GUILayout.Width (120));
										// Overlay Type
										if ( Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType != "" )
											GUILayout.Label(Expression_SlotExpressionsElement.dk_SlotExpressionsElement.OverlayType, Slim, GUILayout.Width (90));
										else 
										{
											GUI.color = Color.yellow ;
											GUILayout.Label("Not an Overlay.", Slim, GUILayout.Width (90));
										}
										// WearWeight
											if ( Expression_SlotExpressionsElement.dk_SlotExpressionsElement.WearWeight == "" ) GUI.color = Red;
											if ( Expression_SlotExpressionsElement.dk_SlotExpressionsElement.WearWeight == "" && GUILayout.Button ( "No Weight" , Slim, GUILayout.Width (50))) {
												
											}
											GUI.color = Green;
											if ( Expression_SlotExpressionsElement.dk_SlotExpressionsElement.WearWeight != "" && GUILayout.Button ( Expression_SlotExpressionsElement.dk_SlotExpressionsElement.WearWeight , Slim, GUILayout.Width (50))) {
												
											}
										
										// Select
										if ( Expression && Expression_SlotExpressionsElement.dk_SlotExpressionsElement.Selected == true ) GUI.color = Green;
										else GUI.color = Color.gray ;
										if (GUILayout.Button ( "Selected" , GUILayout.Width (65))) {
											
											if ( Expression && Expression_SlotExpressionsElement.dk_SlotExpressionsElement.Selected == true )Expression_SlotExpressionsElement.dk_SlotExpressionsElement.Selected = false;
											else Expression_SlotExpressionsElement.dk_SlotExpressionsElement.Selected = true; 
										}
										GUI.color = Red;
										if ( Expression && GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
											DestroyImmediate ( Expression.gameObject ) ;
										}
									}
								}
							}
						}
				}
			}
			#endregion DK Libraries
			
			#region Prepare Libraries
			if ( ShowLibraries )
			{
				if ( EditorVariables.RaceLibraryObj == null ){
						EditorVariables.RaceLibraryObj = GameObject.Find("DKRaceLibrary");	
				}
				EditorVariables._RaceLibrary =  EditorVariables.RaceLibraryObj.GetComponent<DKRaceLibrary>();
				if ( EditorVariables.DKSlotLibraryObj == null ) {
					EditorVariables.DKSlotLibraryObj = GameObject.Find("DKSlotLibrary");
				}
				EditorVariables._DKSlotLibrary =  EditorVariables.DKSlotLibraryObj.GetComponent<DKSlotLibrary>();
				if ( EditorVariables.OverlayLibraryObj == null ){
						EditorVariables.OverlayLibraryObj = GameObject.Find("DKOverlayLibrary");	
				}
				EditorVariables._OverlayLibrary =  EditorVariables.OverlayLibraryObj.GetComponent<DKOverlayLibrary>();

				if ( EditorVariables.DKUMACustomizationObj != null ) {
					EditorVariables.DKUMACustomizationObj = GameObject.Find("DKUMACustomization");	
					EditorVariables.DK_DKUMACustomization =  EditorVariables.DKUMACustomizationObj.GetComponent<DKUMACustomization>();
				}
				DK_UMA = GameObject.Find("DK_UMA");
				if ( DK_UMA == null ) {
					var goDK_UMA = new GameObject();	goDK_UMA.name = "DK_UMA";
					DK_UMA = GameObject.Find("DK_UMA");
				}
				EditorVariables.GeneratorPresets = GameObject.Find("Generator Presets");
				if ( EditorVariables.GeneratorPresets == null ) 
				{
					EditorVariables.GeneratorPresets = (GameObject) Instantiate(Resources.Load("Generator Presets"), Vector3.zero, Quaternion.identity);
					EditorVariables.GeneratorPresets.name = "Generator Presets";
					EditorVariables.GeneratorPresets = GameObject.Find("Generator Presets");
					EditorVariables.GeneratorPresets.transform.parent = DK_UMA.transform;
					EditorVariables.PresetToEdit = null;
				}
				#region title
				if ( !EditorVariables.AutoDetLib )using (new Horizontal()) {	
					GUI.color = Color.white;
					GUILayout.Label("Prepare Libraries Tool", "toolbarbutton", GUILayout.ExpandWidth (true));
					GUI.color = Red;
					if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
						ShowLibraries = false;
						
					}
				}
				if ( EditorVariables.AutoDetLib )using (new Horizontal()) {	
					GUI.color = Color.white;
					GUILayout.Label("Auto Detection", "toolbarbutton", GUILayout.ExpandWidth (true));
					GUI.color = Red;
					if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
						ShowLibraries = false;
						EditorVariables.AutoDetLib = false;
					}
				}
				#endregion
				#region Actions	
				GUILayout.Space(5);
				if ( !EditorVariables.AutoDetLib && !choosePlace && !chooseOverlay && !chooseSlot ) using (new Horizontal()) {	
					GUI.color = Color.yellow ;
					if ( Helper ) GUILayout.TextField("The Auto Detect options will be helpfull to setup quickly the Elements and to reset them in a raw. Open the Menu to use it." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				}
				if ( !choosePlace && !chooseOverlay && !chooseSlot ) using (new Horizontal()) {
					if ( !EditorVariables.AutoDetLib && GUILayout.Button ( "Open the 'Auto Detect All' menu", GUILayout.ExpandWidth (true))) {
						EditorVariables.AutoDetLib = true;
						SlotExpressions = GameObject.Find("Slot Expressions");
						#region Is Expressions Library Empty ?
						if ( SlotExpressions == null ) 
						{
							GUI.color = Red;
							GUILayout.TextField("The Expressions Library is umpty. You can populate it automatically with the Elements Prefabs from the DK UMA assets, or you can fill it manually by droping the elements in the Library Inspector." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
							GUI.color = Green;
							if ( GUILayout.Button ( "Populate with Prefabs", GUILayout.ExpandWidth (true))) {
								// Find prefabs
								DirectoryInfo dir = new DirectoryInfo("Assets/DK Editors/DK_UMA_Editor/Prefabs/DK_SlotExpressions");
								// Assign Prefabs
								FileInfo[] info = dir.GetFiles("*.prefab");
								info.Select(f => f.FullName).ToArray();
								// Action
								// Verify Folders objects
								DK_UMA = GameObject.Find("DK_UMA");
								SlotExpressions = GameObject.Find("Slot Expressions");
								if ( DK_UMA == null ) {
									var goDK_UMA = new GameObject();	goDK_UMA.name = "DK_UMA";
									var goSlotExpressions = new GameObject();	goSlotExpressions.name = "Slot Expressions";
									goSlotExpressions.transform.parent = goDK_UMA.transform;
								}
								else if ( SlotExpressions == null ) {
									SlotExpressions = new GameObject();	SlotExpressions.name = "Slot Expressions";
									SlotExpressions.transform.parent = DK_UMA.transform;
								}
								SlotExpressions = GameObject.Find("Slot Expressions");
								foreach (FileInfo f in info)
								{
									// Instantiate the Element Prefab
									GameObject clone = PrefabUtility.InstantiateAttachedAsset( Resources.LoadAssetAtPath("Assets/DK Editors/DK_UMA_Editor/Prefabs/DK_SlotExpressions/"+ f.Name, typeof(GameObject)) ) as GameObject;
									clone.name = f.Name;
									if ( clone.name.Contains(".prefab")) clone.name = clone.name.Replace(".prefab", "");
									foreach ( Transform child in SlotExpressions.transform )
									{
										if ( clone != null && clone.name == child.name ) GameObject.DestroyImmediate( clone );
									}
									if ( clone != null )
									{
										clone.transform.parent = SlotExpressions.transform;
										// add the Element to the List		
										DK_SlotExpressionsElement _SlotExpressionsElement =  clone.GetComponent<DK_SlotExpressionsElement>();
										_SlotExpressionsElement.dk_SlotExpressionsElement.dk_SlotExpressionsName = clone.name;
										if ( _SlotExpressionsElement.dk_SlotExpressionsElement.ElemAlreadyIn == false )
										{
	/*										 for (int i = 0; i < _SlotExpressionsLibrary.dk_SlotExpressionsElementList.Length ; i++)
									        {
									            if (_SlotExpressionsLibrary.dk_SlotExpressionsElementList[i].dk_SlotExpressionsElement != null && _SlotExpressionsLibrary.dk_SlotExpressionsElementList[i].dk_SlotExpressionsElement.ElemAlreadyIn == true )
									            {
		
									            }
									        }
											if ( _SlotExpressionsElement.dk_SlotExpressionsElement.ElemAlreadyIn == false )
											{
										        var list = new DK_SlotExpressionsElement[_SlotExpressionsLibrary.dk_SlotExpressionsElementList.Length + 1];
										        Array.Copy(_SlotExpressionsLibrary.dk_SlotExpressionsElementList, list, _SlotExpressionsLibrary.dk_SlotExpressionsElementList.Length );
										        list[_SlotExpressionsLibrary.dk_SlotExpressionsElementList.Length] = _SlotExpressionsElement;
										        _SlotExpressionsLibrary.dk_SlotExpressionsElementList = list;
										       	if ( !ElemAlreadyIn )_SlotExpressionsLibrary.dk_SlotExpressionsDictionary.Add(_SlotExpressionsElement.dk_SlotExpressionsElement.dk_SlotExpressionsName, _SlotExpressionsElement.dk_SlotExpressionsElement);
												_SlotExpressionsElement.dk_SlotExpressionsElement.ElemAlreadyIn = true;
											}*/
										}
									}
									PrefabUtility.SetPropertyModifications(clone,PrefabUtility.GetPropertyModifications(Resources.LoadAssetAtPath("Assets/DK Editors/DK_UMA_Editor/Prefabs/DK_SlotExpressions/"+ clone.name, typeof(UnityEngine.Object))));
									
								}
							}
						}
						#endregion Is Expressions Library Empty ?

					}
				}
				#endregion

				#region help
			
				#endregion


				#region AutoDetect Libraries
				if ( EditorVariables.AutoDetLib )
				{
					GUI.color = Color.white;
					using (new ScrollView(ref scroll)) 	{
						GUI.color = Color.yellow;
						if ( Helper )
							GUILayout.TextField("The Auto Detection uses the Expression system, it will compare your new DK elements with the Expression library to try to setup the element. Verify that your Expression library is populated." , 500, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						GUI.color = Green;
						if ( GUILayout.Button ( "Open the Expressions window", GUILayout.ExpandWidth (true))) {
							OpenExpressionsWin();
						}

						// prepare
						GUI.color = Color.yellow;
						if ( Helper )
							GUILayout.TextField("You have to convert your UMA elements for DK to be able to use them." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						GUI.color = Green;
						if ( GUILayout.Button ( "Open the UMA to DK converter", GUILayout.ExpandWidth (true))) {
							OpenAutoDetectWin();
						}

						GUI.color = Color.white;
						GUILayout.Label ( "Libraries", "toolbarbutton", GUILayout.ExpandWidth (true));
						GUI.color = Color.white ;
						using (new Horizontal()) {
							if ( AddRaces ) GUI.color = Green;
							else GUI.color = Color.gray;
						//	if ( GUILayout.Button ( "Add to", GUILayout.ExpandWidth (false))) {
						//		if ( AddRaces ) AddRaces = false;
						//		else AddRaces = true;
						//	}
							GUI.color = Color.yellow;
							GUILayout.Label ( "Races", GUILayout.Width (60));
							GUI.color = Color.white;
								if ( EditorVariables.DK_UMACrowd && EditorVariables.DK_UMACrowd.raceLibrary ) GUILayout.TextField (EditorVariables.DK_UMACrowd.raceLibrary.name, GUILayout.ExpandWidth (true));
								if ( EditorVariables.DK_UMACrowd && EditorVariables.DK_UMACrowd.raceLibrary 
							    && GUILayout.Button ( "Change", GUILayout.Width (60))){
								DK_UMA_Editor.OpenLibrariesWindow();
								ChangeLibrary.CurrentLibN = EditorVariables.DK_UMACrowd.raceLibrary.name;
								ChangeLibrary.CurrentLibrary = EditorVariables.DK_UMACrowd.raceLibrary.gameObject;
								ChangeLibrary.Action = "";
							}
						}
						using (new Horizontal()) {
							if ( AddSlots ) GUI.color = Green;
							else GUI.color = Color.gray;
						//	if ( GUILayout.Button ( "Add to", GUILayout.ExpandWidth (false))) {
						//		if ( AddSlots ) AddSlots = false;
						//		else AddSlots = true;
						//	}
							GUI.color = Color.yellow;
							GUILayout.Label ( "Slots", GUILayout.Width (60));
							GUI.color = Color.white;
							if ( EditorVariables.DK_UMACrowd && EditorVariables.DK_UMACrowd.slotLibrary ) GUILayout.TextField ( EditorVariables.DK_UMACrowd.slotLibrary.name, GUILayout.ExpandWidth (true));
							if ( EditorVariables.DK_UMACrowd && EditorVariables.DK_UMACrowd.slotLibrary 
							    && GUILayout.Button ( "Change", GUILayout.Width (60))){
								DK_UMA_Editor.OpenLibrariesWindow();
								ChangeLibrary.CurrentLibN = EditorVariables.DK_UMACrowd.slotLibrary.name;
								ChangeLibrary.CurrentLibrary = EditorVariables.DK_UMACrowd.slotLibrary.gameObject;
								ChangeLibrary.Action = "";
							}
						}
						using (new Horizontal()) {
							if ( AddOverlays ) GUI.color = Green;
							else GUI.color = Color.gray;
						//	if ( GUILayout.Button ( "Add to", GUILayout.ExpandWidth (false))) {
						//		if ( AddOverlays ) AddOverlays = false;
						//		else AddOverlays = true;
						//	}
							GUI.color = Color.yellow;
							GUILayout.Label ( "Overlays", GUILayout.Width (60));
							GUI.color = Color.white;
							if ( EditorVariables.DK_UMACrowd && EditorVariables.DK_UMACrowd.overlayLibrary ) GUILayout.TextField ( EditorVariables.DK_UMACrowd.overlayLibrary.name, GUILayout.ExpandWidth (true));
							if ( EditorVariables.DK_UMACrowd && EditorVariables.DK_UMACrowd.overlayLibrary 
							    && GUILayout.Button ( "Change", GUILayout.Width (60))){
								DK_UMA_Editor.OpenLibrariesWindow();
								ChangeLibrary.CurrentLibN = EditorVariables.DK_UMACrowd.overlayLibrary.name;
								ChangeLibrary.CurrentLibrary = EditorVariables.DK_UMACrowd.overlayLibrary.gameObject;
								ChangeLibrary.Action = "";
							}
						}

						#region choices
						GUI.color = Color.white;
						GUILayout.Label ( "Settings", "toolbarbutton", GUILayout.ExpandWidth (true));
						if ( Helper ){
							GUI.color = Color.yellow;
							GUILayout.TextField("You can auto detect the Element's Race, Gender, Place and more. Select the settings." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						}
						using (new Horizontal()) {	
							if ( EditorVariables.DetRace ) GUI.color = Green;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Race", GUILayout.ExpandWidth (true))) {
								if ( EditorVariables.DetRace ) EditorVariables.DetRace = false;
								else EditorVariables.DetRace = true;
							}
							if ( EditorVariables.DetGender ) GUI.color = Green;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Gender", GUILayout.ExpandWidth (true))) {
								if ( EditorVariables.DetGender ) EditorVariables.DetGender = false;
								else EditorVariables.DetGender = true;
							}
							if ( EditorVariables.DetPlace ) GUI.color = Green;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Place", GUILayout.ExpandWidth (true))) {
								if ( EditorVariables.DetPlace ) EditorVariables.DetPlace = false;
								else EditorVariables.DetPlace = true;
							}
							if ( EditorVariables.DetOvType ) GUI.color = Green;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Type", GUILayout.ExpandWidth (true))) {
								if ( EditorVariables.DetOvType ) EditorVariables.DetOvType = false;
								else EditorVariables.DetOvType = true;
							}
							if ( EditorVariables.DetWWeight ) GUI.color = Green;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Weight", GUILayout.ExpandWidth (true))) {
								if ( EditorVariables.DetWWeight ) EditorVariables.DetWWeight = false;
								else EditorVariables.DetWWeight = true;
							}
							if ( EditorVariables.DetLink ) GUI.color = Green;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Link", GUILayout.ExpandWidth (true))) {
								if ( EditorVariables.DetLink ) EditorVariables.DetLink = false;
								else EditorVariables.DetLink = true;
							}
						}
						#endregion

						
						using (new Horizontal()) {	
							GUI.color = Color.white;
							GUILayout.Label ( "Detect for :", GUILayout.Width (80));
							if ( EditorVariables.DetRaces ) GUI.color = Green;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Races", GUILayout.ExpandWidth (true))) {
								if ( EditorVariables.DetRaces ) EditorVariables.DetRaces = false;
								else EditorVariables.DetRaces = true;
							}
								if ( EditorVariables.DetSlots ) GUI.color = Green;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Slots", GUILayout.ExpandWidth (true))) {
								if ( EditorVariables.DetSlots ) EditorVariables.DetSlots = false;
								else EditorVariables.DetSlots = true;
							}
								if ( EditorVariables.DetOverlay ) GUI.color = Green;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Overlays", GUILayout.ExpandWidth (true))) {
								if ( EditorVariables.DetOverlay ) EditorVariables.DetOverlay = false;
								else EditorVariables.DetOverlay = true;
							}

						}

						#region Races selection
						GUILayout.Space(5);
						using (new Horizontal()) {
							GUI.color = Color.white;
							GUILayout.Label ( "Race Section", "toolbarbutton", GUILayout.ExpandWidth (true));
						}
						if ( Helper )
						{
							GUILayout.Space(5);
							GUI.color = Color.yellow;
							GUILayout.TextField("You can assign the Races during the detection. Select the races to assign." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						}
						using (new Horizontal()) {	
							GUI.color = Color.white;
							if ( GUILayout.Button ( "Select Race(s)", GUILayout.ExpandWidth (true))) {
								OpenRaceSelectEditor();
							}
						}
						using (new Horizontal()) {	
							GUI.color = Color.white;
							GUILayout.Label("Race(s) to Apply:", GUILayout.ExpandWidth (false));
						}
						using (new Horizontal()) {
							if ( EditorVariables.NewRaceName != "" ) GUI.color = Green;
							else GUI.color = Red;
							GUILayout.TextField(EditorVariables.NewRaceName, 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
							
						}
						GUILayout.Space(5);
						using (new Horizontal()) {
							GUI.color = Color.white;
							GUILayout.Label ( "Apply options", "toolbarbutton", GUILayout.ExpandWidth (true));
						}
						if ( Helper )
						{
							GUI.color = Color.yellow;
							GUILayout.TextField("Select the target of the Auto Detection." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						}
						using (new Horizontal()) {	
							GUILayout.Label("Apply to:", GUILayout.ExpandWidth (false));
							if ( !EditorVariables.ApplyToSelection  ) GUI.color = Color.gray;
							else GUI.color = Green;
							if ( GUILayout.Button ( "Selection", GUILayout.ExpandWidth (true))) {
								EditorVariables.ApplyToSelection = true;
								EditorVariables.ApplyToAll = false;
								EditorVariables.ApplyToEmpty = false;
							}
							if ( !EditorVariables.ApplyToAll  ) GUI.color = Color.gray;
							else GUI.color = Green;
							if ( GUILayout.Button ( "All", GUILayout.ExpandWidth (true))) {
								EditorVariables.ApplyToAll = true;
								EditorVariables.ApplyToSelection = false;
								EditorVariables.ApplyToEmpty = false;
							}
							if ( !EditorVariables.ApplyToEmpty  ) GUI.color = Color.gray;
							else GUI.color = Green;
							if ( GUILayout.Button ( "Umpty", GUILayout.ExpandWidth (true))) {
								EditorVariables.ApplyToAll = false;
								EditorVariables.ApplyToSelection = false;
								EditorVariables.ApplyToEmpty = true;
							}
						}

						#endregion
						using (new Horizontal()) {
							#region Search
							using (new Horizontal()) {
								GUI.color = Color.white;
								GUILayout.Label("Search", GUILayout.ExpandWidth (false));
								SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));
								
							}
							#endregion Search
							if ( EditorVariables.SearchResultsOnly ) GUI.color = Color.yellow;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Results Only", GUILayout.ExpandWidth (false))) {
								if ( EditorVariables.SearchResultsOnly ) EditorVariables.SearchResultsOnly = false;
								else EditorVariables.SearchResultsOnly = true;
							}
						}

						#region Launch the Detection
						GUI.color = Green;
						if ( GUILayout.Button ( "Launch the Detection", GUILayout.ExpandWidth (true))) {
								if ( EditorVariables.ApplyToSelection ) DetectAndAddDK.DetectionSelection ();
								else if ( EditorVariables.ApplyToAll ) DetectAndAddDK.DetectionAll ();

						}
						#endregion Launch the Detection
					}
				}
				#endregion
			
				#region choose Places List
				if ( !EditorVariables.AutoDetLib && choosePlace ) using (new Horizontal()) {
					GUILayout.Label("choose Place for the Element", "toolbarbutton", GUILayout.ExpandWidth (true));
					GUI.color = Red;
					if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
						choosePlace = false;
						
					}
				}
				GUI.color = Color.yellow;
				if ( Helper &&!EditorVariables.AutoDetLib && choosePlace ) GUILayout.TextField("To change the place, click on an Anatomy Element (place) from the list bellow. When finished, click on the close Tab Icon ( X )" , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				GUI.color = Color.white;
					
					GUILayout.Space(5);
				if ( !EditorVariables.AutoDetLib && choosePlace ) using (new Horizontal()) {
					GUI.color = Color.white ;
					GUILayout.Label("Anatomy Slot Name", "toolbarbutton", GUILayout.Width (140));
					GUILayout.Label("Race", "toolbarbutton", GUILayout.Width (60));
					GUILayout.Label("Gender", "toolbarbutton", GUILayout.Width (55));
					GUILayout.Label("Overlay Type", "toolbarbutton", GUILayout.Width (90));
					GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
				}
				if ( !EditorVariables.AutoDetLib && choosePlace ) using (new Horizontal()) {
					if ( !EditorVariables.AutoDetLib && choosePlace ) using (new ScrollView(ref scroll)) 	{
						EditorVariables.SlotsAnatomyLibraryObj = GameObject.Find("DK_SlotsAnatomyLibrary");
						EditorVariables._SlotsAnatomyLibrary =  EditorVariables.SlotsAnatomyLibraryObj.GetComponent<DK_SlotsAnatomyLibrary>();
										
						for(int i = 0; i < EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length; i ++){
							using (new Horizontal(GUILayout.Width (80))) {
								// Element
								if (  EditorVariables.SelectedElemPlace ==  EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i] ) GUI.color = Color.yellow ;
								else GUI.color = Color.white ;
								if (GUILayout.Button ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.dk_SlotsAnatomyName , Slim, GUILayout.Width (140))) {
									 EditorVariables.SelectedElemPlace = EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i];
									EditorUtility.SetDirty(DK_Race.gameObject);
									AssetDatabase.SaveAssets();
								}
								// Race
								if ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.GetComponent<DK_Race>() as DK_Race == null ) {
									EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.AddComponent<DK_Race>();	
								}
								DK_Race = EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].GetComponent("DK_Race") as DK_Race;
								if ( DK_Race.Race.Count == 0 ) GUI.color = Red;
								if ( DK_Race.Race.Count == 0 && GUILayout.Button ( "No Race" , Slim, GUILayout.Width (50))) {
									
								}
								GUI.color = Green;
								if ( DK_Race.Race.Count == 1 && GUILayout.Button ( DK_Race.Race[0] , Slim, GUILayout.Width (50))) {
									
								}
								if ( DK_Race.Race.Count > 1 && GUILayout.Button ( "Multi" , Slim, GUILayout.Width (50))) {
									
								}
								// Gender
								if ( DK_Race.Gender == "" ) GUI.color = Red;
								if ( DK_Race.Gender == "" ) GUILayout.Label ( "N" , "Button") ;
								GUI.color = Green;
								if ( DK_Race.Gender != "" && DK_Race.Gender == "Female" ) GUILayout.Label ( "Female" , Slim, GUILayout.Width (50) );
								if ( DK_Race.Gender != "" && DK_Race.Gender == "Male" ) GUILayout.Label ( "Male" , Slim, GUILayout.Width (50) );
								if ( DK_Race.Gender != "" && DK_Race.Gender == "Both" ) GUILayout.Label ( "Both" , Slim, GUILayout.Width (50) );
								
								// OverlayType
								if ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.GetComponent<DK_Race>() as DK_Race == null ) {
									EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.AddComponent<DK_Race>();	
								}
								DK_Race = EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].GetComponent("DK_Race") as DK_Race;
								if ( DK_Race.OverlayType == "" ) GUI.color = Red;
								if ( DK_Race.OverlayType == "" && GUILayout.Button ( "No Race" , Slim, GUILayout.Width (50))) {
									
								}
								GUI.color = Green;
								if ( DK_Race.OverlayType != "" && GUILayout.Button ( DK_Race.OverlayType , Slim, GUILayout.Width (50))) {
									
								}
							}
						}
					}
				}
				#endregion choose Places List
			}
			#endregion Prepare Libraries
			
			#region Generator Presets
			if ( ShowGenPreset ) 
			{
				#region Edit Generator Preset
				if ( EditGenPreset )
				{
					// variables
					 DK_UMA = GameObject.Find("DK_UMA");
					if ( DK_UMA == null ) {
						var goDK_UMA = new GameObject();	goDK_UMA.name = "DK_UMA";
						DK_UMA = GameObject.Find("DK_UMA");
					}
					EditorVariables.GeneratorPresets = GameObject.Find("Generator Presets");
					if ( EditorVariables.GeneratorPresets == null ) 
					{
						EditorVariables.GeneratorPresets = (GameObject) Instantiate(Resources.Load("Generator Presets"), Vector3.zero, Quaternion.identity);
						EditorVariables.GeneratorPresets.name = "Generator Presets";
						EditorVariables.GeneratorPresets = GameObject.Find("Generator Presets");
						EditorVariables.GeneratorPresets.transform.parent = DK_UMA.transform;
					}
					
					#region Menu
					// Title
					GUILayout.Space(5);
					using (new Horizontal()) {
						GUI.color = Color.white ;
						GUILayout.Label("Edit Generator Presets", "toolbarbutton", GUILayout.ExpandWidth (true));
						GUI.color = Red;
						// actions
						if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
							ShowGenPreset = false;
							EditGenPreset = false;
							EditorVariables.PresetToEdit = null;
							
						}
					}
					#endregion Menu
					
					#region Presets List
					if ( !EditorVariables.PresetToEdit )
					{
						GUI.color = Color.yellow;
						if ( Helper ) GUILayout.TextField("Select a Generator Preset in the List to edit it. You also can define a Preset to be the Active one for the Generator process to use it during the Model Creation, click on a 'Active' button." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						GUILayout.Space(5);
						// list
						GUI.color = Color.white ;
						GUILayout.Label("Presets List", "toolbarbutton", GUILayout.ExpandWidth (true));
						GUILayout.Space(5);
						using (new Horizontal()) {
							GUI.color = Color.white ;
							GUILayout.Label("Preset Name", "toolbarbutton", GUILayout.Width (165));
							GUILayout.Label("Gender", "toolbarbutton", GUILayout.Width (60));
							GUILayout.Label("Activated", "toolbarbutton", GUILayout.Width (60));
							GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
						}
						using (new ScrollView(ref scroll)) 	{
							foreach ( Transform Preset in EditorVariables.GeneratorPresets.transform )
							{
								if ( Preset != null ) using (new Horizontal()) {
									// destroy it
									GUI.color = Red;
									if ( Preset != null && GUILayout.Button ( "X", GUILayout.ExpandWidth (false))) {
										DestroyImmediate ( Preset.gameObject ); 
									}
									// Select it
									GUI.color = Color.white ;
									if ( Preset != null && GUILayout.Button ( Preset.name, "Label",GUILayout.Width (140))) {
										EditorVariables.PresetToEdit = Preset.gameObject;
										_GeneratorPresetLibrary =  EditorVariables.PresetToEdit.GetComponent<DK_GeneratorPresetLibrary>();
										NewPresetGender = _GeneratorPresetLibrary.ToGender;
										NewPresetName = _GeneratorPresetLibrary.PresetName;
										// merge lists
										EditorVariables.SlotsAnatomyLibraryObj = GameObject.Find("DK_SlotsAnatomyLibrary");
										EditorVariables._SlotsAnatomyLibrary =  EditorVariables.SlotsAnatomyLibraryObj.GetComponent<DK_SlotsAnatomyLibrary>();
										for(int i = 0; i < EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length; i ++){
											if (EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i] )
											{
												EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.Selected = false;
												for(int i2 = 0; i2 < _GeneratorPresetLibrary.dk_SlotsAnatomyElementList.Length; i2 ++){
													if ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i] != null
														&& EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement != null 
													    && _GeneratorPresetLibrary.dk_SlotsAnatomyElementList[i2] != null
														&& _GeneratorPresetLibrary.dk_SlotsAnatomyElementList[i2].dk_SlotsAnatomyElement != null
														&& EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement 
														== _GeneratorPresetLibrary.dk_SlotsAnatomyElementList[i2].dk_SlotsAnatomyElement )
													{
														EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.Selected = true;
													}
												}
											}
										}
									}
									if ( Preset ) { _GeneratorPresetLibrary2 = Preset.GetComponent<DK_GeneratorPresetLibrary>();}
									if ( _GeneratorPresetLibrary2 != null )
									{
										// Gender
										GUI.color = Color.yellow ;
										GUILayout.Label(_GeneratorPresetLibrary2.ToGender, Slim, GUILayout.Width (55));
										// Active
										if ( _GeneratorPresetLibrary2.IsActivePreset ) GUI.color = Green;
										else GUI.color = Color.gray ;
										if ( GUILayout.Button ( "Active", GUILayout.Width (60))) {
											foreach ( Transform Presets in EditorVariables.GeneratorPresets.transform )
											{
												DK_GeneratorPresetLibrary _GeneratorPresetLibrarys = Presets.GetComponent<DK_GeneratorPresetLibrary>();
												_GeneratorPresetLibrarys.IsActivePreset = false;
											}
											if ( _GeneratorPresetLibrary2.IsActivePreset ) _GeneratorPresetLibrary2.IsActivePreset = false;
											else _GeneratorPresetLibrary2.IsActivePreset = true;
											Selection.activeGameObject = Preset.gameObject;
										}
									}
								}
							}
						}
					}
					#endregion Presets List
					
					#region Edit Preset
					if ( EditorVariables._SlotsAnatomyLibrary && EditorVariables.PresetToEdit != null ) {
						GUILayout.Space(5);
						using (new Horizontal()) {
							GUI.color = Color.white ;
							GUILayout.Label("Editing Preset '" +EditorVariables.PresetToEdit.name+"'", "toolbarbutton", GUILayout.ExpandWidth (true));
							GUI.color = Red;
							// actions
							if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
								EditorVariables.PresetToEdit = null;
								for(int i = 0; i < EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length; i ++){
									if ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i] != null)
									EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.Selected = false;
								}
							}
						}
							Selection.activeGameObject = EditorVariables.PresetToEdit;	
						// general help	
						if ( Helper )	
						{
							GUI.color = Color.white;
							GUILayout.TextField("Here you can Edit the selected Generator Preset and manage its Slot Elements." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
							GUI.color = Color.yellow;
							GUILayout.TextField("Change the Preset's Name in the field bellow." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						}
						// Preset Name
						using (new Horizontal()) {
							GUI.color = Color.white;
							GUILayout.Label("Preset Name :", GUILayout.Width (100));
								if ( EditorVariables.UMAObj == null ) 
							{
									UMAObjName = EditorVariables.UMAObjDefault;
									EditorVariables.UMAObj =  GameObject.Find(UMAObjName);
							}
							if ( NewPresetName != "" ) GUI.color = Green;
							else GUI.color = Red;
							NewPresetName = GUILayout.TextField(NewPresetName, 50, GUILayout.ExpandWidth (true));
						}
						// Gender
						GUI.color = Color.yellow;
						if ( Helper ) GUILayout.TextField("You can specify a Gender or let it be usable by both (Both is recommended)." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						using (new Horizontal()) {
							if ( NewPresetGender == "Both" ) GUI.color = Green;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Both", GUILayout.ExpandWidth (true))) {
								NewPresetGender = "Both";
							}
							if ( NewPresetGender == "Female" ) GUI.color = Green;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Female", GUILayout.ExpandWidth (true))) {
								NewPresetGender = "Female";
							}
							if ( NewPresetGender == "Male" ) GUI.color = Green;
							else GUI.color = Color.gray;
							if ( GUILayout.Button ( "Male", GUILayout.ExpandWidth (true))) {
								NewPresetGender = "Male";
							}
						}
						if ( Helper )	
						{
							GUI.color = Color.yellow;
							GUILayout.TextField("Make your selections then click on the 'Apply to Preset' button." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						}
						// Apply to generator Preset	
						using (new Horizontal()) {
							GUI.color = Color.white ;
							if ( GUILayout.Button ( "Apply to Preset", GUILayout.ExpandWidth (true))) {
									EditorOptions.ApplyToGeneratorPreset ();
							}
						}
						if ( Helper )	
						{
							GUI.color = Color.white;
							GUILayout.TextField("The list is displaying both Elements of the Preset to Edit and from the Slots Anatomy Library. You can select or unselect any Anatomy Element." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						}
							#region List
						GUILayout.Space(5);
						using (new Horizontal()) {
							GUI.color = Color.white ;
							GUILayout.Label("Anatomy Slot Name", "toolbarbutton", GUILayout.Width (140));
							GUILayout.Label("Race", "toolbarbutton", GUILayout.Width (60));
							GUILayout.Label("Gender", "toolbarbutton", GUILayout.Width (55));
							GUILayout.Label("Select", "toolbarbutton", GUILayout.Width (55));
							GUILayout.Label("Overlay type", "toolbarbutton", GUILayout.Width (75));
							GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
						}
						using (new Horizontal()) {
							if ( EditorVariables._SlotsAnatomyLibrary != null ) using (new ScrollView(ref scroll)) 	{
								for(int i = 0; i < EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length; i ++){
									if ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i] != null) using (new Horizontal(GUILayout.Width (80))) {
										// Element
										GUI.color = Color.white ;
										if ( GUILayout.Button ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.dk_SlotsAnatomyName , Slim, GUILayout.Width (140))) {
										}
										// Race
										if ( EditorVariables._SlotsAnatomyLibrary && EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.GetComponent<DK_Race>() as DK_Race == null ) {
											EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.AddComponent<DK_Race>();	
										}
										DK_Race DK_Race;
										DK_Race = EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].GetComponent("DK_Race") as DK_Race;
										if ( DK_Race.Race.Count == 0) GUI.color = Red;
										if ( DK_Race.Race.Count == 0 && GUILayout.Button ( "No Race" , Slim, GUILayout.Width (50))) {
											
										}
										GUI.color = Green;
										if ( DK_Race.Race.Count == 1 && GUILayout.Button ( DK_Race.Race[0] , Slim, GUILayout.Width (50))) {
											
										}
										if ( DK_Race.Race.Count > 1 && GUILayout.Button ( "Multi" , Slim, GUILayout.Width (50))) {
											
										}	
										// Gender
										if ( DK_Race.Gender == "" ) GUI.color = Red;
										if ( DK_Race.Gender == "" ) GUILayout.Label ( "N" , "Button") ;
										GUI.color = Green;
										if ( DK_Race.Gender != "" && DK_Race.Gender == "Female" ) GUILayout.Label ( "Female" , Slim, GUILayout.Width (50) );
										if ( DK_Race.Gender != "" && DK_Race.Gender == "Male" ) GUILayout.Label ( "Male" , Slim, GUILayout.Width (50) );
										if ( DK_Race.Gender != "" && DK_Race.Gender == "Both" ) GUILayout.Label ( "Both" , Slim, GUILayout.Width (50) );
										// selected
										if ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.Selected == true )GUI.color = Green;
										else GUI.color = Color.gray;
										if ( GUILayout.Button ( "Select" , GUILayout.Width (50)) ) {
											if ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.Selected == false ) EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.Selected = true ;
											else EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.Selected = false;
										}
										// OverlayType
										if ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.GetComponent<DK_Race>() as DK_Race == null ) {
											EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.AddComponent<DK_Race>();	
										}
										DK_Race = EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].GetComponent("DK_Race") as DK_Race;
										if ( DK_Race.OverlayType == "" ) GUI.color = Color.yellow;
										if ( DK_Race.OverlayType == "" && GUILayout.Button ( "No Overlay" , Slim, GUILayout.Width (70))) {
											
										}
										GUI.color = Green;
										if ( DK_Race.OverlayType != "" && GUILayout.Button ( DK_Race.OverlayType , Slim, GUILayout.Width (70))) {
											
										}	
									}
								}
							}
							Selection.activeGameObject = EditorVariables.PresetToEdit;
						}
						#endregion List
					}
					#endregion Edit Preset
				}
				#endregion Edit Generator Preset
				
				#region New Generator Preset
				if ( NewGenPreset )
				{
					#region Menu
					// title	
					GUILayout.Space(5);
					using (new Horizontal()) {
						GUI.color = Color.white ;
						GUILayout.Label("New Generator Preset", "toolbarbutton", GUILayout.ExpandWidth (true));
						GUI.color = Red;
						if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
							ShowGenPreset = false;
							NewGenPreset = false;
						}
					}
					
					// general help	
					GUI.color = Color.white;
					if ( Helper )	
					{
						GUILayout.TextField("Here you can Create a new Generator Preset and add the required Slot Elements to it." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						GUI.color = Color.yellow;
						GUILayout.TextField("Write the new Preset's Name in the field bellow." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					}
					// Preset Name
					using (new Horizontal()) {
						GUI.color = Color.white;
						GUILayout.Label("Preset Name :", GUILayout.Width (100));
							if ( EditorVariables.UMAObj == null ) 
						{
								UMAObjName = EditorVariables.UMAObjDefault;
								EditorVariables.UMAObj =  GameObject.Find(UMAObjName);
						}
						if ( NewPresetName != "" ) GUI.color = Green;
						else GUI.color = Red;
						NewPresetName = GUILayout.TextField(NewPresetName, 50, GUILayout.ExpandWidth (true));
					}
					// Gender
					GUI.color = Color.yellow;
					if ( Helper ) GUILayout.TextField("You can specify a Gender or let it be usable by both (Both is recommended)." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					using (new Horizontal()) {
						if ( NewPresetGender == "Both" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Both", GUILayout.ExpandWidth (true))) {
							NewPresetGender = "Both";
						}
						if ( NewPresetGender == "Female" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Female", GUILayout.ExpandWidth (true))) {
							NewPresetGender = "Female";
						}
						if ( NewPresetGender == "Male" ) GUI.color = Green;
						else GUI.color = Color.gray;
						if ( GUILayout.Button ( "Male", GUILayout.ExpandWidth (true))) {
							NewPresetGender = "Male";
						}
					}
					GUI.color = Color.yellow;
					if ( Helper ) GUILayout.TextField("Select the desired Slots in the following List, the click on the 'Create Preset' button." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					using (new Horizontal()) {
						// Create
						GUI.color = Color.white;
						if ( GUILayout.Button ( "Create Preset", GUILayout.ExpandWidth (true))) {
							EditorOptions.CreateGeneratorPreset ();
							NewGenPreset = false;
						}
						// Reset
						GUI.color = Color.yellow;
						if ( GUILayout.Button ( "Reset Preset", GUILayout.ExpandWidth (true))) {
								for(int i = 0; i < EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length; i ++){
								EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.Selected = false;
							}
						}
					}
					#endregion Menu
					
					#region List
					GUILayout.Space(5);
					using (new Horizontal()) {
						GUI.color = Color.white ;
						GUILayout.Label("Anatomy Slot Name", "toolbarbutton", GUILayout.Width (140));
						GUILayout.Label("Race", "toolbarbutton", GUILayout.Width (60));
						GUILayout.Label("Gender", "toolbarbutton", GUILayout.Width (55));
						GUILayout.Label("Select", "toolbarbutton", GUILayout.Width (55));
						GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
					}
					using (new Horizontal()) {
						using (new ScrollView(ref scroll)) 	{
							for(int i = 0; i < EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList.Length; i ++){
								using (new Horizontal(GUILayout.Width (80))) {
									// Element
									GUI.color = Color.white ;
									if (GUILayout.Button ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.dk_SlotsAnatomyName , Slim, GUILayout.Width (140))) {
									}
									// Race
									if ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.GetComponent<DK_Race>() as DK_Race == null ) {
										EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.AddComponent<DK_Race>();	
									}
									DK_Race DK_Race;
									DK_Race = EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].GetComponent("DK_Race") as DK_Race;
									if ( DK_Race.Race.Count == 0 ) GUI.color = Red;
									if ( DK_Race.Race.Count == 0 && GUILayout.Button ( "No Race" , Slim, GUILayout.Width (50))) {
										
									}
									GUI.color = Green;
									if ( DK_Race.Race.Count == 1 && GUILayout.Button ( DK_Race.Race[0] , Slim, GUILayout.Width (50))) {
										
									}
									if ( DK_Race.Race.Count > 1 && GUILayout.Button ( "Multi" , Slim, GUILayout.Width (50))) {
										
									}
									// Gender
									if ( DK_Race.Gender == "" ) GUI.color = Red;
									if ( DK_Race.Gender == "" ) GUILayout.Label ( "N" , "Button") ;
									GUI.color = Green;
									if ( DK_Race.Gender != "" && DK_Race.Gender == "Female" ) GUILayout.Label ( "Female" , Slim, GUILayout.Width (50) );
									if ( DK_Race.Gender != "" && DK_Race.Gender == "Male" ) GUILayout.Label ( "Male" , Slim, GUILayout.Width (50) );
									if ( DK_Race.Gender != "" && DK_Race.Gender == "Both" ) GUILayout.Label ( "Both" , Slim, GUILayout.Width (50) );
									
									if ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.Selected == true )GUI.color = Green;
									else GUI.color = Color.gray;
									if ( GUILayout.Button ( "Select" , GUILayout.Width (50)) ) {
										if ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.Selected == false ) EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.Selected = true ;
										else EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].dk_SlotsAnatomyElement.Selected = false;
									}
									// OverlayType
									if ( EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.GetComponent<DK_Race>() as DK_Race == null ) {
										EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].gameObject.AddComponent<DK_Race>();	
									}
									DK_Race = EditorVariables._SlotsAnatomyLibrary.dk_SlotsAnatomyElementList[i].GetComponent("DK_Race") as DK_Race;
									if ( DK_Race.OverlayType == "" ) GUI.color = Red;
									if ( DK_Race.OverlayType == "" && GUILayout.Button ( "No Race" , Slim, GUILayout.Width (50))) {
										
									}
									GUI.color = Green;
									if ( DK_Race.OverlayType != "" && GUILayout.Button ( DK_Race.OverlayType , Slim, GUILayout.Width (50))) {
										
									}
								}
							}
						}
						Selection.activeGameObject = EditorVariables.New_DK_GeneratorPresetLibraryObj;
					}
					#endregion List
				}
				#endregion New Generator Preset
			}
			
			#endregion Generator Presets
		}
		#endregion Prepare
		
		#region Create
		if ( showCreate ) {

				DK_UMACrowd _DK_UMACrowd = GameObject.Find("DKUMACrowd").GetComponent<DK_UMACrowd>();
				EditorVariables.DK_UMACrowd = _DK_UMACrowd;
			if ( !EditorVariables._RaceLibrary ) EditorVariables._RaceLibrary =  _DK_UMACrowd.raceLibrary;
			if ( !EditorVariables._DKSlotLibrary ) EditorVariables._DKSlotLibrary = _DK_UMACrowd.slotLibrary;
			if ( !EditorVariables._OverlayLibrary ) EditorVariables._OverlayLibrary =  _DK_UMACrowd.overlayLibrary;
			#region If not ready
			try{
			if ( EditorVariables._RaceLibrary.raceElementList.Length == 0 || EditorVariables._DKSlotLibrary.slotElementList.Length == 0 || EditorVariables._OverlayLibrary.overlayElementList.Length == 0 ) {
				// race library
				if ( EditorVariables._RaceLibrary.raceElementList.Length == 0 ){
					GUI.color = Color.yellow;
					GUILayout.TextField("The DK Races Library is umpty." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					using (new Horizontal()) {
						if ( GUILayout.Button ( "Find all DK races or Convert UMA Races to DK.", GUILayout.ExpandWidth (true))) {
							
						}
					}
				}
				
				// slot library
				if ( EditorVariables._DKSlotLibrary.slotElementList.Length == 0 ){
					GUI.color = Color.yellow;
					GUILayout.TextField("The DK Slots Library is umpty." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					using (new Horizontal()) {
						if ( GUILayout.Button ( "Find all DK Slots or Convert UMA Slots to DK.", GUILayout.ExpandWidth (true))) {
							
						}
					}
				}

				// overlay library
				if ( EditorVariables._OverlayLibrary.overlayElementList.Length == 0 ){
					GUI.color = Color.yellow;
					GUILayout.TextField("The DK Overlays Library is umpty." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					using (new Horizontal()) {
						if ( GUILayout.Button ( "Find all DK Overlays or Convert UMA Overlays to DK.", GUILayout.ExpandWidth (true))) {
							
						}
					}
				}
				
			}
			if ( EditorVariables._RaceLibrary.raceElementList.Length > 0 && EditorVariables._DKSlotLibrary.slotElementList.Length > 0 && EditorVariables._OverlayLibrary.overlayElementList.Length > 0  )
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUI.color = Color.yellow;
				if(GUILayout.Button("Reset Generator")){
							EditorVariables.DK_UMACrowd.generateUMA = false;
							EditorVariables.DK_UMACrowd.generateLotsUMA = false;
							EditorVariables.DK_UMACrowd.UMAGenerated = true;
					EditorVariables.DK_DKUMAGenerator.umaDirtyList.Clear();
					GL.Clear( true,  true, Color.black, 1.0f);
					Resources.UnloadUnusedAssets();
				}
			}

			if ( EditorVariables._RaceLibrary.raceElementList.Length > 0 && EditorVariables._DKSlotLibrary.slotElementList.Length > 0 && EditorVariables._OverlayLibrary.overlayElementList.Length > 0  )
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("Default Name :", GUILayout.ExpandWidth (false));
				if ( EditorVariables.DName == null ) EditorVariables.DName = "";
				EditorVariables.DName = GUILayout.TextField(EditorVariables.DName, 100, GUILayout.ExpandWidth (true));
				if ( EditorVariables.DName != DK_UMACrowd.DName && GUILayout.Button("Apply", GUILayout.ExpandWidth (false))) {
					DK_UMACrowd.DName = EditorVariables.DName;
				}
				if ( DK_UMACrowd.PlusID == true ) GUI.color = Green;
				else GUI.color = Color.gray;
				if ( GUILayout.Button("+ID", GUILayout.ExpandWidth (false))) {
					if ( DK_UMACrowd.PlusID == true ) DK_UMACrowd.PlusID = false;
					else DK_UMACrowd.PlusID = true;
				}
			}
			}catch(NullReferenceException){}

			if ( EditorVariables.UMACrowdObj && EditorVariables.DK_UMACrowd == null ){
				DetectAndAddDK.DetectAll();
					Debug.Log ( "debug");
			}
				//	if ( EditorVariables.DK_UMACrowd && EditorVariables.DK_UMACrowd.GeneratorMode != "Preset" )EditorVariables.DK_UMACrowd.GeneratorMode = "Preset"; 
			#region Create buttons list

			#endregion If not ready
			using (new Horizontal()) {
				if ( DK_RPG_UMA_Generator.AddToRPG == true ) GUI.color = Green;
				else GUI.color = Color.gray;
				if ( GUILayout.Button("Is character", GUILayout.ExpandWidth (true))) {
					if ( DK_RPG_UMA_Generator.AddToRPG == true ) DK_RPG_UMA_Generator.AddToRPG = false;
					else DK_RPG_UMA_Generator.AddToRPG = true;
				}
				GUI.color = Color.white;
				GUILayout.Label("Generate", GUILayout.ExpandWidth (false));
					if ( DK_UMACrowd.GenerateWear == true ) GUI.color = Green;
				else GUI.color = Color.gray;
				if ( GUILayout.Button("wears", GUILayout.ExpandWidth (true))) {
						if ( DK_UMACrowd.GenerateWear == true ) DK_UMACrowd.GenerateWear = false;
						else DK_UMACrowd.GenerateWear = true;
				}
					if ( DK_UMACrowd.GenerateHandled == true ) GUI.color = Green;
				else GUI.color = Color.gray;
				if ( GUILayout.Button("Handled", GUILayout.ExpandWidth (true))) {
						if ( DK_UMACrowd.GenerateHandled == true ) DK_UMACrowd.GenerateHandled = false;
						else DK_UMACrowd.GenerateHandled = true;
				}
					if ( EditorVariables.DK_UMACrowd.ToUMA == true ) GUI.color = Green;
				else GUI.color = Color.gray;
			/*	if ( GUILayout.Button("To UMA", GUILayout.ExpandWidth (true))) {
						if ( EditorVariables.DK_UMACrowd.ToUMA == true ) EditorVariables.DK_UMACrowd.ToUMA = false;
						else EditorVariables.DK_UMACrowd.ToUMA = true;
				}*/
			}

			#region Step0
				try {
			if ( Step0 &&  !MultipleUMASetup 
			    && EditorVariables._RaceLibrary.raceElementList.Length > 0 && EditorVariables._DKSlotLibrary.slotElementList.Length > 0 && EditorVariables._OverlayLibrary.overlayElementList.Length > 0 
			    ) {
				ResetSteps ();
				GUI.color = Color.white;
				if (  !MultipleUMASetup && GUILayout.Button ( "Create a single Model", GUILayout.ExpandWidth (true))) {
					EditorVariables.DK_UMACrowd.RaceAndGender.SingleORMulti = true;
					EditorVariables.DK_UMACrowd.RaceAndGender.MultiRace = "One Race";
					MultiRace = "";
					MultipleUMASetup = false;
					Step0 = false ;
					Step1 = true ;
						if ( EditorVariables.RaceLibraryObj == null ) EditorVariables.RaceLibraryObj = GameObject.Find("DKRaceLibrary");	
					if ( EditorVariables.RaceLibraryObj != null ) EditorVariables._RaceLibrary =  EditorVariables.RaceLibraryObj.GetComponent<DKRaceLibrary>();
					if ( EditorVariables.DKSlotLibraryObj == null ) EditorVariables.DKSlotLibraryObj = GameObject.Find("DKSlotLibrary");	
					if ( EditorVariables.DKSlotLibraryObj != null ) EditorVariables._DKSlotLibrary =  EditorVariables.DKSlotLibraryObj.GetComponent<DKSlotLibrary>();
						if ( EditorVariables.OverlayLibraryObj == null ) EditorVariables.OverlayLibraryObj = GameObject.Find("DKOverlayLibrary");	
					if ( EditorVariables.OverlayLibraryObj != null ) EditorVariables._OverlayLibrary =  EditorVariables.OverlayLibraryObj.GetComponent<DKOverlayLibrary>();
						EditorVariables.DKUMACustomizationObj = GameObject.Find("DKUMACustomization");	
					if ( EditorVariables.DKUMACustomizationObj != null ) EditorVariables.DK_DKUMACustomization =  EditorVariables.DKUMACustomizationObj.GetComponent<DKUMACustomization>();
					
					DK_UMA = GameObject.Find("DK_UMA");
					if ( DK_UMA == null ) {
						var goDK_UMA = new GameObject();	goDK_UMA.name = "DK_UMA";
						DK_UMA = GameObject.Find("DK_UMA");
					}
					EditorVariables.GeneratorPresets = GameObject.Find("Generator Presets");
					if ( EditorVariables.GeneratorPresets == null ) 
					{
						EditorVariables.GeneratorPresets = (GameObject) Instantiate(Resources.Load("Generator Presets"), Vector3.zero, Quaternion.identity);
						EditorVariables.GeneratorPresets.name = "Generator Presets";
						EditorVariables.GeneratorPresets = GameObject.Find("Generator Presets");
						EditorVariables.GeneratorPresets.transform.parent = DK_UMA.transform;
						EditorVariables.PresetToEdit = null;
					}
				}
				if (!MultipleUMASetup && GUILayout.Button ( "Create Multiple Models", GUILayout.ExpandWidth (true))) {
					MultipleUMASetup = true;
					EditorVariables.DK_UMACrowd.RaceAndGender.SingleORMulti = false;
						if ( EditorVariables.RaceLibraryObj == null ) EditorVariables.RaceLibraryObj = GameObject.Find("DKRaceLibrary");	
					if ( EditorVariables.RaceLibraryObj != null ) EditorVariables._RaceLibrary =  EditorVariables.RaceLibraryObj.GetComponent<DKRaceLibrary>();
					if ( EditorVariables.DKSlotLibraryObj == null ) EditorVariables.DKSlotLibraryObj = GameObject.Find("DKSlotLibrary");	
					if ( EditorVariables.DKSlotLibraryObj != null ) EditorVariables._DKSlotLibrary =  EditorVariables.DKSlotLibraryObj.GetComponent<DKSlotLibrary>();
						if ( EditorVariables.OverlayLibraryObj == null ) EditorVariables.OverlayLibraryObj = GameObject.Find("DKOverlayLibrary");	
					if ( EditorVariables.OverlayLibraryObj != null ) EditorVariables._OverlayLibrary =  EditorVariables.OverlayLibraryObj.GetComponent<DKOverlayLibrary>();
						EditorVariables.DKUMACustomizationObj = GameObject.Find("DKUMACustomization");	
					if ( EditorVariables.DKUMACustomizationObj != null ) EditorVariables.DK_DKUMACustomization =  EditorVariables.DKUMACustomizationObj.GetComponent<DKUMACustomization>();
					
					DK_UMA = GameObject.Find("DK_UMA");
					if ( DK_UMA == null ) {
						var goDK_UMA = new GameObject();	goDK_UMA.name = "DK_UMA";
						DK_UMA = GameObject.Find("DK_UMA");
					}
					EditorVariables.GeneratorPresets = GameObject.Find("Generator Presets");
					if ( EditorVariables.GeneratorPresets == null ) 
					{
						EditorVariables.GeneratorPresets = (GameObject) Instantiate(Resources.Load("Generator Presets"), Vector3.zero, Quaternion.identity);
						EditorVariables.GeneratorPresets.name = "Generator Presets";
						EditorVariables.GeneratorPresets = GameObject.Find("Generator Presets");
						EditorVariables.GeneratorPresets.transform.parent = DK_UMA.transform;
						EditorVariables.PresetToEdit = null;
					}
				}
			}
				}catch(NullReferenceException){Debug.LogError ("DK UMA is not installed in the current scene. Please open the Welcome tab of the DK UMA Editor and setup the Editor.");}
			if ( !MultipleUMASetup && !Step0 ) {
				GUI.color = Color.white;
				GUILayout.Label("Create Single", "toolbarbutton", GUILayout.ExpandWidth (true));
			}
			if ( MultipleUMASetup )
			{
				GUI.color = Color.white;
				GUILayout.Label("Create Multiple", "toolbarbutton", GUILayout.ExpandWidth (true));
				if ( MultiRace == "" )MultiRace = "One Race";
				// help
				GUI.color = Color.white;
				GUILayout.Space(5);
				using (new Horizontal()) {
					GUI.color = Color.white;
					if ( Step0 && GUILayout.Button ( "Generate now", GUILayout.ExpandWidth (true))) {
						EditorVariables.DK_UMACrowd.RaceAndGender.GenderType = "Random";
						EditorVariables.DK_UMACrowd.RaceAndGender.Gender = "Random";
						if ( EditorVariables.DK_UMACrowd.Colors != null ) EditorVariables.DK_UMACrowd.Colors.RanColors = true;
						EditorVariables.DK_UMACrowd.Randomize.RanShape = true;
						EditorVariables.DK_UMACrowd.Randomize.RanElements = true;
						if ( EditorVariables.DK_UMACrowd.Wears != null ) EditorVariables.DK_UMACrowd.Wears.RanWearAct = true;
						if ( EditorVariables.DK_UMACrowd.Wears != null ) EditorVariables.DK_UMACrowd.Wears.RanWearChoice = true;
						Step0 = true ;
						Step1 = false ;
						MultipleUMASetup = false ;
						EditorVariables.SingleORMulti = true ;
					//	DK_UMACrowd.GeneratorMode = "Preset";
						DK_UMACrowd.OverlayLibraryObj = EditorVariables.OverlayLibraryObj;
						DK_UMACrowd.DKSlotLibraryObj = EditorVariables.DKSlotLibraryObj;
						DK_UMACrowd.RaceLibraryObj = EditorVariables.RaceLibraryObj;
						EditorVariables.DK_UMACrowd.LaunchGenerateUMA();
						EditorVariables.DK_UMACrowd.generateLotsUMA = true;
						EditorVariables.DK_UMACrowd.umaTimerEnd = 0;
					}
					if ( Step0 && GUILayout.Button ( "Continue to Customize", GUILayout.ExpandWidth (true))) {
						Step0 = false ;
						Step1 = true ;
					}
					GUI.color = Red;
					if (GUILayout.Button ( "Cancel", GUILayout.ExpandWidth (true))) {
						EditorVariables.SingleORMulti = true;
						EditorVariables.DK_UMACrowd.RaceAndGender.SingleORMulti = false;
						Step0 = true ;
						MultipleUMASetup = false;
					}
				}
				// Crowd
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Crowd", GUILayout.ExpandWidth (false)); 
					string Xsize = EditorVariables.DK_UMACrowd.umaCrowdSize.x.ToString();
					GUI.color = Color.white;
					GUILayout.Label ( "X", GUILayout.ExpandWidth (false)); 
					Xsize= GUILayout.TextField(Xsize, 3, GUILayout.Width (30));
					Xsize= Regex.Replace(Xsize, "[^0-9]", "");
					if ( Xsize == "" ) Xsize = "1";
					EditorVariables.DK_UMACrowd.umaCrowdSize.x = Convert.ToInt32(Xsize);
					string Ysize = EditorVariables.DK_UMACrowd.umaCrowdSize.y.ToString();
					GUILayout.Label ( "Y", GUILayout.ExpandWidth (false));
					Ysize= GUILayout.TextField(Ysize, 3, GUILayout.Width (30));
					Ysize= Regex.Replace(Ysize, "[^0-9]", "");
					if ( Ysize == "" ) Ysize = "1";
					EditorVariables.DK_UMACrowd.umaCrowdSize.y = Convert.ToInt32(Ysize);
					if ( EditorVariables.DK_UMACrowd.umaCrowdSize.x < 1 ) EditorVariables.DK_UMACrowd.umaCrowdSize.x = 1;
					if ( EditorVariables.DK_UMACrowd.umaCrowdSize.y < 1 ) EditorVariables.DK_UMACrowd.umaCrowdSize.y = 1;

					GUI.color = Color.yellow;
					GUILayout.Label ( "Spacing", GUILayout.ExpandWidth (false)); 
					string XSpacing = EditorVariables.DK_UMACrowd.Spacing.x.ToString();
					GUI.color = Color.white;
					GUILayout.Label ( "X", GUILayout.ExpandWidth (false)); 
					XSpacing= GUILayout.TextField(XSpacing, 3, GUILayout.Width (30));
					XSpacing= Regex.Replace(XSpacing, "[^0-9]", "");
					if ( XSpacing == "" ) XSpacing = "1";
					EditorVariables.DK_UMACrowd.Spacing.x = Convert.ToInt32(XSpacing);
					string YSpacing = EditorVariables.DK_UMACrowd.Spacing.y.ToString();
					GUILayout.Label ( "Y", GUILayout.ExpandWidth (false));
					YSpacing= GUILayout.TextField(YSpacing, 3, GUILayout.Width (30));
					YSpacing= Regex.Replace(YSpacing, "[^0-9]", "");
					if ( YSpacing == "" ) YSpacing = "1";
					EditorVariables.DK_UMACrowd.Spacing.y = Convert.ToInt32(YSpacing);
					if ( EditorVariables.DK_UMACrowd.Spacing.x < 1 ) EditorVariables.DK_UMACrowd.Spacing.x = 1;
					if ( EditorVariables.DK_UMACrowd.Spacing.y < 1 ) EditorVariables.DK_UMACrowd.Spacing.y = 1;

				}
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Spawn Frequency :", GUILayout.ExpandWidth (false)); 
					GUI.color = Color.white;
					
					SpawnFrequencySlider = GUILayout.HorizontalSlider(SpawnFrequencySlider, 0.005f, 0.5F);
					EditorVariables.DK_UMACrowd.SpawnFrequency = SpawnFrequencySlider;
					GUILayout.Label ( EditorVariables.DK_UMACrowd.SpawnFrequency.ToString(), GUILayout.ExpandWidth (false));
				}
				// Race
				using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label ( "Race :", GUILayout.ExpandWidth (true)); 
					if ( MultiRace == "One Race" ) GUI.color = Green;
					else GUI.color = Color.gray;
					if (GUILayout.Button ( "One Race", GUILayout.ExpandWidth (true))) {
						MultiRace = "One Race";
						EditorVariables.DK_UMACrowd.RaceAndGender.MultiRace = MultiRace;
					}
				
					if ( MultiRace == "Random for One" ) GUI.color = Green;
					else GUI.color = Color.gray;	
					if (GUILayout.Button ( "Random for One", GUILayout.ExpandWidth (true))) {
						MultiRace = "Random for One";
						EditorVariables.DK_UMACrowd.RaceAndGender.MultiRace = MultiRace;
					}	
				}
			
			}
			#endregion Step0
			
			#region Step1
			if ( Step1 ) {
				// Navigate
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					if (GUILayout.Button ( "<", GUILayout.ExpandWidth (false))) 
					{		Step0 = true ;	 EditorVariables.SingleORMulti = true; }
					GUI.color = Red;
					if (GUILayout.Button ( "Reset All", GUILayout.ExpandWidth (true))) 
					{		Step0 = true ;	 EditorVariables.SingleORMulti = true;	}
					GUI.color = Color.yellow;
				}
				// help
				if ( MultiRace == "" ){
					GUI.color = Color.yellow;
					GUILayout.TextField("Select a single Race from the list or let it be randomized" , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					GUILayout.Space(5);
				}
				if ( MultiRace == "One Race" ){
					GUI.color = Color.yellow;
					GUILayout.TextField("Select a single Race from the list." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					GUILayout.Space(5);
				}	
				if ( MultiRace == "Random for All" )
				{
					GUI.color = Color.yellow;
					GUILayout.TextField("You are creating a crowd with a single Race for all the models, click on the Random Race Button." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					GUILayout.Space(5);
				}
				if ( MultiRace == "Random for One" )
				{
					if ( Helper )
					{
						GUI.color = Color.yellow;
						GUILayout.TextField("You are creating a crowd with a different Race for every Model. You just have to click on the 'Next Step' button." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						GUILayout.Space(5);
					}
					using (new Horizontal()) {
						GUI.color = Color.white;
						if (GUILayout.Button ( "Next Step" )) {
							Step1 = false ;
							Step2 = true ;
						}
					}
				}
				GUI.color = Color.white;
				if ( MultiRace == "Random for All" || MultiRace == "One Race"|| MultiRace == ""  ) {
					if ( GUILayout.Button ( "Random Race", GUILayout.ExpandWidth (true))) {
					
						EditorVariables.DK_UMACrowd.RaceAndGender.RaceToCreate = null;
						EditorVariables.DK_UMACrowd.RaceAndGender.Race = "Random";
						EditorVariables.DK_UMACrowd.RaceAndGender.GenderType = "Random";
							
						Step1 = false ;
						Step2 = true ;
					}
				}
				GUILayout.Space(5);
				if ( EditorVariables._RaceLibrary && MultiRace == "One Race" || MultiRace == "" ) using (new ScrollView(ref scroll)) 
				{
					List<string> _RacesList = new List<string>();
					for(int i = 0; i < EditorVariables._RaceLibrary.raceElementList.Length; i ++){
						try{
						if ( _RacesList.Contains(EditorVariables._RaceLibrary.raceElementList[i].Race) == false 
						    && EditorVariables._RaceLibrary.raceElementList[i].Active ){
							_RacesList.Add(EditorVariables._RaceLibrary.raceElementList[i].Race);
						}
						}catch(NullReferenceException){}
					}
					for(int i = 0; i < _RacesList.Count; i ++){
						using (new Horizontal()) {

							if (GUILayout.Button ( _RacesList[i], GUILayout.ExpandWidth (true))) {
								for(int i2 = 0; i2 < EditorVariables._RaceLibrary.raceElementList.Length; i2 ++){
									if ( EditorVariables._RaceLibrary.raceElementList[i2].Race == _RacesList[i] ) 
									{
											EditorVariables.RaceToCreate = EditorVariables._RaceLibrary.raceElementList[i2];
										EditorVariables.DK_UMACrowd.RaceAndGender.Race = EditorVariables._RaceLibrary.raceElementList[i2].Race;	
									}
								}
								Step1 = false ;
								Step2 = true ;
							}
						}
					}
				}
			}
			#endregion Step1
			
			#region Step2
			if ( Step2 ) {
				// Navigate
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					if (GUILayout.Button ( "<", GUILayout.ExpandWidth (false))) 
					{		Step1 = true ;	Step2 = false ;	}
					GUI.color = Red;
					if (GUILayout.Button ( "Reset All", GUILayout.ExpandWidth (true))) 
					{		Step0 = true ;	}
					GUI.color = Color.yellow;
				}
				// help
				if ( Helper )
				{
					GUI.color = Color.yellow;
					GUILayout.TextField("Do you want to create a Completely Random model OR do you want to customize the creation ?" , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					GUILayout.Space(5);
				}
				GUI.color = Color.white;
				if ( EditorVariables.DK_UMACrowd.RaceAndGender.SingleORMulti == true && GUILayout.Button ( "Completely Random model", GUILayout.ExpandWidth (true))) {
					EditorVariables.DK_UMACrowd.RaceAndGender.RaceDone = false;
					Step0 = true ;
					EditorVariables.DK_UMACrowd.RaceAndGender.GenderType = "Random";
					EditorVariables.DK_UMACrowd.RaceAndGender.Gender = "Random";
					EditorVariables.DK_UMACrowd.Colors.RanColors = true;
					EditorVariables.DK_UMACrowd.Randomize.RanShape = true;
					EditorVariables.DK_UMACrowd.Wears.RanWearAct = true;
					EditorVariables.DK_UMACrowd.Wears.RanWearChoice = true;
					GameObject DKUMAGeneratorObj = GameObject.Find("DKUMAGenerator");
					EditorVariables.DK_DKUMAGenerator =  EditorVariables.DKUMAGeneratorObj.GetComponent<DKUMAGenerator>();
					EditorVariables.DK_DKUMAGenerator.AvatarSavePath = EditorVariables.AvatarSavePath;
					EditorVariables.DK_DKUMAGenerator.umaDirtyList.Clear();
					EditorVariables.DK_DKUMAGenerator.Awake();
					
						DK_UMACrowd.OverlayLibraryObj = EditorVariables.OverlayLibraryObj;
						DK_UMACrowd.DKSlotLibraryObj = EditorVariables.DKSlotLibraryObj;
						DK_UMACrowd.RaceLibraryObj = EditorVariables.RaceLibraryObj;
						EditorVariables.DK_UMACrowd.LaunchGenerateUMA();
			
					CreatingUMA = true;
					if ( CreatingUMA == true ) 
					{
							EditorVariables.UMAModel = GameObject.Find("New UMA Model");
							if (EditorVariables.UMAModel != null ) 
						{
								EditorVariables.UMAModel.name = ("New UMA Model (Rename it)");
								EditorVariables.UMAModel.transform.parent = EditorVariables.MFSelectedList.transform;
							CreatingUMA = false;
								Selection.activeGameObject = EditorVariables.UMAModel;
						}
					}
				}
					if (EditorVariables.DK_UMACrowd.RaceAndGender.SingleORMulti == false && GUILayout.Button ( "Generate Crowd now", GUILayout.ExpandWidth (true))) {
						GameObject DKUMAGeneratorObj = GameObject.Find("DKUMAGenerator");
						EditorVariables.DK_DKUMAGenerator =  EditorVariables.DKUMAGeneratorObj.GetComponent<DKUMAGenerator>();
						EditorVariables.DK_DKUMAGenerator.AvatarSavePath = EditorVariables.AvatarSavePath;
						EditorVariables.DK_DKUMAGenerator.umaDirtyList.Clear();
						EditorVariables.DK_UMACrowd.RaceAndGender.GenderType = "Random";
						EditorVariables.DK_UMACrowd.RaceAndGender.Gender = "Random";
						EditorVariables.DK_UMACrowd.Colors.RanColors = true;
						EditorVariables.DK_UMACrowd.Randomize.RanShape = true;
						EditorVariables.DK_UMACrowd.Randomize.RanElements = true;
						EditorVariables.DK_UMACrowd.Wears.RanWearAct = true;
						EditorVariables.DK_UMACrowd.Wears.RanWearChoice = true;
						Step0 = true ;
						Step1 = false ;
						MultipleUMASetup = false ;
						EditorVariables.SingleORMulti = true ;
					//	DK_UMACrowd.GeneratorMode = "Preset";
						DK_UMACrowd.OverlayLibraryObj = EditorVariables.OverlayLibraryObj;
						DK_UMACrowd.DKSlotLibraryObj = EditorVariables.DKSlotLibraryObj;
						DK_UMACrowd.RaceLibraryObj = EditorVariables.RaceLibraryObj;
						EditorVariables.DK_UMACrowd.generateLotsUMA = true;
						EditorVariables.DK_UMACrowd.umaTimerEnd = 0;
					}
				if (GUILayout.Button ( "Customize the creation", GUILayout.ExpandWidth (true))) {
					Step3 = true ;
					Step2 = false ;
				}
			}
			#endregion Step2
			
			#region Step3
			if ( Step3 ) {
				// Navigate
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					if (GUILayout.Button ( "<", GUILayout.ExpandWidth (false))) 
					{		Step2 = true ;	Step3 = false ;	}
					GUI.color = Red;
					if (GUILayout.Button ( "Reset All", GUILayout.ExpandWidth (true))) 
					{		Step0 = true ;	}
					GUI.color = Color.yellow;
				}
				using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label ( "Gender :", GUILayout.ExpandWidth (true)); 
					if ( Gender == "Male" ) GUI.color = Green;
					else GUI.color = Color.white;
					if (GUILayout.Button ( "Male", GUILayout.ExpandWidth (true))) {
						Gender = "Male";
						EditorVariables.DK_UMACrowd.RaceAndGender.Gender = Gender; EditorVariables.DK_UMACrowd.RaceAndGender.GenderType = "Modified";
						Step4 = true ;	Step3 = false ;

					}
					if ( Gender == "Female" ) GUI.color = Green;
					else GUI.color = Color.white;	
					if (GUILayout.Button ( "Female", GUILayout.ExpandWidth (true))) {
						Gender = "Female";
						EditorVariables.DK_UMACrowd.RaceAndGender.Gender = Gender; EditorVariables.DK_UMACrowd.RaceAndGender.GenderType = "Modified";
						Step4 = true ;	Step3 = false ;

					}
					if ( Gender == "" || Gender == null ) EditorVariables.DK_UMACrowd.RaceAndGender.GenderType = "Random";
					if ( EditorVariables.DK_UMACrowd.RaceAndGender.Gender == "Random" ) GUI.color = Green;
					else GUI.color = Color.white;	
					if (GUILayout.Button ( "Random", GUILayout.ExpandWidth (true))) {
						Gender = "Random"; 
						EditorVariables.DK_UMACrowd.RaceAndGender.GenderType = "Random";
						EditorVariables.DK_UMACrowd.RaceAndGender.Gender = Gender;
						Step4 = true ;	Step3 = false ;

					}
				}
			}
			#endregion Step3
			
			#region Step4
			if ( Step4 ) {
				#region Step4 Actions
				// Navigate
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					if (GUILayout.Button ( "<", GUILayout.ExpandWidth (false))) 
					{		Step3 = true ;	Step4 = false ;	}
					GUI.color = Red;
					if (GUILayout.Button ( "Reset All", GUILayout.ExpandWidth (true))) 
					{		Step0 = true ;	}
					GUI.color = Color.yellow;
				}
				GUILayout.Space(5);
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					if ( GUILayout.Button ( "Randomize and go Next", GUILayout.ExpandWidth (true))) 
					{
						EditorVariables.DK_UMACrowd.Colors.RanColors = true;
						Step5 = true ;	Step4 = false ;
						// TO COMPLET
					}
					GUI.color = Green;
					if ( GUILayout.Button ( "Apply and go Next", GUILayout.ExpandWidth (true))) 
					{
						EditorVariables.DK_UMACrowd.Colors.RanColors = false;
						EditorVariables.DK_UMACrowd.Colors.skinColor = EditorVariables.SkinColor;
						EditorVariables.DK_UMACrowd.Colors.EyesColor = EditorVariables.EyesColor;
						EditorVariables.DK_UMACrowd.Colors.HairColor = EditorVariables.HairColor;
						EditorVariables.DK_UMACrowd.Colors.TorsoWearColor = EditorVariables.TorsoWearColor;
						EditorVariables.DK_UMACrowd.Colors.LegsWearColor = EditorVariables.LegsWearColor;
						EditorVariables.DK_UMACrowd.Colors.FeetWearColor = EditorVariables.FeetWearColor;
						EditorVariables.DK_UMACrowd.Colors.HandWearColor = EditorVariables.HandWearColor;
						EditorVariables.DK_UMACrowd.Colors.HeadWearColor = EditorVariables.HeadWearColor;
						EditorVariables.DK_UMACrowd.Colors.BeltWearColor = EditorVariables.BeltWearColor;
						
						EditorVariables.DK_UMACrowd.Colors.skinTone = EditorVariables.SkinTone0;
						EditorVariables.DK_UMACrowd.Colors.skinTone1 = EditorVariables.SkinTone1;
						EditorVariables.DK_UMACrowd.Colors.skinTone2 = EditorVariables.SkinTone2;
						EditorVariables.DK_UMACrowd.Colors.skinTone3 = EditorVariables.SkinTone3;

						EditorVariables.DK_UMACrowd.Colors.HairColor1 = EditorVariables.HairColor1;
						EditorVariables.DK_UMACrowd.Colors.HairColor2 = EditorVariables.HairColor2;
				
						EditorVariables.DK_UMACrowd.Colors.TorsoWearColor1 = EditorVariables.TorsoWearColor1;
						EditorVariables.DK_UMACrowd.Colors.TorsoWearColor2 = EditorVariables.TorsoWearColor2;
						EditorVariables.DK_UMACrowd.Colors.TorsoWearColor3 = EditorVariables.TorsoWearColor3;
				
						EditorVariables.DK_UMACrowd.Colors.LegsWearColor1 = EditorVariables.LegsWearColor1;
						EditorVariables.DK_UMACrowd.Colors.LegsWearColor2 = EditorVariables.LegsWearColor2;
						EditorVariables.DK_UMACrowd.Colors.LegsWearColor3 = EditorVariables.LegsWearColor3;

						EditorVariables.DK_UMACrowd.Colors.HeadWearColor1 = EditorVariables.HeadWearColor1;
						EditorVariables.DK_UMACrowd.Colors.HeadWearColor2 = EditorVariables.HeadWearColor2;
						EditorVariables.DK_UMACrowd.Colors.HeadWearColor3 = EditorVariables.HeadWearColor3;
							
						EditorVariables.DK_UMACrowd.Colors.HandWearColor1 = EditorVariables.HandWearColor1;
						EditorVariables.DK_UMACrowd.Colors.HandWearColor2 = EditorVariables.HandWearColor2;
						EditorVariables.DK_UMACrowd.Colors.HandWearColor3 = EditorVariables.HandWearColor3;
							
						EditorVariables.DK_UMACrowd.Colors.BeltWearColor1 = EditorVariables.BeltWearColor1;
						EditorVariables.DK_UMACrowd.Colors.BeltWearColor2 = EditorVariables.BeltWearColor2;
						EditorVariables.DK_UMACrowd.Colors.BeltWearColor3 = EditorVariables.BeltWearColor3;

						EditorVariables.DK_UMACrowd.Colors.FeetWearColor1 = EditorVariables.FeetWearColor1;
						EditorVariables.DK_UMACrowd.Colors.FeetWearColor2 = EditorVariables.FeetWearColor2;
						EditorVariables.DK_UMACrowd.Colors.FeetWearColor3 = EditorVariables.FeetWearColor3;

						Step5 = true ;	Step4 = false ;	
						// TO COMPLET
						
					}
					
				}
				GUI.color = Color.yellow;
				if ( Helper ) GUILayout.TextField("You can randomize only the selected colors category and customize the desired ones. Setup your colors and click on the colors types you want to let it be randomized. " +
					"Don't forget to uncheck the colors type you are customizing." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			
				using (new Horizontal()) {
					if ( EditorVariables.DK_UMACrowd.Colors.UsePresets == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Use Race Presets", GUILayout.ExpandWidth (true))){
						if ( EditorVariables.DK_UMACrowd.Colors.UsePresets == true ) EditorVariables.DK_UMACrowd.Colors.UsePresets = false;
						else EditorVariables.DK_UMACrowd.Colors.UsePresets = true;
					} 
					if ( EditorVariables.DK_UMACrowd.Colors.UseOverlayPresets == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Use Overlay Presets", GUILayout.ExpandWidth (true))){
						if ( EditorVariables.DK_UMACrowd.Colors.UseOverlayPresets == true ) EditorVariables.DK_UMACrowd.Colors.UseOverlayPresets = false;
						else EditorVariables.DK_UMACrowd.Colors.UseOverlayPresets = true;
					} 
				}
				using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label("Randomize", GUILayout.Width (70));
					if ( EditorVariables.DK_UMACrowd.Colors.RanSkin == true ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Skin", GUILayout.ExpandWidth (true))){
						if ( EditorVariables.DK_UMACrowd.Colors.RanSkin == true ) EditorVariables.DK_UMACrowd.Colors.RanSkin = false;
						else EditorVariables.DK_UMACrowd.Colors.RanSkin = true;
					} 
					if ( EditorVariables.DK_UMACrowd.Colors.RanEyes == true ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Eyes", GUILayout.ExpandWidth (true))){
						if ( EditorVariables.DK_UMACrowd.Colors.RanEyes == true ) EditorVariables.DK_UMACrowd.Colors.RanEyes = false;
						else EditorVariables.DK_UMACrowd.Colors.RanEyes = true;
					} 
					if ( EditorVariables.DK_UMACrowd.Colors.RanHair == true ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Hair", GUILayout.ExpandWidth (true))){
						if ( EditorVariables.DK_UMACrowd.Colors.RanHair == true ) EditorVariables.DK_UMACrowd.Colors.RanHair = false;
						else EditorVariables.DK_UMACrowd.Colors.RanHair = true;
					} 
					if ( EditorVariables.DK_UMACrowd.Colors.RanWear == true ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Wear", GUILayout.ExpandWidth (true))){
						if ( EditorVariables.DK_UMACrowd.Colors.RanWear == true ) EditorVariables.DK_UMACrowd.Colors.RanWear = false;
						else EditorVariables.DK_UMACrowd.Colors.RanWear = true;
					} 
				
				}
				#endregion Step4 Actions
				
				#region Step4 Help
				GUILayout.Space(5);
				// help
				GUI.color = Color.yellow;
				GUILayout.TextField("Select color of the different parts or let it be random." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				GUILayout.Space(5);
				#endregion Step4 Help
				
				#region Step4 Menu
				using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
					if ( ShowSkinTone ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Skin Color", "toolbarbutton", GUILayout.ExpandWidth (true))) 
					{	
						ShowSkinTone = true;
						ShowEyesColor = false;
						ShowHairColor = false;
						ShowTorsoWColor = false;
					}
					if ( ShowEyesColor ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Eyes Color", "toolbarbutton", GUILayout.ExpandWidth (true))) 
					{	
						ShowEyesColor = true;
						ShowSkinTone = false;
						ShowHairColor = false;
						ShowTorsoWColor = false;
					}
					if ( ShowHairColor ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Hair Color", "toolbarbutton", GUILayout.ExpandWidth (true))) 
					{	
						ShowHairColor = true;
						ShowSkinTone = false;
						ShowEyesColor = false;
						ShowTorsoWColor = false;
					}
					if ( ShowTorsoWColor ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Wear / Cloth", "toolbarbutton", GUILayout.ExpandWidth (true))) 
					{	
						ShowTorsoWColor = true;
						ShowSkinTone = false;
						ShowEyesColor = false;
						ShowHairColor = false;
					}
					GUI.color = Color.white;
					GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
				}
				#endregion Step4 Menu
				
				#region Step4 Skin Color
				if ( ShowSkinTone )
				{
						EditorVariables.SkinTone = new Color ( (EditorVariables.SkinTone0 + EditorVariables.SkinTone1),(EditorVariables.SkinTone0 + EditorVariables.SkinTone2),(EditorVariables.SkinTone0 + EditorVariables.SkinTone3), 1 );
						if ( EditorVariables.SkinColorPresetName == "" || EditorVariables.SkinColorPresetName == null ) EditorVariables.SkinColor = EditorVariables.PickedColor + EditorVariables.SkinTone;
					using (new Horizontal()) {
						GUILayout.Label ( "Final Skin Color", GUILayout.Width (100));
						EditorVariables.SkinColor = EditorGUILayout.ColorField("", EditorVariables.SkinColor, GUILayout.Width (100));
					}
					// title
					GUILayout.Space(5);
					using (new Horizontal()) {
						GUI.color = Color.yellow;
						GUILayout.Label("Skin Color", "toolbarbutton", GUILayout.ExpandWidth (true));
					}

					if ( EditorVariables.SkinColorPresetName == "" || EditorVariables.SkinColorPresetName == null ) {
						using (new Horizontal()) {	
							GUI.color = Color.white;
							if (GUILayout.Button ( "Random", GUILayout.Width (100))) 
							{
									EditorVariables.SkinTone0 = UnityEngine.Random.Range(0.1f, 0.6f);
									EditorVariables.SkinTone1 = UnityEngine.Random.Range(0.35f,0.4f);
									EditorVariables.SkinTone2 = UnityEngine.Random.Range(0.25f,0.4f);
									EditorVariables.SkinTone3 = UnityEngine.Random.Range(0.35f,0.4f);	
							}
								EditorVariables.SkinTone = EditorGUILayout.ColorField("", EditorVariables.SkinTone, GUILayout.Width (100));
						}
						using (new Horizontal()) {
							GUILayout.Label("SkinTone0", GUILayout.ExpandWidth (false));
								EditorVariables.SkinTone0 = GUILayout.HorizontalSlider(EditorVariables.SkinTone0 ,0.05f, 0.6f );
								GUILayout.Label(EditorVariables.SkinTone0.ToString(), GUILayout.Width (50));
						}
						using (new Horizontal()) {
							GUILayout.Label("SkinTone1", GUILayout.ExpandWidth (false));
								EditorVariables.SkinTone1 = GUILayout.HorizontalSlider(EditorVariables.SkinTone1 ,0.35f,0.4f );
								GUILayout.Label(EditorVariables.SkinTone1.ToString(), GUILayout.Width (50));
						}
						using (new Horizontal()) {
							GUILayout.Label("SkinTone2", GUILayout.ExpandWidth (false));
								EditorVariables.SkinTone2 = GUILayout.HorizontalSlider(EditorVariables.SkinTone2 ,0.25f,0.4f );
								GUILayout.Label(EditorVariables.SkinTone2.ToString(), GUILayout.Width (50));
						}	
						using (new Horizontal()) {
							GUILayout.Label("SkinTone3", GUILayout.ExpandWidth (false));
								EditorVariables.SkinTone3 = GUILayout.HorizontalSlider(EditorVariables.SkinTone3 ,0.35f,0.4f );
								GUILayout.Label(EditorVariables.SkinTone3.ToString(), GUILayout.Width (50));
						}
					}
					using (new Horizontal()) {
						if ( EditorVariables.SkinColorPresetName != "" && EditorVariables.SkinColorPresetName != null ) GUI.color = Green;
						else GUI.color = Color.gray;
						if (GUILayout.Button ( "Color Preset", GUILayout.Width (100))) 
						{
							OpenColorPresetWin();
							ColorPreset_Editor.Statut = "ApplyTo";
							ColorPreset_Editor.SelectedElement = "Skin";
							ColorPreset_Editor.CurrentElementColor = EditorVariables.SkinColor;
							
						}
						if ( EditorVariables.SkinColorPresetName != "" && EditorVariables.SkinColorPresetName != null ){
							GUILayout.Label(EditorVariables.SkinColorPresetName, GUILayout.Width (125));
						}
						GUI.color = Red;
						if ( EditorVariables.SkinColorPresetName != "" && EditorVariables.SkinColorPresetName != null 
						    && GUILayout.Button ( "X", GUILayout.ExpandWidth (false))) 
						{
							EditorVariables.SkinColorPresetName = "";
						}
					}
				}
				#endregion Step4 Skin Color
				
				#region Step4 Eyes Color
				if ( ShowEyesColor )
				{
						EditorVariables.EyesTone = new Color ( EditorVariables.EyeOverlayAdjustColor,EditorVariables.EyeOverlayAdjustColor,EditorVariables.EyeOverlayAdjustColor, 1 );
						if ( EditorVariables.EyesColorPresetName == "" || EditorVariables.EyesColorPresetName == null )EditorVariables.EyesColor = EditorVariables.PickedEyesColor  +  EditorVariables.EyesTone;
					using (new Horizontal()) {
						GUILayout.Label ( "Final Eyes Color", GUILayout.Width (100));
							EditorVariables.EyesColor = EditorGUILayout.ColorField("", EditorVariables.EyesColor, GUILayout.Width (100));
					}
					// title
					GUILayout.Space(5);
					using (new Horizontal()) {
						GUI.color = Color.yellow;
						GUILayout.Label("Eyes Color", "toolbarbutton", GUILayout.ExpandWidth (true));
					}

					if ( EditorVariables.EyesColorPresetName == "" || EditorVariables.EyesColorPresetName == null ){
						GUILayout.Space(5);
						// options
						using (new Horizontal()) {	
							GUI.color = Color.white;
							if (GUILayout.Button ( "Random", GUILayout.Width (100))) 
							{
								EditorVariables.EyeOverlayAdjustColor = UnityEngine.Random.Range(0.05f, 0.5f);
							}
								EditorVariables.EyesTone = EditorGUILayout.ColorField("", EditorVariables.EyesTone, GUILayout.Width (100));
						}
						using (new Horizontal()) {
							GUILayout.Label("Eye Adjust Color", GUILayout.ExpandWidth (false));
							EditorVariables.EyeOverlayAdjustColor = GUILayout.HorizontalSlider(EditorVariables.EyeOverlayAdjustColor ,0.05f, 0.5f );
							GUILayout.Label(EditorVariables.EyeOverlayAdjustColor.ToString(), GUILayout.Width (50));
						}
					}
					using (new Horizontal()) {
						if ( EditorVariables.EyesColorPresetName != "" && EditorVariables.EyesColorPresetName != null ) GUI.color = Green;
						else GUI.color = Color.gray;
						if (GUILayout.Button ( "Color Preset", GUILayout.Width (100))) 
						{
							OpenColorPresetWin();
								ColorPreset_Editor.Statut = "ApplyTo";
							ColorPreset_Editor.SelectedElement = "Eyes";
							ColorPreset_Editor.CurrentElementColor = EditorVariables.EyesColor;
								
						}
						if ( EditorVariables.EyesColorPresetName != "" && EditorVariables.EyesColorPresetName != null ){
								GUILayout.Label(EditorVariables.EyesColorPresetName, GUILayout.Width (125));
						}
						GUI.color = Red;
						if ( EditorVariables.EyesColorPresetName != "" && EditorVariables.EyesColorPresetName != null 
								&& GUILayout.Button ( "X", GUILayout.ExpandWidth (false))) 
						{
								EditorVariables.EyesColorPresetName = "";
						}
					}
				}
				#endregion Step4 Eyes Color
				
				#region Step4 Hair Color
				if ( ShowHairColor )
				{
						EditorVariables.HairTone = new Color(EditorVariables.HairColor1,EditorVariables.HairColor2,EditorVariables.HairColor2,1.0f);
						if ( EditorVariables.HairColorPresetName == "" || EditorVariables.HairColorPresetName == null ) EditorVariables.HairColor = EditorVariables.PickedHairColor + EditorVariables.HairTone;
					using (new Horizontal()) {
						GUILayout.Label ( "Final Hair Color", GUILayout.Width (100));
							EditorVariables.HairColor = EditorGUILayout.ColorField("", EditorVariables.HairColor, GUILayout.Width (100));
					}
					// title
					GUILayout.Space(5);
					using (new Horizontal()) {
						GUI.color = Color.yellow;
						GUILayout.Label("Hair Color", "toolbarbutton", GUILayout.ExpandWidth (true));
					}

					if ( EditorVariables.HairColorPresetName == "" || EditorVariables.HairColorPresetName == null ){

						// options
						using (new Horizontal()) {	
							GUI.color = Color.white;
							if (GUILayout.Button ( "Random", GUILayout.Width (100))) 
							{
									EditorVariables.HairColor1 = UnityEngine.Random.Range(0.05f, 0.9f);
									EditorVariables.HairColor2 = UnityEngine.Random.Range(0.05f, 0.9f);
							}
								EditorVariables.HairTone = EditorGUILayout.ColorField("", EditorVariables.HairTone, GUILayout.Width (100));
						}
						using (new Horizontal()) {
							GUILayout.Label("Hair Tone 1", GUILayout.ExpandWidth (false));
								EditorVariables.HairColor1 = GUILayout.HorizontalSlider(EditorVariables.HairColor1 ,0.05f, 0.9f );
								GUILayout.Label(EditorVariables.HairColor1.ToString(), GUILayout.Width (50));
						}
						using (new Horizontal()) {
							GUILayout.Label("Hair Tone 2", GUILayout.ExpandWidth (false));
								EditorVariables.HairColor2 = GUILayout.HorizontalSlider(EditorVariables.HairColor2 ,0.05f, 0.9f );
								GUILayout.Label(EditorVariables.HairColor2.ToString(), GUILayout.Width (50));
						}
					}
					using (new Horizontal()) {
						if ( EditorVariables.HairColorPresetName != "" && EditorVariables.HairColorPresetName != null ) GUI.color = Green;
						else GUI.color = Color.gray;
						if (GUILayout.Button ( "Color Preset", GUILayout.Width (100))) 
						{
							OpenColorPresetWin();
								ColorPreset_Editor.Statut = "ApplyTo";
							ColorPreset_Editor.SelectedElement = "Hair";
							ColorPreset_Editor.CurrentElementColor = EditorVariables.HairColor;
								
						}
						if ( EditorVariables.HairColorPresetName != "" && EditorVariables.HairColorPresetName != null ){
							GUILayout.Label( EditorVariables.HairColorPresetName, GUILayout.Width (125));
						}
						GUI.color = Red;
						if ( EditorVariables.HairColorPresetName != "" && EditorVariables.HairColorPresetName != null 
								&& GUILayout.Button ( "X", GUILayout.ExpandWidth (false))) 
						{
							 EditorVariables.HairColorPresetName = "";
						}
					}
				}
				#endregion Step4 Hair Color
				
				#region Step4 Wear Color
				if ( ShowTorsoWColor )
				{
					 EditorVariables.TorsoWearTone = new Color( EditorVariables.TorsoWearColor1, EditorVariables.TorsoWearColor2, EditorVariables.TorsoWearColor3,1);
					if (  EditorVariables.TorsoWColorPresetName == "" ||  EditorVariables.TorsoWColorPresetName == null ) EditorVariables.TorsoWearColor =EditorVariables.PickedTorsoWearColor +  EditorVariables.TorsoWearTone;
						EditorVariables.LegsWearTone = new Color(EditorVariables.LegsWearColor1,EditorVariables.LegsWearColor2,EditorVariables.LegsWearColor3,1);
						if ( EditorVariables.LegsWColorPresetName == "" || EditorVariables.LegsWColorPresetName == null  ) EditorVariables.LegsWearColor =EditorVariables.PickedLegsWearColor + EditorVariables.LegsWearTone;
						EditorVariables.FeetWearTone = new Color(EditorVariables.FeetWearColor1,EditorVariables.FeetWearColor2,EditorVariables.FeetWearColor3,1);
						if ( EditorVariables.FeetWColorPresetName == "" || EditorVariables.FeetWColorPresetName == null  ) EditorVariables.FeetWearColor = EditorVariables.PickedFeetWearColor + EditorVariables.FeetWearTone;
						EditorVariables.HeadWearTone = new Color(EditorVariables.HeadWearColor1,EditorVariables.HeadWearColor2,EditorVariables.HeadWearColor3,1);
						if ( EditorVariables.HeadWColorPresetName == "" || EditorVariables.HeadWColorPresetName == null ) EditorVariables.HeadWearColor = EditorVariables.PickedHeadWearColor + EditorVariables.HeadWearTone;
						EditorVariables.HandWearTone = new Color(EditorVariables.HandWearColor1,EditorVariables.HandWearColor2,EditorVariables.HandWearColor3,1);
						if ( EditorVariables.HandWColorPresetName == "" || EditorVariables.HandWColorPresetName == null  ) EditorVariables.HandWearColor = EditorVariables.PickedHandWearColor + EditorVariables.HandWearTone;
						EditorVariables.BeltWearTone = new Color(EditorVariables.BeltWearColor1,EditorVariables.BeltWearColor2,EditorVariables.BeltWearColor3,1);
						if ( EditorVariables.BeltWColorPresetName == "" || EditorVariables.BeltWColorPresetName == null  ) EditorVariables.BeltWearColor = EditorVariables.PickedBeltWearColor + EditorVariables.BeltWearTone;
					
					using (new Horizontal()) {
						GUILayout.Label ( "Head", GUILayout.Width (50));
							EditorVariables.HeadWearColor = EditorGUILayout.ColorField("", EditorVariables.HeadWearColor, GUILayout.Width (50));
						GUILayout.Label ( "Torso", GUILayout.Width (50));
							EditorVariables.TorsoWearColor = EditorGUILayout.ColorField("", EditorVariables.TorsoWearColor, GUILayout.Width (50));
						GUILayout.Label ( "Leg", GUILayout.Width (50));
							EditorVariables.LegsWearColor = EditorGUILayout.ColorField("", EditorVariables.LegsWearColor, GUILayout.Width (50));
					}
					
					using (new Horizontal()) {
						GUILayout.Label ( "Belt", GUILayout.Width (50));
							EditorVariables.BeltWearColor = EditorGUILayout.ColorField("", EditorVariables.BeltWearColor, GUILayout.Width (50));
						GUILayout.Label ( "Hand", GUILayout.Width (50));
							EditorVariables.HandWearColor = EditorGUILayout.ColorField("", EditorVariables.HandWearColor, GUILayout.Width (50));
						GUILayout.Label ( "Feet", GUILayout.Width (50));
							EditorVariables.FeetWearColor = EditorGUILayout.ColorField("", EditorVariables.FeetWearColor, GUILayout.Width (50));
					}
					
					using (new ScrollView(ref scroll)) 
					{
						#region Step4 Head Color
						// title
						GUILayout.Space(5);
						using (new Horizontal()) {
							GUI.color = Color.yellow;
							GUILayout.Label("HeadWear Color", "toolbarbutton", GUILayout.ExpandWidth (true));
						}
						
						if (EditorVariables.HeadWColorPresetName == "" ||EditorVariables.HeadWColorPresetName == null ){		
							using (new Horizontal()) {	
								GUI.color = Color.white;
								if (GUILayout.Button ( "Random", GUILayout.Width (100))) 
								{
										EditorVariables.HeadWearColor1 = UnityEngine.Random.Range(0.05f, 0.9f);
										EditorVariables.HeadWearColor2 = UnityEngine.Random.Range(0.05f, 0.9f);
										EditorVariables.HeadWearColor3 = UnityEngine.Random.Range(0.05f, 0.9f);
								}
									EditorVariables.HeadWearTone = EditorGUILayout.ColorField("",EditorVariables.HeadWearTone, GUILayout.Width (100));
							}
							using (new Horizontal()) {
								GUILayout.Label("HeadWear Tone 1", GUILayout.ExpandWidth (false));
									EditorVariables.HeadWearColor1 = GUILayout.HorizontalSlider(EditorVariables.HeadWearColor1 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.HeadWearColor1.ToString(), GUILayout.Width (50));
							}
							using (new Horizontal()) {
								GUILayout.Label("HeadWear Tone 2", GUILayout.ExpandWidth (false));
									EditorVariables.HeadWearColor2 = GUILayout.HorizontalSlider(EditorVariables.HeadWearColor2 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.HeadWearColor2.ToString(), GUILayout.Width (50));
							}
							using (new Horizontal()) {
								GUILayout.Label("HeadWear Tone 3", GUILayout.ExpandWidth (false));
									EditorVariables.HeadWearColor3 = GUILayout.HorizontalSlider(EditorVariables.HeadWearColor3 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.HeadWearColor3.ToString(), GUILayout.Width (50));
							}
						}		
						using (new Horizontal()) {
							if (EditorVariables.HeadWColorPresetName != "" &&EditorVariables.HeadWColorPresetName != null ) GUI.color = Green;
							else GUI.color = Color.gray;
							if (GUILayout.Button ( "Color Preset", GUILayout.Width (100))) 
							{
								OpenColorPresetWin();
									ColorPreset_Editor.Statut = "ApplyTo";
								ColorPreset_Editor.SelectedElement = "HeadWear";
								ColorPreset_Editor.CurrentElementColor = EditorVariables.HeadWearColor;
									
							}
							if (EditorVariables.HeadWColorPresetName != "" &&EditorVariables.HeadWColorPresetName != null ){
									GUILayout.Label(EditorVariables.HeadWColorPresetName, GUILayout.Width (125));
							}
							GUI.color = Red;
							if (EditorVariables.HeadWColorPresetName != "" &&EditorVariables.HeadWColorPresetName != null 
									&& GUILayout.Button ( "X", GUILayout.ExpandWidth (false))) 
							{
									EditorVariables.HeadWColorPresetName = "";
							}
						}

						#endregion Step4 Head Color
							
						#region Step4 Torso Color
						// title
						GUILayout.Space(5);
						
						using (new Horizontal()) {
							GUI.color = Color.yellow;
							GUILayout.Label("TorsoWear Color", "toolbarbutton", GUILayout.ExpandWidth (true));
						}
						if (  EditorVariables.TorsoWColorPresetName == "" ||  EditorVariables.TorsoWColorPresetName == null  ){	
						
							using (new Horizontal()) {	
								GUI.color = Color.white;
								if (GUILayout.Button ( "Random", GUILayout.Width (100))) 
								{
									 EditorVariables.TorsoWearColor1 = UnityEngine.Random.Range(0.05f, 0.9f);
									 EditorVariables.TorsoWearColor2 = UnityEngine.Random.Range(0.05f, 0.9f);
									 EditorVariables.TorsoWearColor3 = UnityEngine.Random.Range(0.05f, 0.9f);
								}
									 EditorVariables.TorsoWearTone = EditorGUILayout.ColorField("",  EditorVariables.TorsoWearTone, GUILayout.Width (100));
							}
							using (new Horizontal()) {
								GUILayout.Label("TorsoWear Tone 1", GUILayout.ExpandWidth (false));
								 EditorVariables.TorsoWearColor1 = GUILayout.HorizontalSlider( EditorVariables.TorsoWearColor1 ,0.05f, 0.9f );
								GUILayout.Label( EditorVariables.TorsoWearColor1.ToString(), GUILayout.Width (50));
							}
							using (new Horizontal()) {
								GUILayout.Label("TorsoWear Tone 2", GUILayout.ExpandWidth (false));
								 EditorVariables.TorsoWearColor2 = GUILayout.HorizontalSlider( EditorVariables.TorsoWearColor2 ,0.05f, 0.9f );
								GUILayout.Label( EditorVariables.TorsoWearColor2.ToString(), GUILayout.Width (50));
							}
							using (new Horizontal()) {
								GUILayout.Label("TorsoWear Tone 3", GUILayout.ExpandWidth (false));
								 EditorVariables.TorsoWearColor3 = GUILayout.HorizontalSlider( EditorVariables.TorsoWearColor3 ,0.05f, 0.9f );
								GUILayout.Label( EditorVariables.TorsoWearColor3.ToString(), GUILayout.Width (50));
							}
						}
						using (new Horizontal()) {
							if (  EditorVariables.TorsoWColorPresetName != "" &&  EditorVariables.TorsoWColorPresetName != null ) GUI.color = Green;
							else GUI.color = Color.gray;
							if (GUILayout.Button ( "Color Preset", GUILayout.Width (100))) 
							{
								OpenColorPresetWin();
									ColorPreset_Editor.Statut = "ApplyTo";
								ColorPreset_Editor.SelectedElement = "TorsoWear";
								ColorPreset_Editor.CurrentElementColor = EditorVariables.TorsoWearColor;
									
							}
							if (  EditorVariables.TorsoWColorPresetName != "" &&  EditorVariables.TorsoWColorPresetName != null ){
									GUILayout.Label( EditorVariables.TorsoWColorPresetName, GUILayout.Width (125));
							}
							GUI.color = Red;
							if (  EditorVariables.TorsoWColorPresetName != "" &&  EditorVariables.TorsoWColorPresetName != null 
									&& GUILayout.Button ( "X", GUILayout.ExpandWidth (false))) 
							{
								 EditorVariables.TorsoWColorPresetName = "";
							}
						}
						#endregion Step4 Torso Color
							
						#region Step4 Hand Color
						// title
						GUILayout.Space(5);
						
						using (new Horizontal()) {
							GUI.color = Color.yellow;
							GUILayout.Label("HandWear Color", "toolbarbutton", GUILayout.ExpandWidth (true));
						}
						
						
						if ( EditorVariables.HandWColorPresetName == "" || EditorVariables.HandWColorPresetName == null  ){
							using (new Horizontal()) {	
								GUI.color = Color.white;
								if (GUILayout.Button ( "Random", GUILayout.Width (100))) 
								{
										EditorVariables.HandWearColor1 = UnityEngine.Random.Range(0.05f, 0.9f);
										EditorVariables.HandWearColor2 = UnityEngine.Random.Range(0.05f, 0.9f);
										EditorVariables.HandWearColor3 = UnityEngine.Random.Range(0.05f, 0.9f);
								}
									EditorVariables.HandWearTone = EditorGUILayout.ColorField("", EditorVariables.HandWearTone, GUILayout.Width (100));
							}
							using (new Horizontal()) {
								GUILayout.Label("HandWear Tone 1", GUILayout.ExpandWidth (false));
									EditorVariables.HandWearColor1 = GUILayout.HorizontalSlider(EditorVariables.HandWearColor1 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.HandWearColor1.ToString(), GUILayout.Width (50));
							}
							using (new Horizontal()) {
								GUILayout.Label("HandWear Tone 2", GUILayout.ExpandWidth (false));
									EditorVariables.HandWearColor2 = GUILayout.HorizontalSlider(EditorVariables.HandWearColor2 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.HandWearColor2.ToString(), GUILayout.Width (50));
							}
							using (new Horizontal()) {
								GUILayout.Label("HandWear Tone 3", GUILayout.ExpandWidth (false));
									EditorVariables.HandWearColor3 = GUILayout.HorizontalSlider(EditorVariables.HandWearColor3 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.HandWearColor3.ToString(), GUILayout.Width (50));
							}
						}
						using (new Horizontal()) {
							if ( EditorVariables.HandWColorPresetName != "" && EditorVariables.HandWColorPresetName != null ) GUI.color = Green;
							else GUI.color = Color.gray;
							if (GUILayout.Button ( "Color Preset", GUILayout.Width (100))) 
							{
								OpenColorPresetWin();
								ColorPreset_Editor.Statut = "ApplyTo";
								ColorPreset_Editor.SelectedElement = "HandWear";
								ColorPreset_Editor.CurrentElementColor = EditorVariables.HandWearColor;
							}
							if ( EditorVariables.HandWColorPresetName != "" && EditorVariables.HandWColorPresetName != null ){
									GUILayout.Label(EditorVariables.HandWColorPresetName, GUILayout.Width (125));
							}
							GUI.color = Red;
							if ( EditorVariables.HandWColorPresetName != "" && EditorVariables.HandWColorPresetName != null 
									&& GUILayout.Button ( "X", GUILayout.ExpandWidth (false))) 
							{
									EditorVariables.HandWColorPresetName = "";
							}
						}	
						#endregion Step4 Hand Color
						
						#region Step4 Belt Color
						// title
						GUILayout.Space(5);
						using (new Horizontal()) {
							GUI.color = Color.yellow;
							GUILayout.Label("BeltWear Color", "toolbarbutton", GUILayout.ExpandWidth (true));
						}
						
						if ( EditorVariables.BeltWColorPresetName == "" || EditorVariables.BeltWColorPresetName == null  ){
							
							using (new Horizontal()) {	
								GUI.color = Color.white;
								if (GUILayout.Button ( "Random", GUILayout.Width (100))) 
								{
										EditorVariables.BeltWearColor1 = UnityEngine.Random.Range(0.05f, 0.9f);
										EditorVariables.BeltWearColor2 = UnityEngine.Random.Range(0.05f, 0.9f);
										EditorVariables.BeltWearColor3 = UnityEngine.Random.Range(0.05f, 0.9f);
								}
									EditorVariables.BeltWearTone = EditorGUILayout.ColorField("", EditorVariables.BeltWearTone, GUILayout.Width (100));
							}
							using (new Horizontal()) {
								GUILayout.Label("BeltWear Tone 1", GUILayout.ExpandWidth (false));
									EditorVariables.BeltWearColor1 = GUILayout.HorizontalSlider(EditorVariables.BeltWearColor1 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.BeltWearColor1.ToString(), GUILayout.Width (50));
							}
							using (new Horizontal()) {
								GUILayout.Label("BeltWear Tone 2", GUILayout.ExpandWidth (false));
									EditorVariables.BeltWearColor2 = GUILayout.HorizontalSlider(EditorVariables.BeltWearColor2 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.BeltWearColor2.ToString(), GUILayout.Width (50));
							}
							using (new Horizontal()) {
								GUILayout.Label("BeltWear Tone 3", GUILayout.ExpandWidth (false));
									EditorVariables.BeltWearColor3 = GUILayout.HorizontalSlider(EditorVariables.BeltWearColor3 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.BeltWearColor3.ToString(), GUILayout.Width (50));
							}
						}
						using (new Horizontal()) {
							if ( EditorVariables.BeltWColorPresetName != "" && EditorVariables.BeltWColorPresetName != null ) GUI.color = Green;
							else GUI.color = Color.gray;
							if (GUILayout.Button ( "Color Preset", GUILayout.Width (100))) 
							{
								OpenColorPresetWin();
									ColorPreset_Editor.Statut = "ApplyTo";
								ColorPreset_Editor.SelectedElement = "BeltWear";
								ColorPreset_Editor.CurrentElementColor = EditorVariables.BeltWearColor;
									
							}
							if ( EditorVariables.BeltWColorPresetName != "" && EditorVariables.BeltWColorPresetName != null ){
									GUILayout.Label(EditorVariables.BeltWColorPresetName, GUILayout.Width (125));
							}
							GUI.color = Red;
							if ( EditorVariables.BeltWColorPresetName != "" && EditorVariables.BeltWColorPresetName != null 
									&& GUILayout.Button ( "X", GUILayout.ExpandWidth (false))) 
							{
									EditorVariables.BeltWColorPresetName = "";
							}
						}	
						#endregion Step4 Belt Color
						
						#region Step4 Legs Color
						// title
						GUILayout.Space(5);
						using (new Horizontal()) {
							GUI.color = Color.yellow;
							GUILayout.Label("LegsWear Color", "toolbarbutton", GUILayout.ExpandWidth (true));
						}
						
							if ( EditorVariables.LegsWColorPresetName == "" || EditorVariables.LegsWColorPresetName == null  ){	
							
							using (new Horizontal()) {	
								GUI.color = Color.white;
								if (GUILayout.Button ( "Random", GUILayout.Width (100))) 
								{
										EditorVariables.LegsWearColor1 = UnityEngine.Random.Range(0.05f, 0.9f);
										EditorVariables.LegsWearColor2 = UnityEngine.Random.Range(0.05f, 0.9f);
										EditorVariables.LegsWearColor3 = UnityEngine.Random.Range(0.05f, 0.9f);
								}
									EditorVariables.LegsWearTone = EditorGUILayout.ColorField("", EditorVariables.LegsWearTone, GUILayout.Width (100));
							}
							using (new Horizontal()) {
								GUILayout.Label("LegsWear Tone 1", GUILayout.ExpandWidth (false));
									EditorVariables.LegsWearColor1 = GUILayout.HorizontalSlider(EditorVariables.LegsWearColor1 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.LegsWearColor1.ToString(), GUILayout.Width (50));
							}
							using (new Horizontal()) {
								GUILayout.Label("LegsWear Tone 2", GUILayout.ExpandWidth (false));
									EditorVariables.LegsWearColor2 = GUILayout.HorizontalSlider(EditorVariables.LegsWearColor2 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.LegsWearColor2.ToString(), GUILayout.Width (50));
							}
							using (new Horizontal()) {
								GUILayout.Label("LegsWear Tone 3", GUILayout.ExpandWidth (false));
									EditorVariables.LegsWearColor3 = GUILayout.HorizontalSlider(EditorVariables.LegsWearColor3 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.LegsWearColor3.ToString(), GUILayout.Width (50));
							}
						}
						using (new Horizontal()) {
								if ( EditorVariables.LegsWColorPresetName != "" && EditorVariables.LegsWColorPresetName != null ) GUI.color = Green;
							else GUI.color = Color.gray;
							if (GUILayout.Button ( "Color Preset", GUILayout.Width (100))) 
							{
								OpenColorPresetWin();
									ColorPreset_Editor.Statut = "ApplyTo";
								ColorPreset_Editor.SelectedElement = "LegsWear";
								ColorPreset_Editor.CurrentElementColor = EditorVariables.LegsWearColor;
									
							}
								if ( EditorVariables.LegsWColorPresetName != "" && EditorVariables.LegsWColorPresetName != null ){
									GUILayout.Label(EditorVariables.LegsWColorPresetName, GUILayout.Width (125));
							}
							GUI.color = Red;
								if ( EditorVariables.LegsWColorPresetName != "" && EditorVariables.LegsWColorPresetName != null 
									&& GUILayout.Button ( "X", GUILayout.ExpandWidth (false))) 
							{
									EditorVariables.LegsWColorPresetName = "";
							}
						}
						#endregion Step4 Legs Color
							
						#region Step4 Feet Color
						// title
						GUILayout.Space(5);
						using (new Horizontal()) {
							GUI.color = Color.yellow;
							GUILayout.Label("FeetWear Color", "toolbarbutton", GUILayout.ExpandWidth (true));
						}
						
						GUILayout.Space(5);
							if ( EditorVariables.FeetWColorPresetName == "" || EditorVariables.FeetWColorPresetName == null  ){	
							
							using (new Horizontal()) {	
								GUI.color = Color.white;
								if (GUILayout.Button ( "Random", GUILayout.Width (100))) 
								{
										EditorVariables.FeetWearColor1 = UnityEngine.Random.Range(0.05f, 0.9f);
										EditorVariables.FeetWearColor2 = UnityEngine.Random.Range(0.05f, 0.9f);
										EditorVariables.FeetWearColor3 = UnityEngine.Random.Range(0.05f, 0.9f);
								}
									EditorVariables.FeetWearTone = EditorGUILayout.ColorField("", EditorVariables.FeetWearTone, GUILayout.Width (100));
							}
							using (new Horizontal()) {
								GUILayout.Label("FeetWear Tone 1", GUILayout.ExpandWidth (false));
									EditorVariables.FeetWearColor1 = GUILayout.HorizontalSlider(EditorVariables.FeetWearColor1 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.FeetWearColor1.ToString(), GUILayout.Width (50));
							}
							using (new Horizontal()) {
								GUILayout.Label("FeetWear Tone 2", GUILayout.ExpandWidth (false));
									EditorVariables.FeetWearColor2 = GUILayout.HorizontalSlider(EditorVariables.FeetWearColor2 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.FeetWearColor2.ToString(), GUILayout.Width (50));
							}
							using (new Horizontal()) {
								GUILayout.Label("FeetWear Tone 3", GUILayout.ExpandWidth (false));
									EditorVariables.FeetWearColor3 = GUILayout.HorizontalSlider(EditorVariables.FeetWearColor3 ,0.05f, 0.9f );
									GUILayout.Label(EditorVariables.FeetWearColor3.ToString(), GUILayout.Width (50));
							}
						}
						using (new Horizontal()) {
								if ( EditorVariables.FeetWColorPresetName != "" && EditorVariables.FeetWColorPresetName != null ) GUI.color = Green;
							else GUI.color = Color.gray;
							if (GUILayout.Button ( "Color Preset", GUILayout.Width (100))) 
							{
								OpenColorPresetWin();
									ColorPreset_Editor.Statut = "ApplyTo";
								ColorPreset_Editor.SelectedElement = "FeetWear";
								ColorPreset_Editor.CurrentElementColor = EditorVariables.FeetWearColor;
									
							}
								if ( EditorVariables.FeetWColorPresetName != "" && EditorVariables.FeetWColorPresetName != null ){
									GUILayout.Label(EditorVariables.FeetWColorPresetName, GUILayout.Width (125));
							}
							GUI.color = Red;
								if ( EditorVariables.FeetWColorPresetName != "" && EditorVariables.FeetWColorPresetName != null 
									&& GUILayout.Button ( "X", GUILayout.ExpandWidth (false))) 
							{
									EditorVariables.FeetWColorPresetName = "";
							}
						}	
						#endregion Step4 Feet Color
					}
				}
				#endregion Step4 Wear Color
			}
			#endregion Step4
			
			#region Step5 Shape
			if ( Step5 ) {
				// Navigate
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					if (GUILayout.Button ( "<", GUILayout.ExpandWidth (false))) 
					{		Step4 = true ;	Step5 = false ;}
					GUI.color = Red;
					if (GUILayout.Button ( "Reset All", GUILayout.ExpandWidth (true))) 
					{		Step0 = true ;	}
					GUI.color = Color.yellow;
				}
				GUILayout.Space(5);
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					if ( GUILayout.Button ( "Randomize and go Next", GUILayout.ExpandWidth (true))) 
					{
						EditorVariables.DK_UMACrowd.Randomize.RanShape = true;
						Step6 = true ;	Step5 = false ;	
					}
					GUI.color = Green;
					if ( GUILayout.Button ( "Apply and go Next", GUILayout.ExpandWidth (true))) 
					{
						Step6 = true ;	Step5 = false ;	
					}
				}
		
				GUI.color = Color.white;
				if ( Helper ) GUILayout.TextField("Setup the shape of your model." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

				// Height
				using (new Horizontal()) {
					GUILayout.Label ( "Height :", GUILayout.Width (45));
					if ( EditorVariables.DK_UMACrowd.Randomize.Height == "Random" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Random", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Height = "Random";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Height == "Low" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Low", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Height = "Low";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Height == "Medium" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Medium", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Height = "Medium";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Height == "High" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "High", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Height = "High";
					}
					
				}
				// Weight
				using (new Horizontal()) {
					GUILayout.Label ( "Weight", GUILayout.Width (45));
					if ( EditorVariables.DK_UMACrowd.Randomize.Weight == "Random" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Random", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Weight = "Random";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Weight == "Low" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Low", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Weight = "Low";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Weight == "Medium" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Medium", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Weight = "Medium";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Weight == "High" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "High", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Weight = "High";
					}
					
				}
				// Muscles
				using (new Horizontal()) {
					GUILayout.Label ( "Muscles :", GUILayout.Width (45));
					if ( EditorVariables.DK_UMACrowd.Randomize.Muscles == "Random" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Random", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Muscles = "Random";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Muscles == "Low" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Low", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Muscles = "Low";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Muscles == "Medium" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Medium", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Muscles = "Medium";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Muscles == "High" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "High", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Muscles = "High";
					}
				}
				// Hair
				using (new Horizontal()) {
					GUILayout.Label ( "Hair :", GUILayout.Width (45));
					if ( EditorVariables.DK_UMACrowd.Randomize.Hair == "Random" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Random", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Hair = "Random";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Hair == "None" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "None", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Hair = "None";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Hair.Contains("Simple") ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Simple", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Hair = "Simple";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Hair == "Simple+Modules" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "+Modules", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Hair = "Simple+Modules";
					}
				}
				// Pilosity
				using (new Horizontal()) {
					GUILayout.Label ( "Pilosity :", GUILayout.Width (45));
					if ( EditorVariables.DK_UMACrowd.Randomize.Pilosity == "Random" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Random", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Pilosity = "Random";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Pilosity == "None" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "None", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Pilosity = "None";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Pilosity == "Low" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Low", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Pilosity = "Low";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Pilosity == "Medium" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Medium", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Pilosity = "Medium";
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Pilosity == "High" ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "High", GUILayout.ExpandWidth (true))){
						EditorVariables.DK_UMACrowd.Randomize.Pilosity = "High";
					}
				}
				using (new Horizontal()) {
					// lips
					if ( EditorVariables.DK_UMACrowd.Randomize.Lips == true ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Lips", GUILayout.ExpandWidth (true))){
						if ( EditorVariables.DK_UMACrowd.Randomize.Lips == true ) EditorVariables.DK_UMACrowd.Randomize.Lips = false;
							else EditorVariables.DK_UMACrowd.Randomize.Lips = true;
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Lips == true ){
						GUI.color = Color.white;
						string _value = EditorVariables.DK_UMACrowd.Randomize.LipsChance.ToString();
						_value= GUILayout.TextField(_value, 3, GUILayout.Width (30));
						_value= Regex.Replace(_value, "[^0-9]", "");
						if ( _value == "" ) _value = "1";
						EditorVariables.DK_UMACrowd.Randomize.LipsChance = Convert.ToInt32(_value);	
					}
					// makeup
					if ( EditorVariables.DK_UMACrowd.Randomize.Makeup == true ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Makeup", GUILayout.ExpandWidth (true))){
						if ( EditorVariables.DK_UMACrowd.Randomize.Makeup == true ) EditorVariables.DK_UMACrowd.Randomize.Makeup = false;
							else EditorVariables.DK_UMACrowd.Randomize.Makeup = true;
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Makeup == true ){
						GUI.color = Color.white;
						string _value = EditorVariables.DK_UMACrowd.Randomize.MakeupChance.ToString();
						_value= GUILayout.TextField(_value, 3, GUILayout.Width (30));
						_value= Regex.Replace(_value, "[^0-9]", "");
						if ( _value == "" ) _value = "1";
						EditorVariables.DK_UMACrowd.Randomize.MakeupChance = Convert.ToInt32(_value);	
					}
					// tatoo
					if ( EditorVariables.DK_UMACrowd.Randomize.Tatoo == true ) GUI.color = Green;
					else GUI.color = Color.white;
					if ( GUILayout.Button ( "Tatoo", GUILayout.ExpandWidth (true))){
						if ( EditorVariables.DK_UMACrowd.Randomize.Tatoo == true ) EditorVariables.DK_UMACrowd.Randomize.Tatoo = false;
						else EditorVariables.DK_UMACrowd.Randomize.Tatoo = true;
					}
					if ( EditorVariables.DK_UMACrowd.Randomize.Tatoo == true ){
						GUI.color = Color.white;
						string _value = EditorVariables.DK_UMACrowd.Randomize.TatooChance.ToString();
						_value= GUILayout.TextField(_value, 3, GUILayout.Width (30));
						_value= Regex.Replace(_value, "[^0-9]", "");
						if ( _value == "" ) _value = "1";
						EditorVariables.DK_UMACrowd.Randomize.TatooChance = Convert.ToInt32(_value);	
					}
				}
			}
			#endregion Step5 Shape
			
			#region Step6 Elements
			if ( Step6 ) {
				// Navigate
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					if (GUILayout.Button ( "<", GUILayout.ExpandWidth (false))) 
					{		Step5 = true ;	Step6 = false ;	}
					GUI.color = Red;
					if (GUILayout.Button ( "Reset All", GUILayout.ExpandWidth (true))) 
					{		Step0 = true ;	}
					GUI.color = Color.yellow;
				}
				GUILayout.Space(5);
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					if ( GUILayout.Button ( "Randomize and go Next", GUILayout.ExpandWidth (true))) 
					{
						EditorVariables.DK_UMACrowd.Randomize.RanElements = true;
						Step7 = true ;	
						// TO COMPLET
					}
					GUI.color = Green;
					if ( GUILayout.Button ( "Apply and go Next", GUILayout.ExpandWidth (true))) 
					{
						Step7 = true ;	Step6 = false ;	
					}
				}
				GUILayout.Label ( "Here you can setup the wear creation.", GUILayout.ExpandWidth (true));
				GUILayout.Space(5);

				using (new ScrollView(ref scroll)) 
				{
				using (new Horizontal()) {
					if ( EditorVariables.DK_UMACrowd.Wears.RanWearChoice == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Randomize Wear choice", GUILayout.ExpandWidth (true))) 
					{
						if ( EditorVariables.DK_UMACrowd.Wears.RanWearChoice == true ) EditorVariables.DK_UMACrowd.Wears.RanWearChoice = false;
						else EditorVariables.DK_UMACrowd.Wears.RanWearChoice = true;
					}
				}

				using (new Horizontal()) {
					GUILayout.Label ( "Wear activation :", GUILayout.ExpandWidth (false));
					if ( EditorVariables.DK_UMACrowd.Wears.RanWearYesMax == 0 ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "No", GUILayout.ExpandWidth (false))) 
					{
						EditorVariables.DK_UMACrowd.Wears.RanWearYesMax = 0;
					}
					if ( EditorVariables.DK_UMACrowd.Wears.RanWearYesMax >= 1 ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Yes", GUILayout.ExpandWidth (false))) 
					{
						EditorVariables.DK_UMACrowd.Wears.RanWearYesMax = 1;
					}
					if ( EditorVariables.DK_UMACrowd.Wears.RanWearAct == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Randomize", GUILayout.ExpandWidth (false))) 
					{
						if ( EditorVariables.DK_UMACrowd.Wears.RanWearAct == true ) EditorVariables.DK_UMACrowd.Wears.RanWearAct = false;
						else EditorVariables.DK_UMACrowd.Wears.RanWearAct = true;
					}
				}
				using (new Horizontal()) {
					if ( EditorVariables.DK_UMACrowd.Wears.RanWearAct == true ){
						GUILayout.Label ( "Max chance to activate :", GUILayout.ExpandWidth (false));
						string _value = EditorVariables.DK_UMACrowd.Wears.RanWearYesMax.ToString();
						_value= GUILayout.TextField(_value, 3, GUILayout.Width (30));
						_value= Regex.Replace(_value, "[^0-9]", "");
						if ( _value == "" ) _value = "1";
						EditorVariables.DK_UMACrowd.Wears.RanWearYesMax = Convert.ToInt32(_value);	
					}
				}
				using (new Horizontal()) {
					GUILayout.Label ( "Wear types :", GUILayout.ExpandWidth (false));
					if ( EditorVariables.DK_UMACrowd.Wears.RanUnderwearChoice == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Underwear", GUILayout.ExpandWidth (true))) 
					{
						if ( EditorVariables.DK_UMACrowd.Wears.RanUnderwearChoice == true ) EditorVariables.DK_UMACrowd.Wears.RanUnderwearChoice = false;
						else EditorVariables.DK_UMACrowd.Wears.RanUnderwearChoice = true;
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearOverlays == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Overlays", GUILayout.ExpandWidth (true))) 
					{
						if ( EditorVariables.DK_UMACrowd.Wears.WearOverlays == true ) EditorVariables.DK_UMACrowd.Wears.WearOverlays = false;
						else EditorVariables.DK_UMACrowd.Wears.WearOverlays = true;
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearMeshes == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Meshes", GUILayout.ExpandWidth (true))) 
					{
						if ( EditorVariables.DK_UMACrowd.Wears.WearMeshes == true ) EditorVariables.DK_UMACrowd.Wears.WearMeshes = false;
						else EditorVariables.DK_UMACrowd.Wears.WearMeshes = true;
					}
				}

				if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList.Count == 6 ) using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label ( "Wear Weights :", GUILayout.Width (100));
					GUI.color = Color.yellow;
					if ( GUILayout.Button ( "Add all", GUILayout.ExpandWidth (true))) 
					{
						EditorVariables.DK_UMACrowd.Wears.WearWeightList.Clear();
								EditorVariables.DK_UMACrowd.CreateWeights();
					}
					if ( GUILayout.Button ( "Remove All", GUILayout.ExpandWidth (true))) 
					{
								EditorVariables.DK_UMACrowd.RemoveWeights();
					}
				}
				if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList.Count == 6 ) using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label ( "Add for All :", GUILayout.Width (100));
					GUI.color = Color.yellow;
					if ( GUILayout.Button ( "Light", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Light";
						for (int i = 0; i <  EditorVariables.DK_UMACrowd.Wears.WearWeightList.Count; i ++) {
							if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[i].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[i].Weights.Add(tmpWearWeight);
						}
						
					}
					if ( GUILayout.Button ( "Medium", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Medium";
						for (int i = 0; i <  EditorVariables.DK_UMACrowd.Wears.WearWeightList.Count; i ++) {
							if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[i].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[i].Weights.Add(tmpWearWeight);
						}
					}

					if ( GUILayout.Button ( "High", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "High";
						for (int i = 0; i <  EditorVariables.DK_UMACrowd.Wears.WearWeightList.Count; i ++) {
							if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[i].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[i].Weights.Add(tmpWearWeight);
						}
					}

					if ( GUILayout.Button ( "Heavy", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Heavy";
						for (int i = 0; i <  EditorVariables.DK_UMACrowd.Wears.WearWeightList.Count; i ++) {
							if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[i].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[i].Weights.Add(tmpWearWeight);
						}
					}
				}
				if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList.Count == 6 ) using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Head Weight :", GUILayout.Width (100));
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Contains("Light") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Light", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Light";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Remove(tmpWearWeight);

					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Contains("Medium") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Medium", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Medium";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Contains("High") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "High", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "High";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Contains("Heavy") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Heavy", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Heavy";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[0].Weights.Remove(tmpWearWeight);
					}
				}
				if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList.Count == 6 ) using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Torso Weight :", GUILayout.Width (100));
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Contains("Light") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Light", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Light";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Remove(tmpWearWeight);
						
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Contains("Medium") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Medium", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Medium";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Contains("High") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "High", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "High";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Contains("Heavy") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Heavy", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Heavy";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[1].Weights.Remove(tmpWearWeight);
					}
				}
				if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList.Count == 6 ) using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Hands Weight :", GUILayout.Width (100));
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Contains("Light") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Light", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Light";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Remove(tmpWearWeight);
						
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Contains("Medium") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Medium", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Medium";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Contains("High") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "High", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "High";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Contains("Heavy") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Heavy", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Heavy";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[2].Weights.Remove(tmpWearWeight);
					}
				}
				if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList.Count == 6 ) using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Legs Weight :", GUILayout.Width (100));
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Contains("Light") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Light", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Light";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Contains("Medium") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Medium", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Medium";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Contains("High") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "High", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "High";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Contains("Heavy") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Heavy", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Heavy";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[3].Weights.Remove(tmpWearWeight);
					}
				}
				if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList.Count == 6 ) using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Feet Weight :", GUILayout.Width (100));
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Contains("Light") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Light", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Light";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Contains("Medium") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Medium", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Medium";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Contains("High") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "High", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "High";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Contains("Heavy") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Heavy", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Heavy";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[4].Weights.Remove(tmpWearWeight);
					}
				}
				if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList.Count == 6 ) using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Shoulder Weight :", GUILayout.Width (100));
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Contains("Light") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Light", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Light";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Contains("Medium") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Medium", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Medium";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Contains("High") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "High", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "High";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Remove(tmpWearWeight);
					}
					if ( EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Contains("Heavy") == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if ( GUILayout.Button ( "Heavy", GUILayout.ExpandWidth (true))) 
					{
						tmpWearWeight = "Heavy";
						if (EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Contains(tmpWearWeight) == false ) EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Add(tmpWearWeight);
						else EditorVariables.DK_UMACrowd.Wears.WearWeightList[5].Weights.Remove(tmpWearWeight);
					}
				}
				}
			}
			#endregion Step6 Elements
			
			#region Step7 Create
			if ( Step7 ) {
				// Navigate
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					if (GUILayout.Button ( "<", GUILayout.ExpandWidth (false))) 
					{		Step6 = true ;	Step7 = false ;	}
					GUI.color = Red;
					if (GUILayout.Button ( "Reset All", GUILayout.ExpandWidth (true))) 
					{		Step0 = true ;	}
					GUI.color = Color.yellow;
				}
				GUILayout.Space(5);
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					if (MultipleUMASetup && GUILayout.Button ( "Generate Crowd", GUILayout.ExpandWidth (true))) 
					{
						GameObject DKUMAGeneratorObj = GameObject.Find("DKUMAGenerator");
						EditorVariables.DK_DKUMAGenerator =  EditorVariables.DKUMAGeneratorObj.GetComponent<DKUMAGenerator>();
							EditorVariables.DK_DKUMAGenerator.AvatarSavePath = EditorVariables.AvatarSavePath;
						EditorVariables.DK_DKUMAGenerator.umaDirtyList.Clear();
						Step0 = true ;
						Step1 = false ;
						MultipleUMASetup = false ;
						EditorVariables.SingleORMulti = true ;
					//	DK_UMACrowd.GeneratorMode = "Preset";
						DK_UMACrowd.OverlayLibraryObj = EditorVariables.OverlayLibraryObj;
						DK_UMACrowd.DKSlotLibraryObj = EditorVariables.DKSlotLibraryObj;
						DK_UMACrowd.RaceLibraryObj = EditorVariables.RaceLibraryObj;
						EditorVariables.DK_UMACrowd.generateLotsUMA = true;
						EditorVariables.DK_UMACrowd.umaTimerEnd = 0;
					}
					GUI.color = Green;
					if ( !MultipleUMASetup && GUILayout.Button ( "Generate Model", GUILayout.ExpandWidth (true))) 
					{
						GameObject DKUMAGeneratorObj = GameObject.Find("DKUMAGenerator");
						EditorVariables.DK_DKUMAGenerator =  EditorVariables.DKUMAGeneratorObj.GetComponent<DKUMAGenerator>();
							EditorVariables.DK_DKUMAGenerator.AvatarSavePath = EditorVariables.AvatarSavePath;
						EditorVariables.DK_DKUMAGenerator.umaDirtyList.Clear();
						EditorVariables.DK_UMACrowd.RaceAndGender.RaceDone = false;
						Step0 = true ;
						EditorVariables.DK_DKUMAGenerator.Awake();
						DK_UMACrowd.OverlayLibraryObj = EditorVariables.OverlayLibraryObj;
						DK_UMACrowd.DKSlotLibraryObj = EditorVariables.DKSlotLibraryObj;
						DK_UMACrowd.RaceLibraryObj = EditorVariables.RaceLibraryObj;
						EditorVariables.DK_UMACrowd.LaunchGenerateUMA();
				
						CreatingUMA = true;
						if ( CreatingUMA == true ) 
						{
								EditorVariables.UMAModel = GameObject.Find("New UMA Model");
								if (EditorVariables.UMAModel != null ) 
							{
									EditorVariables.UMAModel.name = ("New UMA Model (Rename it)");
									EditorVariables.UMAModel.transform.parent =EditorVariables.MFSelectedList.transform;
								CreatingUMA = false;
									Selection.activeGameObject = EditorVariables.UMAModel;
								
							}
						}
					}
				}
				using (new Horizontal()) {	
					// Flesh Variation
					GUI.color = Color.white;
					GUILayout.Label("Color Variation :", GUILayout.Width (100));
					GUILayout.Label("Flesh", GUILayout.ExpandWidth (false));
					EditorVariables.DK_UMACrowd.Colors.AdjRanMaxi = GUILayout.HorizontalSlider(EditorVariables.DK_UMACrowd.Colors.AdjRanMaxi ,0,0.5f);
					GUILayout.Label(EditorVariables.DK_UMACrowd.Colors.AdjRanMaxi.ToString(), GUILayout.Width (40));
				}
				using (new Horizontal()) {
					// Hair Variation
					GUILayout.Space(110);
					GUI.color = Color.white;
					GUILayout.Label("Hair", GUILayout.ExpandWidth (false));
					EditorVariables.DK_UMACrowd.Colors.HairAdjRanMaxi = GUILayout.HorizontalSlider(EditorVariables.DK_UMACrowd.Colors.HairAdjRanMaxi ,0,0.5f);
					GUILayout.Label(EditorVariables.DK_UMACrowd.Colors.HairAdjRanMaxi.ToString(), GUILayout.Width (40));
				}
				using (new Horizontal()) {
					// wear Variation
					GUILayout.Space(110);
					GUI.color = Color.white;
					GUILayout.Label("Wear", GUILayout.ExpandWidth (false));
					EditorVariables.DK_UMACrowd.Colors.WearAdjRanMaxi = GUILayout.HorizontalSlider(EditorVariables.DK_UMACrowd.Colors.WearAdjRanMaxi ,0,0.5f);
					GUILayout.Label(EditorVariables.DK_UMACrowd.Colors.WearAdjRanMaxi.ToString(), GUILayout.Width (40));
				}
				GUILayout.Space(5);
				GUI.color = Color.white ;
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Race Library :", GUILayout.Width (110));
					GUI.color = Color.white;
					GUILayout.TextField ( EditorVariables.RaceLibraryObj.name, GUILayout.ExpandWidth (true));
					if ( GUILayout.Button ( "Change", GUILayout.Width (60))){
						OpenLibrariesWindow();
						ChangeLibrary.CurrentLibN = EditorVariables.RaceLibraryObj.name;
						ChangeLibrary.CurrentLibrary = EditorVariables.RaceLibraryObj;
						ChangeLibrary.Action = "";
					}
				}
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Slot Library :", GUILayout.Width (110));
					GUI.color = Color.white;
					GUILayout.TextField ( EditorVariables.DKSlotLibraryObj.name, GUILayout.ExpandWidth (true));
					if ( GUILayout.Button ( "Change", GUILayout.Width (60))){
						OpenLibrariesWindow();
						ChangeLibrary.CurrentLibN = EditorVariables.DKSlotLibraryObj.name;
						ChangeLibrary.CurrentLibrary = EditorVariables.DKSlotLibraryObj;
						ChangeLibrary.Action = "";

					}
				}
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Overlay Library :", GUILayout.Width (110));
					GUI.color = Color.white;
					GUILayout.TextField ( EditorVariables.OverlayLibraryObj.name, GUILayout.ExpandWidth (true));
					if ( GUILayout.Button ( "Change", GUILayout.Width (60))){
						OpenLibrariesWindow();
						ChangeLibrary.CurrentLibN = EditorVariables.OverlayLibraryObj.name;
						ChangeLibrary.CurrentLibrary = EditorVariables.OverlayLibraryObj;
						ChangeLibrary.Action = "";

					}
				}	
				GUILayout.Space(5);
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "UMA Crowd :", GUILayout.Width (110));
					GUI.color = Color.white;
					GUILayout.TextField ( EditorVariables.UMACrowdObj.name, GUILayout.ExpandWidth (true));
				}
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Generator :", GUILayout.Width (110));
					GUI.color = Color.white;
					GUILayout.TextField ( EditorVariables.DKUMAGeneratorObj.name, GUILayout.ExpandWidth (true));
				}	
			}
			#endregion Step7 Create
			#endregion Create buttons list
			
		}
		#endregion Create
			
		#region Model List

		#endregion Model List	
		 
		#region UMA
		if ( Selection.activeGameObject
		    && ( Selection.activeGameObject.GetComponent("DKUMAData") as DKUMAData == true 
		    || (Selection.activeGameObject.transform.parent && Selection.activeGameObject.transform.parent.GetComponent("DKUMAData") as DKUMAData == true )
		    || (Selection.activeGameObject.transform.parent && Selection.activeGameObject.transform.parent.parent && Selection.activeGameObject.transform.parent.parent.GetComponent("DKUMAData") as DKUMAData == true)
		    || ( Selection.activeGameObject.GetComponentInChildren<DKUMAData>() as DKUMAData == true ) )
			&& showModify == true )
		{
				GameObject UMACustomObj = GameObject.Find("DKUMACustomization");
			
				if ( UMACustomObj != null ) 
			
				{
					EditorVariables.DK_DKUMACustomization =  UMACustomObj.GetComponent<DKUMACustomization>();
					// parent
					
						
					if ( Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<DK_Model>() != null ) {
						foreach ( Transform Child in Selection.activeGameObject.transform ) {
							if ( Child.gameObject.GetComponent< DKUMAData >() != null )
							{
								EditorVariables.DK_DKUMACustomization.EditedModel = Child;
								EditedModel = Child;
							}
						}
					}

					else if ( Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<DKUMAData>() != null )

					{
						EditorVariables.DK_DKUMACustomization.EditedModel =  Selection.activeGameObject.transform;
						EditedModel =  Selection.activeGameObject.transform;

					}

					else if ( Selection.activeGameObject != null &&  Selection.activeGameObject.transform.GetComponentInParent<DKUMAData>() != null )
					{
						EditorVariables.DK_DKUMACustomization.EditedModel =  Selection.activeGameObject.transform.parent;
						EditedModel =  Selection.activeGameObject.transform.parent;
					
					}
				}
				
			
	
			#region Menu
			if ( EditedModel == null || Selection.activeGameObject == null ) using (new HorizontalCentered()) {
				GUI.color = Color.yellow ;
				GUILayout.Label("Please use the DK UMA Browser and select a model to edit.", GUILayout.ExpandWidth (false));
			}
			GUI.color = new Color (0.8f, 1f, 0.8f, 1) ;
				if ( !ShowDKLibraries && EditedModel && Selection.activeGameObject && EditedModel == Selection.activeGameObject.transform ){ 
					GUI.color = Color.white ;
					if ( Helper ) GUILayout.TextField("You can convert your avatar for it to be used by the basic UMA engine." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					if ( Helper ) GUILayout.TextField("The avatar will be saved in a text file that can be loaded by UMA as a Save/Load file." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					GUI.color = Color.yellow ;
					if ( Helper ) GUILayout.TextField("Remember that when an Avatar is converted to UMA, its DNA will be afflicted by the UMA DNA Behaviour." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

					using (new Horizontal()) {
						GUI.color = Color.yellow;
					/*	if ( GUILayout.Button ( "Convert to UMA", GUILayout.ExpandWidth (true))) 
						{
							DKUMAData _DKUMAData = EditedModel.GetComponentInChildren<DKUMAData>();
							TransposeDK2UMA _TransposeDK2UMA = _DKUMAData.gameObject.AddComponent<TransposeDK2UMA>();
							_TransposeDK2UMA.Launch ( _DKUMAData.gameObject.GetComponent<DK_RPG_UMA>(), 
							                         EditorVariables.DK_UMACrowd, 
							                         _DKUMAData.streamedUMA );
						}*/
						if ( !ChangeOverlay )using (new Horizontal()) {
							GUI.color = Green;
							if (!AddSlot &&  GUILayout.Button("Apply to Avatar", GUILayout.ExpandWidth (true))){
								EditorSaveAvatar.SaveAvatar();
							}
							if (!AddSlot &&  GUILayout.Button("Rebuild", GUILayout.ExpandWidth (true))){
								DKUMAData _DKUMAData = EditedModel.GetComponentInChildren<DKUMAData>();
								DK_RPG_ReBuild _DK_RPG_ReBuild = _DKUMAData.transform.gameObject.AddComponent<DK_RPG_ReBuild>();;
								_DK_RPG_ReBuild.Launch(_DKUMAData);
							}
						}
					}
				}
				if ( EditedModel && Selection.activeGameObject && EditedModel != Selection.activeGameObject.transform ){ 
				if ( Helper ) GUILayout.TextField("You can modify the morphology and the elements composing your avatar. Also you can quickly set the races limitations." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				if ( Helper ) GUILayout.TextField("The morphology is controlled by the Race's DNA, all the sliders are generated depending on it." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				if ( Helper ) GUILayout.TextField("Click twice on the button to edit your avatar." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

					if ( !ShowDKLibraries && GUILayout.Button ( "Modify the Avatar", GUILayout.ExpandWidth (true))) 
				{
					DKUMAData _DKUMAData = EditedModel.GetComponentInChildren<DKUMAData>();
					Selection.activeGameObject = _DKUMAData.transform.gameObject;
					EditedModel = Selection.activeGameObject.transform;
					EditorVariables.DK_DKUMACustomization.EditedModel = EditedModel;
					CreateDNASliders( _DKUMAData );
				}
			/*		if ( !ShowDKLibraries && GUILayout.Button("Edit the RPG Avatar", GUILayout.ExpandWidth (true))){
					OpenRPGCharacterWin ();
				}*/
			}
			if ( EditedModel && Selection.activeGameObject 
			    && EditedModel == Selection.activeGameObject.transform 
			    &&EditorVariables.NewModelName != null) using (new Horizontal()) 
			{
				GUI.color = Color.white ;
				GUILayout.Label("Name :", GUILayout.ExpandWidth (false));
				if (EditorVariables.NewModelName != "" ) GUI.color = Green;
				else GUI.color = Red;
				if ( (EditorVariables.NewModelName == null ||EditorVariables.NewModelName == "")
				&& (Selection.activeGameObject.GetComponent<DK_Model>() != null) ){
						EditorVariables.NewModelName = Selection.activeGameObject.name;
				}
					EditorVariables.NewModelName = GUILayout.TextField(EditorVariables.NewModelName, 50, GUILayout.Width (200));
				if ( EditedModel != null & GUILayout.Button ( "Change", GUILayout.ExpandWidth (false))) {
					EditedModel.parent.name =EditorVariables.NewModelName;
				}
			}
		/*	if ( EditedModel && Selection.activeGameObject && EditedModel == Selection.activeGameObject.transform )using (new Horizontal()) {
				// Save
				GUI.color = Color.white;
				if(GUILayout.Button("Save", GUILayout.ExpandWidth (true))) {
					SaveAvatar();
				}
				if(GUILayout.Button("Save To", GUILayout.ExpandWidth (true))) {
					DKUMAData umaData = Selection.activeGameObject.transform.GetComponentInChildren<DKUMAData>() as DKUMAData;

					if(umaData ){
						umaData.SaveToMemoryStream();
						string path = EditorUtility.SaveFilePanel("Save serialized Avatar","",umaData.transform.parent.name + ".Avatar","Avatar");
						if(path.Length != 0) {
							System.IO.File.WriteAllText(path, umaData.streamedUMA);
						}
					}
				}
			
				if(GUILayout.Button("Load From", GUILayout.ExpandWidth (true))) {
					DKUMAData umaData = Selection.activeGameObject.transform.GetComponentInChildren<DKUMAData>() as DKUMAData;
					umaData.Loading = true;
		
					if(umaData){
						var path = EditorUtility.OpenFilePanel("Load serialized Avatar","","Avatar");
						GameObject DKUMAGeneratorObj = GameObject.Find("DKUMAGenerator");	
						if ( EditorVariables.DKUMAGeneratorObj != null ) {
							_DKUMAGenerator =  EditorVariables.DKUMAGeneratorObj.GetComponent<DKUMAGenerator>();
							_DKUMAGenerator.umaDirtyList.Clear();
						}
						if (path.Length != 0) {
							umaData.streamedUMA = System.IO.File.ReadAllText(path);
							umaData.LoadFromMemoryStream();
					
							//Different race, we need to create it
							DKUMAData.UMARecipe umaRecipe = new DKUMAData.UMARecipe();
							umaRecipe.raceData = umaData.raceLibrary.raceDictionary[umaData.umaRecipe.raceData.raceName];

				    		Transform tempUMA = (Instantiate(umaRecipe.raceData.racePrefab ,umaData.transform.position,umaData.transform.rotation) as GameObject).transform;
							DKUMAData newUMA = tempUMA.gameObject.GetComponentInChildren<DKUMAData>();
				        	newUMA.umaRecipe = umaRecipe;
							newUMA.EditorVariables.OverlayLibraryObj = umaData.EditorVariables.OverlayLibraryObj;
							newUMA.EditorVariables.RaceLibraryObj = umaData.EditorVariables.RaceLibraryObj;
							newUMA.EditorVariables.DKSlotLibraryObj = umaData.EditorVariables.DKSlotLibraryObj;

							newUMA.streamedUMA = System.IO.File.ReadAllText(path);
							// Added by DK
							newUMA.Awaking();
							// DK
							newUMA.LoadFromMemoryStream();
							newUMA.atlasResolutionScale = umaData.atlasResolutionScale;
							newUMA.Dirty(true, true, true);
							newUMA.transform.parent.gameObject.name = umaData.transform.name;
							newUMA.transform.parent = umaData.transform.parent ;
							Selection.activeGameObject = newUMA.transform.parent.gameObject;

							if (  Application.isPlaying ) Destroy(umaData.transform.gameObject);
							else DestroyImmediate(umaData.transform.gameObject);
						}
					}
				}
			}*/

				if ( !ShowDKLibraries && EditedModel && Selection.activeGameObject && EditedModel == Selection.activeGameObject.transform ) using (new Horizontal()) {
			//	GUILayout.Space (5);
				GUI.color = Color.yellow;
			//	GUILayout.Label("Here you can Edit the UMA Avatar." , style, GUILayout.ExpandWidth (true));
				using (new HorizontalCentered()) {
					GUILayout.Label( Selection.activeGameObject.transform.parent.GetComponent<DK_Model>().Race, GUILayout.ExpandWidth (true));
					GUILayout.Label( " - "+Selection.activeGameObject.transform.parent.GetComponent<DK_Model>().Gender, GUILayout.ExpandWidth (true));

				}
			}

			#endregion Menu
		//	GUILayout.Space (5);
				if ( !ShowDKLibraries && EditedModel && Selection.activeGameObject && EditedModel == Selection.activeGameObject.transform) {
				
				if ( Helper ) GUILayout.TextField("If the Avatar has a prefab, you can refresh the prefab : from the Inspector, click on the Apply button of the Prebab menu, at the top of the inspector." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

				GUILayout.Space (5);
				#region Morphology
				using (new Horizontal()) {
					if (showMorph ) GUI.color = Color.yellow;
					else GUI.color = Color.white;
					if ( GUILayout.Button("Morphology", "toolbarbutton", GUILayout.ExpandWidth (true))){
						showMorph = true;
						showComp = false;
					}
					if (showComp ) GUI.color = Color.yellow;
					else GUI.color = Color.white;
					if ( GUILayout.Button("Avatar Elements", "toolbarbutton", GUILayout.ExpandWidth (true))){
						showMorph = false;
						showComp = true;

					}
				}
				if ( showMorph && GUILayout.Button("Apply to Dna Converter", GUILayout.ExpandWidth (true))){
					DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
					_DnaConverterBehaviour = umaData.umaRecipe.raceData.dnaConverterList[0];
					DK_DNA_Elements _DK_DNA_Elements = _DnaConverterBehaviour.gameObject.GetComponent<DK_DNA_Elements>();

					EditorUtility.SetDirty(_DK_DNA_Elements);
					AssetDatabase.SaveAssets();
				}
			//	GUILayout.Space (5);
				if ( showMorph ) using (new ScrollView(ref scroll)) 
				{
						if ( Helper ) GUILayout.TextField("You can set the max value of the Sliders, write your value in the 'Limit' field then click Apply." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

					#region Search
					using (new Horizontal()) {
						GUI.color = Color.white;
						GUILayout.Label("Search for :", GUILayout.Width (75));
						if (SearchString == null ) SearchString = "";
						SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));
					GUILayout.Label("Limit :", GUILayout.ExpandWidth (false));
					if ( Limit == "" ||  Limit == null ) {
						Limit = "1";
								EditorVariables.ModifLimit = 1;

					}
						Limit = GUILayout.TextField(Limit, 50, GUILayout.ExpandWidth (false));
					if ( GUILayout.Button ( "Apply",  GUILayout.ExpandWidth (false))){
								EditorVariables.ModifLimit = float.Parse( Limit);
					}

					}
					#endregion Search
					using (new Horizontal()) {
						GUILayout.Label ( "Name" , "toolbarbutton", GUILayout.Width (170));
						GUILayout.Label ( "Value" , "toolbarbutton", GUILayout.Width (45));
						GUILayout.Label ( "" , "toolbarbutton", GUILayout.ExpandWidth (true));
						
					}

					DKUMAData _umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
						if ( Helper ) GUILayout.TextField("The sliders are generated automatically regarding the 'DNA Converter Data List' of every Race, you can add or remove a DNA Converter Data. Go to the Race Editor to modify any DNA aspect, all is accessible, so do it at your own risk !" , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

						if ( Helper ) GUILayout.TextField("To ease the limitation of therace, you can set it up from here, use the yellow '<' and '>' at the end of the sliders to mark the minimum and maximum values for the part then click on 'Apply to Dna Converter'." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

					if ( _umaData ) using (new ScrollView(ref scroll3)) 
					{
						for (int i = 0; i <EditorVariables.tmpDNAList.Count ; i++)
						{
							if ( EditorVariables.tmpDNAList[i].Name.ToLower().Contains(SearchString.ToLower())) {
									if ( EditorVariables.tmpDNAList[i].Part == "" ){
										using (new Horizontal()) {
											GUI.color = Red;
											GUI.color = Color.white;
											if (_umaData.DNAList2[i] != null && _umaData.DNAList2[i].Name != null && GUILayout.Button ( _umaData.DNAList2[i].Name, Slim, GUILayout.Width (60))){
										}
										if ( _umaData.DNAList2[i].Name == "Height" ){
										//	float tmpValue = (EditorVariables.tmpDNAList[i].Value * 4);
										//	GUILayout.Label( (Math.Round(tmpValue, 2)).ToString ()+" m" , GUILayout.Width (50));
										}
											EditorVariables.tmpDNAList[i].Value = GUILayout.HorizontalSlider(EditorVariables.tmpDNAList[i].Value ,0,EditorVariables.ModifLimit);
										GUILayout.Label(EditorVariables.tmpDNAList[i].Value.ToString () , GUILayout.Width (40));
										EditorStoreDNA.StoreDNAValues( _umaData , i );

										GUI.color = Color.yellow;
										if ( GUILayout.Button("<" , "toolbarbutton", GUILayout.ExpandWidth (false))) 
										{
											DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
											DKRaceData raceData = umaData.umaRecipe.raceData;
											raceData.ConverterDataList[i].ValueMini = EditorVariables.tmpDNAList[i].Value;
												EditorUtility.SetDirty(raceData);
												AssetDatabase.SaveAssets();

										}
										if ( GUILayout.Button(">" ,  "toolbarbutton", GUILayout.ExpandWidth (false))) 
										{
											DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
											DKRaceData raceData = umaData.umaRecipe.raceData;
											raceData.ConverterDataList[i].ValueMaxi = EditorVariables.tmpDNAList[i].Value;
												EditorUtility.SetDirty(raceData);
												AssetDatabase.SaveAssets();

										}
									}
								}
							}
						}
						if ( Head ) GUI.color = Color.yellow;
						else GUI.color = Color.white;
						if ( GUILayout.Button ( "Head", "foldout", GUILayout.Width (120))){
							if ( Head ) Head = false;
							else Head = true;
						}
						if ( Head ) for (int i = 0; i <EditorVariables.tmpDNAList.Count ; i++)
							{
							if ( EditorVariables.tmpDNAList[i].Part == "Head" )using (new Horizontal()) {
									GUILayout.Space(20);	
								GUI.color = Red;
								GUI.color = Color.white;
								if ( GUILayout.Button ( _umaData.DNAList2[i].Name, Slim, GUILayout.Width (100))){
								}

								EditorVariables.tmpDNAList[i].Value = GUILayout.HorizontalSlider(EditorVariables.tmpDNAList[i].Value ,0,EditorVariables.ModifLimit);
								GUILayout.Label(EditorVariables.tmpDNAList[i].Value.ToString () , GUILayout.Width (40));
									EditorStoreDNA.StoreDNAValues( _umaData , i );

								GUI.color = Color.yellow;
								if ( GUILayout.Button("<" , "toolbarbutton", GUILayout.ExpandWidth (false))) 
								{
									DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
									DKRaceData raceData = umaData.umaRecipe.raceData;
									raceData.ConverterDataList[i].ValueMini = EditorVariables.tmpDNAList[i].Value;
										EditorUtility.SetDirty(raceData);
										AssetDatabase.SaveAssets();
								}
								if ( GUILayout.Button(">" ,  "toolbarbutton", GUILayout.ExpandWidth (false))) 
								{
									DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
									DKRaceData raceData = umaData.umaRecipe.raceData;
									raceData.ConverterDataList[i].ValueMaxi = EditorVariables.tmpDNAList[i].Value;
										EditorUtility.SetDirty(raceData);
										AssetDatabase.SaveAssets();
								}
							}
						}
						if ( Face ) GUI.color = Color.yellow;
						else GUI.color = Color.white;
						if ( GUILayout.Button ( "Face", "foldout", GUILayout.Width (90))){
							if ( Face ) Face = false;
							else Face = true;
						}
						if ( Face ) {
							if ( Eyes ) GUI.color = Color.yellow;
							else GUI.color = Color.white;
							using (new Horizontal()){
								GUILayout.Space(20);
								if ( GUILayout.Button ( "Eyes", "foldout", GUILayout.Width (90))){
									if ( Eyes ) Eyes = false;
									else Eyes = true;
								}
							}
							if ( Eyes ) for (int i = 0; i <_umaData.DNAList2.Count ; i++)
							{
								if ( _umaData.DNAList2[i].Part2 == "Eyes" )using (new Horizontal()) {
									GUILayout.Space(40);	
										
									GUI.color = Color.white;
									if ( GUILayout.Button ( _umaData.DNAList2[i].Name, Slim, GUILayout.Width (90))){
											
									}
									EditorVariables.tmpDNAList[i].Value = GUILayout.HorizontalSlider(EditorVariables.tmpDNAList[i].Value ,0,EditorVariables.ModifLimit);
									GUILayout.Label(EditorVariables.tmpDNAList[i].Value.ToString () , GUILayout.Width (40));
										EditorStoreDNA.StoreDNAValues( _umaData , i );
									GUI.color = Color.yellow;
									if ( GUILayout.Button("<" , "toolbarbutton", GUILayout.ExpandWidth (false))) 
									{
										DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
										DKRaceData raceData = umaData.umaRecipe.raceData;
										raceData.ConverterDataList[i].ValueMini = EditorVariables.tmpDNAList[i].Value;
											EditorUtility.SetDirty(raceData);
											AssetDatabase.SaveAssets();
									}
									if ( GUILayout.Button(">" ,  "toolbarbutton", GUILayout.ExpandWidth (false))) 
									{
										DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
										DKRaceData raceData = umaData.umaRecipe.raceData;
										raceData.ConverterDataList[i].ValueMaxi = EditorVariables.tmpDNAList[i].Value;
											EditorUtility.SetDirty(raceData);
											AssetDatabase.SaveAssets();
									}
								}
							}
							if ( Nose ) GUI.color = Color.yellow;
							else GUI.color = Color.white;
							using (new Horizontal()){
								GUILayout.Space(20);
								if ( GUILayout.Button ( "Nose", "foldout", GUILayout.Width (90))){
									if ( Nose ) Nose = false;
									else Nose = true;
								}
							}
								
							if ( Nose ) for (int i = 0; i <_umaData.DNAList2.Count ; i++)
							{
								if ( _umaData.DNAList2[i].Part2 == "Nose" )using (new Horizontal()) {
									GUILayout.Space(40);	
									GUI.color = Color.white;
									if ( GUILayout.Button ( _umaData.DNAList2[i].Name, Slim, GUILayout.Width (90))){
										
									}
									EditorVariables.tmpDNAList[i].Value = GUILayout.HorizontalSlider(EditorVariables.tmpDNAList[i].Value ,0,EditorVariables.ModifLimit);
									GUILayout.Label(EditorVariables.tmpDNAList[i].Value.ToString () , GUILayout.Width (40));
										EditorStoreDNA.StoreDNAValues( _umaData , i );
									GUI.color = Color.yellow;
									if ( GUILayout.Button("<" , "toolbarbutton", GUILayout.ExpandWidth (false))) 
									{
										DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
										DKRaceData raceData = umaData.umaRecipe.raceData;
										raceData.ConverterDataList[i].ValueMini = EditorVariables.tmpDNAList[i].Value;
											EditorUtility.SetDirty(raceData);
											AssetDatabase.SaveAssets();
									}
									if ( GUILayout.Button(">" ,  "toolbarbutton", GUILayout.ExpandWidth (false))) 
									{
										DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
										DKRaceData raceData = umaData.umaRecipe.raceData;
										raceData.ConverterDataList[i].ValueMaxi = EditorVariables.tmpDNAList[i].Value;
											EditorUtility.SetDirty(raceData);
											AssetDatabase.SaveAssets();
									}
								}
							}
							if ( Cheeks ) GUI.color = Color.yellow;
							else GUI.color = Color.white;
							using (new Horizontal()){
								GUILayout.Space(20);
								if ( GUILayout.Button ( "Cheeks", "foldout", GUILayout.Width (120))){
									if ( Cheeks ) Cheeks = false;
									else Cheeks = true;
								}
							}
							if ( Cheeks ) for (int i = 0; i <_umaData.DNAList2.Count ; i++)
							{
								if ( _umaData.DNAList2[i].Part2 == "Cheeks" )using (new Horizontal()) {
									GUILayout.Space(40);	
									GUI.color = Color.white;
									if ( GUILayout.Button ( _umaData.DNAList2[i].Name, Slim, GUILayout.Width (90))){
										
										
									}
									EditorVariables.tmpDNAList[i].Value = GUILayout.HorizontalSlider(EditorVariables.tmpDNAList[i].Value ,0,EditorVariables.ModifLimit);
									GUILayout.Label(EditorVariables.tmpDNAList[i].Value.ToString () , GUILayout.Width (40));
										EditorStoreDNA.StoreDNAValues( _umaData , i );
									GUI.color = Color.yellow;
									if ( GUILayout.Button("<" , "toolbarbutton", GUILayout.ExpandWidth (false))) 
									{
										DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
										DKRaceData raceData = umaData.umaRecipe.raceData;
										raceData.ConverterDataList[i].ValueMini = EditorVariables.tmpDNAList[i].Value;
											EditorUtility.SetDirty(raceData);
											AssetDatabase.SaveAssets();
									}
									if ( GUILayout.Button(">" ,  "toolbarbutton", GUILayout.ExpandWidth (false))) 
									{
										DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
										DKRaceData raceData = umaData.umaRecipe.raceData;
										raceData.ConverterDataList[i].ValueMaxi = EditorVariables.tmpDNAList[i].Value;
											EditorUtility.SetDirty(raceData);
											AssetDatabase.SaveAssets();
									}
								}
							}
							if ( Mouth ) GUI.color = Color.yellow;
							else GUI.color = Color.white;
							using (new Horizontal()){
								GUILayout.Space(20);
								if ( GUILayout.Button ( "Mouth", "foldout", GUILayout.Width (120))){
									if ( Mouth ) Mouth = false;
									else Mouth = true;
								}
							}
							
							if ( Mouth ) for (int i = 0; i <_umaData.DNAList2.Count ; i++)
							{
								if ( _umaData.DNAList2[i].Part2 == "Mouth" )using (new Horizontal()) {
									GUILayout.Space(40);	
									GUI.color = Color.white;
									if ( GUILayout.Button ( _umaData.DNAList2[i].Name, Slim, GUILayout.Width (90))){
									
									}
							
									EditorVariables.tmpDNAList[i].Value = GUILayout.HorizontalSlider(EditorVariables.tmpDNAList[i].Value ,0,EditorVariables.ModifLimit);
									GUILayout.Label(EditorVariables.tmpDNAList[i].Value.ToString () , GUILayout.Width (40));
										EditorStoreDNA.StoreDNAValues( _umaData , i );
									GUI.color = Color.yellow;
									if ( GUILayout.Button("<" , "toolbarbutton", GUILayout.ExpandWidth (false))) 
									{
										DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
										DKRaceData raceData = umaData.umaRecipe.raceData;
										raceData.ConverterDataList[i].ValueMini = EditorVariables.tmpDNAList[i].Value;
											EditorUtility.SetDirty(raceData);
											AssetDatabase.SaveAssets();
									}
									if ( GUILayout.Button(">" ,  "toolbarbutton", GUILayout.ExpandWidth (false))) 
									{
										DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
										DKRaceData raceData = umaData.umaRecipe.raceData;
										raceData.ConverterDataList[i].ValueMaxi = EditorVariables.tmpDNAList[i].Value;
											EditorUtility.SetDirty(raceData);
											AssetDatabase.SaveAssets();
									}
								}
							}
							if ( Chin ) GUI.color = Color.yellow;
							else GUI.color = Color.white;
							using (new Horizontal()){
								GUILayout.Space(20);
								if ( GUILayout.Button ( "Chin", "foldout", GUILayout.Width (120))){
									if ( Chin ) Chin = false;
									else Chin = true;
								}
							}
							if ( Chin ) for (int i = 0; i <_umaData.DNAList2.Count ; i++)
							{
								if ( _umaData.DNAList2[i].Part2 == "Chin" )using (new Horizontal()) {
									GUILayout.Space(40);	
									GUI.color = Color.white;
									if ( GUILayout.Button ( _umaData.DNAList2[i].Name, Slim, GUILayout.Width (90))){

									}
									EditorVariables.tmpDNAList[i].Value = GUILayout.HorizontalSlider(EditorVariables.tmpDNAList[i].Value ,0,EditorVariables.ModifLimit);
									GUILayout.Label(EditorVariables.tmpDNAList[i].Value.ToString () , GUILayout.Width (40));
										EditorStoreDNA.StoreDNAValues( _umaData , i );
									GUI.color = Color.yellow;
									if ( GUILayout.Button("<" , "toolbarbutton", GUILayout.ExpandWidth (false))) 
									{
										DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
										DKRaceData raceData = umaData.umaRecipe.raceData;
										raceData.ConverterDataList[i].ValueMini = EditorVariables.tmpDNAList[i].Value;
											EditorUtility.SetDirty(raceData);
											AssetDatabase.SaveAssets();
									}
									if ( GUILayout.Button(">" ,  "toolbarbutton", GUILayout.ExpandWidth (false))) 
									{
										DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
										DKRaceData raceData = umaData.umaRecipe.raceData;
										raceData.ConverterDataList[i].ValueMaxi = EditorVariables.tmpDNAList[i].Value;
											EditorUtility.SetDirty(raceData);
											AssetDatabase.SaveAssets();
									}
								}
							}
						}
						if ( Arms ) GUI.color = Color.yellow;
						else GUI.color = Color.white;
						if ( GUILayout.Button ( "Arms", "foldout", GUILayout.Width (120))){
							if ( Arms ) Arms = false;
							else Arms = true;
						}
						if ( Arms ) for (int i = 0; i <_umaData.DNAList2.Count ; i++)
						{
							if ( _umaData.DNAList2[i].Part == "Arms" )using (new Horizontal()) {
								GUILayout.Space(20);	
								
								
								GUI.color = Color.white;
								if ( GUILayout.Button ( _umaData.DNAList2[i].Name, Slim, GUILayout.Width (110))){
									
									
								}
								EditorVariables.tmpDNAList[i].Value = GUILayout.HorizontalSlider(EditorVariables.tmpDNAList[i].Value ,0,EditorVariables.ModifLimit);
								GUILayout.Label(EditorVariables.tmpDNAList[i].Value.ToString () , GUILayout.Width (40));
									EditorStoreDNA.StoreDNAValues( _umaData , i );
								GUI.color = Color.yellow;
								if ( GUILayout.Button("<" , "toolbarbutton", GUILayout.ExpandWidth (false))) 
								{
									DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
									DKRaceData raceData = umaData.umaRecipe.raceData;
									raceData.ConverterDataList[i].ValueMini = EditorVariables.tmpDNAList[i].Value;
										EditorUtility.SetDirty(raceData);
										AssetDatabase.SaveAssets();
								}
								if ( GUILayout.Button(">" ,  "toolbarbutton", GUILayout.ExpandWidth (false))) 
								{
									DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
									DKRaceData raceData = umaData.umaRecipe.raceData;
									raceData.ConverterDataList[i].ValueMaxi = EditorVariables.tmpDNAList[i].Value;
										EditorUtility.SetDirty(raceData);
										AssetDatabase.SaveAssets();
								}
							}
						}
						if ( Torso ) GUI.color = Color.yellow;
						else GUI.color = Color.white;
						if ( GUILayout.Button ( "Torso", "foldout", GUILayout.Width (120))){
							if ( Torso ) Torso = false;
							else Torso = true;
						}
						if ( Torso ) for (int i = 0; i <_umaData.DNAList2.Count ; i++)
						{
							if ( _umaData.DNAList2[i].Part == "Torso" )using (new Horizontal()) {
								GUILayout.Space(20);	
								
								
								GUI.color = Color.white;
								if ( GUILayout.Button ( _umaData.DNAList2[i].Name, Slim, GUILayout.Width (110))){
									
									
								}
								EditorVariables.tmpDNAList[i].Value = GUILayout.HorizontalSlider(EditorVariables.tmpDNAList[i].Value ,0,EditorVariables.ModifLimit);
								GUILayout.Label(EditorVariables.tmpDNAList[i].Value.ToString () , GUILayout.Width (40));
									EditorStoreDNA.StoreDNAValues( _umaData , i );
								GUI.color = Color.yellow;
								if ( GUILayout.Button("<" , "toolbarbutton", GUILayout.ExpandWidth (false))) 
								{
									DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
									DKRaceData raceData = umaData.umaRecipe.raceData;
									raceData.ConverterDataList[i].ValueMini = EditorVariables.tmpDNAList[i].Value;
										EditorUtility.SetDirty(raceData);
										AssetDatabase.SaveAssets();
								}
								if ( GUILayout.Button(">" ,  "toolbarbutton", GUILayout.ExpandWidth (false))) 
								{
									DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
									DKRaceData raceData = umaData.umaRecipe.raceData;
									raceData.ConverterDataList[i].ValueMaxi = EditorVariables.tmpDNAList[i].Value;
										EditorUtility.SetDirty(raceData);
										AssetDatabase.SaveAssets();
								}
							}
						}
						if ( Legs ) GUI.color = Color.yellow;
						else GUI.color = Color.white;
						if ( GUILayout.Button ( "Leg", "foldout", GUILayout.Width (120))){
							if ( Legs ) Legs = false;
							else Legs = true;
						}
						if ( Legs ) for (int i = 0; i <_umaData.DNAList2.Count ; i++)
						{
							if ( _umaData.DNAList2[i].Part == "Legs" )using (new Horizontal()) {
								GUILayout.Space(20);	
								
								
								GUI.color = Color.white;
								if ( GUILayout.Button ( _umaData.DNAList2[i].Name, Slim, GUILayout.Width (110))){
									
									
								}
								EditorVariables.tmpDNAList[i].Value = GUILayout.HorizontalSlider(EditorVariables.tmpDNAList[i].Value ,0,EditorVariables.ModifLimit);
								GUILayout.Label(EditorVariables.tmpDNAList[i].Value.ToString () , GUILayout.Width (40));
									EditorStoreDNA.StoreDNAValues( _umaData , i );
								GUI.color = Color.yellow;
								if ( GUILayout.Button("<" , "toolbarbutton", GUILayout.ExpandWidth (false))) 
								{
									DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
									DKRaceData raceData = umaData.umaRecipe.raceData;
									raceData.ConverterDataList[i].ValueMini = EditorVariables.tmpDNAList[i].Value;
										EditorUtility.SetDirty(raceData);
										AssetDatabase.SaveAssets();
								}
								if ( GUILayout.Button(">" ,  "toolbarbutton", GUILayout.ExpandWidth (false))) 
								{
									DKUMAData umaData = EditedModel.GetComponentInChildren<DKUMAData>() as DKUMAData;
									DKRaceData raceData = umaData.umaRecipe.raceData;
									raceData.ConverterDataList[i].ValueMaxi = EditorVariables.tmpDNAList[i].Value;
										EditorUtility.SetDirty(raceData);
										AssetDatabase.SaveAssets();
								}
							}
						}
					}
				}
				#endregion Morphology
				#region Components
				if ( showComp && Selection.activeGameObject ) {
					DKUMAData umaData = Selection.activeGameObject.GetComponentInChildren<DKUMAData>();

				//	GUILayout.Space (10);
					
					#region ChangeOverlay List
					if (  ChangeOverlay )using (new Horizontal()) {
						
						GUI.color = Color.white ;
						GUILayout.Label("Choose an Overlay for "+umaData.umaRecipe.slotDataList[TmpSlotIndex].slotName, "toolbarbutton", GUILayout.ExpandWidth (true));
						GUI.color = Red;
						if ( GUILayout.Button("X", "toolbarbutton", GUILayout.ExpandWidth (false))){
							ChangeOverlay = false;
						}
					}
					#region Search
					if ( ChangeOverlay && !AddSlot ) using (new Horizontal()) {
						GUI.color = Color.white;
						GUILayout.Label("Search for :", GUILayout.ExpandWidth (false));
						SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));
						
					}
					#endregion Search
					GUI.color = Color.white;
					if ( !choosePlace && !chooseOverlay && !chooseSlot && ChangeOverlay && GUILayout.Button("Assign Overlay to the Slot", GUILayout.ExpandWidth (true))){
							umaData.umaRecipe.slotDataList[TmpSlotIndex].overlayList.Add(new DKOverlayData(EditorVariables.DK_UMACrowd.overlayLibrary,  EditorVariables.SelectedElemOvlay.overlayName) { color = Color.white  });
						EditorSaveAvatar.SaveAvatar();
						 EditorVariables.SelectedElemOvlay = null;
						ChangeOverlay = false;
					}
					GUI.color = Color.white ;
					if ( !choosePlace && !chooseOverlay && !chooseSlot && ChangeOverlay ) using (new Horizontal()) {
						GUILayout.Label("Overlay", "toolbarbutton", GUILayout.Width (160));
						GUILayout.Label("Race", "toolbarbutton", GUILayout.Width (80));
						GUILayout.Label("Gender", "toolbarbutton", GUILayout.Width (90));
						GUILayout.Label("Place", "toolbarbutton", GUILayout.Width (100));
						GUILayout.Label("Overlay Type", "toolbarbutton", GUILayout.Width (140));
						GUILayout.Label("WearWeight", "toolbarbutton", GUILayout.Width (70));
						GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
					}
					
					if ( ChangeOverlay ) 
					{
						#region Overlays
						using (new ScrollView(ref scroll)) 
						{
							using (new Horizontal()) {	
								GUI.color = Color.yellow ;
								GUILayout.Label ( "Overlays Library", GUILayout.ExpandWidth (false));
							}
								for(int i = 0; i < EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList.Length; i ++){
									if (EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i] != null 
									    && ( EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i].overlayName.ToLower().Contains(SearchString.ToLower())
									    || SearchString == "" ) ) using (new Horizontal()) {
										DKOverlayData _DKOverlayData = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i];
									if ( _DKOverlayData.Active == true ) GUI.color = Green;
									else GUI.color = Color.gray ;
									if (GUILayout.Button ( "U",  GUILayout.Width (20))){
										if ( _DKOverlayData.Active == true ) _DKOverlayData.Active = false;
										else _DKOverlayData.Active = true;
										EditorUtility.SetDirty(_DKOverlayData);
										AssetDatabase.SaveAssets();
									} 
									
										if ( umaData.umaRecipe.slotDataList[TmpSlotIndex].overlayList.Contains(EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i]) )  GUI.color =  Color.gray;
									else {
											if ( EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i] ==  EditorVariables.SelectedElemOvlay ) GUI.color = Color.yellow;
										else GUI.color = Color.white;
										
									}
										if (GUILayout.Button ( EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i].overlayName , Slim, GUILayout.Width (140))) {
											if ( umaData.umaRecipe.slotDataList[TmpSlotIndex].overlayList.Contains(EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i]) == false )
												EditorVariables.SelectedElemOvlay = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i];
										
									}
									// Race
									DKOverlayData DK_Race;
										DK_Race = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i];
									if ( umaData.umaRecipe.slotDataList[TmpSlotIndex].overlayList.Contains(DK_Race) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
									if ( DK_Race.Race.Count == 0 && GUILayout.Button ( "No Race" , Slim, GUILayout.Width (70))) {
									}
		
										if ( umaData.umaRecipe.slotDataList[TmpSlotIndex].overlayList.Contains(EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i]) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
									if ( DK_Race.Race.Count == 0 && GUILayout.Button ( "Race" , Slim, GUILayout.Width (70))) {
									}
									// Gender
										if ( umaData.umaRecipe.slotDataList[TmpSlotIndex].overlayList.Contains(EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i]) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
									if ( DK_Race.Gender == "" && GUILayout.Button ( "No Gender" , Slim, GUILayout.Width (70))) {
									}
		
										if ( umaData.umaRecipe.slotDataList[TmpSlotIndex].overlayList.Contains(EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i]) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
									if ( DK_Race.Gender != "" && GUILayout.Button ( DK_Race.Gender , Slim, GUILayout.Width (70))) {
									}
									// Place
										if ( umaData.umaRecipe.slotDataList[TmpSlotIndex].overlayList.Contains(EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i]) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
									if ( DK_Race.Place == null && GUILayout.Button ( "No Place" , Slim, GUILayout.Width (70))) {
										
									}
		
										if ( umaData.umaRecipe.slotDataList[TmpSlotIndex].overlayList.Contains(EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i]) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
									if ( DK_Race.Place != null && GUILayout.Button ( DK_Race.Place.name , Slim, GUILayout.Width (70))) {
									}
									// Overlay Type
										if ( umaData.umaRecipe.slotDataList[TmpSlotIndex].overlayList.Contains(EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i]) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
									if ( DK_Race.OverlayType == "" && GUILayout.Button ( "No Overlay Type" , Slim, GUILayout.Width (70))) {
									}
		
										if ( umaData.umaRecipe.slotDataList[TmpSlotIndex].overlayList.Contains(EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i]) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
									if ( DK_Race.OverlayType != "" && GUILayout.Button ( DK_Race.OverlayType , Slim, GUILayout.Width (90))) {
									}
									// WearWeight
									GUI.color =  Color.gray;
									if (  DK_Race.WearWeight == "" ) 
									{
								
									}
									else
									{
										if ( DK_Race.WearWeight != "" ) GUI.color = Red;
										if ( DK_Race.WearWeight != "" && GUILayout.Button ( "X " , Slim, GUILayout.Width (10))) {
											DK_Race.WearWeight = "";
											EditorUtility.SetDirty(DK_Race);
											AssetDatabase.SaveAssets();
										}
										GUI.color = Color.white ;
										GUILayout.Label( DK_Race.WearWeight , Slim, GUILayout.Width (70));
									}
								}
							}
						}
						#endregion
					}
					#endregion ChangeOverlay List
					
					#region Add Slot List
					if (  AddSlot )using (new Horizontal()) {
						GUI.color = Color.white ;
						GUILayout.Label("Choose a Slot for the Model", "toolbarbutton", GUILayout.ExpandWidth (true));
						GUI.color = Red;
						if ( GUILayout.Button("X", "toolbarbutton", GUILayout.ExpandWidth (false))){
							AddSlot = false;
						}
					}
					GUI.color = Color.white;
					if ( !choosePlace && !chooseOverlay && !chooseSlot && AddSlot && GUILayout.Button("Assign Slot to the Model", GUILayout.ExpandWidth (true))){
						List<DKSlotData> TmpSlotDataList = new List<DKSlotData>();
						TmpSlotDataList = umaData.umaRecipe.slotDataList.ToList();
							TmpSlotDataList.Add(new DKSlotData(EditorVariables.DK_UMACrowd.slotLibrary,  EditorVariables.SelectedElemSlot.slotName) );
						umaData.umaRecipe.slotDataList = TmpSlotDataList.ToArray();
							EditorSaveAvatar.SaveAvatar();
						 EditorVariables.SelectedElemSlot = null;
						AddSlot = false;
					}
					#region Search
					if ( !ChangeOverlay && AddSlot ) using (new Horizontal()) {
						GUI.color = Color.white;
						GUILayout.Label("Search for :", GUILayout.ExpandWidth (false));
						SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));
						
					}
					#endregion Search
					GUI.color = Color.white ;
					if ( !choosePlace && !chooseOverlay && !chooseSlot && AddSlot ) using (new Horizontal()) {
						GUILayout.Label("Slot", "toolbarbutton", GUILayout.Width (160));
						GUILayout.Label("Race", "toolbarbutton", GUILayout.Width (80));
						GUILayout.Label("Gender", "toolbarbutton", GUILayout.Width (90));
						GUILayout.Label("Place", "toolbarbutton", GUILayout.Width (100));
						GUILayout.Label("Overlay Type", "toolbarbutton", GUILayout.Width (140));
						GUILayout.Label("WearWeight", "toolbarbutton", GUILayout.Width (70));
						GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
						
					}
					
					if ( AddSlot ) 
					{
						#region Slot
						using (new ScrollView(ref scroll)) 
						{
							using (new Horizontal()) {	
								GUI.color = Color.yellow ;
								GUILayout.Label ( "Slots Library", GUILayout.ExpandWidth (false));
							}
								for(int i = 0; i < EditorVariables.DK_UMACrowd.slotLibrary.slotElementList.Length; i ++){
									if (EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i] != null
									    && ( EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i].slotName.ToLower().Contains(SearchString.ToLower())
									    || SearchString == "" ) ) using (new Horizontal()) {
										DKSlotData _SlotData = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
									if ( _SlotData.Active == true ) GUI.color = Green;
									else GUI.color = Color.gray ;
									if (GUILayout.Button ( "U",  GUILayout.Width (20))){
										if ( _SlotData.Active == true ) _SlotData.Active = false;
										else _SlotData.Active = true;
										EditorUtility.SetDirty(_SlotData);
										AssetDatabase.SaveAssets();
									} 
									
										if ( umaData.umaRecipe.slotDataList.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
									else {
											if (  EditorVariables.SelectedElemSlot && EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i].slotName ==  EditorVariables.SelectedElemSlot.slotName ) GUI.color = Color.yellow;
										else GUI.color = Color.white;
										
									}
										if (GUILayout.Button ( EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i].slotName , Slim, GUILayout.Width (140))) {
											if ( umaData.umaRecipe.slotDataList.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) == false )
												EditorVariables.SelectedElemSlot = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
										
									}
									// Race
									DKSlotData DK_Race;
										DK_Race = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
									if ( umaData.umaRecipe.slotDataList.Contains(DK_Race) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
									if ( DK_Race.Race.Count == 0 && GUILayout.Button ( "No Race" , Slim, GUILayout.Width (70))) {
									}
		
										if ( umaData.umaRecipe.slotDataList.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
									if ( DK_Race.Race.Count != 0 && GUILayout.Button ( "Race" , Slim, GUILayout.Width (70))) {
									}
									// Gender
										if ( umaData.umaRecipe.slotDataList.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
									if ( DK_Race.Gender == "" && GUILayout.Button ( "No Gender" , Slim, GUILayout.Width (70))) {
									}
		
										if ( umaData.umaRecipe.slotDataList.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
									if ( DK_Race.Gender != "" && GUILayout.Button ( DK_Race.Gender , Slim, GUILayout.Width (70))) {
									}
									// Place
										if ( umaData.umaRecipe.slotDataList.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
									if ( DK_Race.Place == null && GUILayout.Button ( "No Place" , Slim, GUILayout.Width (70))) {
										
									}
		
										if ( umaData.umaRecipe.slotDataList.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
									if ( DK_Race.Place != null && GUILayout.Button ( DK_Race.Place.name , Slim, GUILayout.Width (70))) {
									}
									// Overlay Type
										if ( umaData.umaRecipe.slotDataList.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
										if ( DK_Race.OverlayType == "" && GUILayout.Button ( "No Overlay Type" , Slim, GUILayout.Width (70))) {
									}
		
										if ( umaData.umaRecipe.slotDataList.Contains(EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
									else GUI.color = Color.white;
										if ( DK_Race.OverlayType != "" && GUILayout.Button ( DK_Race.OverlayType , Slim, GUILayout.Width (90))) {
									}
									// WearWeight
									GUI.color =  Color.gray;
										if (  DK_Race.WearWeight == "" ) 
									{
								
									}
									else
									{
											if ( DK_Race.WearWeight != "" ) GUI.color = Red;
											if ( DK_Race.WearWeight != "" && GUILayout.Button ( "X " , Slim, GUILayout.Width (10))) {
												DK_Race.WearWeight = "";
											EditorUtility.SetDirty(DK_Race);
											AssetDatabase.SaveAssets();
										}
										GUI.color = Color.white ;
											GUILayout.Label( DK_Race.WearWeight , Slim, GUILayout.Width (70));
									}
								}
							}
						}
						#endregion Slot
					}
					#endregion Add Slot List
						GUILayout.Space (5);
						GUI.color = Color.yellow;
						GUILayout.TextField("Beware : Modifying the elements from this window is not effective on a RPG Avatar. To Modify the elements of a RPG Avatar, use the RPG Avatar window.", 300, style, GUILayout.ExpandHeight (true),  GUILayout.ExpandWidth (true));
						GUI.color = Green;
					/*	if ( GUILayout.Button("Open RPG Avatar Window", GUILayout.ExpandWidth (true))){
							OpenRPGCharacterWin ();
						}*/
						GUILayout.Space (5);
						if ( !ChangeOverlay && !AddSlot ) using (new Horizontal()) {
							GUI.color = Color.white;
							GUILayout.Label("Model's Elements List", "toolbarbutton", GUILayout.ExpandWidth (true));
							GUI.color = Green;
							if ( GUILayout.Button("Add a Slot", "toolbarbutton", GUILayout.ExpandWidth (true))){
								AddSlot = true;
							}
						}
						#region Search
						using (new Horizontal()) {
							GUI.color = Color.white;
							GUILayout.Label("Search for :", GUILayout.Width (75));
							if (SearchString == null ) SearchString = "";
							SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));
						}
						#endregion Search
						if ( !AddSlot && !ChangeOverlay && Selection.activeGameObject != null ) using (new ScrollView(ref scroll)) 
					{
						GUILayout.Space (10);
						//	# if Editor
							try{
								if (umaData) for(int i = 0; i <  umaData.umaRecipe.slotDataList.Length; i++){
									if ( umaData.umaRecipe.slotDataList[i] != null && ( SearchString == "" || umaData.umaRecipe.slotDataList[i].slotName.ToLower().Contains(SearchString.ToLower()) == true )) {

										using (new Horizontal()) {
											GUI.color = Color.white;
											if (GUILayout.Button("["+i+"] "+umaData.umaRecipe.slotDataList[i].slotName, "toolbarbutton", GUILayout.Width (195))){

											}
											//	GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
											GUI.color = Green;
											if ( GUILayout.Button( "Add Overlay", "toolbarbutton", GUILayout.ExpandWidth (false) )){
												ChangeOverlay = true;
												TmpSlotIndex = i;
											}												GUI.color = Red;
											if ( GUILayout.Button("X", "toolbarbutton", GUILayout.ExpandWidth (false))){
												List<DKSlotData> TmpSlotDataList = new List<DKSlotData>();
												TmpSlotDataList = umaData.umaRecipe.slotDataList.ToList();
												TmpSlotDataList.Remove(umaData.umaRecipe.slotDataList[i]);
												umaData.umaRecipe.slotDataList = TmpSlotDataList.ToArray();
												EditorSaveAvatar.SaveAvatar();
											}
										}
									
										using (new Horizontal()) {
											Texture2D Preview = new Texture2D(75, 75);
										/*	using (new Vertical()) {
												if (Preview == null ){
													string path = AssetDatabase.GetAssetPath ((Selection.activeObject as DKSlotData));
													path = path.Replace ((Selection.activeObject as DKSlotData).name+".asset", "");
													Preview = AssetDatabase.LoadAssetAtPath(path+"Preview-"+(Selection.activeObject as DKSlotData).name+".asset", typeof(Texture2D) ) as Texture2D;
												}
												
												
												if ( Preview != null ) GUI.color = Color.white ;
												else GUI.color = Red;
												if ( GUILayout.Button( Preview ,GUILayout.Width (75), GUILayout.Height (75))){
													
													
												}
											}

											GUILayout.Space (10);*/
									//	}
										/*	using (new Vertical()) {
												if (Preview.name != "Preview-"+umaData.umaRecipe.slotDataList[i].name ){
													string path = AssetDatabase.GetAssetPath (umaData.umaRecipe.slotDataList[i]);
													path = path.Replace (umaData.umaRecipe.slotDataList[i]+".asset", "");
													Preview = AssetDatabase.LoadAssetAtPath(path+"Preview-"+umaData.umaRecipe.slotDataList[i].name+".asset", typeof(Texture2D) ) as Texture2D;
												}
												if ( Preview != null ) GUI.color = Color.white ;
												else GUI.color = Red;
												if ( GUILayout.Button( Preview ,GUILayout.Width (75), GUILayout.Height (75))){
													
													
												}
												GUILayout.Space (10);
											}*/

											using (new Vertical()) {
											
												if ( i < umaData.umaRecipe.slotDataList.Length ) 
												for(int i1 = 0; i1 <  umaData.umaRecipe.slotDataList[i].overlayList.Count; i1++){
													using (new Horizontal()) {
														GUI.color = Color.white;
														if ( GUILayout.Button(umaData.umaRecipe.slotDataList[i].overlayList[i1].overlayName, Slim, GUILayout.Width (110))){
														
														}
														GUI.color = Red;
														if ( GUILayout.Button("X", "toolbarbutton", GUILayout.ExpandWidth (false))){
															umaData.umaRecipe.slotDataList[i].overlayList.Remove(umaData.umaRecipe.slotDataList[i].overlayList[i1]);
														}
														if ( i1 < umaData.umaRecipe.slotDataList[i].overlayList.Count ) 
															umaData.umaRecipe.slotDataList[i].overlayList[i1].color = EditorGUILayout.ColorField("", umaData.umaRecipe.slotDataList[i].overlayList[i1].color, GUILayout.Width (60));
													}
												}
											}
										}
									}
								}
							}catch(NullReferenceException){
								Debug.Log ( "umaRecipe.slotDataList is umpty" );
							}
						}
				}
				#endregion Components
			}
		}
		#endregion UMA
		
		#region Setup menu
			if ( showSetup ) {
			GUI.color = Color.white;
			GUILayout.Label("Setup Menu", "toolbarbutton", GUILayout.ExpandWidth (true));
		
			#region Generation Type
			GUI.color = Color.white;
			GUILayout.Label("Generation Type", "toolbarbutton", GUILayout.ExpandWidth (true));
			if ( Helper ) 
			{
				GUI.color = Color.yellow;
				GUILayout.TextField("Choose the way for the Generation of your Model. Using 'Presets' enables all the advanced DK UMA procedural features." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			}
			GUILayout.Space(5);
			using (new ScrollView(ref scroll)) 
			{
				using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label ( "Generation :", GUILayout.ExpandWidth (false));
				
				//	if ( EditorVariables.DK_UMACrowd && DK_UMACrowd.GeneratorMode != "Preset" )DK_UMACrowd.GeneratorMode = "Preset"; 
					if ( EditorVariables.DK_UMACrowd && DK_UMACrowd.GeneratorMode != "Preset" )  GUI.color = Color.gray;
					else GUI.color = Green;
				
					if (EditorVariables.DK_DKUMAGenerator &&  EditorVariables.DK_DKUMAGenerator.usePRO == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("use PRO")){
						if ( EditorVariables.DK_DKUMAGenerator.usePRO == true ) EditorVariables.DK_DKUMAGenerator.usePRO = false;
						else EditorVariables.DK_DKUMAGenerator.usePRO = true;
						EditorUtility.SetDirty(EditorVariables.DK_DKUMAGenerator.gameObject);
						AssetDatabase.SaveAssets();
						
					}
						if (EditorVariables.DK_UMACrowd &&  DK_UMACrowd.GeneratorMode == "RPG" ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("use RPG")){
							if ( DK_UMACrowd.GeneratorMode == "RPG" ) DK_UMACrowd.GeneratorMode = "Preset";
							else DK_UMACrowd.GeneratorMode = "RPG";
							EditorUtility.SetDirty(EditorVariables.DK_UMACrowd.gameObject);
						AssetDatabase.SaveAssets();
						
					}
					if (EditorVariables.DK_DKUMAGenerator &&  DKUMAGenerator.AutoDel == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("Clear Missing")){
						if ( DKUMAGenerator.AutoDel == true ) DKUMAGenerator.AutoDel = false;
						else DKUMAGenerator.AutoDel = true;
						EditorUtility.SetDirty(EditorVariables.DK_DKUMAGenerator.gameObject);
						AssetDatabase.SaveAssets();
						
					}
				}
				#region Reset
				GUILayout.Space(5);
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					if(GUILayout.Button("Reset Generator")){
							EditorVariables.DK_UMACrowd.generateUMA = false;
						EditorVariables.DK_UMACrowd.generateLotsUMA = false;
							EditorVariables.DK_UMACrowd.UMAGenerated = true;
						EditorVariables.DK_DKUMAGenerator.umaDirtyList.Clear();
						GL.Clear( true,  true, Color.black, 1.0f);
						Resources.UnloadUnusedAssets();

					}
				}
				GUILayout.Space(5);
				#endregion Reset
				#endregion Generation Type
				
				#region Save
			/*	GUI.color = Color.white;
				GUILayout.Label("Save Avatar Setup", "toolbarbutton", GUILayout.ExpandWidth (true));
				if ( Helper ) 
				{
					GUI.color = Color.yellow;
					GUILayout.TextField("Select a path for the Model to be saved during the creation." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					
				}
				using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label ( "Avatar Save Path :", GUILayout.ExpandWidth (false));
					AvatarSavePath = GUILayout.TextField(AvatarSavePath, 100, GUILayout.Width (110));
					if(GUILayout.Button("Change", GUILayout.ExpandWidth (false))){
						AvatarSavePath = EditorUtility.SaveFolderPanel("Choose Save Folder","","");
						EditorVariables.DKUMAGeneratorObj = GameObject.Find("DKUMAGenerator");	
						if ( EditorVariables.DKUMAGeneratorObj != null ) EditorVariables.DK_DKUMAGenerator =  EditorVariables.DKUMAGeneratorObj.GetComponent<DKUMAGenerator>();
						EditorVariables.DK_DKUMAGenerator.AvatarSavePath = AvatarSavePath;
					}		
				}
				using (new Horizontal()) {
					GUILayout.Label ( "Save Avatar :", GUILayout.ExpandWidth (false));
					if ( EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_DKUMAGenerator.ManualSaveModel == false && EditorVariables.DK_DKUMAGenerator.AutoSaveModel == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("Auto")){
						EditorVariables.DKUMAGeneratorObj = GameObject.Find("DKUMAGenerator");	
						if ( EditorVariables.DKUMAGeneratorObj != null ) EditorVariables.DK_DKUMAGenerator =  EditorVariables.DKUMAGeneratorObj.GetComponent<DKUMAGenerator>();
						EditorVariables.DK_DKUMAGenerator.ManualSaveModel = false;
						EditorVariables.DK_DKUMAGenerator.AutoSaveModel = true;
					}
					if ( EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_DKUMAGenerator.ManualSaveModel == true && EditorVariables.DK_DKUMAGenerator.AutoSaveModel == false ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("Manual")){
						EditorVariables.DKUMAGeneratorObj = GameObject.Find("DKDKUMAGenerator");	
						if ( EditorVariables.DKUMAGeneratorObj != null ) EditorVariables.DK_DKUMAGenerator =  EditorVariables.DKUMAGeneratorObj.GetComponent<DKUMAGenerator>();
						EditorVariables.DK_DKUMAGenerator.ManualSaveModel = true;
						EditorVariables.DK_DKUMAGenerator.AutoSaveModel = false;
					}
					if ( EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_DKUMAGenerator.ManualSaveModel == false && EditorVariables.DK_DKUMAGenerator.AutoSaveModel == false ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("None")){
						EditorVariables.DKUMAGeneratorObj = GameObject.Find("DKUMAGenerator");	
						if ( EditorVariables.DKUMAGeneratorObj != null ) EditorVariables.DK_DKUMAGenerator =  EditorVariables.DKUMAGeneratorObj.GetComponent<DKUMAGenerator>();
						EditorVariables.DK_DKUMAGenerator.ManualSaveModel = false;
						EditorVariables.DK_DKUMAGenerator.AutoSaveModel = false;
					}
				}*/
				#endregion Save
				
				#region Engine
				GUILayout.Space(5);
				#region Helper
				GUI.color = Color.white;
				GUILayout.Label("DK UMA Engine Setup", "toolbarbutton", GUILayout.ExpandWidth (true));
				if ( Helper ) 
				{
					GUI.color = Color.yellow;
					GUILayout.TextField("Take care modifying the Engine parameters..." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
					
				}
				#endregion Helper
				#region Atlas and resolution
				if ( EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd ) using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label("Atlas :", GUILayout.ExpandWidth (false));
					// Resolution
					GUILayout.Label("Resolution :", GUILayout.ExpandWidth (false));
					string AtlasRes = EditorVariables.DK_DKUMAGenerator.atlasResolution.ToString();
					AtlasRes = GUILayout.TextField(AtlasRes, 5, GUILayout.Width (40));
					AtlasRes = Regex.Replace(AtlasRes, "[^0-9]", "");
					if ( AtlasRes == "" ) AtlasRes = "1";
					EditorVariables.DK_DKUMAGenerator.atlasResolution = Convert.ToInt32(AtlasRes);
					// scale
					GUILayout.Label("Scale :", GUILayout.ExpandWidth (false));
						string AtlasResSc = EditorVariables.DK_UMACrowd.atlasResolutionScale.ToString();
					AtlasResSc = GUILayout.TextField(AtlasResSc, 3, GUILayout.Width (30));
					AtlasResSc = Regex.Replace(AtlasResSc, "[^0-9]", "");
					if ( AtlasResSc == "" ) AtlasResSc = "1";
						EditorVariables.DK_UMACrowd.atlasResolutionScale = Convert.ToInt32(AtlasResSc);
				}
				if ( EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd ) using (new Horizontal()) {
					// Max Pixels
					GUILayout.Label("Max Pixels :", GUILayout.ExpandWidth (false));
					string MaxPixels = EditorVariables.DK_DKUMAGenerator.maxPixels.ToString();
					MaxPixels = GUILayout.TextField(MaxPixels, 8, GUILayout.Width (60));
					MaxPixels = Regex.Replace(MaxPixels, "[^0-9]", "");
					if ( MaxPixels == "" ) MaxPixels = "1";
					EditorVariables.DK_DKUMAGenerator.maxPixels = Convert.ToInt32(MaxPixels);
				}
				if ( EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd )using (new Horizontal()) {
					if ( EditorVariables.DK_DKUMAGenerator.AtlasCrop == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("Atlas Crop")){
						if ( EditorVariables.DK_DKUMAGenerator.AtlasCrop == true )EditorVariables.DK_DKUMAGenerator.AtlasCrop = false;
						else EditorVariables.DK_DKUMAGenerator.AtlasCrop = true;
					}
					if ( EditorVariables.DK_DKUMAGenerator.fitAtlas == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("Atlas fit")){
						if ( EditorVariables.DK_DKUMAGenerator.fitAtlas == true )EditorVariables.DK_DKUMAGenerator.fitAtlas = false;
						else EditorVariables.DK_DKUMAGenerator.fitAtlas = true;
					}
					if ( EditorVariables.DK_DKUMAGenerator.convertRenderTexture == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("convert Texture")){
						if ( EditorVariables.DK_DKUMAGenerator.convertRenderTexture == true )EditorVariables.DK_DKUMAGenerator.convertRenderTexture = false;
						else EditorVariables.DK_DKUMAGenerator.convertRenderTexture = true;
					}
				}
				#endregion Atlas and resolution
				#region Mesh
				if ( EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd )using (new Horizontal()) {
					// Mesh Updates
					GUI.color = Color.white;
					GUILayout.Label("Mesh Updates :", GUILayout.ExpandWidth (false));
					string _meshUpdates = EditorVariables.DK_DKUMAGenerator.meshUpdates.ToString();
					_meshUpdates = GUILayout.TextField(_meshUpdates, 5, GUILayout.Width (40));
					_meshUpdates = Regex.Replace(_meshUpdates, "[^0-9]", "");
					if ( _meshUpdates == "" ) _meshUpdates = "1";
					EditorVariables.DK_DKUMAGenerator.meshUpdates = Convert.ToInt32(_meshUpdates);
					// Max Mesh Updates
					GUI.color = Color.white;
					GUILayout.Label("Max :", GUILayout.ExpandWidth (false));
					string _MaxmeshUpdates = EditorVariables.DK_DKUMAGenerator.maxMeshUpdates.ToString();
					_MaxmeshUpdates = GUILayout.TextField(_MaxmeshUpdates, 5, GUILayout.Width (40));
					_MaxmeshUpdates = Regex.Replace(_MaxmeshUpdates, "[^0-9]", "");
					if ( _MaxmeshUpdates == "" ) _MaxmeshUpdates = "1";
					EditorVariables.DK_DKUMAGenerator.maxMeshUpdates = Convert.ToInt32(_MaxmeshUpdates);
				}
				if ( EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd )using (new Horizontal()) {	
					// Flesh Variation
					GUI.color = Color.white;
					GUILayout.Label("Color Variation :", GUILayout.ExpandWidth (false));
				}
				if ( EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd )using (new Horizontal()) {	
					GUILayout.Label("Flesh", GUILayout.ExpandWidth (false));
					EditorVariables.DK_UMACrowd.Colors.AdjRanMaxi = GUILayout.HorizontalSlider(EditorVariables.DK_UMACrowd.Colors.AdjRanMaxi ,0,0.5f);
					GUILayout.Label(EditorVariables.DK_UMACrowd.Colors.AdjRanMaxi.ToString(), GUILayout.Width (40));
				}
				using (new Horizontal()) {
					// Hair Variation
					GUI.color = Color.white;
					GUILayout.Label("Hair", GUILayout.ExpandWidth (false));
					EditorVariables.DK_UMACrowd.Colors.HairAdjRanMaxi = GUILayout.HorizontalSlider(EditorVariables.DK_UMACrowd.Colors.HairAdjRanMaxi ,0,0.5f);
					GUILayout.Label(EditorVariables.DK_UMACrowd.Colors.HairAdjRanMaxi.ToString(), GUILayout.Width (40));
				}
				if ( EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd )using (new Horizontal()) {	
					// wear Variation
					GUI.color = Color.white;
					GUILayout.Label("Wear", GUILayout.ExpandWidth (false));
					EditorVariables.DK_UMACrowd.Colors.WearAdjRanMaxi = GUILayout.HorizontalSlider(EditorVariables.DK_UMACrowd.Colors.WearAdjRanMaxi ,0,0.5f);
					GUILayout.Label(EditorVariables.DK_UMACrowd.Colors.WearAdjRanMaxi.ToString(), GUILayout.Width (40));
				}
				if ( EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd )using (new Horizontal()) {	
						if ( EditorVariables.DK_UMACrowd.UseLinkedOv == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("Use Linked Overlay(s)")){
							if ( EditorVariables.DK_UMACrowd.UseLinkedOv == true ) EditorVariables.DK_UMACrowd.UseLinkedOv = false;
							else EditorVariables.DK_UMACrowd.UseLinkedOv = true;
					}
						if ( !EditorVariables._DKUMACustomization ) EditorVariables._DKUMACustomization = GameObject.Find("DKUMACustomization").GetComponent<DKUMACustomization>();
						if ( EditorVariables._DKUMACustomization && EditorVariables._DKUMACustomization.DisplayUI == true ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("Ingame UI")){
							EditorUtility.SetDirty(EditorVariables._DKUMACustomization.gameObject);
							if ( EditorVariables._DKUMACustomization.DisplayUI == true ){
								EditorVariables._DKUMACustomization.DisplayUI = false;
								EditorVariables._DKUMACustomization.CloseSliders();
						}
						else {
								EditorVariables._DKUMACustomization.DisplayUI = true;
								EditorVariables._DKUMACustomization.SetSliders();
						}
						AssetDatabase.SaveAssets();
					}
				}
				#endregion Mesh

				#region Select
				GUILayout.Space(5);
				GUI.color = Color.white;
				GUILayout.Label("Select UMA Components", "toolbarbutton", GUILayout.ExpandWidth (true));
				GUILayout.Space(5);
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Race Library :", GUILayout.Width (110));
					GUI.color = Color.white;
					try{
						try{
							GUILayout.TextField ( EditorVariables.RaceLibraryObj.name, GUILayout.ExpandWidth (true));
							if ( GUILayout.Button ( "Change", GUILayout.Width (60))){
								OpenLibrariesWindow();
								ChangeLibrary.CurrentLibN = EditorVariables.RaceLibraryObj.name;
								ChangeLibrary.CurrentLibrary = EditorVariables.RaceLibraryObj;
								ChangeLibrary.Action = "";
							}
						}catch(MissingReferenceException){}
					}catch(ArgumentException){}
				}
				GUI.color = Color.yellow;
				using (new Horizontal()) {
					GUILayout.Label ( "Slot Library :", GUILayout.Width (110));
					GUI.color = Color.white;
				//	if ( !EditorVariables.DKSlotLibraryObj ) DetectAndAddDK.DetectAll();
					if ( EditorVariables.DKSlotLibraryObj )GUILayout.TextField ( EditorVariables.DKSlotLibraryObj.name, GUILayout.ExpandWidth (true));
					if ( GUILayout.Button ( "Change", GUILayout.Width (60))){
						OpenLibrariesWindow();
						ChangeLibrary.CurrentLibN = EditorVariables.DKSlotLibraryObj.name;
						ChangeLibrary.CurrentLibrary = EditorVariables.DKSlotLibraryObj;
					}
				}
				using (new Horizontal()) {
					GUI.color = Color.yellow;
					GUILayout.Label ( "Overlay Library :", GUILayout.Width (110));
					GUI.color = Color.white;
					try{try{
						GUILayout.TextField ( EditorVariables.OverlayLibraryObj.name, GUILayout.ExpandWidth (true));
						if ( GUILayout.Button ( "Change", GUILayout.Width (60))){
							OpenLibrariesWindow();
							ChangeLibrary.CurrentLibN = EditorVariables.OverlayLibraryObj.name;
							ChangeLibrary.CurrentLibrary = EditorVariables.OverlayLibraryObj;
						}
					}catch(MissingReferenceException){}}catch(ArgumentException){}
				}
				#region Helper
				if ( Helper ) 
				{
					GUI.color = Color.yellow;
					GUILayout.TextField("Quickly select a UMA Component without browsing the Hierarchy." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				}
				#endregion Helper
					if ( EditorVariables.DK_DKUMAGenerator && EditorVariables.DK_UMACrowd )using (new Horizontal()) {	
					if ( Selection.activeGameObject && Selection.activeGameObject != EditorVariables.DK_DKUMAGenerator.gameObject ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("Generator")){
						if ( Selection.activeGameObject != EditorVariables.DK_DKUMAGenerator.gameObject )
						Selection.activeGameObject = EditorVariables.DK_DKUMAGenerator.gameObject;
					}
						if ( Selection.activeGameObject != EditorVariables.DK_UMACrowd.gameObject ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("Crowd")){
							if ( Selection.activeGameObject != EditorVariables.DK_UMACrowd.gameObject )
								Selection.activeGameObject = EditorVariables.DK_UMACrowd.gameObject;
					}
			
					if (EditorVariables.OverlayLibraryObj && Selection.activeGameObject != EditorVariables.OverlayLibraryObj.gameObject ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("Overlay")){
						if ( Selection.activeGameObject != EditorVariables.OverlayLibraryObj.gameObject )
						Selection.activeGameObject = EditorVariables.OverlayLibraryObj.gameObject;
					}
					if ( Selection.activeGameObject && EditorVariables.DKSlotLibraryObj && Selection.activeGameObject != EditorVariables.DKSlotLibraryObj.gameObject ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("Slot")){
						if ( Selection.activeGameObject != EditorVariables.DKSlotLibraryObj.gameObject )
						Selection.activeGameObject = EditorVariables.DKSlotLibraryObj.gameObject;
					}
					if ( Selection.activeGameObject && Selection.activeGameObject != EditorVariables.RaceLibraryObj.gameObject ) GUI.color = Green;
					else GUI.color = Color.gray;
					if(GUILayout.Button("Race")){
						if ( Selection.activeGameObject != EditorVariables.RaceLibraryObj.gameObject )
						Selection.activeGameObject = EditorVariables.RaceLibraryObj.gameObject;
					}
				}
				#endregion Select
			}
			#endregion Engine
		}
		#endregion Setup

		#region Plug In
		if ( showPlugIn == true ) {
			#region Plug In Open
			if ( PlugInDataList.Count != 0 ) {
				GUILayout.Space(5);
				GUI.color = Color.yellow;
				GUILayout.TextField("Select a DK UMA Plug-In from the list and open its window.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				GUI.color = Green;
				if( PlugInSelected != "" && GUILayout.Button("Open it !")){

				}
			}
			#endregion Plug In Open

			#region new Plug In
			GUILayout.Space(5);
			GUI.color = Color.white;
			GUILayout.TextField("You can search for new Plug-Ins in the Assetstore.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUI.color = Green;
			if( GUILayout.Button("Go to the Asset Store !")){
				Application.OpenURL ("https://www.assetstore.unity3d.com/en/#!/search/DK%20UMA%20Plug%20In");
			}
			GUI.color = Red;
			GUILayout.TextField("Creating a new DK UMA Plug-In will require from the user to have an advanced knowledge of the C# usage.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUI.color = Color.yellow;
			GUILayout.TextField("Expert : Create a new DK UMA Plug-In", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));

			GUI.color = Red;
			if( GUILayout.Button("new DK UMA Plug-In")){
				OpenPlugInCreator ();
			}
			#endregion new Plug In

			#region Plug In List
			// Prepare list
			aFilePaths = Directory.GetFiles("Assets/DK Editors/DK_UMA_Editor/PlugIns/");
			aWinPaths = Directory.GetFiles("Assets/DK Editors/DK_UMA_Editor/Editor/");

			GUILayout.Space(5);
			GUI.color = Color.cyan;
			GUILayout.Label ( "Plug-Ins List", "toolbarbutton");
			#region umpty Plug In List
			if ( aFilePaths.Length == 0 ) {
				GUI.color = Color.white;
				GUILayout.TextField("You have no Plug-In installed. " +
				   "Download an available tool or create a new one.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				GUI.color = Green;
				if( GUILayout.Button("Go to the Asset Store !")){
					Application.OpenURL ("https://www.assetstore.unity3d.com/en/#!/search/DK%20UMA%20Plug%20In");
				}
			}
			#endregion umpty Plug In List
			#region populated Plug In List
			else{
				using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label("Name", "toolbarbutton", GUILayout.Width (200));
					GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
				}
				if ( aFilePaths.Length != 0 )using (new ScrollView(ref scroll4)){
					foreach (string sFilePath in aFilePaths) {
						bool _Details;

						if ( sFilePath.Contains(".asset") == true && sFilePath.Contains(".asset.meta") == false  ){

							FileName = sFilePath.Replace( "Assets/DK Editors/DK_UMA_Editor/PlugIns/", "" );
							FileName = FileName.Replace( ".asset", "" );
						}
						var _PlugInData = Resources.LoadAssetAtPath (sFilePath,  typeof(PlugInData)) as PlugInData;
						if ( _PlugInData ) using (new Horizontal()) {
							// Open window
							GUI.color = Color.white;
							if ( GUILayout.Button(FileName, GUILayout.Width (200))){
								EditorWindow _newWin = EditorWindow.Instantiate(_PlugInData.Window) as EditorWindow;
								_newWin.Show();
							}
							if ( SelectedFile == FileName ) GUI.color = Green;
							else GUI.color = Color.white;
							if( GUILayout.Button("?", GUILayout.ExpandWidth (false))){
								if ( SelectedFile == FileName )SelectedFile = "";
								else SelectedFile = FileName;
								
							}
							if ( _PlugInData.Web == null ) _PlugInData.Web = "";
							if ( _PlugInData.Web == "" ) GUI.color = Color.gray;
							else GUI.color = Color.white;
							if( GUILayout.Button("Web", GUILayout.ExpandWidth (false))){
								if ( _PlugInData.Web != "" ) Application.OpenURL ( _PlugInData.Web );							}
						}
						if ( _PlugInData ){

							
						}
						if ( _PlugInData && SelectedFile == FileName ) {
							using (new Horizontal()) {
								GUILayout.Space(15);
								GUILayout.Label("Version :", GUILayout.ExpandWidth (false));
								GUILayout.Label (_PlugInData.Version.ToString(), GUILayout.Width (35));
								GUILayout.Space(5);
								GUILayout.Label("Author :", GUILayout.ExpandWidth (false));
								GUILayout.Label (_PlugInData.Author, GUILayout.Width (140));
							
							}
							if ( _PlugInData.Description == null ) _PlugInData.Description = "";
							if (_PlugInData.Description != "" ) {
								using (new Horizontal()) {
									GUILayout.Space(15);
									GUILayout.TextField (_PlugInData.Description, 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
								}
							}else GUILayout.TextField ( "The Plug-In does not have a Description.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
						}
					}
				}
			
			}
			#endregion populated Plug In List
			#endregion Plug In List
		}
		#endregion Plug In

			#region About
			if ( showAbout ){
				using (new HorizontalCentered()) 
				{
					GUI.color = Color.white;
					GUILayout.Label ( "Dynamic Kit U.M.A.1 Editor (v1.6)", bold);
				}
				using (new HorizontalCentered()) 
				{
					GUILayout.Label ( "version 1650"+" - Unity version 5"+" & UMA 1.");
				}
				GUILayout.Space(5);
				GUI.color = Color.yellow;
				GUILayout.TextField("Greetings to Fernando Ribeiro for the creation of U.M.A. and LaneFox for his models.", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				GUILayout.Space(5);
				
				using (new HorizontalCentered()) 
				{
					GUI.color = Green;
					GUILayout.TextField("(c) 2015 Ricardo Luque Martos", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
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
					if(GUILayout.Button("Web Documentation")){
						OpenDocumentationLink ();
					}
				}
				
				using (new Horizontal()) 
				{
					GUI.color = Color.cyan;
					if(GUILayout.Button("Facebook Page")){
						Application.OpenURL ("https://www.facebook.com/DKeditorsUnity3D");
					}
				}
				GUILayout.TextField("The Premium package contains all the plug-ins :", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				GUI.color = Color.white;
				if(GUILayout.Button("DK UMA Premium")){
					Application.OpenURL ("https://www.assetstore.unity3d.com/#!/content/18606");
				}
				// plugins preview
				GUILayout.TextField("Plug-Ins for DK UMA :", 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
				GUI.color = Color.white;
				if(GUILayout.Button("Natural Behaviour")){
					Application.OpenURL ("https://www.assetstore.unity3d.com/#!/content/20836");
				}
				if(GUILayout.Button("Race DNA Editor")){
					Application.OpenURL ("https://www.assetstore.unity3d.com/#!/content/36635");
				}
				if(GUILayout.Button("RPG Avatar Editor")){
					Application.OpenURL ("https://www.assetstore.unity3d.com/#!/content/36627");
				}
				
				
			}
			#endregion About

		#endregion Menu
		}
	}

	public static DKDnaConverterBehaviour _DnaConverterBehaviour;

	public static void ResetSteps () {
		Step1 = false;Step2 = false;Step3 = false;Step4 = false;Step5 = false;Step6 = false;Step7 = false;

	}

	void CreateDNASliders( DKUMAData _DKUMAData ){
		EditorVariables.tmpDNAList.Clear();
		for (int i = 0; i <_DKUMAData.DNAList2.Count ; i++)
		{
			DKRaceData.DNAConverterData DNA = new DKRaceData.DNAConverterData();
			DNA.Name = _DKUMAData.DNAList2[i].Name;
			DNA.Value = _DKUMAData.DNAList2[i].Value;
			DNA.Part = _DKUMAData.DNAList2[i].Part;
			DNA.Part2 = _DKUMAData.DNAList2[i].Part2;
			EditorVariables.tmpDNAList.Add(DNA);
		}
	}

	void GetDNAValues( int i ){

	}

	void OnSelectionChange() {
		EditorSelectionChange.OnSelectionChange ();

	}
}
