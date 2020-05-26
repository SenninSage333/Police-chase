using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robber : MonoBehaviour
{
    List<GameObject>[] enemies;
    private int track = 1;
    private float[] tracks = { -1.99f, 2.23f, 6.49f, 10.7f };
    bool state = true;
    float nextX;
    float speed;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemies = GameObject.Find("EnemyManager").GetComponent<EnemyManager>().activeEnemies;
        speed = Random.Range(40f, 50f);
    }

    void Update()
    {
        rb.velocity = Vector3.forward * speed;
        if (state)
        {
            Check();
        }
        else
        {
            Avoid();
        }
    }

    void Check()
    {
        for (int i = 0; i < enemies[track].Count; i++)
        {
            if (enemies[track][i].transform.position.z - transform.position.z <= 0) continue;
            if (transform.position.z + 20f > enemies[track][i].transform.position.z)
            {
                state = false;
                int rnd = Random.Range(0, 4);
                while (rnd == track)
                {
                    rnd = Random.Range(0, 4);
                }
                nextX = tracks[rnd];
                track = rnd;
                break;
            }
        }
    }

    void Avoid()
    {
        if (transform.position.x >= nextX - 0.1f && transform.position.x <= nextX + 0.1f)
        {
            state = true;
        }
        transform.position = Vector3.Slerp(transform.position, new Vector3(nextX, transform.position.y, transform.position.z), 0.05f);
    }
}
