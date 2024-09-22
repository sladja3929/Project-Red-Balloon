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
        //    경고화면 페이드
        //    SceneChangeManager.Instance.SetTime(1f, 0f);
        //    SceneChangeManager.Instance.SetAlpha(0f, 1f);
        //    yield return SceneChangeManager.Instance.StartCoroutine("Fade", "In");
        //    warning.SetActive(true);

        //    SceneChangeManager.Instance.SetTime(1f, 0.5f);
        //    yield return SceneChangeManager.Instance.StartCoroutine("Fade", "Out");

        //    SceneChangeManager.Instance.SetTime(1f, 3f);
        //    yield return SceneChangeManager.Instance.StartCoroutine("Fade", "In");

        //    SceneChangeManager.Instance.SetTime(3f, 0f);
        //    SceneManager.LoadScene("Stage0");
        
        //yield return SceneChangeManager.Instance.StartCoroutine(nameof(SceneChangeManager.FadeIn));

        FadingInfo startGameFadingInfo = new FadingInfo(1.5f, 0, 1, 0);
        SceneChangeManager.instance.FadeOut(startGameFadingInfo);
        yield return new WaitUntil(() => SceneChangeManager.instance.FinishFade());

        //SceneChangeManager.Instance.SetTime(3f, 0f);        
        //SceneChangeManager.Instance.StartCoroutine("LoadSceneAsync", "Stage0");
        //SceneManager.LoadScene("Stage0");
        
        SceneChangeManager.instance.LoadSceneAsync("Stage0", () =>
        {
            SceneChangeManager.instance.FadeIn(startGameFadingInfo);
        }
            );
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
