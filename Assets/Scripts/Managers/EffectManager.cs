using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    private readonly List<DeathEffect> _deathEffects = new ();

    //public method that instantiate death effect
    public void ShowDeathEffect(EffectType type, Vector3 position)
    {
        if(type == EffectType.None) return;
        
        //find type equals to type in list of death effects
        DeathEffect effect = _deathEffects.Find(x => x.GetEffectType() == type);
        //if type is not found, instantiate new death effect and add it to list
        if (effect == null)
        {
            effect = Instantiate(Resources.Load<DeathEffect>("DeathEffects/" + type));
            _deathEffects.Add(effect);
        }
        //show death effect
        effect.Show(position);
    }

    public IEnumerator ShowDeathEffectCoroutine(EffectType type, Vector3 position, float t)
    {
        yield return new WaitForSeconds(t);
        ShowDeathEffect(type, position);
    }
    
    //public method that hide death effect
    public void HideDeathEffect(DeathEffect effect)
    {
        effect.Hide();
    }
}
