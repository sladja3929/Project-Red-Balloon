using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowArrow : MonoBehaviour
{
    public GameObject arrow;
    public void Show()
    {
        arrow.SetActive(true);
    }

    public void Hide()
    {
        arrow.SetActive(false);
    }
}
