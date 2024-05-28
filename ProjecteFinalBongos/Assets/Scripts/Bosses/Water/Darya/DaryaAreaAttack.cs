using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaryaAreaAttack : MonoBehaviour
{
    private bool tocao;
    private void Start()
    {
        tocao = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !tocao)
        {
            if (collision.gameObject.TryGetComponent(out PJSMB player))
            {
                tocao = true;
            }
        }
    }

    public void Petao()
    {
        Destroy(gameObject);
    }
}