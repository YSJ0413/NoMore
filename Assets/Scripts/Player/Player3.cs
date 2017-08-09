using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3 : MonoBehaviour {

    Vector3 touchPos;
    Vector3 direction = new Vector3(0, 0, 0);
    Vector2 mousePos;
    Vector3 movePos;

    public float moveSpeed;

    private bool onTouch;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
            Rotate();

        if(Input.GetMouseButton(0))
        {
            if (!CanMove()) return;

            Rotate();
            Vector3 movePos = transform.position + (direction * moveSpeed * Time.deltaTime);
            movePos.z = 0;
            transform.position = movePos;
            return;
        }

        if (!CanMove_Down()) return;

        //transform.Translate(direction * moveSpeed *Time.deltaTime);

        movePos = transform.position + (direction * moveSpeed * Time.deltaTime);
        movePos.z = 0;
        transform.position = movePos;
    }

    private void Rotate()
    {
        direction = touchPos - transform.position;
        direction.z = 0;
        direction.Normalize();//Vector의 길이 => 무조건 1로 만듦
    }

    private bool CanMove_Down()
    {

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.back);

        Debug.Log(hit.collider);

        if (hit.collider == null) return true;

        return !hit.collider.CompareTag("Player"); //Player태그가 붙어있는 컬라이더와 충돌했으면 false05 반환
    }

    private bool CanMove()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.back);

        if (hit.collider == null) return true;

        return !hit.collider.CompareTag("Player"); //Player태그가 붙어있는 컬라이더와 충돌했으면 false05 반환
    }
}
