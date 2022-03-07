using UnityEngine;

namespace Assets.Scripts.Cards.Towers
{
    [CreateAssetMenu(menuName = "Card/WOnSpell/EnableAttack")]
    public class AttackWhenSpell : Card
    {
        public int DefaultTargetsAmount;
        public int BuffedAttackTargetsAmount;
        public int BuffActionTime;

        private BulletTower _tower;
        public override System.Action GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null)
        {
            _tower = tower;
            _tower.SetDefaultTargetsNumber(DefaultTargetsAmount);
            return BuffAttackTargets;
        }

        private void BuffAttackTargets()
        {
            _tower.SetBuffTargets(BuffedAttackTargetsAmount, BuffActionTime);
        }
    }
}
