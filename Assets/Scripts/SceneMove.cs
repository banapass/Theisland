using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public static SceneMove instance;
    private void Start()
    {
        instance = this;
    }
    public void Title()
    {
        SceneManager.LoadScene("Title");
    }
    public void GameStart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void EndingScene()
    {
        SceneManager.LoadScene("Ending");
    }
    public void AppQuip()
    {
        Application.Quit();
    }
    
}
