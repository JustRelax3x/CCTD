using System;
using System.Collections.Generic;

[Serializable]
public class GameBehaviorCollection
{
    private List<GameBehavior> _behaviors = new List<GameBehavior>();
    public bool IsEmpty => _behaviors.Count == 0;

    public int Length => _behaviors.Count;

    public void Add(GameBehavior behavior)
    {
        _behaviors.Add(behavior);
    }

    public GameBehavior Get(int i)
    {
        if (i < Length) return _behaviors[i];
        return null;
    }

    public void GameUpdate()
    {
        for (int i = 0; i < _behaviors.Count; i++)
        {
            if (!_behaviors[i].GameUpdate())
            {
                int lastIndex = _behaviors.Count - 1;
                _behaviors[i] = _behaviors[lastIndex];
                _behaviors.RemoveAt(lastIndex);
                i -= 1;
            }
        }
    }

    public GameBehavior GetRandom()
    {
        int i = UnityEngine.Random.Range(0, Length);
        return _behaviors[i];
    }

    public GameBehavior GetHighestHp()
    {
        if (IsEmpty) return null;
        GameBehavior gameBehavior = _behaviors[0];
        Enemy enemy, enemy1;
        try
        {
            enemy = gameBehavior.GetComponent<Enemy>();
        }
        catch (System.Exception)
        {
            return null;
        }
        for (int i=1; i < Length; i++)
        {
            enemy1 = _behaviors[i].GetComponent<Enemy>();
            if (enemy.Health < enemy1.Health)
            {
                enemy = enemy1;
                gameBehavior = _behaviors[i];
            }
        }
        return gameBehavior;
    }
    
    public void Clear()
    {
        for (int i = 0; i < _behaviors.Count; i++)
        {
            _behaviors[i].Recycle();
        }
        _behaviors.Clear();
    }
}