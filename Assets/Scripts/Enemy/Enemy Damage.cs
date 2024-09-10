using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float damageInterval = 1f;

    private float lastDamageTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
            lastDamageTime = Time.time; 
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
           
            if (Time.time >= lastDamageTime + damageInterval)
            {
                collision.GetComponent<Health>().TakeDamage(damage);
                lastDamageTime = Time.time; 
            }
        }
    }
}
