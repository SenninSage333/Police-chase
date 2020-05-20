using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public GameObject[] tilePrefabs;

    private Transform playerTransform;
    private float spawnZ = -64.0f;
    private float tileLength = 64.0f;
    private int amountOfTiles = 3;

    private float safeZone = 80.0f;
    private float distance;

    private int LastPrefab = 0;

    private bool choose = false;
    int previous = 500;


    private List<GameObject> activeTiles;

    // Start is called before the first frame update
    void Start()
    {
        activeTiles = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        distance = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>().distanceTravelled;

        for (int i=0; i < amountOfTiles; i++)
        {
            SpawnTile();
        }
    }

    // Update is called once per frame
    void Update()
    {
        distance = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>().distanceTravelled;

        if (distance > previous)
        {
            choose = !choose;
            previous += 500;
        }
        // create new road prefab if player position 
        if ( playerTransform.position.z - safeZone > (spawnZ - amountOfTiles * tileLength))
        {
            SpawnTile();
            DeleteTile();
        }
    }

    // Function to create a new road prefab
    void SpawnTile(int prefabIndex = -1)
	{
        // init object
        GameObject go;
        // choose random one from prefab list and make copy of it
        go = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
        // set Parent to the copy 
        go.transform.SetParent(transform);
        // move forward new prefab
        go.transform.position = Vector3.forward * spawnZ;
        // increase next spawn point by length of our road (30)
        spawnZ += tileLength;
        activeTiles.Add(go);
	}

    // Function to delete tile behind player
    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    int RandomPrefabIndex()
    {
        if (tilePrefabs.Length <= 1)
            return 0;

        int randomIndex = LastPrefab;

        while (randomIndex == LastPrefab)
        {
            if (choose)
            {
                randomIndex = Random.Range(3, tilePrefabs.Length);
            } else
            {
                randomIndex = Random.Range(0, 4);
            }
        }

        LastPrefab = randomIndex;
        return randomIndex;
    }
}
