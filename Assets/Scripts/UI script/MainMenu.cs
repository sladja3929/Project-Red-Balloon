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

        StartCoroutine(PlayGameCoroutine());        
    }

    private IEnumerator PlayGameCoroutine()
    {
        FadingInfo startGameFadingInfo = new FadingInfo(1.5f, 0, 1, 0);
        SceneChangeManager.instance.FadeOut(startGameFadingInfo);
        yield return new WaitUntil(() => SceneChangeManager.instance.FinishFade());

        int stage = SaveManager.instance.Stage;
        if (stage == -1)
        {
            stage = 0;
            SaveManager.instance.Stage = stage;
        }
        
        SceneChangeManager.instance.LoadSceneAsync
        ($"Stage{stage}", () => { SceneChangeManager.instance.FadeIn(startGameFadingInfo); });
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public MainOption optionMenu; 
    public GameObject mainMenu;
    
    public void MainToOption()
    {
        optionMenu.OpenPauseMenu();
        
        Debug.Log("MainToOption");
        gameObject.SetActive(false);
    }

    public void OptionToMain()
    {
        gameObject.SetActive(true);
        
        Debug.Log("OptionToMain");
        optionMenu.ClosePauseMenu();
    }
}
