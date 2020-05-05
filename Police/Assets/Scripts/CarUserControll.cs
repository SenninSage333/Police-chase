using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarUserControll : MonoBehaviour
{
    [SerializeField] public float acc = 1.2f; //szybkosc przyspieszania samochodu
    [SerializeField] public float steer = 2; //szybkosc skrecania samochodu na boki
    [SerializeField] public float breakPower = 10; //sila hamowania
    private CarController car;

    private void Awake()
    {
        car = GetComponent<CarController>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        car.Drive(v*(acc/10), v*breakPower*2000, h*(steer/10), v, h);
    }
}
