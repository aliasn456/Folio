using UnityEngine;
using System.Collections;

public class IdleRunJump : MonoBehaviour {

	protected Animator animator;
//	public float DirectionDampTime = .25f;
//	public bool ApplyGravity = true; 
//	private float m_VerticalSpeed = 0;

	void Start () 
	{
		animator = GetComponent<Animator>();
		
		if(animator.layerCount >= 2)
			animator.SetLayerWeight(1, 1);
	}
		
	void Update () 
	{
		this.transform.localPosition = new Vector3 ( 0,0,0 );
		if (animator)
		{
		//	AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);			

		//	if (stateInfo.IsName("Base Layer.Run"))
		//	{
		//		if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2")){
		//			animator.SetBool("Jump", true);  
		//		}
        //    }
		//	else
		//	{
		//		animator.SetBool("Jump", false);                
        //    }
		
      	//	float h = Input.GetAxis("Horizontal");
        //	float v = Input.GetAxis("Vertical");
			
		//	animator.SetFloat("Speed", h*h+v*v);
        //    animator.SetFloat("Direction", h, DirectionDampTime, Time.deltaTime);	
		
		}else{
			animator = GetComponent<Animator>();
		}
	}

	void OnAvatarMove()
	{
	/*	CharacterController controller = GetComponent<CharacterController>();

		if (controller && animator)
		{

			Vector3 deltaPosition = animator.deltaPosition;
			if(ApplyGravity)
			{			
				m_VerticalSpeed += Physics.gravity.y * Time.deltaTime;						
				deltaPosition.y = m_VerticalSpeed * Time.deltaTime;
			}
			if (controller.Move(deltaPosition) == CollisionFlags.Below) m_VerticalSpeed = 0;			
			transform.rotation = animator.rootRotation;
		}*/
	}     
}
