using Assets.Scripts.Cards;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private GameTile[] _tiles;

    [SerializeField]
    Vector2Int _size;

    private GameTileContentFactory _contentFactory;

    private readonly Queue<GameTile> _searchFrontier = new Queue<GameTile>();

    private List<GameTile> _spawnPoints = new List<GameTile>();

    private List<GameTileContent> _contentToUpdate = new List<GameTileContent>();

    private BuffsController _buffsController;

    public int SpawnPointCount => _spawnPoints.Count;

    private UnityAction<GameTile> action;

    public void Initialize(GameTileContentFactory contentFactory,BuffsController buffsController, UnityAction<GameTile> OnTileTouch)
    {
        _contentFactory = contentFactory;
        _buffsController = buffsController;
        action = OnTileTouch;
        for (int i=0,y = 0; y < _size.y; y++)
        {
            for (int x = 0; x < _size.x; x++, i++)
            {
                _tiles[i].OnTouch += action;
                if (x != _size.x - 1)
                {
                    GameTile.MakeEastWestNeighbors(_tiles[i + 1], _tiles[i]);
                }
                if (y != _size.y - 1)
                {
                    GameTile.MakeNorthSouthNeighbors(_tiles[i], _tiles[i + _size.x]);
                }
            }
        }
        Clear();
    }

    private void FindPaths()
    {
        foreach (GameTile v in _tiles)
        {
            if (v.IsDestination)
            {
                v.BecomeDestination();
                v.Content = _contentFactory.Get(GameTileContentType.Destination);
                _searchFrontier.Enqueue(v);
            }
            else if(v.IsSpawn)
            {
                v.Content = _contentFactory.Get(GameTileContentType.Spawn);
                _spawnPoints.Add(v);
            }
        }

        while (_searchFrontier.Count > 0)
        {
            GameTile tile = _searchFrontier.Dequeue();
            if (tile != null)
            {
                _searchFrontier.Enqueue(tile.GrowPath(!tile.IsAlternative));
            }
        }

    }

    public void GameUpdate()
    {
        for (int i=0; i < _contentToUpdate.Count; i++)
        {
            _contentToUpdate[i].GameUpdate();
        }
    }
    public GameTile GetSpawnPoint(int index)
    {
        return _spawnPoints[index];
    }

    public bool TryBuild(GameTile tile, Card card)
    {
        if (tile.Content.Type != GameTileContentType.Empty || tile.Way || card.Content <= GameTileContentType.Destination
            || !Game.TrySpendMoney(card.Cost))
        {
            return false;
        }
        GameTileContent gameTileContent = _contentFactory.Get(card.Content);
        gameTileContent.Initialize(card.Range, card.Damage, card.Class);
        tile.Content = gameTileContent;
        _buffsController.SetUpContent(card, tile);
        _contentToUpdate.Add(gameTileContent);
        return true;
    }

    public void ForceBuild(GameTile tile, GameTileContent content)
    {
        if (content.Type == GameTileContentType.Spawn || content.Type == GameTileContentType.Destination)
        { Debug.LogError("Building spawn or destination!!!!"); }
        tile.Content = content;
        _contentToUpdate.Add(content);
    }

    public void DestroyBuilding(GameTile tile)
    {
        if (tile.Content.Type <= GameTileContentType.Spawn)
            return;

        _contentToUpdate.Remove(tile.Content);
        tile.Content = _contentFactory.Get(GameTileContentType.Empty);
    }

    public bool TryCastSpell(GameTile tile, Card card)
    {
        if ((tile.Content.Type < GameTileContentType.Destination && !tile.Way) || card.Content != GameTileContentType.Spell || !Game.TrySpendMoney(card.Cost))
        {
            return false;
        }
        if (tile.Way)
        {
            card.CastSpell();
        }
        else
        {
            card.CastSpell(tile);
        }
        _buffsController.SpellUsed();
        return true;
    }
    public void Clear()
    {
        _contentToUpdate.Clear();
        _buffsController.Clear();
        foreach (GameTile v in _tiles)
        {
            v.Content = _contentFactory.Get(GameTileContentType.Empty);
        }
        FindPaths();
    }

    private void OnDestroy()
    {
        foreach (var v in _tiles)
        {
            v.OnTouch -= action;
        }
    }
}
