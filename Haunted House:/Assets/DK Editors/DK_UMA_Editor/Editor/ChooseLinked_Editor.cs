using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0472 // Null is true.

public class ChooseLinked_Editor : EditorWindow {

	public static string Action = "";
	bool LinkedOverlayList;
	public static  List<DKOverlayData> overlayList = new List<DKOverlayData>();
	Vector2 LinkedOverlayListScroll = new Vector2();
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

		#region choose Overlay Tab
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
				if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
					Action = "";
					this.Close();
				}
			}

		if ( Selection.activeObject && Selection.activeObject.GetType().ToString() == "DKSlotData" ){
			DKSlotData _SlotElement = Selection.activeObject as DKSlotData;
			overlayList = _SlotElement.LinkedOverlayList;
			using (new Horizontal()) {
				GUI.color = Color.yellow;
				GUILayout.Label("Slot's Info :", GUILayout.ExpandWidth (false));
				GUI.color = Color.white;
				GUILayout.Label("Gender :", Slim ,GUILayout.ExpandWidth (false));
				GUI.color = new Color (0.8f, 1f, 0.8f, 1);
				GUILayout.Label(_SlotElement.Gender , GUILayout.ExpandWidth (false));
				GUI.color = Color.white;
				GUILayout.Label("Overlay Type :", Slim ,GUILayout.ExpandWidth (false));
				GUI.color = new Color (0.8f, 1f, 0.8f, 1);
				GUILayout.Label(_SlotElement.OverlayType , GUILayout.ExpandWidth (false));
				if ( LinkedOverlayList )GUI.color = new Color (0.8f, 1f, 0.8f, 1);
				else GUI.color = Color.white;
				if ( GUILayout.Button ( "Linked Overlay List", GUILayout.ExpandWidth (true))) {
					if ( LinkedOverlayList )LinkedOverlayList = false;
					else LinkedOverlayList = true;
				}
			}
				
				#region Linked Overlay List
			if ( Selection.activeObject && LinkedOverlayList && Selection.activeObject.GetType().ToString() == "DKSlotData" ) {
					GUILayout.Space(5);
					using (new Horizontal()) {	
						GUI.color = Color.white ;
						GUILayout.Label("Linked Overlay List", "toolbarbutton", GUILayout.ExpandWidth (true));
						GUI.color = new Color (0.9f, 0.5f, 0.5f);
						// actions
						if ( GUILayout.Button ( "X", "toolbarbutton", GUILayout.ExpandWidth (false))) {
							LinkedOverlayList = false;
						}
					}
				if ( overlayList.Count == 0 ) {
						using (new HorizontalCentered()) {	
							GUI.color = Color.yellow ;
							GUILayout.Label("No Linked Overlay in the List.", GUILayout.ExpandWidth (true));
						}
					}
					else {
						GUI.color = Color.white ;
						using (new Horizontal()) {
							GUILayout.Label("Linked Overlays", "toolbarbutton", GUILayout.Width (160));
							GUILayout.Label("Race", "toolbarbutton", GUILayout.Width (70));
							GUILayout.Label("Gender", "toolbarbutton", GUILayout.Width (70));
							GUILayout.Label("Place", "toolbarbutton", GUILayout.Width (70));
							GUILayout.Label("Overlay Type", "toolbarbutton", GUILayout.Width (70));
							GUILayout.Label("WearWeight", "toolbarbutton", GUILayout.Width (70));
							GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
						}
						GUILayout.BeginScrollView (LinkedOverlayListScroll, GUILayout.ExpandHeight (true));
						#region Linked Overlays List List
						
						for(int i = 0; i < overlayList.Count; i ++){
							if (overlayList[i] != null ) using (new Horizontal()) {
								DKOverlayData _DK_Race = overlayList[i];
								if ( _DK_Race.Active == true ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
								else GUI.color = Color.gray ;
							if (GUILayout.Button ( "U", "toolbarbutton",  GUILayout.Width (20))){
									if ( _DK_Race.Active == true ) _DK_Race.Active = false;
									else _DK_Race.Active = true;
									EditorUtility.SetDirty(_DK_Race);
									AssetDatabase.SaveAssets();
								} 
								GUI.color = Color.white ;
							if (GUILayout.Button ( overlayList[i].overlayName , "toolbarbutton", GUILayout.Width (120))) {
									
								}
								GUI.color = new Color (0.9f, 0.5f, 0.5f);
							if (  GUILayout.Button ( "X " , "toolbarbutton", GUILayout.Width (20))) {
									DKOverlayData TmpOv = overlayList[i];	
								(Selection.activeObject as DKSlotData).overlayList.Remove(TmpOv);
								TmpOv.LinkedToSlot.Remove((Selection.activeObject as DKSlotData));
									EditorUtility.SetDirty(TmpOv);
									EditorUtility.SetDirty(Selection.activeObject);
									AssetDatabase.SaveAssets();
									overlayList.Remove(TmpOv);
								}
								GUI.color = Color.white ;
								string _Race = "No Race";
								if ( overlayList[i].Race.Count == 0 )  {
								//	GUI.color = new Color (0.9f, 0.5f, 0.5f);
									_Race = "No Race";
								}
								if ( overlayList[i].Race.Count > 1 )  {
									GUI.color = Color.cyan ;
									_Race = "Multi";
								}
								if ( overlayList[i].Race.Count == 1 )  {
									GUI.color = Color.white ;
									_Race = overlayList[i].Race[0];
								}
							if (GUILayout.Button ( _Race , "toolbarbutton", GUILayout.Width (70))) {
									DK_UMA_Editor.OpenRaceSelectEditor();
								}
								GUI.color = Color.white ;
							if (i < overlayList.Count && GUILayout.Button ( overlayList[i].Gender , "toolbarbutton", GUILayout.Width (70))) {
									
								}
							if (i < overlayList.Count && overlayList[i].Place && GUILayout.Button ( overlayList[i].Place.name , "toolbarbutton", GUILayout.Width (70))) {
									
								}
							if (i < overlayList.Count && GUILayout.Button ( overlayList[i].OverlayType , "toolbarbutton", GUILayout.Width (70))) {
									
								}
							if (i < overlayList.Count && GUILayout.Button ( overlayList[i].WearWeight , "toolbarbutton", GUILayout.Width (70))) {
									
								}
							}
						}
						#endregion
						GUILayout.EndScrollView ();	
					}
					
				}
				#endregion
				
				GUILayout.Space(5);
				#region Overlay List
			if ( Selection.activeObject.GetType().ToString() == "DKSlotData" ){
					using (new Horizontal()) {	
						GUI.color = Color.white ;
						GUILayout.Label("Select an Overlay for the Slot", "toolbarbutton", GUILayout.ExpandWidth (true));
					}
					
				using (new Horizontal()) {
					GUI.color = Color.white ;
					GUILayout.Label("Selected Overlay :", GUILayout.ExpandWidth (false));
					if ( EditorVariables.SelectedLinkedOvlay != null ) {
						GUI.color = new Color (0.8f, 1f, 0.8f, 1);
						GUILayout.Label(EditorVariables.SelectedLinkedOvlay.overlayName, GUILayout.ExpandWidth (true));
						if ( GUILayout.Button ( "Link it", GUILayout.Width (90))) {
							DKSlotData SelectedSlotElement =  Selection.activeObject as DKSlotData;
							if ( SelectedSlotElement != null ) 
							{
								SelectedSlotElement.LinkedOverlayList.Add(EditorVariables.SelectedLinkedOvlay);
							}
							DKOverlayData SelectedOverlayElement =  EditorVariables.SelectedLinkedOvlay;
							if ( SelectedOverlayElement != null ) 
							{
								SelectedOverlayElement.LinkedToSlot.Add(SelectedSlotElement);
							}
							EditorUtility.SetDirty(Selection.activeObject);
							EditorUtility.SetDirty(EditorVariables.SelectedLinkedOvlay);
							AssetDatabase.SaveAssets();
						}
					}
					else {
						GUI.color = Color.yellow ;
						GUILayout.Label("Select an Overlay in the following List.", GUILayout.ExpandWidth (true));
					}
				}

					using (new Horizontal()) {
						GUI.color = Color.yellow;
						GUILayout.Label ( "Overlay Library :", GUILayout.Width (110));
						GUI.color = Color.white;
						GUILayout.TextField ( EditorVariables.OverlayLibraryObj.name, GUILayout.ExpandWidth (true));
						if ( GUILayout.Button ( "Change", GUILayout.Width (60))){
							DK_UMA_Editor.OpenLibrariesWindow();
							ChangeLibrary.CurrentLibN = EditorVariables.OverlayLibraryObj.name;
							ChangeLibrary.CurrentLibrary = EditorVariables.OverlayLibraryObj;
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
					GUILayout.Label("Overlay", "toolbarbutton", GUILayout.Width (140));
				//	GUILayout.Label("Race", "toolbarbutton", GUILayout.Width (70));
					GUILayout.Label("Gender", "toolbarbutton", GUILayout.Width (70));
						GUILayout.Label("Place", "toolbarbutton", GUILayout.Width (70));
						GUILayout.Label("Overlay Type", "toolbarbutton", GUILayout.Width (90));
						GUILayout.Label("WearWeight", "toolbarbutton", GUILayout.Width (70));
						GUILayout.Label("", "toolbarbutton", GUILayout.ExpandWidth (true));
					}
					using (new ScrollView(ref scroll)) 
					{
						#region Overlays
						using (new Horizontal()) {	
							GUI.color = Color.yellow ;
							GUILayout.Label ( "Overlays Library :"+EditorVariables._OverlayLibrary.name, GUILayout.ExpandWidth (false));
						}
						for(int i = 0; i < EditorVariables._OverlayLibrary.overlayElementList.Length; i ++){
							if (EditorVariables._OverlayLibrary.overlayElementList[i] != null
							    && (SearchString == "" 
							    || EditorVariables._OverlayLibrary.overlayElementList[i].overlayName.ToLower().Contains(SearchString.ToLower())) )
							using (new Horizontal()) {
								DKOverlayData _DK_Race = EditorVariables._OverlayLibrary.overlayElementList[i];
								if ( _DK_Race.Active == true ) GUI.color = new Color (0.8f, 1f, 0.8f, 1);
								else GUI.color = Color.gray ;
							if (GUILayout.Button ( "U", "toolbarbutton",  GUILayout.Width (20))){
									if ( _DK_Race.Active == true ) _DK_Race.Active = false;
									else _DK_Race.Active = true;
									AssetDatabase.SaveAssets();
								} 
								
						//	if ( overlayList.Contains(EditorVariables._OverlayLibrary.overlayElementList[i]) )  GUI.color =  Green;
							if ( overlayList.Contains(EditorVariables._OverlayLibrary.overlayElementList[i]) )  GUI.color = Color.yellow;
							else GUI.color = Color.white;
							if ( EditorVariables._OverlayLibrary.overlayElementList[i] == EditorVariables.SelectedLinkedOvlay ) GUI.color = Green;
						
							if (GUILayout.Button ( EditorVariables._OverlayLibrary.overlayElementList[i].overlayName , "toolbarbutton", GUILayout.Width (140))) {
									if ( overlayList.Contains(EditorVariables._OverlayLibrary.overlayElementList[i]) == false )
									EditorVariables.SelectedLinkedOvlay = EditorVariables._OverlayLibrary.overlayElementList[i];
									
								}
								// Race
								DKOverlayData DK_Race;
								DK_Race = EditorVariables._OverlayLibrary.overlayElementList[i];
								if ( overlayList.Contains(EditorVariables._OverlayLibrary.overlayElementList[i]) )  GUI.color =  Color.gray;
								else GUI.color = Color.white;
						//	if ( DK_Race.Race.Count == 0 && GUILayout.Button ( "No Race" , "toolbarbutton", GUILayout.Width (70))) {
						//			EditorVariables.SelectedElemOvlay = EditorVariables._OverlayLibrary.overlayElementList[i];
						//		}
								
						//		if ( overlayList.Contains(EditorVariables._OverlayLibrary.overlayElementList[i]) )  GUI.color =  Color.gray;
						//		else GUI.color = Color.white;
						//	if ( DK_Race.Race.Count == 1 && GUILayout.Button ( DK_Race.Race[0] , "toolbarbutton", GUILayout.Width (70))) {
						//			EditorVariables.SelectedElemOvlay = EditorVariables._OverlayLibrary.overlayElementList[i];
						//		}
						//		else 
						//	if ( DK_Race.Race.Count == 1 && GUILayout.Button ( "Multi" , "toolbarbutton", GUILayout.Width (70))) {
						//			EditorVariables.SelectedElemOvlay = EditorVariables._OverlayLibrary.overlayElementList[i];
						//		}
								// Gender
							if ( overlayList.Contains(EditorVariables._OverlayLibrary.overlayElementList[i]) == false )  GUI.color =  Color.gray;
								else GUI.color = Color.white;
							if ( DK_Race.Gender == "" && GUILayout.Button ( "No Gender" , "toolbarbutton", GUILayout.Width (70))) {
									EditorVariables.SelectedElemOvlay = EditorVariables._OverlayLibrary.overlayElementList[i];
								}
								
								if ( overlayList.Contains(EditorVariables._OverlayLibrary.overlayElementList[i]) )  GUI.color =  Color.gray;
								else GUI.color = Color.white;
							if ( DK_Race.Gender != "" && GUILayout.Button ( DK_Race.Gender , "toolbarbutton", GUILayout.Width (70))) {
									EditorVariables.SelectedElemOvlay = EditorVariables._OverlayLibrary.overlayElementList[i];
								}
								// Place
							if ( overlayList.Contains(EditorVariables._OverlayLibrary.overlayElementList[i]) == false )  GUI.color =  Color.gray;
								else GUI.color = Color.white;
							if ( DK_Race.Place == null && GUILayout.Button ( "No Place" , "toolbarbutton", GUILayout.Width (70))) {
									
								}
								
								if ( overlayList.Contains(EditorVariables._OverlayLibrary.overlayElementList[i]) )  GUI.color =  Color.gray;
								else GUI.color = Color.white;
							if ( DK_Race.Place != null && GUILayout.Button ( DK_Race.Place.name , "toolbarbutton", GUILayout.Width (70))) {
									EditorVariables.SelectedElemOvlay = EditorVariables._OverlayLibrary.overlayElementList[i];
								}
								// Overlay Type
								if ( overlayList.Contains(EditorVariables._OverlayLibrary.overlayElementList[i]) == false )  GUI.color =  Color.gray;
								else GUI.color = Color.white;
							if ( DK_Race.OverlayType == "" && GUILayout.Button ( "No Type" , "toolbarbutton", GUILayout.Width (90))) {
									EditorVariables.SelectedElemOvlay = EditorVariables._OverlayLibrary.overlayElementList[i];
								}
								
								if ( overlayList.Contains(EditorVariables._OverlayLibrary.overlayElementList[i]) )  GUI.color =  Color.gray;
								else GUI.color = Color.white;
							if ( DK_Race.OverlayType != "" && GUILayout.Button ( DK_Race.OverlayType , "toolbarbutton", GUILayout.Width (90))) {
									EditorVariables.SelectedElemOvlay = EditorVariables._OverlayLibrary.overlayElementList[i];
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
