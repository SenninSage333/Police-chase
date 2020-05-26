using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public void Go()
    {
        SceneManager.LoadScene("Game");
    }

    public void Menu(){
        SceneManager.LoadScene("Start");
    }
}
