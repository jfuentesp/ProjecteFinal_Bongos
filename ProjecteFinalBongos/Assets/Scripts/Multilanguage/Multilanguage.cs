using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace multilanguaje
{
    [Serializable]
    public class Multilanguage
    {
        public Idioma[] m_Idiomas;

        [Serializable]
        public class Idioma
        {
            public string id;
            public PiccoloChanFrases frasesPiccoloChan;
            public ObjetosDatos[] consumiblesDatos;
            public ObjetosDatos[] equipablesDatos;
            public ObjetosDatos[] habilidadesDatos;
        }

        [Serializable]
        public struct PiccoloChanFrases
        {
            public string[] frasesIniciales;
            public string[] frasesFinales;
        }

        [Serializable]
        public struct ObjetosDatos
        {
            public string id;
            public string name;
            public string descripcion;
        }
    }
}