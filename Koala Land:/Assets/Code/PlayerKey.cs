using UnityEngine;
using System.Collections;


public class PlayerKey : MonoBehaviour {

     public bool hasKey = false;
     
	// Use this for initialization
	void Start () {
	   hasKey = false;
	}
	
	// Update is called once per frame
	void Update () {
	   
	}
    
    void OnCollisionEnter (Collision col)
	{   
        Debug.Log(col.gameObject.name);
        if (col.gameObject.name == "Key"){
            hasKey = true;
            Debug.Log("hasKay = true");
        }
         if (col.gameObject.name == "Ending Rock" && hasKey == true){
             Application.LoadLevel("Cave");
         }
         
        
	}

}
