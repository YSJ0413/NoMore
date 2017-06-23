using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    private Transform ballsStorage;

    public Ball[] balls;



    public float spawnViewportMargin;

    // Use this for initialization
    void Start () {
        ballsStorage = new GameObject("BallStorage").transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Spawn()
    {
        int randomBall = Random.Range(0, balls.Length);
        Ball ball = balls[randomBall];

        Ball ballObject = Instantiate(ball).GetComponent<Ball>();
    }

}
