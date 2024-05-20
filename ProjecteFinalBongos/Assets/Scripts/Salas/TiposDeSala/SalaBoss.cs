using GeneracionSalas;
using SaveLoadGame;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private LevelManager.BossDisponible m_BossSala;
    Transform m_TransformPlayer;
    private int m_BichitosVivos;

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
                m_TransformPlayer = hit.transform;
                OnPlayerIn?.Invoke(m_TransformPlayer);
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
        LevelManager.Instance.GeneracionSalasInstanciacion.onMapaFinalized += SpawnerSala;
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
    protected override void SpawnerSala()
    {
        print("Hola");
        m_BossSala = LevelManager.Instance.GetBossToSpawn(m_NumeroBoss);
        if (m_BossSala.m_HijosBosses.Length > 0)
        {
            m_BichitosVivos = m_BossSala.m_HijosBosses.Length;
            int numero = 0;
            foreach (GameObject Bossito in m_BossSala.m_HijosBosses)
            {
                GameObject jefe = Instantiate(Bossito, transform);

                jefe.GetComponent<BossBehaviour>().OnBossDeath += BichitoMuerto;
                jefe.transform.localPosition = GetPositionToSpawnBoss(numero);
                numero++;
            }
        }
        else
        {
            SpawnBossFinalSala();
        }
    }

    private IEnumerator SpawnFinalBossCoroutine()
    {
        yield return new WaitForSeconds(2);
        SpawnBossFinalSala();
    }

    private void BichitoMuerto()
    {
        m_BichitosVivos--;

        if (m_BichitosVivos == 0)
            StartCoroutine(SpawnFinalBossCoroutine());
    }

    private void SpawnBossFinalSala()
    {
        GameObject jefe = Instantiate(m_BossSala.m_BossPrefab, transform);

        jefe.GetComponent<BossBehaviour>().OnBossDeath += DesbloquearPuertas;
        jefe.transform.localPosition = GetPositionToSpawnBoss(0);
        if (m_TransformPlayer != null)
            OnPlayerIn?.Invoke(m_TransformPlayer);
    }

    Vector3 GetPositionToSpawnBoss(int position)
    {
        if (EsSalaPar())
        {
            print("Es par");
            Vector2 vectorin = GetPosicionSalaPar(GetMitadPar(0), GetMitadPar(1));
            vectorin.x += position;
            return vectorin;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private int GetMitadPar(int numerin)
    {
        int mitadSalas = m_ListaSalas.Count / 2;
        int mitadInferior = mitadSalas - 1;
        if (numerin == 0)
        {
            return mitadInferior;
        }
        else
        {
            return mitadSalas;
        }
    }

    private Vector2 GetPosicionSalaPar(int numeroMenor, int numeroMayor)
    {
        Vector2 puntoMedio = new Vector2((m_ListaSalas[numeroMenor].x + m_ListaSalas[numeroMayor].x) / 2, (m_ListaSalas[numeroMenor].y + m_ListaSalas[numeroMayor].y) / 2);
        return puntoMedio;
    }

    private bool EsSalaPar()
    {
        if (m_ListaSalas.Count % 2 == 0)
            return true;
        else
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
}
