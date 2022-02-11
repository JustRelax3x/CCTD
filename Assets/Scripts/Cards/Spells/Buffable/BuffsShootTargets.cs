using UnityEngine;
using System.Collections;
namespace Assets.Scripts.Cards.Spells
{
    [CreateAssetMenu(menuName = "Spell/Buffable/BuffsShootTargets")]
    public class BuffsShootTargets : Spell
    {
        /// <summary>
        /// Buffs on 1 shot number of targets. 
        /// (-1 for all enemies)
        /// </summary>
        public int BuffTargets;

        public override void CastSpell(GameTile tile)
        {
            Cast(BuffTarget(BuffTargets, tile));  
        }
        private IEnumerator BuffTarget(int buffTargets, GameTile tile)
        {
            tile.Content.SetBuffTargets(buffTargets);
            yield return 0;
        }
    }
}
