using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1CutScene : CutScene
{
    [SerializeField] private float windPower;
    
    private bool isPlayed;

    protected override void Awake()
    {
        base.Awake();
        isPlayed = false;
    }
    
    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        
        if (hasExecuted && !isPlayed)
        {
            SoundManager.instance.FadeOutBackgroundVolume();
            myCoroutine = StartCoroutine("PlayCutScene");
            isPlayed = true;
        }
    }
    
    
    private IEnumerator PlayCutScene()
    {
        GameManager.instance.CinematicMode();
        FadingInfo fadingInfo = new FadingInfo(1f, 0, 1, 0);
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;
        
        SceneChangeManager.instance.FadeIn(fadingInfo);
        cameraMovements[0].cutSceneCamera.Priority = 14;
        _isRotation = true;
        cameraMovements[0].dollyCart.enabled = true;
        //particleObject.Play();
        yield return new WaitForSeconds(cameraMovements[0].timeToMove - 1f);

        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;
        
        SceneChangeManager.instance.FadeIn(fadingInfo);
        cameraMovements[1].cutSceneCamera.Priority = 16;
        SoundManager.instance.SfxPlay("windcut1", soundEffect[0], cameraMovements[0].dollyCart.transform.position, 0.9f, 50, 50);

        yield return new WaitForSeconds(3f);
        
        balloon.GetComponent<Rigidbody>().AddForce((Vector3.right + Vector3.up) * windPower, ForceMode.Impulse);
        yield return new WaitForSeconds(cameraMovements[1].timeToMove - 3f);
        
        SceneChangeManager.instance.FadeOut(fadingInfo);
        //스팀도전과제
        SteamManager.instance.UpdateClearStage(1);
        yield return waitingFadeFinish;
        
        fadingInfo.playTime = 5;
        void FadeIn() => SceneChangeManager.instance.FadeIn(fadingInfo);
        SceneChangeManager.instance.LoadSceneAsync("stage2", onFinish: FadeIn);
    }
}
