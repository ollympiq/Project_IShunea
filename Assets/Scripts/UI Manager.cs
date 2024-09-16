using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;



    private void Awake()
    {
        gameOverScreen.SetActive(false);

    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void soundVolume() 
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }
    public void musicVolume() 
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
}
