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
        //if (signTransforms.Length > 0)
        //{
        //    UpdateSignPosition(1);
        //}
    }

    public bool CheckUpdateSignPos(int index)
    {
        if (index < 0 || index >= signTransforms.Length)//index값 예외처리
        {
            return false;
        }

        if (currentSignPosIndex != index)//현재 세이브포인트와 다를 경우 표지판위치 업데이트해야 함
        {
            nextSignPosIndex = index;
            shouldUpdateSignPos = true;

            return true;
        }

        return false;
    }

    public void UpdateSignPosition(int index)
    {
        if (index < 0 || index >= signTransforms.Length)//index값 예외처리
        {
            Debug.Log("Index out of range in UpdateSignPosition");
            return;
        }
        //표지판 위치 업데이트해야하는지 + index값 예외처리
        if (!shouldUpdateSignPos || nextSignPosIndex < 0 || nextSignPosIndex >= signTransforms.Length)
        {
            Debug.Log("Invalid state for updating sign position");
            return;
        }
        Debug.Log("UpdateSignPosition called");
        sign.transform.SetPositionAndRotation(signTransforms[index].position, signTransforms[index].rotation);
        currentSignPosIndex = nextSignPosIndex;
        shouldUpdateSignPos = false;
    }

    public Transform GetTransform()
    {
        return sign.transform;
    }
}


