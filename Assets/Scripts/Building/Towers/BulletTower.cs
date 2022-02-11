using Assets.Scripts.Cards;
using UnityEngine;

public class BulletTower : Tower
{
    private float _damage;
    
    [SerializeField, Range(1f, 1000f)]
    private float _attackSpeed = 10f;

    [SerializeField]
    private Transform _turret;

    private float _attackTimer = 0f;

    private BulletPoolProvider _poolProvider;

    private TargetPoint _target;

    private EffectType _bulletType;

    private CardClass _cardClass;

    private const float _rangeCoefficient = 0.7f;
    private int _buffedDamage;
    private int _defaultbuffedDamage;
    private int _buffedRange;
    private int _targetsNum;
    private int _defaultTargetsNum;
    private int _buffedShots;
    private bool _allTargetShot;
    private bool _keepDamageBuffs;
    private short _maxDamage;
    private bool _onlyGroupTargets;
    private bool _doubleDamageGroupTarget;
    private bool _doubleDamageSecondTargetOnly;

    public override void Initialize(int range, int damage, CardClass cardClass)
    {
        _targetingRange += range * _rangeCoefficient;
        _damage = damage;
        _cardClass = cardClass;
        _defaultTargetsNum = 1;
        _targetsNum = _defaultTargetsNum;
        _poolProvider = BulletPoolProvider.Instance;
        switch (_cardClass)
        {
            case CardClass.Pyromancer:
                _bulletType = EffectType.Defualt;
                break;
            case CardClass.Warlock:
                _bulletType = EffectType.Defualt;
                break;
            case CardClass.Priest:
                _bulletType = EffectType.Electrisity;
                break;
            case CardClass.Wizard:
                _bulletType = EffectType.Defualt;
                break;
            case CardClass.Druid:
                _bulletType = EffectType.Defualt;
                break;
            case CardClass.Neutral:
                _bulletType = EffectType.Defualt;
                break;
        }
    }

    public override void GameUpdate()
    {
        if (IsTargetTracked(ref _target) || IsAcquireTarget(out _target))
        {
            ChooseShotType();   
        }
        _attackTimer -= Time.deltaTime * _attackSpeed;
    }

    public override void SetBuffDamage(int buffedDamage)
    {
        if (!_keepDamageBuffs)
        _damage -= _buffedDamage;
        _buffedDamage = buffedDamage + _defaultbuffedDamage;
        _damage += _buffedDamage;
        if (_keepDamageBuffs && _damage > _maxDamage)
            _damage = _maxDamage;

    }

    public override void SetBuffRange(int buffedRange)
    {
        _targetingRange -= _buffedRange * _rangeCoefficient;
        _buffedRange = buffedRange;
        _targetingRange += _buffedRange * _rangeCoefficient;
    }

    public override void SetBuffTargets(int targets)
    {
        _buffedShots += 1;
        if (targets == -1)
        {
            _allTargetShot = true;
        }
        else
        {
            _targetsNum = targets;
        }
    }

    public void ForceShoot()
    {
        float cooldown = _attackTimer;
        _attackTimer = 0;
        ChooseShotType();
        _attackTimer = cooldown;
    }

    private void ChooseShotType()
    {
        if (_damage > 0 && _targetsNum != 0)
        {
            if (_targetsNum == 1 && !_allTargetShot && !_onlyGroupTargets)
                Shoot();
            else
                MultyTargetShoot();
        }
    }

    private void Shoot()
    {
        if (_target == null) return;
        var point = _target.Position;
        point -= transform.position;
        if (point.x <= transform.position.x)
        {
            _turret.eulerAngles = new Vector3(0f, 0f, Vector3.Angle(point, Vector3.up));
        }
        else
        {
            _turret.eulerAngles = new Vector3(0f, 0f, -Vector3.Angle(point, Vector3.up));
        }
        if (_attackTimer <= 0)
        {
            _attackTimer = Game.AttackInterval;
            var pool = _poolProvider.GetPool(_bulletType);
            var bullet = pool.GetBullet(transform.position);
            bullet.Init(_damage, _attackSpeed / 2f, _target.Enemy, pool);
            if (_buffedShots > 0)
            {
                BuffShotUsed();
            }
        }
    }

    private void MultyTargetShoot()
    {
        if (_target != null)
        {
            var point = _target.Position;
            point -= transform.position;
            if (point.x <= transform.position.x)
            {
                _turret.eulerAngles = new Vector3(0f, 0f, Vector3.Angle(point, Vector3.up));
            }
            else
            {
                _turret.eulerAngles = new Vector3(0f, 0f, -Vector3.Angle(point, Vector3.up));
            }
        }
        if (_attackTimer <= 0)
        {
           
            if (!_allTargetShot)
            {
                var enemies = TargetPoint.GetAllBufferedInBox(transform.position, _targetingRange * Vector3.one);
                if (_onlyGroupTargets)
                {
                    if ( _targetsNum <= 1 || enemies[1] == null)
                        return;
                }
                int damageCoef;
                for (int i = 0; i < _targetsNum; i++)
                {
                    if (enemies[i] == null) break;
                    var pool = _poolProvider.GetPool(_bulletType);
                    var bullet = pool.GetBullet(transform.position);
                    damageCoef = 1;
                    if (_doubleDamageGroupTarget || (_doubleDamageSecondTargetOnly && i == 1))
                    {
                        damageCoef = 2;
                    }
                    bullet.Init(_damage * damageCoef, _attackSpeed / 2f, enemies[i].transform.root.GetComponent<Enemy>(), pool);
                }
            }
            else
            { //shoot all enemies
                var enemies = Game.GetEnemies();
                for (int i = 0; i < enemies.Length; i++)
                {
                    var pool = _poolProvider.GetPool(_bulletType);
                    var bullet = pool.GetBullet(transform.position);
                    int damageCoef = 1;
                    if (_doubleDamageGroupTarget || (_doubleDamageSecondTargetOnly && i==1))
                    {
                        damageCoef = 2;
                    }
                    bullet.Init(_damage * damageCoef, _attackSpeed / 2f, enemies.Get(i).GetComponent<Enemy>(), pool);
                }
                _allTargetShot = false;
            } 
            _attackTimer = Game.AttackInterval;
            if (_buffedShots > 0)
            {
                BuffShotUsed();
            }
        }
    }
    private void BuffShotUsed()
    {
        _buffedShots--;
        if (_buffedShots == 0)
        {
            _targetsNum = _defaultTargetsNum;
        }
    }

    public void SetBuffTargets(int targets, int sec)
    {
        _buffedShots += (int)(sec * _attackSpeed / Game.AttackInterval);
        _targetsNum = targets;
    }

    public void AddBuffTargets(int sec)
    {
        _buffedShots += (int)(sec * _attackSpeed / Game.AttackInterval);
        _targetsNum += 1;
    }

    public void SetDefaultTargetsNumber(int targetsNumber)
    {
        _defaultTargetsNum = targetsNumber;
        _targetsNum = _defaultTargetsNum;
    }

    public void SetDefaultBuffedDamage(int buffedDamage)
    {
        _defaultbuffedDamage = buffedDamage;
    }

    public void KeepAllDamageBuffs(short maxDamage)
    {
        _keepDamageBuffs = true;
        _maxDamage = maxDamage;
    }

    public void SetOnlyGroupTargets()
    {
        _onlyGroupTargets = true;
    }

    public void SetDoubleDamageGroupOnly()
    {
        _doubleDamageGroupTarget = true;
    }

    public void SetDoubleDamageSecondTargetOnly()
    {
        _doubleDamageSecondTargetOnly = true;
    }

    public void SetBulletType(EffectType bulletType)
    {
        _bulletType = bulletType;
    }
}
