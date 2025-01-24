using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gimmick : MonoBehaviour
{
    [SerializeField] protected bool isGimmickEnable;

    public virtual void GimmickOn() => isGimmickEnable = true;
    public void GimmickOff() => isGimmickEnable = false;
    public void GimmickSwitch() => isGimmickEnable = !isGimmickEnable;
    public virtual void Execute()
    {
        Debug.Log(gameObject.name + " : Execute 함수를 사용 할 수 없는 기믹입니다.");
        throw new System.NotImplementedException();
    }
}
