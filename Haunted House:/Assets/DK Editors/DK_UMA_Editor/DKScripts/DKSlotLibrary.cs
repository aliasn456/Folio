using UnityEngine;
using System.Collections.Generic;

public class DKSlotLibrary : MonoBehaviour {
	public DKSlotData[] slotElementList = new DKSlotData[0];
	public Dictionary<string,DKSlotData> slotDictionary = new Dictionary<string,DKSlotData>();
	
	// modified by DK
	public void Awake() {
		UpdateDictionary();
	}
	// end modified by DK


	public void UpdateDictionary(){
		slotDictionary.Clear();
		for(int i = 0; i < slotElementList.Length; i++){			
			if(slotElementList[i]){	
				if(!slotDictionary.ContainsKey(slotElementList[i].slotName)){
					slotElementList[i].listID = i;
					slotDictionary.Add(slotElementList[i].slotName,slotElementList[i]);	
				}
			}
		}
	}
	
	public void AddSlot(string name, DKSlotData slot)
	{
		var list = new DKSlotData[slotElementList.Length + 1];
		for (int i = 0; i < slotElementList.Length; i++)
		{
			if (slotElementList[i].slotName == name)
			{
				slotElementList[i] = slot;
				return;
			}
			list[i] = slotElementList[i];
		}
		list[list.Length - 1] = slot;
		slotElementList = list;
		slotDictionary.Add(name, slot);
	}


	
	public DKSlotData InstantiateSlot(string name){
		DKSlotData source;
		if (!slotDictionary.TryGetValue(name, out source))
		{
		//	Debug.LogError("Unable to find " + name+" : The slot is not present in the current DK Slots Library. If you are converting a UMA avatar to DK UMA, you need to convert the UMA slot, set it up then add it to the Library.");
			return null;
		}else{
			return source.Duplicate();
		}
	}


	public DKSlotData InstantiateSlot(string name,List<DKOverlayData> overlayList){
		DKSlotData source;
		if (!slotDictionary.TryGetValue(name, out source))
		{
			Debug.LogError("Unable to find " + name);
			return null;
		}else{
			source = source.Duplicate();
			source.overlayList = overlayList;
			return source;
		}
	}
}