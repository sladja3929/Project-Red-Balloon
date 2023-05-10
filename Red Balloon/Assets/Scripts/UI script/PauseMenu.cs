using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject menuUI;

    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    private void OpenPauseMenu()
    {
        backGround.SetActive(true);
        menuUI.SetActive(true);

        GameManager.Instance.IsPause = true;

        sfxVolumeSlider.value = SoundManager.Instance.GetSfxSoundVolume();
        musicVolumeSlider.value = SoundManager.Instance.GetBackgroundVolume();
    }

    public void ClosePauseMenu()
    {
        backGround.SetActive(false);
        menuUI.SetActive(false);

        GameManager.Instance.IsPause = false;
    }

    public void SetSfxVolume()
    {
        float value = sfxVolumeSlider.value;
        SoundManager.Instance.SetSfxSoundVolume(value);
    }

    public void SetBackgroundVolume()
    {
        float value = musicVolumeSlider.value;
        SoundManager.Instance.SetBackgroundVolume(value);
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
        GameManager.Instance.QuitGame();
    }

    public void BackToMainMenu()
    {
        GameManager.Instance.GoToMainMenu();
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
