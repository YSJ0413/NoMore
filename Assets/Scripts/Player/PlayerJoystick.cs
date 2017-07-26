using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerJoystick : MonoBehaviour {

    public int moveSpeed = 5;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 moveVec = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical")) * moveSpeed;
        moveVec.Normalize();
        transform.position = transform.position + (moveVec * Time.deltaTime * moveSpeed);
    }
}
