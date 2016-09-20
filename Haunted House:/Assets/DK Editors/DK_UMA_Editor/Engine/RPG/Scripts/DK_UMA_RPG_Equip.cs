using UnityEngine;
using System.Collections;

public class DK_UMA_RPG_Equip : MonoBehaviour {

	public static DKOverlayData _overlay;
	public static Color _color = Color.black;
	public static void PrepareEquipSlotElement (DKSlotData _slot, DKOverlayData overlay, DK_RPG_UMA _DK_RPG_UMA) {
		if ( _slot ){
			// choose overlay
			if ( overlay == null && _slot.LinkedOverlayList.Count > 0 ){
				int ran = Random.Range(0, _slot.LinkedOverlayList.Count);
				_overlay = _slot.LinkedOverlayList[ran];
			}
			else _overlay = overlay;
		}
		else if ( overlay ) {
			_slot = null;
			_overlay = overlay;
			// choose color
			if ( _overlay && _overlay.ColorPresets.Count > 0 ) {
				int ran = Random.Range(0, _overlay.ColorPresets.Count-1);
				_color = _overlay.ColorPresets[ran].PresetColor;
			}
			// if no color overlay, randomise color
			else {
				float ran1 = Random.Range(0f, 0.99f);
				float ran2 = Random.Range(0f, 0.99f);
				float ran3 = Random.Range(0f, 0.99f);
				_color = new Color (ran1, ran2, ran3 );
			}
		}
		if ( _DK_RPG_UMA != null /*&& _color != null*/ )
			EquipSlotElement ( _slot, _overlay, _DK_RPG_UMA, _color );
	}

	public static void EquipSlotElement ( DKSlotData _slot, DKOverlayData _overlay, DK_RPG_UMA _DK_RPG_UMA, Color color ){
		// for a slot element
		if ( _slot != null ){
			#region Equipment
			// find the correct place
			if ( _slot.OverlayType == "FeetWear" ) {
			//	Debug.Log ("FeetWear LinkedOverlayList 1");

				_DK_RPG_UMA._Equipment._Feet.Slot = _slot;
				// if the overlay is already assigned
				if ( _overlay ) {
					_DK_RPG_UMA._Equipment._Feet.Overlay = _overlay;
					if ( _overlay && _overlay.ColorPresets.Count > 0 ) {
						int ran2 = Random.Range(0, _overlay.ColorPresets.Count-1);
						color = _overlay.ColorPresets[ran2].PresetColor;
					}
				}
				// if the overlay is not assigned, random one from the linked overlays of the slot
				else {

					if ( _slot.LinkedOverlayList.Count > 0 ) {
						Debug.Log ("FeetWear LinkedOverlayList 2");
						int ran = Random.Range(0, _slot.LinkedOverlayList.Count-1);
						_DK_RPG_UMA._Equipment._Feet.Overlay = _slot.LinkedOverlayList[ran];
						if ( _slot.LinkedOverlayList[ran] && _slot.LinkedOverlayList[ran].ColorPresets.Count > 0 ) {
							int ran2 = Random.Range(0, _slot.LinkedOverlayList[ran].ColorPresets.Count-1);
							color = _slot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
						}
					}
				}
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Feet.Color = color;
			}
			else if ( _slot.OverlayType == "HandsWear" ) {
				_DK_RPG_UMA._Equipment._Hands.Slot = _slot;
				// if the overlay is already assigned
				if ( _overlay ) {
					_DK_RPG_UMA._Equipment._Hands.Overlay = _overlay;
					if ( _overlay && _overlay.ColorPresets.Count > 0 ) {
						int ran2 = Random.Range(0, _overlay.ColorPresets.Count-1);
						color = _overlay.ColorPresets[ran2].PresetColor;
					}
				}
				// if the overlay is not assigned, random one from the linked overlays of the slot
				else {
					if ( _slot.LinkedOverlayList.Count > 0 ) {
						int ran = Random.Range(0, _slot.LinkedOverlayList.Count-1);
						_DK_RPG_UMA._Equipment._Hands.Overlay = _slot.LinkedOverlayList[ran];
						if ( _slot.LinkedOverlayList[ran] && _slot.LinkedOverlayList[ran].ColorPresets.Count > 0 ) {
							int ran2 = Random.Range(0, _slot.LinkedOverlayList[ran].ColorPresets.Count-1);
							color = _slot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
						}
					}
				}
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Hands.Color = color;
			}
			else if ( _slot.OverlayType == "HeadWear" ) {
				_DK_RPG_UMA._Equipment._Head.Slot = _slot;
				// if the overlay is already assigned
				if ( _overlay ) {
					_DK_RPG_UMA._Equipment._Head.Overlay = _overlay;
					if ( _overlay && _overlay.ColorPresets.Count > 0 ) {
						int ran2 = Random.Range(0, _overlay.ColorPresets.Count-1);
						color = _overlay.ColorPresets[ran2].PresetColor;
					}
				}
				// if the overlay is not assigned, random one from the linked overlays of the slot
				else {
					if ( _slot.LinkedOverlayList.Count > 0 ) {
						int ran = Random.Range(0, _slot.LinkedOverlayList.Count-1);
						_DK_RPG_UMA._Equipment._Head.Overlay = _slot.LinkedOverlayList[ran];
						if ( _slot.LinkedOverlayList[ran] && _slot.LinkedOverlayList[ran].ColorPresets.Count > 0 ) {
							int ran2 = Random.Range(0, _slot.LinkedOverlayList[ran].ColorPresets.Count-1);
							color = _slot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
						}
					}
				}
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Head.Color = color;
			}
			else if ( _slot.OverlayType == "LegsWear" ) {
				_DK_RPG_UMA._Equipment._Legs.Slot = _slot;
				// if the overlay is already assigned
				if ( _overlay ) {
					_DK_RPG_UMA._Equipment._Legs.Overlay = _overlay;
					if ( _overlay && _overlay.ColorPresets.Count > 0 ) {
						int ran2 = Random.Range(0, _overlay.ColorPresets.Count-1);
						color = _overlay.ColorPresets[ran2].PresetColor;
					}
				}
				// if the overlay is not assigned, random one from the linked overlays of the slot
				else {
					if ( _slot.LinkedOverlayList.Count > 0 ) {
						int ran = Random.Range(0, _slot.LinkedOverlayList.Count-1);
						_DK_RPG_UMA._Equipment._Legs.Overlay = _slot.LinkedOverlayList[ran];
						if ( _slot.LinkedOverlayList[ran] && _slot.LinkedOverlayList[ran].ColorPresets.Count > 0 ) {
							int ran2 = Random.Range(0, _slot.LinkedOverlayList[ran].ColorPresets.Count-1);
							color = _slot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
						}
					}
				}
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Legs.Color = color;
			}
			else if ( _slot.OverlayType == "ShoulderWear" ) {
				_DK_RPG_UMA._Equipment._Shoulder.Slot = _slot;
				// if the overlay is already assigned
				if ( _overlay ) {
					_DK_RPG_UMA._Equipment._Shoulder.Overlay = _overlay;
					if ( _overlay && _overlay.ColorPresets.Count > 0 ) {
						int ran2 = Random.Range(0, _overlay.ColorPresets.Count-1);
						color = _overlay.ColorPresets[ran2].PresetColor;
					}
				}
				// if the overlay is not assigned, random one from the linked overlays of the slot
				else {
					if ( _slot.LinkedOverlayList.Count > 0 ) {
						int ran = Random.Range(0, _slot.LinkedOverlayList.Count-1);
						_DK_RPG_UMA._Equipment._Shoulder.Overlay = _slot.LinkedOverlayList[ran];
						if ( _slot.LinkedOverlayList[ran] && _slot.LinkedOverlayList[ran].ColorPresets.Count > 0 ) {
							int ran2 = Random.Range(0, _slot.LinkedOverlayList[ran].ColorPresets.Count-1);
							color = _slot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
						}
					}
				}
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Shoulder.Color = color;
			}
			else if ( _slot.OverlayType == "TorsoWear" ) {
				_DK_RPG_UMA._Equipment._Torso.Slot = _slot;
				// if the overlay is already assigned
				if ( _overlay ) {
					_DK_RPG_UMA._Equipment._Torso.Overlay = _overlay;
					if ( _overlay && _overlay.ColorPresets.Count > 0 ) {
						int ran2 = Random.Range(0, _overlay.ColorPresets.Count-1);
						color = _overlay.ColorPresets[ran2].PresetColor;
					}
				}
				// if the overlay is not assigned, random one from the linked overlays of the slot
				else {
					if ( _slot.LinkedOverlayList.Count > 0 ) {
						int ran = Random.Range(0, _slot.LinkedOverlayList.Count-1);
						_DK_RPG_UMA._Equipment._Torso.Overlay = _slot.LinkedOverlayList[ran];
						if ( _slot.LinkedOverlayList[ran] && _slot.LinkedOverlayList[ran].ColorPresets.Count > 0 ) {
							int ran2 = Random.Range(0, _slot.LinkedOverlayList[ran].ColorPresets.Count-1);
							color = _slot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
						}
					}
				}
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Torso.Color = color;
			}
			else if ( _slot.Place.name == "HandledLeft" ) {
				_DK_RPG_UMA._Equipment._LeftHand.Slot = _slot;
				// if the overlay is already assigned
				if ( _overlay ) {
					_DK_RPG_UMA._Equipment._LeftHand.Overlay = _overlay;
					if ( _overlay && _overlay.ColorPresets.Count > 0 ) {
						int ran2 = Random.Range(0, _overlay.ColorPresets.Count-1);
						color = _overlay.ColorPresets[ran2].PresetColor;
					}
				}
				// if the overlay is not assigned, random one from the linked overlays of the slot
				else {
					if ( _slot.LinkedOverlayList.Count > 0 ) {
						int ran = Random.Range(0, _slot.LinkedOverlayList.Count-1);
						_DK_RPG_UMA._Equipment._LeftHand.Overlay = _slot.LinkedOverlayList[ran];
						if ( _slot.LinkedOverlayList[ran] && _slot.LinkedOverlayList[ran].ColorPresets.Count > 0 ) {
							int ran2 = Random.Range(0, _slot.LinkedOverlayList[ran].ColorPresets.Count-1);
							color = _slot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
						}
					}
				}
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._LeftHand.Color = color;
			}
			else if ( _slot.Place.name == "HandledRight" ) {
				_DK_RPG_UMA._Equipment._RightHand.Slot = _slot;
				// if the overlay is already assigned
				if ( _overlay ) {
					_DK_RPG_UMA._Equipment._RightHand.Overlay = _overlay;
					if ( _overlay && _overlay.ColorPresets.Count > 0 ) {
						int ran2 = Random.Range(0, _overlay.ColorPresets.Count-1);
						color = _overlay.ColorPresets[ran2].PresetColor;
					}
				}
				// if the overlay is not assigned, random one from the linked overlays of the slot
				else {
					if ( _slot.LinkedOverlayList.Count > 0 ) {
						int ran = Random.Range(0, _slot.LinkedOverlayList.Count-1);
						_DK_RPG_UMA._Equipment._RightHand.Overlay = _slot.LinkedOverlayList[ran];
						if ( _slot.LinkedOverlayList[ran] && _slot.LinkedOverlayList[ran].ColorPresets.Count > 0 ) {
							int ran2 = Random.Range(0, _slot.LinkedOverlayList[ran].ColorPresets.Count-1);
							color = _slot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
						}
					}
				}
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._RightHand.Color = color;
			}
			else if ( _slot.Place.name == "BeltWear" ) {
				_DK_RPG_UMA._Equipment._Belt.Slot = _slot;
				// if the overlay is already assigned
				if ( _overlay ) {
					_DK_RPG_UMA._Equipment._Belt.Overlay = _overlay;
					if ( _overlay && _overlay.ColorPresets.Count > 0 ) {
						int ran2 = Random.Range(0, _overlay.ColorPresets.Count-1);
						color = _overlay.ColorPresets[ran2].PresetColor;
					}
				}
				// if the overlay is not assigned, random one from the linked overlays of the slot
				else {
					if ( _slot.LinkedOverlayList.Count > 0 ) {
						int ran = Random.Range(0, _slot.LinkedOverlayList.Count-1);
						_DK_RPG_UMA._Equipment._Belt.Overlay = _slot.LinkedOverlayList[ran];
						if ( _slot.LinkedOverlayList[ran] && _slot.LinkedOverlayList[ran].ColorPresets.Count > 0 ) {
							int ran2 = Random.Range(0, _slot.LinkedOverlayList[ran].ColorPresets.Count-1);
							color = _slot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
						}
					}
				}
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Belt.Color = color;
			}
			else if ( _slot.Place.name == "ArmbandWear" ) {
				_DK_RPG_UMA._Equipment._ArmBand.Slot = _slot;
				// if the overlay is already assigned
				if ( _overlay ) {
					_DK_RPG_UMA._Equipment._ArmBand.Overlay = _overlay;
					if ( _overlay && _overlay.ColorPresets.Count > 0 ) {
						int ran2 = Random.Range(0, _overlay.ColorPresets.Count-1);
						color = _overlay.ColorPresets[ran2].PresetColor;
					}
				}
				// if the overlay is not assigned, random one from the linked overlays of the slot
				else {
					if ( _slot.LinkedOverlayList.Count > 0 ) {
						int ran = Random.Range(0, _slot.LinkedOverlayList.Count-1);
						_DK_RPG_UMA._Equipment._ArmBand.Overlay = _slot.LinkedOverlayList[ran];
						if ( _slot.LinkedOverlayList[ran] && _slot.LinkedOverlayList[ran].ColorPresets.Count > 0 ) {
							int ran2 = Random.Range(0, _slot.LinkedOverlayList[ran].ColorPresets.Count-1);
							color = _slot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
						}
					}
				}
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._ArmBand.Color = color;
			}
			else if ( _slot.Place.name == "WristWear" ) {
				_DK_RPG_UMA._Equipment._Wrist.Slot = _slot;
				// if the overlay is already assigned
				if ( _overlay ) {
					_DK_RPG_UMA._Equipment._Wrist.Overlay = _overlay;
					if ( _overlay && _overlay.ColorPresets.Count > 0 ) {
						int ran2 = Random.Range(0, _overlay.ColorPresets.Count-1);
						color = _overlay.ColorPresets[ran2].PresetColor;
					}
				}
				// if the overlay is not assigned, random one from the linked overlays of the slot
				else {
					if ( _slot.LinkedOverlayList.Count > 0 ) {
						int ran = Random.Range(0, _slot.LinkedOverlayList.Count-1);
						_DK_RPG_UMA._Equipment._Wrist.Overlay = _slot.LinkedOverlayList[ran];
						if ( _slot.LinkedOverlayList[ran] && _slot.LinkedOverlayList[ran].ColorPresets.Count > 0 ) {
							int ran2 = Random.Range(0, _slot.LinkedOverlayList[ran].ColorPresets.Count-1);
							color = _slot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
						}
					}
				}
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Wrist.Color = color;
			}
			else if ( _slot.Place.name == "CloakWear" ) {
				_DK_RPG_UMA._Equipment._Cloak.Slot = _slot;
				// if the overlay is already assigned
				if ( _overlay ) {
					_DK_RPG_UMA._Equipment._Cloak.Overlay = _overlay;
					if ( _overlay && _overlay.ColorPresets.Count > 0 ) {
						int ran2 = Random.Range(0, _overlay.ColorPresets.Count-1);
						color = _overlay.ColorPresets[ran2].PresetColor;
					}
				}
				// if the overlay is not assigned, random one from the linked overlays of the slot
				else {
					if ( _slot.LinkedOverlayList.Count > 0 ) {
						int ran = Random.Range(0, _slot.LinkedOverlayList.Count-1);
						_DK_RPG_UMA._Equipment._Cloak.Overlay = _slot.LinkedOverlayList[ran];
						if ( _slot.LinkedOverlayList[ran] && _slot.LinkedOverlayList[ran].ColorPresets.Count > 0 ) {
							int ran2 = Random.Range(0, _slot.LinkedOverlayList[ran].ColorPresets.Count-1);
							color = _slot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
						}
					}
				}
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Cloak.Color = color;
			}
			else if ( _slot.Place.name == "Collar" ) {
				_DK_RPG_UMA._Equipment._Collar.Slot = _slot;
				// if the overlay is already assigned
				if ( _overlay ) {
					_DK_RPG_UMA._Equipment._Collar.Overlay = _overlay;
					if ( _overlay && _overlay.ColorPresets.Count > 0 ) {
						int ran2 = Random.Range(0, _overlay.ColorPresets.Count-1);
						color = _overlay.ColorPresets[ran2].PresetColor;
					}
				}
				// if the overlay is not assigned, random one from the linked overlays of the slot
				else {
					if ( _slot.LinkedOverlayList.Count > 0 ) {
						int ran = Random.Range(0, _slot.LinkedOverlayList.Count-1);
						_DK_RPG_UMA._Equipment._Collar.Overlay = _slot.LinkedOverlayList[ran];
						if ( _slot.LinkedOverlayList[ran] && _slot.LinkedOverlayList[ran].ColorPresets.Count > 0 ) {
							int ran2 = Random.Range(0, _slot.LinkedOverlayList[ran].ColorPresets.Count-1);
							color = _slot.LinkedOverlayList[ran].ColorPresets[ran2].PresetColor;
						}
					}
				}
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Collar.Color = color;
			}
			#endregion Equipment
		}
		// for an Overlay element
		else if ( _overlay ) {
		//	Debug.Log ( "test");
			#region Equipment
			if ( _overlay.OverlayType == "FeetWear" ) {
				_DK_RPG_UMA._Equipment._Feet.Slot = null;
				_DK_RPG_UMA._Equipment._Feet.Overlay = _overlay;
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Feet.Color = color;
			}
			else if ( _overlay.OverlayType == "HandsWear" ) {
				_DK_RPG_UMA._Equipment._Hands.Slot = null;
				_DK_RPG_UMA._Equipment._Hands.Overlay = _overlay;
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Hands.Color = color;
			}
			else if ( _overlay.OverlayType == "HeadWear" ) {
				_DK_RPG_UMA._Equipment._Head.Slot = null;
				_DK_RPG_UMA._Equipment._Head.Overlay = _overlay;
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Head.Color = color;
			}
			else if ( _overlay.OverlayType == "LegsWear" ) {
				_DK_RPG_UMA._Equipment._Legs.Slot = null;
				_DK_RPG_UMA._Equipment._Legs.Overlay = _overlay;
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Legs.Color = color;
			}
			else if ( _overlay.OverlayType == "TorsoWear" ) {
				_DK_RPG_UMA._Equipment._Torso.Slot = null;
				_DK_RPG_UMA._Equipment._Torso.Overlay = _overlay;
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Torso.Color = color;
			}
			else if ( _overlay.OverlayType == "ArmbandWear" ) {
				_DK_RPG_UMA._Equipment._ArmBand.Slot = null;
				_DK_RPG_UMA._Equipment._ArmBand.Overlay = _overlay;
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._ArmBand.Color = color;
			}
			else if ( _overlay.OverlayType == "WristWear" ) {
				_DK_RPG_UMA._Equipment._Wrist.Slot = null;
				_DK_RPG_UMA._Equipment._Wrist.Overlay = _overlay;
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Wrist.Color = color;
			}
			else if ( _overlay.OverlayType == "CloakWear" ) {
				_DK_RPG_UMA._Equipment._Cloak.Slot = null;
				_DK_RPG_UMA._Equipment._Cloak.Overlay = _overlay;
				if ( color != Color.black ) _DK_RPG_UMA._Equipment._Cloak.Color = color;
			}
			else if ( _overlay.OverlayType == "Underwear" ) {
				_DK_RPG_UMA._Avatar._Body._Underwear.Slot = null;
				_DK_RPG_UMA._Avatar._Body._Underwear.Overlay = _overlay;
				if ( color != Color.black ) _DK_RPG_UMA._Avatar._Body._Underwear.Color = color;
			}
			#endregion Equipment
		}


		DK_RPG_ReBuild _DK_RPG_ReBuild = _DK_RPG_UMA.gameObject.GetComponent<DK_RPG_ReBuild>();
		if ( _DK_RPG_ReBuild == null ) _DK_RPG_ReBuild = _DK_RPG_UMA.gameObject.AddComponent<DK_RPG_ReBuild>();
		DKUMAData _DKUMAData = _DK_RPG_UMA.gameObject.GetComponent<DKUMAData>();
		_DK_RPG_ReBuild.Launch (_DKUMAData);
	}
}
