using UnityEngine;
using System.Collections;

public class SideCamera : MonoBehaviour {

    float rotSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetKey (KeyCode.UpArrow)) {
			 transform.Rotate(Vector3.up * rotSpeed);
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			 transform.Rotate(Vector3.down * rotSpeed);
		}
        if (Input.GetKey (KeyCode.RightArrow)) {
			 transform.Rotate(Vector3.right * rotSpeed);
		}
        if (Input.GetKey (KeyCode.LeftArrow)) {
			 transform.Rotate(Vector3.left * rotSpeed);
		}
        
	}
}
