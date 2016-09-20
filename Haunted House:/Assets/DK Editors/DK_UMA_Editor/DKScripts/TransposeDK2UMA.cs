using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UMA;

public class TransposeDK2UMA : MonoBehaviour {

	// UMA Variables
	UMA.UMAData umaData;
	UMA.RaceData _RaceData;

	UMACrowd _UMACrowd;
	UMA.UMAGeneratorBase generator;
	Transform tempUMA;

	SlotLibrary slotLibrary;
	OverlayLibrary overlayLibrary;
	RaceLibrary raceLibrary;
	GameObject DefaultUMA;

	string _StreamedUMA;
	LoadUMA _LoadUMA;
//	UMADynamicAvatar umaDynamicAvatar;

	// DK UMA Variables
	DK_UMACrowd _DK_UMACrowd;
	DKUMAData _DKUMAData;

	// Avatar Variables
	DK_RPG_UMA _DK_RPG_UMA;
	string Gender;

	public float MiniDNA = 0.20f;
	public float MaxiDNA = 0.78f;


	public void Launch (DK_RPG_UMA DKRPGUMA, DK_UMACrowd DKUMACrowd, string streamed ){
		// assign scripts
		_DK_UMACrowd = DKUMACrowd;
		_DK_RPG_UMA = DKRPGUMA;
		Gender = DKRPGUMA.Gender;
	//	_DK_UMACrowd = GameObject.Find ("DKUMACrowd").GetComponent<DK_UMACrowd>();

		// get libraries
		try {
		raceLibrary = GameObject.Find ("RaceLibrary").GetComponent<RaceLibrary>();
		slotLibrary = GameObject.Find ("SlotLibrary").GetComponent<SlotLibrary>();
		overlayLibrary = GameObject.Find ("OverlayLibrary").GetComponent<OverlayLibrary>();
		}catch ( NullReferenceException ) { 
			Debug.LogError ( "UMA is missing from your scene. UMA is required to generate a UMA avatar." ); 
		}
		// assign recipe
		_StreamedUMA = streamed;
		
		// Start the creation of the avatar
		ConvertStreamedUMA (streamed);
		CreateUMAData ();
	}

	void SendRaceToUMA (){
		if ( Gender == "Male" ) _RaceData = raceLibrary.GetRace ("HumanMale");
		else if ( Gender == "Female" ) _RaceData = raceLibrary.GetRace ("HumanFemale");
		Debug.Log ( _RaceData.raceName );
	//	DefaultUMA.GetComponent<UMA.UMAData>().umaRecipe.SetRace(_RaceData); 
	}

	void SendSlotsToUMA (){
		List<SlotData> tmpSlotList = new List<SlotData>();

		for (int i = 0; i <  _DK_UMACrowd.tempSlotList.Count; i ++) {
			DKSlotData dkSlot = _DK_UMACrowd.tempSlotList[i];
			if ( dkSlot._UMA != null ){
				// find overlays
				List<OverlayData> tmpOverlayList = new List<OverlayData>();
				for (int i1 = 0; i1 <  _DK_UMACrowd.tempSlotList[i].overlayList.Count; i1 ++) {
					DKOverlayData dkOverlay = _DK_UMACrowd.tempSlotList[i].overlayList[i1];
					if ( dkOverlay._UMA != null ){
						tmpOverlayList.Add ( overlayLibrary.InstantiateOverlay ( dkOverlay._UMA.overlayName, dkOverlay.color ) );
					}
					else Debug.LogError ( "Warning : The DKSlot '"+dkSlot.slotName+"' has the DKOverlay '"
					                     +dkOverlay.overlayName+"' and the UMA Link is missing from it. " +
					                     	"Fix it by selecting the concerned DKOverlay and adding the UMA Link.");
				}

				// add the UMA slot when its overlay tmplist
				tmpSlotList.Add (slotLibrary.InstantiateSlot( dkSlot._UMA.slotName, tmpOverlayList ) );

			}
			else Debug.LogError ( "Warning : The DKSlot '"+dkSlot.slotName+"' has a missing UMA Link for it. " +
				"Fix it by selecting the concerned DKSlot and add the UMA Link.");
		}
		// transfert to the UMAData
		DefaultUMA.GetComponent<UMA.UMAData>().umaRecipe.slotDataList = tmpSlotList.ToArray();
	}

	
	protected virtual void GenerateUMAShapes()
	{

	}


	void ConvertStreamedUMA (string streamed){
		DKUMAData _DKUMAData = _DK_RPG_UMA.gameObject.GetComponent<DKUMAData>();
		
	//	string newStreamedUMA;
		
		// find the StreamedUMA
		_StreamedUMA = streamed;
		
		// Translate compressions
		_StreamedUMA = _StreamedUMA.Replace(@"sID","slotID");
		_StreamedUMA = _StreamedUMA.Replace ( @"oS" , "overlayScale" );
		_StreamedUMA = _StreamedUMA.Replace ( @"cOI" , "copyOverlayIndex" );
		_StreamedUMA = _StreamedUMA.Replace ( @"ODL" , "OverlayDataList" );
		_StreamedUMA = _StreamedUMA.Replace ( @"oID" , "overlayID" );
		_StreamedUMA = _StreamedUMA.Replace ( @"rL" , "rectList" );
		
		_StreamedUMA = _StreamedUMA.Replace ( @"cL" , "colorList" );
		_StreamedUMA = _StreamedUMA.Replace ( @"cML" , "channelMaskList" );
		_StreamedUMA = _StreamedUMA.Replace ( @"cAML" , "channelAdditiveMaskList" );
		_StreamedUMA = _StreamedUMA.Replace ( @"DKUMADnaHumanoid" , "UMADnaHumanoid" );
		// About Race
		// is a female
		if ( _DKUMAData.myRenderer.name.Contains("Female")) {
			_StreamedUMA = _StreamedUMA.Replace ( @""+_DKUMAData.umaRecipe.raceData.raceName+"" , "HumanFemale" );
			//	Debug.Log ( "Female" );
		}
		else {
			_StreamedUMA = _StreamedUMA.Replace ( @""+_DKUMAData.umaRecipe.raceData.raceName+"" , "HumanMale" );
			//	Debug.Log ( "Male" );
		}
		// Modify DNA names
		_StreamedUMA = _StreamedUMA.Replace ( @"N0\" , "height\\" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N10" , "legSeparation" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N11" , "upperMuscle" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N12" , "lowerMuscle" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N13" , "upperWeight" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N14" , "lowerWeight" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N15" , "legsSize" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N16" , "belly" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N17" , "waist" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N18" , "gluteusSize" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N19" , "earsSize" );
		
		_StreamedUMA = _StreamedUMA.Replace ( @"N1\" , "headSize\\" );
		
		_StreamedUMA = _StreamedUMA.Replace ( @"N20" , "earsPosition" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N21" , "earsRotation" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N22" , "noseSize" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N23" , "noseCurve" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N24" , "noseWidth" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N25" , "noseInclination" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N26" , "nosePosition" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N27" , "nosePronounced" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N28" , "noseFlatten" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N29" , "chinSize" );
		
		_StreamedUMA = _StreamedUMA.Replace ( @"N2\" , "headWidth\\" );
		
		_StreamedUMA = _StreamedUMA.Replace ( @"N30" , "chinPronounced" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N31" , "chinPosition" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N32" , "mandibleSize" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N33" , "jawsSize" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N34" , "jawsPosition" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N35" , "cheekSize" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N36" , "cheekPosition" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N37" , "lowCheekPronounced" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N38" , "lowCheekPosition" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N39" , "foreheadSize" );
		
		_StreamedUMA = _StreamedUMA.Replace ( @"N3\" , "neckThickness\\" );
		
		_StreamedUMA = _StreamedUMA.Replace ( @"N40" , "foreheadPosition" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N41" , "lipsSize" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N42" , "mouthSize" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N43" , "eyeRotation" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N44" , "eyeSize" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N45" , "breastSize" );
		//		_StreamedUMA = _StreamedUMA.Replace ( "N46" , "" );
		//		_StreamedUMA = _StreamedUMA.Replace ( "N47" , "" );
		//		_StreamedUMA = _StreamedUMA.Replace ( "N48" , "" );
		//		_StreamedUMA = _StreamedUMA.Replace ( "N49" , "" );
		
		_StreamedUMA = _StreamedUMA.Replace ( @"N4\" , "armLength\\" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N5\" , "forearmLength\\" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N6\" , "armWidth\\" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N7\" , "forearmWidth\\" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N8\" , "handsSize\\" );
		_StreamedUMA = _StreamedUMA.Replace ( @"N9\" , "feetSize\\" );
	}


	void CreateUMAData (){
		DefaultUMA = (GameObject) GameObject.Instantiate(Resources.Load("DefaultUMABase") as GameObject);
		if ( _DK_UMACrowd.zeroPoint) DefaultUMA.transform.position = _DK_UMACrowd.zeroPoint.transform.position;

	//	var newGO = new GameObject("Generated Character");
	//	newGO.transform.parent = transform;
	//	umaDynamicAvatar = DefaultUMA.GetComponent<UMADynamicAvatar>();
	//	umaDynamicAvatar.Initialize();

		umaData = DefaultUMA.GetComponent<UMADynamicAvatar>().umaData;
	//	umaDynamicAvatar.umaGenerator = generator;
	//	umaData.umaGenerator = generator;
	//	var umaRecipe = umaDynamicAvatar.umaData.umaRecipe;

	/*	if ( _DK_RPG_UMA.Gender == "Male" ){

			umaRecipe.SetRace(raceLibrary.GetRace("HumanMale"));
		}
		else if ( _DK_RPG_UMA.Gender == "Female" ){
			umaRecipe.SetRace(raceLibrary.GetRace("HumanFemale"));
		}
		*/

		//	SetUMAData();

	/*	if ( _UMACrowd.animationController != null)
		{
			umaDynamicAvatar.animationController = _UMACrowd.animationController;
		}
		umaDynamicAvatar.UpdateNewRace();
		umaDynamicAvatar.umaData.myRenderer.enabled = false;
		tempUMA = newGO.transform;
		
		if (_DK_UMACrowd.zeroPoint)
		{
			tempUMA.position = new Vector3(_DK_UMACrowd.zeroPoint.position.x, _DK_UMACrowd.zeroPoint.position.y, _DK_UMACrowd.zeroPoint.position.z);
		}
		else
		{
			tempUMA.position = new Vector3(0, 0, 0);
		}
*/
		AssignRecipe ();
	}

	#region From original UMA
	protected virtual void SetUMAData(){
		_UMACrowd = GameObject.Find ("UMACrowd").GetComponent<UMACrowd>();
		umaData.atlasResolutionScale = _UMACrowd.atlasResolutionScale;
		umaData.OnCharacterUpdated += myColliderUpdateMethod;
	}
	void myColliderUpdateMethod(UMA.UMAData umaData)
	{
		CapsuleCollider tempCollider = umaData.umaRoot.gameObject.GetComponent("CapsuleCollider") as CapsuleCollider;
		if (tempCollider)
		{
			UMA.UMADnaHumanoid umaDna = umaData.umaRecipe.GetDna<UMA.UMADnaHumanoid>();
			tempCollider.height = (umaDna.height + 0.5f) * 2 + 0.1f;
			tempCollider.center = new Vector3(0, tempCollider.height * 0.5f - 0.04f, 0);
		}
	}
	#endregion From original UMA


	void AssignRecipe (){
		// Add the new auto loader for the avatar. May be obsolete
		_LoadUMA = DefaultUMA.AddComponent <LoadUMA>();
	//	DefaultUMA.AddComponent <NaturalLauncher>();
		_LoadUMA._StreamedUMA = _StreamedUMA;

	//	SendRaceToUMA ();
	//	SendSlotsToUMA ();
		SendDNAToUMA ();

		// Move the Avatar
		if (_DK_UMACrowd.zeroPoint)
		{
			DefaultUMA.transform.position = new Vector3(_DK_UMACrowd.zeroPoint.position.x, _DK_UMACrowd.zeroPoint.position.y, _DK_UMACrowd.zeroPoint.position.z);
		}
		else
		{
			DefaultUMA.transform.position = new Vector3(0, 0, 0);
		}

	/*	var selectedTransform = umaData.transform;
		var avatar = selectedTransform.GetComponent<UMAAvatarBase>();
			
		while (avatar == null && selectedTransform.parent != null){
			selectedTransform = selectedTransform.parent;
			avatar = selectedTransform.GetComponent<UMAAvatarBase>();
		}
		
		if (avatar != null){
			var asset = ScriptableObject.CreateInstance<UMATextRecipe>();
			asset.recipeString = _StreamedUMA;
			avatar.Load(asset);
			Destroy(asset);
		}*/
	}

	void SendDNAToUMA (){
		umaData = DefaultUMA.GetComponent<UMAData>();
	//	UMADnaHumanoid umaDna = new UMADnaHumanoid();
	//	UMADnaHumanoid umaDna = _LoadUMA.umaDna;
	//	umaRecipe.ClearDna();
	//	umaRecipe.AddDna(umaDna);
		UMAAvatarBase avatar = DefaultUMA.GetComponent<UMAAvatarBase>();

		if (avatar.context == null) {
			GameObject tmpObj = GameObject.Find("UMAContext");
			if ( tmpObj ) avatar.context = tmpObj.GetComponent<UMAContext>();
			else Debug.LogError ( "UMA is missing from your scene. UMA is required to generate a UMA avatar." );

		}
		try{
			if (avatar.context != null) foreach ( DKRaceData.DNAConverterData dkDNA in _DK_UMACrowd.umaData.DNAList2 )  {
		//	DKRaceData.DNAConverterData dkDNA = _DK_UMACrowd.umaData.DNAList2[i];
			float DnaValue = 0;

		//	if ( dkDNA.Value < 0.0f ) DnaValue = 0.0f;
		//	else if ( dkDNA.Value > 0.9f ) DnaValue = 0.9f;
		//	else 
				DnaValue = dkDNA.Value;

		//	Debug.Log ( dkDNA.Name+" "+dkDNA.Value );
		//	Debug.Log ( _LoadUMA.umaDna.height );

			if ( dkDNA.Name.ToLower() == "height" ){
				_LoadUMA.umaDna.height = DnaValue;
			}
			else if ( dkDNA.Name.ToLower() == "headsize" ) {
				_LoadUMA.umaDna.headSize = DnaValue;
			}
			else if ( dkDNA.Name.ToLower() == "headwidth" ) _LoadUMA.umaDna.headWidth = DnaValue;
			else if ( dkDNA.Name.ToLower() == "neckthickness" ) _LoadUMA.umaDna.neckThickness = DnaValue;
			else if ( dkDNA.Name.ToLower() == "handssize" ) _LoadUMA.umaDna.handsSize = DnaValue;
			else if ( dkDNA.Name.ToLower() == "feetsize" ) _LoadUMA.umaDna.feetSize = DnaValue;
			else if ( dkDNA.Name.ToLower() == "legsseparation" ) _LoadUMA.umaDna.legSeparation = DnaValue;
			else if ( dkDNA.Name.ToLower() == "waist" ) _LoadUMA.umaDna.waist = DnaValue;
			else if ( dkDNA.Name.ToLower() == "armlength" ) _LoadUMA.umaDna.armLength = DnaValue;
			else if ( dkDNA.Name.ToLower() == "forearmlength" ) _LoadUMA.umaDna.forearmLength = DnaValue;
			else if ( dkDNA.Name.ToLower() == "armwidth" ) _LoadUMA.umaDna.armWidth = DnaValue;
			else if ( dkDNA.Name.ToLower() == "forearmwidth" ) _LoadUMA.umaDna.forearmWidth = DnaValue;
			else if ( dkDNA.Name.ToLower() == "uppermuscle" ) _LoadUMA.umaDna.upperMuscle = DnaValue;
			else if ( dkDNA.Name.ToLower() == "upperweight" ) _LoadUMA.umaDna.upperWeight = DnaValue;
			else if ( dkDNA.Name.ToLower() == "lowermuscle" ) _LoadUMA.umaDna.lowerMuscle = DnaValue;
			else if ( dkDNA.Name.ToLower() == "lowerweight" ) _LoadUMA.umaDna.lowerWeight = DnaValue;
			else if ( dkDNA.Name.ToLower() == "belly" ) _LoadUMA.umaDna.belly = DnaValue;
			else if ( dkDNA.Name.ToLower() == "legssize" ) _LoadUMA.umaDna.legsSize = DnaValue;
			else if ( dkDNA.Name.ToLower() == "gluteussize" ) _LoadUMA.umaDna.gluteusSize = DnaValue;
			else if ( dkDNA.Name.ToLower() == "earssize" ) _LoadUMA.umaDna.earsSize = DnaValue;
			else if ( dkDNA.Name.ToLower() == "earsposition" ) _LoadUMA.umaDna.earsPosition = DnaValue;
			else if ( dkDNA.Name.ToLower() == "earsrotation" ) _LoadUMA.umaDna.earsRotation = DnaValue;
			else if ( dkDNA.Name.ToLower() == "nosesize" ) _LoadUMA.umaDna.noseSize = DnaValue;
			else if ( dkDNA.Name.ToLower() == "nosecurve" ) _LoadUMA.umaDna.noseCurve = DnaValue;
			else if ( dkDNA.Name.ToLower() == "nosewidth" ) _LoadUMA.umaDna.noseWidth = DnaValue;
			else if ( dkDNA.Name.ToLower() == "noseinclination" ) _LoadUMA.umaDna.noseInclination = DnaValue;
			else if ( dkDNA.Name.ToLower() == "noseposition" ) _LoadUMA.umaDna.nosePosition = DnaValue;
			else if ( dkDNA.Name.ToLower() == "nosepronounced" ) _LoadUMA.umaDna.nosePronounced = DnaValue;
			else if ( dkDNA.Name.ToLower() == "noseflatten" ) _LoadUMA.umaDna.noseFlatten = DnaValue;
			else if ( dkDNA.Name.ToLower() == "chinsize" ) _LoadUMA.umaDna.chinSize = DnaValue;
			else if ( dkDNA.Name.ToLower() == "chinpronounced" ) _LoadUMA.umaDna.chinPronounced = DnaValue;
			else if ( dkDNA.Name.ToLower() == "chinposition" ) _LoadUMA.umaDna.chinPosition = DnaValue;
			else if ( dkDNA.Name.ToLower() == "mandiblesize" ) _LoadUMA.umaDna.mandibleSize = DnaValue;
			else if ( dkDNA.Name.ToLower() == "jawssize" ) _LoadUMA.umaDna.jawsSize = DnaValue;
			else if ( dkDNA.Name.ToLower() == "jawsposition" ) _LoadUMA.umaDna.jawsPosition = DnaValue;
			else if ( dkDNA.Name.ToLower() == "cheeksize" ) _LoadUMA.umaDna.cheekSize = DnaValue;
			else if ( dkDNA.Name.ToLower() == "cheekposition" ) _LoadUMA.umaDna.cheekPosition = DnaValue;
			else if ( dkDNA.Name.ToLower() == "lowcheekpronounced" ) _LoadUMA.umaDna.lowCheekPronounced = DnaValue;
			else if ( dkDNA.Name.ToLower() == "lowcheekposition" ) _LoadUMA.umaDna.lowCheekPosition = DnaValue;
			else if ( dkDNA.Name.ToLower() == "foreheadsize" ) _LoadUMA.umaDna.foreheadSize = DnaValue;
			else if ( dkDNA.Name.ToLower() == "foreheadposition" ) _LoadUMA.umaDna.foreheadPosition = DnaValue;
			else if ( dkDNA.Name.ToLower() == "lipssize" ) _LoadUMA.umaDna.lipsSize = DnaValue;
			else if ( dkDNA.Name.ToLower() == "mouthsize" ) _LoadUMA.umaDna.mouthSize = DnaValue;
			else if ( dkDNA.Name.ToLower() == "eyerotation" ) _LoadUMA.umaDna.eyeRotation = DnaValue;
			else if ( dkDNA.Name.ToLower() == "eyesize" ) _LoadUMA.umaDna.eyeSize = DnaValue;
			else if ( dkDNA.Name.ToLower() == "breastsize" ) _LoadUMA.umaDna.breastSize = DnaValue;
			//	_LoadUMA.umaDna = umaDna;
		}
		}catch(NullReferenceException){}
		Finish ();
	}

	void Finish (){
		DestroyImmediate (this);
	}
}
