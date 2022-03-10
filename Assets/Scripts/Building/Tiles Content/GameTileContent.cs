using Assets.Scripts.Cards;
using UnityEngine;

[SelectionBase]
public class GameTileContent : MonoBehaviour
{
    [SerializeField]
    private GameTileContentType _type;

    public GameTileContentType Type => _type; 

    public GameTileContentFactory OriginFactory { get; set; }

    public void Recycle()
    {
        OriginFactory.Reclaim(this);
    }

    public virtual void GameUpdate() { }

    public virtual void Initialize(int rangeCoefficient, int damage, CardClass cardClass) { }

    public virtual void AddBuffDamage(int buffableDamage) {}

    public virtual void AddBuffRange(int buffedRange) {}
    public virtual void SetBuffTargets(int targets) {}
}

public enum GameTileContentType
{
    Empty=0, 
    Spawn=1,
    Destination=2,

    Wall=51,

    Spell = 101,

    Tower=201,
    LaserTower=202,
    MortarTower=203,
    BulletTower=204,
}
