using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	GameObject MainCam;
    float rotSpeed = 1;
	
	// Use this for initialization
	void Start () {
		Debug.Log ("Calling the camera start");
        rotSpeed = 1;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
        
        if (Input.GetKey (KeyCode.UpArrow)) {
            Debug.Log ("up");
			 transform.Rotate(Vector3.left * rotSpeed);
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			 transform.Rotate(Vector3.right * rotSpeed);
		}
        if (Input.GetKey (KeyCode.RightArrow)) {
			 transform.Rotate(Vector3.up * rotSpeed);
		}
        if (Input.GetKey (KeyCode.LeftArrow)) {
			 transform.Rotate(Vector3.down * rotSpeed);
		}
       
        
	}
}
