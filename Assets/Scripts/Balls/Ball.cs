using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    Rigidbody2D myrigid;

    CircleCollider2D myColider;

    Ball_Moter playerScript;

    public Sprite redBar;
    public Transform _player;
    public float moveSpeed = 10;
    public int bounceCount = 5;
    public bool inViewport = false;
    public float redBallSpellDelay = 0.5f;

    public bool onBlueEffect;

    public bool b_startBludEffect = false;

    private bool r_redBallSpell_sw = false;

    private bool b_blueBallSpell_sw = false;

    private bool br_BrownBallEffect_sw = false;

    private float b_tempBallSpeed;
    private float b_tempPlayerSpeed;

    private float br_tempPlayerSpeed;

    private CircleCollider2D b_tempBallCollider;
    private CircleCollider2D b_tempPlayerCollider;

    private bool b_resetToSpeeds;

    Vector3 direction;
    
    bool canMove = true;

	void Start () {
        myrigid = GetComponent<Rigidbody2D>();
        myColider = GetComponent<CircleCollider2D>();
        SetDestination();
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
        if (r_redBallSpell_sw) return;

        SetDestination();

        Vector3 direction_c = direction;

        float angle = Mathf.Atan2(direction_c.y, direction_c.x) * Mathf.Rad2Deg;

        Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = newRotation;

        r_redBallSpell_sw = true;

    }

    void Move()
    {
        if (!canMove) return;

        direction.z = 0;

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(0.8f);
        inViewport = true;
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        if (!inViewport) return;
        //(공용) * 파란공이 효과가 터지면 else 쪽으로 넘어감. 
        if (!b_blueBallSpell_sw)
        {
            Ball hitBall = FindObjectOfType<Ball>();

            if (hit.tag == "Blue" && hitBall.b_startBludEffect) return;

            if (hit.tag == "Player")
            {
                HitPlayer();
            }

            bounceCount--;

            if (bounceCount <= 0)
            {
                NoCount();
            }

            if (hit.tag == "Wall")
                SetDestination();

            else
            {
                direction = new Vector3(0, 0, 0);
                direction = transform.position - hit.transform.position;
                direction.Normalize();
            }
        }
        //(파란공 전용) 효과가 터졌을시 
        else
        {
            Ball hitBall = FindObjectOfType<Ball>();

            Ball_Moter hitPlayer = FindObjectOfType<Ball_Moter>();

            float speed = 0.3f;


            if (b_resetToSpeeds)
            {
                hitPlayer.moveSpeed = b_tempPlayerSpeed;

                if (hit == hitBall)
                    hitBall.moveSpeed = b_tempBallSpeed;

                return;
            }

            if (!hitPlayer.onBlueEffect)
            {
                b_tempPlayerSpeed = hitPlayer.moveSpeed;
                hitPlayer.onBlueEffect = true;
            }

            if (hit == hitBall && !hitBall.onBlueEffect)
            {
                b_tempBallSpeed = hitBall.moveSpeed;
                hitBall.onBlueEffect = true;
            }

            switch (hit.tag)
            {
                case "Red":
                case "Blue":
                case "Brown":
                    if (hit == hitBall)
                        hitBall.moveSpeed += speed;
                    break;
                case "Player":
                    hitPlayer.moveSpeed += speed;
                    break;
                default:
                    return;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D hit)
    {
        Ball hitBall = FindObjectOfType<Ball>();

        Ball_Moter hitPlayer = FindObjectOfType<Ball_Moter>();

        if (!b_blueBallSpell_sw) return;

        float speed = 3f;

        if (b_resetToSpeeds)
        {
            hitPlayer.moveSpeed = b_tempPlayerSpeed;

            hitPlayer.onBlueEffect = false;

            if (hit == hitBall)
            {
                hitBall.moveSpeed = b_tempBallSpeed;

                hitBall.onBlueEffect = false;
            }

            return;
        }

        if (hitPlayer.onBlueEffect)
        {
            hitPlayer.moveSpeed = b_tempPlayerSpeed;
            hitPlayer.onBlueEffect = false;
        }

        if (hit == hitBall && hitBall.onBlueEffect)
        {
            hitBall.moveSpeed = b_tempBallSpeed;
            hitBall.onBlueEffect = false;
        }

        if (hitPlayer.moveSpeed == 0)
            Debug.Log("Error: Player's moveSpeed is zero.");
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
                b_startBludEffect = true;
                StartCoroutine(BlueBallEffect());
                break;
            case "Brown":
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
                b_startBludEffect = true;
                bounceCount = 0;
                break;
            case "Brown":
                StartCoroutine(BrownBallEffect());
                break;
            default:
                Debug.Log("GameOver");
                return;
        }
    } 

    IEnumerator RedBallEffect()
    {
        yield return new WaitForSeconds(redBallSpellDelay);

        myColider.enabled = false;

        BoxCollider2D redEffectCollider = GetComponent<BoxCollider2D>();

        redEffectCollider.enabled = true;

        for (int i = 0; i < 80; i++)
        {
            transform.localScale += new Vector3(1, 0, 0);
            yield return new WaitForSeconds(0.005f);
        }

        Destroy(this.gameObject);
    }

    IEnumerator BlueBallEffect()
    {
        canMove = false;

        myColider.radius = 0.7f;

        transform.position += new Vector3(0, 0, 10);

        transform.localScale = new Vector3(5, 5, 1);

        b_blueBallSpell_sw = true;

        yield return new WaitForSeconds(5f);

        transform.localScale = new Vector3(0, 0, 0);

        myColider.radius = 0f;

        b_resetToSpeeds = true;

        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }

    IEnumerator BrownBallEffect()
    {

        if (br_BrownBallEffect_sw) yield break;

        br_BrownBallEffect_sw = true;

        Ball_Moter hitPlayer = FindObjectOfType<Ball_Moter>();

        float speed = 2f;

        transform.localScale = new Vector3(0, 0, 0);

        br_tempPlayerSpeed = hitPlayer.moveSpeed;

        hitPlayer.moveSpeed -= speed;

        myColider.radius = 0f;
        yield return new WaitForSeconds(0.005f);

        Debug.Log("Speed Down");

        yield return new WaitForSeconds(3.5f);

        hitPlayer.moveSpeed = br_tempPlayerSpeed;

        Debug.Log("Speed return");

        yield return new WaitForSeconds(5f);

        Destroy(this.gameObject);
        
    }
}
