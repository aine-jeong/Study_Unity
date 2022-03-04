using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    public Sprite[] sprites;

    public string enemyName;
    public int enemyScore;

    public GameObject player;
    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public float maxShotDelay;
    public float curShotDelay;

    public GameObject itemCoin;
    public GameObject itemBoom;
    public GameObject itemPower;

    public ObjectManager objectManager;

    SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable() { 
        //OnEnable(): 컴포넌트가 활성화될 때 호출되는 생명주기 함수
        
        switch (enemyName) {
            case "L":
                health = 20;
                break;
            case "M":
                health = 10;
                break;
            case "S":
                health = 3;
                break;
        }
    }

    void Update()
    {
       Fire();
       Reload();
    }

    //슈팅
    void Fire() {
        if(curShotDelay < maxShotDelay)
            return;

        if(enemyName == "S") {
            GameObject bullet = objectManager.MakeObj("bulletEnemyA");
            bullet.transform.position = transform.position;
            //= Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            //적의 총알이 플레이어의 방향으로 움직여야 한다.
            //목표물로의 방향 = 플레이어의 위치 - 자신의 위치
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }else if(enemyName == "L") {
            GameObject bulletR = objectManager.MakeObj("bulletEnemyB");
            bulletR.transform.position = transform.position + Vector3.right * 0.3f;
            //= Instantiate(bulletObjB, transform.position + Vector3.right *0.3f, transform.rotation);
            GameObject bulletL = objectManager.MakeObj("bulletEnemyB");
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;
            //= Instantiate(bulletObjB, transform.position + Vector3.left *0.3f, transform.rotation);
            
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            //적의 총알이 플레이어의 방향으로 움직여야 한다.
            //목표물로의 방향 = 플레이어의 위치 - 자신의 위치
            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right *0.3f);
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.right *0.3f);

            rigidR.AddForce(dirVecR.normalized * 7, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 7, ForceMode2D.Impulse);

        }

        curShotDelay = 0;
    }

    //장전
    void Reload(){
        curShotDelay += Time.deltaTime;
    }


    //플레이어의 총알에 맞았을 때
    public void OnHit(int dmg) {

        if (health <= 0)
            return;

        health -= dmg;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if(health <= 0) {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            //Random Ratio Item Drop
            int ran = Random.Range(0, 10);
            if(ran < 3) { //Not Item
                Debug.Log("Not Item");
            }else if (ran < 6) { //Coin
                GameObject itemCoin = objectManager.MakeObj("itemCoin");
                itemCoin.transform.position = transform.position;
                //Instantiate(itemCoin, transform.position, itemCoin.transform.rotation);
            }else if (ran < 8) { //Power
                GameObject itemPower = objectManager.MakeObj("itemPower");
                itemPower.transform.position = transform.position;
                //Instantiate(itemPower, transform.position, itemCoin.transform.rotation);
            }else if (ran < 10) { //Boom
                GameObject itemBoom = objectManager.MakeObj("itemBoom");
                itemBoom.transform.position = transform.position;
                //Instantiate(itemBoom, transform.position, itemCoin.transform.rotation);
            }

            gameObject.SetActive(false);
            //Quaternion.identity: 기본 회전값 = 0
            transform.rotation = Quaternion.identity;
        }
    }

    void ReturnSprite() {
        spriteRenderer.sprite = sprites[0];
    }
    
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "BorderBullet") {
            //화면 밖으로 사라졌을 때
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
        else if(collision.gameObject.tag == "PlayerBullet") {
            //플레이어에게 피격당했을 때
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);

            collision.gameObject.SetActive(false);
        }
    }
}
