using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static SaveLoadGame.SaveGame;

namespace SaveLoadGame
{
    public class SaveDataManager : MonoBehaviour
    {
        private const string saveFileName = "savegame.json";

        public void SaveData()
        {

            ISaveableSalasData dataSalas = FindObjectOfType<GeneracionSalas.GeneracionSalasMatriz>();
            ISaveableSalaBossData[] dataBosses = FindObjectsByType<SalaBoss>(FindObjectsSortMode.None);
            ISaveableTiendasData[] dataTiendas = FindObjectsByType<PasilloTienda>(FindObjectsSortMode.None);
            ISaveableObjetosData[] dataPasilloObjetos = FindObjectsByType<PasilloObjetos>(FindObjectsSortMode.None);

            SaveGame data = new SaveGame();
            data.PopulateDataMapaSalas(dataSalas);
            data.PopulateDataSalasBoss(dataBosses);
            data.PopulateDataPasilloTienda(dataTiendas);
            data.PopulateDataPasilloObjetos(dataPasilloObjetos);
            
            string jsonData = JsonUtility.ToJson(data);

            try
            {
                File.WriteAllText(saveFileName, jsonData);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while trying to save {Path.Combine(Application.persistentDataPath, saveFileName)} with exception {e}");
            }
        }
        public void LoadData()
        {
            try
            {
                string jsonData = File.ReadAllText(saveFileName);
                SaveGame data = new SaveGame();
                JsonUtility.FromJsonOverwrite(jsonData, data);

                FindObjectOfType<GeneracionSalas.GeneracionSalasMatriz>().Load(data.m_Mapa);

                SalaBoss[] salasLoot = FindObjectsByType<SalaBoss>(FindObjectsSortMode.None);
                for (int i = 0; i < salasLoot.Length; i++)
                {
                    foreach (SaveGame.SalaBossData salita in data.m_Bosses)
                    {
                        if (salita.m_SalaTransform == salasLoot[i].transform.position)
                            salasLoot[i].Load(salita);
                    }
                }

                PasilloTienda[] pasillosTienda = FindObjectsByType<PasilloTienda>(FindObjectsSortMode.None);

                for (int i = 0; i < pasillosTienda.Length; i++)
                {
                    foreach (SaveGame.PasilloTiendaData pasillito in data.m_PiccolosChad)
                    {
                        if (pasillito.m_SalaTransform == pasillosTienda[i].transform.position)
                            pasillosTienda[i].Load(pasillito);
                    }
                }

                PasilloObjetos[] pasillosObjetos = FindObjectsByType<PasilloObjetos>(FindObjectsSortMode.None);

                for (int i = 0; i < pasillosObjetos.Length; i++)
                {
                    foreach (SaveGame.PasilloObjetosData pasillito in data.m_PasilloObjetos)
                    {
                        if (pasillito.m_SalaTransform == pasillosObjetos[i].transform.position)
                            pasillosObjetos[i].Load(pasillito);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while trying to load {Path.Combine(Application.persistentDataPath, saveFileName)} with exception {e}");
            }
        }
    }

}
