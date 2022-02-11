using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Cards.Spells
{
    [CreateAssetMenu(menuName = "Spell/Damageable/DamageMixedHR")]
    public class DamageMixedHR : Spell
    {
        /// <summary>
        /// Decreases damage(-=) each cast 
        /// </summary>
        public int DecreasingDamage;
        /// <summary>
        /// How many times to cast the spell to higher hp enemy.
        /// </summary>
        public int TimesCastingHigherHp;
        /// <summary>
        /// How many times to cast the spell to random enemy.
        /// </summary>
        public int TimesCastingRandom;
        /// <summary>
        /// Higher hp enemies first, then random enemy?
        /// </summary>
        public bool QHigherHpThenRandom;


        public override void CastSpell()
        {
            Cast(CastMixedSpell(TimesCastingHigherHp, TimesCastingRandom, QHigherHpThenRandom, Damage, DecreasingDamage));
        }

        private IEnumerator CastMixedSpell(int timesCastingHigher,int timesCastingRandom, bool orderHR, int spellDamage, int decreasingDamage)
        {
            if (orderHR)
            {
                while (timesCastingHigher + timesCastingRandom > 0 && spellDamage > 0)
                {
                    if (timesCastingHigher > 0)
                    {
                        CastHighestHp(spellDamage);
                        timesCastingHigher--;
                        spellDamage -= decreasingDamage;
                        yield return new WaitForSeconds(0.5f);
                        continue;
                    }
                    CastRandom(spellDamage);
                    timesCastingRandom--;
                    spellDamage -= decreasingDamage;
                    yield return new WaitForSeconds(0.5f);
                }
            }
            else
            {
                while (timesCastingHigher + timesCastingRandom > 0 && spellDamage > 0)
                {
                    if (timesCastingRandom > 0)
                    {
                        CastRandom(spellDamage);
                        timesCastingRandom--;
                        spellDamage -= decreasingDamage;
                        yield return new WaitForSeconds(0.5f);
                        continue;
                    }
                    CastHighestHp(spellDamage);
                    timesCastingHigher--;
                    spellDamage -= decreasingDamage;
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }
}
