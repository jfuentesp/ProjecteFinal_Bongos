using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveLoadGame
{
    [Serializable]
    public class SavePreSets : MonoBehaviour
    {
        [Serializable]
        public struct PreSets
        {
            public string m_Resolucion;
            public bool m_FullScreen;
            public string idioma;
        }

        public PreSets m_PreSets;

        public void PopulateDataPreSets(ISaveablePreSetsData _PreSets)
        {
            m_PreSets = _PreSets.Save();
        }
        public interface ISaveablePreSetsData
        {
            public PreSets Save();
            public void Load(PreSets _salaData);
        }
    }
}
