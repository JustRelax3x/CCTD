using Assets.Scripts.Cards;
using UnityEngine;

public class BulletTower : Tower
{
    private int _damage;
    
    [SerializeField, Range(1f, 1000f)]
    private float _attackSpeed = 10f;

    [SerializeField]
    private Transform _turret;

    private float _attackTimer = 0f;

    private BulletPoolProvider _poolProvider;

    private TargetPoint _target;

    private EffectType _bulletType;

    public CardClass CardClass { get; private set; }

    private const float _rangeCoefficient = 0.7f;
    private int _buffedDamage, _defaultbuffedDamage, _buffedRange,_targetsNum,_defaultTargetsNum;
    private float _buffedTime;
    private short _maxDamage;
    private bool _allTargetShot,_keepDamageBuffs,_onlyGroupTargets,_doubleDamageGroupTarget,_doubleDamageSecondTargetOnly;
    private bool _BulletEffectDamageEqualTD;
    public enum AttackType
    {
        Default,
        MaxHp,
        FullHp,
    }

    private AttackType _attackType;

    public override void Initialize(int range, int damage, CardClass cardClass)
    {
        _targetingRange += range * _rangeCoefficient;
        _damage = damage;
        CardClass = cardClass;
        _defaultTargetsNum = 1;
        _targetsNum = _defaultTargetsNum;
        _poolProvider = BulletPoolProvider.Instance;
        _attackType = AttackType.Default;
        _bulletType = _poolProvider.GetBuletEffect(CardClass);
    }

    public override void GameUpdate()
    {
        if (IsTargetTracked(ref _target) || IsAcquireTarget(out _target))
        {
            ChooseShotType();   
        }
        float tick = Time.deltaTime;
        _attackTimer -= tick * _attackSpeed;
        if (_buffedTime > 0)
        {
            BuffTimeTick(tick);
        }
    }

    public override void AddBuffDamage(int buffedDamage)
    {
        if (!_keepDamageBuffs)
        _damage -= _buffedDamage;
        _buffedDamage += buffedDamage > 0 ? buffedDamage + _defaultbuffedDamage : buffedDamage - _defaultbuffedDamage;
        _damage += _buffedDamage;
        if (_keepDamageBuffs && _damage > _maxDamage)
            _damage = _maxDamage;

    }

    public override void AddBuffRange(int buffedRange)
    {
        _targetingRange -= _buffedRange * _rangeCoefficient;
        _buffedRange += buffedRange;
        _targetingRange += _buffedRange * _rangeCoefficient;
    }

    public override void SetBuffTargets(int targets)
    {
        _buffedTime += 1;
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
            InitBullet(ChooseEnemyByAttackType());
            _attackTimer = Constants.AttackInterval;
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
                var enemiesColliders = TargetPoint.GetAllBufferedInBox(transform.position, _targetingRange * Vector3.one);
                Enemy[] enemies = ChooseEnemies(enemiesColliders);
                if (_onlyGroupTargets)
                {
                    if ( _targetsNum <= 1 || enemies[1] == null)
                        return;
                }
                int damageCoef;
                for (int i = 0; i < _targetsNum; i++)
                {
                    if (enemies[i] == null) break;
                    damageCoef = 1;
                    if (_doubleDamageGroupTarget || (_doubleDamageSecondTargetOnly && i == 1))
                    {
                        damageCoef *= 2;
                    }
                    InitBullet(enemies[i], damageCoef);
                }
            }
            else
            { 
                var enemies = GameController.GetEnemies();
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
            _attackTimer = Constants.AttackInterval;
        }
    }

    private Enemy ChooseEnemyByAttackType()
    {
        Enemy enemy = _target.Enemy;
        switch (_attackType)
        {
            case AttackType.Default:
                break;
            case AttackType.MaxHp:
                {
                    var enemies = TargetPoint.GetAllBufferedInBox(transform.position, _targetingRange * Vector3.one);
                    if (enemies[1] != null)
                    { 
                       
                        int i = 0;
                        float maxhp = 0;
                        while (enemies[i] != null)
                        {
                            Enemy enemy1 = enemies[i].transform.root.GetComponent<Enemy>();
                            if (enemy1.Health > maxhp)
                            {
                                maxhp = enemy1.Health;
                                enemy = enemy1;
                            }
                            i++;
                        }
                    }
                    break;
                }
            case AttackType.FullHp:
                break;
        }
        return enemy;
    }
    private Enemy[] ChooseEnemies(Collider[] enemies)
    {
        Enemy[] enemiesMaxHp = new Enemy[_targetsNum];
        if (enemies[_targetsNum] == null)
        {
            for (int i = 0; i < _targetsNum; i++)
            {
                if (enemies[i] == null) return enemiesMaxHp;
                enemiesMaxHp[i] = enemies[i].transform.root.GetComponent<Enemy>();
            }
        }
        switch (_attackType)
        {
            case AttackType.Default:
            {
                    for (int i = 0; i < _targetsNum; i++) {
                        enemiesMaxHp[i] = enemies[i].transform.root.GetComponent<Enemy>();
                    }
                    break;
            }
            case AttackType.MaxHp:
            {
                float[] maxHp = new float[_targetsNum];
                int i = 0, j;
                while (enemies[i] != null)
                {
                    Enemy enemy = enemies[i].transform.root.GetComponent<Enemy>();
                    float hp = enemy.Health;
                    for (j = 0; j < _targetsNum; j++)
                    {
                        if (hp > maxHp[j])
                        {
                            int k = _targetsNum - 1;
                            while (j < k)
                            {
                                maxHp[k] = maxHp[k - 1];
                                enemiesMaxHp[k] = enemiesMaxHp[k - 1];
                            }
                            maxHp[j] = hp;
                            enemiesMaxHp[j] = enemy;
                            break;
                        }
                    }
                    i++;
                }
                break;
            }
            case AttackType.FullHp:
                goto case AttackType.Default; //change
        }
        return enemiesMaxHp;
    }

    private void InitBullet(Enemy enemy, int damageCoef = 1)
    {
        var pool = _poolProvider.GetPool(_bulletType);
        var bullet = pool.GetBullet(transform.position);
        if (_BulletEffectDamageEqualTD)
        {
            bullet.SetUpEffectDamage(_damage);
        }
        bullet.Init(_damage*damageCoef, _attackSpeed / 2f, enemy, pool);
    }
    private void BuffTimeTick(float time)
    {
        _buffedTime -= time;
        if (_buffedTime <= 0)
        {
            _targetsNum = _defaultTargetsNum;
        }
    }

    public void SetBuffTargets(int targets, int sec)
    {
        _buffedTime += sec;
        _targetsNum = targets;
    }

    public void AddBuffTargets(int sec)
    {
        _buffedTime += sec;
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

    public void ActivateOnlyGroupTargets()
    {
        _onlyGroupTargets = true;
    }

    public void ActivateDoubleDamageGroupOnly()
    {
        _doubleDamageGroupTarget = true;
    }

    public void ActivateDoubleDamageSecondTargetOnly()
    {
        _doubleDamageSecondTargetOnly = true;
    }

    public void SetBulletType(EffectType bulletType)
    {
        _bulletType = bulletType;
    }
    public void ActivateBulletEffectDamageEqualTD()
    {
        _BulletEffectDamageEqualTD = true;
    }
}
