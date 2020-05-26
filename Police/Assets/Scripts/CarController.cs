using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CarController : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] private GameObject[] WheelMeshes = new GameObject[2];
    [SerializeField] private Skid[] skid = new Skid[2];
    [SerializeField] public float topSpeed = 200; //maksymalna predkosc samochodu
    [SerializeField] public float speedDownLimit = 0;
    [SerializeField] public int noOfGears = 5; //ilosc biegow
    [SerializeField] private static float revsRange = 1f; //zakres obrotow silnika na jednym biegu (od 0)
    [SerializeField] public int maxHealth = 100;
    public float currentSpeed { get { return rb.velocity.z * 3.6f; } }
    private int gearNum = 0; //aktualny bieg
    private float gearFactor;
    public float revs { get; private set; } //obroty silnika
    public float accel { get; private set; }
    public bool breaking { get; private set; }
    public float distanceTravelled = 0;
    public Vector3 lastPosition;
    public HealthBar healthBar;
    public int health;


    void Start()
    {
        breaking = false;
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, speedDownLimit / 3.6f);
        lastPosition = transform.position;
        health = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        distanceTravelled = 0;
    }

    void Update()
    {
        distanceTravelled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        if (health <= 0)
        {
            SceneManager.LoadScene("Restart");
        }
    }

    public void Drive(float v, float b, float h, float vr, float hr)
    {
        accel = Mathf.Clamp(vr, 0, 1);
        rb.velocity = new Vector3(rb.velocity.x + h, 0, rb.velocity.z);
        if (h == 0)
        {
            rb.velocity = new Vector3(0, 0, rb.velocity.z);
        }
        if (b < 0 && rb.velocity.z * 3.6f > speedDownLimit + 1)
        {
            rb.AddForce(new Vector3(0, 0, b));
            rb.rotation = Quaternion.Euler(vr * -2, hr * 3, hr * 5);
        }
        else if (b > 0)
        {
            rb.rotation = Quaternion.Euler(vr * -2, hr * 3, hr * 5);
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z + v);
        }
        else
        {
            rb.rotation = Quaternion.Euler(0, hr * 3, hr * 5);
        }

        CapSpeed();
        CalculateRevs();
        GearChanging();
        Break(vr);
    }

    private void Break(float br)
    {
        if (br < 0 && !skid[0].PlayingAudio && rb.velocity.z * 3.6f > speedDownLimit + 1)
        {
            for (int i = 0; i < 2; i++)
            {
                skid[i].PlayAudio();
                breaking = true;
            }
        }
        else if (br >= 0 || rb.velocity.z * 3.6f <= speedDownLimit + 1)
        {
            for (int i = 0; i < 2; i++)
            {
                skid[i].StopAudio();
                breaking = false;
            }
        }
    }

    private void CapSpeed()
    {
        float speed = rb.velocity.z;
        speed *= 3.6f;
        if (speed > topSpeed)
        {
            rb.velocity = (topSpeed / 3.6f) * rb.velocity.normalized;
        }
        if (speed <= speedDownLimit)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, speedDownLimit / 3.6f);
        }
    }

    private void GearChanging()
    {
        float f = Mathf.Abs(currentSpeed / topSpeed);
        float upgearlimit = (1 / (float)noOfGears) * (gearNum + 1);
        float downgearlimit = (1 / (float)noOfGears) * gearNum;

        if (gearNum > 0 && f < downgearlimit)
        {
            gearNum--;
        }

        if (f > upgearlimit && (gearNum < (noOfGears - 1)))
        {
            gearNum++;
        }
    }

    private void CalculateGearFactor()
    {
        float f = (1 / (float)noOfGears);
        var targetGearFactor = Mathf.InverseLerp(f * gearNum, f * (gearNum + 1), Mathf.Abs(currentSpeed / topSpeed));
        gearFactor = Mathf.Lerp(gearFactor, targetGearFactor, Time.deltaTime * 5f);
    }

    private void CalculateRevs()
    {
        CalculateGearFactor();
        var gearNumFactor = gearNum / (float)noOfGears;
        var revsRangeMin = ULerp(0f, revsRange, CurveFactor(gearNumFactor));
        var revsRangeMax = ULerp(revsRange, 1f, gearNumFactor);
        revs = ULerp(revsRangeMin, revsRangeMax, gearFactor);
    }

    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }

    private static float CurveFactor(float factor)
    {
        return 1 - (1 - factor) * (1 - factor);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Robber")
        {
            SceneManager.LoadScene("Win");
        }
        else
        {
            health -= 5;
            healthBar.setHealth(health);
        }
    }
}
