using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarScript : MonoBehaviour
{
    public GameObject healthBar;
    private HealthBarScript hbs;

    private float currentHealth;

    public float decayRate;
    public float holoframeAdjustment;
    public float chunkEndAdjustment;

    private void Start()
    {
        hbs = healthBar.GetComponent<HealthBarScript>();
    }

    private void Update()
    {
        currentHealth += -decayRate * Time.deltaTime;
        hbs.SetHealth(currentHealth);
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public void SetHealth(float health)
    {
        currentHealth = health;
        currentHealth = Mathf.Clamp(currentHealth, hbs.slider.minValue, hbs.slider.maxValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Holoframe"))
        {
            currentHealth += holoframeAdjustment;
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            currentHealth += chunkEndAdjustment;
        }
        currentHealth = Mathf.Clamp(currentHealth, hbs.slider.minValue, hbs.slider.maxValue);
    }
}
