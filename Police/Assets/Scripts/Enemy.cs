using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody rb;
    List<GameObject>[] activeEnemies; // lista wszystkich torów, pobierana z EnemyManager
    private List<GameObject> myTrack; // lista wszystkich modeli na torze po którym porusza się model
    public float speed; // prędkość modelu, randomowa
    private float track; // numer toru
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        activeEnemies = GameObject.Find("EnemyManager").GetComponent<EnemyManager>().activeEnemies;
        speed = Random.Range(20f, 40f);
        rb.velocity = Vector3.forward * speed;
        track = transform.position.x;
        switch (track)
        {
            case -1.99f: myTrack = activeEnemies[0]; break;
            case 2.23f: myTrack = activeEnemies[1]; break;
            case 6.49f: myTrack = activeEnemies[2]; break;
            case 10.7f: myTrack = activeEnemies[3]; break;
        }
    }

    void Update()
    {
        speed = CapSpeed();
        rb.velocity = Vector3.forward * speed;
    }

    void FixedUpdate()
    {
        Vector3 target = new Vector3(track, rb.position.y, rb.position.z);
        Vector3 toTarget = target - rb.position;
        //rb.MovePosition(target);
    }

    private float CapSpeed()
    {
        int enemyInFront = myTrack.IndexOf(this.gameObject);
        for (int i = 0; i < myTrack.Count; i++)
        {
            if (myTrack[i].transform.position.z - transform.position.z <= 0) continue;
            if (transform.position.z + 15f > myTrack[i].transform.position.z)
            {
                enemyInFront = i;
                break;
            }
        }
        if (enemyInFront != myTrack.IndexOf(this.gameObject))
        {
            return myTrack[enemyInFront].GetComponent<Enemy>().speed;
        }
        else
        {
            return speed;
        }
    }
}
