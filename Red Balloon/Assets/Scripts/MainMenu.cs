using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //LoadSceneManager.~~
    }
    
    public void QuitGame()
    {
        //Application.Quit();
        Debug.Log("Quit");
    }


    public GameObject optionMenu;
    public GameObject mainMenu;
    
    public void MainToOption()
    {
        optionMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void OptionToMain()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
    }
}