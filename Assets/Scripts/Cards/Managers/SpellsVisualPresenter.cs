using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Cards
{
    public class SpellsVisualPresenter : MonoBehaviour
    {
        [SerializeField] 
        private ParticleSystem _electricity;



        private ParticleSystem _patricle;
        public void VisualizeSpell(Spell spell, Transform transform)
        {
            if (spell.SpellType == Spell.Type.None) return;

            _patricle = GetParticles(spell.SpellType);
            _patricle.transform.position = transform.position;
            _patricle.Play();
        }
        public void StartSpellCoroutine(IEnumerator enumerator)
        {
            StartCoroutine(enumerator);
        }
        public void StopAll()
        {
            StopAllCoroutines();
        }
        private ParticleSystem GetParticles(Spell.Type type)
        {
            switch (type)
            {
                case Spell.Type.Electrisity:
                    return _electricity;
                case Spell.Type.None:
                    return null;
            }
            Debug.LogError($"No particles for: {type}");
            return null;

        }
    }


}
