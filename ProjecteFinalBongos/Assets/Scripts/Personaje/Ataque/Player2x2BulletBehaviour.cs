using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2x2BulletBehaviour : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BossHurtBox") || collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * 3f ;
    }


}
