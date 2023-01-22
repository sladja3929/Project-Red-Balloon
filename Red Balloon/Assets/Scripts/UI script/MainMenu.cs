using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        IEnumerator LoadSceneCoroutine(string target)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(target);
            asyncOperation.allowSceneActivation = false;
        
            while (asyncOperation.progress < 0.9f)
            {
                yield return null;
                Debug.Log(asyncOperation.progress);
            }

            asyncOperation.allowSceneActivation = true;
        }

        StartCoroutine(LoadSceneCoroutine("Stage0"));
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
        mainMenu.SetActive (false);
    }

    public void OptionToMain()
    {
        mainMenu.SetActive   (true);
        optionMenu.SetActive(false);
    }
}
