using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
// using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

public class DK_RPG_ReBuild : MonoBehaviour {
	DK_UMACrowd Crowd;

	DKUMAData umaData;
	DK_RPG_UMA _DK_RPG_UMA;
	DK_RPG_UMA_Generator _DK_RPG_UMA_Generator;

	public List<DKSlotData> TmpSlotDataList = new List<DKSlotData>();
	public List<DKSlotData> AssignedSlotsList = new List<DKSlotData>();
	public List<DKOverlayData> AssignedOverlayList = new List<DKOverlayData>();
	public List<DKOverlayData> TmpTorsoOverLayList = new List<DKOverlayData>();

	public DKSlotData FaceSlot;
	public int HeadIndex = 0;
	public DKSlotData TorsoSlot;
	public int TorsoIndex = 0;
	public DKSlotData _Slot;
	public DKOverlayData _Overlay;
	public DKOverlayData _FaceOverlay;
	public Color _Color;

	public void Launch (DKUMAData UMAData){

		umaData = UMAData;
		_DK_RPG_UMA = this.gameObject.GetComponent<DK_RPG_UMA>();
		Crowd = GameObject.Find ("DKUMACrowd").GetComponent<DK_UMACrowd>();
		_DK_RPG_UMA._Avatar.HeadIndex = HeadIndex;
		TorsoIndex = _DK_RPG_UMA._Avatar.TorsoIndex;
		TmpSlotDataList.Clear();
		AssignedSlotsList.Clear();
		AssignedOverlayList.Clear();
		TmpTorsoOverLayList.Clear();
		Crowd.Wears.HideUnderwear = false;

		RebuildFace ();
	}

	public void RebuildFace (){

		// _Head
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Avatar._Face._Head.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Face._Head.Overlay;
		_Color = _DK_RPG_UMA._Avatar.SkinColor;
		if ( _Slot && _Overlay){
		
			AssigningSlot (Crowd, _Slot, _Overlay, "_Head", _Color);	// assign the slot and its overlays				
			_FaceOverlay = _Overlay;
			HeadIndex = 0;
			_DK_RPG_UMA._Avatar.HeadIndex = HeadIndex;
		}

		// _Eyes
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Avatar._Face._Eyes.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Face._Eyes.Overlay;
		_Color = _DK_RPG_UMA._Avatar._Face._Eyes.Color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_Eyes", _Color);	// assign the slot and its overlays				

		// _EyesAdjust
		_Slot = null;
		_Overlay = null;
		_Overlay = _DK_RPG_UMA._Avatar._Face._Eyes.Adjust;
		_Color = _DK_RPG_UMA._Avatar.EyeColor;
		if ( _Overlay)
			AssigningOverlay (Crowd, TmpSlotDataList.Count-1, _Overlay, "_EyesAdjust", true, _Color);	// assign the slot and its overlays				

		// _Ears
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Avatar._Face._Ears.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Face._Ears.Overlay;
		_Color = _DK_RPG_UMA._Avatar.SkinColor;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_Ears", _Color);	// assign the slot and its overlays	

		// _Head_Tatoo
		_Overlay = null;
		_Overlay = _DK_RPG_UMA._Avatar._Face._Head.Tattoo;
		_Color = _DK_RPG_UMA._Avatar._Face._Head.TattooColor;
		if ( _Overlay)
			AssigningOverlay (Crowd, HeadIndex, _Overlay, "_Head_Tatoo", true, _Color);	// assign the slot and its overlays				

		// _Head_MakeUp
		_Overlay = null;
		_Overlay = _DK_RPG_UMA._Avatar._Face._Head.Makeup;
		_Color = _DK_RPG_UMA._Avatar._Face._Head.MakeupColor;
		if ( _Overlay)
			AssigningOverlay (Crowd, HeadIndex, _Overlay, "_Head_MakeUp", true, _Color);	// assign the slot and its overlays				

		// _Lips
		_Overlay = null;
		_Overlay = _DK_RPG_UMA._Avatar._Face._Mouth.Lips;
		_Color = _DK_RPG_UMA._Avatar._Face._Mouth.LipsColor;
		if ( _Overlay)
			AssigningOverlay (Crowd, HeadIndex, _Overlay, "_Lips", true, _Color);	// assign the slot and its overlays				

		#region _FaceHair
		// _Eyebrow
		_Overlay = null;
		_Overlay = _DK_RPG_UMA._Avatar._Face._FaceHair.EyeBrows;
		_Color = _DK_RPG_UMA._Avatar._Face._FaceHair.EyeBrowsColor;
		if ( _Overlay)
			AssigningOverlay (Crowd, HeadIndex, _Overlay, "_Eyebrow", true, _Color);	// assign the slot and its overlays				

		#region  _BeardOverlayOnly
		// _Beard1
		_Overlay = null;
		_Overlay = _DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard1;
		_Color = _DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard1Color;
		if ( _Overlay)
			AssigningOverlay (Crowd, HeadIndex, _Overlay, "_Beard1", true, _Color);	// assign the slot and its overlays				

		// _Beard2
		_Overlay = null;
		_Overlay = _DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard2;
		_Color = _DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard2Color;
		if ( _Overlay)
			AssigningOverlay (Crowd, HeadIndex, _Overlay, "_Beard2", true, _Color);	// assign the slot and its overlays				

		// _Beard3
		_Overlay = null;
		_Overlay = _DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard3;
		_Color = _DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard3Color;
		if ( _Overlay)
			AssigningOverlay (Crowd, HeadIndex, _Overlay, "_Beard3", true, _Color);	// assign the slot and its overlays				
		#endregion _BeardOverlayOnly

		#region _BeardSlotOnly
		// _BeardSlotOnly
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Avatar._Face._FaceHair._BeardSlotOnly.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Face._FaceHair._BeardSlotOnly.Overlay;
		_Color = _DK_RPG_UMA._Avatar._Face._FaceHair._BeardSlotOnly.Color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_BeardSlotOnly", _Color);	// assign the slot and its overlays				
		#endregion _BeardSlotOnly
		#endregion _FaceHair

		#region Hair
		#region _SlotOnly
		// _HairSlotOnly
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Avatar._Hair._SlotOnly.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Hair._SlotOnly.Overlay;
		_Color = _DK_RPG_UMA._Avatar.HairColor;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_HairSlotOnly", _Color);	// assign the slot and its overlays		
		#region _Hair_Module
		// _HairSlotOnlyModule
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Avatar._Hair._SlotOnly._HairModule.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Hair._SlotOnly._HairModule.Overlay;
		_Color = _DK_RPG_UMA._Avatar.HairColor;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_HairSlotOnlyModule", _Color);	// assign the slot and its overlays
		#endregion _Hair_Module
		#endregion _SlotOnly

		#region _OverlayOnly
		// _HairOverlayOnly
		_Overlay = null;
		_Overlay = _DK_RPG_UMA._Avatar._Hair._OverlayOnly.Overlay;
		_Color = _DK_RPG_UMA._Avatar.HairColor;
		if ( _Overlay)
			AssigningOverlay (Crowd, HeadIndex, _Overlay, "_HairOverlayOnly", true, _Color);
		#endregion _OverlayOnly
		#endregion Hair

		#region _Face

		// _EyeLash
		_Slot = null;
		_Overlay = null;
		
		_Slot = _DK_RPG_UMA._Avatar._Face._EyeLash.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Face._EyeLash.Overlay;
		_Color = _DK_RPG_UMA._Avatar._Face._EyeLash.Color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_EyeLash", _Color);	// assign the slot and its overlays	

		// _EyeLids
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Avatar._Face._EyeLids.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Face._EyeLids.Overlay;
		_Color = _DK_RPG_UMA._Avatar.SkinColor;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_EyeLids", _Color);	// assign the slot and its overlays	

		// _Nose
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Avatar._Face._Nose.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Face._Nose.Overlay;
		_Color = _DK_RPG_UMA._Avatar.SkinColor;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_Nose", _Color);	// assign the slot and its overlays	

		// _Mouth
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Avatar._Face._Mouth.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Face._Mouth.Overlay;
		_Overlay = _FaceOverlay;
		_Color = _DK_RPG_UMA._Avatar.SkinColor;
		if ( _Slot && _Overlay )
			AssigningSlot (Crowd, _Slot, _Overlay, "_Mouth", _Color);	// assign the slot and its overlays	

		// _InnerMouth
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Avatar._Face._Mouth._InnerMouth.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Face._Mouth._InnerMouth.Overlay;
		_Color = _DK_RPG_UMA._Avatar._Face._Mouth._InnerMouth.color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_InnerMouth", _Color);	// assign the slot and its overlays	
		#endregion _Face

		RebuildBody ();
	}

	public void RebuildBody (){

		TmpTorsoOverLayList.Add(Crowd.overlayLibrary.InstantiateOverlay(_DK_RPG_UMA._Avatar._Body._Torso.Overlay.overlayName, _DK_RPG_UMA._Avatar.SkinColor));
		if ( _DK_RPG_UMA._Avatar._Body._Torso.Tattoo ) TmpTorsoOverLayList.Add(Crowd.overlayLibrary.InstantiateOverlay(_DK_RPG_UMA._Avatar._Body._Torso.Tattoo.overlayName, _DK_RPG_UMA._Avatar._Body._Torso.TattooColor));
		if ( _DK_RPG_UMA._Avatar._Body._Torso.Makeup ) TmpTorsoOverLayList.Add(Crowd.overlayLibrary.InstantiateOverlay(_DK_RPG_UMA._Avatar._Body._Torso.Makeup.overlayName, _DK_RPG_UMA._Avatar._Body._Torso.MakeupColor));
		if ( _DK_RPG_UMA._Avatar._Body._Underwear.Overlay ) TmpTorsoOverLayList.Add(Crowd.overlayLibrary.InstantiateOverlay(_DK_RPG_UMA._Avatar._Body._Underwear.Overlay.overlayName, _DK_RPG_UMA._Avatar._Body._Underwear.Color));
		if ( _DK_RPG_UMA._Equipment._Hands.Slot == null && _DK_RPG_UMA._Equipment._Hands.Overlay != null ) TmpTorsoOverLayList.Add(Crowd.overlayLibrary.InstantiateOverlay(_DK_RPG_UMA._Equipment._Hands.Overlay.overlayName, _DK_RPG_UMA._Equipment._Hands.Color));
		if ( _DK_RPG_UMA._Equipment._Feet.Slot == null && _DK_RPG_UMA._Equipment._Feet.Overlay != null ) TmpTorsoOverLayList.Add(Crowd.overlayLibrary.InstantiateOverlay(_DK_RPG_UMA._Equipment._Feet.Overlay.overlayName, _DK_RPG_UMA._Equipment._Feet.Color));
		if ( _DK_RPG_UMA._Equipment._Torso.Slot == null && _DK_RPG_UMA._Equipment._Torso.Overlay != null ) TmpTorsoOverLayList.Add(Crowd.overlayLibrary.InstantiateOverlay(_DK_RPG_UMA._Equipment._Torso.Overlay.overlayName, _DK_RPG_UMA._Equipment._Torso.Color));
		if ( _DK_RPG_UMA._Equipment._Legs.Slot == null && _DK_RPG_UMA._Equipment._Legs.Overlay != null ) TmpTorsoOverLayList.Add(Crowd.overlayLibrary.InstantiateOverlay(_DK_RPG_UMA._Equipment._Legs.Overlay.overlayName, _DK_RPG_UMA._Equipment._Legs.Color));

		_DK_RPG_UMA.TmpTorsoOverLayList = TmpTorsoOverLayList;

		// _Hands
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Avatar._Body._Hands.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Body._Hands.Overlay;
		_Color = _DK_RPG_UMA._Avatar.SkinColor;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_Hands", _Color);	// assign the slot and its overlays	


		// _Feet
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Avatar._Body._Feet.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Body._Feet.Overlay;
		_Color = _DK_RPG_UMA._Avatar.SkinColor;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_Feet", _Color);	// assign the slot and its overlays	

		// _Torso
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Avatar._Body._Torso.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Body._Torso.Overlay;
		_Color = _DK_RPG_UMA._Avatar.SkinColor;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_Torso", _Color);	// assign the slot and its overlays	
		
		// _Legs
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Avatar._Body._Legs.Slot;
		_Overlay = _DK_RPG_UMA._Avatar._Body._Legs.Overlay;
		_Color = _DK_RPG_UMA._Avatar.SkinColor;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_Legs", _Color);	// assign the slot and its overlays	


		RebuildEquipment ();
	}

	public void RebuildEquipment (){

		// _HeadWear
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Equipment._Head.Slot;
		_Overlay = _DK_RPG_UMA._Equipment._Head.Overlay;
		_Color = _DK_RPG_UMA._Equipment._Head.Color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_HeadWear", _Color);	// assign the slot and its overlays	

		// _TorsoWear
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Equipment._Torso.Slot;
		_Overlay = _DK_RPG_UMA._Equipment._Torso.Overlay;
		_Color = _DK_RPG_UMA._Equipment._Torso.Color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_TorsoWear", _Color);	// assign the slot and its overlays	

		// _HandsWear
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Equipment._Hands.Slot;
		_Overlay = _DK_RPG_UMA._Equipment._Hands.Overlay;
		_Color = _DK_RPG_UMA._Equipment._Hands.Color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_HandsWear", _Color);	// assign the slot and its overlays	

		// _LegsWear
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Equipment._Legs.Slot;
		_Overlay = _DK_RPG_UMA._Equipment._Legs.Overlay;
		_Color = _DK_RPG_UMA._Equipment._Legs.Color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_LegsWear", _Color);	// assign the slot and its overlays	

		// _FeetWear
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Equipment._Feet.Slot;
		_Overlay = _DK_RPG_UMA._Equipment._Feet.Overlay;
		_Color = _DK_RPG_UMA._Equipment._Feet.Color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_FeetWear", _Color);	// assign the slot and its overlays	

		// _ShoulderWear
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Equipment._Shoulder.Slot;
		_Overlay = _DK_RPG_UMA._Equipment._Shoulder.Overlay;
		_Color = _DK_RPG_UMA._Equipment._Shoulder.Color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_ShoulderWear", _Color);	// assign the slot and its overlays	

		// _BeltWear
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Equipment._Belt.Slot;
		_Overlay = _DK_RPG_UMA._Equipment._Belt.Overlay;
		_Color = _DK_RPG_UMA._Equipment._Belt.Color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_BeltWear", _Color);	// assign the slot and its overlays	

		// _LeftHand
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Equipment._LeftHand.Slot;
		_Overlay = _DK_RPG_UMA._Equipment._LeftHand.Overlay;
		_Color = _DK_RPG_UMA._Equipment._LeftHand.Color;
		if ( _Slot && _Overlay){
			AssigningSlot (Crowd, _Slot, _Overlay, "_LeftHand", _Color);	// assign the slot and its overlays	
		}
		// _RightHand
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Equipment._RightHand.Slot;
		_Overlay = _DK_RPG_UMA._Equipment._RightHand.Overlay;
		_Color = _DK_RPG_UMA._Equipment._RightHand.Color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_RightHand", _Color);	// assign the slot and its overlays	

		// _ArmBand
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Equipment._ArmBand.Slot;
		_Overlay = _DK_RPG_UMA._Equipment._ArmBand.Overlay;
		_Color = _DK_RPG_UMA._Equipment._ArmBand.Color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_ArmBand", _Color);	// assign the slot and its overlays	

		// _Wrist
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Equipment._Wrist.Slot;
		_Overlay = _DK_RPG_UMA._Equipment._Wrist.Overlay;
		_Color = _DK_RPG_UMA._Equipment._Wrist.Color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_Wrist", _Color);	// assign the slot and its overlays	

		// _Cloak
		_Slot = null;
		_Overlay = null;
		_Slot = _DK_RPG_UMA._Equipment._Cloak.Slot;
		_Overlay = _DK_RPG_UMA._Equipment._Cloak.Overlay;
		_Color = _DK_RPG_UMA._Equipment._Cloak.Color;
		if ( _Slot && _Overlay)
			AssigningSlot (Crowd, _Slot, _Overlay, "_Cloak", _Color);	// assign the slot and its overlays	


		Cleaning ();
	}


	public void AssigningSlot (DK_UMACrowd Crowd, DKSlotData slot, DKOverlayData Overlay, string type, Color color){
	//	try{
		if ( type == "_EyeLids" || type == "_Nose" || type == "_Mouth" || type == "_Ears" ){
			if ( slot.LinkedOverlayList.Count == 0 
			    && Crowd && Crowd.slotLibrary
			    && slot && slot.slotName != null 
			    && TmpSlotDataList[HeadIndex] )
				TmpSlotDataList.Add(Crowd.slotLibrary.InstantiateSlot(slot.slotName,TmpSlotDataList[HeadIndex].overlayList));
			else {
				int _index = TmpSlotDataList.Count;
				TmpSlotDataList.Add(Crowd.slotLibrary.InstantiateSlot( slot.slotName ));
				Overlay = slot.LinkedOverlayList[0];
				AssigningOverlay (Crowd, _index, Overlay, type, false, color);
			}
		}
		else if ( type == "_Torso" || type == "_Hands" || type == "_Legs" || type == "_Feet" ){
			TmpSlotDataList.Add(Crowd.slotLibrary.InstantiateSlot(slot.slotName, TmpTorsoOverLayList));
		}
		else {
			int _index = TmpSlotDataList.Count;
			TmpSlotDataList.Add(Crowd.slotLibrary.InstantiateSlot( slot.slotName ));
			AssigningOverlay (Crowd, _index, Overlay, type, false, color);
		}
	//	}catch ( System.ArgumentOutOfRangeException ){}
		// Copy the values
		CopyValues (Crowd, slot, TmpSlotDataList.Count-1);

		// Remove the face elements for a head wear
		if ( type == "_HeadWear" )
		for (int i1 = 0; i1 < TmpSlotDataList.Count; i1 ++){
			#region if Hide Hair
			if ( slot._HideData.HideHair == true ) {
				if ( TmpSlotDataList[i1].Place && TmpSlotDataList[i1].Place.name == "Hair" ) {
				
					ToRemoveList.Add(TmpSlotDataList[i1]);
				}
			}
			#endregion if Hide Hair 

			#region if Hide Hair Module
			if ( slot._HideData.HideHairModule == true ) {
				if ( TmpSlotDataList[i1].Place && TmpSlotDataList[i1].Place.name == "Hair_Module" ) {
				
					ToRemoveList.Add(TmpSlotDataList[i1]);
				}
			}
			#endregion if Hide Hair Module 
			
			#region if Hide Mouth
			if ( slot._HideData.HideMouth == true ) {
				if ( TmpSlotDataList[i1].Place && TmpSlotDataList[i1].Place.name == "Mouth" ) {
				
					ToRemoveList.Add(TmpSlotDataList[i1]);
				}
			}
			#endregion if Hide Mouth 
			
			#region if Hide Beard
			if ( slot._HideData.HideBeard == true ) {

				if ( TmpSlotDataList[i1].OverlayType == "Beard" ) {
				
					ToRemoveList.Add(TmpSlotDataList[i1]);
				}
			}
			#endregion if Hide Mouth 

			#region if Hide Ears
			if ( slot._HideData.HideEars == true ) {
				if ( TmpSlotDataList[i1].Place && TmpSlotDataList[i1].Place.name == "Ears" ) {
					ToRemoveList.Add(TmpSlotDataList[i1]);
				
				}
			}
			#endregion if Hide Ears
		}
	}

	public DKOverlayData overlay;
	public void AssigningOverlay (DK_UMACrowd Crowd, int index, DKOverlayData Overlay, string type, bool OverlayOnly, Color color){
		Color ColorToApply = color;
		overlay = Overlay;

		// Assign the Overlay
		try {
		if ( type.Contains("Wear") ) TmpSlotDataList[index].overlayList.Clear();
		}catch (System.NullReferenceException e) { Debug.Log ( e );}
		if ( !overlay ) Debug.LogError ( "Overlay is missing, skipping it." );
		if ( !TmpSlotDataList[index] ) {/*Debug.LogError ( this.transform.parent.name+" Slot is missing, skipping it." );*/}
		else{
			TmpSlotDataList[index].overlayList.Add(Crowd.overlayLibrary.InstantiateOverlay(overlay.overlayName,ColorToApply));
			TmpSlotDataList[index].overlayList[TmpSlotDataList[index].overlayList.Count-1].OverlayType = overlay.OverlayType;
		}
		if ( type == "_Head" ) {
			_FaceOverlay = overlay;
		}
		else if ( type == "_Ears" ) {
		}
	}

	List<DKSlotData> ToRemoveList = new List<DKSlotData>();
	void Cleaning (){

		#region Cleaning Avatar
		#region Legacy
		for(int i = 0; i < TmpSlotDataList.Count; i ++){
			#region if Replace activated
			if ( TmpSlotDataList[i].Replace == true ) {
				for(int i1 = 0; i1 < TmpSlotDataList.Count; i1 ++){
					if ( TmpSlotDataList[i].Place.dk_SlotsAnatomyElement.Place == TmpSlotDataList[i1].Place ) {
						ToRemoveList.Add ( TmpSlotDataList[i1] );
					}
				}
			}
			#endregion if Replace activated
			
			if ( TmpSlotDataList[i].Place != null ) {
				#region hide shoulders
				// detect 'hide shoulders'
				if ( TmpSlotDataList[i]._HideData.HideShoulders ){
					Crowd.Wears.HideShoulders = true;
				}
				// detect the shoulders
				if ( TmpSlotDataList[i].Place.name == "ShoulderWear" ){
					Crowd.Wears.Shoulders = TmpSlotDataList[i];
				}
				#endregion hide shoulders
				
				#region hide LegWear
				// detect 'hide Legs'
				if ( TmpSlotDataList[i]._HideData.HideLegs ){
					Crowd.Wears.HideLegs = true;
				}
				// detect the Legs
				if ( TmpSlotDataList[i].Place.name == "LegsWear" ){
					Crowd.Wears.Legs = TmpSlotDataList[i];
				}
				#endregion hide LegWear

				#region hide BeltWear
				// detect 'hide Belt'
				if ( TmpSlotDataList[i]._HideData.HideBelt ){
					Crowd.Wears.HideBelt = true;
				}
				// detect the belt
				if ( TmpSlotDataList[i].Place.name == "BeltWear" ){
					Crowd.Wears.Belt = TmpSlotDataList[i];
				}
				#endregion hide BeltWear

				#region hide armband
				// detect 'ArmbandWear'
				if ( TmpSlotDataList[i]._HideData.HideArmBand ){
					Crowd.Wears.HideArmBand = true;
				}
				// detect the ArmbandWear
				if ( TmpSlotDataList[i].Place.name == "ArmbandWear" ){
					Crowd.Wears.ArmBand = TmpSlotDataList[i];
				}
				#endregion hide armband

				#region hide wrist
				// detect 'hide wrist'
				if ( TmpSlotDataList[i]._HideData.HideWrist ){
					Crowd.Wears.HideWrist = true;
				}
				// detect the WristWear
				if ( TmpSlotDataList[i].Place.name == "WristWear" ){
					Crowd.Wears.Wrist = TmpSlotDataList[i];
				}
				#endregion hide armband

				#region hide leg band
				// detect 'hide leg band'
				if ( TmpSlotDataList[i]._HideData.HideLegBand ){
					Crowd.Wears.HideLegBand = true;
				}
				// detect the LegBandWear
				if ( TmpSlotDataList[i].Place.name == "LegBandWear" ){
					Crowd.Wears.LegBand = TmpSlotDataList[i];
				}
				#endregion hide armband

				#region hide underwear
				// detect 'hide underwear'
				if ( TmpSlotDataList[i]._HideData.HideUnderwear ){
					Crowd.Wears.HideUnderwear = true;
				}
				if ( TmpSlotDataList[i].Place.name == "Torso" && TmpSlotDataList[i].OverlayType == "Flesh" )
					TorsoIndex = i;
				#endregion hide underwear
			}
			else{}

			// detect the underwear
			if ( Crowd.Wears.HideUnderwear){
				for(int i1 = 0; i1 < TmpSlotDataList[TorsoIndex].overlayList.Count; i1 ++){
					if ( _DK_RPG_UMA._Avatar._Body._Underwear.Overlay 
					    && TmpSlotDataList[TorsoIndex].overlayList[i1].overlayName == _DK_RPG_UMA._Avatar._Body._Underwear.Overlay.overlayName )
						TmpSlotDataList[TorsoIndex].overlayList.Remove ( TmpSlotDataList[TorsoIndex].overlayList[i1] );
				}
			}

			// detect Legacy
			DKSlotData Slot = TmpSlotDataList[i];
			if ( Slot._LegacyData.HasLegacy == true && ToRemoveList.Contains (Slot) == false ) {
				if ( Slot._LegacyData.LegacyList.Count > 0 ){
					foreach ( DKSlotData LegacySlot in Slot._LegacyData.LegacyList ){
						// select the overlay
						try {

							#region Choose Color to apply
							if ( _DK_RPG_UMA == null ){
								if ( LegacySlot.OverlayType == "Hair" ) Crowd.Colors.ColorToApply =Crowd.Colors.HairColor;
								else if ( LegacySlot.OverlayType == "Beard" ) Crowd.Colors.ColorToApply =Crowd.Colors.HairColor;
								else if ( LegacySlot.OverlayType == "TorsoWear" ) Crowd.Colors.ColorToApply =Crowd.Colors.TorsoWearColor;
								else if ( LegacySlot.OverlayType == "LegsWear" ) Crowd.Colors.ColorToApply =Crowd.Colors.LegsWearColor;
								else if ( LegacySlot.OverlayType == "HandWear" ) Crowd.Colors.ColorToApply =Crowd.Colors.HandWearColor;
								else if ( LegacySlot.OverlayType == "FeetWear" ) Crowd.Colors.ColorToApply =Crowd.Colors.FeetWearColor;
								else if ( LegacySlot.OverlayType == "ShoulderWear" ) Crowd.Colors.ColorToApply =Crowd.Colors.TorsoWearColor;
								else if ( LegacySlot.OverlayType == "Eyes" ) Crowd.Colors.ColorToApply =Crowd.Colors.EyesColor;
								else if ( LegacySlot.OverlayType == "Face" ) Crowd.Colors.ColorToApply =Crowd.Colors.skinColor;
								else if ( LegacySlot.OverlayType == "Flesh" ) Crowd.Colors.ColorToApply =Crowd.Colors.skinColor;
								else if ( LegacySlot.Place && LegacySlot.Place.name == "InnerMouth" ) Crowd.Colors.ColorToApply =Crowd.Colors.InnerMouthColor;
								else {
									Crowd.Colors.ColorToApply = new Color (UnityEngine.Random.Range(0.01f,0.9f),UnityEngine.Random.Range(0.01f,0.9f),UnityEngine.Random.Range(0.01f,0.9f));
									//	Colors.BeltWearColor1 = UnityEngine.Random.Range(0.01f,0.9f);
								}
							}
							// for the DK UMA RPG Avatars
							else {
								if ( LegacySlot.OverlayType == "Hair" ) Crowd.Colors.ColorToApply =_DK_RPG_UMA._Avatar.HairColor;
								else if ( LegacySlot.OverlayType == "Beard" ) Crowd.Colors.ColorToApply =_DK_RPG_UMA._Avatar.HairColor;
								else if ( LegacySlot.OverlayType == "TorsoWear" )Crowd.Colors.ColorToApply =_DK_RPG_UMA._Equipment._Torso.Color;
								else if ( LegacySlot.OverlayType == "LegsWear" ) Crowd.Colors.ColorToApply =_DK_RPG_UMA._Equipment._Legs.Color;
								else if ( LegacySlot.OverlayType == "HandWear" ) Crowd.Colors.ColorToApply =_DK_RPG_UMA._Equipment._Hands.Color;
								else if ( LegacySlot.OverlayType == "FeetWear" ) Crowd.Colors.ColorToApply =_DK_RPG_UMA._Equipment._Feet.Color;
								else if ( LegacySlot.OverlayType == "ShoulderWear" ) Crowd.Colors.ColorToApply =_DK_RPG_UMA._Equipment._Shoulder.Color;
								else if ( LegacySlot.OverlayType == "Eyes" ) Crowd.Colors.ColorToApply =Crowd.Colors.EyesColor;
								else if ( LegacySlot.OverlayType == "Face" ) Crowd.Colors.ColorToApply =_DK_RPG_UMA._Avatar.SkinColor;
								else if ( LegacySlot.OverlayType == "Flesh" ) Crowd.Colors.ColorToApply =_DK_RPG_UMA._Avatar.SkinColor;
								else if ( LegacySlot.Place && LegacySlot.Place.name == "InnerMouth" ) Crowd.Colors.ColorToApply =_DK_RPG_UMA._Avatar._Face._Mouth._InnerMouth.color;
								else {
									Crowd.Colors.ColorToApply = new Color (UnityEngine.Random.Range(0.01f,0.9f),UnityEngine.Random.Range(0.01f,0.9f),UnityEngine.Random.Range(0.01f,0.9f));
									//	Colors.BeltWearColor1 = UnityEngine.Random.Range(0.01f,0.9f);
								}
							}
							#endregion Choose Color to apply

							DKSlotData placeHolder = ScriptableObject.CreateInstance("DKSlotData") as DKSlotData;
							foreach ( DKSlotData slot in TmpSlotDataList ){
								if ( slot.Place == LegacySlot.Place ) {
									placeHolder = slot;
								}
							}

							// add
							// For flesh
							if ( LegacySlot.OverlayType != null && LegacySlot.OverlayType == "Flesh" ) {

								TmpSlotDataList.Add(Crowd.slotLibrary.InstantiateSlot(LegacySlot.slotName,TmpTorsoOverLayList));

								DKSlotData slot = TmpSlotDataList[TmpSlotDataList.Count-1];
							
								slot._LegacyData.IsLegacy = true;
								slot._LegacyData.ElderList.Add(TmpSlotDataList[i]);

								// del placeHolder if necessary
								if ( LegacySlot._LegacyData.Replace ) {
									ToRemoveList.Add (placeHolder);
								}
							}

							// for Wear and hair
							else {

								float c1 = UnityEngine.Random.Range(0.01f,0.9f);
								float c2 = UnityEngine.Random.Range(0.01f,0.9f);
								float c3 = UnityEngine.Random.Range(0.01f,0.9f);
								Crowd.Colors.ColorToApply = new Color (c1,c2,c3);
								DKSlotData slot = Crowd.slotLibrary.InstantiateSlot(LegacySlot.slotName);
								TmpSlotDataList.Add(slot);
								slot._LegacyData.IsLegacy = true;
								slot._LegacyData.ElderList.Add(TmpSlotDataList[TmpSlotDataList.Count-1]);
						
								int ran = 0;
								int ran2 = 0;
								// define legacy slot's overlay
								if ( LegacySlot.LinkedOverlayList.Count > 0 ) {
									ran = UnityEngine.Random.Range(0, LegacySlot.LinkedOverlayList.Count-1);

									// define color preset
									if (LegacySlot.LinkedOverlayList[ran].ColorPresets.Count > 0) {
										ran2 = UnityEngine.Random.Range(0, LegacySlot.LinkedOverlayList[ran].ColorPresets.Count-1);
									//	Debug.Log ( "color preset "+ran2+" / "+LegacySlot.LinkedOverlayList.Count );
										Crowd.Colors.ColorToApply = LegacySlot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
									}
									AssignedOverlayList.Add (LegacySlot.LinkedOverlayList[ran]);
									TmpSlotDataList[TmpSlotDataList.Count-1].overlayList.Add(Crowd.overlayLibrary.InstantiateOverlay(LegacySlot.LinkedOverlayList[ran].overlayName,Crowd.Colors.ColorToApply));
								}
							//	if ( TmpSlotDataList[TmpSlotDataList.Count-1].overlayList.Count == 0 )
							//		TmpSlotDataList[TmpSlotDataList.Count-1].overlayList.Add(Crowd.overlayLibrary.InstantiateOverlay(LegacySlot.LinkedOverlayList[0].overlayName,Crowd.Colors.ColorToApply));
							
							}
						}
						catch (System.NullReferenceException) {
							if ( LegacySlot == null ){
								Debug.LogError ( "slot '"+Slot.slotName+"' Legacy can't be generated. The legacy slot is missing. Verify the setting of '"+Slot.slotName+"' about the legacy. Skipping the legacy for "+Slot.slotName);
								#if UNITY_EDITOR
								#endif
							}
						}
					}
				}
			}
		}
	
		// clear the list of place holders
		foreach (DKSlotData placeHolder in ToRemoveList ){
			TmpSlotDataList.Remove (placeHolder);
		}
		#endregion Legacy
		
		#region hide shoulders
		if ( Crowd.Wears.Shoulders && Crowd.Wears.HideShoulders == true ) {
			TmpSlotDataList.Remove(Crowd.Wears.Shoulders);
		}
		Crowd.Wears.Shoulders = null;
		Crowd.Wears.HideShoulders = false; 
		#endregion hide shoulders
		
		#region hide Legs
		if ( Crowd.Wears.Legs && Crowd.Wears.HideLegs == true ) {
			TmpSlotDataList.Remove(Crowd.Wears.Legs);
		}
		Crowd.Wears.Legs = null;
		Crowd.Wears.HideLegs = false; 
		#endregion hide legs

		#region hide BeltWear
		if ( Crowd.Wears.Belt && Crowd.Wears.HideBelt == true ) {
			TmpSlotDataList.Remove(Crowd.Wears.Belt);
		}
		Crowd.Wears.Belt = null;
		Crowd.Wears.HideBelt = false; 
		#endregion hide BeltWear
		
		#region hide armband
		if ( Crowd.Wears.ArmBand && Crowd.Wears.HideArmBand == true ) {
			TmpSlotDataList.Remove(Crowd.Wears.ArmBand);
		}
		Crowd.Wears.ArmBand = null;
		Crowd.Wears.HideArmBand = false; 
		#endregion hide armband

		#region hide wrist
		if ( Crowd.Wears.Wrist && Crowd.Wears.HideWrist == true ) {
			TmpSlotDataList.Remove(Crowd.Wears.Wrist);
		}
		Crowd.Wears.Wrist = null;
		Crowd.Wears.HideWrist = false; 
		#endregion hide wrist

		 
		#endregion Cleaning Avatar
		
		Finishing();
	}


	public void AddToRPG ( DK_UMACrowd Crowd, DKSlotData slot, DKOverlayData overlay, string type, bool OverlayOnly, Color color){
		// Add the slot to the RPG values of the Avatar
		#region Head
		if ( type == "_Head" ){
			_DK_RPG_UMA._Avatar._Face._Head.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._Head.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Face._Head.Color = color;
		}
		else if ( type == "_Head_Tatoo" ){
			_DK_RPG_UMA._Avatar._Face._Head.Tattoo = overlay;
			_DK_RPG_UMA._Avatar._Face._Head.TattooColor = color;
		}
		else if ( type == "_Head_MakeUp" ){
			_DK_RPG_UMA._Avatar._Face._Head.Makeup = overlay;
			_DK_RPG_UMA._Avatar._Face._Head.MakeupColor = color;
		}
		
		#region FaceHair
		else if ( type == "_EyeBrows" ){
			_DK_RPG_UMA._Avatar._Face._FaceHair.EyeBrows = overlay;
			_DK_RPG_UMA._Avatar._Face._FaceHair.EyeBrowsColor = color;
		}
		if ( type == "_BeardSlotOnly" ){
			_DK_RPG_UMA._Avatar._Face._FaceHair._BeardSlotOnly.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._FaceHair._BeardSlotOnly.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Face._FaceHair._BeardSlotOnly.Color = color;
		}
		// Overlay only
		else if ( type == "_Beard1" ){
			_DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard1 = overlay;
			_DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard1Color = color;
		}
		else if ( type == "Beard2" ){
			_DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard2 = overlay;
			_DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard2Color = color;
			
		}
		else if ( type == "Beard3" ){
			_DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard3 = overlay;
			_DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard3Color = color;
		}
		#endregion FaceHair
		
		else if ( type == "_EyeLash" ){
			_DK_RPG_UMA._Avatar._Face._EyeLash.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._EyeLash.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Face._EyeLash.Color = color;
		}
		else if ( type == "_EyeLids" ){
			_DK_RPG_UMA._Avatar._Face._EyeLids.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._EyeLids.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Face._EyeLids.Color = color;
		}
		else if ( type == "_Eyes" ){
			_DK_RPG_UMA._Avatar._Face._Eyes.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._Eyes.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Face._Eyes.Color = color;
		}
		else if ( type == "_Ears" ){
			_DK_RPG_UMA._Avatar._Face._Ears.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._Ears.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Face._Ears.Color = color;
		}
		else if ( type == "_Nose" ){
			_DK_RPG_UMA._Avatar._Face._Nose.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._Nose.Overlay = overlay;
		}
		else if ( type == "_Mouth" ){
			_DK_RPG_UMA._Avatar._Face._Mouth.Slot = slot;
		}
		else if ( type == "_Lips" ){
			_DK_RPG_UMA._Avatar._Face._Mouth.Lips = overlay;
			_DK_RPG_UMA._Avatar._Face._Mouth.LipsColor = color;
		}
		else if ( type == "_InnerMouth" ){
			_DK_RPG_UMA._Avatar._Face._Mouth._InnerMouth.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._Mouth._InnerMouth.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Face._Mouth._InnerMouth.color = color;
		}
		#endregion Head
		
		#region Hair
		else if ( type == "_HairSlotOnly" ){
			_DK_RPG_UMA._Avatar._Hair._SlotOnly.Slot = slot;
			_DK_RPG_UMA._Avatar._Hair._SlotOnly.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Hair._SlotOnly.Color = color;
		}
		// Overlay only
		else if ( type == "_HairOverlayOnly" ){
			_DK_RPG_UMA._Avatar._Hair._OverlayOnly.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Hair._OverlayOnly.Color = color;
		}
		#endregion Hair
		
		#region Body
		#region _Torso
		if ( type == "_Torso" ){
			_DK_RPG_UMA._Avatar._Body._Torso.Slot = slot;
			_DK_RPG_UMA._Avatar._Body._Torso.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Body._Torso.Color = color;
		}
		else if ( type == "_Head_Tatoo" ){
			_DK_RPG_UMA._Avatar._Body._Torso.Tattoo = overlay;
			_DK_RPG_UMA._Avatar._Body._Torso.TattooColor = color;
		}
		else if ( type == "_Head_MakeUp" ){
			_DK_RPG_UMA._Avatar._Body._Torso.Makeup = overlay;
			_DK_RPG_UMA._Avatar._Body._Torso.MakeupColor = color;
		}
		#endregion _Torso
		#region _Hands
		if ( type == "_Hands" ){
			_DK_RPG_UMA._Avatar._Body._Hands.Slot = slot;
			_DK_RPG_UMA._Avatar._Body._Hands.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Body._Hands.Color = color;
		}

		else if ( type == "_Hands_Tatoo" ){
			_DK_RPG_UMA._Avatar._Body._Hands.Tattoo = overlay;
			_DK_RPG_UMA._Avatar._Body._Hands.TattooColor = color;
		}
		else if ( type == "_Hands_MakeUp" ){
			_DK_RPG_UMA._Avatar._Body._Hands.Makeup = overlay;
			_DK_RPG_UMA._Avatar._Body._Hands.MakeupColor = color;
		}
		#endregion _Hands
		#region _Legs
		if ( type == "_Legs" ){
			_DK_RPG_UMA._Avatar._Body._Legs.Slot = slot;
			_DK_RPG_UMA._Avatar._Body._Legs.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Body._Legs.Color = color;
		}
		else if ( type == "_Legs_Tatoo" ){
			_DK_RPG_UMA._Avatar._Body._Legs.Tattoo = overlay;
			_DK_RPG_UMA._Avatar._Body._Legs.TattooColor = color;
		}
		else if ( type == "_Legs_MakeUp" ){
			_DK_RPG_UMA._Avatar._Body._Legs.Makeup = overlay;
			_DK_RPG_UMA._Avatar._Body._Legs.MakeupColor = color;
		}
		#endregion _Legs
		#region _Feet
		if ( type == "_Feet" ){
			_DK_RPG_UMA._Avatar._Body._Feet.Slot = slot;
			_DK_RPG_UMA._Avatar._Body._Feet.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Body._Feet.Color = color;
		}
		else if ( type == "_Feet_Tatoo" ){
			_DK_RPG_UMA._Avatar._Body._Feet.Tattoo = overlay;
			_DK_RPG_UMA._Avatar._Body._Feet.TattooColor = color;
		}
		else if ( type == "_Feet_MakeUp" ){
			_DK_RPG_UMA._Avatar._Body._Feet.Makeup = overlay;
			_DK_RPG_UMA._Avatar._Body._Feet.MakeupColor = color;
		}
		#endregion _Feet
		#endregion Body
	}
	
	public void CopyValues (DK_UMACrowd Crowd, DKSlotData slot, int index){
		if ( !overlay ) Debug.LogError ( "Overlay is missing, skipping it." );
		if ( !TmpSlotDataList[index] ){/* Debug.LogError ( this.transform.parent.name+" Slot is missing, skipping it." );*/}
		else{
			TmpSlotDataList[index].OverlayType = slot.OverlayType;
			TmpSlotDataList[index].Place = slot.Place;
			TmpSlotDataList[index]._UMA = slot._UMA;
			TmpSlotDataList[index].Replace = slot.Replace;
			TmpSlotDataList[index]._HideData.HideHair = slot._HideData.HideHair;
			TmpSlotDataList[index]._HideData.HideHairModule = slot._HideData.HideHairModule;
			TmpSlotDataList[index]._HideData.HideLegs = slot._HideData.HideLegs;
			TmpSlotDataList[index]._HideData.HideBelt = slot._HideData.HideBelt;
			TmpSlotDataList[index]._HideData.HideArmBand = slot._HideData.HideArmBand;
			TmpSlotDataList[index]._HideData.HideLegBand = slot._HideData.HideLegBand;
			TmpSlotDataList[index]._HideData.HideWrist = slot._HideData.HideWrist;
			TmpSlotDataList[index]._HideData.HideUnderwear = slot._HideData.HideUnderwear;
			TmpSlotDataList[index]._HideData.HideWrist = slot._HideData.HideWrist;
			TmpSlotDataList[index]._HideData.HideMouth = slot._HideData.HideMouth;
			TmpSlotDataList[index]._HideData.HideShoulders = slot._HideData.HideShoulders;
			TmpSlotDataList[index]._HideData.HideBeard = slot._HideData.HideBeard;
			TmpSlotDataList[index]._HideData.HideEars = slot._HideData.HideEars;
			TmpSlotDataList[index]._LegacyData.HasLegacy = slot._LegacyData.HasLegacy;
			TmpSlotDataList[index]._LegacyData.LegacyList = slot._LegacyData.LegacyList;
			TmpSlotDataList[index]._LegacyData.IsLegacy = slot._LegacyData.IsLegacy;
			TmpSlotDataList[index]._LegacyData.ElderList = slot._LegacyData.ElderList;
		}
	}

	void Finishing (){
	//	Debug.Log ("finishing rebuilt" );
		// assign the recipe
		umaData.umaRecipe.slotDataList = TmpSlotDataList.ToArray();

		SaveAvatar ();
	}

	public static DKDnaConverterBehaviour _DnaConverterBehaviour;
	public void SaveAvatar (){
	
	//	DK_RPG_SelfGenerator _DKUMAGenerator =  this.gameObject.GetComponent<DK_RPG_SelfGenerator>();
		DKUMAData umaData = this.gameObject.GetComponent<DKUMAData>();
		if ( !umaData ) umaData = this.gameObject.GetComponent<DKUMAData>();
		if ( umaData && umaData.transform.parent != null ){

		//	DKUMASaveTool umaSaveTool = umaData.transform.GetComponent<DKUMASaveTool>();

			umaData.dirty = false;
			umaData.Dirty(true, true, true);
			
			umaData.Loading = true;
		}
		DeleteScripts ();
	}

	void DeleteScripts (){
		if (  Application.isPlaying ) Destroy(this);
		else DestroyImmediate(this);
	}
}
