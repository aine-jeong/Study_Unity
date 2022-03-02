using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isTouchTop;
    public bool isTouchButtom;
    public bool isTouchRight;
    public bool isTouchLeft;

    public int life;
    public int score;

    public float speed;
    public int maxPower;
    public int power;
    public int maxBoom;
    public int boom;
    public float maxShotDelay;
    public float curShotDelay;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject boomEffect;

    public GameManager gameManager;
    public ObjectManager objectManager;
    public bool isHit;
    public bool isBoomTime;

    Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
    }
    

    void Update()
    {
       Move();
       Fire();
       Boom();
       Reload();
    }

    void Move() {
         float h = Input.GetAxisRaw("Horizontal");
        if((isTouchRight && h == 1) || (isTouchLeft && h == -1))
            h = 0;
        float v = Input.GetAxisRaw("Vertical");
        if((isTouchTop && v == 1) || (isTouchButtom && v == -1))
            v = 0;
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        // 애니메이션 적용
        if(Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal")) {
            anim.SetInteger("Input", (int)h);
        }
    }

    //슈팅
    void Fire() {
        if(!Input.GetButton("Fire1"))
            return;

        if(curShotDelay < maxShotDelay)
            return;

        switch(power) {
            case 1:
                //Power 1
                GameObject bullet = objectManager.MakeObj("bulletPlayerA");
                bullet.transform.position = transform.position;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                //Power 2
                GameObject bulletR = objectManager.MakeObj("bulletPlayerA");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;

                GameObject bulletL = objectManager.MakeObj("bulletPlayerA");
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 3:
                //Power 3
                GameObject bulletRR = objectManager.MakeObj("bulletPlayerA");
                bulletRR.transform.position = transform.position + Vector3.right * 0.35f;

                GameObject bulletCC = objectManager.MakeObj("bulletPlayerB");
                //= Instantiate(bulletObjB, transform.position, transform.rotation);
                bulletCC.transform.position = transform.position;

                GameObject bulletLL = objectManager.MakeObj("bulletPlayerA"); 
               // = Instantiate(bulletObjA, transform.position + Vector3.left * 0.35f, transform.rotation);
                bulletLL.transform.position = transform.position + Vector3.left * 0.35f;
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }

        curShotDelay = 0;
    }

    //장전
    void Reload(){
        curShotDelay += Time.deltaTime;
    }

    void Boom() {
        if(!Input.GetButton("Fire2"))
            return;
        
        if(isBoomTime)
            return;
        
        if(boom == 0)
            return;
        
        boom--;
        isBoomTime = true;
        gameManager.UpdateBoomIcon(boom);

         //1. Effect visible
        boomEffect.SetActive(true);
        //시간차로 boomEffect 끄기
        Invoke("OffBoomEffect", 2f);

        //2. Remove Enemy
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //오브젝트를 직접 찾는 Find계열 함수 -> 성능 부하 유발
        GameObject[] enemiesL = objectManager.GetPool("enemyL");
        GameObject[] enemiesM = objectManager.GetPool("enemyM");
        GameObject[] enemiesS = objectManager.GetPool("enemyS");

        for(int index = 0; index < enemiesL.Length; index++) {
            if(enemiesL[index].activeSelf) {
                //로직 가져오기
                Enemy enemyLogic = enemiesL[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for(int index = 0; index < enemiesM.Length; index++) {
            if(enemiesM[index].activeSelf) {
                //로직 가져오기
                Enemy enemyLogic = enemiesM[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for(int index = 0; index < enemiesS.Length; index++) {
            if(enemiesS[index].activeSelf) {
                //로직 가져오기
                Enemy enemyLogic = enemiesS[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        //3. Remove Enemy Bullet
        GameObject[] bulletsA = objectManager.GetPool("bulletEnemyA");
        GameObject[] bulletsB = objectManager.GetPool("bulletEnemyB");

        for(int index = 0; index < bulletsA.Length; index++) {
            if (bulletsA[index].activeSelf) {
            //Destroy(bullets[index]);
                bulletsA[index].SetActive(false);
            }
        }

        for(int index = 0; index < bulletsB.Length; index++) {
            if (bulletsB[index].activeSelf) {
            //Destroy(bullets[index]);
                bulletsB[index].SetActive(false);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Border") {
            switch (collision.gameObject.name) {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchButtom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                
            }
        }

        else if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet") {

            if(isHit)
                return;
                
            isHit = true;
            life--;
            gameManager.UpdateLifeIcon(life);

            if(life == 0) {
                gameManager.GameOver();
            }else {
                gameManager.RespawnPlayer();
            }

            gameObject.SetActive(false);
            //collision.gameObject.SetActive(false);
            
        }

        else if(collision.gameObject.tag == "Item") {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type) {
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if(power == maxPower)
                        score += 500;
                    else
                        power++;
                    break;
                case "Boom":
                    if(boom == maxBoom)
                        score += 500;
                    else {
                        boom++;
                        gameManager.UpdateBoomIcon(boom);
                    }
                    break;
            }
            collision.gameObject.SetActive(false);

        }
    }

    void OffBoomEffect() {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "Border") {
            switch (collision.gameObject.name) {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchButtom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
                
            }
        }
    }
}
