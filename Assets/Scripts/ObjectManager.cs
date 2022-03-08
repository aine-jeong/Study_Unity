using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyLPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemySPrefab;
    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;
    public GameObject bulletPlayerAPrefab;
    public GameObject bulletPlayerBPrefab;
    public GameObject bulletEnemyAPrefab;
    public GameObject bulletEnemyBPrefab;
    public GameObject bulletFollowerPrefab;

    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;

    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;
    GameObject[] bulletFollower;

    GameObject[] targetPool;
    
    void Awake() {
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[10];

        itemCoin = new GameObject[20];
        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];

        bulletPlayerA = new GameObject[100];
        bulletPlayerB = new GameObject[100];
        bulletEnemyA = new GameObject[100];
        bulletEnemyB = new GameObject[100];
        bulletFollower = new GameObject[100];

        Generate();
    }

    void Generate() {
        //Enemy
        for(int index = 0; index < enemyL.Length; index++) {
           enemyL[index] = Instantiate(enemyLPrefab);
           enemyL[index].SetActive(false);
        }

        for(int index = 0; index < enemyM.Length; index++) {
           enemyM[index] = Instantiate(enemyMPrefab);
           enemyM[index].SetActive(false);
        }

        for(int index = 0; index < enemyS.Length; index++) {
           enemyS[index] = Instantiate(enemySPrefab);
           enemyS[index].SetActive(false);
        }

        //Item
        for(int index = 0; index < itemCoin.Length; index++) {
           itemCoin[index] = Instantiate(itemCoinPrefab);
           itemCoin[index].SetActive(false);
        }

        for(int index = 0; index < itemPower.Length; index++) {
           itemPower[index] = Instantiate(itemPowerPrefab);
           itemPower[index].SetActive(false);
        }

        for(int index = 0; index < itemBoom.Length; index++) {
           itemBoom[index] = Instantiate(itemBoomPrefab);
           itemBoom[index].SetActive(false);
        }

        //bulletPlayer
        for(int index = 0; index < bulletPlayerA.Length; index++) {
           bulletPlayerA[index] = Instantiate(bulletPlayerAPrefab);
           bulletPlayerA[index].SetActive(false);
        }

        for(int index = 0; index < bulletPlayerB.Length; index++) {
           bulletPlayerB[index] = Instantiate(bulletPlayerBPrefab);
           bulletPlayerB[index].SetActive(false);
        }

        //bulletEnemy
        for(int index = 0; index < bulletEnemyA.Length; index++) {
           bulletEnemyA[index] = Instantiate(bulletEnemyAPrefab);
           bulletEnemyA[index].SetActive(false);
        }

        for(int index = 0; index < bulletEnemyB.Length; index++) {
           bulletEnemyB[index] = Instantiate(bulletEnemyBPrefab);
           bulletEnemyB[index].SetActive(false);
        }

        //bulletFollower
        for(int index = 0; index < bulletFollower.Length; index++) {
           bulletFollower[index] = Instantiate(bulletFollowerPrefab);
           bulletFollower[index].SetActive(false);
        }
    }

    public GameObject MakeObj(string type) {
        switch (type) {
            case "enemyL":
                targetPool = enemyL;
                break;
            case "enemyM":
                targetPool = enemyM;
                break;
            case "enemyS":
                targetPool = enemyS;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
            case "itemPower":
                targetPool = itemPower;
                break;
            case "itemBoom":
                targetPool = itemBoom;
                break;
            case "bulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "bulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "bulletEnemyA":   
                targetPool = bulletEnemyA;
                break;
            case "bulletEnemyB":
                targetPool = bulletEnemyB;
                break;
            case "bulletFollower":
                targetPool = bulletFollower;
                break;
        }

        for(int index = 0; index < targetPool.Length; index++) {
            if (!targetPool[index].activeSelf) {
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }

        return null; 
    }

    public GameObject[] GetPool(string type) {
        switch (type) {
            case "enemyL":
                targetPool = enemyL;
                break;
            case "enemyM":
                targetPool = enemyM;
                break;
            case "enemyS":
                targetPool = enemyS;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
            case "itemPower":
                targetPool = itemPower;
                break;
            case "itemBoom":
                targetPool = itemBoom;
                break;
            case "bulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "bulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "bulletEnemyA":   
                targetPool = bulletEnemyA;
                break;
            case "bulletEnemyB":
                targetPool = bulletEnemyB;
                break;
            case "bulletFollower":
                targetPool = bulletFollower;
                break;
        }
        
        return targetPool;
    }
}
