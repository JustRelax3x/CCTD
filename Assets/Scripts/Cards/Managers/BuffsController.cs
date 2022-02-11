using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Cards
{
    public class BuffsController : MonoBehaviour
    {
        private UnityAction _onSpellUsed;
        public void SetUpContent(Card card, GameTile tile)
        {
            BulletTower tower;
            try
            {
                tower = tile.Content.GetComponent<BulletTower>();
            }
            catch(System.Exception)
            {
                Debug.LogError("Trying to buff" + tile.Content + "Idk how.");
                return;
            }
            UnityAction action = card.GetTowerSetUpOnSpell(tower, tile);
            if (action != null)
                _onSpellUsed += action;
        }



        public void SpellUsed()
        {
            _onSpellUsed?.Invoke();
        }

        public void Clear()
        {
            StopAllCoroutines();
            _onSpellUsed = null;
        }
    }
}