using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Cards
{
    public class BuffsHandler : MonoBehaviour
    {
        [SerializeField]
        private PlayerClassEffect _playerClassEffect;

        private System.Action _onSpellUsed;

        private List<BulletTower> _towers = new List<BulletTower>();

        private ClassEffectPresenter _classEffectPresenter = new ClassEffectPresenter();

        public void Initialize()
        {
            _classEffectPresenter.Initialize(in _playerClassEffect);
            GameController.SubscribeOnHpChanged(PlayerHpChanged);
        }
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
            System.Action action = card.GetTowerSetUpOnSpell(tower, tile);
            if (action != null)
                _onSpellUsed += action;
            _towers.Add(tower);
            _classEffectPresenter.TowerBuilt(tower);
        }

        public void SpellUsed()
        {
            _onSpellUsed?.Invoke();
            _classEffectPresenter.SpellUsed(_towers);
        }

        private void PlayerHpChanged()
        {
            _classEffectPresenter.PlayerHpChanged(_towers);
        }

        public void Clear()
        {
            _onSpellUsed = null;
            _towers.Clear();
            _classEffectPresenter.Clear();
        }
    }
}