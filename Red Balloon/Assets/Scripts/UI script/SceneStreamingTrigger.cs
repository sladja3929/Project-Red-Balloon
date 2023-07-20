using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


internal enum LoadType
{
    UnloadScene,
    LoadScene
}

public class SceneStreamingTrigger : MonoBehaviour
{
    [SerializeField] private string streamTargetSceneName;
    [SerializeField] private string triggerOwnSceneName;
    [SerializeField] private LoadType loadType;
    
    private IEnumerator StreamingTargetScene()
    {
        var targetScene = SceneManager.GetSceneByName(streamTargetSceneName);
        if (targetScene.isLoaded) yield break;

        var op = SceneManager.LoadSceneAsync(streamTargetSceneName, LoadSceneMode.Additive);

        while (!op.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator UnloadStreamingScene()
    {
        Debug.Log("Scene Unload Call");

        var targetScene = SceneManager.GetSceneByName(streamTargetSceneName);
        if (!targetScene.isLoaded) yield break;

        var currentScene = SceneManager.GetSceneByName(triggerOwnSceneName);
        SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("MainPlayObject"), currentScene);

        var op = SceneManager.UnloadSceneAsync(streamTargetSceneName);

        while (!op.isDone)
            yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") is false) return;
        
        if (loadType == LoadType.LoadScene) StartCoroutine(StreamingTargetScene());
        if (loadType == LoadType.UnloadScene) StartCoroutine(UnloadStreamingScene());
    }
}
