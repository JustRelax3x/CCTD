using System;
using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
    [Serializable]
    class EnemyConfig
    {
        public Enemy Prefab;
        
        public Vector2 MinMaxScale = new Vector2(0.5f, 2f);
        
        public float Speed = 1f;

        public int Health = 100;
    }
        [SerializeField] 
        private EnemyConfig _troll, _slime, _zombi;

    public Enemy Get(EnemyType type)
    {
        var config = GetConfig(type);
        Enemy instance = CreateGameObjectInstance(config.Prefab);
        instance.OriginFactory = this;
        instance.Initialize(UnityEngine.Random.Range(config.MinMaxScale.x,config.MinMaxScale.y),
            config.Speed, config.Health);
        return instance;
    }

    private EnemyConfig GetConfig(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Troll:
                return _troll;
            case EnemyType.Slime:
                return _slime;
            case EnemyType.Zombi:
                return _zombi;
        }
        Debug.LogError($"No config for: {type}");
        return _slime;
    }

    public void Reclaim(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}