using UnityEngine;

[CreateAssetMenu(menuName = "Card/Default")]
public class Card : ScriptableObject
{
    public int Id;
    public int Damage;
    public int Range;
    public int Cost;
    public GameTileContentType Content;
    public Sprite Sprite;
    public CardRarity Rarity;
    public CardClass Class;

    public virtual void CastSpell() {}
    public virtual void CastSpell(GameTile tile = null) {}
    public virtual System.Action GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null) { return null; }
}

public enum CardRarity
{
    Common, 
    Rare, 
    Epic,
    Legendary,
}

public enum CardClass {
    Pyromancer, 
    Warlock, 
    Priest,
    Wizard, 
    Druid,
    Neutral
}

