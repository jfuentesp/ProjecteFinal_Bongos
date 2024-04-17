using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
public interface IFaster { 
    public void Faster();
} 
public interface IVampire {
   public int Regenerate(float lifeStealed);
}

public interface ISpike { 
    public void Spike();
}

public interface IRegenrate {
    public void Regenerate(float life, float time);
}




