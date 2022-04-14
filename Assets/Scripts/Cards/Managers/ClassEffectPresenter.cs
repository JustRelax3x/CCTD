using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassEffectPresenter
{
    private ClassEffectHandler _classEffectHandler = new ClassEffectHandler();
    
    private bool _extraZipStatus = false;
    
    public void Initialize(in PlayerClassEffect playerClassEffect)
    {
        int l = playerClassEffect.ClassEffects.Length;
        for (int i = 0; i < l; i++)
        {
            if (playerClassEffect.ClassEffects[i] != null)
            {
                _classEffectHandler.ActivateEffect(i, playerClassEffect.ClassEffects[i]);
            }
        }
    }

    public void TowerBuilt(BulletTower tower) 
    {
        ExtraZipEffectTrigger(tower);
    }

    public void SpellUsed(List<BulletTower> towers)
    {
        VoltageEffectTrigger(towers);
    }

    public void PlayerHpChanged(List<BulletTower> towers)
    {
        ExtraZipEffectTrigger(towers);
    }

    private void VoltageEffectTrigger(List<BulletTower> towers)
    {
        if (_classEffectHandler.EffectBuffStatus(VoltagePriest.CClass, new VoltagePriest()))
        {
            int length = towers.Count;
            for (int i = 0; i < length; i++)
            {
                if (towers[i].CardClass == VoltagePriest.CClass)
                {
                    GameController.StartSpellCoroutine(BuffDamage(VoltagePriest.Damage, towers[i]));
                }
            }
        }
    }
    private void ExtraZipEffectTrigger(List<BulletTower> towers)
    {
        if (_extraZipStatus == _classEffectHandler.EffectBuffStatus(ExtraZipPriest.CClass, new ExtraZipPriest())) return;
        _extraZipStatus = !_extraZipStatus;
        EffectType effect = _extraZipStatus ? ExtraZipPriest.Effect : BulletPoolProvider.Instance.GetBuletEffect(ExtraZipPriest.CClass);
        int length = towers.Count;
        for (int i = 0; i < length; i++)
        {
            if (towers[i].CardClass == ExtraZipPriest.CClass)
                towers[i].SetBulletType(effect);
        }
    }

    private void ExtraZipEffectTrigger(BulletTower tower)
    {
        if (!_extraZipStatus || tower.CardClass != ExtraZipPriest.CClass) return;
        tower.SetBulletType(ExtraZipPriest.Effect);
    }

    private readonly WaitForSeconds _waitVoltageTime = new WaitForSeconds(VoltagePriest.Time);
    private IEnumerator BuffDamage(int buffableDamage, BulletTower tower)
    {
        tower.AddBuffDamage(buffableDamage);
        yield return _waitVoltageTime;
        tower.AddBuffDamage(-buffableDamage);
    }

    public void Clear()
    {
        _extraZipStatus = false;
    }
}

