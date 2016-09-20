using UnityEngine;
using System.Collections;

public class ReflectCode : MonoBehaviour {

    public GameObject mirror;
    public GameObject player;
    public float offset;
    public Direction directionFaced;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	   
       if (directionFaced == Direction.X)
       {
            offset = (mirror.transform.position.x - player.transform.position.x);
            
            
            Vector3 temp = transform.position;
            temp.x = mirror.transform.position.x + offset;
            temp.y = player.transform.position.y;
            temp.z = player.transform.position.z;
            
            transform.position = temp;
       }
        if (directionFaced == Direction.Y)
       {
            offset = (mirror.transform.position.y - player.transform.position.y);
            
            
            Vector3 temp = transform.position;
            temp.x = player.transform.position.x;
            temp.y = mirror.transform.position.y + offset;
            temp.z = player.transform.position.z;
            
            transform.position = temp;
       }
       
         if (directionFaced == Direction.Z)
       {
            offset = (mirror.transform.position.z - player.transform.position.z);
            
            
            Vector3 temp = transform.position;
            temp.x = player.transform.position.x;
            temp.y = player.transform.position.y;
            temp.z = mirror.transform.position.z + offset;
            
            transform.position = temp;
       }
	}
    
    public enum Direction{
        X,Y,Z
    };
}
