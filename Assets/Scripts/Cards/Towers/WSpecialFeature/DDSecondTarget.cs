using UnityEngine;

namespace Assets.Scripts.Cards.Towers.WSpecialFeature
{
    [CreateAssetMenu(menuName = "Card/WSpecialFeature/DDSecondTarget")]
    public class DDSecondTarget : Card
    {
        public int DefaultTargetsNumber;
        public override System.Action GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null)
        {
            tower.SetDefaultTargetsNumber(DefaultTargetsNumber);
            tower.ActivateDoubleDamageSecondTargetOnly();
            return null;
        }
    }
}
