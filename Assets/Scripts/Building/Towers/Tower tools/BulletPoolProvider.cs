using Assets.Scripts.Building.Towers.Tower_tools;
using UnityEngine;

public class BulletPoolProvider : MonoBehaviour
{
    [SerializeField]
    private BulletPool _electrisityPool;
    [SerializeField]
    private BulletPool _bulletPool;
    [SerializeField]
    private EffectPool _effectPool;

    public static BulletPoolProvider Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public BulletPool GetPool(EffectType type)
    {
        switch (type)
        {
            case EffectType.Electrisity:
                return _electrisityPool;
            case EffectType.Defualt:
                return _bulletPool;
            default:
                throw new System.ArgumentOutOfRangeException($"No pool for type {type}");
        }
    }

    public void PlayEffect(EffectType effect, Vector3 position, int damage)
    {
        _effectPool.PlayEffect(effect,position,damage);
    }
}

public enum EffectType
{
    
    Electrisity=0,
    Defualt=1, //last one! 
}
