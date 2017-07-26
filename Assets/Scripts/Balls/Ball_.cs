using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_ : MonoBehaviour {

    void OnCollisionEnter(Collision coll)
    {
        GameObject target = coll.gameObject;

        Vector3 inNormal = Vector3.Normalize(transform.position - target.transform.position);
        Vector3 bounceVector = Vector3.Reflect(coll.relativeVelocity, inNormal);

        // 알아보기 쉽게 하기 위해 force값을 곱해 충돌시 속도를 20%로 줄였습니다. 
        // 충돌했던 제 속도를 그대로 내시려면 수식에서 force 곱해주는 부분을 빼면 됩니다. 
        target.GetComponent<Rigidbody>().AddForce(bounceVector , ForceMode.VelocityChange);
    }
}
