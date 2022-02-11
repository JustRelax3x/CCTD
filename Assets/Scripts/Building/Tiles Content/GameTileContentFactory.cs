using UnityEngine;
[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{ 
    [SerializeField]
    private GameTileContent _destinationPrefab;
    [SerializeField]
    private GameTileContent _emptyPrefab;
    [SerializeField]
    private GameTileContent _wallPrefab;
    [SerializeField]
    private GameTileContent _spawnPrefab;
    [SerializeField]
    private GameTileContent _laserPrefab; 
    [SerializeField]
    private GameTileContent _mortarPrefab; 
    [SerializeField]
    private GameTileContent _bulletPrefab;


    public void Reclaim(GameTileContent content)
    {
        Destroy(content.gameObject);
    }

    public GameTileContent Get(GameTileContentType type)
    {
        switch (type)
        {
            case GameTileContentType.Destination:
                return Get(_destinationPrefab);
            case GameTileContentType.Empty:
                return Get(_emptyPrefab);
            case GameTileContentType.Wall:
                return Get(_wallPrefab);
            case GameTileContentType.Spawn:
                return Get(_spawnPrefab);
            case GameTileContentType.LaserTower:
                return Get(_laserPrefab); 
            case GameTileContentType.MortarTower:
                return Get(_mortarPrefab);
            case GameTileContentType.BulletTower:
                return Get(_bulletPrefab);

        }
        return null;
    }

    private T Get<T>(T prefab) where T : GameTileContent
    {
        T instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }
}
