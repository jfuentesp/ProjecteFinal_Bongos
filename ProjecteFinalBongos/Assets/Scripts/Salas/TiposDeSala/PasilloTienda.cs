using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasilloTienda : TipoSala
{

    [SerializeField]
    private GameObject m_Vendedor;
    protected override void SpawnerSala()
    {
        GameObject vendedor = Instantiate(m_Vendedor, transform.position, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_CanOpenDoor = true;
        SpawnerSala();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
