﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System;
using System.Linq;


public class DK_RPG_TextureProcessPROCoroutine : WorkerCoroutine
{
	Transform[] textureModuleList;
	DKUMAData umaData;
	RenderTexture destinationTexture;
	Texture[] resultingTextures;
	Rect[] tempAtlasRect;
	DK_RPG_SelfGenerator umaGenerator;
	DKUMAGenerator _DKumaGenerator;
	int atlasIndex;
	float resolutionScale;
	Camera renderCamera;
	
    public void Prepare(DKUMAData _umaData,DK_RPG_SelfGenerator _umaGenerator)
    {
		umaData = _umaData;
		umaGenerator = _umaGenerator;
		_DKumaGenerator = GameObject.Find ("DKUMAGenerator" ).GetComponent<DKUMAGenerator>();

    }

    protected override void Start()
    {}

    protected override IEnumerator workerMethod()
    {	
		for(int atlasIndex = 0; atlasIndex < umaData.atlasList.atlas.Count; atlasIndex ++){
			//Rendering Atlas
			int moduleCount = 0;

			//Process all necessary TextureModules
			for(int i = 0; i < umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions.Count; i++){
				if(!umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions[i].isRectShared){
					moduleCount++;
					moduleCount = moduleCount + umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions[i].source.overlays.Length;
				}
			}
			
			while(umaGenerator.textureMerge.textureModuleList.Count < moduleCount){
				Transform tempModule = UnityEngine.Object.Instantiate(umaGenerator.textureMerge.textureModule,new Vector3(0,0,3),Quaternion.identity) as Transform;
				tempModule.gameObject.GetComponent<Renderer>().material = UnityEngine.Object.Instantiate(umaGenerator.textureMerge.material) as Material;
				umaGenerator.textureMerge.textureModuleList.Add(tempModule);
			}
			
			textureModuleList = umaGenerator.textureMerge.textureModuleList.ToArray();
			for(int i = 0; i < moduleCount; i++){
				textureModuleList[i].localEulerAngles = new Vector3(textureModuleList[i].localEulerAngles.x,180.0f,textureModuleList[i].localEulerAngles.z);
				textureModuleList[i].parent = umaGenerator.textureMerge.myTransform;
				textureModuleList[i].name = "tempModule";
				textureModuleList[i].gameObject.SetActive(true);
			}
			
			moduleCount = 0;
			
			resultingTextures = new Texture[umaGenerator.textureNameList.Length];
			Rect nullRect = new Rect(0,0,0,0);
			
			for(int textureType = 0; textureType < umaGenerator.textureNameList.Length; textureType++){
				
				if(umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions[0].source.materialSample.HasProperty(umaGenerator.textureNameList[textureType])){
					for(int i = 0; i < umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions.Count; i++){
						
						DKUMAData.AtlasMaterialDefinition atlasElement = umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions[i];
						resolutionScale = umaData.atlasList.atlas[atlasIndex].resolutionScale * umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions[i].source.slotData.overlayScale;
						
						Vector2 offsetAdjust = new Vector2(_DKumaGenerator.atlasResolution/1024,_DKumaGenerator.atlasResolution/1024);
						
						if(!atlasElement.isRectShared){
							if(textureType == 0){
								textureModuleList[moduleCount].localScale = new Vector3(atlasElement.atlasRegion.width/_DKumaGenerator.atlasResolution,atlasElement.atlasRegion.height/_DKumaGenerator.atlasResolution,1);
								
								textureModuleList[moduleCount].localPosition = new Vector3( Mathf.Lerp(-1,1,(offsetAdjust.x + atlasElement.atlasRegion.x + atlasElement.atlasRegion.width*0.5f)/_DKumaGenerator.atlasResolution),
								Mathf.Lerp(-1,1,(offsetAdjust.y + atlasElement.atlasRegion.y + atlasElement.atlasRegion.height*0.5f)/_DKumaGenerator.atlasResolution),3.0f);
							}
							
//							Material tempMaterial = UnityEngine.Object.Instantiate(umaGenerator.textureMerge.material) as Material;
//							textureModuleList[moduleCount].renderer.material = tempMaterial;
							
							if(atlasElement.source.baseTexture[textureType]){
								atlasElement.source.baseTexture[textureType].filterMode = FilterMode.Point;
								atlasElement.source.baseTexture[0].filterMode = FilterMode.Point;
							}
							#if UNITY_EDITOR
							textureModuleList[moduleCount].GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex",atlasElement.source.baseTexture[textureType]);
							textureModuleList[moduleCount].GetComponent<Renderer>().sharedMaterial.SetTexture("_ExtraTex",atlasElement.source.baseTexture[0]);
		                    textureModuleList[moduleCount].GetComponent<Renderer>().sharedMaterial.SetColor("_Color", atlasElement.source.GetMultiplier(0, textureType));
		                    textureModuleList[moduleCount].GetComponent<Renderer>().sharedMaterial.SetColor("_AdditiveColor", atlasElement.source.GetAdditive(0, textureType));
							textureModuleList[moduleCount].name = atlasElement.source.baseTexture[textureType].name;
							#else
							textureModuleList[moduleCount].renderer.material.SetTexture("_MainTex",atlasElement.source.baseTexture[textureType]);
							textureModuleList[moduleCount].renderer.material.SetTexture("_ExtraTex",atlasElement.source.baseTexture[0]);
		                    textureModuleList[moduleCount].renderer.material.SetColor("_Color", atlasElement.source.GetMultiplier(0, textureType));
		                    textureModuleList[moduleCount].renderer.material.SetColor("_AdditiveColor", atlasElement.source.GetAdditive(0, textureType));
							textureModuleList[moduleCount].name = atlasElement.source.baseTexture[textureType].name;
							#endif
							Transform tempModule = textureModuleList[moduleCount];
							moduleCount++;
							
							for(int i2 = 0; i2 < atlasElement.source.overlays.Length; i2++){
							
								if(atlasElement.source.rects[i2] != nullRect){
									textureModuleList[moduleCount].localScale = new Vector3((atlasElement.source.rects[i2].width/_DKumaGenerator.atlasResolution)*resolutionScale,(atlasElement.source.rects[i2].height/_DKumaGenerator.atlasResolution)*resolutionScale,1);
									textureModuleList[moduleCount].localPosition = new Vector3(Mathf.Lerp(-1,1,(offsetAdjust.x + atlasElement.atlasRegion.x + atlasElement.source.rects[i2].x*resolutionScale + atlasElement.source.rects[i2].width*0.5f*resolutionScale)/_DKumaGenerator.atlasResolution),
									Mathf.Lerp(-1,1,(offsetAdjust.y + atlasElement.atlasRegion.y + atlasElement.source.rects[i2].y*resolutionScale + atlasElement.source.rects[i2].height*0.5f*resolutionScale)/_DKumaGenerator.atlasResolution),tempModule.localPosition.z - 0.1f - 0.1f*i2);
								}else{
									textureModuleList[moduleCount].localScale = tempModule.localScale;
									textureModuleList[moduleCount].localPosition = new Vector3(tempModule.localPosition.x,tempModule.localPosition.y,tempModule.localPosition.z - 0.1f - 0.1f*i2);
								}
								
//								Material tempGenMaterial = umaGenerator.textureMerge.GenerateMaterial(umaGenerator.textureMerge.material);
//								textureModuleList[moduleCount].renderer.material = tempGenMaterial;
								
								atlasElement.source.overlays[i2].textureList[textureType].filterMode = FilterMode.Point;
								atlasElement.source.overlays[i2].textureList[0].filterMode = FilterMode.Point;
								#if UNITY_EDITOR
								textureModuleList[moduleCount].GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex",atlasElement.source.overlays[i2].textureList[textureType]);
								textureModuleList[moduleCount].GetComponent<Renderer>().sharedMaterial.SetTexture("_ExtraTex",atlasElement.source.overlays[i2].textureList[0]);
		                        textureModuleList[moduleCount].GetComponent<Renderer>().sharedMaterial.SetColor("_Color", atlasElement.source.GetMultiplier(i2 + 1, textureType));
		                        textureModuleList[moduleCount].GetComponent<Renderer>().sharedMaterial.SetColor("_AdditiveColor", atlasElement.source.GetAdditive(i2 + 1, textureType));
								#endif
								#if !UNITY_EDITOR
								textureModuleList[moduleCount].renderer.material.SetTexture("_MainTex",atlasElement.source.overlays[i2].textureList[textureType]);
								textureModuleList[moduleCount].renderer.material.SetTexture("_ExtraTex",atlasElement.source.overlays[i2].textureList[0]);
		                        textureModuleList[moduleCount].renderer.material.SetColor("_Color", atlasElement.source.GetMultiplier(i2 + 1, textureType));
		                        textureModuleList[moduleCount].renderer.material.SetColor("_AdditiveColor", atlasElement.source.GetAdditive(i2 + 1, textureType));
								#endif
								textureModuleList[moduleCount].name = atlasElement.source.overlays[i2].textureList[textureType].name;
								
								moduleCount++;
							}
//							yield return null;
						}
					}
					
					//last element for this textureType
					moduleCount = 0;
					
					umaGenerator.textureMerge.gameObject.SetActive(true);
		
					destinationTexture = new RenderTexture(Mathf.FloorToInt(umaData.atlasList.atlas[atlasIndex].cropResolution.x),Mathf.FloorToInt(umaData.atlasList.atlas[atlasIndex].cropResolution.y),0,RenderTextureFormat.ARGB32,RenderTextureReadWrite.Default);					
					destinationTexture.filterMode = FilterMode.Point;
					renderCamera = umaGenerator.textureMerge.myCamera;
					Vector3 tempPosition = renderCamera.transform.position;
					
					renderCamera.orthographicSize = umaData.atlasList.atlas[atlasIndex].cropResolution.y/_DKumaGenerator.atlasResolution;		
					renderCamera.transform.position = tempPosition + (-Vector3.right*(1 - umaData.atlasList.atlas[atlasIndex].cropResolution.x/_DKumaGenerator.atlasResolution)) + (-Vector3.up*(1 - renderCamera.orthographicSize));
		
					renderCamera.targetTexture = destinationTexture;
			        RenderTexture.active = destinationTexture;
			        renderCamera.Render();
					
					renderCamera.transform.position = tempPosition;
					
					Texture2D tempTexture;
					if(_DKumaGenerator.convertRenderTexture){
						tempTexture = new Texture2D(destinationTexture.width,destinationTexture.height,TextureFormat.ARGB32,true);
						tempTexture.ReadPixels(new Rect(0,0,destinationTexture.width,destinationTexture.height),0,0,false);	
						
						resultingTextures[textureType] = tempTexture as Texture;
							
						RenderTexture.active = null;
						
						destinationTexture.Release();
						if (  Application.isPlaying ) UnityEngine.GameObject.Destroy(destinationTexture);
						# if Editor
						GameObject.DestroyImmediate(destinationTexture);
						#endif
						yield return null;
						tempTexture = resultingTextures[textureType] as Texture2D;
						tempTexture.Apply();
						
						resultingTextures[textureType]= tempTexture;	
						yield return null;
					}else{
						destinationTexture.filterMode = FilterMode.Bilinear;
						resultingTextures[textureType] = destinationTexture;
					}
					umaGenerator.textureMerge.gameObject.SetActive(false);
				}else{
						
				}
			}
			
			for(int textureModuleIndex = 0; textureModuleIndex < textureModuleList.Length; textureModuleIndex++){
				textureModuleList[textureModuleIndex].gameObject.SetActive(false);
//				UnityEngine.Object.DestroyImmediate(textureModuleList[textureModuleIndex].gameObject.renderer.material);
//				UnityEngine.Object.DestroyImmediate(textureModuleList[textureModuleIndex].gameObject);
			}
			
			umaData.atlasList.atlas[atlasIndex].resultingAtlasList = resultingTextures;
			umaData.atlasList.atlas[atlasIndex].materialSample = UnityEngine.Object.Instantiate(umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions[0].source.materialSample) as Material;
			umaData.atlasList.atlas[atlasIndex].materialSample.name = umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions[0].source.materialSample.name;
			for(int finalTextureType = 0; finalTextureType < umaGenerator.textureNameList.Length; finalTextureType++){
				if(umaData.atlasList.atlas[atlasIndex].materialSample.HasProperty(umaGenerator.textureNameList[finalTextureType])){
					umaData.atlasList.atlas[atlasIndex].materialSample.SetTexture(umaGenerator.textureNameList[finalTextureType],resultingTextures[finalTextureType]);
				}
			}
		}		
	}


	
	
    protected override void Stop()
    {
		
	}
}