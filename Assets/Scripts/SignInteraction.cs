using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SignInteraction : MonoBehaviour
{
    public GameObject uiPanel;
    public Text uiText;
    public string signMessage = "This is the message on the sign.";  
    private bool isPlayerNearby = false;

    private void Awake()
    {
        uiPanel.SetActive(false);
    }
    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            OpenSignUI();
        }
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }


    private void OpenSignUI()
    {
        uiPanel.SetActive(true);
        uiText.text = signMessage;
    }
    
    public void CloseSignUI()
    {
        uiPanel.SetActive(false);  
    }

}
