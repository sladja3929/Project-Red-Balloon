using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool starting = false;
    public void PlayGame()
    {
        if (starting) return;
        starting = true;
        SceneChangeManager.Instance.SetTime(3f, 0f);
        SceneChangeManager.Instance.SetAlpha(0f, 0.7f);
        SceneChangeManager.Instance.StartCoroutine("LoadSceneAsync", "Stage0");
    }
    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public GameObject optionMenu; 
    public GameObject mainMenu;
    
    public void MainToOption()
    {
        optionMenu.SetActive(true);
        mainMenu.SetActive (false);
    }

    public void OptionToMain()
    {
        mainMenu.SetActive   (true);
        optionMenu.SetActive(false);
    }
}
