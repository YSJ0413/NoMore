using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    Rigidbody2D myrigid;

    Ball_Moter playerScript;

    public Sprite redBar;
    public Transform _player;
    public float moveSpeed = 10;
    public int bounceCount = 5;
    public bool inViewport = false;
    public float redBallSpellDelay = 0.5f;

    private bool blueBallSpell_sw = false;

    Vector3 direction;
    
    bool canMove;

	void Start () {
        myrigid = GetComponent<Rigidbody2D>();

        StartCoroutine(StartDelay());
	}
	
	void Update () {

        Move();
	}

    void SetDestination()
    {
        direction = _player.position - transform.position;
        direction.Normalize();

        while(direction.x == 0 && direction.y == 0)
        {
            direction.x += Random.Range(1f, -1f);
            direction.y += Random.Range(1f, -1f);

            if (direction.x != 0 && direction.y != 0)
                break;
        }
    }

    void LookatPlayer()
    {
        SetDestination();
        Vector3 direction_c = direction;
        float angle = Mathf.Atan2(direction_c.y, direction_c.x) * Mathf.Rad2Deg;

        Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = newRotation;

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
        CircleCollider2D myCollider = GetComponent<CircleCollider2D>();
        yield return new WaitForSeconds(0.5f);
        inViewport = true;
}

    void OnTriggerEnter2D(Collider2D hit)
    {
        if (!inViewport) return;

        if(blueBallSpell_sw)
        {
            return;
        }

        if (hit.tag == "Player")
        {
            HitPlayer();
        }

        bounceCount--;

        if (bounceCount <= 0)
        {
            NoCount();
        }

        if(hit.tag == "Wall")
            SetDestination();
        else
        {
            direction = new Vector3(0, 0, 0);
            direction = transform.position - hit.transform.position;
            direction.Normalize();
        }
    }

    private void OnTriggerStay2D(Collider2D hit)
    {
        if (!blueBallSpell_sw) return;
        float speedUp = 3f;
        switch (hit.tag)
        {
            case "Red":
            case "Blue":
            case "Brown":
                Ball hitBall = Instantiate(hit).GetComponent<Ball>();
                hitBall.moveSpeed += speedUp;
                break;
            case "Player":
                Ball_Moter hitPlayer = Instantiate(hit).GetComponent<Ball_Moter>();
                hitPlayer.moveSpeed += speedUp;
                break;
            default:
                return;
        }
    }

    void NoCount()
    {
        canMove = false;
        switch(this.tag)
        {
            case "Red":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = redBar;
                LookatPlayer();
                StartCoroutine(RedBallEffect());
                break;
            case "Blue":
                StartCoroutine(BlueBallEffect());
                break;
            case "Brown":
                Destroy(this.gameObject);
                break;
            default:
                Destroy(this.gameObject);
                return;
        }
    }

    void HitPlayer()
    {
        switch (this.tag)
        {
            case "Red":
                Debug.Log("GameOver");
                Destroy(this.gameObject);
                break;
            case "Blue":
                StartCoroutine(BlueBallEffect());
                //Destroy(this.gameObject);
                break;
            case "Brown":
                StartCoroutine(BlownBallEffect());
                //Destroy(this.gameObject);
                break;
            default:
                Debug.Log("GameOver");
                Destroy(this.gameObject);
                return;
        }
    } 

    IEnumerator RedBallEffect()
    {
        yield return new WaitForSeconds(redBallSpellDelay);

        for (int i = 0; i < 10000; i++)
        {
            transform.localScale += new Vector3(1, 0, 0);
            yield return new WaitForSeconds(0.005f);
        }
        //Destroy(this.gameObject);
    }

    IEnumerator BlueBallEffect()
    {
        canMove = false;
        transform.position += new Vector3(0, 0, 10);
        transform.localScale = new Vector3(5, 5, 1);
        blueBallSpell_sw = true;
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

    IEnumerator BlownBallEffect()
    {
        transform.localScale = new Vector3(0, 0, 0);
        //playerScript.HitBlown();
        yield return new WaitForSeconds(3.5f);
        Destroy(this.gameObject);
    }
}
