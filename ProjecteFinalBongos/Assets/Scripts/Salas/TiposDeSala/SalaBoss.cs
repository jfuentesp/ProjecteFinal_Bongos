using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalaBoss : TipoSala
{
    [SerializeField]
    private GameObject m_JefeFinal;
    public Action OnPLayerInSala;
    
    protected override void SpawnerSala()
    {
        GameObject jefe = Instantiate(m_JefeFinal, transform.position, Quaternion.identity);
        jefe.transform.parent = transform;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }
        else
        {
            m_CanOpenDoor = false;
            cambioPuerta?.Invoke(false);
            OnPLayerInSala?.Invoke();
        }
    }

    public Vector2 GetPosicionAleatoriaEnSala()
    {
        Vector2 posicion = Vector2.zero;



        return posicion;
    }

}
