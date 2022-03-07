using UnityEngine;

namespace Assets.Scripts.Cards.Towers.HasOnCardUsedAbility
{
[CreateAssetMenu(menuName = "Card/OnCardUsed/BuffNeighbours")]
public class BuffNeighbours : Card
{
    public int BuffActionSeconds;
    public override System.Action GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null)
    {
        if (tile.North.Content.Type == GameTileContentType.BulletTower)
        {
            tile.North.Content.GetComponent<BulletTower>().AddBuffTargets(BuffActionSeconds);
        }
        if (tile.East.Content.Type == GameTileContentType.BulletTower)
        {
            tile.East.Content.GetComponent<BulletTower>().AddBuffTargets(BuffActionSeconds);
        }
        if (tile.South.Content.Type == GameTileContentType.BulletTower)
        {
            tile.South.Content.GetComponent<BulletTower>().AddBuffTargets(BuffActionSeconds);
        }
        if (tile.West.Content.Type == GameTileContentType.BulletTower)
        {
            tile.West.Content.GetComponent<BulletTower>().AddBuffTargets(BuffActionSeconds);
        }
        return null;
    }
}
}
