using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleBehaviour : MonoBehaviour
{
    [SerializeField] private float Damage;

    public void Morir()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox")) {
            if (collision.gameObject.GetComponent<SMBPlayerParryState>().parry)
                Morir();
            collision.gameObject.GetComponent<PJSMB>().recibirDaño(Damage);
        }
    }
}
