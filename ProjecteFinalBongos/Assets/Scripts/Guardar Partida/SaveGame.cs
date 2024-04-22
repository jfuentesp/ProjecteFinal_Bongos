using GeneracionSalas;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SaveLoadGame.SaveGame;


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
        public struct PasilloTiendaData
        {
            public string[] m_ObjetosId;
            public Vector3 m_SalaTransform;
            public int m_PiccoloId;

            public PasilloTiendaData(string[] _ObjetosId, Vector3 _SalaTransform, int _PiccoloId)
            {
                m_ObjetosId = _ObjetosId;
                m_SalaTransform = _SalaTransform;
                m_PiccoloId = _PiccoloId;
            }
        }

        [Serializable]
        public struct PasilloObjetosData
        {
            public string[] m_ObjetosId;
            public Vector3 m_SalaTransform;

            public PasilloObjetosData(string[] _ObjetosId, Vector3 _SalaTransform)
            {
                m_ObjetosId = _ObjetosId;
                m_SalaTransform = _SalaTransform;
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
        public PasilloTiendaData[] m_PiccolosChad;
        public PasilloObjetosData[] m_PasilloObjetos;


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

        public void PopulateDataPasilloTienda(ISaveableTiendasData[] _TiendaData)
        {
            m_PiccolosChad = new PasilloTiendaData[_TiendaData.Length];
            for (int i = 0; i < _TiendaData.Length; i++)
                m_PiccolosChad[i] = _TiendaData[i].Save();
        }

        public void PopulateDataPasilloObjetos(ISaveableObjetosData[] _PasilloObjetosData)
        {
            m_PasilloObjetos = new PasilloObjetosData[_PasilloObjetosData.Length];
            for (int i = 0; i < _PasilloObjetosData.Length; i++)
                m_PasilloObjetos[i] = _PasilloObjetosData[i].Save();
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

        public interface ISaveableTiendasData
        {
            public PasilloTiendaData Save();
            public void Load(PasilloTiendaData _pasilloTiendaData);
        }
        public interface ISaveableObjetosData
        {
            public PasilloObjetosData Save();
            public void Load(PasilloObjetosData _pasilloTiendaData);
        }
    }
}
