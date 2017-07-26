using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    private Transform ballsStorage;

    public Ball[] balls;
    public Transform player;
    public float spawnDelay = 3;

    bool isSpawn = false;

    // Use this for initialization

    void Start () {
        ballsStorage = new GameObject("BallStorage").transform;
    }
	
	// Update is called once per frame
	void Update () {
        Spawn();
	}

    void Spawn()
    {
        if (isSpawn) return;
        StartCoroutine(BallSpawn());
    }

    IEnumerator BallSpawn()
    {
        isSpawn = true;
        WaitForSeconds spawnDelay = new WaitForSeconds(this.spawnDelay);

        yield return spawnDelay;
        int randomBall = Random.Range(0, balls.Length);
        Ball ball = balls[randomBall];

        Ball ballObject = Instantiate(ball).GetComponent<Ball>();
        ballObject.transform.position = player.position;
        ballObject.transform.SetParent(ballsStorage);
        ballObject.player = player;
        isSpawn = false;
    }

}
