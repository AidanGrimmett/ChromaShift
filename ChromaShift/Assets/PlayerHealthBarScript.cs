using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarScript : MonoBehaviour
{
    public GameObject healthBar;
    private HealthBarScript hbs;
    private PlayerMovement pms;

    private float currentHealth;

    public float decayRate;
    public float holoframeAdjustment;
    public float chunkEndAdjustment;

    private float delay;

    private void Start()
    {
        hbs = healthBar.GetComponent<HealthBarScript>();
        pms = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (pms.CurrentState == PlayerMovement.PlayerStates.OnWalls)
        {
            delay = 0.90f;
            return;
        }
        
        delay -= Time.deltaTime;
        if (delay > 0) return;

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
            delay = 0.85f;
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            currentHealth += chunkEndAdjustment;
            delay = 1.5f;
        }
        currentHealth = Mathf.Clamp(currentHealth, hbs.slider.minValue, hbs.slider.maxValue);
    }
}
