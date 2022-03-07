using UnityEngine;

namespace Assets.Scripts.Cards.Towers
{
    [CreateAssetMenu(menuName = "Card/WOnSpell/ForceShoot")]
    public class ShootWhenSpell : Card
    {
        public override System.Action GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null)
        {
            return tower.ForceShoot;
        }
    }
}
