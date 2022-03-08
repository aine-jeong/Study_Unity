using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float maxShotDelay;
    public float curShotDelay;
    public ObjectManager objectManager;

    void Update()
    {
       Follow();
       Fire();
       Reload();
    }

    void Follow() {

    }

    
    //슈팅
    void Fire() {
        if(!Input.GetButton("Fire1"))
            return;

        if(curShotDelay < maxShotDelay)
            return;

         //Power 1
        GameObject bullet = objectManager.MakeObj("bulletFollower");
        bullet.transform.position = transform.position;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        curShotDelay = 0;
    }

    //장전
    void Reload(){
        curShotDelay += Time.deltaTime;
    }


}
