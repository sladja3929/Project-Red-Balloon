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
        deathCountText.text = $"<sprite=0> �� {deathCount}";
    }
    // ���� Ƚ�� �� �ҷ����� ��� ���� ���� �ʿ�
    private void LoadDeathCount()
    {
        deathCount = PlayerPrefs.GetInt("DeathCount");
    }
}
