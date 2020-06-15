using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public static int health;
    public static int maxHealth;

    public HealthBar healthBar;

    Text HealthText;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = PlayerStats.healthLevel;
        health = maxHealth;
        HealthText = GetComponent<Text>();

        healthBar.SetMaxHealth(maxHealth);


    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetHealth(health);
        HealthText.text = "HEALTH: " + health + " / " + maxHealth;

        if (health >= maxHealth)
        {
            health = maxHealth;
        }
      
    }

    
}
