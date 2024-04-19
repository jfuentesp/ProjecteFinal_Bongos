using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
public interface IFaster { 
    public void Faster();
} 
public interface IVampire {
   public void Regenerate(GameObject boss, GameObject player);
}

public interface ISpike { 
    public void Spike(GameObject boss);

}
public interface IRegenrate {
    public void Regenerate(float life, float time);
}

public interface IStateChanger
{
    public void ChangeState(GameObject go);
}





