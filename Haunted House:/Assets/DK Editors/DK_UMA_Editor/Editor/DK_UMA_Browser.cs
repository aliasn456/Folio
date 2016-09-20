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


[InitializeOnLoad]
public class DK_UMA_Browser : EditorWindow {
	
	#region Variables
	Color Green = new Color (0.8f, 1f, 0.8f, 1);
	Color Red = new Color (0.9f, 0.5f, 0.5f);

	Vector2 scroll;
	
	GameObject ObjToFind;
	Transform MFSelectedList;
	
	public static GameObject UMAModel;
	bool CreatingUMA;
	bool Helper;
	bool AllUMAModels = true;
	bool AllElements;
	bool AllPrefabElements;
	bool AllPrefabUMAModels;
	public DKUMAGenerator _DKUMAGenerator;
	public DK_UMACrowd DK_UMACrowd;
	public DKUMACustomization _DKUMACustomization;
//	public DK_DKUMACustomization DK_DKUMACustomization;
	GameObject DKUMAGeneratorObj;
	GameObject UMACrowdObj;
	GameObject DKUMACustomizationObj;
	string myPath;
	public static string SearchString = "";
	string NName = "";
	// fid
	 public Type DKUMAData = typeof(DKUMAData);
	Transform SelectedGrp;

	bool displaySlots;
	bool displayRaces;
	bool displayOverlays;
	bool SpawnAtZP;

	string _Group;
	string _Unit;
	GameObject tmpGrp;
	GameObject SelectedInstance;

	List<GameObject> OpenedList = new List<GameObject>();
	List<string> OpGrpList = new List<string>();
	List<string> OpList = new List<string>();

	private static Index<string, Index<string, List<object>>> _assetStoreM = new Index<string, Index<string, List<object>>>();
	private Dictionary<string, bool> openM = new Dictionary<string, bool> ();
	private static Index<string, Index<string, List<object>>> _assetStore = new Index<string, Index<string, List<object>>>();
	private Dictionary<string, bool> open = new Dictionary<string, bool> ();
	private static Index<string, Index<string, List<object>>> _assetStoreOv = new Index<string, Index<string, List<object>>>();
	private Dictionary<string, bool> openOv = new Dictionary<string, bool> ();
	private static Index<string, Index<string, List<object>>> _assetStoreRa = new Index<string, Index<string, List<object>>>();
	private Dictionary<string, bool> openRa = new Dictionary<string, bool> ();

	GUIContent _Prefab = new GUIContent("P", "Create or Delete an Asset.");
	GUIContent _AutoDelMiss = new GUIContent("Auto Delete Missings", "Verify the Library and delete the missing fields, multiple clicks.");
	GUIContent _Delete = new GUIContent("X", "Delete.");
	GUIContent _Duplic = new GUIContent("C", "Create a copy.");
	GUIContent _GroupIt = new GUIContent("G", "Add the Model to the selected Group. If the Model is already in a Group, the model is removed and placed at the Root.");
	GUIContent _Inst = new GUIContent("Instantiate", "Create an Instance of the Asset.");
	#endregion Variables
	
	[MenuItem("UMA/DK Editor/UMA Browser %;")]
	[MenuItem("Window/DK Editors/DK UMA/UMA Browser %,")]

    public static void Init()
    {
       GetWindow(typeof(DK_UMA_Browser), false, "UMA Browser");

	}

	public static void OpenEditorWindow(){
		GetWindow(typeof(DK_UMA_Editor), false, "DK UMA Editor");
	}

	public static void OpenAddProcessWindow(){
		GetWindow(typeof(DKUMAProcessWindow), false, "Processing...");
		DKUMAProcessWindow.AddAssetsProcess();
	}

	public void AddAssets(){
		OpenAddProcessWindow();
	}

	void OnGUI () {
		this.minSize = new Vector2(330, 500);
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
	
	#region Variables
		ObjToFind = GameObject.Find("NPC Models");
		if ( ObjToFind == null ) 
		{	var go = new GameObject();	go.name = "NPC Models";	ObjToFind = GameObject.Find("NPC Models");		}
		if ( ObjToFind != null )
		{
			MFSelectedList = ObjToFind.transform;
			DKUMAGeneratorObj = GameObject.Find("DKUMAGenerator");	
			UMACrowdObj = GameObject.Find("DKUMACrowd");	
			DKUMACustomizationObj = GameObject.Find("DKUMACustomization");	
			if ( DKUMAGeneratorObj != null ) _DKUMAGenerator =  DKUMAGeneratorObj.GetComponent<DKUMAGenerator>();
			if ( DKUMACustomizationObj != null ) _DKUMACustomization =  DKUMACustomizationObj.GetComponent<DKUMACustomization>();			
		}
		#endregion Variables
		
		#region Title
		using (new Horizontal()) {
			GUILayout.Label ( "Dynamic Kit U.M.A. Browser", "toolbarbutton", GUILayout.ExpandWidth (true));
			if (GUILayout.Button ( "Editor", "toolbarbutton", GUILayout.ExpandWidth (false))) {
				OpenEditorWindow();
			}
		}
		#endregion Title
		
		#region Main Menu
		using (new Horizontal()) {
			// Instantiated Models
			if ( AllUMAModels == true ) GUI.color = Green;
			else GUI.color = Color.white;
			if (GUILayout.Button ( "models", "toolbarbutton", GUILayout.ExpandWidth (false))) {
				AllUMAModels = true;
				AllPrefabUMAModels = false;
				AllElements = false;
				AllPrefabElements = false;
			}
			if ( AllPrefabUMAModels == true ) GUI.color = Green;
			else GUI.color = Color.white;
			if (GUILayout.Button ( "Prefabs", "toolbarbutton", GUILayout.ExpandWidth (false))) {
				AllUMAModels = false;
				AllPrefabUMAModels = true;
				AllElements = false;
				AllPrefabElements = false;
			}
			if ( AllElements == true ) GUI.color = Green;
			else GUI.color = Color.white;
			if (GUILayout.Button ( "Elements", "toolbarbutton", GUILayout.ExpandWidth (false))) {
				AllUMAModels = false;
				AllPrefabUMAModels = false;
				AllElements = true;
				AllPrefabElements = false;
			}
			if ( AllPrefabElements == true ) GUI.color = Green;
			else GUI.color = Color.white;
			if (GUILayout.Button ( "Assets", "toolbarbutton", GUILayout.ExpandWidth (false))) {
				AllUMAModels = false;
				AllPrefabUMAModels = false;
				AllElements = false;
				AllPrefabElements = true;

			}
			GUI.color = Color.white;
			GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
			if ( !Helper )  GUI.color = Color.yellow;
			else GUI.color = Green;
			if (GUILayout.Button ( "?", "toolbarbutton", GUILayout.ExpandWidth (false))) {
				if ( Helper ) Helper = false;
				else Helper = true;
			}
		}
		
		#endregion Main Menu
		
		#region Welcome
		if ( !AllUMAModels && !AllPrefabUMAModels && !AllElements && !AllPrefabElements ) {
			GUILayout.Space(15);
			using (new HorizontalCentered()) {
				GUI.color = Color.white;
				GUILayout.TextField("Welcome in the DK UMA Browser", 256, bold, GUILayout.ExpandWidth (true));
			}
			GUILayout.Space(15);
			GUI.color = Color.yellow;
			GUILayout.TextField("To correctly manipulate the UMA models, you need to use the DK UMA Browser to select the UMA models.", 256, style, GUILayout.ExpandWidth (true));
			GUILayout.Space(5);
			GUI.color = Color.white;
			GUILayout.TextField("To manipulate the instantiated UMA models present in the current scene, open UMA models Tab by clicking in the on top menu.", 256, style, GUILayout.ExpandWidth (true));
			GUILayout.Space(5);
			GUILayout.TextField("To manipulate the Prefab UMA models stored in your project, open Prefab UMA models Tab by clicking in the on top menu.", 256, style, GUILayout.ExpandWidth (true));
			GUILayout.Space(5);
			GUI.color = Color.yellow;
			GUILayout.TextField("Help : You can activate the help comments by clicking on the '?' at the end of the ontop menu. " +
				"The comments are displayed to explain to you any aspect of the DK UMA Editor." , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			
		}
		#endregion Welcome
		
		GUILayout.Space(5);
		#region Helper : Main
		GUI.color = Color.yellow;
		#endregion Helper : Main
		#region Helper: Edit
		GUI.color = Color.white;
		#endregion Helper: Edit
		
		#region Lists
		#region UMA Instances
		if ( AllUMAModels == true ) {

			if ( EditorVariables.MFSelectedList && EditorVariables.MFSelectedList.childCount != 0 ) 
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("Name :", GUILayout.Width (75));
				if ( EditorVariables.MFSelectedList && EditorVariables.MFSelectedList.childCount == 0 ) 
					GUILayout.TextField("List is umpty, create some models." , 256, style, GUILayout.ExpandWidth (true));
				NName = GUILayout.TextField(NName, 100, GUILayout.ExpandWidth (true));
				if ( Selection.activeGameObject && UMAModel && Selection.activeGameObject == UMAModel.gameObject
				    && GUILayout.Button("Rename", GUILayout.ExpandWidth (false)))
				{
					UMAModel.gameObject.name = NName;
				}
			}
			#region grp asset
			using (new Horizontal()) {

				GUI.color = Color.white;
				GUILayout.Label("Group :", GUILayout.Width (75));
				if ( SelectedGrp ) GUILayout.Label(SelectedGrp.name+" (Selected)", GUILayout.ExpandWidth (true));
				if ( GUILayout.Button("New", GUILayout.ExpandWidth (false)))
				{
					GameObject NewGrp = new GameObject();
					NewGrp.AddComponent<ModelGrp>();
					NewGrp.name = "New Group";
					NName = "New Group";
					NewGrp.transform.parent = MFSelectedList.transform;
					SelectedGrp = NewGrp.transform;
					GameObject ZPoint = GameObject.Find("ZeroPoint");
					NewGrp.transform.position = ZPoint.transform.position;

					Selection.activeGameObject = NewGrp;
				}
				GUI.color = Color.cyan;
				if ( Selection.activeGameObject && Selection.activeGameObject.GetComponent<ModelGrp>() == true
				    && GUILayout.Button("Add Loners", GUILayout.ExpandWidth (false)))
				{
					
					if ( EditorVariables.MFSelectedList != null ) foreach (Transform Model in EditorVariables.MFSelectedList.transform)
					{
						DK_Model _DK_Model = Model.gameObject.GetComponent< DK_Model >();
						if ( Model && _DK_Model 
						    && _DK_Model.IsUmaModel )
						{
							Model.transform.parent = SelectedGrp.transform;
						}
					}
				}
			}
			#endregion grp asset
			GUI.color = Color.yellow;
			if ( EditorVariables.MFSelectedList && EditorVariables.MFSelectedList.childCount != 0 && Helper ) GUILayout.TextField("You can show or hide all the models. " +
				"Also you can create a Prefab for any model instance by clicking on the 'Add' button." 
				, 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			GUI.color = Red;
			if ( EditorVariables.MFSelectedList && EditorVariables.MFSelectedList.childCount != 0 && Helper ) GUILayout.TextField("Please take care using the following options, you don't have any Undo option about that."
			    , 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			if ( EditorVariables.MFSelectedList && EditorVariables.MFSelectedList.childCount != 0 ) using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("Act on All :",  GUILayout.Width (75));
				if ( GUILayout.Button("Show", GUILayout.ExpandWidth (true)))
				{
					if ( EditorVariables.MFSelectedList != null ) foreach (Transform Model in EditorVariables.MFSelectedList.transform)
		        	{
						Model.gameObject.SetActive(true);
					}
				}
				if ( GUILayout.Button("Hide", GUILayout.ExpandWidth (true)))
				{
					if ( EditorVariables.MFSelectedList != null ) foreach (Transform Model in EditorVariables.MFSelectedList.transform)
		        	{
						Model.gameObject.SetActive(false);
					}
				}
				GUI.color = Color.cyan;
				if ( GUILayout.Button("Add", GUILayout.ExpandWidth (true)))
				{
					AddAssets();
				}
			/*	GUI.color = Color.cyan;
				if ( GUILayout.Button("Del", GUILayout.ExpandWidth (true)))
				{
					DK_UMA_Editor.OpenDeleteAsset();
					DeleteAsset.UMAModel = false;
					DeleteAsset.MultiUMAModel = true;
				}*/
				GUI.color = Red;
				if ( GUILayout.Button("Del", GUILayout.ExpandWidth (true)))
				{
					DK_UMA_Editor.OpenDeleteAsset();
					DeleteAsset.UMAModel = false;
					DeleteAsset.MultiUMAModel = false;
					DeleteAsset.ProcessName = "Delete all models";
				}
			}
			if ( UMAModel != null )  using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("Selected :",  GUILayout.Width (75));
				if (UMAModel.gameObject 
				    && ( UMAModel.gameObject.GetComponent<ModelGrp>() != null || UMAModel.gameObject.GetComponent<DK_Model>() != null)
				    &&	UMAModel.gameObject.activeInHierarchy != true 
				    && GUILayout.Button("Show", GUILayout.ExpandWidth (true)))
				{
					if ( UMAModel.gameObject.GetComponent<ModelGrp>() != null || UMAModel.gameObject.GetComponent<DK_Model>() != null ){
						UMAModel.gameObject.SetActive(true);
					}
				}
				if (UMAModel.gameObject 
				    && ( UMAModel.gameObject.GetComponent<ModelGrp>() != null || UMAModel.gameObject.GetComponent<DK_Model>() != null)
				    &&	UMAModel.gameObject.activeInHierarchy == true 
					&&	GUILayout.Button("Hide", GUILayout.ExpandWidth (true)))
				{
					if ( UMAModel.gameObject.GetComponent<ModelGrp>() != null || UMAModel.gameObject.GetComponent<DK_Model>() != null ){
						UMAModel.gameObject.SetActive(false);
					}
				}
				GUI.color = Color.white;
				if ( GUILayout.Button("Copy", GUILayout.ExpandWidth (true))){
					GameObject _Copy = Instantiate(UMAModel.gameObject) as GameObject;
					_Copy.name = UMAModel.name + " (Copy)";
					_Copy.transform.parent = UMAModel.transform.parent;
					_Copy.gameObject.GetComponentInChildren<DKUMASaveTool>().AutoLoad();
					_Copy.transform.position = new Vector3(UMAModel.transform.position.x + 1.0f, UMAModel.transform.position.y, UMAModel.transform.position.z );
				
				}
				GUI.color = Color.white;
				if ( GUILayout.Button("to ZPoint", GUILayout.ExpandWidth (true))){
					GameObject ZPoint = GameObject.Find("ZeroPoint");
					UMAModel.transform.position =  ZPoint.transform.position;
				}
				// Prefab
				if ( PrefabUtility.GetPrefabParent( UMAModel.gameObject ) != null )
					myPath = PrefabUtility.GetPrefabParent( UMAModel.gameObject ).ToString() ;
				else myPath = "null";
				GUI.color = Color.cyan;
				if ( myPath == "null" ){
					if ( GUILayout.Button ( "Add", GUILayout.ExpandWidth (false))) {
						// Create Prefab
						if ( myPath == "null" ) 
						{
							if ( UMAModel.GetComponent<DK_Model>() == true ){
								PrefabUtility.CreatePrefab("Assets/DK Editors/DK_UMA_Editor/Prefabs/Models/" + UMAModel.name + ".prefab", UMAModel.gameObject,ReplacePrefabOptions.ConnectToPrefab );

							}
							else
							if ( UMAModel.GetComponent<ModelGrp>() == true ){
								PrefabUtility.CreatePrefab("Assets/DK Editors/DK_UMA_Editor/Prefabs/Models/Groups/" + UMAModel.name + ".prefab", UMAModel.gameObject,ReplacePrefabOptions.ConnectToPrefab );

							}
						}
					}
				}
				else{
					GUI.color = Color.cyan;
					if ( GUILayout.Button("Del", GUILayout.ExpandWidth (true)))
					{
						DK_UMA_Editor.OpenDeleteAsset();
						DeleteAsset.MultiUMAModel = false;
						DeleteAsset.UMAModel = true;
					}
				}
				GUI.color = Red;
				if ( GUILayout.Button("Del", GUILayout.ExpandWidth (true)))
				{
					DestroyImmediate( UMAModel.gameObject );
				}

			}
			GUILayout.Space(5);

			#region Search
			if ( EditorVariables.MFSelectedList && EditorVariables.MFSelectedList.childCount != 0 ) using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("Search for :", GUILayout.Width (75));
				SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));

			}
			#endregion Search

			GUILayout.Space(5);
			// Find Models and store in the list
			GUI.color = Color.yellow;
			if ( EditorVariables.MFSelectedList && EditorVariables.MFSelectedList.childCount != 0 && Helper ) GUILayout.TextField("To select a model, click in the list bellow. " +
				"To modify a UMA Model, open the 'Modify' Tab of the DK UMA Editor window." 
				, 256, style, GUILayout.ExpandWidth (true), GUILayout.ExpandWidth (true));
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("UMA Models List", "toolbarbutton", GUILayout.ExpandWidth (true));
			}
			using (new Horizontal()) {
				GUI.color = Color.cyan;
				GUILayout.Label("Model's Name", "toolbarbutton", GUILayout.Width (200));
				GUILayout.Label("Race", "toolbarbutton", GUILayout.Width (80));
				GUILayout.Label("Gender", "toolbarbutton", GUILayout.Width (80));


				GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
			}
			GUI.color = Color.white;
			using (new ScrollView(ref scroll)) 
			{
		        if ( EditorVariables.MFSelectedList != null ) foreach (Transform Model in EditorVariables.MFSelectedList.transform)
		        {
					DK_Model _DK_Model = Model.gameObject.GetComponent< DK_Model >();
					ModelGrp _ModelGrp = Model.gameObject.GetComponent< ModelGrp >();

					if ( _ModelGrp == null && _DK_Model == null )  Model.gameObject.AddComponent< ModelGrp >();

					// group list
					if ( Model && _DK_Model == null) using (new Horizontal()) 
					{
						// select
						if (Model && UMAModel && _ModelGrp && Selection.activeGameObject == Model.gameObject  )
						    GUI.color = Color.yellow;
						else if ( Model != null && Model.gameObject.activeInHierarchy == false ) GUI.color = Color.gray;
						else GUI.color = Color.white;
						if ( Model != null && GUILayout.Button(Model.name+" ("+Model.childCount.ToString()+")", "FoldOut", GUILayout.ExpandWidth (true)))
						{
							UMAModel = Model.gameObject;
							EditorVariables.EditedModel = Model;
							Selection.activeGameObject = Model.gameObject;
							EditorVariables.NewModelName = Model.name;
							if ( _ModelGrp && _ModelGrp.Show ) _ModelGrp.Show = false;
							else if ( _ModelGrp ) _ModelGrp.Show = true;
							NName = Model.gameObject.name;
							SelectedGrp = Model;
						}
						// Prefab
						// Prefab Verification
						if ( PrefabUtility.GetPrefabParent( Model.gameObject ) != null )
							myPath = PrefabUtility.GetPrefabParent( Model.gameObject ).ToString() ;
						else myPath = "null";
						if ( myPath == "null" ) GUI.color = Color.gray;
						else GUI.color = Color.cyan;
						if ( GUILayout.Button ( new GUIContent(_Prefab), "toolbarbutton", GUILayout.ExpandWidth (false))) {

							// Create Prefab
							if ( myPath == "null" ) 
							{
								PrefabUtility.CreatePrefab("Assets/DK Editors/DK_UMA_Editor/Prefabs/Models/Groups/" + Model.name + ".prefab", Model.gameObject,ReplacePrefabOptions.ConnectToPrefab  );
							
							}
							else {
								DeleteAsset.MultiUMAModel = false;
								DeleteAsset.UMAModel = true;
								DK_UMA_Editor.OpenDeleteAsset();
								Selection.activeGameObject = Model.gameObject;

							}
						}
					}
					if ( _ModelGrp && _ModelGrp.Show ) foreach (Transform Model2 in Model.transform)
					{
						DK_Model _DK_Model2 = Model2.gameObject.GetComponent< DK_Model >();
						if ( Model2 && _DK_Model2 
						    && _DK_Model2.IsUmaModel 
						    &&  ( SearchString == "" 
						     || Model2.name.ToLower().Contains(SearchString.ToLower()) ) ) using (new Horizontal()) 
						{
							GUILayout.Space(40);
							// Prefab
							// Prefab Verification
							if ( PrefabUtility.GetPrefabParent( Model2.gameObject ) != null )
								myPath = PrefabUtility.GetPrefabParent( Model2.gameObject ).ToString() ;
							else myPath = "null";
							if ( myPath == "null" ) GUI.color = Color.gray;
							else GUI.color = Color.cyan;
					
							GUI.color = Green;
							if ( GUILayout.Button ( new GUIContent(_GroupIt), "toolbarbutton", GUILayout.ExpandWidth (false))) {
								// Group to
								if ( Model2.parent.GetComponent<ModelGrp>() == true ) Model2.parent = Model2.parent.parent;
								else
								Model2.parent = SelectedGrp;
							}
							// select
							try{ if (Model2 && UMAModel && Selection.activeGameObject 
								     && ( Selection.activeGameObject == Model2.gameObject) 
								    || Model2.gameObject == UMAModel ) 
								{
									GUI.color = Color.yellow;
								}
								else if ( Model2 != null && Model2.gameObject.activeInHierarchy == false ) GUI.color = Color.gray;
								else GUI.color = Color.white;
							}
							catch (MissingReferenceException ) {
								
							}
							if ( Model2 != null && GUILayout.Button(Model2.name, "toolbarbutton", GUILayout.Width (160)))
							{
								UMAModel = Model2.gameObject;
								EditorVariables.EditedModel = Model2;
								Selection.activeGameObject = Model2.gameObject;
								EditorVariables.NewModelName = Model2.name;
								NName = Model2.gameObject.name;
							}
							// race
							if ( Model2 != null && Selection.activeGameObject == Model2.gameObject ) GUI.color = Color.yellow;
							else if ( Model2 != null && Model2.gameObject.activeInHierarchy == false ) GUI.color = Color.gray;
							else GUI.color = Color.white;
							if ( Model2 != null && GUILayout.Button(_DK_Model2.Race, Slim, GUILayout.Width (80)))
							{
								Selection.activeGameObject = Model2.gameObject;
							}
							// overlay
							if ( Model2 != null && Selection.activeGameObject == Model2.gameObject ) GUI.color = Color.yellow;
							else if ( Model2 != null && Model2.gameObject.activeInHierarchy == false ) GUI.color = Color.gray;
							else GUI.color = Color.white;
							if ( Model2 != null && Model2 != null && GUILayout.Button(_DK_Model2.Gender, Slim, GUILayout.Width (80)))
							{
								Selection.activeGameObject = Model2.gameObject;
							}
						}
					}
				}


				if ( EditorVariables.MFSelectedList != null ) foreach (Transform Model in EditorVariables.MFSelectedList.transform)
				{
					DK_Model _DK_Model = Model.gameObject.GetComponent< DK_Model >();
					if ( Model && _DK_Model 
					    && _DK_Model.IsUmaModel 
					    &&  ( SearchString == "" 
					     || Model.name.ToLower().Contains(SearchString.ToLower()) ) ) using (new Horizontal()) 
					{
						// Prefab
						// Prefab Verification
						if ( PrefabUtility.GetPrefabParent( Model.gameObject ) != null )
							myPath = PrefabUtility.GetPrefabParent( Model.gameObject ).ToString() ;
						else myPath = "null";
						if ( myPath == "null" ) GUI.color = Color.gray;
						else GUI.color = Color.cyan;
						if ( GUILayout.Button ( new GUIContent(_Prefab), "toolbarbutton", GUILayout.ExpandWidth (false))) {
							// Create Prefab
							if ( myPath == "null" ) 
							{
								PrefabUtility.CreatePrefab("Assets/DK Editors/DK_UMA_Editor/Prefabs/Models/" + Model.name + ".prefab", Model.gameObject,ReplacePrefabOptions.ConnectToPrefab );
							}
							else {
								Selection.activeGameObject = Model.gameObject;
								DK_UMA_Editor.OpenDeleteAsset();
								DeleteAsset.MultiUMAModel = false;
								DeleteAsset.UMAModel = true;
							}
							
						}
						GUI.color = Green;
						if ( GUILayout.Button ( new GUIContent(_GroupIt), "toolbarbutton", GUILayout.ExpandWidth (false))) {
							// Group to
							Model.parent = SelectedGrp;
						}
						// select
						try{ if (Model && UMAModel && Selection.activeGameObject 
							         && ( Selection.activeGameObject == Model.gameObject) 
							         || Model.gameObject == UMAModel ) GUI.color = Color.yellow;
							else if ( Model != null && Model.gameObject.activeInHierarchy == false ) GUI.color = Color.gray;
							else GUI.color = Color.white;
							if ( Model != null && GUILayout.Button(Model.name, "toolbarbutton", GUILayout.Width (165)))
							{
								UMAModel = Model.gameObject;
								EditorVariables.EditedModel = Model;
								Selection.activeGameObject = Model.gameObject;
								EditorVariables.NewModelName = Model.name;
							}
						}
						catch (MissingReferenceException ) {
							
						}
						// race
						if ( Model != null && Selection.activeGameObject == Model.gameObject ) GUI.color = Color.yellow;
						else if ( Model != null && Model.gameObject.activeInHierarchy == false ) GUI.color = Color.gray;
						else GUI.color = Color.white;
			       	    if ( Model != null && GUILayout.Button(_DK_Model.Race, Slim, GUILayout.Width (80)))
						{
							Selection.activeGameObject = Model.gameObject;
						}
						// overlay
						if ( Model != null && Selection.activeGameObject == Model.gameObject ) GUI.color = Color.yellow;
						else if ( Model != null && Model.gameObject.activeInHierarchy == false ) GUI.color = Color.gray;
						else GUI.color = Color.white;
			       	    if ( Model != null && Model != null && GUILayout.Button(_DK_Model.Gender, Slim, GUILayout.Width (80)))
						{
							Selection.activeGameObject = Model.gameObject;
						}
					}
		        }
			}
		}
		#endregion UMA Instances

		#region UMA Prefabs Elements
		if ( AllPrefabElements ) {
			GUI.color = Color.white ;
			using (new Horizontal()) {
				GUI.color = Color.yellow;
				GUILayout.Label ( "Race Library :", GUILayout.Width (110));
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
				GUI.color = Color.yellow;
				GUILayout.Label ( "Slot Library :", GUILayout.Width (110));
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
				GUI.color = Color.yellow;
				GUILayout.Label ( "Overlay Library :", GUILayout.Width (110));
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
			if ( Selection.activeObject != null
			    && ( Selection.activeObject.GetType().ToString() == "DKRaceData" 
			    || Selection.activeObject.GetType().ToString() == "DKSlotData" 
			    || Selection.activeObject.GetType().ToString() == "DKOverlayData" )  )
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("Actions :", GUILayout.ExpandWidth (false));
				GUI.color = Green;
				if ( GUILayout.Button("Duplicate", GUILayout.ExpandWidth (true))){
					DuplicateRace();

				}
				GUI.color = Color.yellow;
				if ( GUILayout.Button("Move To", GUILayout.ExpandWidth (true))){
					DK_UMA_Editor.OpenLibrariesWindow();
					ChangeLibrary.Action = "MoveTo";
					if ( Selection.activeObject.GetType().ToString() == "DKSlotData"  ){
						ChangeLibrary.CurrentLibN = EditorVariables.DK_UMACrowd.slotLibrary.name;
						ChangeLibrary.CurrentLibrary = EditorVariables.DK_UMACrowd.slotLibrary.gameObject;
					}
					if ( Selection.activeObject.GetType().ToString() == "DKOverlayData"  ){
						ChangeLibrary.CurrentLibN = EditorVariables.DK_UMACrowd.overlayLibrary.name;
						ChangeLibrary.CurrentLibrary = EditorVariables.DK_UMACrowd.overlayLibrary.gameObject;
					}
				}
				GUI.color = Red;
				if ( GUILayout.Button("Delete", GUILayout.ExpandWidth (true))){
					DeleteAsset.ProcessName = "Delete Asset";
					DK_UMA_Editor.OpenDeleteAsset();
					DeleteAsset.MultiUMAModel = false;
					DeleteAsset.UMAModel = false;
				}
				
			}
			GUILayout.Space(5);
			#region Search
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("Search for :", GUILayout.ExpandWidth (false));
				SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));
				if ( GUILayout.Button("Refresh", GUILayout.ExpandWidth (false))){
					BuildLocalAssetStore();
					BuildLocalAssetStoreOv();
					BuildLocalAssetStoreRa();
				}
			}
			#endregion Search
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("UMA Assets", "toolbarbutton", GUILayout.ExpandWidth (true));
				if ( displayRaces ) GUI.color = Color.white;
				else GUI.color = Color.gray;
				if ( GUILayout.Button("Races", "toolbarbutton", GUILayout.ExpandWidth (false))){
					DetectAndAddDK.DetectAll();
					if ( displayRaces ) displayRaces = false;
					else displayRaces = true;
				}
				if ( displaySlots ) GUI.color = Color.white;
				else GUI.color = Color.gray;
				if ( GUILayout.Button("Slots", "toolbarbutton", GUILayout.ExpandWidth (false))){
					DetectAndAddDK.DetectAll();
					if ( displaySlots ) displaySlots = false;
					else displaySlots = true;
				}
				if ( displayOverlays ) GUI.color = Color.white;
				else GUI.color = Color.gray;
				if ( GUILayout.Button("Overlays", "toolbarbutton", GUILayout.ExpandWidth (false))){
					DetectAndAddDK.DetectAll();
					if ( displayOverlays ) displayOverlays = false;
					else displayOverlays = true;
				}
				GUI.color = Green;
				if ( GUILayout.Button("Open Libraries", "toolbarbutton", GUILayout.ExpandWidth (false))){
					DK_UMA_Editor.OpenLibrariesWindow();
					ChangeLibrary.EditorMode = true;
					ChangeLibrary.Action = "Editor";
				}
			}

			#region List
			if ( displaySlots ) if(_assetStore.Count==0){
				BuildLocalAssetStore();
			}
			if ( displayOverlays ) if(_assetStoreOv.Count==0){
				BuildLocalAssetStoreOv();
			}
			if ( displayRaces ) if(_assetStoreRa.Count==0){
				BuildLocalAssetStoreRa();
			}
			GUI.color = Color.white;
			using (new ScrollView(ref scroll)) {
				if ( displayRaces ) {
					GUI.color = Color.cyan;
					GUILayout.Label("Races :", "toolbarbutton", GUILayout.ExpandWidth (true));
					GUI.color = Color.white;
					foreach (var tp in _assetStoreRa) {
						if ( !openRa.ContainsKey (tp.Key))
							openRa [tp.Key] = false;
						foreach (var n in tp.Value.OrderByDescending(q=>q.Value.Count).ThenBy(q=>q.Key)) {
							foreach (var i in n.Value.Cast<UnityEngine.Object>()) {
								try{
									if ( i.name.ToString ().ToLower().Contains(SearchString) ) {
										var addOn="";
										if(!AssetDatabase.IsMainAsset(i) && !AssetDatabase.IsSubAsset(i))
										{
											GUI.color = Color.red;
											addOn = " (internal to Unity)";
										}
										else if ( i.name.ToString ().ToLower().Contains(SearchString) ) {
												if ( EditorVariables.DK_UMACrowd && EditorVariables.DK_UMACrowd.raceLibrary
												 && EditorVariables.DK_UMACrowd.raceLibrary.raceElementList.Contains(i as DKRaceData)
											     && Selection.activeObject != i ) GUI.color = Color.white;
											else if (  Selection.activeObject == i ) GUI.color = Color.yellow;
											else GUI.color = Color.cyan;
											using (new Horizontal()){
												GUILayout.Space (20);
												if (EditorVariables.DK_UMACrowd && EditorVariables.DK_UMACrowd.raceLibrary
												  && EditorVariables.DK_UMACrowd.raceLibrary.raceElementList.Contains(i as DKRaceData)){}
												else 
												{
													if ( EditorVariables.DK_UMACrowd &&EditorVariables.DK_UMACrowd.raceLibrary 
													   && GUILayout.Button ( "Add to "+EditorVariables.DK_UMACrowd.raceLibrary.name, GUILayout.ExpandWidth (false)))
													{
														Selection.activeObject = i;
														EditorGUIUtility.PingObject(i);
														List<DKRaceData> tmpList = new List<DKRaceData>();
														tmpList = EditorVariables.DK_UMACrowd.raceLibrary.raceElementList.ToList();
														tmpList.Add(( Selection.activeObject as DKRaceData));
														EditorVariables.DK_UMACrowd.raceLibrary.raceElementList = tmpList.ToArray();
														EditorUtility.SetDirty(EditorVariables.DK_UMACrowd.raceLibrary);
														AssetDatabase.SaveAssets();
													}
												}
												if (GUILayout.Button (i.name.ToString () + addOn, "label",GUILayout.ExpandWidth (true))) {
													Selection.activeObject = i;
													EditorGUIUtility.PingObject(i);
												}
											}

											GUI.color = Color.white;
											if ( Selection.activeObject && Selection.activeObject == i ) {
												GameObject UMA = GameObject.Find("UMA");
												foreach (Transform Lib in UMA.transform)
												{
													DKRaceLibrary RaceLibrary = Lib.gameObject.GetComponent<DKRaceLibrary>();
													if ( RaceLibrary && RaceLibrary.raceElementList.Contains( Selection.activeObject as DKRaceData) ){
														using (new Horizontal()) {
															GUILayout.Space (40);
															GUI.color = Color.white;
															GUILayout.TextField ( RaceLibrary.name, GUILayout.Width (160));
															if (GUILayout.Button ( "Remove", GUILayout.ExpandWidth (false))) {
																List<DKRaceData> tmpList = new List<DKRaceData>();
																tmpList = RaceLibrary.raceElementList.ToList();
																tmpList.Remove(( Selection.activeObject as DKRaceData));
																RaceLibrary.raceElementList = tmpList.ToArray();
															}
														}
													}
												}
											}										
										}
									}
								}catch (MissingReferenceException){}
							}
						}
					}
				}
				if ( displaySlots ) {
					GUI.color = Color.cyan;
					GUILayout.Label("Slots :", "toolbarbutton", GUILayout.ExpandWidth (true));
					GUI.color = Color.white;
					foreach (var tp in _assetStore) {
						if ( !open.ContainsKey (tp.Key))
							open [tp.Key] = false;
						foreach (var n in tp.Value.OrderByDescending(q=>q.Value.Count).ThenBy(q=>q.Key)) {
							foreach (var i in n.Value.Cast<UnityEngine.Object>()) {
								try{ 
									if ( i.name.ToString ().ToLower().Contains(SearchString) ){
										var addOn="";
										if(!AssetDatabase.IsMainAsset(i) && !AssetDatabase.IsSubAsset(i))
										{
											GUI.color = Color.red;
											addOn = " (internal to Unity)";
										}
										else if ( i.name.ToString ().ToLower().Contains(SearchString) ) {
											try{ if ( EditorVariables.DK_UMACrowd.slotLibrary.slotElementList.Contains(i as DKSlotData)
												         && Selection.activeObject != i ) GUI.color = Color.white;
												else if ( Selection.activeObject && Selection.activeObject == i ) GUI.color = Color.yellow;
												else GUI.color = Color.cyan;
												using (new Horizontal()) {
													GUILayout.Space (20);
													if (EditorVariables.DK_UMACrowd.slotLibrary.slotElementList.Contains(i as DKSlotData)){}
													else 
													{
														if ( GUILayout.Button ( "Add to "+EditorVariables.DK_UMACrowd.slotLibrary.name, GUILayout.ExpandWidth (false)))
														{
															Selection.activeObject = i;
															EditorGUIUtility.PingObject(i);
															List<DKSlotData> tmpList = new List<DKSlotData>();
															tmpList = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList.ToList();
															tmpList.Add(( Selection.activeObject as DKSlotData));
															EditorVariables.DK_UMACrowd.slotLibrary.slotElementList = tmpList.ToArray();
														}
													}
													if (GUILayout.Button (i.name.ToString () + addOn, "label",GUILayout.ExpandWidth (true))) {
														Selection.activeObject = i as DKSlotData;
														EditorGUIUtility.PingObject(i);
													}
												}
											}catch (NullReferenceException ) {}
											GUI.color = Color.white;
											if ( Selection.activeObject && Selection.activeObject == i ) {
												GameObject UMA = GameObject.Find("UMA");
												foreach (Transform Lib in UMA.transform)
												{
													DKSlotLibrary DKSlotLibrary = Lib.gameObject.GetComponent<DKSlotLibrary>();
													DKOverlayLibrary OverlayLibrary = Lib.gameObject.GetComponent<DKOverlayLibrary>();
													if ( DKSlotLibrary && DKSlotLibrary.slotElementList.Contains( Selection.activeObject as DKSlotData) ){
														using (new Horizontal()) {
															GUILayout.Space (40);
															GUI.color = Color.white;
															GUILayout.TextField ( DKSlotLibrary.name, GUILayout.Width (160));
															if (GUILayout.Button ( "Remove", GUILayout.ExpandWidth (false))) {
																List<DKSlotData> tmpList = new List<DKSlotData>();
																tmpList = DKSlotLibrary.slotElementList.ToList();
																tmpList.Remove(( Selection.activeObject as DKSlotData));
																DKSlotLibrary.slotElementList = tmpList.ToArray();
															}
														}
													}
												}
											}	
										}
									}
								}
								catch (MissingReferenceException ) {}
							}
						}
					}
				}
				if ( displayOverlays ) {
					GUI.color = Color.cyan;
					GUILayout.Label("Overlay :", "toolbarbutton", GUILayout.ExpandWidth (true));
					GUI.color = Color.white;
					foreach (var tp in _assetStoreOv) {
						if ( !openOv.ContainsKey (tp.Key))
							openOv [tp.Key] = false;
						foreach (var n in tp.Value.OrderByDescending(q=>q.Value.Count).ThenBy(q=>q.Key)) {
							foreach (var i in n.Value.Cast<UnityEngine.Object>()) {
								try{
									if ( i.name.ToString ().ToLower().Contains(SearchString) ) {
										var addOn="";
										if(!AssetDatabase.IsMainAsset(i) && !AssetDatabase.IsSubAsset(i))
										{
											GUI.color = Color.red;
											addOn = " (internal to Unity)";
										}
										else if ( i.name.ToString ().ToLower().Contains(SearchString) ) {
											try{
												if ( EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList.Contains(i as DKOverlayData)
												    && Selection.activeObject != i ) GUI.color = Color.white;
												else if ( Selection.activeObject && Selection.activeObject == i ) GUI.color = Color.yellow;
												else GUI.color = Color.cyan;
												using (new Horizontal()) {
													GUILayout.Space (20);
													if (EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList.Contains(i as DKOverlayData)){}
													else 
													{
														if ( GUILayout.Button ( "Add to "+EditorVariables.DK_UMACrowd.overlayLibrary.name, GUILayout.ExpandWidth (false)))
														{
															Selection.activeObject = i;
															EditorGUIUtility.PingObject(i);
															List<DKOverlayData> tmpList = new List<DKOverlayData>();
															tmpList = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList.ToList();
															tmpList.Add(( Selection.activeObject as DKOverlayData));
															EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList = tmpList.ToArray();
														}
													}
													if (GUILayout.Button (i.name.ToString () + addOn, "label",GUILayout.ExpandWidth (true))) {
														Selection.activeObject = i;
														EditorGUIUtility.PingObject(i);
													}
												}
												GUI.color = Color.white;
												if ( Selection.activeObject && Selection.activeObject == i ) {
													GameObject UMA = GameObject.Find("UMA");
													foreach (Transform Lib in UMA.transform)
													{
														DKOverlayLibrary OverlayLibrary = Lib.gameObject.GetComponent<DKOverlayLibrary>();

														if ( OverlayLibrary && OverlayLibrary.overlayElementList.Contains( Selection.activeObject as DKOverlayData) ){
															using (new Horizontal()) {
																GUILayout.Space (40);
																GUI.color = Color.white;
																GUILayout.TextField ( OverlayLibrary.name, GUILayout.Width (130));
																if (GUILayout.Button ( "Remove", GUILayout.ExpandWidth (false))) {
																	List<DKOverlayData> tmpList = new List<DKOverlayData>();
																	tmpList = OverlayLibrary.overlayElementList.ToList();
																	tmpList.Remove(( Selection.activeObject as DKOverlayData));
																	OverlayLibrary.overlayElementList = tmpList.ToArray();
																}
															}
														}
													}
												}
											}catch (NullReferenceException ) {}
										}
									}
								}catch (MissingReferenceException ) {}
							}
						}
					}
				}
			}
		}
		#endregion 
		#endregion UMA Prefabs Elements

		#region UMA Prefabs Models
		if ( AllPrefabUMAModels) {
			DirectoryInfo dir_Group = new DirectoryInfo("Assets/DK Editors/DK_UMA_Editor/Prefabs/Models/Groups/");
			DirectoryInfo dir_Unit = new DirectoryInfo("Assets/DK Editors/DK_UMA_Editor/Prefabs/Models/");
			FileInfo[] info_Group = dir_Group.GetFiles("*.prefab");
			FileInfo[] info_Unit = dir_Unit.GetFiles("*.prefab");
			using (new Horizontal()) {
				GUI.color = Green;
				GUILayout.Label("ZPoint :", GUILayout.ExpandWidth (true));
				if ( GUILayout.Button("Move to", GUILayout.ExpandWidth (true))){
					GameObject ZPoint = GameObject.Find("ZeroPoint");
					Selection.activeGameObject.transform.position =  ZPoint.transform.position;
				}
				if ( SpawnAtZP ) GUI.color = Color.yellow;
				else GUI.color = Color.gray;
				if ( GUILayout.Button("Spawn at", GUILayout.ExpandWidth (true))){
					if ( SpawnAtZP ) SpawnAtZP = false;
					else SpawnAtZP = true;
				}
			}
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("All :", GUILayout.ExpandWidth (true));
				if ( GUILayout.Button("Open", GUILayout.ExpandWidth (true))){
					OpList.Clear();
					OpGrpList.Clear();
					List<GameObject> _TmpList = new List<GameObject>();
					foreach (FileInfo f in info_Group)
					{
						OpGrpList.Add(f.Name);
						ModelGrp[]  goList = FindObjectsOfType(typeof(ModelGrp)) as ModelGrp[];
						
						for(int i = 0; i < goList.Length; i ++){
							string  tmp_Group = f.Name.Replace( ".prefab", "");
							if ( PrefabUtility.GetPrefabParent(goList[i]) != null && PrefabUtility.GetPrefabParent(goList[i].gameObject).name == tmp_Group ){
								_TmpList.Add(goList[i].gameObject);
							}
						}
						for(int i = 0; i < _TmpList.Count; i ++){
							_TmpList[i].GetComponent<ModelGrp>().Show = true;
						}
					}
					foreach (FileInfo f in info_Unit)
					{
						OpList.Add(f.Name);
					}
				}
				if ( GUILayout.Button("Close", GUILayout.ExpandWidth (true))){
					OpList.Clear();
					OpGrpList.Clear();
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
			using (new ScrollView(ref scroll)) 
			{
				using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label ( "Groups :", "toolbarbutton", GUILayout.Width (110));
					GUILayout.Label ( "", "toolbarbutton", GUILayout.ExpandWidth (true));
				}

				if ( tmpGrp && tmpGrp.name.Contains("(Clone)") == true ) tmpGrp.name.Replace("(Clone)", "");
				foreach (FileInfo f in info_Group)
				{
					List<GameObject> _TmpList = new List<GameObject>();

					using (new Horizontal()) {
						GUI.color = Color.cyan;
						if ( GUILayout.Button ( new GUIContent(_Inst), GUILayout.ExpandWidth (false))){
							myPath = ("Assets/DK Editors/DK_UMA_Editor/Prefabs/Models/Groups/"+f.Name.ToString());
							GameObject NewGrp =  PrefabUtility.InstantiatePrefab(Resources.LoadAssetAtPath(myPath , typeof(GameObject))  ) as GameObject;
							GameObject ZPoint = GameObject.Find("ZeroPoint");
							if ( SpawnAtZP ) NewGrp.transform.position =  ZPoint.transform.position;
							PrefabUtility.ReconnectToLastPrefab(NewGrp);
						}
						string  tmpN2 = f.Name.ToString();
						string  toRepl2 = ".prefab";
						tmpN2 = tmpN2.Replace(toRepl2, "");
						GameObject FindObj = GameObject.Find(tmpN2);
						if ( FindObj != null &&  OpGrpList.Contains(f.Name) == false ) GUI.color = Color.gray;
						else if ( OpGrpList.Contains(f.Name) == true ) GUI.color = Color.cyan;
						else GUI.color = Color.white;
						if ( GUILayout.Button ( f.Name, "foldout", GUILayout.ExpandWidth (true))){
							if ( OpGrpList.Contains(f.Name) == false) {
								_Group = f.Name;
								OpGrpList.Add(f.Name);

							}
							else {
								_Group = "";
								OpGrpList.Remove(f.Name);

							}
						}
						if (  OpGrpList.Contains(f.Name) == true) {
							ModelGrp[]  goList = FindObjectsOfType(typeof(ModelGrp)) as ModelGrp[];
							
							for(int i = 0; i < goList.Length; i ++){
								string  tmp_Group = f.Name.Replace( ".prefab", "");
								if ( PrefabUtility.GetPrefabParent(goList[i]) != null && PrefabUtility.GetPrefabParent(goList[i].gameObject).name == tmp_Group ){
									_TmpList.Add(goList[i].gameObject);
								}
							}
							GUILayout.Label (_TmpList.Count.ToString(), GUILayout.ExpandWidth (false));
							GUILayout.Label ("/"+goList.Length.ToString(), GUILayout.ExpandWidth (false));
							GUI.color = Red;
						}
					}
					if ( OpGrpList.Contains(f.Name)  == true
						) {
						for(int i = 0; i < _TmpList.Count; i ++){
							using (new Horizontal()) {
								GUILayout.Space(20);
								GUI.color = Red;
								try{
									if ( _TmpList[i].GetComponent<ModelGrp>().Show == true  ) GUI.color = Color.yellow;
									else GUI.color = Color.white;
									 if ( GUILayout.Button ( _TmpList[i].name , "foldout", GUILayout.ExpandWidth (true))){
										if ( _TmpList[i].GetComponent<ModelGrp>().Show == true ){ 
											_TmpList[i].GetComponent<ModelGrp>().Show = false;
											Selection.activeGameObject = null;
										}
										else {
											_TmpList[i].GetComponent<ModelGrp>().Show = true;
											Selection.activeGameObject = _TmpList[i];
										}
									}
								}catch (MissingReferenceException ) {}
							}
							try{
								if ( _TmpList[i].GetComponent<ModelGrp>().Show == true) 
								foreach (Transform child in _TmpList[i].transform)
								{
									if ( child.name.ToLower().Contains(SearchString.ToLower()))
									using (new Horizontal()) {
										GUILayout.Space(40);
										GUI.color = Red;
										if ( Selection.activeGameObject && Selection.activeGameObject == child.gameObject ) GUI.color = Color.yellow;
										else GUI.color = Color.white;
										if ( GUILayout.Button ( child.name , Slim, GUILayout.ExpandWidth (true))){
											Selection.activeGameObject = child.gameObject;
										}
									}
								}
							}catch (MissingReferenceException ) {}
						}
					}
				}

				using (new Horizontal()) {
					GUI.color = Color.white;
					GUILayout.Label ( "Units :", "toolbarbutton", GUILayout.Width (110));
					GUILayout.Label ( "", "toolbarbutton", GUILayout.ExpandWidth (true));
				}
				foreach (FileInfo f in info_Unit)
				{
					List<GameObject> TmpList2 = new List<GameObject>();
					if ( f.Name.ToLower().Contains(SearchString.ToLower())) using (new Horizontal()) {
						GUI.color = Color.cyan;
						if ( GUILayout.Button ( new GUIContent(_Inst), GUILayout.ExpandWidth (false))){
							myPath = ("Assets/DK Editors/DK_UMA_Editor/Prefabs/Models/"+f.Name.ToString());
							GameObject NewGrp =  PrefabUtility.InstantiatePrefab(Resources.LoadAssetAtPath(myPath , typeof(GameObject))  ) as GameObject;
							GameObject ZPoint = GameObject.Find("ZeroPoint");
							if ( SpawnAtZP ) NewGrp.transform.position =  ZPoint.transform.position;
							PrefabUtility.ReconnectToLastPrefab(NewGrp);
							NewGrp.GetComponentInChildren<DKUMASaveTool>().AutoLoadSelf();
						}
						string  tmpN2 = f.Name.ToString();
						string  toRepl2 = ".prefab";
						tmpN2 = tmpN2.Replace(toRepl2, "");
						GameObject FindObj = GameObject.Find(tmpN2);
						if ( FindObj &&  OpList.Contains(f.Name) == false ) GUI.color = Color.gray;
						else if ( OpList.Contains(f.Name) == true ) GUI.color = Color.cyan;
						else GUI.color = Color.white;
						if ( GUILayout.Button ( f.Name, "foldout", GUILayout.ExpandWidth (true))){
							if ( OpList.Contains(f.Name) == false ) {
								OpList.Add(f.Name);
							}
							else {
								OpList.Remove(f.Name);
							}
						}
							if ( OpList.Contains(f.Name)  == true ) {
							DK_Model[]  goList = FindObjectsOfType(typeof(DK_Model)) as DK_Model[];
							for(int i = 0; i < goList.Length; i ++){
								string  tmp_Group = f.Name.Replace( ".prefab", "");
								if ( PrefabUtility.GetPrefabParent(goList[i]) != null && PrefabUtility.GetPrefabParent(goList[i].gameObject).name == tmp_Group ){
									TmpList2.Add(goList[i].gameObject);
								}
							}
							GUILayout.Label (TmpList2.Count.ToString(), GUILayout.ExpandWidth (false));
							GUILayout.Label ("/"+goList.Length.ToString(), GUILayout.ExpandWidth (false));
							GUI.color = Red;
						}
					}
						if ( OpList.Contains(f.Name)  == true ) for(int i = 0; i < TmpList2.Count; i ++){
							TmpList2[i].GetComponent<DK_Model>().Show = true;
							if (  TmpList2[i].GetComponent<DK_Model>().Show == true
							&& TmpList2[i].name.ToLower().Contains(SearchString.ToLower()))
							using (new Horizontal()) {
							GUILayout.Space(20);
							GUI.color = Red;
							if ( Selection.activeGameObject &&Selection.activeGameObject == TmpList2[i] ) GUI.color = Color.yellow;
							else GUI.color = Color.white;
							try{
							if ( GUILayout.Button ( TmpList2[i].name , Slim, GUILayout.ExpandWidth (true))){
								if ( Selection.activeGameObject && Selection.activeGameObject == TmpList2[i] ) 
									Selection.activeGameObject = null;
								else Selection.activeGameObject = TmpList2[i];
							}
							}catch(MissingReferenceException){}
						}
					}
				}
			}
		}
		#endregion UMA Prefabs Models

		#region UMA Elements
		if ( AllElements == true ) {
			GUI.color = Color.white ;
			using (new Horizontal()) {
				GUI.color = Color.yellow;
				GUILayout.Label ( "Race Library :", GUILayout.Width (110));
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
				GUI.color = Color.yellow;
				GUILayout.Label ( "Slot Library :", GUILayout.Width (110));
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
				GUI.color = Color.yellow;
				GUILayout.Label ( "Overlay Library :", GUILayout.Width (110));
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
			if ( Selection.activeObject != null
			    && ( Selection.activeObject.GetType().ToString() == "DKRaceData" 
			    || Selection.activeObject.GetType().ToString() == "DKSlotData" 
			    || Selection.activeObject.GetType().ToString() == "DKOverlayData" )  )
			    using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("Actions :", GUILayout.ExpandWidth (false));
				GUI.color = Green;
				if ( GUILayout.Button("Duplicate", GUILayout.ExpandWidth (true))){
					DuplicateRace();
				}
				GUI.color = Color.yellow;
				if ( GUILayout.Button("Move To", GUILayout.ExpandWidth (true))){
					DK_UMA_Editor.OpenLibrariesWindow();
					ChangeLibrary.Action = "MoveTo";
					if ( Selection.activeObject.GetType().ToString() == "DKSlotData"  ){
						ChangeLibrary.CurrentLibN = EditorVariables.DK_UMACrowd.slotLibrary.name;
						ChangeLibrary.CurrentLibrary = EditorVariables.DK_UMACrowd.slotLibrary.gameObject;
					}
					if ( Selection.activeObject.GetType().ToString() == "DKOverlayData"  ){
						ChangeLibrary.CurrentLibN = EditorVariables.DK_UMACrowd.overlayLibrary.name;
						ChangeLibrary.CurrentLibrary = EditorVariables.DK_UMACrowd.overlayLibrary.gameObject;
					}
				}
				GUI.color = Red;
				if ( GUILayout.Button("Delete", GUILayout.ExpandWidth (true))){
					DK_UMA_Editor.OpenDeleteAsset();
					DeleteAsset.ProcessName = "Delete from Library";
					DeleteAsset.MultiUMAModel = false;
					DeleteAsset.UMAModel = false;
				}
			}
			GUILayout.Space(5);
			#region Search
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("Search for :", GUILayout.ExpandWidth (false));
				SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));
			}

			#endregion Search
			GUILayout.Space(2);
			using (new Horizontal()) {
				GUI.color = Color.white;
				GUILayout.Label("UMA Elements", "toolbarbutton", GUILayout.ExpandWidth (true));
				if ( displayRaces ) GUI.color = Color.white;
				else GUI.color = Color.gray;
				if ( GUILayout.Button("Races", "toolbarbutton", GUILayout.ExpandWidth (false))){
					DetectAndAddDK.DetectAll();
					if ( displayRaces ) displayRaces = false;
					else displayRaces = true;
				}
				if ( displaySlots ) GUI.color = Color.white;
				else GUI.color = Color.gray;
				if ( GUILayout.Button("Slots", "toolbarbutton", GUILayout.ExpandWidth (false))){
					DetectAndAddDK.DetectAll();
					if ( displaySlots ) displaySlots = false;
					else displaySlots = true;
				}
				if ( displayOverlays ) GUI.color = Color.white;
				else GUI.color = Color.gray;
				if ( GUILayout.Button("Overlays", "toolbarbutton", GUILayout.ExpandWidth (false))){
					DetectAndAddDK.DetectAll();
					if ( displayOverlays ) displayOverlays = false;
					else displayOverlays = true;
				}
				GUI.color = Green;
				if ( GUILayout.Button("Open Libraries", "toolbarbutton", GUILayout.ExpandWidth (false))){
					DK_UMA_Editor.OpenLibrariesWindow();
					ChangeLibrary.EditorMode = true;
					ChangeLibrary.Action = "Editor";
				}
			}

			#region List
			GUI.color = Color.cyan ;
			using (new Horizontal()) {
				GUILayout.Label("Elements", "toolbarbutton", GUILayout.Width (160));
				GUILayout.Label("Race", "toolbarbutton", GUILayout.Width (50));
				GUILayout.Label("Gender", "toolbarbutton", GUILayout.Width (50));
				GUILayout.Label("Place", "toolbarbutton", GUILayout.Width (50));
				GUILayout.Label("Type", "toolbarbutton", GUILayout.Width (50));
				GUILayout.Label("Weight", "toolbarbutton", GUILayout.Width (50));
				GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
			}
			if ( EditorVariables._RaceLibrary ) using (new ScrollView(ref scroll)) 
			{
			
				#region Races
				if ( displayRaces ) using (new Horizontal()) {	
					GUI.color = Color.yellow ;
					GUILayout.Label ( "Races Library = "+EditorVariables._RaceLibrary.name+" ("+EditorVariables.DK_UMACrowd.raceLibrary.raceElementList.Length.ToString()+")", "toolbarbutton", GUILayout.Width (240));
					GUILayout.Label ( "", "toolbarbutton", GUILayout.ExpandWidth (true));
				}
				if ( displayRaces ) for(int i = 0; i < EditorVariables._RaceLibrary.raceElementList.Length; i ++){
					if ( ( SearchString == "" || EditorVariables._RaceLibrary.raceElementList[i].name.ToLower().Contains(SearchString.ToLower()) ) 
					&& EditorVariables._RaceLibrary.raceElementList[i] != null ) 
					using (new Horizontal()) {

						DKRaceData _DK_Race = EditorVariables._RaceLibrary.raceElementList[i];
						if ( _DK_Race.Active == true ) GUI.color = Green;
						else GUI.color = Color.gray ;
						
						if (GUILayout.Button ( "U", "toolbarbutton", GUILayout.Width (20))){
							if ( _DK_Race.Active == true ) _DK_Race.Active = false;
							else _DK_Race.Active = true;
							AssetDatabase.SaveAssets();
						} 

						if ( Selection.activeObject == EditorVariables._RaceLibrary.raceElementList[i] ) GUI.color = Color.yellow ;
						else GUI.color = Color.white ;
						if (EditorVariables._RaceLibrary.raceElementList[i] != null 
						    && GUILayout.Button ( EditorVariables._RaceLibrary.raceElementList[i].raceName , "toolbarbutton", GUILayout.Width (140))) {
							Selection.activeObject = EditorVariables._RaceLibrary.raceElementList[i];
							EditorVariables.SelectedElementName = (Selection.activeObject as DKRaceData).raceName;
							EditorVariables.SelectedElementGender = _DK_Race.Gender;
							EditorVariables.SelectedElementType = "RaceElement";
							EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
						}
						// Race
						if ( _DK_Race.Race == "" ) GUI.color = Red;
						if ( _DK_Race.Race == "" && GUILayout.Button ( "No Race" , Slim, GUILayout.Width (50))) {
							
						}
						if ( _DK_Race.Race != "" ) GUI.color = Red;
					
						GUI.color = Green;
						if ( _DK_Race.Race != "" && GUILayout.Button ( _DK_Race.Race , Slim, GUILayout.Width (50))) {
							
						}
						// Gender
						if ( _DK_Race.Gender == "" ) GUI.color = Red;
						if ( _DK_Race.Gender == "" && GUILayout.Button ( "No Gender" , Slim, GUILayout.Width (50))) {
							
						}
						if ( _DK_Race.Gender != "" ) GUI.color = Red;
					
						GUI.color = Green;
						if ( _DK_Race.Gender != "" && GUILayout.Button ( _DK_Race.Gender , Slim, GUILayout.Width (50))) {
							
						}
						
						
					}
				}
				#endregion
				
				#region Slots
				if ( displaySlots 
				    && EditorVariables.DK_UMACrowd
				    && EditorVariables.DK_UMACrowd.slotLibrary ) using (new Horizontal()) {	
					GUI.color = Color.yellow ;
					GUILayout.Label ( "Slots Library = "+EditorVariables.DK_UMACrowd.slotLibrary.name+" ("+EditorVariables.DK_UMACrowd.slotLibrary.slotElementList.Length.ToString()+")", "toolbarbutton", GUILayout.Width (240));
					GUILayout.Label ( "", "toolbarbutton", GUILayout.ExpandWidth (true));
				}
				if ( displaySlots
				    && EditorVariables.DK_UMACrowd
				    && EditorVariables.DK_UMACrowd.slotLibrary) if ( EditorVariables.DK_UMACrowd == null ) DetectAndAddDK.DetectAll();
				if ( displaySlots
				    && EditorVariables.DK_UMACrowd
				    && EditorVariables.DK_UMACrowd.slotLibrary ) for(int i = 0; i < EditorVariables.DK_UMACrowd.slotLibrary.slotElementList.Length; i ++){
					if (EditorVariables.DK_UMACrowd 
					    && EditorVariables.DK_UMACrowd.slotLibrary
					    && EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i]
					    && ( SearchString == "" || EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i].name.ToLower().Contains(SearchString.ToLower()) ) && EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i] != null )  using (new Horizontal()) {
						DKSlotData _DK_Race = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
						if ( _DK_Race.Active == true ) GUI.color = Green;
						else GUI.color = Color.gray ;
						
						if (GUILayout.Button ( "U", "toolbarbutton",  GUILayout.Width (20))){
							if ( _DK_Race.Active == true ) _DK_Race.Active = false;
							else _DK_Race.Active = true;
							AssetDatabase.SaveAssets();
						} 
						if ( Selection.activeObject == EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i] ) GUI.color = Color.yellow ;
						else GUI.color = Color.white ;
						if (GUILayout.Button ( EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i].slotName, "toolbarbutton", GUILayout.Width (140))) {
							Selection.activeObject = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
							EditorVariables.SelectedElementName = (Selection.activeObject as DKSlotData).slotName;
							EditorVariables.SelectedElementGender = _DK_Race.Gender;
							EditorVariables.SelectedElementType = "SlotElement";
							EditorVariables.overlayList = _DK_Race.overlayList;
							EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
							EditorVariables.SelectedElementOverlayType = _DK_Race.OverlayType;
							EditorVariables.Replace = _DK_Race.Replace;
							if ( _DK_Race.Place != null ) EditorVariables.SelectedElemPlace = _DK_Race.Place;
						}
						// Race
						_DK_Race = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
						if ( _DK_Race.Race.Count == 0 ) GUI.color = Red;
						if ( _DK_Race.Race.Count == 0 && GUILayout.Button ( "No Race" , Slim, GUILayout.Width (50))) {
							
						}
						if ( _DK_Race.Race.Count != 0 ) GUI.color = Red;
					
						GUI.color = Green;
						
						if ( _DK_Race.Race.Count == 1 && GUILayout.Button ( _DK_Race.Race[0] , Slim, GUILayout.Width (50))) {
							
						}
						if ( _DK_Race.Race.Count > 1 && GUILayout.Button ( "Multi" , Slim, GUILayout.Width (50))) {
							
						}
						// Gender
						if ( _DK_Race.Gender == "" ) GUI.color = Red;
						if ( _DK_Race.Gender == "" && GUILayout.Button ( "No Gender" , Slim, GUILayout.Width (50))) {
							Selection.activeObject = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
							EditorVariables.SelectedElementName = (Selection.activeObject as DKSlotData).slotName;
							EditorVariables.SelectedElementGender = _DK_Race.Gender;
							EditorVariables.overlayList = _DK_Race.overlayList;
							EditorVariables.SelectedElementType = "SlotElement";
							EditorVariables.SelectedElementObj = Selection.activeGameObject;
							EditorVariables.SelectedElementOverlayType = _DK_Race.OverlayType;
							EditorVariables.Replace = _DK_Race.Replace;
							if ( _DK_Race.Place != null ) EditorVariables.SelectedElemPlace = _DK_Race.Place;
						}
						if ( _DK_Race.Gender != "" ) GUI.color = Red;
					
						GUI.color = Green;
						if ( _DK_Race.Gender != "" && GUILayout.Button ( _DK_Race.Gender , Slim, GUILayout.Width (50))) {
							Selection.activeObject = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
							EditorVariables.SelectedElementName = (Selection.activeObject as DKSlotData).slotName;
							EditorVariables.SelectedElementGender = _DK_Race.Gender;
							EditorVariables.overlayList = _DK_Race.overlayList;
							EditorVariables.SelectedElementType = "SlotElement";
							EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
							EditorVariables.SelectedElementOverlayType = _DK_Race.OverlayType;
							EditorVariables.Replace = _DK_Race.Replace;
							if ( _DK_Race.Place != null ) EditorVariables.SelectedElemPlace = _DK_Race.Place;
						}
						// Place
						if ( _DK_Race.Place == null ) GUI.color = Red;
						if ( _DK_Race.Place == null && GUILayout.Button ( "No Place" , Slim, GUILayout.Width (50))) {
							Selection.activeObject = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
							EditorVariables.SelectedElementName = (Selection.activeObject as DKSlotData).slotName;
							EditorVariables.SelectedElementGender = _DK_Race.Gender;
							EditorVariables.overlayList = _DK_Race.overlayList;
							EditorVariables.SelectedElementType = "SlotElement";
							EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
							EditorVariables.SelectedElementOverlayType = _DK_Race.OverlayType;
							EditorVariables.Replace = _DK_Race.Replace;
							if ( _DK_Race.Place != null ) EditorVariables.SelectedElemPlace = _DK_Race.Place;
						}
					
						GUI.color = Green;
						if ( _DK_Race.Place != null && GUILayout.Button ( _DK_Race.Place.name , Slim, GUILayout.Width (50))) {
							Selection.activeObject = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList[i];
							EditorVariables.SelectedElementName = (Selection.activeObject as DKSlotData).slotName;
							EditorVariables.SelectedElementGender = _DK_Race.Gender;
							EditorVariables.overlayList = _DK_Race.overlayList;
							EditorVariables.SelectedElementType = "SlotElement";
							EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
							EditorVariables.SelectedElementOverlayType = _DK_Race.OverlayType;
							EditorVariables.Replace = _DK_Race.Replace;
							if ( _DK_Race.Place != null ) EditorVariables.SelectedElemPlace = _DK_Race.Place;
						}
						if (  _DK_Race.OverlayType == "" ) 
						{
							GUI.color = Color.yellow ;
							GUILayout.Label ( "No Type");
						}
						else
						{
							GUI.color = Color.white ;
							GUILayout.Label( _DK_Race.OverlayType , Slim, GUILayout.Width (50));
						}
						// WearWeight
						GUI.color =  Color.gray;
						if (  _DK_Race.WearWeight == "" ) 
						{

						}
						else
						{
							GUI.color = Color.white ;
							GUILayout.Label( _DK_Race.WearWeight , Slim, GUILayout.Width (50));
						}
					}
				}
				#endregion
				#region Overlays
				if ( displayOverlays && EditorVariables.DK_UMACrowd
				    && EditorVariables.DK_UMACrowd.slotLibrary) using (new Horizontal()) {	
					GUI.color = Color.yellow ;
					GUILayout.Label ( "Overlays Library = "+EditorVariables.DK_UMACrowd.overlayLibrary.name+" ("+EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList.Length.ToString()+")", "toolbarbutton", GUILayout.Width (240));
					GUILayout.Label ( "", "toolbarbutton", GUILayout.ExpandWidth (true));
				}
				if ( displayOverlays && EditorVariables.DK_UMACrowd
				    && EditorVariables.DK_UMACrowd.slotLibrary) for(int i = 0; i < EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList.Length; i ++){
					if (EditorVariables.DK_UMACrowd 
					    && EditorVariables.DK_UMACrowd.overlayLibrary
					    && EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i]
						&& ( SearchString == "" || EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i].name.ToLower().Contains(SearchString.ToLower()) ) 
					    && EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i] != null ) 
					using (new Horizontal()) {
						DKOverlayData _DK_Race = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i];
						if ( _DK_Race.Active == true ) GUI.color = Green;
						else GUI.color = Color.gray ;
						if (GUILayout.Button ( "U", "toolbarbutton",  GUILayout.Width (20))){
							if ( _DK_Race.Active == true ) _DK_Race.Active = false;
							else _DK_Race.Active = true;
							AssetDatabase.SaveAssets();
						} 
						
						if ( Selection.activeObject == EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i] ) GUI.color = Color.yellow ;
						else GUI.color = Color.white ;
						if (GUILayout.Button ( EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i].overlayName, "toolbarbutton", GUILayout.Width (140))) {
							Selection.activeObject = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i];
							EditorVariables.SelectedElementName = (Selection.activeObject as DKOverlayData).overlayName;
							EditorVariables.SelectedElementGender = _DK_Race.Gender;
							EditorVariables.SelectedElementWearWeight = _DK_Race.WearWeight;
							if ( _DK_Race.Race.Count == 0 ){
								EditorVariables.SelectedElementRace = "No Race";
							}
							if ( _DK_Race.Race.Count == 1 ){
								EditorVariables.SelectedElementRace = _DK_Race.Race[0];
							}
							if ( _DK_Race.Race.Count > 1 ){
								EditorVariables.SelectedElementRace = "Multi";
							}
							EditorVariables.SelectedElementType = "OverlayElement";
							EditorVariables.SelectedElementOverlayType = _DK_Race.OverlayType;
							EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
							if ( _DK_Race.Place != null ) EditorVariables.SelectedElemPlace = _DK_Race.Place;
							
						}
						// Race
						DKOverlayData DK_Race;
						DK_Race = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i];
						if ( DK_Race.Race.Count == 0 ) GUI.color = Red;
						if ( DK_Race.Race.Count == 0 && GUILayout.Button ( "No Race" , Slim, GUILayout.Width (50))) {
							Selection.activeObject = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i];
							EditorVariables.SelectedElementName = (Selection.activeObject as DKOverlayData).overlayName;
							EditorVariables.SelectedElementGender = _DK_Race.Gender;
							if ( _DK_Race.Race.Count == 0 ){
								EditorVariables.SelectedElementRace = "No Race";
							}
							if ( _DK_Race.Race.Count == 1 ){
								EditorVariables.SelectedElementRace = _DK_Race.Race[0];
							}
							if ( _DK_Race.Race.Count > 1 ){
								EditorVariables.SelectedElementRace = "Multi";
							}
							EditorVariables.SelectedElementType = "OverlayElement";
							EditorVariables.SelectedElementOverlayType = _DK_Race.OverlayType;
							EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
							if ( _DK_Race.Place != null ) EditorVariables.SelectedElemPlace = _DK_Race.Place;
						}
						GUI.color = Green;
						if ( DK_Race.Race.Count != 0 && GUILayout.Button ( "Race" , Slim, GUILayout.Width (50))) {
							Selection.activeObject = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i];
							EditorVariables.SelectedElementName = (Selection.activeObject as DKOverlayData).overlayName;
							EditorVariables.SelectedElementGender = _DK_Race.Gender;
							if ( _DK_Race.Race.Count == 0 ){
								EditorVariables.SelectedElementRace = "No Race";
							}
							if ( _DK_Race.Race.Count == 1 ){
								EditorVariables.SelectedElementRace = _DK_Race.Race[0];
							}
							if ( _DK_Race.Race.Count > 1 ){
								EditorVariables.SelectedElementRace = "Multi";
							}
							EditorVariables.SelectedElementType = "OverlayElement";
							EditorVariables.SelectedElementOverlayType = _DK_Race.OverlayType;
							EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
							if ( _DK_Race.Place != null ) EditorVariables.SelectedElemPlace = _DK_Race.Place;
						}
						// Gender
						if ( DK_Race.Gender == "" ) GUI.color = Red;
						if ( DK_Race.Gender == "" && GUILayout.Button ( "No Gender" , Slim, GUILayout.Width (50))) {
							Selection.activeObject = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i];
							EditorVariables.SelectedElementName = (Selection.activeObject as DKOverlayData).overlayName;
							EditorVariables.SelectedElementGender = _DK_Race.Gender;
							if ( _DK_Race.Race.Count == 0 ){
								EditorVariables.SelectedElementRace = "No Race";
							}
							if ( _DK_Race.Race.Count == 1 ){
								EditorVariables.SelectedElementRace = _DK_Race.Race[0];
							}
							if ( _DK_Race.Race.Count > 1 ){
								EditorVariables.SelectedElementRace = "Multi";
							}
							EditorVariables.SelectedElementType = "OverlayElement";
							EditorVariables.SelectedElementOverlayType = _DK_Race.OverlayType;
							EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
							if ( _DK_Race.Place != null ) EditorVariables.SelectedElemPlace = _DK_Race.Place;
						}
						GUI.color = Green;
						if ( DK_Race.Gender != "" && GUILayout.Button ( DK_Race.Gender , Slim, GUILayout.Width (50))) {
							Selection.activeObject = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i];
							EditorVariables.SelectedElementName = (Selection.activeObject as DKOverlayData).overlayName;
							EditorVariables.SelectedElementGender = _DK_Race.Gender;
							if ( _DK_Race.Race.Count == 0 ){
								EditorVariables.SelectedElementRace = "No Race";
							}
							if ( _DK_Race.Race.Count == 1 ){
								EditorVariables.SelectedElementRace = _DK_Race.Race[0];
							}
							if ( _DK_Race.Race.Count > 1 ){
								EditorVariables.SelectedElementRace = "Multi";
							}
							EditorVariables.SelectedElementType = "OverlayElement";
							EditorVariables.SelectedElementOverlayType = _DK_Race.OverlayType;
							EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
							if ( _DK_Race.Place != null ) EditorVariables.SelectedElemPlace = _DK_Race.Place;
						}
						// Place
						if ( DK_Race.Place == null ) GUI.color = Red;
						if ( DK_Race.Place == null && GUILayout.Button ( "No Place" , Slim, GUILayout.Width (50))) {
							
						}
						GUI.color = Green;
						if ( DK_Race.Place != null && GUILayout.Button ( DK_Race.Place.name , Slim, GUILayout.Width (50))) {
							Selection.activeObject = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i];
							EditorVariables.SelectedElementName = (Selection.activeObject as DKOverlayData).overlayName;
							EditorVariables.SelectedElementGender = _DK_Race.Gender;
							
							if ( _DK_Race.Race.Count == 0 ){
								EditorVariables.SelectedElementRace = "No Race";
							}
							if ( _DK_Race.Race.Count == 1 ){
								EditorVariables.SelectedElementRace = _DK_Race.Race[0];
							}
							if ( _DK_Race.Race.Count > 1 ){
								EditorVariables.SelectedElementRace = "Multi";
							}
							EditorVariables.SelectedElementType = "OverlayElement";
							EditorVariables.SelectedElementOverlayType = _DK_Race.OverlayType;
							EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
							if ( _DK_Race.Place != null ) EditorVariables.SelectedElemPlace = _DK_Race.Place;
						}
						// Overlay Type
						if ( DK_Race.OverlayType == "" ) GUI.color = Red;
						if ( DK_Race.OverlayType == "" && GUILayout.Button ( "No Overlay Type" , Slim, GUILayout.Width (50))) {
							Selection.activeObject = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i];
							EditorVariables.SelectedElementName = (Selection.activeObject as DKOverlayData).overlayName;
							EditorVariables.SelectedElementGender = _DK_Race.Gender;
							EditorVariables.SelectedElementType = "OverlayElement";
							EditorVariables.SelectedElementOverlayType = _DK_Race.OverlayType;
							EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
							if ( _DK_Race.Place != null ) EditorVariables.SelectedElemPlace = _DK_Race.Place;
						}
						GUI.color = Green;
						if ( DK_Race.OverlayType != "" && GUILayout.Button ( DK_Race.OverlayType , Slim, GUILayout.Width (50))) {
							Selection.activeObject = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList[i];
							EditorVariables.SelectedElementName = (Selection.activeObject as DKOverlayData).overlayName;
							EditorVariables.SelectedElementGender = _DK_Race.Gender;
							EditorVariables.SelectedElementType = "OverlayElement";
							EditorVariables.SelectedElementOverlayType = _DK_Race.OverlayType;
							if ( _DK_Race.Place != null ) EditorVariables.SelectedElemPlace = _DK_Race.Place;
							EditorVariables.SelectedElementObj = Selection.activeObject as GameObject;
							if ( _DK_Race.Place != null ) EditorVariables.SelectedElemPlace = _DK_Race.Place;
						}
						// WearWeight
						GUI.color =  Color.gray;
						if (  DK_Race.WearWeight == "" ) 
						{
						
						}
						else
						{
							GUI.color = Color.white ;
							GUILayout.Label( DK_Race.WearWeight , Slim, GUILayout.Width (50));
						}
					}
				}
				#endregion
			}
			#endregion list

		}
		#endregion UMA Prefabs
		#endregion List
	}

	string path;
	public static UnityEngine.Object NewAsset;
	bool Duplicating;

	void DuplicateRace(){
		string file = AssetDatabase.GetAssetPath(Selection.activeObject);
		if ( file.Contains(".asset"))
		{
			path = file.Replace(".asset", "");
		}
		AssetDatabase.CopyAsset(file, path+"(Copy).asset");
		AssetDatabase.Refresh();
		Duplicating = true;
		BuildLocalAssetStore();
		BuildLocalAssetStoreOv();
		BuildLocalAssetStoreRa();
	}

	public DK_UMA_Browser ()
	{
		EditorApplication.update += Update;
	}

	void Update() {
		if ( Duplicating )
		if ( !NewAsset ){
			NewAsset = AssetDatabase.LoadMainAssetAtPath(path+"(Copy).asset");
		}
		else if ( Selection.activeObject.GetType().ToString() == "DKRaceData") {
			Selection.activeObject = NewAsset;
			List<DKRaceData> TmpList = new List<DKRaceData>();
			TmpList = EditorVariables.DK_UMACrowd.raceLibrary.raceElementList.ToList();
			TmpList.Add(NewAsset as DKRaceData);
			EditorVariables.DK_UMACrowd.raceLibrary.raceElementList = TmpList.ToArray();
			NewAsset = null;
			Duplicating = false;
			DKUMAProcessWindow.Processing = false;
			EditorUtility.SetDirty(EditorVariables.DK_UMACrowd.raceLibrary);
			AssetDatabase.SaveAssets();
		}
		else if ( Selection.activeObject.GetType().ToString() == "DKSlotData") {
			Selection.activeObject = NewAsset;
			Debug.Log ( NewAsset.name );
			List<DKSlotData> TmpList = new List<DKSlotData>();
			TmpList = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList.ToList();
			TmpList.Add(NewAsset as DKSlotData);
			EditorVariables.DK_UMACrowd.slotLibrary.slotElementList = TmpList.ToArray();
			NewAsset = null;
			Duplicating = false;
			DKUMAProcessWindow.Processing = false;
			EditorUtility.SetDirty(EditorVariables.DK_UMACrowd.slotLibrary);
			AssetDatabase.SaveAssets();
		}
		else if ( Selection.activeObject.GetType().ToString() == "DKOverlayData") {
			Selection.activeObject = NewAsset;
			Debug.Log ( NewAsset.name );
			List<DKOverlayData> TmpList = new List<DKOverlayData>();
			TmpList = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList.ToList();
			TmpList.Add(NewAsset as DKOverlayData);
			EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList = TmpList.ToArray();
			NewAsset = null;
			Duplicating = false;
			DKUMAProcessWindow.Processing = false;
			EditorUtility.SetDirty(EditorVariables.DK_UMACrowd.overlayLibrary);
			AssetDatabase.SaveAssets();
		}
	}
	
	public static void MoveElement ( UnityEngine.Object Element , GameObject LibraryObj) {
		if ( Element.GetType().ToString() == "DKRaceData"){
			List<DKRaceData> TmpList = new List<DKRaceData>();
			TmpList = EditorVariables.DK_UMACrowd.raceLibrary.raceElementList.ToList();
			TmpList.Remove(Selection.activeObject as DKRaceData);
			EditorVariables.DK_UMACrowd.raceLibrary.raceElementList = TmpList.ToArray();
			TmpList.Clear();
			TmpList = LibraryObj.GetComponent<DKRaceLibrary>().raceElementList.ToList();
			TmpList.Add(Selection.activeObject as DKRaceData);
			LibraryObj.GetComponent<DKRaceLibrary>().raceElementList = TmpList.ToArray();
			EditorUtility.SetDirty(LibraryObj);
			EditorUtility.SetDirty(EditorVariables.DK_UMACrowd.raceLibrary);
			AssetDatabase.SaveAssets();
		}
		if ( Element.GetType().ToString() == "DKSlotData"){
			List<DKSlotData> TmpList = new List<DKSlotData>();
			TmpList = EditorVariables.DK_UMACrowd.slotLibrary.slotElementList.ToList();
			TmpList.Remove(Selection.activeObject as DKSlotData);
			EditorVariables.DK_UMACrowd.slotLibrary.slotElementList = TmpList.ToArray();
			TmpList.Clear();
			TmpList = LibraryObj.GetComponent<DKSlotLibrary>().slotElementList.ToList();
			TmpList.Add(Selection.activeObject as DKSlotData);
			LibraryObj.GetComponent<DKSlotLibrary>().slotElementList = TmpList.ToArray();
			EditorUtility.SetDirty(LibraryObj);
			EditorUtility.SetDirty(EditorVariables.DK_UMACrowd.slotLibrary);
			AssetDatabase.SaveAssets();
		}
		if ( Element.GetType().ToString() == "DKOverlayData"){
			List<DKOverlayData> TmpList = new List<DKOverlayData>();
			TmpList = EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList.ToList();
			TmpList.Remove(Selection.activeObject as DKOverlayData);
			EditorVariables.DK_UMACrowd.overlayLibrary.overlayElementList = TmpList.ToArray();
			TmpList.Clear();
			TmpList = LibraryObj.GetComponent<DKOverlayLibrary>().overlayElementList.ToList();
			TmpList.Add(Selection.activeObject as DKOverlayData);
			LibraryObj.GetComponent<DKOverlayLibrary>().overlayElementList = TmpList.ToArray();
			EditorUtility.SetDirty(LibraryObj);
			EditorUtility.SetDirty(EditorVariables.DK_UMACrowd.overlayLibrary);
			AssetDatabase.SaveAssets();
		}
	}

	void BuildLocalAssetStore()
	{
		var tmp = Resources.FindObjectsOfTypeAll(typeof(DKSlotData))
			.Distinct()
				.ToList();
		var assets = tmp
			.Where(g=>g!=null && !string.IsNullOrEmpty(g.name) )
				.Where(a=>AssetDatabase.IsMainAsset(a) || AssetDatabase.IsSubAsset(a))
				.Distinct()
				.ToList();
		_assetStore.Clear();
		foreach(var a in assets)
		{
			_assetStore[a.GetType().Name][a.name].Add(a);
			
		}
		foreach(var a in tmp)
		{
			if(_assetStore.ContainsKey(a.GetType().Name) && _assetStore[a.GetType().Name].ContainsKey(a.name) && !_assetStore[a.GetType().Name][a.name].Contains(a))
			{
				_assetStore[a.GetType().Name][a.name].Add(a);
			}
		}
		
	}
	void BuildLocalAssetStoreOv()
	{
		var tmp = Resources.FindObjectsOfTypeAll(typeof(DKOverlayData))
			.Distinct()
				.ToList();
		var assets = tmp
			.Where(g=>g!=null && !string.IsNullOrEmpty(g.name) )
				.Where(a=>AssetDatabase.IsMainAsset(a) || AssetDatabase.IsSubAsset(a))
				.Distinct()
				.ToList();
		_assetStoreOv.Clear();
		foreach(var a in assets)
		{
			_assetStoreOv[a.GetType().Name][a.name].Add(a);
			
		}
		foreach(var a in tmp)
		{
			if(_assetStoreOv.ContainsKey(a.GetType().Name) && _assetStoreOv[a.GetType().Name].ContainsKey(a.name) && !_assetStoreOv[a.GetType().Name][a.name].Contains(a))
			{
				_assetStoreOv[a.GetType().Name][a.name].Add(a);
			}
		}
		
	}
	void BuildLocalAssetStoreRa()
	{
		var tmp = Resources.FindObjectsOfTypeAll(typeof(DKRaceData))
			.Distinct()
				.ToList();
		var assets = tmp
			.Where(g=>g!=null && !string.IsNullOrEmpty(g.name) )
				.Where(a=>AssetDatabase.IsMainAsset(a) || AssetDatabase.IsSubAsset(a))
				.Distinct()
				.ToList();
		_assetStoreRa.Clear();
		foreach(var a in assets)
		{
			_assetStoreRa[a.GetType().Name][a.name].Add(a);
			
		}
		foreach(var a in tmp)
		{
			if(_assetStoreRa.ContainsKey(a.GetType().Name) && _assetStoreRa[a.GetType().Name].ContainsKey(a.name) && !_assetStoreRa[a.GetType().Name][a.name].Contains(a))
			{
				_assetStoreRa[a.GetType().Name][a.name].Add(a);
			}
		}
		
	}
	void OnSelectionChange() {
		if ( Selection.activeGameObject ){

			if ( Selection.activeGameObject.GetComponent<SkinnedMeshRenderer>() == true
			    && Selection.activeGameObject.transform.parent.parent != null ){
				UMAModel = Selection.activeGameObject.transform.parent.parent.gameObject;
				NName = UMAModel.name;
			}
			if ( Selection.activeGameObject.GetComponent<DKUMAData>() == true && Selection.activeGameObject.transform.parent != null ){
				UMAModel = Selection.activeGameObject.transform.parent.gameObject;
				NName = UMAModel.name;
			}
			if ( Selection.activeGameObject.GetComponent<DK_Model>() == true ){
				UMAModel = Selection.activeGameObject.transform.gameObject;
				NName = UMAModel.name;
			}
			if ( UMAModel ){
				EditorVariables.EditedModel = UMAModel.transform;
				NName = UMAModel.name;
			}
		}
	}
}
