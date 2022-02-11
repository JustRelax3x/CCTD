using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Cards.Towers
{
    [CreateAssetMenu(menuName = "Card/WOnBuff/KeepBuffs")]
    public class KeepBuffs : Card
    {
        public bool KeepDamage;
        public short MaxDamage;
        public bool KeepRange;
        public short MaxRange;

        public override UnityAction GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null)
        {
            if (KeepDamage)
                tower.KeepAllDamageBuffs(MaxDamage);
            if (KeepRange)
                Debug.LogError("Can't set keep range to tower. Idk how.");
            return null;
        }
    }
}
