using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Cards
{
    public class BuffsController : MonoBehaviour
    {
        private System.Action _onSpellUsed;

        private DevotionHandler _devotionHandler = new DevotionHandler();

        private List<BulletTower> _towers = new List<BulletTower>();

        public void Initialize(System.Action SpellUsedAction)
        {
            SpellUsedAction += SpellUsed;
            ExtraZipDevotionTrigger();
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
            ExtraZipDevotionTrigger(tower);
        }



        public void SpellUsed()
        {
            _onSpellUsed?.Invoke();
            if (_devotionHandler.DevotionBuffStatus(VoltagePriest.CClass, new VoltagePriest()))
            {
                int length = _towers.Count;
                for (int i = 0; i < length; i++)
                {
                    if (_towers[i].CardClass == VoltagePriest.CClass)
                    GameController.StartSpellCoroutine(BuffDamage(VoltagePriest.Damage, VoltagePriest.Time, _towers[i]));
                }
            }
        }

        private void PlayerHpChanged()
        {
            ExtraZipDevotionTrigger();
        }

        private bool _extraZipStatus = false;
        private void ExtraZipDevotionTrigger()
        {
            if (_extraZipStatus == _devotionHandler.DevotionBuffStatus(ExtraZipPriest.CClass, new ExtraZipPriest())) return;
            _extraZipStatus = !_extraZipStatus;
            EffectType effect = _extraZipStatus ? ExtraZipPriest.Effect : BulletPoolProvider.Instance.GetBuletEffect(ExtraZipPriest.CClass);
            int length = _towers.Count;
            for (int i = 0; i < length; i++)
            {
                if (_towers[i].CardClass == ExtraZipPriest.CClass)
                    _towers[i].SetBulletType(effect);
            }
        }
        private void ExtraZipDevotionTrigger(BulletTower tower)
        {
            if (!_extraZipStatus || tower.CardClass != ExtraZipPriest.CClass) return;
            tower.SetBulletType(ExtraZipPriest.Effect);
        }

        public void Clear()
        {
            StopAllCoroutines();
            _onSpellUsed = null;
            _towers.Clear();
        }

        private IEnumerator BuffDamage(int buffableDamage, int time, BulletTower tower)
        {
            tower.AddBuffDamage(buffableDamage);
            yield return new WaitForSeconds(time);
            tower.AddBuffDamage(-buffableDamage);
        }
    }
}