using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TritoChains : Bullet
{
    public override void Init(Vector2 direction)
    {
        base.Init(direction);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!enabled)
            return;
    }
}
