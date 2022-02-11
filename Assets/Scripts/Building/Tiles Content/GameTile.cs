using UnityEngine;
using UnityEngine.Events;

public class GameTile : MonoBehaviour
{
    [SerializeField]
    private bool IsWay = false;
    public bool Way => IsWay;

    public bool IsDestination = false;

    public bool IsSpawn = false;

    public bool IsAlternative = false; //если поиск пути против часовой

    private GameTile _north, _east, _south, _west, _nextOnPath;

    private int _distance = int.MaxValue;

    public bool HasPath => _distance != int.MaxValue;


    private GameTileContent _content;

    public GameTile NextTileOnPath => _nextOnPath;
    public GameTile North => _north;
    public GameTile East => _east;
    public GameTile South => _south;
    public GameTile West => _west;

    public Vector3 ExitPoint { get; private set; }

    //public Direction PathDirection { get; private set; }

    public event UnityAction<GameTile> OnTouch;

    public GameTileContent Content
    {
        get => _content;
        set
        {
            if (_content != null)
            {
                _content.Recycle();
            }

            _content = value;
            _content.transform.localPosition = transform.position;
            _content.transform.localPosition = new Vector3(_content.transform.localPosition.x, _content.transform.localPosition.y, 0f);
        }
    }

    public static void MakeEastWestNeighbors(GameTile east, GameTile west)
    {
        west._east = east;
        east._west = west;
    }

    public static void MakeNorthSouthNeighbors(GameTile north, GameTile south)
    {
        north._south = south;
        south._north = north;
    }

    public void BecomeDestination()
    {
        _distance = 0;
        _nextOnPath = null;
        ExitPoint = transform.position;
    }

    private GameTile GrowPathTo(GameTile neighbor) // Direction direction)
    {
        if (!HasPath || neighbor == null || neighbor.HasPath || !neighbor.IsWay)
        {
            return null;
        }

        neighbor._distance = _distance + 1;
        neighbor._nextOnPath = this;
        neighbor.ExitPoint = neighbor.transform.position; //+ transform.position) * 0.5f; // + direction.GetHalfVector();
                                                          //neighbor.PathDirection = direction;
        return neighbor;
    }

    public GameTile GrowPathNorth() => GrowPathTo(_north); // Direction.South);
    public GameTile GrowPathEast() => GrowPathTo(_east); // Direction.West);
    public GameTile GrowPathSouth() => GrowPathTo(_south); // Direction.North);
    public GameTile GrowPathWest() => GrowPathTo(_west); // Direction.East);

    public GameTile GrowPath(bool wayClockwise) // по часовой или против 
    {
        GameTile tile;
        if (wayClockwise)
        {
            tile = GrowPathNorth();
            if (tile != null) return tile;
            tile = GrowPathEast();
            if (tile != null) return tile;
            tile = GrowPathSouth();
            if (tile != null) return tile;
            return GrowPathWest();
        }
        else
        {
            tile = GrowPathWest();
            if (tile != null) return tile;
            tile = GrowPathSouth();
            if (tile != null) return tile;
            tile = GrowPathEast();
            if (tile != null) return tile;
            return GrowPathNorth();
        }   
    }

    private void OnMouseDown()
    {
        OnTouch?.Invoke(this);
    }
}