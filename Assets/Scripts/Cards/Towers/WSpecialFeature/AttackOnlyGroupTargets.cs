using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Cards.Towers.WSpecialFeature
{
    [CreateAssetMenu(menuName = "Card/WSpecialFeature/AttackOnlyGroupTargets")]
    public class AttackOnlyGroupTargets : Card
    {
        public int DefaultTargetsNumber;
        public override UnityAction GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null)
        {
            tower.SetDefaultTargetsNumber(DefaultTargetsNumber);
            tower.SetOnlyGroupTargets();
            return null;
        }
    }
}
