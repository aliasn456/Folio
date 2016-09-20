using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
// using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public class DK_RPG_UMA_Generator : MonoBehaviour {
	#region Variables

	// Libraries
	public DKUMA_Variables _DKUMA_Variables;

	public DKSlotLibrary _DKSlotLibrary;
	public DKOverlayLibrary _DKOverlayLibrary;
	public DKRaceLibrary _DKRaceLibrary;

	public Transform _DK_UMA;

	public static bool AddToRPG = true;
	public bool Done = false;

	#region Lists
	#region avatar Meshs and Textures
	[System.Serializable]
	public class AvatarData {
		#region Face
		[System.Serializable]
		public class FaceData {
			#region Head
			[System.Serializable]
			public class HeadData {
				public List<DKSlotData> SlotList = new List<DKSlotData>();
				public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
				public List<DKOverlayData> TattooList = new List<DKOverlayData>();
				public List<DKOverlayData> MakeupList = new List<DKOverlayData>();
				public List<DKOverlayData> LipsList = new List<DKOverlayData>();
				public List<DKOverlayData> OptionalList = new List<DKOverlayData>();

				public void ClearAll(){
					SlotList.Clear();
					OverlayList.Clear();
					TattooList.Clear();
					MakeupList.Clear();
					OptionalList.Clear();

					Debug.Log ("Head Lists Cleared");
				}

				public void Select(){
					Debug.Log ("Test Select");
				}

				public void Assign(){
					Debug.Log ("Test Assign");
				}
			}
			public HeadData _Head = new HeadData();
			#endregion Head
			
			#region FaceHair
			[System.Serializable]
			public class FaceHairData {
				public List<DKOverlayData> EyeBrowsList = new List<DKOverlayData>();

				[System.Serializable]
				public class BeardOvOnlyData {
					public List<DKOverlayData> BeardList = new List<DKOverlayData>();
				}
				public BeardOvOnlyData _BeardOverlayOnly = new BeardOvOnlyData();
				
				[System.Serializable]
				public class BeardSlotOnlyData {
					public List<DKSlotData> SlotList = new List<DKSlotData>();
					public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
				}
				public BeardSlotOnlyData _BeardSlotOnly = new BeardSlotOnlyData();

			}
			public FaceHairData _FaceHair = new FaceHairData();
			#endregion FaceHair
			
			#region Eyes
			[System.Serializable]
			public class EyesData {
				public List<DKSlotData> SlotList = new List<DKSlotData>();
				public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
				public List<DKOverlayData> AdjustList = new List<DKOverlayData>();
				public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			}
			public EyesData _Eyes = new EyesData();
			#endregion Eyes
			
			#region EyeLash
			[System.Serializable]
			public class EyeLashData {
				public List<DKSlotData> SlotList = new List<DKSlotData>();
				public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			}
			public EyeLashData _EyeLash = new EyeLashData();
			#endregion EyeLash
			
			#region Lids
			[System.Serializable]
			public class EyeLidsData {
				public List<DKSlotData> SlotList = new List<DKSlotData>();
				public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
				public List<DKOverlayData> TattooList = new List<DKOverlayData>();
				public List<DKOverlayData> MakeupList = new List<DKOverlayData>();
				public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			}
			public EyeLidsData _EyeLids = new EyeLidsData();
			#endregion Lids
			
			#region Ears
			[System.Serializable]
			public class EarsData {
				public List<DKSlotData> SlotList = new List<DKSlotData>();
				public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
				public List<DKOverlayData> TattooList = new List<DKOverlayData>();
				public List<DKOverlayData> MakeupList = new List<DKOverlayData>();
				public List<DKOverlayData> OptionalList = new List<DKOverlayData>();

			}
			public EarsData _Ears = new EarsData();
			#endregion Ears
			
			#region Nose
			[System.Serializable]
			public class NoseData {
				public List<DKSlotData> SlotList = new List<DKSlotData>();
				public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
				public List<DKOverlayData> TattooList = new List<DKOverlayData>();
				public List<DKOverlayData> MakeupList = new List<DKOverlayData>();
				public List<DKOverlayData> OptionalList = new List<DKOverlayData>();

			}
			public NoseData _Nose = new NoseData();
			#endregion Nose
			
			#region Mouth
			[System.Serializable]
			public class MouthData {
				public List<DKSlotData> SlotList = new List<DKSlotData>();
				public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
				public List<DKOverlayData> TattooList = new List<DKOverlayData>();
				public List<DKOverlayData> LipsList = new List<DKOverlayData>();
				public List<DKOverlayData> MakeupList = new List<DKOverlayData>();
				public List<DKOverlayData> OptionalList = new List<DKOverlayData>();

				
				[System.Serializable]
				public class InnerMouthData {
					public List<DKSlotData> SlotList = new List<DKSlotData>();
					public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
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
				public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			}
			public OvOnlyData _OverlayOnly = new OvOnlyData();
			
			[System.Serializable]
			public class SlotOnlyData {
				public List<DKSlotData> SlotList = new List<DKSlotData>();
				public List<DKOverlayData> OverlayList = new List<DKOverlayData>();

				[System.Serializable]
				public class HairModuleData {
					public List<DKSlotData> SlotList = new List<DKSlotData>();
					public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
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
				public List<DKSlotData> SlotList = new List<DKSlotData>();
				public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
				public List<DKOverlayData> TattooList = new List<DKOverlayData>();
				public List<DKOverlayData> MakeupList = new List<DKOverlayData>();
				public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			} 
			public TorsoData _Torso = new TorsoData();
			
			[System.Serializable]
			public class HandsData {
				public List<DKSlotData> SlotList = new List<DKSlotData>();
				public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
				public List<DKOverlayData> TattooList = new List<DKOverlayData>();
				public List<DKOverlayData> MakeupList = new List<DKOverlayData>();
				public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			} 
			public HandsData _Hands = new HandsData();
			
			[System.Serializable]
			public class LegsData {
				public List<DKSlotData> SlotList = new List<DKSlotData>();
				public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
				public List<DKOverlayData> TattooList = new List<DKOverlayData>();
				public List<DKOverlayData> MakeupList = new List<DKOverlayData>();
				public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			} 
			public LegsData _Legs = new LegsData();
			
			[System.Serializable]
			public class FeetData {
				public List<DKSlotData> SlotList = new List<DKSlotData>();
				public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
				public List<DKOverlayData> TattooList = new List<DKOverlayData>();
				public List<DKOverlayData> MakeupList = new List<DKOverlayData>();
				public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			} 
			public FeetData _Feet = new FeetData();

			[System.Serializable]
			public class UnderwearData {
				public List<DKSlotData> SlotList = new List<DKSlotData>();
				public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
				public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			} 
			public UnderwearData _Underwear = new UnderwearData();
		} 
		public BodyData _Body = new BodyData();
		#endregion Body
		
	} 
	public AvatarData _MaleAvatar = new AvatarData();
	public AvatarData _FemaleAvatar = new AvatarData();

	#endregion avatar Meshs and Textures
	
	#region Equipment
	[System.Serializable]
	public class EquipmentData {
		
		[System.Serializable]
		public class HeadData {
			public List<DKSlotData> SlotList = new List<DKSlotData>();
			public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			public List<DKOverlayData> OverlayOnlyList = new List<DKOverlayData>();


		} 
		public HeadData _Head = new HeadData();
		
		[System.Serializable]
		public class ShoulderData {
			public List<DKSlotData> SlotList = new List<DKSlotData>();
			public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			public List<DKOverlayData> OverlayOnlyList = new List<DKOverlayData>();

		} 
		public ShoulderData _Shoulder = new ShoulderData();
		
		[System.Serializable]
		public class TorsoData {
			public List<DKSlotData> SlotList = new List<DKSlotData>();
			public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			public List<DKOverlayData> OverlayOnlyList = new List<DKOverlayData>();

		} 
		public TorsoData _Torso = new TorsoData();

		[System.Serializable]
		public class ArmbandData {
			public List<DKSlotData> SlotList = new List<DKSlotData>();
			public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			public List<DKOverlayData> OverlayOnlyList = new List<DKOverlayData>();
			
		} 
		public ArmbandData _Armband = new ArmbandData();

		[System.Serializable]
		public class LegBandData {
			public List<DKSlotData> SlotList = new List<DKSlotData>();
			public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			public List<DKOverlayData> OverlayOnlyList = new List<DKOverlayData>();
			
		} 
		public LegBandData _LegBand = new LegBandData();


		[System.Serializable]
		public class WristData {
			public List<DKSlotData> SlotList = new List<DKSlotData>();
			public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			public List<DKOverlayData> OverlayOnlyList = new List<DKOverlayData>();
			
		} 
		public WristData _Wrist = new WristData();

		[System.Serializable]
		public class HandsData {
			public List<DKSlotData> SlotList = new List<DKSlotData>();
			public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			public List<DKOverlayData> OverlayOnlyList = new List<DKOverlayData>();

		} 
		public HandsData _Hands = new HandsData();
		
		[System.Serializable]
		public class LegsData {
			public List<DKSlotData> SlotList = new List<DKSlotData>();
			public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			public List<DKOverlayData> OverlayOnlyList = new List<DKOverlayData>();

		} 
		public LegsData _Legs = new LegsData();
		
		[System.Serializable]
		public class FeetData {
			public List<DKSlotData> SlotList = new List<DKSlotData>();
			public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			public List<DKOverlayData> OverlayOnlyList = new List<DKOverlayData>();
		} 
		public FeetData _Feet = new FeetData();

		[System.Serializable]
		public class BeltData {
			public List<DKSlotData> SlotList = new List<DKSlotData>();
			public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
			public List<DKOverlayData> OverlayOnlyList = new List<DKOverlayData>();

		} 
		public BeltData _Belt = new BeltData();

		[System.Serializable]
		public class CloakData {
			public List<DKSlotData> SlotList = new List<DKSlotData>();
			public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
		} 
		public CloakData _Cloak = new CloakData();

		[System.Serializable]
		public class BackpackData {
			public List<DKSlotData> SlotList = new List<DKSlotData>();
			public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
		} 
		public BackpackData _Backpack = new BackpackData();

		[System.Serializable]
		public class LeftHandData {
			public List<DKSlotData> SlotList = new List<DKSlotData>();
			public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
		} 
		public LeftHandData _LeftHand = new LeftHandData();
		
		[System.Serializable]
		public class RightHandData {
			public List<DKSlotData> SlotList = new List<DKSlotData>();
			public List<DKOverlayData> OverlayList = new List<DKOverlayData>();
			public List<DKOverlayData> OptionalList = new List<DKOverlayData>();
		} 
		public RightHandData _RightHand = new RightHandData();
	} 
	public EquipmentData _MaleEquipment = new EquipmentData();
	public EquipmentData _FemaleEquipment = new EquipmentData();

	#endregion Equipment
	#endregion Lists

	DK_UMACrowd _DK_UMACrowd;

	#endregion Variables

	void Start () {}

	public void PopulateAllLists(){
		// Find _DK_UMA
		_DK_UMA = GameObject.Find ("DK_UMA").transform;
		if (_DK_UMA == null){
			_DK_UMA = new GameObject().transform;
			_DK_UMA.name = "DK_UMA";
		}
		_DK_UMACrowd = GameObject.Find ("DKUMACrowd").GetComponent<DK_UMACrowd>();

		// Find the DKUMA_Variables
		_DKUMA_Variables = _DK_UMA.GetComponent<DKUMA_Variables>();
		if ( _DKUMA_Variables == null ) _DKUMA_Variables = _DK_UMA.gameObject.AddComponent<DKUMA_Variables>() as DKUMA_Variables;
		_DK_UMACrowd.CleanLibraries ();
		PopulateLists();
	}

	public void PopulateLists(){
		_DKRaceLibrary = _DK_UMACrowd.raceLibrary;
		_DKSlotLibrary = _DK_UMACrowd.slotLibrary;
		_DKOverlayLibrary = _DK_UMACrowd.overlayLibrary;

		foreach ( DKRaceData race in _DKRaceLibrary.raceElementList ){
			_MaleAvatar = new AvatarData();
			_FemaleAvatar = new AvatarData();
			_MaleEquipment = new EquipmentData();
			_FemaleEquipment = new EquipmentData();

			// Find all the slots in the library
			foreach ( DKSlotData Slot in _DKSlotLibrary.slotElementList ){
				if ( Slot == null ) {
					Debug.LogError ( "The librarie reports a missing element, please use the 'Clean Lib' button of the 'Prepare DK Elements' window.");
				//	SlToRemoveList
				}
				else
				if ( Slot.Active && Slot._UMA != null && Slot.meshRenderer != null ){
					if ( Slot.materialSample == null ) Slot.materialSample = _DKUMA_Variables.DKSlotsList[0].materialSample;

					#region Populate Face Lists
					// Add the Head slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
						&& Slot.Place.name ==  "Head" && Slot.OverlayType == "Face" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race)  )
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Face._Head.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Face._Head.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Face._Head.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Face._Head.SlotList.Add (Slot);
					}
					// Add the Beard slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
						&& Slot.Place.name ==  "Beard" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race) )
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Face._FaceHair._BeardSlotOnly.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Face._FaceHair._BeardSlotOnly.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Face._FaceHair._BeardSlotOnly.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Face._FaceHair._BeardSlotOnly.SlotList.Add (Slot);

					}
					// Add the Eyes slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "Eyes" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Face._Eyes.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Face._Eyes.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Face._Eyes.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Face._Eyes.SlotList.Add (Slot);

					}
					// Add the Eyelash slots to the list
					if (Slot.Place != null
					    && Slot.Place.name ==  "Eyelash" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
					//	Debug.Log ("Eyelash test");
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Face._EyeLash.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Face._EyeLash.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Face._EyeLash.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Face._EyeLash.SlotList.Add (Slot);
						
					}
					// Add the eyelid slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "eyelid" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Face._EyeLids.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Face._EyeLids.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Face._EyeLids.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Face._EyeLids.SlotList.Add (Slot);
					}

					// Add the Ears slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "Ears" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Face._Ears.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Face._Ears.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Face._Ears.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Face._Ears.SlotList.Add (Slot);
						
					}
					// Add the Nose slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "Nose" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Face._Nose.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Face._Nose.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Face._Nose.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Face._Nose.SlotList.Add (Slot);
						
					}
					// Add the Mouth slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "Mouth" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Face._Mouth.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Face._Mouth.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Face._Mouth.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Face._Mouth.SlotList.Add (Slot);
						
					}
					// Add the InnerMouth slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "InnerMouth" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Face._Mouth._InnerMouth.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Face._Mouth._InnerMouth.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Face._Mouth._InnerMouth.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Face._Mouth._InnerMouth.SlotList.Add (Slot);
						
					}
					#endregion Populate Face Lists

					#region Populate Hair Lists
					// Add the Hair slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "Hair" && Slot.Elem == false  && Slot.OverlayType == "Hair"
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Hair._SlotOnly.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Hair._SlotOnly.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Hair._SlotOnly.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Hair._SlotOnly.SlotList.Add (Slot);
						
					}
					// Add the Hair Module slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "Hair_Module" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Hair._SlotOnly._HairModule.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Hair._SlotOnly._HairModule.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Hair._SlotOnly._HairModule.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Hair._SlotOnly._HairModule.SlotList.Add (Slot);
						
					}
					#endregion Populate Hair Lists

					#region Populate Body Lists
					// Add the Torso slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "Torso" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Body._Torso.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Body._Torso.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Torso.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Body._Torso.SlotList.Add (Slot);
						
					}
					// Add the Hands slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name == "Hands" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Body._Hands.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Body._Hands.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Hands.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Body._Hands.SlotList.Add (Slot);
						
					}
					// Add the Legs slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "Legs" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Body._Legs.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Body._Legs.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Legs.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Body._Legs.SlotList.Add (Slot);
						
					}
					// Add the Feet slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "Feet" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Body._Feet.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Body._Feet.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Feet.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Body._Feet.SlotList.Add (Slot);
						
					}
					// Add the Underwear slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "Underwear" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleAvatar._Body._Underwear.SlotList.Contains(Slot) == false  ) 
							_MaleAvatar._Body._Underwear.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Underwear.SlotList.Contains(Slot) == false  ) 
							_FemaleAvatar._Body._Underwear.SlotList.Add (Slot);
					}
					#endregion Populate Body Lists

					#region Populate Equipment Lists
					// Add the Head slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "HeadWear" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleEquipment._Head.SlotList.Contains(Slot) == false  ) 
							_MaleEquipment._Head.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleEquipment._Head.SlotList.Contains(Slot) == false  ) 
							_FemaleEquipment._Head.SlotList.Add (Slot);
						
					}

					// Add the Shoulder slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "ShoulderWear" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleEquipment._Shoulder.SlotList.Contains(Slot) == false  ) 
							_MaleEquipment._Shoulder.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleEquipment._Shoulder.SlotList.Contains(Slot) == false  ) 
							_FemaleEquipment._Shoulder.SlotList.Add (Slot);
					}

					// Add the WristWear slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "WristWear" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleEquipment._Wrist.SlotList.Contains(Slot) == false  ) 
							_MaleEquipment._Wrist.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleEquipment._Wrist.SlotList.Contains(Slot) == false  ) 
							_FemaleEquipment._Wrist.SlotList.Add (Slot);
					}

					// Add the ArmbandWear slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "ArmbandWear" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleEquipment._Armband.SlotList.Contains(Slot) == false  ) 
							_MaleEquipment._Armband.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleEquipment._Armband.SlotList.Contains(Slot) == false  ) 
							_FemaleEquipment._Armband.SlotList.Add (Slot);
					}

					// Add the Torso slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "TorsoWear" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleEquipment._Torso.SlotList.Contains(Slot) == false  ) 
							_MaleEquipment._Torso.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleEquipment._Torso.SlotList.Contains(Slot) == false  ) 
							_FemaleEquipment._Torso.SlotList.Add (Slot);
						
					}

					// Add the Hands slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "HandWear" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleEquipment._Hands.SlotList.Contains(Slot) == false  ) 
							_MaleEquipment._Hands.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleEquipment._Hands.SlotList.Contains(Slot) == false  ) 
							_FemaleEquipment._Hands.SlotList.Add (Slot);
						
					}
					// Add the Legs slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "LegsWear" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleEquipment._Legs.SlotList.Contains(Slot) == false  ) 
							_MaleEquipment._Legs.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleEquipment._Legs.SlotList.Contains(Slot) == false  ) 
							_FemaleEquipment._Legs.SlotList.Add (Slot);
						
					}
					// Add the Feet slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "FeetWear" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleEquipment._Feet.SlotList.Contains(Slot) == false  ) 
							_MaleEquipment._Feet.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleEquipment._Feet.SlotList.Contains(Slot) == false  ) 
							_FemaleEquipment._Feet.SlotList.Add (Slot);
						
					}
					// Add the Belt slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "BeltWear" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleEquipment._Belt.SlotList.Contains(Slot) == false  ) 
							_MaleEquipment._Belt.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleEquipment._Belt.SlotList.Contains(Slot) == false  ) 
							_FemaleEquipment._Belt.SlotList.Add (Slot);
						
					}
					// Add the Cloak slots to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "CloakWear" 
					    && Slot._LegacyData.IsLegacy == false
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleEquipment._Cloak.SlotList.Contains(Slot) == false  ) 
							_MaleEquipment._Cloak.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleEquipment._Cloak.SlotList.Contains(Slot) == false  ) 
							_FemaleEquipment._Cloak.SlotList.Add (Slot);
						
					}
					// Add the HandledLeft Slot to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "HandledLeft" 
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleEquipment._LeftHand.SlotList.Contains(Slot) == false  ) 
							_MaleEquipment._LeftHand.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleEquipment._LeftHand.SlotList.Contains(Slot) == false  ) 
							_FemaleEquipment._LeftHand.SlotList.Add (Slot);
						
					}
					// Add the HandledRight Overlay to the list
					if (Slot.Place != null && Slot.OverlayType != null
					    && Slot.Place.name ==  "HandledRight" 
					    && Slot.Race.Contains(race.Race))
					{
						if ((Slot.Gender == "Both" || Slot.Gender == "Male" )
						    && _MaleEquipment._RightHand.SlotList.Contains(Slot) == false  ) 
							_MaleEquipment._RightHand.SlotList.Add (Slot);
						if ((Slot.Gender == "Both" || Slot.Gender == "Female" ) 
						    && _FemaleEquipment._RightHand.SlotList.Contains(Slot) == false  ) 
							_FemaleEquipment._RightHand.SlotList.Add (Slot);
						
					}
					#endregion Populate Equipment Lists
				}
			}

			// Find all the Overlays in the library
			foreach ( DKOverlayData Overlay in _DKOverlayLibrary.overlayElementList ){
				if ( Overlay == null ) {
					Debug.LogError ( "The librarie reports a missing element, please use the 'Clean Lib' button of the 'Prepare DK Elements' window.");
				}
				else
				if ( Overlay.Active && Overlay.textureList[0] != null ){
					#region Populate Face Lists
					// Add the Head Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Head" && Overlay.OverlayType == "Face"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Face._Head.OverlayList.Contains(Overlay) == false  ) 
							_MaleAvatar._Face._Head.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Face._Head.OverlayList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Face._Head.OverlayList.Add (Overlay);
							
					}	
					// Add the Tatoo Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Head" && Overlay.OverlayType == "Tatoo"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Face._Head.TattooList.Contains(Overlay) == false  ) 
							_MaleAvatar._Face._Head.TattooList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Face._Head.TattooList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Face._Head.TattooList.Add (Overlay);
						
					}	
					// Add the Makeup Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Head" && Overlay.OverlayType == "Makeup"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Face._Head.MakeupList.Contains(Overlay) == false  ) 
							_MaleAvatar._Face._Head.MakeupList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Face._Head.MakeupList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Face._Head.MakeupList.Add (Overlay);
						
					}
					// Add the Lips Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Head" && Overlay.OverlayType == "Lips"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Face._Head.LipsList.Contains(Overlay) == false  ) 
							_MaleAvatar._Face._Head.LipsList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Face._Head.LipsList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Face._Head.LipsList.Add (Overlay);
						
					}
					// Add the Beard slot Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Beard" && Overlay.OverlayType == "Beard"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Face._FaceHair._BeardSlotOnly.OverlayList.Contains(Overlay) == false  ) 
							_MaleAvatar._Face._FaceHair._BeardSlotOnly.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Face._FaceHair._BeardSlotOnly.OverlayList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Face._FaceHair._BeardSlotOnly.OverlayList.Add (Overlay);
						
					}
					// Add the Beard Overlay Only to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Head" && Overlay.OverlayType == "Beard"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Face._FaceHair._BeardOverlayOnly.BeardList.Contains(Overlay) == false  ) 
							_MaleAvatar._Face._FaceHair._BeardOverlayOnly.BeardList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Face._FaceHair._BeardOverlayOnly.BeardList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Face._FaceHair._BeardOverlayOnly.BeardList.Add (Overlay);
						
					}
					// Add the Eye brow Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Head" && Overlay.OverlayType == "Eyebrow"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Face._FaceHair.EyeBrowsList.Contains(Overlay) == false  ) 
							_MaleAvatar._Face._FaceHair.EyeBrowsList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Face._FaceHair.EyeBrowsList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Face._FaceHair.EyeBrowsList.Add (Overlay);
						
					}
					// Add the Eyes Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null && Overlay.name != null
					    && Overlay.Place.name ==  "Eyes" && Overlay.OverlayType == "Eyes" 
					    && Overlay.name.Contains("Adjust") == false
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Face._Eyes.OverlayList.Contains(Overlay) == false  ) 
							_MaleAvatar._Face._Eyes.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Face._Eyes.OverlayList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Face._Eyes.OverlayList.Add (Overlay);
						
					}
					// Add the Eyes Overlay Adjust to the list
					if (Overlay.Place != null && Overlay.OverlayType != null && Overlay.name != null
					    && Overlay.Place.name ==  "Eyes" && Overlay.OverlayType == "Eyes" 
					    && Overlay.name.Contains("Adjust") == true
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Face._Eyes.AdjustList.Contains(Overlay) == false  ) 
							_MaleAvatar._Face._Eyes.AdjustList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Face._Eyes.AdjustList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Face._Eyes.AdjustList.Add (Overlay);
						
					}

					// Add the Eyelash Overlay Adjust to the list
					if (Overlay.Place != null && Overlay.OverlayType == null
					    && Overlay.Place.name ==  "Eyelash"
					    && Overlay.Race.Contains(race.Race) )
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Face._EyeLash.OverlayList.Contains(Overlay) == false  ) 
							_MaleAvatar._Face._EyeLash.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Face._EyeLash.OverlayList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Face._EyeLash.OverlayList.Add (Overlay);
						
					}
					// Add the InnerMouth Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "InnerMouth"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Face._Mouth._InnerMouth.OverlayList.Contains(Overlay) == false  ) 
							_MaleAvatar._Face._Mouth._InnerMouth.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Face._Mouth._InnerMouth.OverlayList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Face._Mouth._InnerMouth.OverlayList.Add (Overlay);
						
					}

					#endregion Populate Face Lists

					#region Populate Hair Lists
					// Add the Hair Overlay Only to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Hair" && Overlay.OverlayType == "Hair"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Hair._OverlayOnly.OverlayList.Contains(Overlay) == false  ) 
							_MaleAvatar._Hair._OverlayOnly.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Hair._OverlayOnly.OverlayList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Hair._OverlayOnly.OverlayList.Add (Overlay);
						
					}
					// Add the Hair slot Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Hair" && Overlay.OverlayType == "Hair" && Overlay.Elem == false
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Hair._SlotOnly.OverlayList.Contains(Overlay) == false  ) 
							_MaleAvatar._Hair._SlotOnly.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Hair._SlotOnly.OverlayList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Hair._SlotOnly.OverlayList.Add (Overlay);
						
					}
					// Add the Hair module slot Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Hair_Module" && Overlay.OverlayType == "Hair" && Overlay.Elem == true
						&& Overlay.Race.Contains(race.Race)){
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Hair._SlotOnly._HairModule.OverlayList.Contains(Overlay) == false  ) 
							_MaleAvatar._Hair._SlotOnly._HairModule.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Hair._SlotOnly._HairModule.OverlayList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Hair._SlotOnly._HairModule.OverlayList.Add (Overlay);
						
					}
					#endregion Populate Hair Lists

					#region Populate Body Lists
					// Add the Torso Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Torso" && Overlay.OverlayType == "Flesh"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Body._Torso.OverlayList.Contains(Overlay) == false  ) 
							_MaleAvatar._Body._Torso.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Torso.OverlayList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Body._Torso.OverlayList.Add (Overlay);
						
					}
					// Add the Torso Tatoo Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Torso" && Overlay.OverlayType == "Tatoo"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Body._Torso.TattooList.Contains(Overlay) == false  ) 
							_MaleAvatar._Body._Torso.TattooList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Torso.TattooList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Body._Torso.TattooList.Add (Overlay);
						
					}	
					// Add the Torso Makeup Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Torso" && Overlay.OverlayType == "Makeup"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Body._Torso.MakeupList.Contains(Overlay) == false  ) 
							_MaleAvatar._Body._Torso.MakeupList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Torso.MakeupList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Body._Torso.MakeupList.Add (Overlay);
						
					}
					// Add the Hands Tatoo Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Hands" && Overlay.OverlayType == "Tatoo"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Body._Hands.TattooList.Contains(Overlay) == false  ) 
							_MaleAvatar._Body._Hands.TattooList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Hands.TattooList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Body._Hands.TattooList.Add (Overlay);
						
					}	
					// Add the Hands Makeup Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Hands" && Overlay.OverlayType == "Makeup"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Body._Hands.MakeupList.Contains(Overlay) == false  ) 
							_MaleAvatar._Body._Hands.MakeupList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Hands.MakeupList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Body._Hands.MakeupList.Add (Overlay);
						
					}
					// Add the Legs Tatoo Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Legs" && Overlay.OverlayType == "Tatoo"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Body._Legs.TattooList.Contains(Overlay) == false  ) 
							_MaleAvatar._Body._Legs.TattooList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Legs.TattooList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Body._Legs.TattooList.Add (Overlay);
						
					}	
					// Add the Legs Makeup Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Hands" && Overlay.OverlayType == "Makeup"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Body._Legs.MakeupList.Contains(Overlay) == false  ) 
							_MaleAvatar._Body._Legs.MakeupList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Legs.MakeupList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Body._Legs.MakeupList.Add (Overlay);
						
					}
					// Add the Feet Tatoo Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Feet" && Overlay.OverlayType == "Tatoo"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Body._Feet.TattooList.Contains(Overlay) == false  ) 
							_MaleAvatar._Body._Feet.TattooList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Feet.TattooList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Body._Feet.TattooList.Add (Overlay);
						
					}	
					// Add the Feet Makeup Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Feet" && Overlay.OverlayType == "Makeup"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Body._Feet.MakeupList.Contains(Overlay) == false  ) 
							_MaleAvatar._Body._Feet.MakeupList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Feet.MakeupList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Body._Feet.MakeupList.Add (Overlay);
						
					}
					// Add the Underwear slots to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.OverlayType ==  "Underwear" 
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleAvatar._Body._Underwear.OverlayList.Contains(Overlay) == false  ) 
							_MaleAvatar._Body._Underwear.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleAvatar._Body._Underwear.OverlayList.Contains(Overlay) == false  ) 
							_FemaleAvatar._Body._Underwear.OverlayList.Add (Overlay);
					}
					#endregion Populate Body Lists

					#region Populate Wear Lists
					// Add the Head Wear Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "HeadWear" && Overlay.OverlayType == "HeadWear"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Head.OverlayList.Contains(Overlay) == false  ) 
							_MaleEquipment._Head.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Head.OverlayList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Head.OverlayList.Add (Overlay);
						
					}
					// Add the Head Wear Overlay Only to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Head" 
					    && Overlay.OverlayType == "HeadWear"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Head.OverlayOnlyList.Contains(Overlay) == false  ) 
							_MaleEquipment._Head.OverlayOnlyList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Head.OverlayOnlyList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Head.OverlayOnlyList.Add (Overlay);
						
					}
					// Add the Shoulder Wear Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "ShoulderWear" && Overlay.OverlayType == "ShoulderWear"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Shoulder.OverlayList.Contains(Overlay) == false  ) 
							_MaleEquipment._Shoulder.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Shoulder.OverlayList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Shoulder.OverlayList.Add (Overlay);
						
					}
					// Add the Shoulder Wear Overlay Only to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && (Overlay.Place.name ==  "Torso" || Overlay.Place.name ==  "Shoulder"
					    && Overlay.Race.Contains(race.Race)) 
					    && Overlay.OverlayType == "ShoulderWear")
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Shoulder.OverlayOnlyList.Contains(Overlay) == false  ) 
							_MaleEquipment._Shoulder.OverlayOnlyList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Shoulder.OverlayOnlyList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Shoulder.OverlayOnlyList.Add (Overlay);
						
					}
					// Add the Torso Wear Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "TorsoWear" && Overlay.OverlayType == "TorsoWear"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Torso.OverlayList.Contains(Overlay) == false  ) 
							_MaleEquipment._Torso.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Torso.OverlayList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Torso.OverlayList.Add (Overlay);
						
					}
					// Add the Torso Wear Overlay Only to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "Torso" 
					    && Overlay.OverlayType == "TorsoWear"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Torso.OverlayOnlyList.Contains(Overlay) == false  ) 
							_MaleEquipment._Torso.OverlayOnlyList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Torso.OverlayOnlyList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Torso.OverlayOnlyList.Add (Overlay);
						
					}
					// Add the Hands Wear Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "HandsWear" && Overlay.OverlayType == "HandsWear"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Hands.OverlayList.Contains(Overlay) == false  ) 
							_MaleEquipment._Hands.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Hands.OverlayList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Hands.OverlayList.Add (Overlay);
						
					}
					// Add the Hands Wear Overlay Only to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && (Overlay.Place.name ==  "Torso" || Overlay.Place.name ==  "Hands"
					    && Overlay.Race.Contains(race.Race)) 
					    && Overlay.OverlayType == "HandsWear")
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Hands.OverlayOnlyList.Contains(Overlay) == false  ) 
							_MaleEquipment._Hands.OverlayOnlyList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Hands.OverlayOnlyList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Hands.OverlayOnlyList.Add (Overlay);
						
					}
					// Add the Legs Wear Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "LegsWear" && Overlay.OverlayType == "LegsWear"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Legs.OverlayList.Contains(Overlay) == false  ) 
							_MaleEquipment._Legs.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Legs.OverlayList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Legs.OverlayList.Add (Overlay);
						
					}
					// Add the Legs Wear Overlay Only to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && ( Overlay.Place.name ==  "Torso" || Overlay.Place.name ==  "Legs" 
					    && Overlay.Race.Contains(race.Race))
					    && Overlay.OverlayType == "LegsWear")
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Legs.OverlayOnlyList.Contains(Overlay) == false  ) 
							_MaleEquipment._Legs.OverlayOnlyList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Legs.OverlayOnlyList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Legs.OverlayOnlyList.Add (Overlay);
						
					}
					// Add the Feet Wear Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "FeetWear" && Overlay.OverlayType == "FeetWear"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Feet.OverlayList.Contains(Overlay) == false  ) 
							_MaleEquipment._Feet.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Feet.OverlayList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Feet.OverlayList.Add (Overlay);
						
					}
					// Add the Legs Wear Overlay Only to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && ( Overlay.Place.name ==  "Torso" || Overlay.Place.name ==  "Feet" 
					    && Overlay.Race.Contains(race.Race))
					    && Overlay.OverlayType == "FeetWear")
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Feet.OverlayOnlyList.Contains(Overlay) == false  ) 
							_MaleEquipment._Feet.OverlayOnlyList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Feet.OverlayOnlyList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Feet.OverlayOnlyList.Add (Overlay);
						
					}
					// Add the Feet Wear Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "FeetWear" && Overlay.OverlayType == "FeetWear"
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Feet.OverlayList.Contains(Overlay) == false  ) 
							_MaleEquipment._Feet.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Feet.OverlayList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Feet.OverlayList.Add (Overlay);
						
					}
					// Add the Feet Wear Overlay Only to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && ( Overlay.Place.name ==  "Torso" || Overlay.Place.name ==  "Feet" 
					    && Overlay.Race.Contains(race.Race))
					    && Overlay.OverlayType == "FeetWear")
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Feet.OverlayOnlyList.Contains(Overlay) == false  ) 
							_MaleEquipment._Feet.OverlayOnlyList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Feet.OverlayOnlyList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Feet.OverlayOnlyList.Add (Overlay);
						
					}
					// Add the Belt Wear Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "BeltWear" 
					    && ( Overlay.OverlayType == "BeltWear" || Overlay.OverlayType == "" 
					    && Overlay.Race.Contains(race.Race)))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Belt.OverlayList.Contains(Overlay) == false  ) 
							_MaleEquipment._Belt.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Belt.OverlayList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Belt.OverlayList.Add (Overlay);
						
					}
					// Add the Belt Wear Overlay Only to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && ( Overlay.Place.name ==  "Torso" || Overlay.Place.name ==  "Belt" 
					    && Overlay.Race.Contains(race.Race))
					    && Overlay.OverlayType == "BeltWear")
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Belt.OverlayOnlyList.Contains(Overlay) == false  ) 
							_MaleEquipment._Belt.OverlayOnlyList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Belt.OverlayOnlyList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Belt.OverlayOnlyList.Add (Overlay);
						
					}
					// Add the Cloak Wear Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "CloakWear" 
					    && ( Overlay.OverlayType == "CloakWear" || Overlay.OverlayType == "" 
					    && Overlay.Race.Contains(race.Race)))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._Cloak.OverlayList.Contains(Overlay) == false  ) 
							_MaleEquipment._Cloak.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._Cloak.OverlayList.Contains(Overlay) == false  ) 
							_FemaleEquipment._Cloak.OverlayList.Add (Overlay);
						
					}
					// Add the HandledLeft Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "HandledLeft" 
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._LeftHand.OverlayList.Contains(Overlay) == false  ) 
							_MaleEquipment._LeftHand.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._LeftHand.OverlayList.Contains(Overlay) == false  ) 
							_FemaleEquipment._LeftHand.OverlayList.Add (Overlay);
						
					}
					// Add the HandledRight Overlay to the list
					if (Overlay.Place != null && Overlay.OverlayType != null
					    && Overlay.Place.name ==  "HandledRight" 
					    && Overlay.Race.Contains(race.Race))
					{
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Male" )
						    && _MaleEquipment._RightHand.OverlayList.Contains(Overlay) == false  ) 
							_MaleEquipment._RightHand.OverlayList.Add (Overlay);
						if ((Overlay.Gender == "Both" || Overlay.Gender == "Female" ) 
						    && _FemaleEquipment._RightHand.OverlayList.Contains(Overlay) == false  ) 
							_FemaleEquipment._RightHand.OverlayList.Add (Overlay);
					}
					#endregion Populate Wear Lists
				}
			}
			race._Male._AvatarData = _MaleAvatar;
			race._Male._EquipmentData = _MaleEquipment;
			race._Female._AvatarData = _FemaleAvatar;
			race._Female._EquipmentData = _FemaleEquipment;
			EditorUtility.SetDirty (race);
		}
		AssetDatabase.SaveAssets ();
	//	Debug.Log ("all RPG lists populated.");
	//	Debug.Log ("DK Helper : You are now ready to generate your RPG avatars. Open the 'Create' Tab of the DK UMA Editor window, then enable 'Is Character' for the avatar to be generated as a RPG avatar. Create your avatar using the various options of DK UMA.");

		Done = true;
	}
}
