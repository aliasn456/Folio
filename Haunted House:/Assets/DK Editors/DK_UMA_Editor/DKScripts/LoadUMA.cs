using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UMA;

public class LoadUMA : MonoBehaviour {
	public string _StreamedUMA;
//	UMAData umaData;
	UMAAvatarBase avatar;
	public UMADnaHumanoid umaDna = new UMADnaHumanoid();




	void Start (){
		Loading();
		InvokeRepeating ( "Testing" , 0.01f, 0.1f );
	}

	void Loading (){
		if (  Application.isPlaying ){
		//	umaData = gameObject.GetComponent<UMAData>();

			var selectedTransform = transform;
			avatar = selectedTransform.GetComponent<UMAAvatarBase>();
			
			while (avatar == null && selectedTransform.parent != null){
				selectedTransform = selectedTransform.parent;
				avatar = selectedTransform.GetComponent<UMAAvatarBase>();
			}
			if (avatar != null ){
				var asset = ScriptableObject.CreateInstance<UMATextRecipe>();
				asset.recipeString = _StreamedUMA;

				if ( avatar.umaData == null ) 
					avatar.umaData = gameObject.AddComponent<UMAData>();
				if ( avatar.umaData.umaRecipe == null ) 
					avatar.umaData.umaRecipe = new UMAData.UMARecipe();

				avatar.Load(asset);
				Destroy(asset);
				Debug.Log ( "Auto Loading" );
			//	Invoke ( "ApplyDNA", 3f );
			//	ApplyDNA();
			}
		//	if (  avatar.umaData != null ){
		//		ApplyDNA();
		//	}
		}
	}

	void Testing (){
	//	Debug.Log ( " invoke");
		if (  avatar.umaData != null ){
		/*	var asset = ScriptableObject.CreateInstance<UMATextRecipe>();
			asset.recipeString = _StreamedUMA;
			avatar.Load(asset);
			Destroy(asset);
			Debug.Log ( "Auto Loading" );*/

			ApplyDNA();
			CancelInvoke();
		}
	}

	void ApplyDNA (){
	//	Debug.Log ( "applying dna" );
		avatar.umaData.umaRecipe.ClearDna();
		avatar.umaData.umaRecipe.AddDna(umaDna);
	}
}
