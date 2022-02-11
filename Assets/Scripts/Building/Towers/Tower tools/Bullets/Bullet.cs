using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected float _ownerAttackSpeed;
    protected float _ownerDamage;
    protected Enemy _target;
    protected BulletPool _pool;

    public void Init(float Damage, float AttackSpeed, Enemy target, BulletPool pool)
    {
        _ownerAttackSpeed = AttackSpeed;
        _ownerDamage = Damage;
        _target = target;
        _pool = pool;
    }
    public virtual void SetUpEffect(int effectDamage) { }

    protected void Update()
    {
        if (_target == null)
        {
            _pool.ReturnBullet(this);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, _target.Position,
            _ownerAttackSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        _target.TakeDamage(_ownerDamage);
        _pool.ReturnBullet(this);
    }
}