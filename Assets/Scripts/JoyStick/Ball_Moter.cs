﻿//공을 움직이는 코드
using UnityEngine;
using System.Collections;

public class Ball_Moter : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float drag = 0.5f;
    public Vector3 MoveVector { set; get; }
    public VirtualJoystick joystick;
    public bool onBlueEffect;

    private Rigidbody2D thisRigidbody;

    void Start()
    {
        thisRigidbody = gameObject.AddComponent<Rigidbody2D>();
        thisRigidbody = GetComponent<Rigidbody2D>();
        thisRigidbody.drag = drag;
    }

    private void Update()
    {
        MoveVector = PoolInput();

        Move();
    }

    private void Move()
    {
        thisRigidbody.velocity = MoveVector * moveSpeed;
    }

    private Vector3 PoolInput()
    {
        Vector3 dir = Vector3.zero;

        //dir.x= Input.GetAxis("Horizontal");
        //dir.z= Input.GetAxis("vertical");

        dir.x = joystick.Horizontal();
        dir.y = joystick.Vertical();

        if (dir.magnitude > 1)
            dir.Normalize();

        return dir;
    }
}