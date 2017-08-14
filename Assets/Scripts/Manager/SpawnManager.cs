using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    private Transform ballsStorage;

    public Sprite redBarSprite;

    public Ball[] balls; //공들 오브젝트 (기본공/ 특수공으로 구별해서 넣을거임)
    public Transform player;
    public float spawnDelay = 3;
    public float spawnViewportMargin;

    bool isSpawn = false; //공이 스폰 되었는지 구별 (flase=안됨/true=스폰됨)

    void Start () {
        ballsStorage = new GameObject("BallStorage").transform;
    }
	
	void Update () {
        Spawn();
	}

    void Spawn()
    {
        if (isSpawn) return;// 1. 스폰되었으면 리턴시켜서 코루틴을 실행 못하게함.
        StartCoroutine(BallSpawn());
    }

    IEnumerator BallSpawn() 
    {
        isSpawn = true;//2.트루로 바꿔주면서 스폰이 완료 될때까지 실행 못하게 함. (1번 참조)
        WaitForSeconds spawnDelay = new WaitForSeconds(this.spawnDelay);//2-1.스폰 딜레이 만큼 기다림.

        yield return spawnDelay;
        int randomBall = Random.Range(0, balls.Length);//0부터 balls배열의 끝번호 까지 랜덤으로 돌림.
        Ball ball = balls[randomBall];//ball변수에 랜던으로 돌려 확정된 공을 집어 넣음.

        Ball ballObject = Instantiate(ball).GetComponent<Ball>();//선택된 공을 인스탠트 시킴.
        ballObject.transform.position = GetRandomSpawnPoint();
        ballObject.inViewport = false;//공의 포지션을 플레이어 포지션으로 이동시킴.(초기화)
        ballObject.transform.SetParent(ballsStorage);//공의 부모를 ballsStorage로 함.
        ballObject._player = player;//볼 오브젝트.플레이어랑 플레이어랑 햇갈리지 말것.(위에 선언한 플레이어를 확인할 것)
        ballObject.redBar = redBarSprite;
        isSpawn = false;//3.펄스로 바꾸면서 코루틴 종료.
    }

    #region Random point

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 result;
        float x, y;
        float randomAxis = GetRandomAxis();
        float randomPosition = GetRandomPosition();

        randomAxis += 0.5f;
        randomPosition += 0.5f;

        RandomInit(randomAxis, randomPosition, out x, out y);

        result = Camera.main.ViewportToWorldPoint(new Vector2(x, y));
        result.z = 0;
        return result;
    }

    private void RandomInit(float value1, float value2, out float a, out float b)
    {
        if (Random.Range(0, 2) == 0)
        {
            float temp = value1;
            value1 = value2;
            value2 = temp;
        }

        a = value1;
        b = value2;
    }

    private float GetRandomAxis()
    {
        float result = spawnViewportMargin;
        if (Random.Range(0, 2) == 0)
        {
            result *= -1;
        }

        return result;
    }

    private float GetRandomPosition()
    {
        float result = Random.Range(0, spawnViewportMargin * 2) - spawnViewportMargin;
        return result;
    }
    #endregion
}
