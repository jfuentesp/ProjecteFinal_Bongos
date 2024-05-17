using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphShaderController : MonoBehaviour
{
    [SerializeField, Range(-0.5f, 1.5f)]
    private float m_Evadido;
    private Material m_MaterialPadre;
    private Material m_MaterialHijo;


    private void Awake()
    {
        m_MaterialPadre = GetComponent<SpriteRenderer>().material;
        m_MaterialHijo = transform.GetChild(0).GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        m_MaterialPadre.SetFloat("_Dissolve", m_Evadido);
        m_MaterialHijo.SetFloat("_Dissolve", m_Evadido);
    }
}
