using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSignPos : MonoBehaviour
{
    [SerializeField] private GameObject sign;
    [SerializeField] public Transform[] signTransforms;
    private int currentSignPosIndex = 0;
    public bool shouldUpdateSignPos = false;
    private int nextSignPosIndex = 0;
    void Start()
    {
        if (signTransforms.Length > 0)
        {
            sign.transform.position = signTransforms[0].position;
            sign.transform.rotation = signTransforms[0].rotation;
            currentSignPosIndex = 0;
        }
    }

    public bool CheckUpdateSignPos(int index)
    {
        if (index < 0 || index >= signTransforms.Length)
        {
            return false;
        }

        if (currentSignPosIndex != index)
        {
            nextSignPosIndex = index;
            shouldUpdateSignPos = true;

            return true;
        }

        return false;
    }
    public void UpdateSignPosition(int index)
    {
        if(index < 0 || index >= signTransforms.Length)
        {
            return;
        }
        if (!shouldUpdateSignPos || nextSignPosIndex < 0 || nextSignPosIndex >= signTransforms.Length)
        {
            return;
        }

        sign.transform.position = signTransforms[index].position;
        sign.transform.rotation = signTransforms[index].rotation;
        currentSignPosIndex = nextSignPosIndex;
        shouldUpdateSignPos = false;
    }

    public Transform GetTransform()
    {
        return sign.transform;
    }
}
