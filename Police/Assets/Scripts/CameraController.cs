using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset;
    public GameObject car;

    private void Start()
    {
        car = GameObject.FindGameObjectWithTag("Player");
        offset = new Vector3(0, 10, -10) - new Vector3(0, 0.5f, 0);
        transform.rotation = Quaternion.Euler(33f, 0, 0);
    }
    void LateUpdate()
    {
        transform.position = car.transform.position + offset;
    }
}
