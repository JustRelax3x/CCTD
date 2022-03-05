using UnityEngine;

namespace Assets.Scripts.Building.Towers.Tower_tools.Bullets
{         
public class ElectrisityBullet : Bullet
    {
        private int _electrisityDamage = 1;

        public override void SetUpEffectDamage(int effectDamage)
        {
            _electrisityDamage = effectDamage;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (_target != null)
            _target.TakeDamage(_ownerDamage, EffectType.Electrisity, _electrisityDamage);
            _pool.ReturnBullet(this);
        }
    }
}
