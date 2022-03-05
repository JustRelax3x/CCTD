using UnityEngine;

public class Enemy : GameBehavior
{
    [SerializeField]
    private Transform _model;
    [SerializeField]
    private Transform _bar; 
    [SerializeField]
    private Animator _animator;
    
    public EnemyFactory OriginFactory { get; set; }

    private GameTile _tileFrom, _tileTo;
    private Vector3 _positionFrom, _positionTo;
    private float _progressMovement;

    private float _speed;

    private float _maxHealth;

    private bool[] _effects = new bool[(int)EffectType.Defualt];

    private int _effectDamage;

    public float Scale { get; private set; } 
    public float Health { get; private set; }

    public Vector3 Position => transform.position;

    public void Initialize(float scale, float speed, float health)
    {
        Scale = scale;
        _model.localScale = new Vector3(scale, scale, 1f);
        _speed = speed / scale;
        Health = health * scale;
        _maxHealth = Health;
    }

    public void SpawnOn(GameTile tile)
    {
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, 0f);
        _tileFrom = tile;
        _positionFrom = transform.position;
        _tileTo = tile.NextTileOnPath;
        _positionTo = _tileTo.ExitPoint;
        _positionTo = new Vector3(_positionTo.x, _positionTo.y, 0f);
        _progressMovement = 0f;
    }

    public override bool GameUpdate()
    {
        if (Health <= 0f)
        {
            if (_effects[(int)EffectType.Electrisity])
            {
                BulletPoolProvider.Instance.PlayEffect(EffectType.Electrisity, transform.position, _effectDamage);
            }
            Recycle();
            return false;
        }
        _progressMovement += Time.deltaTime * _speed;
        while(_progressMovement >= 1f)
        {
            _tileFrom = _tileTo;
            _tileTo = _tileTo.NextTileOnPath;
            if (_tileTo == null)
            {
                GameController.EnemyReachedDestination();
                Recycle();
                return false;
            }
            _positionFrom = _positionTo;
            _positionTo = _tileTo.ExitPoint;
            Vector3 direction = Vector3.Normalize(_positionTo - _positionFrom);
            _animator.SetFloat("Horizontal", direction.x);
            _animator.SetFloat("Vertical", direction.y);
            _positionTo = new Vector3(_positionTo.x, _positionTo.y, 0f);
            _progressMovement -= 1f;
        }
        transform.position = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progressMovement);
        return true;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        _bar.localScale = new Vector3(Health / _maxHealth, _bar.localScale.y);
    }

    public void TakeDamage(float damage, EffectType effectType, int effectDamage)
    {
        TakeDamage(damage);
        if (!_effects[(int) effectType])
        {
            _effects[(int)effectType] = true;
            _effectDamage = effectDamage;
        }
    }

    public override void Recycle()
    {
        OriginFactory.Reclaim(this);
    }
}

public enum EnemyType
{
    Troll,
    Slime,
    Zombi,
}