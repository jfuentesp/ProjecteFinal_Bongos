using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasilloFuente : TipoSala
{
    [SerializeField]
    private GameObject m_FountainPrefab;
    protected override void SpawnerSala()
    {
        GameObject fuente = Instantiate(m_FountainPrefab, transform);
        fuente.transform.localPosition = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnerSala(); 
    }
}
