using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class TipoSala : MonoBehaviour
{
    protected bool m_CanOpenDoor;
    public bool CanOpenDoor => m_CanOpenDoor;
    public Action<bool> cambioPuerta;

    protected Vector3 GetPosicionSPawnear()
    {
        float posRandomX = Random.Range(transform.position.x - 7, transform.position.x + 7);
        float posRandomY = Random.Range(transform.position.y - 3, transform.position.y + 3);

        Vector3 posicionRandom = new Vector3(posRandomX, posRandomY, 0);
        return posicionRandom;
    }
}

