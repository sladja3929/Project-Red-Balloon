using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSignManager : Singleton<TempSignManager>
{
    private int savePointIndex = 0;
    // Start is called before the first frame update
    private new void Awake()
    {
        base.Awake();
    }

    public int GetSavePointIndex()
    {
        return savePointIndex;
    }

    public void IncrementSavePointIndex()
    {
        savePointIndex++;
    }

    public void DecrementSavePointIndex()
    {
        savePointIndex--;
    }
}
