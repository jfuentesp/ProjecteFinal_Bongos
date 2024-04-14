using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using Random = UnityEngine.Random;

public class SalaBoss : TipoSala
{
    [SerializeField]
    private GameObject m_JefeFinal;
    public Action OnPLayerInSala;
    [SerializeField]
    GeneracionSalas.GeneracionSalasFinal.ListaSalasConHijos m_ListaSalasPadreHijas;
    private List<GeneracionSalas.GeneracionSalasFinal.ListaSalas> m_ListaSalas;
    [SerializeField]
    private Transform m_Target;
    private float minX, minY, maxX, maxY;

    private void Start()
    {
        MaximosMinimosSala();
        TodasLasSalasEnUnaLista();
        GameObject jefe = Instantiate(m_JefeFinal, transform);
        jefe.transform.localPosition = Vector3.zero;
        jefe.GetComponent<BossBehaviour>().Init(m_Target);
    }

    private void TodasLasSalasEnUnaLista()
    {
        m_ListaSalas = new List<GeneracionSalas.GeneracionSalasFinal.ListaSalas>
        {
            m_ListaSalasPadreHijas.m_HabitacionPadre
        };
        if (m_ListaSalasPadreHijas.m_HabitacionesHijas.Count > 0)
        {
            foreach (GeneracionSalas.GeneracionSalasFinal.ListaSalas sala in m_ListaSalasPadreHijas.m_HabitacionesHijas)
            {
                m_ListaSalas.Add(sala);
            }
        }
    }

    private void MaximosMinimosSala()
    {
        minX = m_ListaSalasPadreHijas.m_HabitacionPadre.x;
        minY = m_ListaSalasPadreHijas.m_HabitacionPadre.y;
        maxX = m_ListaSalasPadreHijas.m_HabitacionPadre.x;
        maxY = m_ListaSalasPadreHijas.m_HabitacionPadre.y;
        if (m_ListaSalasPadreHijas.m_HabitacionesHijas.Count > 0)
        {
            foreach (GeneracionSalas.GeneracionSalasFinal.ListaSalas sala in m_ListaSalasPadreHijas.m_HabitacionesHijas)
            {
                if (sala.x < minX)
                    minX = sala.x;
                if (sala.y < minY)
                    minY = sala.y;
                if (sala.x > maxX)
                    maxX = sala.x;
                if (sala.y > maxY)
                    maxY = sala.y;
            }
        }
        minX = minX * 20 - 10f;
        minY = minY * 20 - 10f;
        maxX = maxX * 20 + 10f;
        maxY = maxY * 20 + 10f;
    }

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
        Vector2 posicion = new(Random.Range(minX, maxX), Random.Range(minY, maxY));
        if (!EstaEnAlgunaSala(posicion))
        {
            posicion = GetPosicionAleatoriaEnSala();
        }
        return posicion;
    }

    private bool EstaEnAlgunaSala(Vector2 posicion)
    {
        float x = (int) posicion.x / 20;
        if (posicion.x % 20 > 10)
            x++;
        if (posicion.x % 20 < -10)
            x--;
        float y = (int) posicion.y / 20;
        if (posicion.y % 20 > 10)
            y++;
        if (posicion.y % 20 < -10)
            y--;
        foreach (GeneracionSalas.GeneracionSalasFinal.ListaSalas sala in m_ListaSalas)
        {
            if (sala.x == x && sala.y == y)
            {
                if (posicion.x >= x * 20 - 10 && posicion.x <= x * 20 + 10 && posicion.y >= y * 20 - 10 && posicion.y <= y * 20 + 10)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
