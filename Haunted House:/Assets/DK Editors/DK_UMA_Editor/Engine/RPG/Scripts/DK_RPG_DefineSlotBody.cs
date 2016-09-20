using UnityEngine;
# if Editor
using UnityEditor;
# endif
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

public class DK_RPG_DefineSlotBody : MonoBehaviour {

	public static bool Skipping = false;
	public static List<DKSlotData> AssignedSlotsList = new List<DKSlotData>();
	public static List<DKOverlayData> AssignedOverlayList = new List<DKOverlayData>();
	public static List<DKOverlayData> TmpTorsoOverLayList = new List<DKOverlayData>();
	public static DK_RPG_UMA tmp_DK_RPG_UMA;
	public static DK_RPG_UMA _DK_RPG_UMA;

	public static int ranTatoo;
	public static int ranMakeup;


	public static void DefineSlots (DK_UMACrowd Crowd){

	//	List<DKSlotData> HeadSlotsList = new List<DKSlotData>();
	//	List<DKSlotData> BodySlotsList = new List<DKSlotData>();

	//	Debug.Log ("Defining Body Slots");
		// create the RPG script in the UMA avatar
		if ( Crowd.umaData ) DK_RPG_DefineSlotBody.tmp_DK_RPG_UMA = Crowd.umaData.gameObject.AddComponent<DK_RPG_UMA>() as DK_RPG_UMA;
		if ( tmp_DK_RPG_UMA ) _DK_RPG_UMA = tmp_DK_RPG_UMA;

		Crowd._FaceOverlay = null;
		Crowd.Randomize.HairDone = false;

		tmp_DK_RPG_UMA.Gender = Crowd.RaceAndGender.Gender;
		tmp_DK_RPG_UMA.Race = Crowd.RaceAndGender.Race;


		AssignedSlotsList.Clear();
		AssignedOverlayList.Clear();

		if ( DK_UMACrowd.GeneratorMode == "RPG" || DK_UMACrowd.GeneratorMode == "Preset" 
		    //&& Crowd.UMAGenerated  
		    )
		{
			Crowd.UMAGenerated = false;

			#region Create lists
			// Create a tmp list for the slots to generate
			Crowd.tempSlotList = new List<DKSlotData>();Crowd.tempSlotList.Clear();
		
			TmpTorsoOverLayList.Clear();
			AssignedSlotsList.Clear();
			#endregion Create lists

			DKRaceData _Race = Crowd.RaceAndGender.RaceToCreate;
			string _Gender = Crowd.RaceAndGender.Gender;
		//	List<DKSlotData> TmpSlotlist = new List<DKSlotData>();
			List<DKOverlayData> TmpOvlist = new List<DKOverlayData>();

			#region Male
			if ( _Gender == "Male" ){
				#region Head
				// _Eyes
				if ( _Race._Male._AvatarData._Face._Eyes.SlotList.Count > 0 ){
					Crowd.Colors.EyesColor = new Color (1,1,1);
					Color color =   Crowd.Colors.EyesColor;
					int ran = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Face._Eyes.SlotList.Count);
					DKSlotData slot = _Race._Male._AvatarData._Face._Eyes.SlotList[ran];
					TmpOvlist = _Race._Male._AvatarData._Face._Eyes.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Eyes", color);	// assign the slot and its overlays				
				}

				// Assign the Eye Adjust overlay
				if ( _Race._Male._AvatarData._Face._Eyes.AdjustList.Count > 0 )
				{ 
					
					Crowd.Colors.EyeOverlayAdjustColor1 = UnityEngine.Random.Range(0.1f,0.9f);
					Crowd.Colors.EyeOverlayAdjustColor2 = UnityEngine.Random.Range(0.1f,0.9f);
					Crowd.Colors.EyeOverlayAdjustColor3 = UnityEngine.Random.Range(0.1f,0.9f);
					Crowd.Colors.EyeOverlayAdjustColor = new Color(Crowd.Colors.EyeOverlayAdjustColor1 , Crowd.Colors.EyeOverlayAdjustColor2 , Crowd.Colors.EyeOverlayAdjustColor3,1);
					Crowd.Colors.EyesColor = new Color(Crowd.Colors.EyeOverlayAdjustColor1 ,Crowd.Colors.EyeOverlayAdjustColor2,Crowd.Colors.EyeOverlayAdjustColor3,1);
					Crowd.Colors.EyeOverlayAdjustColor = new Color(Crowd.Colors.EyeOverlayAdjustColor1 ,Crowd.Colors.EyeOverlayAdjustColor2,Crowd.Colors.EyeOverlayAdjustColor3,1);
					Color color =   Crowd.Colors.EyeOverlayAdjustColor;
					
					
					TmpOvlist = _Race._Male._AvatarData._Face._Eyes.AdjustList;
					DKSlotData slot = Crowd.tempSlotList[ Crowd.tempSlotList.Count-1 ];
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.tempSlotList.Count-1, TmpOvlist, _Race, "_EyesAdjust", true, color);	// assign the slot and its overlays				
				}

				// _Head
				if ( _Race._Male._AvatarData._Face._Head.SlotList.Count > 0 ){
					int ran = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Face._Head.SlotList.Count);
					DKSlotData slot = _Race._Male._AvatarData._Face._Head.SlotList[ran];
					// Special : Assign face slot to variable
					Crowd.FaceSlot = Crowd.tempSlotList.Count;	
					// assign the overlays list
					TmpOvlist = _Race._Male._AvatarData._Face._Head.OverlayList;
					// find color
					Color color =  Crowd.Colors.skinColor;
					// assign the slot and its overlays and color
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Head", color);	

				}
				// _Tatoo
				ranTatoo = UnityEngine.Random.Range(0,100);
				if ( ranTatoo <= Crowd.Randomize.TatooChance && _Race._Male._AvatarData._Face._Head.TattooList.Count > 0 ){
					// assign the overlays list
					TmpOvlist = _Race._Male._AvatarData._Face._Head.TattooList;
					// Assign the destination slot
					DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
					// find color
					Color color =  new Color (0,0,0,1);
					// assign the slot and its overlays and color
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_Head_Tatoo", true, color);	// assign the slot and its overlays				
				}
				// _Makup
				ranMakeup = UnityEngine.Random.Range(0,100);
				if ( ranTatoo <= Crowd.Randomize.MakeupChance && _Race._Male._AvatarData._Face._Head.MakeupList.Count > 0 ){
					// assign the overlays list
					TmpOvlist = _Race._Male._AvatarData._Face._Head.MakeupList;
					// Assign the destination slot
					DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
					// find color
					Color color =  new Color (UnityEngine.Random.Range(0.1f,0.9f),UnityEngine.Random.Range(0.1f,0.9f),UnityEngine.Random.Range(0.1f,0.9f),1);
					// assign the slot and its overlays and color
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_Head_MakeUp", true, color);	// assign the slot and its overlays				
				}
				// _Lips
				if ( _Race._Male._AvatarData._Face._Head.LipsList.Count > 0 ){
					// assign the overlays list
					TmpOvlist = _Race._Male._AvatarData._Face._Head.LipsList;
					// Assign the destination slot
					DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
					// find color
					Color color =  new Color (UnityEngine.Random.Range(0,1),UnityEngine.Random.Range(0,1),UnityEngine.Random.Range(0,1),1);
					// assign the slot and its overlays and color
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_Lips", true, color);	// assign the slot and its overlays				
				}

				#region _FaceHair
				// _EyeBrows
				if ( _Race._Male._AvatarData._Face._FaceHair.EyeBrowsList.Count > 0 ){
					// color preparation
					float AdjRan = UnityEngine.Random.Range(0,Crowd.Colors.HairAdjRanMaxi);
					Crowd.Colors.ColorToApply = Crowd.Colors.HairColor;
					Color _Adjust = new Color(AdjRan ,AdjRan,AdjRan,1);
					Color color = Crowd.Colors.ColorToApply + _Adjust;
					
					TmpOvlist = _Race._Male._AvatarData._Face._FaceHair.EyeBrowsList;
					DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_EyeBrows", true, color);	// assign the slot and its overlays				
				}
				
				#region  _BeardOverlayOnly
				if ( _Race._Male._AvatarData._Face._FaceHair._BeardOverlayOnly.BeardList.Count > 0 ){
					if ( Crowd.Randomize.Pilosity != "None" ){
						// color preparation
						float AdjRan = UnityEngine.Random.Range(0,Crowd.Colors.HairAdjRanMaxi);
						Crowd.Colors.ColorToApply = Crowd.Colors.HairColor;
						Color _Adjust = new Color(AdjRan ,AdjRan,AdjRan,1);
						Color color = Crowd.Colors.ColorToApply + _Adjust;
						
						// Reset and prepare Pilosity
						Crowd.Randomize.PiloAmount = 0;
						if ( Crowd.Randomize.Pilosity == "Random" ) Crowd.Randomize.PiloMaxi = UnityEngine.Random.Range(0, 3);
						if ( Crowd.Randomize.Pilosity == "Low" ) Crowd.Randomize.PiloMaxi = 1;
						if ( Crowd.Randomize.Pilosity == "Medium" ) Crowd.Randomize.PiloMaxi = 2;
						if ( Crowd.Randomize.Pilosity == "High" ) Crowd.Randomize.PiloMaxi = 3;
						// Assign the Beard1 overlay
						int Ran = UnityEngine.Random.Range(0, 10 );
						if ( Crowd.Randomize.PiloAmount < Crowd.Randomize.PiloMaxi 
						    && Ran >= 5 )
						{ 
							TmpOvlist = _Race._Male._AvatarData._Face._FaceHair._BeardOverlayOnly.BeardList;
							DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
							DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_Beard1", true,color);	// assign the slot and its overlays				
							Crowd.Randomize.PiloAmount = 1;
						}
						// Assign the Beard2 overlay
						Ran = UnityEngine.Random.Range(0, 10 );
						if ( Crowd.Randomize.PiloAmount < Crowd.Randomize.PiloMaxi 
						    && Ran >= 5 )
						{ 
							TmpOvlist = _Race._Male._AvatarData._Face._FaceHair._BeardOverlayOnly.BeardList;
							DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
							DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_Beard2", true,color);	// assign the slot and its overlays				
							Crowd.Randomize.PiloAmount = 2;
						}
						// Assign the Beard3 overlay
						Ran = UnityEngine.Random.Range(0, 10 );
						if ( Crowd.Randomize.PiloAmount < Crowd.Randomize.PiloMaxi 
						    && Ran >= 5 )
						{ 
							TmpOvlist = _Race._Male._AvatarData._Face._FaceHair._BeardOverlayOnly.BeardList;
							DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
							DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_Beard3", true, color);	// assign the slot and its overlays				
							Crowd.Randomize.PiloAmount = 3;
						}
					}
				}
				#endregion _BeardOverlayOnly
				
				#region _BeardSlotOnly
				if ( _Race._Male._AvatarData._Face._FaceHair._BeardSlotOnly.SlotList.Count > 0 ){
					if ( Crowd.Randomize.Pilosity != "None" /*&& Crowd.Randomize.PiloMaxi == 3*/ ){
						// color preparation
						float AdjRan = UnityEngine.Random.Range(0,Crowd.Colors.HairAdjRanMaxi);
						Crowd.Colors.ColorToApply = Crowd.Colors.HairColor;
						Color _Adjust = new Color(AdjRan ,AdjRan,AdjRan,1);
						Color color = Crowd.Colors.ColorToApply + _Adjust;
						int ran = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Face._FaceHair._BeardSlotOnly.SlotList.Count);
						DKSlotData slot = _Race._Male._AvatarData._Face._FaceHair._BeardSlotOnly.SlotList[ran];
						TmpOvlist = _Race._Male._AvatarData._Face._FaceHair._BeardSlotOnly.OverlayList;
						DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_BeardSlotOnly", color);	// assign the slot and its overlays				
						
					}
				}
				#endregion _BeardSlotOnly
				#endregion _FaceHair

				#region _Face
				// _Ears
				if ( _Race._Male._AvatarData._Face._Ears.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Face._Ears.SlotList.Count);
					DKSlotData slot = _Race._Male._AvatarData._Face._Ears.SlotList[ran];
					TmpOvlist = _Race._Male._AvatarData._Face._Ears.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Ears", color);	// assign the slot and its overlays				
				}
				// _Nose
				if ( _Race._Male._AvatarData._Face._Nose.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Face._Nose.SlotList.Count);
					DKSlotData slot = _Race._Male._AvatarData._Face._Nose.SlotList[ran];
					TmpOvlist = _Race._Male._AvatarData._Face._Nose.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Nose", color);	// assign the slot and its overlays				
				}
				// _EyeLash
				if ( _Race._Male._AvatarData._Face._EyeLash.SlotList.Count > 0 ){
					Color color =  new Color (UnityEngine.Random.Range(0,1),UnityEngine.Random.Range(0,1),UnityEngine.Random.Range(0,1),1);
					int ran = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Face._EyeLash.SlotList.Count);
					DKSlotData slot = _Race._Male._AvatarData._Face._EyeLash.SlotList[ran];
					TmpOvlist = _Race._Male._AvatarData._Face._EyeLash.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_EyeLash", color);	// assign the slot and its overlays				
				}
				// _EyeLids
				if ( _Race._Male._AvatarData._Face._EyeLids.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Face._EyeLids.SlotList.Count);
					DKSlotData slot = _Race._Male._AvatarData._Face._EyeLids.SlotList[ran];
					TmpOvlist = _Race._Male._AvatarData._Face._EyeLids.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_EyeLids", color);	// assign the slot and its overlays				
				}

				// _Mouth
				if ( _Race._Male._AvatarData._Face._Mouth.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Face._Mouth.SlotList.Count);
					DKSlotData slot = _Race._Male._AvatarData._Face._Mouth.SlotList[ran];
					TmpOvlist = _Race._Male._AvatarData._Face._Mouth.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Mouth", color);	// assign the slot and its overlays				
				}
				// _InnerMouth
				if ( _Race._Male._AvatarData._Face._Mouth._InnerMouth.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.InnerMouthColor;
					int ran = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Face._Mouth._InnerMouth.SlotList.Count);
					DKSlotData slot = _Race._Male._AvatarData._Face._Mouth._InnerMouth.SlotList[ran];
					TmpOvlist = _Race._Male._AvatarData._Face._Mouth._InnerMouth.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_InnerMouth", color);	// assign the slot and its overlays				
				}

				#endregion _Face

				#endregion Head

				#region Hair
				// determine if hair slot or only overlay
				int RanHair = UnityEngine.Random.Range(0,100);
				if ( _Race._Male._AvatarData._Hair._OverlayOnly.OverlayList.Count == 0 ) RanHair = 100;
				#region _SlotOnly
				if ( RanHair >= 50 && _Race._Male._AvatarData._Hair._SlotOnly.SlotList.Count > 0 ){
					// color preparation
					float AdjRan = UnityEngine.Random.Range(0,Crowd.Colors.HairAdjRanMaxi);
					Crowd.Colors.ColorToApply = Crowd.Colors.HairColor;
					Color _Adjust = new Color(AdjRan ,AdjRan,AdjRan,1);
					Color color = Crowd.Colors.ColorToApply + _Adjust;

					int ran = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Hair._SlotOnly.SlotList.Count);
					DKSlotData slot = _Race._Male._AvatarData._Hair._SlotOnly.SlotList[ran];
					TmpOvlist = _Race._Male._AvatarData._Hair._SlotOnly.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_HairSlotOnly", color);	// assign the slot and its overlays				

					#region _Hair_Module
					int RanHairModule = UnityEngine.Random.Range(0,100);
					if ( RanHairModule >= 75 && _Race._Male._AvatarData._Hair._SlotOnly._HairModule.SlotList.Count > 0 ){
						int ran1 = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Hair._SlotOnly._HairModule.SlotList.Count);
						DKSlotData slot1 = _Race._Male._AvatarData._Hair._SlotOnly._HairModule.SlotList[ran1];
						TmpOvlist = _Race._Male._AvatarData._Hair._SlotOnly._HairModule.OverlayList;
						DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot1, TmpOvlist, _Race, "_HairSlotOnlyModule", color);	// assign the slot and its overlays				
					}
					#endregion _Hair_Module
				}
				#endregion _SlotOnly
				#region _OverlayOnly
				else if ( _Race._Male._AvatarData._Hair._OverlayOnly.OverlayList.Count > 0 ){
					// color preparation
					float AdjRan = UnityEngine.Random.Range(0,Crowd.Colors.HairAdjRanMaxi);
					Crowd.Colors.ColorToApply = Crowd.Colors.HairColor;
					Color _Adjust = new Color(AdjRan ,AdjRan,AdjRan,1);
					Color color = Crowd.Colors.ColorToApply + _Adjust;

					TmpOvlist = _Race._Male._AvatarData._Hair._OverlayOnly.OverlayList;
					DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_HairOverlayOnly", true, color);	// assign the slot and its overlays				
				}
				#endregion _OverlayOnly

				#endregion Hair

				if ( DK_RPG_UMA_Generator.AddToRPG ){
				//	HeadSlotsList = Crowd.tempSlotList;
				}
				#region Body
				// _Torso
				if ( _Race._Male._AvatarData._Body._Torso.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Body._Torso.SlotList.Count);
					DKSlotData slot = _Race._Male._AvatarData._Body._Torso.SlotList[ran];
					TmpOvlist = _Race._Male._AvatarData._Body._Torso.OverlayList;
					Crowd.TorsoIndex = Crowd.tempSlotList.Count;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Torso", color);	// assign the slot and its overlays				
				}
				// _Tatoo
				ranTatoo = UnityEngine.Random.Range(0,100);
				if ( ranTatoo <= Crowd.Randomize.TatooChance && _Race._Male._AvatarData._Body._Torso.TattooList.Count > 0 ){
					// assign the overlays list
					TmpOvlist = _Race._Male._AvatarData._Body._Torso.TattooList;
					// Assign the destination slot
					DKSlotData slot = Crowd.tempSlotList[ Crowd.TorsoIndex ];
					// find color
					Color color =  new Color (0,0,0,1);
					// assign the slot and its overlays and color
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.TorsoIndex, TmpOvlist, _Race, "_Torso_Tatoo", true, color);	// assign the slot and its overlays				
					
				}
				// _Makup
				ranMakeup = UnityEngine.Random.Range(0,100);
				if ( ranTatoo <= Crowd.Randomize.MakeupChance && _Race._Male._AvatarData._Body._Torso.MakeupList.Count > 0 ){
					// assign the overlays list
					TmpOvlist = _Race._Male._AvatarData._Body._Torso.MakeupList;
					// Assign the destination slot
					DKSlotData slot = Crowd.tempSlotList[ Crowd.TorsoIndex ];
					// find color
					Color color =  new Color (UnityEngine.Random.Range(0.1f,0.9f),UnityEngine.Random.Range(0.1f,0.9f),UnityEngine.Random.Range(0.1f,0.9f),1);
					//	Color color =  new Color (UnityEngine.Random.Range(0,1),UnityEngine.Random.Range(0,1),UnityEngine.Random.Range(0,1),1);
					// assign the slot and its overlays and color
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.TorsoIndex, TmpOvlist, _Race, "_Torso_MakeUp", true, color);	// assign the slot and its overlays				
				}
				// _Hands
				if ( _Race._Male._AvatarData._Body._Hands.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Body._Hands.SlotList.Count);
					DKSlotData slot = _Race._Male._AvatarData._Body._Hands.SlotList[ran];
					TmpOvlist = _Race._Male._AvatarData._Body._Hands.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Hands", color);	// assign the slot and its overlays				
				}
				// _Legs
				if ( _Race._Male._AvatarData._Body._Legs.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Body._Legs.SlotList.Count);
					DKSlotData slot = _Race._Male._AvatarData._Body._Legs.SlotList[ran];
					TmpOvlist = _Race._Male._AvatarData._Body._Legs.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Legs", color);	// assign the slot and its overlays				
				}
				// _Feet
				if ( _Race._Male._AvatarData._Body._Feet.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Male._AvatarData._Body._Feet.SlotList.Count);
					DKSlotData slot = _Race._Male._AvatarData._Body._Feet.SlotList[ran];
					TmpOvlist = _Race._Male._AvatarData._Body._Feet.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Feet", color);	// assign the slot and its overlays				
				}
				#endregion Body
			
				#region Underwear
				// _Underwear
				if ( _Race._Male._AvatarData._Body._Underwear.OverlayList.Count > 0 ){
					// color preparation
					float AdjRan1 = UnityEngine.Random.Range(0,1);
					float AdjRan2 = UnityEngine.Random.Range(0,1);
					float AdjRan3 = UnityEngine.Random.Range(0,1);
					Color color = new Color(AdjRan1 ,AdjRan2,AdjRan3);

					TmpOvlist = _Race._Male._AvatarData._Body._Underwear.OverlayList;
					DKSlotData slot = Crowd.tempSlotList[ Crowd.TorsoIndex ];
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.TorsoIndex, TmpOvlist, _Race, "_Underwear", true, color);	// assign the slot and its overlays				
				}

				#endregion Underwear
			}
			#endregion Male
		
			#region Female
			if ( _Gender == "Female" ){
				#region Head
				// _Eyes
				if ( _Race._Female._AvatarData._Face._Eyes.SlotList.Count > 0 ){
					Crowd.Colors.EyesColor = new Color (1,1,1);
					Color color =   Crowd.Colors.EyesColor;
					int ran = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Face._Eyes.SlotList.Count);
					DKSlotData slot = _Race._Female._AvatarData._Face._Eyes.SlotList[ran];
					TmpOvlist = _Race._Female._AvatarData._Face._Eyes.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Eyes", color);	// assign the slot and its overlays				
				}
				// Assign the Eye Adjust overlay
				if ( _Race._Female._AvatarData._Face._Eyes.AdjustList.Count > 0 )
				{ 
					Crowd.Colors.EyeOverlayAdjustColor1 = UnityEngine.Random.Range(0.1f,0.9f);
					Crowd.Colors.EyeOverlayAdjustColor2 = UnityEngine.Random.Range(0.1f,0.9f);
					Crowd.Colors.EyeOverlayAdjustColor3 = UnityEngine.Random.Range(0.1f,0.9f);
					Crowd.Colors.EyeOverlayAdjustColor = new Color(Crowd.Colors.EyeOverlayAdjustColor1 , Crowd.Colors.EyeOverlayAdjustColor2 , Crowd.Colors.EyeOverlayAdjustColor3,1);
					Crowd.Colors.EyesColor = new Color(Crowd.Colors.EyeOverlayAdjustColor1 ,Crowd.Colors.EyeOverlayAdjustColor2,Crowd.Colors.EyeOverlayAdjustColor3,1);
					Crowd.Colors.EyeOverlayAdjustColor = new Color(Crowd.Colors.EyeOverlayAdjustColor1 ,Crowd.Colors.EyeOverlayAdjustColor2,Crowd.Colors.EyeOverlayAdjustColor3,1);
					Color color =   Crowd.Colors.EyeOverlayAdjustColor;
					
					TmpOvlist = _Race._Female._AvatarData._Face._Eyes.AdjustList;
					DKSlotData slot = Crowd.tempSlotList[ Crowd.tempSlotList.Count-1 ];
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.tempSlotList.Count-1, TmpOvlist, _Race, "_EyesAdjust", true, color);	// assign the slot and its overlays				
				}
				#endregion _Face

				// _Head
				if ( _Race._Female._AvatarData._Face._Head.SlotList.Count > 0 ){
					int ran = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Face._Head.SlotList.Count);
					DKSlotData slot = _Race._Female._AvatarData._Face._Head.SlotList[ran];
					// Special : Assign face slot to variable
					Crowd.FaceSlot = Crowd.tempSlotList.Count;	
					// assign the overlays list
					TmpOvlist = _Race._Female._AvatarData._Face._Head.OverlayList;
					// find color
					Color color =  Crowd.Colors.skinColor;
					// assign the slot and its overlays and color
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Head", color);	
					
				}
				// _Tatoo
				ranTatoo = UnityEngine.Random.Range(0,100);
				if ( ranTatoo <= Crowd.Randomize.TatooChance && _Race._Female._AvatarData._Face._Head.TattooList.Count > 0 ){
					// assign the overlays list
					TmpOvlist = _Race._Female._AvatarData._Face._Head.TattooList;
					// Assign the destination slot
					DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
					// find color
					Color color =  new Color (0,0,0,1);
					// assign the slot and its overlays and color
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_Head_Tatoo", true, color);	// assign the slot and its overlays				

				}
				// _Makup
				ranMakeup = UnityEngine.Random.Range(0,100);
				if ( ranTatoo <= Crowd.Randomize.MakeupChance && _Race._Female._AvatarData._Face._Head.MakeupList.Count > 0 ){
					// assign the overlays list
					TmpOvlist = _Race._Female._AvatarData._Face._Head.MakeupList;
					// Assign the destination slot
					DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
					// find color
					Color color =  new Color (UnityEngine.Random.Range(0.1f,0.9f),UnityEngine.Random.Range(0.1f,0.9f),UnityEngine.Random.Range(0.1f,0.9f),1);
					// assign the slot and its overlays and color
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_Head_MakeUp", true, color);	// assign the slot and its overlays				
				}

				// _Lips
				if ( _Race._Female._AvatarData._Face._Head.LipsList.Count > 0 ){
					// assign the overlays list
					TmpOvlist = _Race._Female._AvatarData._Face._Head.LipsList;
					// Assign the destination slot
					DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
					// find color
					Color color =  new Color (UnityEngine.Random.Range(0,1),UnityEngine.Random.Range(0,1),UnityEngine.Random.Range(0,1),1);
					// assign the slot and its overlays and color
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_Lips", true, color);	// assign the slot and its overlays				
				}
				
				#region _FaceHair
				// _EyeBrows
				if ( _Race._Female._AvatarData._Face._FaceHair.EyeBrowsList.Count > 0 ){
					// color preparation
					float AdjRan = UnityEngine.Random.Range(0,Crowd.Colors.HairAdjRanMaxi);
					Crowd.Colors.ColorToApply = Crowd.Colors.HairColor;
					Color _Adjust = new Color(AdjRan ,AdjRan,AdjRan,1);
					Color color = Crowd.Colors.ColorToApply + _Adjust;
					
					TmpOvlist = _Race._Female._AvatarData._Face._FaceHair.EyeBrowsList;
					DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_EyeBrows", true, color);	// assign the slot and its overlays				
				}
				
				#region  _BeardOverlayOnly
				if ( _Race._Female._AvatarData._Face._FaceHair._BeardOverlayOnly.BeardList.Count > 0 ){
					if ( Crowd.Randomize.Pilosity != "None" ){
						// color preparation
						float AdjRan = UnityEngine.Random.Range(0,Crowd.Colors.HairAdjRanMaxi);
						Crowd.Colors.ColorToApply = Crowd.Colors.HairColor;
						Color _Adjust = new Color(AdjRan ,AdjRan,AdjRan,1);
						Color color = Crowd.Colors.ColorToApply + _Adjust;
						
						// Reset and prepare Pilosity
						Crowd.Randomize.PiloAmount = 0;
						if ( Crowd.Randomize.Pilosity == "Random" ) Crowd.Randomize.PiloMaxi = UnityEngine.Random.Range(0, 3);
						if ( Crowd.Randomize.Pilosity == "Low" ) Crowd.Randomize.PiloMaxi = 1;
						if ( Crowd.Randomize.Pilosity == "Medium" ) Crowd.Randomize.PiloMaxi = 2;
						if ( Crowd.Randomize.Pilosity == "High" ) Crowd.Randomize.PiloMaxi = 3;
						// Assign the Beard1 overlay
						int Ran = UnityEngine.Random.Range(0, 10 );
						if ( Crowd.Randomize.PiloAmount < Crowd.Randomize.PiloMaxi 
						    && Ran >= 5 )
						{ 
							TmpOvlist = _Race._Female._AvatarData._Face._FaceHair._BeardOverlayOnly.BeardList;
							DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
							DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_Beard1", true,color);	// assign the slot and its overlays				
							Crowd.Randomize.PiloAmount = 1;
						}
						// Assign the Beard2 overlay
						Ran = UnityEngine.Random.Range(0, 10 );
						if ( Crowd.Randomize.PiloAmount < Crowd.Randomize.PiloMaxi 
						    && Ran >= 5 )
						{ 
							TmpOvlist = _Race._Female._AvatarData._Face._FaceHair._BeardOverlayOnly.BeardList;
							DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
							DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_Beard2", true,color);	// assign the slot and its overlays				
							Crowd.Randomize.PiloAmount = 2;
						}
						// Assign the Beard3 overlay
						Ran = UnityEngine.Random.Range(0, 10 );
						if ( Crowd.Randomize.PiloAmount < Crowd.Randomize.PiloMaxi 
						    && Ran >= 5 )
						{ 
							TmpOvlist = _Race._Female._AvatarData._Face._FaceHair._BeardOverlayOnly.BeardList;
							DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
							DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_Beard3", true, color);	// assign the slot and its overlays				
							Crowd.Randomize.PiloAmount = 3;
						}
					}
				}
				#endregion _BeardOverlayOnly
				
				#region _BeardSlotOnly
				if ( _Race._Female._AvatarData._Face._FaceHair._BeardSlotOnly.SlotList.Count > 0 ){
					if ( Crowd.Randomize.Pilosity != "None" && Crowd.Randomize.PiloMaxi == 3 ){
						// color preparation
						float AdjRan = UnityEngine.Random.Range(0,Crowd.Colors.HairAdjRanMaxi);
						Crowd.Colors.ColorToApply = Crowd.Colors.HairColor;
						Color _Adjust = new Color(AdjRan ,AdjRan,AdjRan,1);
						Color color = Crowd.Colors.ColorToApply + _Adjust;
						int ran = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Face._FaceHair._BeardSlotOnly.SlotList.Count);
						DKSlotData slot = _Race._Female._AvatarData._Face._FaceHair._BeardSlotOnly.SlotList[ran];
						TmpOvlist = _Race._Female._AvatarData._Face._FaceHair._BeardSlotOnly.OverlayList;
						DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_BeardSlotOnly", color);	// assign the slot and its overlays				
						
					}
				}
				#endregion _BeardSlotOnly
				#endregion _FaceHair
				
				#region _Face
				// _Ears
				if ( _Race._Female._AvatarData._Face._Ears.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Face._Ears.SlotList.Count);
					DKSlotData slot = _Race._Female._AvatarData._Face._Ears.SlotList[ran];
					TmpOvlist = _Race._Female._AvatarData._Face._Ears.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Ears", color);	// assign the slot and its overlays				
				}
				// _Nose
				if ( _Race._Female._AvatarData._Face._Nose.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Face._Nose.SlotList.Count);
					DKSlotData slot = _Race._Female._AvatarData._Face._Nose.SlotList[ran];
					TmpOvlist = _Race._Female._AvatarData._Face._Nose.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Nose", color);	// assign the slot and its overlays				
				}
				// _EyeLash
				if ( _Race._Female._AvatarData._Face._EyeLash.SlotList.Count > 0 ){
					Color color =  new Color (UnityEngine.Random.Range(0,1),UnityEngine.Random.Range(0,1),UnityEngine.Random.Range(0,1),1);
					int ran = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Face._EyeLash.SlotList.Count);
					DKSlotData slot = _Race._Female._AvatarData._Face._EyeLash.SlotList[ran];
					TmpOvlist = _Race._Female._AvatarData._Face._EyeLash.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_EyeLash", color);	// assign the slot and its overlays				
				}
				// _EyeLids
				if ( _Race._Female._AvatarData._Face._EyeLids.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Face._EyeLids.SlotList.Count);
					DKSlotData slot = _Race._Female._AvatarData._Face._EyeLids.SlotList[ran];
					TmpOvlist = _Race._Female._AvatarData._Face._EyeLids.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_EyeLids", color);	// assign the slot and its overlays				
				}

				// _Mouth
				if ( _Race._Female._AvatarData._Face._Mouth.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Face._Mouth.SlotList.Count);
					DKSlotData slot = _Race._Female._AvatarData._Face._Mouth.SlotList[ran];
					TmpOvlist = _Race._Female._AvatarData._Face._Mouth.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Mouth", color);	// assign the slot and its overlays				
				}
				// _InnerMouth
				if ( _Race._Female._AvatarData._Face._Mouth._InnerMouth.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.InnerMouthColor;
					int ran = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Face._Mouth._InnerMouth.SlotList.Count);
					DKSlotData slot = _Race._Female._AvatarData._Face._Mouth._InnerMouth.SlotList[ran];
					TmpOvlist = _Race._Female._AvatarData._Face._Mouth._InnerMouth.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_InnerMouth", color);	// assign the slot and its overlays				
				}


				#endregion Head
				
				#region Hair

				// determine if hair slot or only overlay
				int RanHair = UnityEngine.Random.Range(0,100);
				if ( _Race._Female._AvatarData._Hair._OverlayOnly.OverlayList.Count == 0 ) RanHair = 100;

				#region _SlotOnly
				if ( RanHair >= 50 && _Race._Female._AvatarData._Hair._SlotOnly.SlotList.Count > 0 ){
					// color preparation
					float AdjRan = UnityEngine.Random.Range(0,Crowd.Colors.HairAdjRanMaxi);
					Crowd.Colors.ColorToApply = Crowd.Colors.HairColor;
					Color _Adjust = new Color(AdjRan ,AdjRan,AdjRan,1);
					Color color = Crowd.Colors.ColorToApply + _Adjust;
					
					int ran = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Hair._SlotOnly.SlotList.Count);
					DKSlotData slot = _Race._Female._AvatarData._Hair._SlotOnly.SlotList[ran];
					TmpOvlist = _Race._Female._AvatarData._Hair._SlotOnly.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_HairSlotOnly", color);	// assign the slot and its overlays				
					
					#region _Hair_Module
					int RanHairModule = UnityEngine.Random.Range(0,100);
					if ( RanHairModule >= 75 && _Race._Female._AvatarData._Hair._SlotOnly._HairModule.SlotList.Count > 0 ){
						int ran1 = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Hair._SlotOnly._HairModule.SlotList.Count);
						DKSlotData slot1 = _Race._Female._AvatarData._Hair._SlotOnly._HairModule.SlotList[ran1];
						TmpOvlist = _Race._Female._AvatarData._Hair._SlotOnly._HairModule.OverlayList;
						DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot1, TmpOvlist, _Race, "_Hair_Module", color);	// assign the slot and its overlays				
					}
					#endregion _Hair_Module
				}
				#endregion _SlotOnly

				#region _OverlayOnly
				else if ( _Race._Female._AvatarData._Hair._OverlayOnly.OverlayList.Count > 0 ){
					// color preparation
					float AdjRan = UnityEngine.Random.Range(0,Crowd.Colors.HairAdjRanMaxi);
					Crowd.Colors.ColorToApply = Crowd.Colors.HairColor;
					Color _Adjust = new Color(AdjRan ,AdjRan,AdjRan,1);
					Color color = Crowd.Colors.ColorToApply + _Adjust;
					
					TmpOvlist = _Race._Female._AvatarData._Hair._OverlayOnly.OverlayList;
					DKSlotData slot = Crowd.tempSlotList[ Crowd.FaceSlot ];
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.FaceSlot, TmpOvlist, _Race, "_HairOverlayOnly", true, color);	// assign the slot and its overlays				
				}
				#endregion _OverlayOnly

				#endregion Hair

				if ( DK_RPG_UMA_Generator.AddToRPG ){
				//	HeadSlotsList = Crowd.tempSlotList;
				}

				#region Body
				// _Torso
				if ( _Race._Female._AvatarData._Body._Torso.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Body._Torso.SlotList.Count);
					DKSlotData slot = _Race._Female._AvatarData._Body._Torso.SlotList[ran];
					TmpOvlist = _Race._Female._AvatarData._Body._Torso.OverlayList;
					Crowd.TorsoIndex = Crowd.tempSlotList.Count;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Torso", color);	// assign the slot and its overlays				
				}
				// _Tatoo
				ranTatoo = UnityEngine.Random.Range(0,100);
				if ( ranTatoo <= Crowd.Randomize.TatooChance && _Race._Female._AvatarData._Body._Torso.TattooList.Count > 0 ){
					// assign the overlays list
					TmpOvlist = _Race._Female._AvatarData._Body._Torso.TattooList;
					// Assign the destination slot
					DKSlotData slot = Crowd.tempSlotList[ Crowd.TorsoIndex ];
					// find color
					Color color =  new Color (0,0,0,1);
					// assign the slot and its overlays and color
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.TorsoIndex, TmpOvlist, _Race, "_Torso_Tatoo", true, color);	// assign the slot and its overlays				
					
				}
				// _Makup
				ranMakeup = UnityEngine.Random.Range(0,100);
				if ( ranTatoo <= Crowd.Randomize.MakeupChance && _Race._Female._AvatarData._Body._Torso.MakeupList.Count > 0 ){
					// assign the overlays list
					TmpOvlist = _Race._Female._AvatarData._Body._Torso.MakeupList;
					// Assign the destination slot
					DKSlotData slot = Crowd.tempSlotList[ Crowd.TorsoIndex ];
					// find color
					Color color =  new Color (UnityEngine.Random.Range(0.1f,0.9f),UnityEngine.Random.Range(0.1f,0.9f),UnityEngine.Random.Range(0.1f,0.9f),1);
					//	Color color =  new Color (UnityEngine.Random.Range(0,1),UnityEngine.Random.Range(0,1),UnityEngine.Random.Range(0,1),1);
					// assign the slot and its overlays and color
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.TorsoIndex, TmpOvlist, _Race, "_Torso_MakeUp", true, color);	// assign the slot and its overlays				
				}
				// _Hands
				if ( _Race._Female._AvatarData._Body._Hands.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Body._Hands.SlotList.Count);
					DKSlotData slot = _Race._Female._AvatarData._Body._Hands.SlotList[ran];
					TmpOvlist = _Race._Female._AvatarData._Body._Hands.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Hands", color);	// assign the slot and its overlays				
				}
				// _Legs
				if ( _Race._Female._AvatarData._Body._Legs.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Body._Legs.SlotList.Count);
					DKSlotData slot = _Race._Female._AvatarData._Body._Legs.SlotList[ran];
					TmpOvlist = _Race._Female._AvatarData._Body._Legs.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Legs", color);	// assign the slot and its overlays				
				//	Debug.Log ( slot.slotName );
				}
				// _Feet
				if ( _Race._Female._AvatarData._Body._Feet.SlotList.Count > 0 ){
					Color color =   Crowd.Colors.skinColor;
					int ran = UnityEngine.Random.Range (0, _Race._Female._AvatarData._Body._Feet.SlotList.Count);
					DKSlotData slot = _Race._Female._AvatarData._Body._Feet.SlotList[ran];
					TmpOvlist = _Race._Female._AvatarData._Body._Feet.OverlayList;
					DK_RPG_DefineSlotBody.AssigningSlot (Crowd, slot, TmpOvlist, _Race, "_Feet", color);	// assign the slot and its overlays				
				}
				#region Underwear
				// _Underwear
				if ( _Race._Female._AvatarData._Body._Underwear.OverlayList.Count > 0 ){
					// color preparation
					float AdjRan1 = UnityEngine.Random.Range(0f,1f);
					float AdjRan2 = UnityEngine.Random.Range(0f,1f);
					float AdjRan3 = UnityEngine.Random.Range(0f,1f);
					Color color = new Color( AdjRan1 , AdjRan2, AdjRan3 );
					
					TmpOvlist = _Race._Female._AvatarData._Body._Underwear.OverlayList;
					DKSlotData slot = Crowd.tempSlotList[ Crowd.TorsoIndex ];
					DK_RPG_DefineSlotBody.AssigningOverlay (Crowd, slot, Crowd.TorsoIndex, TmpOvlist, _Race, "_Underwear", true, color);	// assign the slot and its overlays				
				}
				
				#endregion Underwear

				#endregion Body
			//	if ( DK_RPG_UMA_Generator.AddToRPG )	BodySlotsList = Crowd.tempSlotList;
			}
			#endregion Female
			if ( DK_RPG_UMA_Generator.AddToRPG ){
				_DK_RPG_UMA._Avatar.HeadIndex = Crowd.HeadIndex;
				_DK_RPG_UMA._Avatar.TorsoIndex = Crowd.TorsoIndex;
			}
		}
		if ( DK_UMACrowd.GenerateWear == false ) Cleaning (Crowd);
		else Finishing ( Crowd );

	}

	public static void AssigningSlot (DK_UMACrowd Crowd, DKSlotData slot, List<DKOverlayData> list, DKRaceData _Race, string type, Color color){

		// Verify if not already in the list
		if ( AssignedSlotsList.Contains(slot) == false ){

			// Assign the Slot
			AssignedSlotsList.Add (slot);

			if ( type == "_EyeLids" || type == "_Mouth" ){
				Crowd.tempSlotList.Add(Crowd.slotLibrary.InstantiateSlot(slot.slotName,Crowd.tempSlotList[Crowd.FaceSlot].overlayList));
				if ( DK_RPG_UMA_Generator.AddToRPG == true ) AddToRPG (Crowd, slot, overlay, type, false, color );

			}
			else if ( type == "_Hands" || type == "_Legs" || type == "_Feet" ){
				Crowd.tempSlotList.Add(Crowd.slotLibrary.InstantiateSlot(slot.slotName,Crowd.tempSlotList[Crowd.TorsoIndex].overlayList));
				if ( DK_RPG_UMA_Generator.AddToRPG == true ) AddToRPG (Crowd, slot, overlay, type, false, color );

			}
			else {

				int index = Crowd.tempSlotList.Count;
				Crowd.tempSlotList.Add(Crowd.slotLibrary.InstantiateSlot( slot.slotName ));

				AssigningOverlay (Crowd, slot, index, list, _Race, type, false, color);
			}
			// Copy the values
			CopyValues (Crowd, slot, Crowd.tempSlotList.Count-1);
		}
	}

	public static DKOverlayData overlay;
	public static void AssigningOverlay (DK_UMACrowd Crowd, DKSlotData slot, int index, List<DKOverlayData> list, DKRaceData _Race, string type, bool OverlayOnly, Color color){
		Color ColorToApply;
		if ( type == "_EyeLash" ) ColorToApply = Crowd.Colors.EyeOverlayAdjustColor;
		else ColorToApply = color;

		// choose a linked overlay only
		if ( !OverlayOnly && slot.LinkedOverlayList.Count > 0 ){
			int ranOv = UnityEngine.Random.Range (0, slot.LinkedOverlayList.Count);
			overlay = slot.LinkedOverlayList[ranOv];

			// find color presets if available
			if ( overlay.ColorPresets.Count > 0 ){
				int ranColorPreset = UnityEngine.Random.Range (0, overlay.ColorPresets.Count);
				ColorToApply = overlay.ColorPresets[ranColorPreset].PresetColor;
				// adjust the color
				float ranAdj = 0;
				if ( slot.OverlayType.Contains("Hair") ) ranAdj = UnityEngine.Random.Range (0.01f, Crowd.Colors.HairAdjRanMaxi);
				else ranAdj = UnityEngine.Random.Range (0.01f, Crowd.Colors.AdjRanMaxi);
				Color adj = new Color (ranAdj,ranAdj,ranAdj);
				ColorToApply = ColorToApply + adj;
			}

			// Verify if not already in the list
			if ( AssignedOverlayList.Contains(overlay) == false ){
				// Assign the Slot
				AssignedOverlayList.Add (overlay);
				Crowd.tempSlotList[index].overlayList.Add(Crowd.overlayLibrary.InstantiateOverlay(overlay.name,ColorToApply));
				Crowd.tempSlotList[index].overlayList[Crowd.tempSlotList[index].overlayList.Count-1].OverlayType
					= overlay.OverlayType;
				if ( type == "_Head" ) Crowd._FaceOverlay = overlay;
				if ( DK_RPG_UMA_Generator.AddToRPG == true ) AddToRPG (Crowd, slot, overlay, type, false, ColorToApply );
			}
		}

		else if ( ( type == "_Ears" || type == "_Nose" ) && slot.LinkedOverlayList.Count == 0 ){
			Crowd.tempSlotList[index].overlayList.Add(Crowd.overlayLibrary.InstantiateOverlay(Crowd._FaceOverlay.name,ColorToApply));
			if ( DK_RPG_UMA_Generator.AddToRPG == true ) AddToRPG (Crowd, slot, Crowd._FaceOverlay, type, false, ColorToApply );
		}
		
		// if no linked overlay, choose a random overlay
		else {
			if ( type == "_EyeLash"  ) Debug.Log ( "Test Eyelash "+slot.slotName+" "+list.Count);
			if ( list.Count > 0 ) {
				int ranOv = UnityEngine.Random.Range (0, list.Count);
				overlay = list[ranOv];


				// find color presets if available
				if ( overlay == null ) Debug.LogError ("overlay is missing" );

				if ( overlay.ColorPresets.Count > 0 ){
					int ranColorPreset = UnityEngine.Random.Range (0, overlay.ColorPresets.Count);
					ColorToApply = overlay.ColorPresets[ranColorPreset].PresetColor;
					// adjust the color
					float ranAdj = 0;
					if ( slot.OverlayType.Contains("Wear") ) ranAdj = UnityEngine.Random.Range (0.01f, Crowd.Colors.WearAdjRanMaxi);
					else if ( slot.OverlayType.Contains("Hair") ) ranAdj = UnityEngine.Random.Range (0.01f, Crowd.Colors.HairAdjRanMaxi);
					
					Color adj = new Color (ranAdj,ranAdj,ranAdj);
					ColorToApply = ColorToApply + adj;
				//	Debug.Log ("Color Preset :"+overlay.ColorPresets[ranColorPreset].ColorPresetName );
				}
				else ColorToApply = color;

				// Verify if not already in the list
				if ( AssignedOverlayList.Contains(overlay) == false ){
					// Assign to Slot
					AssignedOverlayList.Add (overlay);
					Crowd.tempSlotList[index].overlayList.Add(Crowd.overlayLibrary.InstantiateOverlay(overlay.name,ColorToApply));
					Crowd.tempSlotList[index].overlayList[Crowd.tempSlotList[index].overlayList.Count-1].OverlayType
						= overlay.OverlayType;
					if ( type == "_Head" ) Crowd._FaceOverlay = overlay;
				}
				if ( DK_RPG_UMA_Generator.AddToRPG == true ) AddToRPG (Crowd, slot, overlay, type, true, ColorToApply );
			}
		}

		if ( type == "_EyesAdjust" ){
			if ( DK_RPG_UMA_Generator.AddToRPG == true ) AddToRPG (Crowd, slot, overlay, type, false, ColorToApply );
		}
	}

	public static void AddToRPG ( DK_UMACrowd Crowd, DKSlotData slot, DKOverlayData overlay, string type, bool OverlayOnly, Color color){

		// Add the slot to the RPG values of the Avatar
	/*	if ( type == "_Belt" ){
			if ( OverlayOnly == false ){
				tmp_DK_RPG_UMA._Equipment._Belt.Slot = slot;
				tmp_DK_RPG_UMA._Equipment._Belt.Overlay = overlay;
				tmp_DK_RPG_UMA._Equipment._Belt.Color = color;
			}
			else{ 
				tmp_DK_RPG_UMA._Equipment._Belt.Overlay = overlay;
				tmp_DK_RPG_UMA._Equipment._Belt.Color = color;
			}
		}*/
		#region Head
		if ( type == "_Head" ){
			_DK_RPG_UMA._Avatar._Face._Head.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._Head.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Face._Head.Color = color;
			_DK_RPG_UMA._Avatar.SkinColor = color;
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
		else if ( type == "_Beard2" ){
			_DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard2 = overlay;
			_DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard2Color = color;
			
		}
		else if ( type == "_Beard3" ){
			_DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard3 = overlay;
			_DK_RPG_UMA._Avatar._Face._FaceHair._BeardOverlayOnly.Beard3Color = color;
		}
		#endregion FaceHair
		else if ( type == "_EyeLash" ){
		//	Debug.Log ( "Test Eyelash");
			_DK_RPG_UMA._Avatar._Face._EyeLash.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._EyeLash.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Face._EyeLash.Color = color;
		}
		else if ( type == "_EyeLids" ){
			_DK_RPG_UMA._Avatar._Face._EyeLids.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._EyeLids.Overlay = _DK_RPG_UMA._Avatar._Face._Head.Overlay;
			_DK_RPG_UMA._Avatar._Face._EyeLids.Color = color;
		}

		else if ( type == "_Eyes" ){
			_DK_RPG_UMA._Avatar._Face._Eyes.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._Eyes.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Face._Eyes.Color = color;
			_DK_RPG_UMA._Avatar.EyeColor = color;
		
		}else if ( type == "_EyesAdjust" ){
		//	Debug.Log ("test adjust" );
			_DK_RPG_UMA._Avatar._Face._Eyes.Adjust = overlay;
			_DK_RPG_UMA._Avatar._Face._Eyes.AdjustColor = color;
			_DK_RPG_UMA._Avatar.EyeColor = color;
		}

		else if ( type == "_Ears" ){
			_DK_RPG_UMA._Avatar._Face._Ears.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._Ears.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Face._Ears.Color = color;
		}
		else if ( type == "_Nose" ){
			_DK_RPG_UMA._Avatar._Face._Nose.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._Nose.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Face._Nose.Color = color;
		}
		else if ( type == "_Mouth" ){
			_DK_RPG_UMA._Avatar._Face._Mouth.Slot = slot;
			_DK_RPG_UMA._Avatar._Face._Mouth.Overlay = _DK_RPG_UMA._Avatar._Face._Head.Overlay;
			_DK_RPG_UMA._Avatar._Face._Mouth.Color = color;
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
			_DK_RPG_UMA._Avatar.HairColor = color;
		}
		// Overlay only
		else if ( type == "_HairOverlayOnly" ){
			_DK_RPG_UMA._Avatar._Hair._OverlayOnly.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Hair._OverlayOnly.Color = color;
			_DK_RPG_UMA._Avatar.HairColor = color;
		}
		#endregion Hair

		#region Body

		#region _Torso
		if ( type == "_Torso" ){
			_DK_RPG_UMA._Avatar._Body._Torso.Slot = slot;
			_DK_RPG_UMA._Avatar._Body._Torso.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Body._Torso.Color = color;
		}
		else if ( type == "_Torso_Tatoo" ){
			_DK_RPG_UMA._Avatar._Body._Torso.Tattoo = overlay;
			_DK_RPG_UMA._Avatar._Body._Torso.TattooColor = color;
		}
		else if ( type == "_Torso_MakeUp" ){
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
	/*	else if ( type == "_Hands_Tatoo" ){
			_DK_RPG_UMA._Avatar._Body._Hands.Tattoo = overlay;
			_DK_RPG_UMA._Avatar._Body._Hands.TattooColor = color;
		}
		else if ( type == "_Hands_MakeUp" ){
			_DK_RPG_UMA._Avatar._Body._Hands.Makeup = overlay;
			_DK_RPG_UMA._Avatar._Body._Hands.MakeupColor = color;
		}*/
		#endregion _Hands
		#region _Legs
		if ( type == "_Legs" ){
			_DK_RPG_UMA._Avatar._Body._Legs.Slot = slot;
			_DK_RPG_UMA._Avatar._Body._Legs.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Body._Legs.Color = color;
		}
	/*	else if ( type == "_Legs_Tatoo" ){
			_DK_RPG_UMA._Avatar._Body._Legs.Tattoo = overlay;
			_DK_RPG_UMA._Avatar._Body._Legs.TattooColor = color;
		}
		else if ( type == "_Legs_MakeUp" ){
			_DK_RPG_UMA._Avatar._Body._Legs.Makeup = overlay;
			_DK_RPG_UMA._Avatar._Body._Legs.MakeupColor = color;
		}*/
		#endregion _Legs
		#region _Feet
		if ( type == "_Feet" ){
			_DK_RPG_UMA._Avatar._Body._Feet.Slot = slot;
			_DK_RPG_UMA._Avatar._Body._Feet.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Body._Feet.Color = color;
		}
	/*	else if ( type == "_Feet_Tatoo" ){
			_DK_RPG_UMA._Avatar._Body._Feet.Tattoo = overlay;
			_DK_RPG_UMA._Avatar._Body._Feet.TattooColor = color;
		}
		else if ( type == "_Feet_MakeUp" ){
			_DK_RPG_UMA._Avatar._Body._Feet.Makeup = overlay;
			_DK_RPG_UMA._Avatar._Body._Feet.MakeupColor = color;
		}*/
		#endregion _Feet
		#region _Feet
		if ( type == "_Underwear" ){
			_DK_RPG_UMA._Avatar._Body._Underwear.Slot = slot;
			_DK_RPG_UMA._Avatar._Body._Underwear.Overlay = overlay;
			_DK_RPG_UMA._Avatar._Body._Underwear.Color = color;
		}

		#endregion _Feet
		#endregion Body
	}

	public static void CopyValues (DK_UMACrowd Crowd, DKSlotData slot, int index){
		Crowd.tempSlotList[index].OverlayType = slot.OverlayType;
		Crowd.tempSlotList[index].Place = slot.Place;
		Crowd.tempSlotList[index]._UMA = slot._UMA;
		Crowd.tempSlotList[index].Replace = slot.Replace;
		Crowd.tempSlotList[index]._LegacyData.HasLegacy = slot._LegacyData.HasLegacy;
		Crowd.tempSlotList[index]._LegacyData.LegacyList = slot._LegacyData.LegacyList;
		Crowd.tempSlotList[index]._LegacyData.IsLegacy = slot._LegacyData.IsLegacy;
		Crowd.tempSlotList[index]._LegacyData.ElderList = slot._LegacyData.ElderList;
	}

	public static void Cleaning( DK_UMACrowd Crowd ){
		#region Finishing
		List<DKSlotData> ToRemoveList = new List<DKSlotData>();
		#region Cleaning Avatar
		foreach( DKSlotData slot in Crowd.tempSlotList){
			#region if Replace activated
			if ( slot.Replace == true ) {
				for(int i1 = 0; i1 < Crowd.tempSlotList.Count; i1 ++){
					if ( slot.Place.dk_SlotsAnatomyElement.Place == Crowd.tempSlotList[i1].Place ) {
						ToRemoveList.Add(Crowd.tempSlotList[i1]);
					}
				}
			}
			#endregion if Replace activated
			
			#region hide Handled
			// detect 'hide Handled'
			if (  DK_UMACrowd.GenerateWear == false && DK_UMACrowd.GenerateHandled == false ){
				if ( slot.Place.name.Contains("Handled") == true ){
					ToRemoveList.Add( slot );
				}
			}
			#endregion hide Handled
		}
		
		#region Legacy
		for(int i = 0; i < Crowd.tempSlotList.Count; i ++){
			// detect Legacy
			DKSlotData Slot = Crowd.tempSlotList[i];
			if ( Slot._LegacyData.HasLegacy == true ) {
				if ( Slot._LegacyData.LegacyList.Count > 0 ){
					//	int Ran = 0;
					foreach ( DKSlotData LegacySlot in Slot._LegacyData.LegacyList ){
						// select the overlay
						try {
							if ( LegacySlot.overlayList.Count > 0
							    || (LegacySlot.OverlayType != null && LegacySlot.OverlayType == "Flesh") )
							{
								//				Ran = UnityEngine.Random.Range(0, LegacySlot.overlayList.Count-1);
								//	LegacySlot = Slot._LegacyData.LegacyList[i4];
							}
							
							#region Choose Color to apply
							if ( LegacySlot.OverlayType == "Hair" ) Crowd.Colors.ColorToApply =Crowd.Colors.HairColor;
							else if ( LegacySlot.OverlayType == "Beard" ) Crowd.Colors.ColorToApply =Crowd.Colors.HairColor;
							else if ( LegacySlot.OverlayType == "Eyes" ) Crowd.Colors.ColorToApply =Crowd.Colors.EyesColor;
							else if ( LegacySlot.OverlayType == "Face" ) Crowd.Colors.ColorToApply =Crowd.Colors.skinColor;
							else if ( LegacySlot.OverlayType == "Flesh" ) Crowd.Colors.ColorToApply =Crowd.Colors.skinColor;
							else if ( LegacySlot.Place && LegacySlot.Place.name == "InnerMouth" ) Crowd.Colors.ColorToApply =Crowd.Colors.InnerMouthColor;

							else {

								Crowd.Colors.ColorToApply = new Color (UnityEngine.Random.Range(0.01f,0.9f),UnityEngine.Random.Range(0.01f,0.9f),UnityEngine.Random.Range(0.01f,0.9f));
								//	Colors.BeltWearColor1 = UnityEngine.Random.Range(0.01f,0.9f);
							}
							#endregion Choose Color to apply
							
							// add
							// For flesh
							if ( LegacySlot.OverlayType != null && LegacySlot.OverlayType == "Flesh" ) {
								bool AlreadyIn = false;
								DKSlotData placeHolder = ScriptableObject.CreateInstance("DKSlotData") as DKSlotData;
								foreach ( DKSlotData slot in Crowd.tempSlotList ){
									if ( slot.slotName == LegacySlot.slotName ) AlreadyIn = true;
									if ( slot.Place == LegacySlot.Place ) {
										placeHolder = slot;
										
									}
								}
								if ( AssignedSlotsList.Contains(LegacySlot) == true ) AlreadyIn = true;
								if ( !AlreadyIn ){
									DKSlotData slot = Crowd.slotLibrary.InstantiateSlot(LegacySlot.slotName);
									Crowd.tempSlotList.Add(slot);
									slot._LegacyData.IsLegacy = true;
									slot._LegacyData.ElderList.Add(Crowd.tempSlotList[Crowd.tempSlotList.Count-1]);
									Crowd.tempSlotList[Crowd.tempSlotList.Count-1].overlayList = TmpTorsoOverLayList;
									AssignedSlotsList.Add (LegacySlot);
									Crowd.umaData.tmpRecipeList = Crowd.tempSlotList;
								}
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
								Crowd.tempSlotList.Add(slot);

								slot._LegacyData.IsLegacy = true;
								slot._LegacyData.ElderList.Add(Crowd.tempSlotList[Crowd.tempSlotList.Count-1]);

								if ( Crowd.tempSlotList[Crowd.tempSlotList.Count-1].overlayList.Count == 0 ){

									// define legacy slot's overlay
									if ( LegacySlot.LinkedOverlayList.Count > 0 ) {
										int ran = UnityEngine.Random.Range(0, LegacySlot.LinkedOverlayList.Count-1);
										// define color preset
										if (LegacySlot.LinkedOverlayList[ran].ColorPresets.Count > 0) {
											int ran2 = UnityEngine.Random.Range(0, LegacySlot.LinkedOverlayList[ran].ColorPresets.Count-1);
											Crowd.Colors.ColorToApply = LegacySlot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
										}
										AssignedOverlayList.Add (LegacySlot.LinkedOverlayList[ran]);
										Crowd.tempSlotList[Crowd.tempSlotList.Count-1].overlayList.Add(Crowd.overlayLibrary.InstantiateOverlay(LegacySlot.LinkedOverlayList[ran].name,Crowd.Colors.ColorToApply));
									}
								}
							}
						}
						catch (System.NullReferenceException) {
							if ( LegacySlot == null ){
								Debug.LogError ( "slot '"+Slot.slotName+"' Legacy can't be generated. The legacy slot is missing. Verify the setting of '"+Slot.slotName+"' about the legacy. Skipping the legacy for "+Slot.slotName);
								#if UNITY_EDITOR
								//	EditorWindow.GetWindow(typeof(DK_UMA_Error_Win), false, "DK Errors");
								//	DK_UMA_Error_Win.AddError_LegacyNotFound();
								#endif
								
							}
						}
					}
				}
			}
		}
		
		for(int i = 0; i < Crowd.tempSlotList.Count; i ++){
			// detect Legacy
			DKSlotData Slot = Crowd.tempSlotList[i];
			// remove the legacy item if the elder is missing / deleted
			if ( Slot._LegacyData.IsLegacy == true ) {
				foreach ( DKSlotData ElderSlot in Slot._LegacyData.ElderList ){
					if ( Crowd.tempSlotList.Contains(ElderSlot) == false 
					    || ToRemoveList.Contains(ElderSlot) == true ) ToRemoveList.Add (Slot);
				}
			}
		}
		
		// clear the list of place holders
		foreach (DKSlotData placeHolder in ToRemoveList ){
			Crowd.tempSlotList.Remove (placeHolder);
			//	Debug.Log ("removing "+placeHolder.slotName );
		}
		#endregion Legacy
		
		// Clean the slots of the avatar if no overlay	
		//	for(int i = 0; i < Crowd.tempSlotList.Count; i ++){
		//		if ( Crowd.tempSlotList[i].overlayList.Count == 0 ) Crowd.tempSlotList.Remove(Crowd.tempSlotList[i]);
		//	}
		#endregion Cleaning Avatar

		Finishing ( Crowd );
		#endregion Finishing
	}

	public static void Finishing ( DK_UMACrowd Crowd ){
	//	Debug.Log ( "Finishing RPG Body" );
		if ( DK_UMACrowd.GenerateWear == true ) DK_RPG_DefineSlotWear.DefineSlots ( Crowd );
		else DK_DefineSlotFinishing.Finish(Crowd);
	}
}
