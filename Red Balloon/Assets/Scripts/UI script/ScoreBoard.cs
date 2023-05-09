using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public GameObject scoreText;
    public List<GameObject> lines;

    private void Awake()
    {
        lines = new List<GameObject>();
    }
    private void OnEnable()
    {
        var records = GameManager.Instance.records;
        foreach (var r in records)
        {
            var go = Instantiate(scoreText, transform).GetComponent<Text>();
            go.text = r + "second";
        }
    }

    private void OnDisable()
    {
        lines.Clear();
    }

    public void BackToMenu()
    {
        
        IEnumerator LoadSceneCoroutine(string target)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(target);
            asyncOperation.allowSceneActivation = false;
        
            while (asyncOperation.progress < 0.9f)
            {
                yield return null;
                Debug.Log(asyncOperation.progress);
            }

            asyncOperation.allowSceneActivation = true;
        }

        StartCoroutine(LoadSceneCoroutine("MainMenu"));
    }
}
