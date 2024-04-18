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
    [SerializeField]
    private GameObject m_JefeFinal;
    [SerializeField] GeneracionSalasMatriz.ListaSalasConHijos m_ListaSalasPadreHijas;
    private List<GeneracionSalasMatriz.ListaSalas> m_ListaSalas;
    [SerializeField] private Transform m_Target;
    private float minX, minY, maxX, maxY;
    private bool m_HaEntradoElPlayer;
    [SerializeField]
    private Vector2 m_BoxCastSize;
    [SerializeField]
    private float m_BoxCastRange;
    [SerializeField]
    private LayerMask m_BoxLayerMask;
    [SerializeField]
    private float m_TimeBetweenBoxCast;
    private int m_NumeroBoss;

    public Action<Transform> OnPlayerIn;
    private void Start()
    {
        m_HaEntradoElPlayer = false;
        StartCoroutine(DeteccionPlayer());
    }

    private IEnumerator DeteccionPlayer()
    {
        while (!m_HaEntradoElPlayer)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, m_BoxCastSize, 0, transform.position, m_BoxCastRange, m_BoxLayerMask);
            if (hit.collider != null)
            {
                print("Player dentro");
                OnPlayerIn?.Invoke(hit.transform);
                m_HaEntradoElPlayer = true;
            }
            yield return new WaitForSeconds(m_TimeBetweenBoxCast);
        }
    }

    private void Update()
    {
       
    }

    public void Init(GeneracionSalasMatriz.ListaSalasConHijos _ListaSalasPadreHijas, int numeroBoss)
    {
        m_NumeroBoss = numeroBoss;
        m_ListaSalasPadreHijas = _ListaSalasPadreHijas;
        TodasLasSalasEnUnaLista();
        MaximosMinimosSala();
        GameObject jefe = Instantiate(LevelManager.Instance.GetBossToSpawn(m_NumeroBoss), transform);
        jefe.transform.localPosition = Vector3.zero;
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

        return salaBossData;
    }

    public void Load(SalaBossData _salaBossData)
    {
        throw new NotImplementedException();
    }
}
