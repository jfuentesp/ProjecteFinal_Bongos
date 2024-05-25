using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class TipoSala : MonoBehaviour
{
    protected bool m_CanOpenDoor;
    protected abstract void SpawnerSala();
    public bool CanOpenDoor => m_CanOpenDoor;
    public Action<bool> cambioPuerta;

    protected Vector3 GetPosicionSPawnear(bool horizontal)
    {
        float posRandomX;
        float posRandomY;

        if (horizontal)
        {
            posRandomX = Random.Range(transform.position.x - 8.2f, transform.position.x + 8.2f);
            posRandomY = Random.Range(transform.position.y - 5.2f, transform.position.y + 5.2f);
        }
        else
        {
            posRandomX = Random.Range(transform.position.x - 5.2f, transform.position.x + 5.2f);
            posRandomY = Random.Range(transform.position.y - 8.2f, transform.position.y + 8.2f);
        }

        Vector3 posicionRandom = new Vector3(posRandomX, posRandomY, 0);
        return posicionRandom;
    }
}

