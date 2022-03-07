using UnityEngine;

namespace Assets.Scripts.Cards.Towers.WSpecialFeature
{
    [CreateAssetMenu(menuName = "Card/WSpecialFeature/DDWhenGroupTarget")]
    public class DDWhenGroupTarget : Card
    {
        public int DefaultTargetsNumber;
        public override System.Action GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null)
        {
            tower.SetDefaultTargetsNumber(DefaultTargetsNumber);
            tower.ActivateDoubleDamageGroupOnly();
            return null;
        }
    }
}