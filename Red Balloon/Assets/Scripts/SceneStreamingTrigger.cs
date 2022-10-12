using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStreamingTrigger : MonoBehaviour
{
    [SerializeField] private string streamTargetSceneName;
    [SerializeField] private string triggerOwnScene;

    enum LoadType
    {
        UnloadScene,
        LoadScene
    }

    [SerializeField] private LoadType loadType;

    private IEnumerator StreamingTargetScene()
    {
        var targetScene = SceneManager.GetSceneByName(streamTargetSceneName);
        if (!targetScene.isLoaded)
        {
            var op = SceneManager.LoadSceneAsync(streamTargetSceneName, LoadSceneMode.Additive);

            while (!op.isDone)
            {
                yield return null;
            }
        }
    }

    private IEnumerator UnloadStreamingScene()
    {
        Debug.Log("Scene Unload Call");
        
        var targetScene = SceneManager.GetSceneByName(streamTargetSceneName);
        if (targetScene.isLoaded)
        {
            var currentScene = SceneManager.GetSceneByName(triggerOwnScene);
            SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("MainPlayObject"), currentScene);
            
            var op = SceneManager.UnloadSceneAsync(streamTargetSceneName);

            while (!op.isDone)
            {
                yield return null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(loadType == LoadType.LoadScene) StartCoroutine(StreamingTargetScene());
            if(loadType == LoadType.UnloadScene) StartCoroutine(UnloadStreamingScene());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
