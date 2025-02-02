using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Stage2CutScene : CutScene
{
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
            myCoroutine = StartCoroutine("PlayCutScene");
            isPlayed = true;
        }
    }
    
    private IEnumerator PlayCutScene()
    {
        //init
        GameManager.instance.CinematicMode();
        FadingInfo fadingInfo = new FadingInfo(1f, 0, 1, 0);
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;
        
        //top view
        SceneChangeManager.instance.FadeIn(fadingInfo);
        cameraMovements[0].cutSceneCamera.Priority = 14;
        cameraMovements[0].dollyCart.enabled = true;
        StartCoroutine(RotateCamera(cameraMovements[0].timeToMove));
        yield return new WaitForSeconds(cameraMovements[0].timeToMove - 1f);

        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;
        
        //side view
        SceneChangeManager.instance.FadeIn(fadingInfo);
        commands[0].ExecuteCommand();
        cameraMovements[1].cutSceneCamera.Priority = 16;
        yield return new WaitForSeconds(1f);
        
        //volcano event
        var volcano = GameObject.Find("Volcano").GetComponent<Volcano>();
        volcano.CutSceneEvent();
        yield return new WaitForSeconds(cameraMovements[1].timeToMove - 2f);
        
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;
        
        //change scene
        fadingInfo.playTime = 5;
        void FadeIn() => SceneChangeManager.instance.FadeIn(fadingInfo);
        SceneChangeManager.instance.LoadSceneAsync("stage3", onFinish: FadeIn);
    }
    
    private IEnumerator RotateCamera(float time)
    {
        float t = 0;
        Vector3 originAngle = cameraMovements[0].dollyCart.transform.rotation.eulerAngles;
        
        while (t < time)
        {
            t += Time.deltaTime;
            float angleZ = Mathf.LerpAngle(originAngle.z, originAngle.z + 70f, t / time);
            Quaternion rotation = Quaternion.Euler(originAngle.x, originAngle.y, angleZ);
            cameraMovements[0].dollyCart.transform.rotation = rotation;
            
            yield return null;
        }
    }
}