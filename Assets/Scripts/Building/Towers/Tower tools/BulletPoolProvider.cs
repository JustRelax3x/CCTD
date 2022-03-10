using Assets.Scripts.Building.Towers.Tower_tools;
using UnityEngine;

public class BulletPoolProvider : MonoBehaviour
{
    [SerializeField]
    private BulletPool _electrisityPool;
    [SerializeField]
    private BulletPool _electrisityExtraZipPool;
    [SerializeField]
    private BulletPool _bulletPool;
    [SerializeField]
    private EffectPool _effectPool;

    //To do 1 pool for all bullets

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
            case EffectType.ElectrisityExtraZip:
                return _electrisityExtraZipPool;
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
    public EffectType GetBuletEffect(CardClass cardClass)
    {
        EffectType bulletType = EffectType.Defualt;
        switch (cardClass)
        {
            case CardClass.Pyromancer:
                bulletType = EffectType.Defualt;
                break;
            case CardClass.Warlock:
                bulletType = EffectType.Defualt;
                break;
            case CardClass.Priest:
                bulletType = EffectType.Electrisity;
                break;
            case CardClass.Wizard:
                bulletType = EffectType.Defualt;
                break;
            case CardClass.Druid:
                bulletType = EffectType.Defualt;
                break;
            case CardClass.Neutral:
                bulletType = EffectType.Defualt;
                break;
        }
        return bulletType;
    }
}

public enum EffectType
{
    Electrisity=0,
    ElectrisityExtraZip=1,
    Defualt=2, 
}
