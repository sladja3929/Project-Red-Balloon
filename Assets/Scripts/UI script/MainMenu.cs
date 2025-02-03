using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class MainMenu : MonoBehaviour
{
    public bool starting = false;
    
    public MainOption optionMenu; 
    public GameObject mainMenu;
    public GameObject credits;
    public Button continueButton;
    public GameObject titleText;
    
    public void NewGame()
    {
        if (starting) return;
        starting = true;

        SaveManager.instance.ResetSave();
        StartCoroutine(PlayGameCoroutine());
    }
    
    public void ContinueGame()
    {
        if (SaveManager.instance.IsNewSave()) return;
        
        if (starting) return;
        starting = true;

        StartCoroutine(PlayGameCoroutine());        
    }

    private IEnumerator PlayGameCoroutine()
    {
        FadingInfo startGameFadingInfo = new (
            1.5f, 
            0, 
            1, 
            0
            );
        
        SceneChangeManager.instance.FadeOut(startGameFadingInfo);
        yield return new WaitUntil(() => SceneChangeManager.instance.FinishFade());

        int stage = SaveManager.instance.Stage - 1;
        if (stage == -1)
        {
            stage = 0;
            SaveManager.instance.Stage = stage;
        }
        
        SceneChangeManager.instance.LoadSceneAsync
        ($"Stage{stage}", () =>
        {
            SceneChangeManager.instance.FadeIn(startGameFadingInfo);
        });
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void Awake()
    {
        if (SaveManager.instance.IsNewSave())
        {
            // 클릭 불가능하게 만들기
            continueButton.interactable = false;
            
            // 이미지 바꾸기
            Image image = continueButton.targetGraphic as Image;
            if (image != null)
            {
                image.color = new Color(1, 1, 1, 0.5f);
            }
        }
        
        
    }
    
    public void OpenPauseMenu()
    {
        if (gameObject.activeSelf == false) return;
        optionMenu.OpenPauseMenu();
        
        Debug.Log("MainToOption");
        gameObject.SetActive(false);
        titleText.SetActive(false);
    }

    public void OpenCredits()
    {
        if (gameObject.activeSelf == false) return;
        
        credits.SetActive(true);
        mainMenu.SetActive(false);
        titleText.SetActive(false);
    }
    
    public void OpenHowToPlay()
    {
        if (gameObject.activeSelf == false) return;
        
        mainMenu.SetActive(false);
        titleText.SetActive(false);
    }

    public void BackToMenu()
    {
        gameObject.SetActive(true);
        titleText.SetActive(true);
        
        Debug.Log("OptionToMain");
        optionMenu.ClosePauseMenu();
        credits.SetActive(false);
    }
}
