using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    /*
     * 모든 효과음, 배경음악의 재생을 관리하는 스크립트
     * 싱글톤 패턴을 사용하여 Scene의 변경에 관계없이 무조건 단 한개만 static하게 접근 가능하도록 만들었다
     *
     * 배경음악 재생 방식 : inspector창에서 리스트에 집어넣고 나면 Scene의 이름과 완전히 동일한 파일이
     * 배경음악으로 재생된다
     *
     * 효과음 재생 방식 : 특정 스크립트에서 static하게 SoundManager에 접근해서
     * SFXPlay(string name, AudioClip clip)을 실행하면 효과음이 재생된다.
     */
    public AudioSource backgroundSound;
    public AudioClip[] backgroundSoundList;
    
    [SerializeField] private float backgroundVolume;
    [SerializeField] private float sfxVolume;
    [SerializeField] private float delayTime = 2;

    //싱글톤 처리

    private new void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        //수정필요 - 사운드매니저가 시작시작화면에 들어가면 해결
        foreach (var t in backgroundSoundList)
        {
            if ("MainMenu" == t.name) StartCoroutine(BackgroundSoundPlayCoroutine(t));
        }
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        foreach (var t in backgroundSoundList)
        {
            if (arg0.name == t.name) StartCoroutine(BackgroundSoundPlayCoroutine(t));
        }
    }
    
    public void SfxPlay(string sfxName, AudioClip clip, Vector3 position)
    {
        if (clip == null) return;
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        go.transform.position = position;
        
        audioSource.clip = clip;
        audioSource.volume = sfxVolume;
        audioSource.minDistance = 10;
        audioSource.maxDistance = 20;
        audioSource.Play();
        
        Destroy(go, clip.length);
    }

    public void SfxPlay(string sfxName, AudioClip clip, Transform parent)
    {
        if (clip == null) return;
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        go.transform.SetParent(parent);
        go.transform.localPosition = Vector3.zero;
        
        audioSource.clip = clip;
        audioSource.volume = sfxVolume;
        audioSource.minDistance = 10;
        audioSource.maxDistance = 20;
        audioSource.Play();
        
        Destroy(go, clip.length);
    }

    private IEnumerator BackgroundSoundPlayCoroutine(AudioClip clip)
    {
        backgroundSound.Stop();
        if (clip.name != "MainMenu")
        {
            yield return new WaitForSeconds(delayTime);
        }

        backgroundSound.clip = clip;
        backgroundSound.loop = true;
        backgroundSound.volume = backgroundVolume;        
        backgroundSound.Play();
    }

    public void FadeOutBackgroundVolume()
    {
        StartCoroutine(BackGroundVolumeFadeOutCoroutine());
    }
    private IEnumerator BackGroundVolumeFadeOutCoroutine()
    {
        while(backgroundSound.volume > 0)
        {
            backgroundSound.volume -= Time.deltaTime * 0.1f;

            yield return null;
        }
    }

    public void SetBackgroundVolume(float volume)
    {
        backgroundVolume = volume;
        backgroundSound.volume = backgroundVolume;
    }

    public void SetSfxSoundVolume(float volume)
    {
        sfxVolume = volume;
    }

    public float GetBackgroundVolume() => backgroundVolume;
    public float GetSfxSoundVolume() => sfxVolume;
}