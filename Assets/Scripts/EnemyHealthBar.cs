using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = enemyHealth.transform.position + offset;
        float fillAmount = enemyHealth.currentHealth / enemyHealth.GetStartingHealth();
        healthBarFill.fillAmount = fillAmount;
    }
}
