using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WarEntity : GameBehavior
{
    public WarFactory OriginFactory { get; set; }

    public override void Recycle()
    {
        OriginFactory.Reclaim(this);
    }
    
}
