using UnityEngine;

namespace Assets.Scripts.Cards.Towers.WSpecialFeature
{
    [CreateAssetMenu(menuName = "Card/WSpecialFeature/AttacksHigherHp")]
    public class AttacksHigherHp : Card
    {
        public override System.Action GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null)
        {
            tower.SetBulletType(EffectType.Electrisity);
            return null;
        }
    }
}