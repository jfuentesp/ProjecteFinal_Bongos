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
            public ConsumiblesDatos[] consumiblesDatos;
            public EquipablesDatos[] equipablesDatos;
        }

        [Serializable]
        public struct PiccoloChanFrases
        {
            public string[] frasesIniciales;
            public string[] frasesFinales;
        }

        [Serializable]
        public struct ConsumiblesDatos
        {
            public int id;
            public string descripcion;
        }

        [Serializable]
        public struct EquipablesDatos
        {
            public int id;
            public string descripcion;
        }
    }
}