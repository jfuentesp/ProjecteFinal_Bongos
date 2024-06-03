using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveLoadGame
{
    [Serializable]
    public class SaveAllRanquing
    {
        public SaveRecordTimer[] m_SavedRanquings;
    }

    [Serializable]
    public class SaveRecordTimer : MonoBehaviour
    {
        [Serializable]
        public struct RecordTimer
        {
            public string m_NombreJugador;
            public string m_TiempoJugador;
        }

        public RecordTimer m_RecordTimer;

        public void PopulateDataRecordTimer(ISaveableRecordTimerData _RecordTimer)
        {
            m_RecordTimer = _RecordTimer.Save();
        }
        public interface ISaveableRecordTimerData
        {
            public RecordTimer Save();
            public void Load(RecordTimer _salaData);
        }
    }
}
