using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    //public GameObject scoreText;
    //public List<GameObject> lines;
    public Text Score;

    private void Awake()
    {
        //lines = new List<GameObject>();
    }
    private void OnEnable()
    {
        //var records = GameManager.Instance.records;
        //foreach (var r in records)
        //{
        //    var go = Instantiate(scoreText, transform).GetComponent<Text>();
        //    go.text = r + "second";
        //}
        Score.text = GameManager.instance.record;
    }

    private void OnDisable()
    {
        //lines.Clear();
    }

    public void BackToMainMenu()
    {
        GameManager.GoToMainMenu();
        transform.parent.gameObject.SetActive(false);
    }
}
