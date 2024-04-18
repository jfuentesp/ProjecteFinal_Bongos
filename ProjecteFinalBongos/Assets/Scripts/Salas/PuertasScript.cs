using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertasScript : MonoBehaviour
{
    private TipoSala m_TipoSala;

    private void Start()
    {
        if (transform.parent.TryGetComponent(out SalaBoss esHijoDeBoss) || transform.parent.parent.TryGetComponent(out SalaBoss esHijoDeHijoDeBoss))
        {
            m_TipoSala = transform.parent.GetComponentInParent<TipoSala>();
            m_TipoSala.cambioPuerta += TryOpenDoor;
            TryOpenDoor(m_TipoSala.CanOpenDoor);
        }
    }

    private void TryOpenDoor(bool canOpenDoor)
    {
        if (canOpenDoor)
            transform.GetChild(0).gameObject.SetActive(false);
        else
            transform.GetChild(0).gameObject.SetActive(true);
    }

}
