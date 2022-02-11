using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Cards.Towers
{
    [CreateAssetMenu(menuName = "Card/WOnBuff/EffectiveBuffs")]
    public class EffectiveBuffs : Card
    {
        public int DefaultBuffedDamage;

        public int DefaultBuffedRange;
        public override UnityAction GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null)
        {
            if (DefaultBuffedDamage > 0) 
            tower.SetDefaultBuffedDamage(DefaultBuffedDamage);
            if (DefaultBuffedRange > 0)
                Debug.LogError("Can't set default range to tower. Idk how.");
            return null;
        }
    }
}
