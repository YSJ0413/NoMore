using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    public Wall wall;

    private Wall[] wallObj = new Wall[4];

    void Start () {
        for (int i = 0; i < wallObj.Length; i++)
        {
            wallObj[i] = Instantiate(wall).GetComponent<Wall>();

            if (i >= 0 && i <= 1)
                wallObj[i].transform.localScale = new Vector3(20, 1, 1);
            else if(i >= 2 && i <= 3)
                wallObj[i].transform.localScale = new Vector3(1, 10, 1);
            switch(i)
            {
                case 0:
                    wallObj[i].transform.position = new Vector3(0, 5.5f, 0);
                    break;
                case 1:
                    wallObj[i].transform.position = new Vector3(0, -5.5f, 0);
                    break;
                case 2:
                    wallObj[i].transform.position = new Vector3(9.4f, 0, 0);
                    break;
                case 3:
                    wallObj[i].transform.position = new Vector3(-9.4f, 0, 0);
                    break;
                default:
                    return;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
