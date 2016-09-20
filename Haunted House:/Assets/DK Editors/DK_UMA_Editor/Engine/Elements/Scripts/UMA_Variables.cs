using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public class UMA_Variables : MonoBehaviour {

	public bool UseUMA = true;

	UMACrowd UMACrowd;
	UMAGenerator _UMAGenerator ;
	UMACustomization _UMACustomization;

	public RaceLibrary _RaceLibrary;
	public SlotLibrary _SlotLibrary;
	public OverlayLibrary _OverlayLibrary;

	public List<UMA.SlotData> UMASlotsList = new List<UMA.SlotData>();
	
	public List<Texture2D> PreviewsList = new List<Texture2D>();
	public List<Texture2D> OvPreviewsList = new List<Texture2D>();
	
	public List<UMA.OverlayData> UMAOverlaysList = new List<UMA.OverlayData>();

	public UMA.SlotData DefaultSlotType;
	public UMA.OverlayData DefaultOverlayType;

}
