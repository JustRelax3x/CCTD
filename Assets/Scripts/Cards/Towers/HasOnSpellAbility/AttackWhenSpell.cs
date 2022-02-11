using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Cards.Towers
{
    [CreateAssetMenu(menuName = "Card/WOnSpell/EnableAttack")]
    public class AttackWhenSpell : Card
    {
        public int DefaultAttackTargets;
        public int BuffedAttackTargets;
        public int BuffActionTime;

        private BulletTower _tower;
        public override UnityAction GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null)
        {
            _tower = tower;
            _tower.SetDefaultTargetsNumber(DefaultAttackTargets);
            return BuffAttackTargets;
        }

        private void BuffAttackTargets()
        {
            _tower.SetBuffTargets(BuffedAttackTargets, BuffActionTime);
        }
    }
}
