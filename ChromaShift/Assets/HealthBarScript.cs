using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;
    private float currentHealth;
    public Gradient gradient;
    public Image fill;

    private void Start()
    {
        currentHealth = slider.maxValue;
        SetHealth(currentHealth);
    }

    public void SetHealth(float health)
    {
        currentHealth = health;
        slider.value = currentHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void AdjustHealth(float health)
    {
        SetHealth(currentHealth + health);
    }
}
