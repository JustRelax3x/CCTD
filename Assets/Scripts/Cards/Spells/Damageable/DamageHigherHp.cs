using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Cards.Spells
{   
    [CreateAssetMenu(menuName = "Spell/Damageable/DamageHigherHp")]
    public class DamageHigherHp : Spell 
    {
        /// <summary>
        /// Decreases damage(-=) each cast 
        /// </summary>
        public int DecreasingDamage;
        /// <summary>
        /// How many times to cast the spell.
        /// Negative values for endless casting only(wait sec)!
        /// </summary>
        public int TimesCasting;


        public override void CastSpell()
        {
                    if (TimesCasting < 0)
                    {
                        Cast(LegendaryCastHighestHp(-TimesCasting));
                    }
                    else
                    {
                        Cast(CastElectrisitySpell(TimesCasting, Damage, DecreasingDamage));
                    }
        }

        private IEnumerator LegendaryCastHighestHp(int sec)
        {
            while (true)
            {
                CastHighestHp(Damage);
                yield return new WaitForSeconds(sec);
            }
        }
        private IEnumerator CastElectrisitySpell(int timeCasting, int spellDamage, int decreasingDamage)
        {
            while (timeCasting > 0 && spellDamage > 0)
            {
                CastHighestHp(spellDamage);
                timeCasting--;
                spellDamage -= decreasingDamage;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
