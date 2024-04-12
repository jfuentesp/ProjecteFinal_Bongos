using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [Serializable]
    public class SaveGame
    {
        [Serializable]
        public struct SalasData
        {
            public GeneracionSalas.GeneracionSalasFinal.ListaSalasConHijos[] m_SalasBosses;
            public GeneracionSalas.GeneracionSalasFinal.ListaSalasConHijos[] m_ListaPasillos;

            public SalasData(GeneracionSalas.GeneracionSalasFinal.ListaSalasConHijos[] _SalasBosses, GeneracionSalas.GeneracionSalasFinal.ListaSalasConHijos[] _ListaPasillos)
            {
                m_SalasBosses = _SalasBosses;
                m_ListaPasillos = _ListaPasillos;
            }
        }


        public SalasData matriz;
        public void PopulateDataSalas(ISaveableSalaData _matrixData)
        {
            matriz = _matrixData.Save();
        }
        
    public interface ISaveableSalaData
    {
        public SalasData Save();
        public void Load(SalasData _salaData);
    }

   
}

