using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Cards.Towers
{
    [CreateAssetMenu(menuName = "Card/WOnSpell/ForceShoot")]
    public class ShootWhenSpell : Card
    {
        public override UnityAction GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null)
        {
            return tower.ForceShoot;
        }
    }
}
