﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;

    public void SetStamina(float stamina)
    {
        slider.value = stamina;
    }

    public float GetStamina()
    {
        return slider.value;
    }

    public void SetMaxStamina(float stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
    }
}
