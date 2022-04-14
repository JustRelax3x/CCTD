using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public Enemy Enemy { get; private set; }
    private bool _isEnabled;
    public bool IsEnabled
    {
        get { return _isEnabled; }
        set
        {
            _collider.enabled = value;
            _isEnabled = value;
        }
    }
    public Vector3 Position => transform.position;

    public float ColliderSize { get; private set; }

    private const int ENEMY_LAYER_MASK = 1 << 9;

    private static Collider[] _buffer = new Collider[20];
    public static int BufferedCount { get; private set; }

    private SphereCollider _collider;

    private void Awake()
    {
        Enemy = transform.root.GetComponent<Enemy>();
        _collider = GetComponent<SphereCollider>();
        ColliderSize = _collider.radius * transform.localScale.x;
    }

    public static bool FillBufferInCapsule(Vector3 position, float range)
    {
        ClearBuffer();
        BufferedCount = Physics.OverlapCapsuleNonAlloc(position, position, range, _buffer, ENEMY_LAYER_MASK);
        return BufferedCount > 0;
    }

    public static bool FillBufferInBox(Vector3 position, Vector3 halfSize)
    {
        ClearBuffer();
        BufferedCount = Physics.OverlapBoxNonAlloc(position, halfSize, _buffer,
            Quaternion.identity, ENEMY_LAYER_MASK);
        return BufferedCount > 0;
    }
    public static Collider[] GetAllBufferedInBox(Vector3 position, Vector3 halfSize)
    {
        ClearBuffer();
        BufferedCount = Physics.OverlapBoxNonAlloc(position, halfSize, _buffer,
            Quaternion.identity, ENEMY_LAYER_MASK);
        return _buffer;
    }

    private static void ClearBuffer()
    {
        for (int i=0; i < BufferedCount; i++)
        {
            _buffer[i] = null;
        }
        BufferedCount = 0;
    } 

    public static TargetPoint GetBuffered(int index)
    {
        var target = _buffer[index].GetComponent<TargetPoint>();
        return target;
    } 
    public static TargetPoint GetBuffered(Vector3 position)
    {
        int index = 0;
        float range = Vector3.Distance(_buffer[0].transform.root.position, position);
        for (int i=1; i<BufferedCount; i++)
        {
            if (Vector3.Distance(_buffer[i].transform.root.position,position) < range)
            {
                index = i;
            }
        }
        var target = _buffer[index].GetComponent<TargetPoint>();
        return target;
    }
} 