using UnityEngine;

namespace Assets.Scripts.Cards.Towers.WSpecialFeature
{
    [CreateAssetMenu(menuName = "Card/WSpecialFeature/MultyTarget")]
    public class MultyTarget : Card
    {
        public int DefaultTargetsNumber;
        public override System.Action GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null)
        {
            tower.SetDefaultTargetsNumber(DefaultTargetsNumber);
            return null;
        }
    }
}
