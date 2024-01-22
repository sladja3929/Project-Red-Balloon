using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeathEffect : MonoBehaviour
{
    public abstract void Show(Vector3 position);
    public abstract void Hide();

    public abstract EffectType GetEffectType();
}

public enum EffectType
{
    None,
    Shark,
    Explosion,
    Blood
}
