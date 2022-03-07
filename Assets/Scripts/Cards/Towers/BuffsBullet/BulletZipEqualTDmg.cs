using UnityEngine;

namespace Assets.Scripts.Cards.Towers.BuffsBullet
{
    [CreateAssetMenu(menuName = "Card/WOnBullet/BulletZipEqualTDmg")]
    public class BulletZipEqualTDmg : Card
    {
        public int DefaultBuffedDamage;

        public int DefaultBuffedRange;
        public override System.Action GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null)
        {
            tower.ActivateBulletEffectDamageEqualTD();
            return null;
        }
    }
}
