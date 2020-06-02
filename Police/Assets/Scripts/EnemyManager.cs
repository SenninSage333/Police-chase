using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyPrefab; // lista modeli innych samochodow
    private Transform playerTransform; // pozycja gracza
    public List<GameObject>[] activeEnemies; // lista aktywnych modeli innych samochodow na scenie
    private float spawnZ;// odleglosc od gracza w ktorej pojawia sie modele na scenie
    private int activeEnemiesCount; // ilosc aktywnych modeli innych samochodow na scenie, zmienna pomocnicza
    private float time;
    private float previousTime;
    private GameObject robber;

    private void Awake()
    {
        activeEnemies = new List<GameObject>[4];
        for (int i = 0; i < 4; i++)
        {
            activeEnemies[i] = new List<GameObject>();
        }
    }
    void Start()
    {
        robber = GameObject.FindGameObjectWithTag("Robber").gameObject;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        spawnZ = Random.Range(7, 10);
        SpawnEnemy();
        spawnZ += Random.Range(20, 30);
        SpawnEnemy();
        spawnZ += Random.Range(20, 30);
        SpawnEnemy();
    }

    void Update()
    {
        spawnZ = playerTransform.position.z + (int)Random.Range(170, 220);
        if (spawnZ <= robber.transform.position.z + 10f && spawnZ >= robber.transform.position.z - 10f)
        {
            spawnZ += 30f;
        }
        if (Time.time >= previousTime + time && activeEnemiesCount <= 10)
        {
            SpawnEnemy();
        }
        for (int i = 0; i < activeEnemies.Length; i++)
        {
            if (activeEnemies[i].Count == 0)
            {
                continue;
            }
            for (int j = 0; j < activeEnemies[i].Count; j++)
            {
                if (playerTransform.position.z - 20f > (activeEnemies[i][j].transform.position.z))
                {
                    DeleteEnemy(activeEnemies[i][j], i);
                }
                else if (playerTransform.position.z + 500f < (activeEnemies[i][j].transform.position.z))
                {
                    DeleteEnemy(activeEnemies[i][j], i);
                }
            }
        }
    }

    void SpawnEnemy()
    {
        HashSet<int> h = new HashSet<int>();
        int c = Random.Range(1, 3);
        int[] tracks = new int[c];
        while (h.Count < c)
        {
            h.Add(Random.Range(0, 4));
        }
        h.CopyTo(tracks);
        for (int i = 0; i < c; i++)
        {
            float posX = PositionX(tracks[i]);
            GameObject enemy;
            enemy = Instantiate(enemyPrefab[Random.Range(0, 7)]) as GameObject;
            enemy.transform.position = new Vector3(posX, enemy.transform.position.y, spawnZ);
            switch (posX)
            {
                case -1.99f: activeEnemies[0].Add(enemy); break;
                case 2.23f: activeEnemies[1].Add(enemy); break;
                case 6.49f: activeEnemies[2].Add(enemy); break;
                case 10.7f: activeEnemies[3].Add(enemy); break;
            }
            activeEnemiesCount++;
        }
        previousTime = Time.time;
        time = Random.Range(2, 4);
    }

    void DeleteEnemy(GameObject enemy, int tab)
    {
        Destroy(enemy);
        activeEnemies[tab].Remove(enemy);
        activeEnemiesCount--;
    }

    float PositionX(int track)
    {
        float result = -1.99f;
        switch (track)
        {
            case 0: result = -1.99f; break;
            case 1: result = 2.23f; break;
            case 2: result = 6.49f; break;
            case 3: result = 10.7f; break;
        }
        return result;
    }
}
