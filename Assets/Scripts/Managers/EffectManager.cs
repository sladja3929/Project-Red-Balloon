using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    private readonly List<DeathEffect> _deathEffects = new ();

    //public method that instantiate death effect
    public void ShowDeathEffect(EffectType type)
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
        effect.Show(GameManager.instance.GetBalloonPosition());
    }

    public IEnumerator ShowDeathEffectCoroutine(EffectType type,  float t)
    {
        yield return new WaitForSeconds(t);
        ShowDeathEffect(type);
    }
    
    //public method that hide death effect
    public void HideDeathEffect(DeathEffect effect)
    {
        effect.Hide();
    }
}
