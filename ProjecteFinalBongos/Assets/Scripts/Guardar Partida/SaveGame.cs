using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SaveGame
{
    [Serializable]
    public class SaveGame
    {
        [Serializable]
        public struct SalasData
        {
            public GeneracionSalas.GeneracionSalasMatriz.ListaSalasConHijos[] m_SalasBosses;
            public GeneracionSalas.GeneracionSalasMatriz.ListaSalasConHijos[] m_ListaPasillos;

            public SalasData(GeneracionSalas.GeneracionSalasMatriz.ListaSalasConHijos[] _SalasBosses, GeneracionSalas.GeneracionSalasMatriz.ListaSalasConHijos[] _ListaPasillos)
            {
                m_SalasBosses = _SalasBosses;
                m_ListaPasillos = _ListaPasillos;
            }
        }
        [Serializable]
        public struct SalaBossData
        {
            public Vector3 m_PosicionBoss;
            public SalaBossData(Vector3 _PosicionBoss)
            {
                m_PosicionBoss = _PosicionBoss; 
            }
        }


        public SalasData matriz;
        public void PopulateDataSalas(ISaveableSalasData _matrixData)
        {
            matriz = _matrixData.Save();
        }

        public interface ISaveableSalasData
        {
            public SalasData Save();
            public void Load(SalasData _salaData);
        }

        public interface ISaveableSalaBossData
        {
            public SalaBossData Save();
            public void Load(SalaBossData _salaBossData);
        }
    }
}
