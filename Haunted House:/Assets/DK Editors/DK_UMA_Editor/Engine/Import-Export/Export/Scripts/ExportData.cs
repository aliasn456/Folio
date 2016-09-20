using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ExportData : ScriptableObject{



	[System.Serializable]
	public class ExportRaceData{

	}
	[System.Serializable]
	public class ExportSlotData{
		
	}
	[System.Serializable]
	public class ExportDKOverlayData{
		
	}
	public string Status;
	[System.Serializable]
	public class _raceData{
		public DKRaceData RaceData;
		public string Name;
		public string Path;
		public GameObject racePrefab;
		public UMA.UmaTPose TPose;
		public List<_overlayData> _OverlaysList = new List<_overlayData>();
		public List<_slotData> _SlotsList = new List<_slotData>();
		public List<ColorPresetData> _ColorsList = new List<ColorPresetData>();
	}
	
	[System.Serializable]
	public class _slotData{
		public DKSlotData SlotData;
		public string Name;
		public string Path;
		public SkinnedMeshRenderer meshRenderer;
		public Material materialSample;
		public DK_SlotsAnatomyElement Place;
		public List<string> _Race = new List<string>();
		public List<_raceData> _RacesList = new List<_raceData>();
		public List<_overlayData> _OverlaysList = new List<_overlayData>();
		public List<ColorPresetData> _ColorsList = new List<ColorPresetData>();
	}
	[System.Serializable]
	public class _overlayData{
		public DKOverlayData DKOverlayData;
		public string Name;
		public string Path;
		public List<Texture2D> textureList = new List<Texture2D>();
		public DK_SlotsAnatomyElement Place;
		public List<string> _Race = new List<string>();
		public List<_raceData> _RacesList = new List<_raceData>();
		public List<ColorPresetData> _ColorsList = new List<ColorPresetData>();

	}
	//	[System.Serializable]
	public string PackageName;
	public string PackageType;
	public float Mini;
	public float Maxi;
	public List<_raceData> RacesList = new List<_raceData>();
	public List<string> ExportsPathList = new List<string>();
	public List<_overlayData> OverlaysList = new List<_overlayData>();
	public List<_slotData> SlotsList = new List<_slotData>();
	public List<GameObject> ModelsList = new List<GameObject>();
}
