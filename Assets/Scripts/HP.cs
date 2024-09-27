using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    private Health playerHealth;

    private void Awake()
    {
       
        playerHealth = FindObjectOfType<Health>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player") && playerHealth != null)
        {
            
            playerHealth.Heal(10);
            Debug.Log("Player healed by 10 HP!");

            
            Destroy(gameObject);
        }
    }
}
