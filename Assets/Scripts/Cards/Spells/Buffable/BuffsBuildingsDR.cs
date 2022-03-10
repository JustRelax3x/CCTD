using UnityEngine;
using System.Collections;
namespace Assets.Scripts.Cards.Spells
{
    [CreateAssetMenu(menuName = "Spell/Buffable/BuffsBuildingsDR")]
    public class BuffsBuildingsDR : Spell
    {
        /// <summary>
        /// Buffs on this amount of time.
        /// </summary>
        public int BuffTime;

        public override void CastSpell(GameTile tile)
        {
            if (Damage != 0)
            {
                Cast(BuffDamage(Damage, BuffTime, tile));
            }
            if (Range != 0)
            {
                Cast(BuffRange(Range, BuffTime, tile));
            }
        }
        private IEnumerator BuffDamage(int buffableDamage, int time, GameTile tile)
        {
            tile.Content.AddBuffDamage(buffableDamage);
                yield return new WaitForSeconds(time);
            tile.Content.AddBuffDamage(-buffableDamage);
        }

        private IEnumerator BuffRange(int buffableRange, int time, GameTile tile)
        {
            tile.Content.AddBuffRange(buffableRange);
            yield return new WaitForSeconds(time);
            tile.Content.AddBuffRange(-buffableRange);
        }
    }
}
