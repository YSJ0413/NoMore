using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Vector3 touchPos;

    public float moveSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    private void Move()
    {
        

        if (!Input.GetMouseButton(0)) return;
        if (!CanMove()) return;

        touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = touchPos - transform.position;
        direction = direction.normalized;// direction.Normalize();//Vector의 길이 => 무조건 1로 만듦
        direction.z = 0;

        Debug.Log(direction + " : " + moveSpeed);

        //transform.Translate(direction * moveSpeed *Time.deltaTime);

        Vector3 movePos = transform.position + (direction * moveSpeed * Time.deltaTime);
        movePos.z = 0; 
        transform.position = movePos;
    }

    private void Rotate()
    {

        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotateSpeed);
    }

    private bool CanMove()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.back);

        Debug.Log(hit.collider);

        if (hit.collider == null) return true;

        return !hit.collider.CompareTag("Player"); //Player태그가 붙어있는 컬라이더와 충돌했으면 false05 반환
    }

    /* 
    private Vector3 CustomNomalize(Vector3 origin) 
    {
        if (origin.sqrMagnitude >= 1)
            
        return Vector3.zero;
    }
    */
}
