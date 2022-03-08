using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public string[] enemyObjs;
    public Transform[] spawnPoints;

    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;

    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;
    public ObjectManager objectManager;

    //커스텀배치구현-구조체
    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    void Awake() {
        //gameOverSet.SetActive(false);
        enemyObjs = new string[] { "enemyS", "enemyM", "enemyL" };

        spawnList = new List<Spawn>();
        ReadSpawnFile();
    }

    void ReadSpawnFile() {
        //1. 변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        //2. 리스폰파일 읽기
        TextAsset textFile = Resources.Load("Stage0") as TextAsset; //as TextAsset: 검증 (TextAsset이 아니면 null 입력된다.)
        StringReader stringReader = new StringReader(textFile.text);

        while(stringReader != null) {
            string line = stringReader.ReadLine();
            Debug.Log(line);

            if(line == null)
                break;

            //3. 리스폰 데이터 생성
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);

            spawnList.Add(spawnData);
        }

        //텍스트파일 닫기
        stringReader.Close();
        
        //첫번째 스폰 딜레이 적용
        nextSpawnDelay = spawnList[0].delay;
    }
    
    void Update() {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > nextSpawnDelay && !spawnEnd) {
            SpawnEnemy();
            //@@ nextSpawnDelay = Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }

        //UI Score Update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    void SpawnEnemy() {

        //@@ int ranEnemy = Random.Range(0, 3);
        //@@ 기존 적 생성 로직을 구조체를 활용한 로직으로 교체
        int enemyIndex = 0;
        switch(spawnList[spawnIndex].type) {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
        }
        
        //@@ int ranPoint = Random.Range(0, 9);
        int enemyPoint = spawnList[spawnIndex].point;

        //@@ GameObject enemy = objectManager.MakeObj(enemyObjs[ranEnemy]);
        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);

        //@@ enemy.transform.position = spawnPoints[ranPoint].position;
        enemy.transform.position = spawnPoints[enemyPoint].position;


        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();

        //prefeb의 경우 이미 생성되어있는 오브젝트에 접근 불가능하므로,
        //적 생성 직후에 플레이어 변수를 넘겨줘야 한다.
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

        /* @@
        if(ranPoint == 5 || ranPoint == 6) { //Right Spawn
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else if (ranPoint == 7 || ranPoint == 8) { //Left Spawn
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }
        */

        if(enemyPoint == 5 || enemyPoint == 6) { //Right Spawn
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else if (enemyPoint == 7 || enemyPoint == 8) { //Left Spawn
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }

        else { //Front Spawn
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }

        //# 리스폰 인덱스 증가
        spawnIndex++;
        if(spawnIndex == spawnList.Count) {
            spawnEnd = true;
            return;
        }

        //# 다음 리스폰 딜레이 갱신
        nextSpawnDelay = spawnList[spawnIndex].delay;
    }

    public void UpdateLifeIcon(int life) {
        //UI Life Init Disable
        for (int index = 0; index < 3; index++) {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        //UI Life Active
        for(int index = 0; index < life; index++) {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom) {
        //UI Boom Init Disable
        for (int index = 0; index < 3; index++) {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }

        //UI Boom Active
        for(int index = 0; index < boom; index++) {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }
    
    public void RespawnPlayer() {
        Invoke("RespawnPlayerExe", 2f);
    }

    void RespawnPlayerExe() {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void GameOver() {
        gameOverSet.SetActive(true);
    }

    public void GameRetry() {
        SceneManager.LoadScene(0);
    }
}
