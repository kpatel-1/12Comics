using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityHealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetCityHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetCity(int health)
    {
        slider.value = health;
    }
}
