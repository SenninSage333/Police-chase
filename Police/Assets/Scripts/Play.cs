using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public Text text;
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Win")
        {
            text = GameObject.Find("Time").GetComponent<Text>();
            text.text = (PlayerPrefs.GetFloat("time") / 60f).ToString("0.00") + " minutes!";
        }

        if (SceneManager.GetActiveScene().name == "Restart")
        {
            text = GameObject.Find("Message").GetComponent<Text>();
            text.text = PlayerPrefs.GetString("message");
        }
    }
    public void Go()
    {
        SceneManager.LoadScene("Game");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Start");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
