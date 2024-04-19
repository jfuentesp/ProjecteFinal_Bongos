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
            
            SaveGame data = new SaveGame();
            data.PopulateDataMapaSalas(dataSalas);
            data.PopulateDataSalasBoss(dataBosses);
            
            string jsonData = JsonUtility.ToJson(data);

            try
            {
                Debug.Log("Saving: ");
                Debug.Log(jsonData);

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
                print("cargar");
                string jsonData = File.ReadAllText(saveFileName);
                print(jsonData);
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

            }
            catch (Exception e)
            {
                Debug.LogError($"Error while trying to load {Path.Combine(Application.persistentDataPath, saveFileName)} with exception {e}");
            }
        }
    }

}
