using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DKOverlayData : ScriptableObject
{
    public string overlayName;
    [System.NonSerialized]
    public int listID;

    public Color color = new Color(1, 1, 1, 1);
    public Rect rect;
    public Texture2D[] textureList;
    public Color32[] channelMask;
    public Color32[] channelAdditiveMask;
    [System.NonSerialized]
    public DKUMAData umaData;
	public UMA.OverlayData _UMA;
	public string[] tags;

	public bool Active = true;
	public bool Elem = false;
	public DK_SlotsAnatomyElement Place;
	public List<string> Race = new List<string>();
	public string Gender;
	public string OverlayType;
	public string WearWeight;
	public bool Default = false;
	public List<DKSlotData> LinkedToSlot = new List<DKSlotData>();
	public List<ColorPresetData> ColorPresets = new List<ColorPresetData>();



    public DKOverlayData Duplicate()
    {
        DKOverlayData tempOverlay = CreateInstance<DKOverlayData>();
        tempOverlay.overlayName = overlayName;
        tempOverlay.listID = listID;
        tempOverlay.color = color;
        tempOverlay.rect = rect;
        tempOverlay.textureList = new Texture2D[textureList.Length];
        for (int i = 0; i < textureList.Length; i++)
        {
            tempOverlay.textureList[i] = textureList[i];
        }

        return tempOverlay;
    }

    public DKOverlayData()
    {

    }

    public bool useAdvancedMasks { get { return channelMask != null && channelMask.Length > 0; } }
    public DKOverlayData(DKOverlayLibrary _overlayLibrary, string elementName)
    {

        DKOverlayData source;
        if (!_overlayLibrary.overlayDictionary.TryGetValue(elementName, out source))
        {
            Debug.LogError("Unable to find DKOverlayData " + elementName);
            this.overlayName = elementName;
            return;
        }

        this.overlayName = source.overlayName;
        this.listID = source.listID;
        this.color = new Color(source.color.r, source.color.g, source.color.b, color.a);
        this.rect = source.rect;
        this.textureList = new Texture2D[source.textureList.Length];
        for (int i = 0; i < textureList.Length; i++)
        {
            this.textureList[i] = source.textureList[i];
        }
    }


    public void SetColor(int overlay, Color32 color)
    {
        if (useAdvancedMasks)
        {
            channelMask[overlay] = color;
        }
        else if (overlay == 0)
        {
            this.color = color;
        }
        else
        {
            AllocateAdvancedMasks();
            channelMask[overlay] = color;
        }
        if (umaData != null)
        {
            umaData.Dirty(false, true, false);
        }
    }

    public void SetAdditive(int overlay, Color32 color)
    {
        if (!useAdvancedMasks)
        {
            AllocateAdvancedMasks();
        }
        channelAdditiveMask[overlay] = color;
        if (umaData != null)
        {
            umaData.Dirty(false, true, false);
        }
    }

    private void AllocateAdvancedMasks()
    {
        int channels = umaData != null ? umaData.umaGenerator.textureNameList.Length : 2;
        channelMask = new Color32[channels];
        channelAdditiveMask = new Color32[channels];
        for (int i = 0; i < channels; i++)
        {
            channelMask[i] = new Color32(255, 255, 255, 255);
            channelAdditiveMask[i] = new Color32(0, 0, 0, 0);
        }
        channelMask[0] = color;

    }

}