using GeneracionSalas;
using SaveLoadGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SaveLoadGame.SaveGame;
using Random = UnityEngine.Random;

public class SalaBoss : TipoSala, ISaveableSalaBossData
{
    [Header("Listas salas hijas")]
    GeneracionSalasMatriz.ListaSalasConHijos m_ListaSalasPadreHijas;
    private List<GeneracionSalasMatriz.ListaSalas> m_ListaSalas;
    private float minX, minY, maxX, maxY;

    [Header("Variables Boxcast")]
    [SerializeField]
    private Vector2 m_BoxCastSize;
    [SerializeField]
    private float m_BoxCastRange;
    [SerializeField]
    private LayerMask m_BoxLayerMask;
    [SerializeField]
    private float m_TimeBetweenBoxCast;

    [Header("Variables sala")]
    private int m_NumeroBoss;
    private bool m_HaEntradoElPlayer;
    public Action<Transform> OnPlayerIn;
    [SerializeField] private GameEvent m_JugadorEnSalaEvent;

    private void Start()
    {
        m_HaEntradoElPlayer = false;
        m_CanOpenDoor = true;
        StartCoroutine(DeteccionPlayer());
    }

    private IEnumerator DeteccionPlayer()
    {
        while (!m_HaEntradoElPlayer)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, m_BoxCastSize, 0, transform.position, m_BoxCastRange, m_BoxLayerMask);
            if (hit.collider != null)
            {
                m_CanOpenDoor = false;
                cambioPuerta?.Invoke(false);
                OnPlayerIn?.Invoke(hit.transform);
                m_JugadorEnSalaEvent.Raise();
                m_HaEntradoElPlayer = true;
            }
            yield return new WaitForSeconds(m_TimeBetweenBoxCast);
        }
    }

    public void Init(GeneracionSalasMatriz.ListaSalasConHijos _ListaSalasPadreHijas, int numeroBoss)
    {
        m_NumeroBoss = numeroBoss;
        m_ListaSalasPadreHijas = _ListaSalasPadreHijas;
        TodasLasSalasEnUnaLista();
        MaximosMinimosSala();
        SpawnerSala();
    }

    private void DesbloquearPuertas()
    {
        cambioPuerta?.Invoke(true);
    }

    private void TodasLasSalasEnUnaLista()
    {
        m_ListaSalas = m_ListaSalasPadreHijas.m_HabitacionesHijas;
    }

    private void MaximosMinimosSala()
    {
        minX = m_ListaSalas[0].x;
        minY = m_ListaSalas[0].y;
        maxX = m_ListaSalas[0].x;
        maxY = m_ListaSalas[0].y;
        foreach (GeneracionSalasMatriz.ListaSalas sala in m_ListaSalas)
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
        minX = minX * 20 - 10f;
        minY = minY * 20 - 10f;
        maxX = maxX * 20 + 10f;
        maxY = maxY * 20 + 10f;
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
        float x = (int)posicion.x / 20;
        if (posicion.x % 20 > 10)
            x++;
        if (posicion.x % 20 < -10)
            x--;
        float y = (int)posicion.y / 20;
        if (posicion.y % 20 > 10)
            y++;
        if (posicion.y % 20 < -10)
            y--;
        foreach (GeneracionSalasMatriz.ListaSalas sala in m_ListaSalas)
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

    public SalaBossData Save()
    {
        SaveGame.SalaBossData salaBossData = new SalaBossData();
        salaBossData.m_NumeroBoss = m_NumeroBoss;
        salaBossData.m_SalasHijas = m_ListaSalasPadreHijas;
        salaBossData.m_SalaTransform = transform.position;

        return salaBossData;
    }

    public void Load(SalaBossData _salaBossData)
    {
        m_NumeroBoss = _salaBossData.m_NumeroBoss;
        m_ListaSalasPadreHijas = _salaBossData.m_SalasHijas;
        Init(m_ListaSalasPadreHijas, m_NumeroBoss);
    }

    protected override void SpawnerSala()
    {
        print(LevelManager.Instance.GetBossToSpawn(m_NumeroBoss));
        GameObject jefe = Instantiate(LevelManager.Instance.GetBossToSpawn(m_NumeroBoss), transform);

        jefe.GetComponent<BossBehaviour>().OnBossDeath += DesbloquearPuertas;
        jefe.transform.localPosition = Vector3.zero;
    }
}
