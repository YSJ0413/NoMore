using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
        BoxCollider2D myCollider = GetComponent<BoxCollider2D>();

        Rigidbody2D myRigid = GetComponent<Rigidbody2D>(); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
