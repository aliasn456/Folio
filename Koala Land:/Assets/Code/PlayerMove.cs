using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	public float moveSpeed;
	public float jumpHeight;
	private bool grounded;
	private bool doubleJump;
    private float speed = 10;

    

	// Use this for initialization
	void Start () {
		doubleJump = false;
       
	}
    
    // Update is called once per frame
	void Update () 
    {
        StartCoroutine(CoUpdate());
	}

	void FixedUpdate ()
	{
		if (grounded) {
			doubleJump=false;
		}
		if (Input.GetKeyDown (KeyCode.Space) && grounded) {
			string s = "Getting in here";
			Debug.Log(s);
			Jump();
		}
		
		if ( Input.GetKeyDown(KeyCode.P) && !doubleJump && !grounded) {
			string s = "Not Grounded";
			Debug.Log (s);
			Jump ();
			doubleJump = true;
		}

    }
    
    IEnumerator CoUpdate() 
    {
      //directions
      // fix air glide 
		if (Input.GetKey (KeyCode.L)) {
			transform.position += transform.right * Time.deltaTime * speed;
            transform.Rotate(Vector3.up);
            yield return null;
		}
		if (Input.GetKey (KeyCode.J)) {
			transform.position += -transform.right * Time.deltaTime * speed;
            transform.Rotate(Vector3.down);
            yield return null;
		}
		if (Input.GetKey (KeyCode.I)) {
			transform.position += transform.forward * Time.deltaTime * speed;
            yield return null;
		}
		if (Input.GetKey (KeyCode.K)) {
		    transform.position += -transform.forward * Time.deltaTime * speed;
            yield return null;	
         }   
    }
    
    
    void move(Vector3 direction)
     {
         GetComponent<Rigidbody>().velocity = Vector3.zero;
         transform.Translate(direction * speed * Time.deltaTime, Space.Self);
     }

    
    
	
	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag  == "Ground" && col.transform.position.y < transform.position.y)
		{
			grounded = true;
		}

        if(col.gameObject.name == "Cube(yellow)" && col.transform.position.y > transform.position.y)
        {
            GameObject box = GameObject.Find("Cube(yellow)");
			box.transform.Translate(Vector3.up * 1);
        }
        
        
	}
	
	public void Jump()
    {
		GetComponent<Rigidbody>().velocity = new Vector3 (GetComponent<Rigidbody> ().velocity.x, jumpHeight);
		grounded = false;
	}
    

}