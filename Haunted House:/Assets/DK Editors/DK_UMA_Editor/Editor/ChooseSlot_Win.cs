using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0472 // Null is true.

public class ChooseSlot_Win : EditorWindow {

	public static string Action = "";
	bool LinkedSlotList;
	public static  List<DKSlotData> LegacyList = new List<DKSlotData>();
	Vector2 LinkedSlotListScroll = new Vector2();
	Vector2 scroll = new Vector2();
	string SearchString = "";

	Color Green = new Color (0.8f, 1f, 0.8f, 1);
	Color Red = new Color (0.9f, 0.5f, 0.5f);


	void OnGUI () {
		this.minSize = new Vector2(460, 500);
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

		#region choose Slot Tab
		Repaint();
		if ( Selection.activeObject && Selection.activeObject.GetType().ToString() != "DKSlotData" ){
				using (new Horizontal()) {	
					GUI.color = Color.yellow;
					GUILayout.Label("First you need to select a slot, close this Tab to return to the slots list.", GUILayout.ExpandWidth (false));
					GUI.color = new Color (0.8f, 1f, 0.8f, 1);
					
				}
				if ( GUILayout.Button ( "Close", "toolbarbutton", GUILayout.ExpandWidth (true))){
					Action = "";
					this.Close();
				}
			}
			// title
		if ( Selection.activeObject && Selection.activeObject.GetType().ToString() == "DKSlotData" )
			using (new Horizontal()) {	
				GUI.color = Color.white ;
				GUILayout.Label("Slot information", "toolbarbutton", GUILayout.ExpandWidth (true));
				GUI.color = new Color (0.9f, 0.5f, 0.5f);
				// actions
				if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.Width (20))) {
					Action = "";
					this.Close();
				}
			}
		if ( Selection.activeObject && Selection.activeObject.GetType().ToString() == "DKSlotData" ){
			DKSlotData _SlotElement = Selection.activeObject as DKSlotData;
			LegacyList = _SlotElement._LegacyData.LegacyList;
			using (new Horizontal()) {
				GUI.color = Color.yellow;
				GUILayout.Label("Slot's Info :", GUILayout.ExpandWidth (false));
				GUI.color = Color.white;
				GUILayout.Label("Gender :", Slim ,GUILayout.ExpandWidth (false));
				GUI.color = new Color (0.8f, 1f, 0.8f, 1);
				GUILayout.Label(_SlotElement.Gender , GUILayout.ExpandWidth (false));
				GUI.color = Color.white;
				GUILayout.Label("Slot Type :", Slim ,GUILayout.ExpandWidth (false));
				GUI.color = new Color (0.8f, 1f, 0.8f, 1);
			//	GUILayout.Label(_SlotElement.OverlayType , GUILayout.ExpandWidth (false));
				if ( LinkedSlotList )GUI.color = new Color (0.8f, 1f, 0.8f, 1);
				else GUI.color = Color.white;
				if ( GUILayout.Button ( "Legacy Slot List", GUILayout.ExpandWidth (true))) {
					if ( LinkedSlotList )LinkedSlotList = false;
					else LinkedSlotList = true;
				}
			}
				
				#region Linked Slot List
			if ( Selection.activeObject && LinkedSlotList && Selection.activeObject.GetType().ToString() == "DKSlotData" ) {
					GUILayout.Space(5);
					using (new Horizontal()) {	
						GUI.color = Color.white ;
						GUILayout.Label("Linked Slot List", "toolbarbutton", GUILayout.ExpandWidth (true));
						GUI.color = new Color (0.9f, 0.5f, 0.5f);
						// actions
						if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
							LinkedSlotList = false;
						}
					}
				if ( LegacyList.Count == 0 ) {
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
							GUILayout.Label("Slot Type", "toolbarbutton", GUILayout.Width (70));
							GUILayout.Label("WearWeight", "toolbarbutton", GUILayout.Width (70));
							GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
						}
						GUILayout.BeginScrollView (LinkedSlotListScroll, GUILayout.ExpandHeight (true));
						#region Linked Slots List List
						
					for(int i = 0; i < LegacyList.Count; i ++){
						if (LegacyList[i] != null ) using (new Horizontal()) {
								DKSlotData _DK_Race = LegacyList[i];
								if ( _DK_Race.Active == true ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
								else GUI.color = Color.gray ;
							if (GUILayout.Button ( "U", "toolbarbutton",  GUILayout.Width (20))){
									if ( _DK_Race.Active == true ) _DK_Race.Active = false;
									else _DK_Race.Active = true;
									EditorUtility.SetDirty(_DK_Race);
									AssetDatabase.SaveAssets();
								} 
								GUI.color = Color.white ;
								if (GUILayout.Button ( LegacyList[i].slotName , "toolbarbutton", GUILayout.Width (120))) {
										
								}
									GUI.color = new Color (0.9f, 0.5f, 0.5f);
								if (  GUILayout.Button ( "X " , "toolbarbutton", GUILayout.Width (20))) 
								{
									DKSlotData TmpSl = LegacyList[i];	
									(Selection.activeObject as DKSlotData)._LegacyData.LegacyList.Remove(TmpSl);
									TmpSl._LegacyData.ElderList.Remove((Selection.activeObject as DKSlotData));
									EditorUtility.SetDirty(TmpSl);
									EditorUtility.SetDirty(Selection.activeObject);
									AssetDatabase.SaveAssets();
									LegacyList.Remove(TmpSl);
								}

								GUI.color = Color.white ;
								string _Race = "No Race";
								if ( LegacyList[i].Race.Count == 0 )  {
									GUI.color = new Color (0.9f, 0.5f, 0.5f);
									_Race = "No Race";
								}
								if ( LegacyList[i].Race.Count > 1 )  {
									GUI.color = Color.cyan ;
									_Race = "Multi";
								}
								if ( LegacyList[i].Race.Count == 1 )  {
									GUI.color = Color.white ;
									_Race = LegacyList[i].Race[0];
								}
								if (GUILayout.Button ( _Race , "toolbarbutton", GUILayout.Width (70))) {
										DK_UMA_Editor.OpenRaceSelectEditor();
									}
									GUI.color = Color.white ;
								if (i < LegacyList.Count && GUILayout.Button ( LegacyList[i].Gender , "toolbarbutton", GUILayout.Width (70))) {
										
									}
								if (i < LegacyList.Count && LegacyList[i].Place && GUILayout.Button ( LegacyList[i].Place.name , "toolbarbutton", GUILayout.Width (70))) {
										
									}
								if (i < LegacyList.Count && GUILayout.Button ( LegacyList[i].OverlayType , "toolbarbutton", GUILayout.Width (70))) {
										
									}
								if (i < LegacyList.Count && GUILayout.Button ( LegacyList[i].WearWeight , "toolbarbutton", GUILayout.Width (70))) {
									
								}
							}
						}
						#endregion
						GUILayout.EndScrollView ();	
					}
					
				}
				#endregion
				
				GUILayout.Space(5);
				#region Slot List
			if ( Selection.activeObject.GetType().ToString() == "DKSlotData" ){
					using (new Horizontal()) {	
						GUI.color = Color.white ;
						GUILayout.Label("Select a Legacy slot for the Slot", "toolbarbutton", GUILayout.ExpandWidth (true));
					}
					
					using (new Horizontal()) {
						GUI.color = Color.white ;
						GUILayout.Label("Selected Slot :", GUILayout.ExpandWidth (false));
					if ( EditorVariables.SelectedLegacyElemSlot != null ) {
							GUI.color = new Color (0.8f, 1f, 0.8f, 1);
						GUILayout.Label(EditorVariables.SelectedLegacyElemSlot.slotName, GUILayout.ExpandWidth (true));
							if ( GUILayout.Button ( "Assign", GUILayout.Width (90))) {
							DKSlotData SelectedSlotElement =  Selection.activeObject as DKSlotData;
								
							if ( SelectedSlotElement != null ) {
								SelectedSlotElement._LegacyData.LegacyList.Add(EditorVariables.SelectedLegacyElemSlot);
								EditorVariables.SelectedLegacyElemSlot._LegacyData.ElderList.Add(Selection.activeObject as DKSlotData);
								EditorVariables.SelectedLegacyElemSlot._LegacyData.IsLegacy = true;
								Debug.Log ( "test");
							
							}
								EditorUtility.SetDirty(Selection.activeObject);
								EditorUtility.SetDirty(EditorVariables.SelectedLegacyElemSlot);
								AssetDatabase.SaveAssets();
						}
						}
						else {
							GUI.color = Color.yellow ;
							GUILayout.Label("Select a Slot in the following List.", GUILayout.ExpandWidth (true));
						}
					}

					using (new Horizontal()) {
						GUI.color = Color.yellow;
						GUILayout.Label ( "Slot Library :", GUILayout.Width (110));
						GUI.color = Color.white;
						GUILayout.TextField ( EditorVariables.DKSlotLibraryObj.name, GUILayout.ExpandWidth (true));
						if ( GUILayout.Button ( "Change", GUILayout.Width (60))){
							DK_UMA_Editor.OpenLibrariesWindow();
						ChangeLibrary.CurrentLibN = EditorVariables.DKSlotLibraryObj.name;
						ChangeLibrary.CurrentLibrary = EditorVariables.DKSlotLibraryObj;
							ChangeLibrary.Action = "";
						}
					}	
					#region Search
					using (new Horizontal()) {
						GUI.color = Color.white;
						GUILayout.Label("Search for :", GUILayout.Width (75));
						SearchString = GUILayout.TextField(SearchString, 100, GUILayout.ExpandWidth (true));
						
					}
					#endregion Search

					GUI.color = Color.cyan ;
					using (new Horizontal()) {
					GUILayout.Label("U", "toolbarbutton", GUILayout.Width (20));
					GUILayout.Label("Slot", "toolbarbutton", GUILayout.Width (140));
				//	GUILayout.Label("Race", "toolbarbutton", GUILayout.Width (70));
					GUILayout.Label("Gender", "toolbarbutton", GUILayout.Width (70));
						GUILayout.Label("Place", "toolbarbutton", GUILayout.Width (70));
						GUILayout.Label("Slot Type", "toolbarbutton", GUILayout.Width (90));
						GUILayout.Label("WearWeight", "toolbarbutton", GUILayout.Width (70));
						GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
					}
					using (new ScrollView(ref scroll)) 
					{
						#region Slots
						using (new Horizontal()) {	
							GUI.color = Color.yellow ;
							GUILayout.Label ( "Slots Library :"+EditorVariables._DKSlotLibrary.name, GUILayout.ExpandWidth (false));
						}
						for(int i = 0; i < EditorVariables._DKSlotLibrary.slotElementList.Length; i ++){
							if (EditorVariables._DKSlotLibrary.slotElementList[i] != null
							    && (SearchString == "" 
							    || EditorVariables._DKSlotLibrary.slotElementList[i].slotName.ToLower().Contains(SearchString.ToLower())) )
							using (new Horizontal()) {
								DKSlotData _DK_Race = EditorVariables._DKSlotLibrary.slotElementList[i];
								if ( _DK_Race.Active == true ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
								else GUI.color = Color.gray ;
							if (GUILayout.Button ( "U", "toolbarbutton",  GUILayout.Width (20))){
									if ( _DK_Race.Active == true ) _DK_Race.Active = false;
									else _DK_Race.Active = true;
									AssetDatabase.SaveAssets();
								} 
								
							if ( LegacyList.Contains(EditorVariables._DKSlotLibrary.slotElementList[i]) == true )  GUI.color =  Color.gray;
							else GUI.color = Color.white;
							if ( EditorVariables._DKSlotLibrary.slotElementList[i] == EditorVariables.SelectedElemSlot ) GUI.color = Color.yellow;
							if ( EditorVariables._DKSlotLibrary.slotElementList[i] == EditorVariables.SelectedLegacyElemSlot ) GUI.color = Green;

							if (GUILayout.Button ( EditorVariables._DKSlotLibrary.slotElementList[i].slotName , "toolbarbutton", GUILayout.Width (140))) {
									if ( LegacyList.Contains(EditorVariables._DKSlotLibrary.slotElementList[i]) == false )
										EditorVariables.SelectedLegacyElemSlot = EditorVariables._DKSlotLibrary.slotElementList[i];
									
								}
								// Race
								DKSlotData DK_Race;
								DK_Race = EditorVariables._DKSlotLibrary.slotElementList[i];
								if ( LegacyList.Contains(EditorVariables._DKSlotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
								else GUI.color = Color.white;
						//	if ( DK_Race.Race.Count == 0 && GUILayout.Button ( "No Race" , "toolbarbutton", GUILayout.Width (70))) {
						//			EditorVariables.SelectedElemSlot = EditorVariables._DKSlotLibrary.slotElementList[i];
						//		}
								
						//		if ( LegacyList.Contains(EditorVariables._DKSlotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
						//		else GUI.color = Color.white;
						//	if ( DK_Race.Race.Count == 1 && GUILayout.Button ( DK_Race.Race[0] , "toolbarbutton", GUILayout.Width (70))) {
						//			EditorVariables.SelectedElemSlot = EditorVariables._DKSlotLibrary.slotElementList[i];
						//		}
						//		else 
						//	if ( DK_Race.Race.Count == 1 && GUILayout.Button ( "Multi" , "toolbarbutton", GUILayout.Width (70))) {
						//			EditorVariables.SelectedElemSlot = EditorVariables._DKSlotLibrary.slotElementList[i];
						//		}
								// Gender
							if ( LegacyList.Contains(EditorVariables._DKSlotLibrary.slotElementList[i]) == false )  GUI.color =  Color.gray;
								else GUI.color = Color.white;
							if ( DK_Race.Gender == "" && GUILayout.Button ( "No Gender" , "toolbarbutton", GUILayout.Width (70))) {
									EditorVariables.SelectedElemSlot = EditorVariables._DKSlotLibrary.slotElementList[i];
								}
								
								if ( LegacyList.Contains(EditorVariables._DKSlotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
								else GUI.color = Color.white;
							if ( DK_Race.Gender != "" && GUILayout.Button ( DK_Race.Gender , "toolbarbutton", GUILayout.Width (70))) {
									EditorVariables.SelectedElemSlot = EditorVariables._DKSlotLibrary.slotElementList[i];
								}
								// Place
							if ( LegacyList.Contains(EditorVariables._DKSlotLibrary.slotElementList[i]) == false )  GUI.color =  Color.gray;
								else GUI.color = Color.white;
							if ( DK_Race.Place == null && GUILayout.Button ( "No Place" , "toolbarbutton", GUILayout.Width (70))) {
									
								}
								
								if ( LegacyList.Contains(EditorVariables._DKSlotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
								else GUI.color = Color.white;
							if ( DK_Race.Place != null && GUILayout.Button ( DK_Race.Place.name , "toolbarbutton", GUILayout.Width (70))) {
									EditorVariables.SelectedElemSlot = EditorVariables._DKSlotLibrary.slotElementList[i];
								}
								// Slot Type
								if ( LegacyList.Contains(EditorVariables._DKSlotLibrary.slotElementList[i]) == false )  GUI.color =  Color.gray;
								else GUI.color = Color.white;
							if ( DK_Race.OverlayType == "" && GUILayout.Button ( "No Type" , "toolbarbutton", GUILayout.Width (90))) {
									EditorVariables.SelectedElemSlot = EditorVariables._DKSlotLibrary.slotElementList[i];
								}
								
								if ( LegacyList.Contains(EditorVariables._DKSlotLibrary.slotElementList[i]) )  GUI.color =  Color.gray;
								else GUI.color = Color.white;
							if ( DK_Race.OverlayType != "" && GUILayout.Button ( DK_Race.OverlayType , "toolbarbutton", GUILayout.Width (90))) {
									EditorVariables.SelectedElemSlot = EditorVariables._DKSlotLibrary.slotElementList[i];
								}
								// WearWeight
								GUI.color =  Color.gray;
								if (  DK_Race.WearWeight == "" ) 
								{
									//	GUI.color = Color.gray ;
									//	GUILayout.Space(55);
									//	GUILayout.Label ( "No Weight");
								}
								else
								{
								/*	if ( DK_Race.WearWeight != "" ) GUI.color = new Color (0.9f, 0.5f, 0.5f);
									if ( DK_Race.WearWeight != "" && GUILayout.Button ( "X " , "toolbarbutton", GUILayout.Width (20))) {
										DK_Race.WearWeight = "";
										EditorUtility.SetDirty(DK_Race);
										AssetDatabase.SaveAssets();
									}*/
									GUI.color = Color.white ;
								GUILayout.Label( DK_Race.WearWeight , "toolbarbutton", GUILayout.Width (70));
								}
							}
						#endregion
					#endregion
					}
				}
			}
			#endregion
		}
	}
}
