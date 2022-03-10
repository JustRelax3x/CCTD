using UnityEngine;

public class ElectrisityExtraZip : Bullet
{
    private int _electrisityDamage = 1;

    private bool _extraZipUsed = false; 

    private const int _extraZipDamage = 1;

    public override void SetUpEffectDamage(int effectDamage)
    {
        if (effectDamage <= 0) return;
        _electrisityDamage = effectDamage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_target != null)
            _target.TakeDamage(_ownerDamage, EffectType.Electrisity, _electrisityDamage);
        if (!_extraZipUsed)
        {
            var enemies = TargetPoint.GetAllBufferedInBox(transform.position, Constants.HalfTile * Vector3.one);
            bool hasEnemy = enemies[0] != null;
            if (hasEnemy)
            {
                _target = enemies[0].transform.root.GetComponent<Enemy>();
                _ownerDamage = _extraZipDamage;
                _extraZipUsed = true;
                return;
            }
        }
        _pool.ReturnBullet(this);
    }
}


