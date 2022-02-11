using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameBehavior : MonoBehaviour
{
    public virtual bool GameUpdate() => true;

    public abstract void Recycle();
}
