using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DK_RPG_SelfGeneratorCoroutine : WorkerCoroutine
{
	DK_RPG_TextureProcessPROCoroutine textureProcessPROCoroutine;
	DK_RPG_TextureProcessIndieCoroutine textureProcessIndieCoroutine;
		
	MaxRectsBinPack packTexture;
	
	List<DKUMAData.MaterialDefinition> materialDefinitionList;
	DKUMAData.MaterialDefinition[] orderedMaterialDefinition;
	
	List<DKUMAData.AtlasMaterialDefinition> atlasMaterialDefinitionList;
	
	float atlasResolutionScale;
	int mipMapAdjust;
	
	DK_RPG_SelfGenerator umaGenerator;
	DKUMAGenerator _DKumaGenerator;

    public void Prepare(DK_RPG_SelfGenerator _umaGenerator)
    {
		umaGenerator = _umaGenerator;	
   }
// Added by DK	
    public void EditorStart()
    {
		_DKumaGenerator = GameObject.Find ( "DKUMAGenerator" ).GetComponent<DKUMAGenerator>();
	//	umaGenerator.umaData.cleanTextures();	
		
		materialDefinitionList = new List<DKUMAData.MaterialDefinition>();
		
		//Update atlas area can be handled here
		DKUMAData.MaterialDefinition tempMaterialDefinition = new DKUMAData.MaterialDefinition();
		
		DKSlotData[] slots = umaGenerator.umaData.umaRecipe.slotDataList;
		if ( slots == null ){
			Debug.LogError ( umaGenerator.umaData.gameObject.name+" can not be generated : the Slot list is umpty. Verify your Elements configuration." );
		}
		else
		for(int i = 0; i < slots.Length; i++){	
			if(slots[i] != null &&  slots[i].overlayList.Count != 0 ) {
				tempMaterialDefinition = new DKUMAData.MaterialDefinition();
				tempMaterialDefinition.baseTexture = slots[i].overlayList[0].textureList;
				tempMaterialDefinition.baseColor = slots[i].overlayList[0].color;
				tempMaterialDefinition.materialSample = slots[i].materialSample;
				tempMaterialDefinition.overlays = new DKUMAData.textureData[slots[i].overlayList.Count -1];
				tempMaterialDefinition.overlayColors = new Color32[tempMaterialDefinition.overlays.Length];
	            tempMaterialDefinition.rects = new Rect[tempMaterialDefinition.overlays.Length];
	            tempMaterialDefinition.channelMask = new Color32[tempMaterialDefinition.overlays.Length+1][];
	            tempMaterialDefinition.channelAdditiveMask = new Color32[tempMaterialDefinition.overlays.Length+1][];
	            tempMaterialDefinition.channelMask[0] = slots[i].overlayList[0].channelMask;
	            tempMaterialDefinition.channelAdditiveMask[0] = slots[i].overlayList[0].channelAdditiveMask;                
				tempMaterialDefinition.slotData = slots[i];
				
				for(int overlayID = 0; overlayID < slots[i].overlayList.Count-1; overlayID++){
					tempMaterialDefinition.overlays[overlayID] = new DKUMAData.textureData();
					tempMaterialDefinition.rects[overlayID] = slots[i].overlayList[overlayID+1].rect;
					tempMaterialDefinition.overlays[overlayID].textureList = slots[i].overlayList[overlayID+1].textureList;
					tempMaterialDefinition.overlayColors[overlayID] = slots[i].overlayList[overlayID+1].color;
					tempMaterialDefinition.channelMask[overlayID+1] = slots[i].overlayList[overlayID + 1].channelMask;
	                tempMaterialDefinition.channelAdditiveMask[overlayID+1] = slots[i].overlayList[overlayID + 1].channelAdditiveMask;
				}
				materialDefinitionList.Add(tempMaterialDefinition);
			}
		}
		
		if(_DKumaGenerator.usePRO && UnityEditorInternal.InternalEditorUtility.HasPro()){
			textureProcessPROCoroutine = new DK_RPG_TextureProcessPROCoroutine();
		}else{
			textureProcessIndieCoroutine = new DK_RPG_TextureProcessIndieCoroutine();
		}
		packTexture = new MaxRectsBinPack(_DKumaGenerator.atlasResolution,_DKumaGenerator.atlasResolution,false);
	}

    protected override void Start()
	{
		_DKumaGenerator = GameObject.Find ( "DKUMAGenerator" ).GetComponent<DKUMAGenerator>();
	//	umaGenerator.umaData.cleanTextures();	
		
		materialDefinitionList = new List<DKUMAData.MaterialDefinition>();
		
		//Update atlas area can be handled here
		DKUMAData.MaterialDefinition tempMaterialDefinition = new DKUMAData.MaterialDefinition();
		
		DKSlotData[] slots = umaGenerator.umaData.umaRecipe.slotDataList;
		if ( slots == null ){
			Debug.LogError ( umaGenerator.umaData.gameObject.name+" can not be generated : the Slot list is umpty. Verify your Elements configuration." );
		}
		else
		if ( slots.Length > 0 ) for(int i = 0; i < slots.Length; i++){	
			if(slots[i] != null && slots[i].overlayList.Count !=0 ){
				tempMaterialDefinition = new DKUMAData.MaterialDefinition();
				tempMaterialDefinition.baseTexture = slots[i].overlayList[0].textureList;
				tempMaterialDefinition.baseColor = slots[i].overlayList[0].color;
				tempMaterialDefinition.materialSample = slots[i].materialSample;
				tempMaterialDefinition.overlays = new DKUMAData.textureData[slots[i].overlayList.Count -1];
				tempMaterialDefinition.overlayColors = new Color32[tempMaterialDefinition.overlays.Length];
	            tempMaterialDefinition.rects = new Rect[tempMaterialDefinition.overlays.Length];
	            tempMaterialDefinition.channelMask = new Color32[tempMaterialDefinition.overlays.Length+1][];
	            tempMaterialDefinition.channelAdditiveMask = new Color32[tempMaterialDefinition.overlays.Length+1][];
	            tempMaterialDefinition.channelMask[0] = slots[i].overlayList[0].channelMask;
	            tempMaterialDefinition.channelAdditiveMask[0] = slots[i].overlayList[0].channelAdditiveMask;                
				tempMaterialDefinition.slotData = slots[i];
				
				for(int overlayID = 0; overlayID < slots[i].overlayList.Count-1; overlayID++){
					tempMaterialDefinition.overlays[overlayID] = new DKUMAData.textureData();
					tempMaterialDefinition.rects[overlayID] = slots[i].overlayList[overlayID+1].rect;
					tempMaterialDefinition.overlays[overlayID].textureList = slots[i].overlayList[overlayID+1].textureList;
					tempMaterialDefinition.overlayColors[overlayID] = slots[i].overlayList[overlayID+1].color;
					tempMaterialDefinition.channelMask[overlayID+1] = slots[i].overlayList[overlayID + 1].channelMask;
	                tempMaterialDefinition.channelAdditiveMask[overlayID+1] = slots[i].overlayList[overlayID + 1].channelAdditiveMask;
				}
				materialDefinitionList.Add(tempMaterialDefinition);
			}
		}
		
		if(_DKumaGenerator.usePRO && UnityEditorInternal.InternalEditorUtility.HasPro()){
			textureProcessPROCoroutine = new DK_RPG_TextureProcessPROCoroutine();
		}else{
			textureProcessIndieCoroutine = new DK_RPG_TextureProcessIndieCoroutine();
		}

		packTexture = new MaxRectsBinPack(_DKumaGenerator.atlasResolution,_DKumaGenerator.atlasResolution,false);
	}

    protected override IEnumerator workerMethod()
    {	
		
		orderedMaterialDefinition = new DKUMAData.MaterialDefinition[materialDefinitionList.Count];
		for(int i = 0; i < materialDefinitionList.Count; i++){
			orderedMaterialDefinition[i] = materialDefinitionList[i];
		}
			
		OrderMaterialDefinition();
		
		//resolutionAdjust code
        atlasResolutionScale = umaGenerator.umaData.atlasResolutionScale == 0f ? 1f : umaGenerator.umaData.atlasResolutionScale;
		mipMapAdjust = Mathf.FloorToInt(Mathf.Log(1/(atlasResolutionScale),2));
	
		
		umaGenerator.umaData.atlasList = new DKUMAData.AtlasList();
		umaGenerator.umaData.atlasList.atlas = new List<DKUMAData.AtlasElement>();

		GenerateAtlasData();
		CalculateRects();
		if(_DKumaGenerator.AtlasCrop){
			OptimizeAtlas();		
		}
		
		if(_DKumaGenerator.usePRO && UnityEditorInternal.InternalEditorUtility.HasPro()){
			
			textureProcessPROCoroutine.Prepare(umaGenerator.umaData,umaGenerator);
			yield return textureProcessPROCoroutine;
		}else{
			textureProcessIndieCoroutine.Prepare(umaGenerator.umaData,umaGenerator);
			yield return textureProcessIndieCoroutine;	
		}
		
		UpdateUV();
    }

    protected override void Stop()
    {

    }
	
	private void OrderMaterialDefinition(){
		//Ordering List based on textureSize, for atlas calculation
		for(int i = 0; i < orderedMaterialDefinition.Length; i++){
			int LargestIndex = i;
			
			for(int i2 = i; i2 < orderedMaterialDefinition.Length; i2++){

				if(orderedMaterialDefinition[LargestIndex].baseTexture[0].width*orderedMaterialDefinition[LargestIndex].baseTexture[0].height < orderedMaterialDefinition[i2].baseTexture[0].width*orderedMaterialDefinition[i2].baseTexture[0].height){
					LargestIndex = i2;					
				}
				
				if(i2 == orderedMaterialDefinition.Length-1){
					DKUMAData.MaterialDefinition tempMaterialDefinition = orderedMaterialDefinition[i];
					orderedMaterialDefinition[i] = orderedMaterialDefinition[LargestIndex];
					orderedMaterialDefinition[LargestIndex] = tempMaterialDefinition;
				}
			}
		}	
	}
	
	
	private void GenerateAtlasData(){
		for(int i = 0; i < orderedMaterialDefinition.Length; i++){		
			atlasMaterialDefinitionList = new List<DKUMAData.AtlasMaterialDefinition>();
			DKUMAData.AtlasElement atlasElement = new DKUMAData.AtlasElement();	
			DKUMAData.AtlasMaterialDefinition tempAtlasMaterialDefinition = new DKUMAData.AtlasMaterialDefinition();
				
			//This guarantee not including on atlas duplicated textures
			if(orderedMaterialDefinition[i] != null){
				tempAtlasMaterialDefinition.source = orderedMaterialDefinition[i];
				atlasMaterialDefinitionList.Add(tempAtlasMaterialDefinition);
				
				for(int i2 = i; i2 < orderedMaterialDefinition.Length; i2++){
					//Look for same shader
					
					if(orderedMaterialDefinition[i2] != null){
						if(i2 != i){
							tempAtlasMaterialDefinition = new DKUMAData.AtlasMaterialDefinition();
							try {
								if(orderedMaterialDefinition[i].materialSample.shader == orderedMaterialDefinition[i2].materialSample.shader){				
									tempAtlasMaterialDefinition.source = orderedMaterialDefinition[i2];
									atlasMaterialDefinitionList.Add(tempAtlasMaterialDefinition);
									orderedMaterialDefinition[i2] = null;
								}
							}
							catch(MissingReferenceException e ){ Debug.LogError (umaGenerator.umaData.name+" : "+e); }
						}
					}
	
					if(i2 == orderedMaterialDefinition.Length-1 && atlasMaterialDefinitionList.Count > 0){
						//All slots sharing same shader are on same atlasElement
						atlasElement.atlasMaterialDefinitions = atlasMaterialDefinitionList;
						atlasElement.shader = atlasMaterialDefinitionList[0].source.materialSample.shader;
						atlasElement.materialSample = atlasMaterialDefinitionList[0].source.materialSample;
						
						umaGenerator.umaData.atlasList.atlas.Add(atlasElement);
					}
				
				}
				
				orderedMaterialDefinition[i] = null;
			}
		}
	}
	
	
	private void CalculateRects(){
		Rect nullRect = new Rect(0,0,0,0);
		DKUMAData.AtlasList umaAtlasList = umaGenerator.umaData.atlasList;

		
		for(int atlasIndex = 0; atlasIndex < umaAtlasList.atlas.Count; atlasIndex++){
			
			umaAtlasList.atlas[atlasIndex].cropResolution = new Vector2(_DKumaGenerator.atlasResolution,_DKumaGenerator.atlasResolution);
			umaAtlasList.atlas[atlasIndex].resolutionScale = atlasResolutionScale;
			umaAtlasList.atlas[atlasIndex].mipmap = mipMapAdjust;
			packTexture.Init(_DKumaGenerator.atlasResolution,_DKumaGenerator.atlasResolution,false);
			bool textureFit = true;
			
			for(int atlasElementIndex = 0; atlasElementIndex < umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions.Count; atlasElementIndex++){
				DKUMAData.AtlasMaterialDefinition tempMaterialDef = umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions[atlasElementIndex];
				
				if(tempMaterialDef.atlasRegion == nullRect){
					
					tempMaterialDef.atlasRegion = packTexture.Insert(Mathf.FloorToInt(tempMaterialDef.source.baseTexture[0].width*umaAtlasList.atlas[atlasIndex].resolutionScale*tempMaterialDef.source.slotData.overlayScale),Mathf.FloorToInt(tempMaterialDef.source.baseTexture[0].height*umaAtlasList.atlas[atlasIndex].resolutionScale*tempMaterialDef.source.slotData.overlayScale),MaxRectsBinPack.FreeRectChoiceHeuristic.RectBestLongSideFit);
					tempMaterialDef.isRectShared = false;
					umaAtlasList.atlas[atlasIndex].shader = tempMaterialDef.source.materialSample.shader;
					
					if(tempMaterialDef.atlasRegion == nullRect){
						textureFit = false;
						
						if(_DKumaGenerator.fitAtlas){
							Debug.LogWarning("Atlas resolution is too small, Textures will be reduced.");
						}else{
							Debug.LogError("Atlas resolution is too small, not all textures will fit.");
						}
					}

					for(int atlasElementIndex2 = atlasElementIndex; atlasElementIndex2 < umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions.Count; atlasElementIndex2++){
						if(atlasElementIndex != atlasElementIndex2){
							if(tempMaterialDef.source.baseTexture[0] == umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions[atlasElementIndex2].source.baseTexture[0]){	
								umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions[atlasElementIndex2].atlasRegion = tempMaterialDef.atlasRegion;
								umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions[atlasElementIndex2].isRectShared = true;
							}
						}
					}
											
				}
				
				if(!textureFit && _DKumaGenerator.fitAtlas){
					//Reset calculation and reduce texture sizes
					textureFit = true;
					atlasElementIndex = -1;
					umaAtlasList.atlas[atlasIndex].resolutionScale = umaAtlasList.atlas[atlasIndex].resolutionScale * 0.5f;
					umaAtlasList.atlas[atlasIndex].mipmap ++;
					
					packTexture.Init(_DKumaGenerator.atlasResolution,_DKumaGenerator.atlasResolution,false);					
					for(int atlasElementIndex2 = 0; atlasElementIndex2 < umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions.Count; atlasElementIndex2++){
						umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions[atlasElementIndex2].atlasRegion = nullRect;
					}
				}
			}
		}
	}
	
	private void OptimizeAtlas(){
		DKUMAData.AtlasList umaAtlasList = umaGenerator.umaData.atlasList;
		for(int atlasIndex = 0; atlasIndex < umaAtlasList.atlas.Count; atlasIndex++){
			Vector2 usedArea = new Vector2(0,0);
			for(int atlasElementIndex = 0; atlasElementIndex < umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions.Count; atlasElementIndex++){
				if(umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions[atlasElementIndex].atlasRegion.xMax > usedArea.x){
					usedArea.x = umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions[atlasElementIndex].atlasRegion.xMax;
				}
				
				if(umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions[atlasElementIndex].atlasRegion.yMax > usedArea.y){
					usedArea.y = umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions[atlasElementIndex].atlasRegion.yMax;
				}
			}
			
			Vector2 tempResolution = new Vector2(_DKumaGenerator.atlasResolution,_DKumaGenerator.atlasResolution);
			
			bool done = false;
			while(!done){
				if(tempResolution.x*0.5f >= usedArea.x){
					tempResolution = new Vector2(tempResolution.x*0.5f,tempResolution.y);
				}else{
					done = true;
				}				
			}
	
			done = false;
			while(!done){
				
				if(tempResolution.y*0.5f >= usedArea.y){
					tempResolution = new Vector2(tempResolution.x,tempResolution.y*0.5f);
				}else{
					done = true;
				}				
			}
			
			umaAtlasList.atlas[atlasIndex].cropResolution = tempResolution;
		}
	}
	
	
	
	private void UpdateUV(){
		DKUMAData.AtlasList umaAtlasList = umaGenerator.umaData.atlasList;
		
		for(int atlasIndex = 0; atlasIndex < umaAtlasList.atlas.Count; atlasIndex++){			
			Vector2 finalAtlasAspect = new Vector2(_DKumaGenerator.atlasResolution/umaAtlasList.atlas[atlasIndex].cropResolution.x,_DKumaGenerator.atlasResolution/umaAtlasList.atlas[atlasIndex].cropResolution.y);
					
			for(int atlasElementIndex = 0; atlasElementIndex < umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions.Count; atlasElementIndex++){
				Rect tempRect = umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions[atlasElementIndex].atlasRegion;
				tempRect.xMin = tempRect.xMin*finalAtlasAspect.x;
				tempRect.xMax = tempRect.xMax*finalAtlasAspect.x;			
				tempRect.yMin = tempRect.yMin*finalAtlasAspect.y;
				tempRect.yMax = tempRect.yMax*finalAtlasAspect.y;
				umaAtlasList.atlas[atlasIndex].atlasMaterialDefinitions[atlasElementIndex].atlasRegion = tempRect;
			}
		}		
	}
}