using UnityEngine;
using System;
using System.Collections.Generic;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

[Serializable]
public class DKRaceData : ScriptableObject {
    public string raceName;
    public GameObject racePrefab;
//	public UmaTPose TPose;
	public UMA.UmaTPose TPose;
	public string Race;
	public string Gender;
	public bool Active = true;
    public DKDnaConverterBehaviour[] dnaConverterList = new DKDnaConverterBehaviour[0];
    public String[] AnimatedBones = new string[0];



	[System.Serializable]
	public class Male{
		public DK_RPG_UMA_Generator.AvatarData _AvatarData;
		public DK_RPG_UMA_Generator.EquipmentData _EquipmentData;
	}
	public Male _Male = new Male();

	[System.Serializable]
	public class Female{
		public DK_RPG_UMA_Generator.AvatarData _AvatarData;
		public DK_RPG_UMA_Generator.EquipmentData _EquipmentData;
	}
	public Female _Female = new Female();

	[System.Serializable]
	public class DNAConverterData{
		public string Name;
		public float Value;
		public string Part;
		public string Part2;
		public string Type;
	}
	[System.Serializable]
	public class AgeDNAData{
		public string Name;
		public float Age;
		public float Modif;
		public string Part;
		public string Part2;
		public string Type;
	}
	[System.Serializable]
	public class ConverterData{
		public string Name;
		public float ValueMini;
		public float ValueMaxi;
		public string Part;
		public string Part2;
		public string Type;
		public string LinkedTo;
	}
	[System.Serializable]
	public class Conv{
		public string Name;
		public int Index;

	}
	[System.Serializable]
	public class ClampData{
		public int Type;
		public int Index;
		//	public float Value;
		public Conv Conv1;
		public Conv Conv2;
		public Conv Conv3;
		public Vector3 XYZ;
		public Quaternion WXYZ1;
		public Quaternion WXYZ2;

	}

	[System.Serializable]
	public class BoneData{
		public string Name;
		public bool Active;
		public bool UseClamps;
		public ClampData Clamp1;
		public ClampData Clamp2;
		public ClampData Clamp3;
		public bool IsClone;
		public int LinkedIndex;
		public string LinkedTo;
	}
	public List<DNAConverterData> DNAConverterDataList = new List<DNAConverterData>();
	public List<ConverterData> ConverterDataList = new List<ConverterData>();
	public List<BoneData> BoneDataList = new List<BoneData>();
	public List<ColorPresetData> ColorPresetDataList = new List<ColorPresetData>();
	public Dictionary<Type, System.Action<DKUMAData>> raceDictionary = new Dictionary<Type, System.Action<DKUMAData>>();
   



    void Awake()
    {
        UpdateDictionary();
    }

    public void UpdateDictionary()
    {
        raceDictionary.Clear();
        for (int i = 0; i < dnaConverterList.Length; i++)
        {
            if (dnaConverterList[i])
            {
                if (!raceDictionary.ContainsKey(dnaConverterList[i].DNAType))
                {
                    raceDictionary.Add(dnaConverterList[i].DNAType, dnaConverterList[i].ApplyDnaAction);
                }
            }
        }
    }

    internal void UpdateAnimatedBones()
    {
        throw new NotImplementedException();
    }
}