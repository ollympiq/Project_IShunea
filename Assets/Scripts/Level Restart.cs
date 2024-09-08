using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Restart : MonoBehaviour
{
    public void RestartLevel1()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void RestartLevel2()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void RestartLevel3()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void RestartLevel4()
    {
        SceneManager.LoadSceneAsync(4);
    }
    public void RestartLevel5()
    {
        SceneManager.LoadSceneAsync(5);
    }
}
