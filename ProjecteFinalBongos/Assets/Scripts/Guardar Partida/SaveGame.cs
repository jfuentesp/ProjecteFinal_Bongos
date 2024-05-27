using GeneracionSalas;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static SaveLoadGame.SaveGame;


namespace SaveLoadGame
{
    [Serializable]
    public class SaveAllGames
    {
        public SaveGame[] m_SavedGames;
    }

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
            public string[] m_EquipablesId;
            public Vector3 m_SalaTransform;
            public int m_PiccoloId;

            public PasilloTiendaData(string[] _ObjetosId, string[] _EquipablesId, Vector3 _SalaTransform, int _PiccoloId)
            {
                m_ObjetosId = _ObjetosId;
                m_SalaTransform = _SalaTransform;
                m_PiccoloId = _PiccoloId;
                m_EquipablesId = _EquipablesId;
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
        [Serializable]
        public struct NameAndWorld
        {
            public string m_Name;
            public MundoEnum m_Mundo;

            public NameAndWorld(string _Name, MundoEnum _Mundo)
            {
                m_Name = _Name;
                m_Mundo = _Mundo;
            }
        }
        [Serializable]
        public struct BackPack
        {
            public string[] m_ConsumableSlotId;
            public string[] m_EquipableSlotId;
            public string[] m_QuickCosnumableSlotsId;

            public BackPack(string[] _ConsumableSlotId, string[] _EquipableSlotId, string[] _QuickCosnumableSlotsId)
            {
                m_ConsumableSlotId = _ConsumableSlotId;
                m_EquipableSlotId = _EquipableSlotId;
                m_QuickCosnumableSlotsId = _QuickCosnumableSlotsId;
            }
        }


        //Variables de guardado
        public SalasData m_Mapa;
        public SalaBossData[] m_Bosses;
        public PasilloTiendaData[] m_PiccolosChad;
        public PasilloObjetosData[] m_PasilloObjetos;
        public NameAndWorld m_NameAndWorld;
        public BackPack m_BackPack;


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

        public void PopulateDataBackPack(ISaveableBackPackData _BackPack)
        {
            m_BackPack = _BackPack.Save();
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

        public interface ISaveableBackPackData
        {
            public BackPack Save();
            public void Load(BackPack _BackPack);
        }
    }
}
