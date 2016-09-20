using UnityEngine;
using System.Collections.Generic;

public class DKOverlayLibrary : MonoBehaviour {
    public DKOverlayData[] overlayElementList = new DKOverlayData[0];
	public Dictionary<string,DKOverlayData> overlayDictionary = new Dictionary<string,DKOverlayData>();
	
	public int scaleAdjust = 1;
	public bool readWrite = false;
	public bool compress = false;
	
	// modified by DK
	public void Awake() {
		UpdateDictionary();
	}
	// end modified by DK
	public void UpdateDictionary(){
		overlayDictionary.Clear();
		for(int i = 0; i < overlayElementList.Length; i++){			
			if(overlayElementList[i]){				
				if(!overlayDictionary.ContainsKey(overlayElementList[i].overlayName)){
					overlayElementList[i].listID = i;
					overlayDictionary.Add(overlayElementList[i].overlayName,overlayElementList[i]);	
				}
			}
		}
	}

    public void AddOverlay(string name, DKOverlayData overlay)
    {
        var list = new DKOverlayData[overlayElementList.Length + 1];
        for (int i = 0; i < overlayElementList.Length; i++)
        {
            if (overlayElementList[i].overlayName == name)
            {
                overlayElementList[i] = overlay;
                return;
            }
            list[i] = overlayElementList[i];
        }
        list[list.Length - 1] = overlay;
        overlayElementList = list;
        overlayDictionary.Add(name, overlay);
    }
	
	public DKOverlayData GetOverlay(string name){		
		return new DKOverlayData(this, name);
	}
	
	public DKOverlayData InstantiateOverlay(string name){
		DKOverlayData source;
        if (!overlayDictionary.TryGetValue(name, out source))
        {
			Debug.LogError("Unable to find " +name+" in '"+overlayDictionary.ToString());
			return null;
        }else{
			return source.Duplicate();
		}
	}
	
	public DKOverlayData InstantiateOverlay(string name, Color color){
		DKOverlayData source;
        if (!overlayDictionary.TryGetValue(name, out source))
        {
			Debug.LogError("Unable to find " +name+" in '"+overlayDictionary.ToString());
			return null;
        }else{
			source = source.Duplicate();
			source.color = color;
			return source;
		}
	}
}