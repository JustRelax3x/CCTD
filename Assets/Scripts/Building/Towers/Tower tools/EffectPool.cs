using UnityEngine;

namespace Assets.Scripts.Building.Towers.Tower_tools
{
    class EffectPool : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _electrisityEffect;
        [SerializeField]
        private ParticleSystem _defaultEffect;

        private ParticleSystem _particle;

        private ParticleSystem GetParticle(EffectType type)
        {
            switch (type)
            {
                case EffectType.Electrisity:
                    return _electrisityEffect;
                case EffectType.Defualt:
                    return _defaultEffect;
                default:
                    throw new System.ArgumentOutOfRangeException($"No pool for type {type}");
            }
        }

        public void PlayEffect(EffectType effectType, Vector3 position, int damageEffect)
        {
            switch (effectType)
            {
                case EffectType.Electrisity:
                    ElectrisityEffect(position, damageEffect);
                    break;
                case EffectType.Defualt:
                    break;
            }
        }

        private void ElectrisityEffect(Vector3 position, int damageEffect)
        {
            _particle = GetParticle(EffectType.Electrisity);
            var enemies = TargetPoint.GetAllBufferedInBox(position, Constants.HalfTile * Vector3.one);
            int i = 0;
            while (enemies[i] != null)
            {
                enemies[i].transform.root.GetComponent<Enemy>().TakeDamage(damageEffect);
                i++;
            }
            if (i > 0)
            {
                _particle.gameObject.transform.position = position;
                _particle.Play();
            }
        }
    }
}
