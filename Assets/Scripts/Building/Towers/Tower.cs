using UnityEngine;

public abstract class Tower : GameTileContent
{
    protected float _targetingRange = 0.1f;

    protected bool IsAcquireTarget(out TargetPoint target)
    {
        if (TargetPoint.FillBufferInBox(transform.position, _targetingRange * Vector3.one))
        {
            target = TargetPoint.GetBuffered(transform.position);
            return true;
        }

        target = null;
        return false;
    }

    protected bool IsTargetTracked(ref TargetPoint target)
    {
        if (target == null)
        {
            return false;
        }

        Vector3 myPos = transform.position;
        Vector3 targetPos = target.Position;
        if (Vector3.Distance(myPos, targetPos) > _targetingRange +
            target.ColliderSize * target.Enemy.Scale)
        {
            target = null;
            return false;
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.position;
        position.y += 0.01f;
        Gizmos.DrawWireCube(position, 2f * _targetingRange * Vector3.one);
    }
}