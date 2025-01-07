using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ShowDeathCnt : MonoBehaviour
{
    public TextMeshPro deathCountText;
    [SerializeField] private int deathCount;
    // Start is called before the first frame update
    void Start()
    {
        LoadDeathCount();
    }

    void Update()
    {
        LoadDeathCount();
        if(deathCount > 0)
            ShowDeathCount();
    }
    // Update is called once per frame
    private void ShowDeathCount()
    {
        deathCountText.text = $"<sprite=0> × {deathCount}";
    }
    // 죽은 횟수 값 불러오는 방법 추후 수정 필요
    private void LoadDeathCount()
    {
        deathCount = PlayerPrefs.GetInt("DeathCount");
    }
}
