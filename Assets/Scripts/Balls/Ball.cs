﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    Rigidbody2D myrigid;

    public Transform player;
    public float moveSpeed = 10;
    public int bounceCount = 5;

    Vector3 direction;
    
    bool canMove;

	// Use this for initialization
	void Start () {
        myrigid = GetComponent<Rigidbody2D>();
        CircleCollider2D myCollider = GetComponent<CircleCollider2D>();
        StartCoroutine(StartDelay());
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    void SetDestination()
    {
        direction = player.position - transform.position;
        direction.Normalize();

        while(direction.x == 0 && direction.y == 0)
        {
            direction.x += Random.Range(1f, -1f);
            direction.y += Random.Range(1f, -1f);

            if (direction.x != 0 && direction.y != 0)
                break;
        }
    }

    void Move()
    {
        if (!canMove) return;
        direction.z = 0;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(2f);
        canMove = true;
        SetDestination();
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        if (bounceCount <= 0) Destroy(this.gameObject);
        bounceCount--;
        direction = new Vector3(0, 0, 0);
        direction = transform.position - hit.transform.position;
        direction.Normalize();
    }

    
}
