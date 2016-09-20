using System.IO;
using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class DKUMA_Variables : MonoBehaviour {

	public bool UseDkUMA;
	public bool UseUMA;

	DK_UMACrowd _DK_UMACrowd;
	DKUMAGenerator _DKUMAGenerator ;
	DKUMACustomization _DKUMACustomization;

	DK_GeneratorPresetLibrary ActivePresetLib;

	public DKSlotLibrary ActiveSlotLibrary;
	public DKSlotLibrary PrefabSlotLibrary;
//	public DKSlotLibrary BodySlotLibrary;
//	public DKSlotLibrary WearSlotLibrary;

	public DKOverlayLibrary ActiveOverlayLibrary;
	public DKOverlayLibrary PrefabOverlayLibrary;
//	public DKOverlayLibrary BodyOverlayLibrary;
//	public DKOverlayLibrary WearOverlayLibrary;

	public DKRaceLibrary ActiveRaceLibrary;
	public DKRaceLibrary DefaultRaceLibrary;
	public DKRaceLibrary RaceLibrary;

	public static RaceLibrary _RaceLibrary;
	public static SlotLibrary _SlotLibrary;
	public static OverlayLibrary _OverlayLibrary;

	public List<DKSlotData> DKSlotsList = new List<DKSlotData>();
	public List<string> DKSlotsNamesList = new List<string>();
	
	public List<UMA.SlotData> UMASlotsList = new List<UMA.SlotData>();
	
	public List<DKOverlayData> DKOverlaysList = new List<DKOverlayData>();
	public List<string> DKOverlaysNamesList = new List<string>();
	public List<Texture2D> PreviewsList = new List<Texture2D>();
	public List<Texture2D> OvPreviewsList = new List<Texture2D>();
	
	public List<UMA.OverlayData> UMAOverlaysList = new List<UMA.OverlayData>();

	public PhysicMaterial DK_UMA_Collider_Material;

//	string Action = "";

	#region Search Voids
	public void SearchAll (){
	//	Action = "Detecting";
		DKSlotsNamesList.Clear ();
		DKOverlaysNamesList.Clear ();
		
		SearchUMASlots ();
		SearchDKSlots ();
		SearchUMAOverlays ();
		SearchDKOverlays ();

	//	Action = "";
	}

	public void SearchDKUMA (){
	//	Action = "Detecting";
		DKSlotsNamesList.Clear ();
		DKOverlaysNamesList.Clear ();
		
		SearchDKSlots ();
		SearchDKOverlays ();
 
	//	Action = "";
	}

	public void SearchUMA (){
	//	Action = "Detecting";

		SearchUMASlots ();
		SearchUMAOverlays ();

	//	Action = "";
	}

	void SearchUMASlots (){
		UMASlotsList.Clear ();
		GameObject UMACrowdObj = GameObject.Find ("DKUMACrowd");
		DK_UMACrowd _DK_UMACrowd =  UMACrowdObj.GetComponent<DK_UMACrowd>();
		System.Type type = _DK_UMACrowd.Default.ResearchDefault._SlotData.GetType() ;
		GetAssetsOfType( type , ".asset" );
	}
	void SearchDKSlots (){
		DKSlotsList.Clear ();
		DKSlotsNamesList.Clear ();
		PreviewsList.Clear ();
		GameObject UMACrowdObj = GameObject.Find ("DKUMACrowd");
		DK_UMACrowd _DK_UMACrowd =  UMACrowdObj.GetComponent<DK_UMACrowd>();
		System.Type typeDK = _DK_UMACrowd.Default.ResearchDefault._DKSlotData.GetType() ;
		GetAssetsOfType( typeDK , ".asset" );
	}
	void SearchUMAOverlays (){
		UMAOverlaysList.Clear ();
		GameObject UMACrowdObj = GameObject.Find ("DKUMACrowd");
		DK_UMACrowd _DK_UMACrowd =  UMACrowdObj.GetComponent<DK_UMACrowd>();
		System.Type type = _DK_UMACrowd.Default.ResearchDefault._OverlayData.GetType() ;
		GetAssetsOfType( type , ".asset" );
	}
	void SearchDKOverlays (){
		DKOverlaysList.Clear ();
		DKOverlaysNamesList.Clear ();
		OvPreviewsList.Clear ();
		
		GameObject UMACrowdObj = GameObject.Find ("DKUMACrowd");
		DK_UMACrowd _DK_UMACrowd =  UMACrowdObj.GetComponent<DK_UMACrowd>();
		System.Type typeDK = _DK_UMACrowd.Default.ResearchDefault._DKOverlayData.GetType() ;
		GetAssetsOfType( typeDK , ".asset" );
		
		//	DK_RPG_UMA_Generator _DK_RPG_UMA_Generator = DK_UMA.GetComponent<DK_RPG_UMA_Generator>();
		//	_DK_RPG_UMA_Generator.PopulateAllLists();
	}
	#endregion Search Voids
	
	#region Search Elements
	public UnityEngine.Object[] GetAssetsOfType(System.Type type, string fileExtension)
	{
		List<UnityEngine.Object> tempObjects = new List<UnityEngine.Object>();
		DirectoryInfo directory = new DirectoryInfo(Application.dataPath+"/DK Editors/DK_UMA_Editor/Elements");
		DirectoryInfo UMAdirectory = new DirectoryInfo(Application.dataPath+"/UMA");

		List<FileInfo> goFileInfo = new List<FileInfo>();
		goFileInfo.AddRange(directory.GetFiles("*" + fileExtension, SearchOption.AllDirectories));
		goFileInfo.AddRange(UMAdirectory.GetFiles("*" + fileExtension, SearchOption.AllDirectories));

	//	FileInfo[] goFileInfo = directory.GetFiles("*" + fileExtension, SearchOption.AllDirectories);

		int i = 0; int goFileInfoLength = goFileInfo.Count;
		FileInfo tempGoFileInfo; string tempFilePath;
		UnityEngine.Object tempGO;
		for (; i < goFileInfoLength; i++)
		{
		//	try {
				tempGoFileInfo = goFileInfo[i];
				if (tempGoFileInfo == null)
					continue;
				
				tempFilePath = tempGoFileInfo.FullName;
				tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
				try{
					tempGO = Resources.LoadAssetAtPath(tempFilePath, typeof(UnityEngine.Object)) as UnityEngine.Object;
					if (tempGO == null)
					{
						//	Debug.LogWarning("Skipping Null");
						continue;
					}
					else if (tempGO.GetType() != type)
					{
						//	Debug.LogWarning("Skipping " + tempGO.GetType().ToString());
						continue;
					}
					
					// finishing
					if ( tempGO.name.Contains("DefaultDK") == false 
					    && tempGO.name.Contains("DefaultSlotType") == false
					    && tempGO.name.Contains("DefaultOverlayType") == false
					    && tempGO.name.Contains("DefaultUMA") == false){
						tempObjects.Add(tempGO);
						//	Debug.Log ( "Type : "+type.ToString()+"Item : "+tempGO.name+" / "+tempGO.GetType().ToString());
						
						// UMA Slots
						if ( tempGO.GetType().ToString() == "UMA.SlotData" ){
							UMASlotsList.Add (tempGO as UMA.SlotData);
						}
						// DK Slots
						if ( tempGO.GetType().ToString() == "DKSlotData" ){
							DKSlotsList.Add (tempGO as DKSlotData);
							DKSlotsNamesList.Add (tempGO.name);

						}
						// UMA Overlays
						if ( tempGO.GetType().ToString() == "UMA.OverlayData" ){
							UMAOverlaysList.Add (tempGO as UMA.OverlayData);
						}
						// DK Overlays
						if ( tempGO.GetType().ToString() == "DKOverlayData" ){
							DKOverlaysList.Add (tempGO as DKOverlayData);
							DKOverlaysNamesList.Add (tempGO.name);
						}
					}
				}catch(Exception e){Debug.Log ( "test"+e.ToString());}
		//	}catch(Exception e){Debug.Log (e.ToString());}
		}
		return tempObjects.ToArray();
	}
	#endregion Search Elements

	public void CleanLibraries (){
		List<DKSlotData> tmpList = new List<DKSlotData>();
		List<DKSlotData> RemoveList = new List<DKSlotData>();
		List<DKOverlayData> tmpList2 = new List<DKOverlayData>();
		List<DKOverlayData> removeList2 = new List<DKOverlayData>();

		if ( _DK_UMACrowd == null )
		try{
			_DK_UMACrowd = GameObject.Find ("DKUMACrowd").GetComponent<DK_UMACrowd>();
		}catch(NullReferenceException){ Debug.LogError ( "DK UMA is not installed in your scene, please install DK UMA." );}

		tmpList = _DK_UMACrowd.slotLibrary.slotElementList.ToList();
		for(int i = 0; i < tmpList.Count; i ++){
			if ( tmpList[i] == null ) RemoveList.Add(tmpList[i]);
		}
		
		tmpList2 = _DK_UMACrowd.overlayLibrary.overlayElementList.ToList();
		for(int i = 0; i < tmpList2.Count; i ++){
			if ( tmpList2[i] == null ) removeList2.Add(tmpList2[i]);
		}
		foreach ( DKSlotData slot in RemoveList ){
			if ( tmpList.Contains (slot) ) tmpList.Remove (slot);
		}
		foreach ( DKOverlayData overlay in removeList2 ){
			if ( tmpList2.Contains (overlay) ) tmpList2.Remove (overlay);
		}
		
		_DK_UMACrowd.slotLibrary.slotElementList = tmpList.ToArray();
		_DK_UMACrowd.overlayLibrary.overlayElementList = tmpList2.ToArray();
		
	//	Debug.Log ("All libraries cleaned of missing references.");
	}

}
