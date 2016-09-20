using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class DKSlotData : ScriptableObject
{
    public string slotName;
    [System.NonSerialized]
    public int listID = -1;

	public SkinnedMeshRenderer meshRenderer;
# if Editor
	public Texture2D Preview;
# endif
	public Material materialSample;
    public float overlayScale = 1.0f;
    public Transform[] umaBoneData;
	public DKDnaConverterBehaviour slotDNA;
   
	public DK_SlotsAnatomyElement Place;
	public UMA.SlotData _UMA;
	public List<string> Race = new List<string>();
//	public string Race;
	public string Gender = "";
	public string OverlayType = "";
	public bool Active = true;
	public bool Default = false;
	public bool Elem = false;
	public string WearWeight = "";
//	public bool IsMesh = false;
	public bool Replace = false;
	/*
	public bool HideMouth = false;
	public bool HideEars = false;
	public bool HideBeard = false;
	public bool HideHair = false;
	public bool HideHairModule = false;
	public bool HideShoulders = false;
	public bool HideLegs = false;
	public bool HideBelt = false;
	public bool HideArmBand = false;
	public bool HideWrist = false;
	*/
	[System.Serializable]
	public class HideData{
		public bool HideMouth = false;
		public bool HideEars = false;
		public bool HideBeard = false;
		public bool HideHair = false;
		public bool HideHairModule = false;
		public bool HideShoulders = false;
		public bool HideLegs = false;
		public bool HideBelt = false;
		public bool HideArmBand = false;
		public bool HideWrist = false;
		public bool HideCollar = false;
		public bool HideLegBand = false;
		public bool HideRing = false;
		public bool HideUnderwear = false;

	}
	public HideData _HideData = new HideData();

	public List<DKOverlayData> overlayList = new List<DKOverlayData>();
	public List<DKOverlayData> LinkedOverlayList = new List<DKOverlayData>();



	[System.Serializable]
	public class LegacyData{
		public bool HasLegacy = false;
		public List<DKSlotData> LegacyList = new List<DKSlotData>();
		public bool IsLegacy = false;
		public List<DKSlotData> ElderList = new List<DKSlotData>();
		public bool Replace = false;
		//	DK_SlotsAnatomyData Place;
	}
	public LegacyData _LegacyData = new LegacyData();



	public DKSlotData Duplicate()
    {
		DKSlotData tempSlotData = CreateInstance<DKSlotData>();

        tempSlotData.slotName = slotName;
        tempSlotData.listID = listID;
        tempSlotData.materialSample = materialSample;
        tempSlotData.overlayScale = overlayScale;
        tempSlotData.slotDNA = slotDNA;

        // All this data is passed as reference
        tempSlotData.meshRenderer = meshRenderer;

        tempSlotData.umaBoneData = umaBoneData;

        //Overlays are duplicated, to lose reference
        for (int i = 0; i < overlayList.Count; i++)
        {
            tempSlotData.overlayList.Add(overlayList[i].Duplicate());
        }

        return tempSlotData;
    }

    public DKSlotData()
    {

    }

    public DKSlotData(DKSlotLibrary _slotLibrary, string elementName)
    {
		DKSlotData source;
        if (!_slotLibrary.slotDictionary.TryGetValue(elementName, out source))
        {
# if Editor
			Debug.LogError("Unable to find DKSlotData " + elementName);
# endif
            this.slotName = elementName;
            return;
        }

        this.slotName = source.slotName;
        this.listID = source.listID;

        // All this data is passed as reference
        this.meshRenderer = source.meshRenderer;
        //this.shader = source.shader;
        this.materialSample = source.materialSample;
        this.overlayScale = source.overlayScale;
        this.umaBoneData = source.umaBoneData;
        this.slotDNA = source.slotDNA;

        //Overlays are duplicated, to lose reference
        for (int i = 0; i < source.overlayList.Count; i++)
        {
            this.overlayList.Add(source.overlayList[i].Duplicate());
        }
    }

	public DKSlotData(DKSlotLibrary _slotLibrary, string elementName, Color color)
        : this(_slotLibrary, elementName)
    {
        var source = _slotLibrary.slotDictionary[elementName];

        this.overlayList[0] = source.overlayList[0].Duplicate();
        this.overlayList[0].color = color;
    }

    internal bool RemoveOverlay(params string[] names)
    {
        bool changed = false;
        foreach (var name in names)
        {
            for (int i = 0; i < overlayList.Count; i++)
            {
                if (overlayList[i].overlayName == name)
                {
                    overlayList.RemoveAt(i);
                    changed = true;
                    break;
                }
            }
        }
        return changed;
    }

    internal bool SetOverlayColor(Color32 color, params string[] names)
    {
        bool changed = false;
        foreach (var name in names)
        {
            foreach (var overlay in overlayList)
            {
                if (overlay.overlayName == name)
                {
                    overlay.color = color;
                    changed = true;
                }
            }
        }
        return changed;
    }

    internal DKOverlayData GetOverlay(params string[] names)
    {
        foreach (var name in names)
        {
            foreach (var overlay in overlayList)
            {
                if (overlay.overlayName == name)
                {
                    return overlay;
                }
            }
        }
        return null;
    }
}
