using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class Health : MonoBehaviour
{
    [SerializeField]private float startingHealth;
    private Animator anim;
    public float currentHealth { get; private set; }
    private bool dead;
    [SerializeField] private float delayBeforeSceneChange = 2f;

    [SerializeField] private TextMeshProUGUI deathMessageText;

    private void Awake()
    {
        deathMessageText.gameObject.SetActive(false);
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage) {

        currentHealth = Mathf.Clamp(currentHealth - _damage,0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
        }
        else {
            if (!dead) {
                anim.SetTrigger("die");
                GetComponent<PlayerMovement>().enabled = false;
                dead = true;
                deathMessageText.text = "YOU DIED";
                deathMessageText.gameObject.SetActive(true);
                StartCoroutine(ChangeSceneAfterDelay());
            }
        }
    }

    IEnumerator ChangeSceneAfterDelay()
    {
      
        yield return new WaitForSeconds(delayBeforeSceneChange);

        
        SceneManager.LoadScene(0);
    }
}
