using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject menuUI;

    [SerializeField] private Slider volumeSlider;
    
    private void OpenPauseMenu()
    {
        backGround.SetActive(true);
        menuUI.SetActive(true);
        
        GameManager.instance.Pause();
    }

    public void ClosePauseMenu()
    {
        backGround.SetActive(false);
        menuUI.SetActive(false);

        GameManager.instance.Continue();
    }

    public void SetVolume()
    {
        float value = volumeSlider.value;
        
        SoundManager.instance.SetSfxSoundVolume(value);
        SoundManager.instance.SetBackgroundVolume(value);
    }

    public void SetQuality(int level)
    {
        //todo: level에 따라 지정된 그래픽 수준 변경하기 (높음, 중간, 낮음)
    }

    public void SetResolution()
    {
        //todo: 해상도 조정하는법 익히고 코드 작성하기
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuUI.activeSelf)
                ClosePauseMenu();
            else 
                OpenPauseMenu();
        }
    }
}
