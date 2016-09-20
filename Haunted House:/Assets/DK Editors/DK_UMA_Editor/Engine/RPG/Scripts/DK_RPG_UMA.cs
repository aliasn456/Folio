using UnityEngine;
# if Editor
using UnityEditor;
# endif
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

public class DK_RPG_UMA : MonoBehaviour {
	public string Name = "";
	public string Gender = "";
	public string Race = "";
	public DKRaceData RaceData;
	public string Weight = "";

	public bool IsPlayer = true;


	#region avatar Meshs and Textures
	[System.Serializable]
	public class AvatarData {

		public int HeadIndex = 0;
		public int TorsoIndex = 0;
		public Color SkinColor;
		public Color HairColor;
		public Color EyeColor;


		#region Face
		[System.Serializable]
		public class FaceData {
			#region Head
			[System.Serializable]
			public class HeadData {
				public DKSlotData Slot;
				public DKOverlayData Overlay;
				public Color Color;
				public DKOverlayData Tattoo;
				public Color TattooColor;
				public DKOverlayData Makeup;
				public Color MakeupColor;
				public DKOverlayData Optional;
				public Color OptionalColor;
			}
			public HeadData _Head = new HeadData();
			#endregion Head

			#region FaceHair
			[System.Serializable]
			public class FaceHairData {
				public DKOverlayData EyeBrows;
				public Color EyeBrowsColor;

				[System.Serializable]
				public class BeardOvOnlyData {
					public DKOverlayData Beard1;
					public Color Beard1Color;
					public DKOverlayData Beard2;
					public Color Beard2Color;
					public DKOverlayData Beard3;
					public Color Beard3Color;
				}
				public BeardOvOnlyData _BeardOverlayOnly = new BeardOvOnlyData();

				[System.Serializable]
				public class BeardSlotOnlyData {
					public DKSlotData Slot;
					public DKOverlayData Overlay;
					public Color Color;

				}
				public BeardSlotOnlyData _BeardSlotOnly = new BeardSlotOnlyData();
			}
			public FaceHairData _FaceHair = new FaceHairData();
			#endregion FaceHair

			#region Eyes
			[System.Serializable]
			public class EyesData {
				public DKSlotData Slot;
				public DKOverlayData Overlay;
				public Color Color;
				public DKOverlayData Adjust;
				public Color AdjustColor;
				public DKOverlayData Optional;
				public Color OptionalColor;
			}
			public EyesData _Eyes = new EyesData();
			#endregion Eyes

			#region EyeLash
			[System.Serializable]
			public class EyeLashData {
				public DKSlotData Slot;
				public DKOverlayData Overlay;
				public Color Color;
			}
			public EyeLashData _EyeLash = new EyeLashData();
			#endregion EyeLash

			#region Lids
			[System.Serializable]
			public class EyeLidsData {
				public DKSlotData Slot;
				public DKOverlayData Overlay;
				public Color Color;
				public DKOverlayData Tattoo;
				public Color TattooColor;
				public DKOverlayData Makeup;
				public Color MakeupColor;
				public DKOverlayData Optional;
				public Color OptionalColor;
			}
			public EyeLidsData _EyeLids = new EyeLidsData();
			#endregion Lids

			#region Ears
			[System.Serializable]
			public class EarsData {
				public DKSlotData Slot;
				public DKOverlayData Overlay;
				public Color Color;
				public DKOverlayData Tattoo;
				public Color TattooColor;
				public DKOverlayData Makeup;
				public Color MakeupColor;
				public DKOverlayData Optional;
				public Color OptionalColor;

			}
			public EarsData _Ears = new EarsData();
			#endregion Ears

			#region Nose
			[System.Serializable]
			public class NoseData {
				public DKSlotData Slot;
				public DKOverlayData Overlay;
				public Color Color;
				public DKOverlayData Tattoo;
				public Color TattooColor;
				public DKOverlayData Makeup;
				public Color MakeupColor;
				public DKOverlayData Optional;
				public Color OptionalColor;

			}
			public NoseData _Nose = new NoseData();
			#endregion Nose

			#region Mouth
			[System.Serializable]
			public class MouthData {
				public DKSlotData Slot;
				public DKOverlayData Overlay;
				public Color Color;
				public DKOverlayData Lips;
				public Color LipsColor;
				public DKOverlayData Tattoo;
				public Color TattooColor;
				public DKOverlayData Makeup;
				public Color MakeupColor;
				public DKOverlayData Optional;
				public Color OptionalColor;

				[System.Serializable]
				public class InnerMouthData {
					public DKSlotData Slot;
					public DKOverlayData Overlay;
					public Color color;

				}
				public InnerMouthData _InnerMouth = new InnerMouthData();

			}
			public MouthData _Mouth = new MouthData();
			#endregion Mouth
		} 
		public FaceData _Face = new FaceData();
		#endregion Face

		#region Hair
		[System.Serializable]
		public class HairData {

			[System.Serializable]
			public class OvOnlyData {
				public DKOverlayData Overlay;
				public Color Color;
			}
			public OvOnlyData _OverlayOnly = new OvOnlyData();

			[System.Serializable]
			public class SlotOnlyData {
				public DKSlotData Slot;
				public DKOverlayData Overlay;
				public Color Color;

				[System.Serializable]
				public class HairModuleData {
					public DKSlotData Slot;
					public DKOverlayData Overlay;
					public Color Color;
				}
				public HairModuleData _HairModule = new HairModuleData();
			}
			public SlotOnlyData _SlotOnly = new SlotOnlyData();
		} 
		public HairData _Hair = new HairData();
		#endregion Hair

		#region Body
		[System.Serializable]
		public class BodyData {
			[System.Serializable]
			public class TorsoData {
				public DKSlotData Slot;
				public DKOverlayData Overlay;
				public Color Color;
				public DKOverlayData Tattoo;
				public Color TattooColor;
				public DKOverlayData Makeup;
				public Color MakeupColor;
				public DKOverlayData Optional;
				public Color OptionalColor;
			} 
			public TorsoData _Torso = new TorsoData();


			[System.Serializable]
			public class HandsData {
				public DKSlotData Slot;
				public DKOverlayData Overlay;
				public Color Color;
				public DKOverlayData Tattoo;
				public Color TattooColor;
				public DKOverlayData Makeup;
				public Color MakeupColor;
				public DKOverlayData Optional;
				public Color OptionalColor;
			} 
			public HandsData _Hands = new HandsData();

			[System.Serializable]
			public class LegsData {
				public DKSlotData Slot;
				public DKOverlayData Overlay;
				public Color Color;
				public DKOverlayData Tattoo;
				public Color TattooColor;
				public DKOverlayData Makeup;
				public Color MakeupColor;
				public DKOverlayData Optional;
				public Color OptionalColor;
			} 
			public LegsData _Legs = new LegsData();

			[System.Serializable]
			public class FeetData {
				public DKSlotData Slot;
				public DKOverlayData Overlay;
				public Color Color;
				public DKOverlayData Tattoo;
				public Color TattooColor;
				public DKOverlayData Makeup;
				public Color MakeupColor;
				public DKOverlayData Optional;
				public Color OptionalColor;
			} 
			public FeetData _Feet = new FeetData();

			[System.Serializable]
			public class UnderwearData {
				public DKSlotData Slot;
				public DKOverlayData Overlay;
				public Color Color;
				public DKOverlayData Optional;
			} 
			public UnderwearData _Underwear = new UnderwearData();
		} 
		public BodyData _Body = new BodyData();
		#endregion Body

	} 
	public AvatarData _Avatar = new AvatarData();
	#endregion avatar Meshs and Textures

	#region Equipment
	[System.Serializable]
	public class EquipmentData {

		[System.Serializable]
		public class HeadData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public HeadData _Head = new HeadData();

		[System.Serializable]
		public class ShoulderData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public ShoulderData _Shoulder = new ShoulderData();

		[System.Serializable]
		public class TorsoData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public TorsoData _Torso = new TorsoData();
		
		[System.Serializable]
		public class HandsData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public HandsData _Hands = new HandsData();
		
		[System.Serializable]
		public class LegsData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public LegsData _Legs = new LegsData();
		
		[System.Serializable]
		public class FeetData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public FeetData _Feet = new FeetData();

		[System.Serializable]
		public class ArmBandData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public ArmBandData _ArmBand = new ArmBandData();

		[System.Serializable]
		public class WristData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public WristData _Wrist = new WristData();

		[System.Serializable]
		public class LegBandData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public LegBandData _LegBand = new LegBandData();

		[System.Serializable]
		public class CollarData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public CollarData _Collar = new CollarData();

		[System.Serializable]
		public class RingData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public RingData _RingLeft = new RingData();
		public RingData _RingRight = new RingData();

		[System.Serializable]
		public class BeltData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public BeltData _Belt = new BeltData();

		[System.Serializable]
		public class CloakData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public CloakData _Cloak = new CloakData();

		[System.Serializable]
		public class BackpackData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public BackpackData _Backpack = new BackpackData();

		[System.Serializable]
		public class LeftHandData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public LeftHandData _LeftHand = new LeftHandData();

		[System.Serializable]
		public class RightHandData {
			public DKSlotData Slot;
			public DKOverlayData Overlay;
			public Color Color;
			public DKOverlayData Optional;
			public Color OptionalColor;
		} 
		public RightHandData _RightHand = new RightHandData();
	} 
	public EquipmentData _Equipment = new EquipmentData();
	#endregion Equipment
	public List<DKOverlayData> TmpTorsoOverLayList = new List<DKOverlayData>();

	void OnEnable (){
		CapsuleCollider _CapsuleCollider = this.gameObject.GetComponent<CapsuleCollider>();
		_CapsuleCollider.material = GameObject.Find ("DK_UMA").GetComponent<DKUMA_Variables>().DK_UMA_Collider_Material;
		_CapsuleCollider.material.name = "DK_UMA_Collider_Material";
	}

}
