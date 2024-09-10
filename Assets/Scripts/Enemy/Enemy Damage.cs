using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] private float damageInterval = 1f;

    private float lastDamageTime;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
            lastDamageTime = Time.time; 
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
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
