using GeneracionSalas;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SaveLoadGame
{
    [Serializable]
    public class SaveGame
    {
        [Serializable]
        public struct SalasData
        {
            public GeneracionSalasMatriz.ListaSalasConHijos[] m_SalasBosses;
            public GeneracionSalasMatriz.ListaSalasConHijos[] m_ListaPasillos;
            public int[] m_Matriz;

            public SalasData(GeneracionSalasMatriz.ListaSalasConHijos[] _SalasBosses, GeneracionSalasMatriz.ListaSalasConHijos[] _ListaPasillos, int[] _Matriz)
            {
                m_SalasBosses = _SalasBosses;
                m_ListaPasillos = _ListaPasillos;
                m_Matriz = _Matriz;
            }
        }

        [Serializable]
        public struct SalaBossData
        {
            public int m_NumeroBoss;
            public GeneracionSalasMatriz.ListaSalasConHijos m_SalasHijas;
            public Vector3 m_SalaTransform;

            public SalaBossData(GeneracionSalasMatriz.ListaSalasConHijos _SalasHijas, int _NumeroBoss, Vector3 _SalaTransform)
            {
                m_SalaTransform = _SalaTransform;
                m_NumeroBoss = _NumeroBoss;
                m_SalasHijas = _SalasHijas;
            }
        }


        //Variables de guardado
        public SalasData m_Mapa;
        public SalaBossData[] m_Bosses;




        //Populates
        public void PopulateDataMapaSalas(ISaveableSalasData _matrixData)
        {
            m_Mapa = _matrixData.Save();
        }

        public void PopulateDataSalasBoss(ISaveableSalaBossData[] _SalasBossData)
        {
            m_Bosses = new SalaBossData[_SalasBossData.Length];
            for (int i = 0; i < _SalasBossData.Length; i++)
                m_Bosses[i] = _SalasBossData[i].Save();
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
