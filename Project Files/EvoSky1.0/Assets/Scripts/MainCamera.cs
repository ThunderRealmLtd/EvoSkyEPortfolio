using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        // Grab the player's position
        Transform HuPlayer = GameObject.FindWithTag("HuPlayer").transform;

        // Create a container to hold our current position
        Vector3 tempPos = transform.position;
        // Which we then move to the player's position
        tempPos.x = HuPlayer.position.x;
        tempPos.z = HuPlayer.position.z;

        // Move the camera to that position
        transform.position = tempPos;
    }
	
	// Update is called once per physics step
	void FixedUpdate ()
    {
        // Grab the player's position
        Transform HuPlayer = GameObject.FindWithTag("HuPlayer").transform;

        // Create a container to hold our current position
        Vector3 tempPos = transform.position;
        // Which we then move to the player's position
        tempPos.x = HuPlayer.position.x;
        tempPos.z = HuPlayer.position.z;

        // Move the camera to that position
        transform.position = tempPos;
    }
}
