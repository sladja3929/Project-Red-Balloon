using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkEffect : DeathEffect
{
    [SerializeField] private AnimationCurve trajectory;
    [SerializeField] private float moveTime;
    [SerializeField] private float moveDistance;
    [SerializeField] private Vector3 positionPreset;
    public override void Show(Vector3 position)
    {
        gameObject.SetActive(true);

        _currentTime = 0;
        position += positionPreset;
        SetStartPos(position);

        StartCoroutine(MoveCoroutine());
    }
    
    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override EffectType GetEffectType()
    {
        return EffectType.Shark;
    }

    #region TEST
    
    [Header("Test")]
    [SerializeField] private Vector3 testPosition;

    [ContextMenu("Play")]
    private void Play()
    {
        Show(testPosition);
    }
    
    #endregion
    
    #region PRIVATE
    
    private float _currentTime;
    private Vector3 _startPos;
    private Vector3 _endPos;

    private void SetStartPos(Vector3 pos)
    {
        transform.position = pos;

        _startPos = pos;
        _endPos = pos + Vector3.forward * moveDistance;
    }

    private IEnumerator MoveCoroutine()
    {
        while (_currentTime < moveTime)
        {
            var curPosition = transform.position;
            
            _currentTime += Time.deltaTime;
            var height = trajectory.Evaluate(_currentTime / moveTime) * moveDistance;
            
            curPosition = Vector3.Lerp(_startPos, _endPos, _currentTime / moveTime);
            curPosition.y += height;
            
            transform.LookAt(curPosition);
            transform.position = curPosition;
            
            yield return null;
        }

        Hide();
    }
    
    #endregion
}
