using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private Image fill;
    public void setMaxValue(int health){
        slider.maxValue = health;
        slider.value = health;
    }
    public void setHealth(int health){
        slider.value = health;
    }

    private void Start() {
        fill =  GameObject.Find("Fill").GetComponent<Image>();
        slider = GetComponent<Slider>();
    }

    private void Update() {
        if(slider.value >= (slider.maxValue * 2/3)){
            fill.color = new Color(0, 255, 0, 1);
        }
        else if(slider.value >= (slider.maxValue * 1/3) && slider.value < (slider.maxValue * 2/3)){
            fill.color = new Color(255, 128, 0, 1);
        }
        else {
            fill.color = new Color(255, 0, 0, 1);
        }
    }
}
