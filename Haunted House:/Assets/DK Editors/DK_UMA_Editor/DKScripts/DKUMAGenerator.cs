using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Object = UnityEngine.Object;

[ExecuteInEditMode]
public class DKUMAGenerator : MonoBehaviour {	
	public bool usePRO = true;
	public static bool AutoDel;
	public bool convertRenderTexture;
	public bool fitAtlas;
	public bool AtlasCrop;
	public bool AutoSaveModel;
	public bool ManualSaveModel;
	public DK_Model _DK_Model;
	
	GameObject DK_UMACrowdObj;
	public string AvatarSavePath;
	public DK_UMACrowd _DK_UMACrowd;


	
	public int meshUpdates;
	public int maxMeshUpdates;
	
	public int atlasResolution;
	public int maxPixels;


	public DKUMAData umaData;
	public List<DKUMAData> umaDirtyList = new List<DKUMAData>();
	
	public string[] textureNameList;
	public DKUMAGeneratorCoroutine umaGeneratorCoroutine;
	
	public Transform textureMergePrefab;
	public DKTextureMerge textureMerge;	
	Matrix4x4 tempMatrix;

	bool EditorStarted = false;

	public bool FixedHead = false;

    List<SkinnedMeshCombiner.CombineInstance> combinedMeshList;
	List<Material> combinedMaterialList;
	
	public DKUMAGenerator ()
	{
		#if UNITY_EDITOR     
		EditorApplication.update += Update;
		#endif
	}
	/*
	public void OnEnable () {
		DK_UMACrowdObj = GameObject.Find("UMACrowd");
		_DK_UMACrowd = DK_UMACrowdObj.gameObject.GetComponent<  DK_UMACrowd >();
		_DK_UMACrowd.generateUMA = false;
			_DK_UMACrowd.generateLotsUMA = false;
			_DK_UMACrowd.UMAGenerated = true;
			umaDirtyList.Clear();
	}
	*/
	public void Awake () {
	/*	try {
			Destroy ( textureMerge.gameObject );
		}catch (MissingReferenceException){}*/
		DK_UMACrowdObj = GameObject.Find("DKUMACrowd");
		if ( DK_UMACrowdObj ) _DK_UMACrowd = DK_UMACrowdObj.gameObject.GetComponent<  DK_UMACrowd >();
		#if UNITY_EDITOR     
			if(usePRO && !UnityEditorInternal.InternalEditorUtility.HasPro()){
			//	Debug.LogWarning("You might need to disable usePRO option in the Setup tab of DK UMA Editor window.");
				usePRO = false;
			}
		//	else usePRO = true;
		#endif
		
	//		_DK_UMACrowd.generateUMA = false;
	//		_DK_UMACrowd.generateLotsUMA = false;
	//		_DK_UMACrowd.UMAGenerated = true;
	//		umaDirtyList.Clear();
		
		maxMeshUpdates = 1;
		if( atlasResolution == 0 ) atlasResolution = 256;
		umaGeneratorCoroutine = new DKUMAGeneratorCoroutine();
		
		if(!textureMerge && DK_UMACrowd.GeneratorMode == "Preset"){
			GameObject obj = GameObject.Find ("TextureMerge");
			if ( obj ) textureMerge = obj.GetComponent("DKTextureMerge") as DKTextureMerge;
			if(!textureMerge){
				Transform tempTextureMerger = Instantiate(textureMergePrefab,Vector3.zero,Quaternion.identity) as Transform;
				tempTextureMerger.name = "TextureMerge";
				textureMerge = tempTextureMerger.GetComponent("DKTextureMerge") as DKTextureMerge;
				textureMerge.transform.parent = transform;
				textureMerge.gameObject.SetActive(false);
			}
		}
		else {}


		//Garbage Collection hack
        var mb = (System.GC.GetTotalMemory(false) / (1024 * 1024));
        if (mb < 10)
        {
            byte[] data = new byte[10 * 1024 * 1024];
            data[0] = 0;
            data[10 * 1024 * 1024 - 1] = 0;
        }
	}
	
	void Update () {
		if( umaDirtyList.Count > 0){
			OnDirtyUpdate();	
		}
		meshUpdates = 0;	
	}
// Added by DK 
	#if UNITY_EDITOR
	public void UpdateEditor () {
		if(umaDirtyList.Count > 0){
			OnDirtyUpdate();	
		}
		meshUpdates = 0;
	}	
	#endif	
	
// end Added by DK 	
	public virtual void OnDirtyUpdate() {
		umaData = umaDirtyList[0];
//		Debug.Log ( "umaDirtyList.Count = "+ umaDirtyList.Count.ToString() );
		if(umaData && umaData.isMeshDirty){
			if(!umaData.isTextureDirty){
				UpdateUMAMesh(false);
			}
			umaData.isMeshDirty = false;
		}
        if(umaData && umaData.isTextureDirty){
			
			umaGeneratorCoroutine.Prepare(this);
// Added by DK
			if ( !EditorStarted ){
				umaGeneratorCoroutine.EditorStart();
				EditorStarted = true;
			//	Debug.Log ( " EditorStart() " );
			}
// end Added by DK
            if (umaGeneratorCoroutine.Work())
            {
                UpdateUMAMesh(true);
                umaData.isTextureDirty = false;
            }
            else
            {
                return;
            }
		}
        if(umaData && umaData.isShapeDirty){
			UpdateUMABody(umaData);
			umaData.isShapeDirty = false;
			
			UMAReady();
		
		}else{
			
			UMAReady();
			
		}
	//	UMAReady();	
	}	
	public virtual void addDirtyUMA(DKUMAData umaToAdd) {	
		if(umaToAdd){
	//		Debug.Log ( "addDirtyUMA "+umaToAdd.transform.name);
			umaDirtyList.Add(umaToAdd);
		}
	}
	
	public virtual void UMAReady(){	
		if(umaData){
			umaData.myRenderer.enabled = true;
		    umaDirtyList.RemoveAt(0);
		    umaData.FireUpdatedEvent(); 
		//	Debug.Log ( "Remove from umaDirtyList :"+umaData.transform.parent.name );

			GameObject ModelsObj = GameObject.Find("NPC Models");
			if ( umaData.transform.parent.parent == null ){
				umaData.transform.parent.parent = ModelsObj.transform;
			}

			if ( umaData.Loading != true ){
		//		umaData.transform.parent.parent = ModelsObj.transform;
			}

			if ( umaData.transform.parent.parent != null && umaData.transform.parent.parent.parent == null ){
				umaData.transform.parent.parent.parent = ModelsObj.transform;
			}
			_DK_Model = umaData.transform.parent.gameObject.GetComponent< DK_Model >();
		//	_DK_Model.path = ( AvatarSavePath +"/"+ umaData.transform.parent.name + ".Avatar" ) ;
			umaData.Loading = true;

		//	umaData.SaveToMemoryStream();

			if ( _DK_UMACrowd.RaceAndGender.MultiRace == "" || _DK_UMACrowd.RaceAndGender.MultiRace == "Random for One" ) _DK_UMACrowd.RaceAndGender.Race = "Random";
		//	var animator = umaData.GetComponent<Animator>();
		//	if (animator) Object.DestroyImmediate(animator);
			SaveAvatar ();
		//	Debug.Log ( "UMAReady "+umaData.transform.name);
		}
    }
	
	public void SaveAvatar (){

		if(umaData && _DK_UMACrowd.UMAGenerated != true){
		//	Debug.Log ( " Autosaving 1" );
			umaData.SaveToMemoryStream();
			#if UNITY_EDITOR
		//	var path = EditorUtility.SaveFilePanel("Save serialized Avatar","",umaData.transform.parent.name + ".Avatar","Avatar");
		//	if(path.Length != 0) {
			//	System.IO.File.WriteAllText(path, umaData.streamedUMA);
		//	}
			#endif
		}
		else {
			if(umaData &&  _DK_UMACrowd.UMAGenerated != true){
				Debug.Log ( " Autosaving 2" );
				umaData.SaveToMemoryStream();
			//	Debug.Log ( umaData.transform.parent.name+" Autosaving to "+AvatarSavePath );
			//	if ( _DK_Model.path != null || _DK_Model.path != "" ) System.IO.File.WriteAllText(_DK_Model.path, umaData.streamedUMA);
			//	else Debug.Log ( "No Save Path Selected, you need to configure it in the 'Setup' Panel." );
				_DK_UMACrowd.UMAGenerated = true;
			}
		}
		if ( umaData && umaData.transform.parent != null  && _DK_UMACrowd.UMAGenerated != true ){
		//	Debug.Log ( " Autosaving 3" );
			umaData.Loading = true;
			_DK_UMACrowd.UMAGenerated = true;
		
		}
		
	}

#if !UNITY_4_2
    private struct AnimationState
    {
        public int stateHash;
        public float stateTime;
    }
#endif

    public virtual void UpdateUMABody (DKUMAData umaData){
		if(umaData)
        {
/*
#if !UNITY_4_2
            AnimationState[] snapshot = null;
#endif
*/
            if (animationController)
            {

				/*
#if !UNITY_4_2
                var animator = umaData.GetComponent<Animator>();
                if (animator != null)
                {

                    snapshot = new AnimationState[animator.layerCount];
                    for (int i = 0; i < animator.layerCount; i++)
                    {
                        var state = animator.GetCurrentAnimatorStateInfo(i);
                        snapshot[i].stateHash = state.nameHash;
                        snapshot[i].stateTime = Mathf.Max(0, state.normalizedTime - Time.deltaTime / state.length);
                    }
                }
#endif
*/
                foreach (var entry in umaData.boneList)
                {
                    entry.Value.boneTransform.localPosition = entry.Value.originalBonePosition;
					entry.Value.boneTransform.localScale = entry.Value.originalBoneScale;
                    entry.Value.boneTransform.localRotation = entry.Value.originalBoneRotation;
                }
            }
		    umaData.ApplyDNA();
			if (animationController && FixedHead )
		    {
                var animator = umaData.GetComponent<Animator>();
                if (animator) Object.DestroyImmediate(animator);
		        var oldParent = umaData.transform.parent;
                umaData.transform.parent = null;
                CreateAnimator(umaData.gameObject, umaData.umaRecipe.raceData.TPose, animationController);
		        umaData.transform.parent = oldParent;
                animator = umaData.GetComponent<Animator>();
#if !UNITY_4_2
             /*   if (snapshot != null)
                {
                    for (int i = 0; i < animator.layerCount; i++)
                    {
						animator.Play(i, snapshot[i].stateHash, snapshot[i].stateTime);
                    }
                    animator.Update(0);
                }*/
#endif
		    }
		    umaData.UpdateCollider();
		}
	}


	public static void CreateAnimator(GameObject root, UMA.UmaTPose umaTPose, RuntimeAnimatorController controller)
    {
        umaTPose.DeSerialize();
        var animator = root.AddComponent<Animator>();
        animator.avatar = CreateAvatar(root, umaTPose);
        animator.runtimeAnimatorController = controller;
        animator.applyRootMotion = true;
     //   animator.animatePhysics = false;
	//	AnimationMode.
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
    }

	public static Avatar CreateAvatar(GameObject root, UMA.UmaTPose umaTPose)
    {
        HumanDescription description = CreateHumanDescription(root, umaTPose);
        Avatar res = AvatarBuilder.BuildHumanAvatar(root, description);
        return res;
    }

	public static HumanDescription CreateHumanDescription(GameObject root, UMA.UmaTPose umaTPose)
    {
        var res = new HumanDescription();
        res.armStretch = 0;
        res.feetSpacing = 0;
        res.legStretch = 0;
        res.lowerArmTwist = 0.2f;
        res.lowerLegTwist = 1f;
        res.upperArmTwist = 0.5f;
        res.upperLegTwist = 0.1f;

        res.human = umaTPose.humanInfo;
        res.skeleton = umaTPose.boneInfo;
        res.skeleton[0].name = root.name;
        SkeletonModifier(root, ref res.skeleton);
        return res;
    }

    private static void SkeletonModifier(GameObject root, ref SkeletonBone[] bones)
    {
        var umaData = root.GetComponent<DKUMAData>();
        for(var i = 0; i < bones.Length; i++)
        {
            var skeletonbone = bones[i];
            DKUMAData.BoneData entry;
            if (umaData.boneList.TryGetValue(skeletonbone.name, out entry))
            {
                //var entry = umaData.boneList[skeletonbone.name];
                skeletonbone.position = entry.boneTransform.localPosition;
                //skeletonbone.rotation = entry.boneTransform.localRotation;
                skeletonbone.scale = entry.boneTransform.localScale;
                bones[i] = skeletonbone;
            }
        }
    }

    public RuntimeAnimatorController animationController;

	public virtual void UpdateUMAMesh(bool updatedAtlas){
		

        combinedMeshList = new List<SkinnedMeshCombiner.CombineInstance>();
        combinedMaterialList = new List<Material>();

        if (updatedAtlas)
        {
            CombineByShader();
        }
        else
        {
			CombineByMaterial();
        }
			
		if( umaData.firstBake )
        {
            umaData.myRenderer.sharedMesh = new Mesh();
        }
		
		var boneMap = new Dictionary<Transform, Transform>();
        SkinnedMeshCombiner.CombineMeshes(umaData.myRenderer, combinedMeshList.ToArray(), boneMap);

        if (updatedAtlas)
        {
            RecalculateUV();
        }

        umaData.umaRecipe.ClearDNAConverters();
        for (int i = 0; i < umaData.umaRecipe.slotDataList.Length; i++)
        {
			DKSlotData slotData = umaData.umaRecipe.slotDataList[i];
			if(slotData != null){

            	umaData.EnsureBoneData(slotData.umaBoneData, boneMap);
				
            	umaData.umaRecipe.AddDNAUpdater(slotData.slotDNA);
			}
        }

        umaData.myRenderer.quality = SkinQuality.Bone4;
        umaData.myRenderer.useLightProbes = true;
        umaData.myRenderer.sharedMaterials = combinedMaterialList.ToArray();
		//umaData.myRenderer.sharedMesh.RecalculateBounds();
        umaData.myRenderer.sharedMesh.name = "UMAMesh";

        umaData.firstBake = false;
    }

	void CombineByShader(){
		SkinnedMeshCombiner.CombineInstance combineInstance;
		
		for(int atlasIndex = 0; atlasIndex < umaData.atlasList.atlas.Count; atlasIndex++){
			combinedMaterialList.Add(umaData.atlasList.atlas[atlasIndex].materialSample);
			
			for(int materialDefinitionIndex = 0; materialDefinitionIndex < umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions.Count; materialDefinitionIndex++){
			
				combineInstance = new SkinnedMeshCombiner.CombineInstance();
	           
				combineInstance.destMesh = new int[1];
	            combineInstance.mesh = umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions[materialDefinitionIndex].source.slotData.meshRenderer.sharedMesh;
	            combineInstance.bones = umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions[materialDefinitionIndex].source.slotData.meshRenderer.bones;
	            
	            combineInstance.destMesh[0]=atlasIndex;
	            combinedMeshList.Add(combineInstance);
			}
		}
	}
	
	void CombineByMaterial()
    {		
		DKSlotData[] slots = umaData.umaRecipe.slotDataList;
        bool[] shareMaterial = new bool[slots.Length];
		
		SkinnedMeshCombiner.CombineInstance combineInstance;
        
		int indexCount = 0;
        for(int slotIndex = 0; slotIndex < slots.Length; slotIndex++){
			if(slots[slotIndex] != null){
				if(!shareMaterial[slotIndex]){
					combineInstance = new SkinnedMeshCombiner.CombineInstance();
					combineInstance.destMesh = new int[1];
		            combineInstance.mesh = slots[slotIndex].meshRenderer.sharedMesh;
		            combineInstance.bones = slots[slotIndex].meshRenderer.bones;
		            
		            combineInstance.destMesh[0]=indexCount;
		            combinedMeshList.Add(combineInstance);
					
					Material tempMaterial = Instantiate(slots[slotIndex].materialSample) as Material;
					tempMaterial.name = slots[slotIndex].slotName;
					for(int textureType = 0; textureType < textureNameList.Length; textureType++){
						if(tempMaterial.HasProperty(textureNameList[textureType])){
							slots[slotIndex].overlayList[0].textureList[textureType].filterMode = FilterMode.Bilinear;
							tempMaterial.SetTexture(textureNameList[textureType],slots[slotIndex].overlayList[0].textureList[textureType]);
						}
					}
					combinedMaterialList.Add(tempMaterial);
					
					
					shareMaterial[slotIndex] = true;
					
					for(int slotIndex2 = slotIndex; slotIndex2 < slots.Length; slotIndex2++){
						if(slots[slotIndex2] != null){
							if(slotIndex2 != slotIndex && !shareMaterial[slotIndex2]){
								if(slots[slotIndex].overlayList[0].textureList[0].name == slots[slotIndex2].overlayList[0].textureList[0].name){	
									combineInstance = new SkinnedMeshCombiner.CombineInstance();
									combineInstance.destMesh = new int[1];
						            combineInstance.mesh = slots[slotIndex2].meshRenderer.sharedMesh;
						            combineInstance.bones = slots[slotIndex2].meshRenderer.bones;
						            
						            combineInstance.destMesh[0]=indexCount;
						            combinedMeshList.Add(combineInstance);
									
									shareMaterial[slotIndex2] = true;
								}
							}
						}
					}
					indexCount++;	
					
				}
			}else{
				shareMaterial[slotIndex] = true;
			}
		}
	}
	
	void RecalculateUV(){
		List<Rect> tempAtlasRect = new List<Rect>();
		List<int> meshVertexAmount = new List<int>();
		
		for(int atlasIndex = 0; atlasIndex < umaData.atlasList.atlas.Count; atlasIndex++){
			for(int materialDefinitionIndex = 0; materialDefinitionIndex < umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions.Count; materialDefinitionIndex++){
				tempAtlasRect.Add(umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions[materialDefinitionIndex].atlasRegion);
				meshVertexAmount.Add(umaData.atlasList.atlas[atlasIndex].atlasMaterialDefinitions[materialDefinitionIndex].source.slotData.meshRenderer.sharedMesh.vertexCount);
			}
		}

		Vector2[] originalUVs = umaData.myRenderer.sharedMesh.uv;
        Vector2[] atlasUVs = new Vector2[originalUVs.Length];
		
        int rectIndex = 0;
        int vertTracker = 0;
		
		for(int i = 0; i < atlasUVs.Length; i++ ) {
			
			atlasUVs[i].x = Mathf.Lerp( tempAtlasRect[rectIndex].xMin/atlasResolution, tempAtlasRect[rectIndex].xMax/atlasResolution, originalUVs[i].x );
            atlasUVs[i].y = Mathf.Lerp( tempAtlasRect[rectIndex].yMin/atlasResolution, tempAtlasRect[rectIndex].yMax/atlasResolution, originalUVs[i].y );            
			
			if(originalUVs[i].x > 1 || originalUVs[i].y > 1){
			//	Debug.Log(i);	
			}
			
            if(i >= (meshVertexAmount[rectIndex] + vertTracker) - 1) {
				vertTracker = vertTracker + meshVertexAmount[rectIndex];
                rectIndex++;
            }
        }
		umaData.myRenderer.sharedMesh.uv = atlasUVs;	
	}	
}