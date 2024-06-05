using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static SaveLoadGame.SaveGame;
using static SaveLoadGame.SaveRecordTimer;

namespace SaveLoadGame
{
    public class SaveDataManager : MonoBehaviour
    {
        public void SaveData()
        {

            ISaveableSalasData dataSalas = FindObjectOfType<GeneracionSalas.GeneracionSalasMatriz>();
            ISaveableSalaBossData[] dataBosses = FindObjectsByType<SalaBoss>(FindObjectsSortMode.None);
            ISaveableTiendasData[] dataTiendas = FindObjectsByType<PasilloTienda>(FindObjectsSortMode.None);
            ISaveableObjetosData[] dataPasilloObjetos = FindObjectsByType<PasilloObjetos>(FindObjectsSortMode.None);
            ISaveableAbilitiesPlayerData dataAbilites = FindObjectOfType<AbilitiesGUIController>();
            ISaveableBackPackData dataBackpack = FindObjectOfType<InventoryController>();
            ISaveablePlayerData dataPlayer = FindObjectOfType<PJSMB>();
            ISaveableTimerData dataTimer = FindObjectOfType<GameManager>();

            SaveGame data = new SaveGame();
            data.PopulateDataMapaSalas(dataSalas);
            data.PopulateDataSalasBoss(dataBosses);
            data.PopulateDataPasilloTienda(dataTiendas);
            data.PopulateDataPasilloObjetos(dataPasilloObjetos);
            data.PopulateDataBackPack(dataBackpack);
            data.PopulateDataPlayer(dataPlayer);
            data.PopulateDataAbilities(dataAbilites);
            data.PopulateDataTimer(dataTimer);

            data.m_NameAndWorld = new NameAndWorld(GameManager.Instance.PlayerName, LevelManager.Instance.MundoActualJugador);


            try
            {
                string jsonDataLectura = File.ReadAllText(GameManager.Instance.RutaCompleta);
                SaveAllGames dataLectura = new SaveAllGames();
                JsonUtility.FromJsonOverwrite(jsonDataLectura, dataLectura);

                for (int i = 0; i < dataLectura.m_SavedGames.Length; i++)
                {
                    if (dataLectura.m_SavedGames[i].m_NameAndWorld.m_Name == GameManager.Instance.PlayerName)
                        dataLectura.m_SavedGames[i] = data;
                }

                string jsonData = JsonUtility.ToJson(dataLectura);
                File.WriteAllText(GameManager.Instance.RutaCompleta, jsonData);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while trying to save {Path.Combine(Application.persistentDataPath, GameManager.Instance.RutaCompleta)} with exception {e}");
            }
        }
        public void LoadData()
        {
            try
            {
                print("CargarPartida");
                SaveGame data = new SaveGame();
                string jsonDataLectura = File.ReadAllText(GameManager.Instance.RutaCompleta);
                SaveAllGames dataLectura = new SaveAllGames();
                JsonUtility.FromJsonOverwrite(jsonDataLectura, dataLectura);

                for (int i = 0; i < dataLectura.m_SavedGames.Length; i++)
                {
                    if (dataLectura.m_SavedGames[i].m_NameAndWorld.m_Name == GameManager.Instance.PlayerName)
                        data = dataLectura.m_SavedGames[i];
                }

                //JsonUtility.FromJsonOverwrite(jsonDataLectura, data);

                FindObjectOfType<GeneracionSalas.GeneracionSalasMatriz>().Load(data.m_Mapa);


                FindObjectOfType<GeneracionSalas.GeneracionSalaInstanciacion>().onMapaFinalized += SeguirElCargado;


            }
            catch (Exception e)
            {
                Debug.LogError($"Error while trying to load {Path.Combine(Application.persistentDataPath, GameManager.Instance.RutaCompleta)} with exception {e}");
            }
        }

        private void SeguirElCargado()
        {
            SaveGame data = new SaveGame();
            string jsonDataLectura = File.ReadAllText(GameManager.Instance.RutaCompleta);
            SaveAllGames dataLectura = new SaveAllGames();
            JsonUtility.FromJsonOverwrite(jsonDataLectura, dataLectura);

            for (int i = 0; i < dataLectura.m_SavedGames.Length; i++)
            {
                if (dataLectura.m_SavedGames[i].m_NameAndWorld.m_Name == GameManager.Instance.PlayerName)
                    data = dataLectura.m_SavedGames[i];
            }

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

            FindObjectOfType<InventoryController>().Load(data.m_BackPack, false);
            PJSMB player = FindObjectOfType<PJSMB>();
            player.Load(data.m_PlayerStats);

            FindObjectOfType<GameManager>().Load(data.m_PlayerTimer);

            /*AbilitySlotBehaviour[] abilities = FindObjectsByType<AbilitySlotBehaviour>(FindObjectsSortMode.None);
            for(int i = 0; i < abilities.Length; i++)
            {
                foreach(SaveGame.PlayerAbilities ability in data.m_PlayerAbilities)
                {
                    if(ability.m_ButtonId == abilities[i].ButtonId)
                    {
                        abilities[i].Load(ability);
                    }
                }
            }*/
            FindObjectOfType<AbilitiesGUIController>().Load(data.m_PlayerAbilities, false);
            GameManager.Instance.SetNamePlayer(data.m_NameAndWorld.m_Name);
        }
        public void LoadDataBetweenScenes()
        {
            try
            {
                print("CargarPartidaEntreEscenas");
                SaveGame data = new SaveGame();
                string jsonDataLectura = File.ReadAllText(GameManager.Instance.RutaCompleta);
                SaveAllGames dataLectura = new SaveAllGames();
                JsonUtility.FromJsonOverwrite(jsonDataLectura, dataLectura);

                for (int i = 0; i < dataLectura.m_SavedGames.Length; i++)
                {
                    if (dataLectura.m_SavedGames[i].m_NameAndWorld.m_Name == GameManager.Instance.PlayerName)
                        data = dataLectura.m_SavedGames[i];
                }

                //JsonUtility.FromJsonOverwrite(jsonDataLectura, data);

                FindObjectOfType<AbilitiesGUIController>().Load(data.m_PlayerAbilities, true);

                FindObjectOfType<InventoryController>().Load(data.m_BackPack, true);

            }
            catch (Exception e)
            {
                Debug.LogError($"Error while trying to load {Path.Combine(Application.persistentDataPath, GameManager.Instance.RutaCompleta)} with exception {e}");
            }
        }

        public void SaveTimerRanking()
        {
            ISaveableRecordTimerData m_TimerRanking = FindObjectOfType<GameManager>();
            SaveRecordTimer data = new();
            data.PopulateDataRecordTimer(m_TimerRanking);

            try
            {
                string jsonDataLectura = File.ReadAllText(GameManager.Instance.RutaCompletaRanquing);
                SaveAllRanquing ranquingTotal = new();
                JsonUtility.FromJsonOverwrite(jsonDataLectura, ranquingTotal);

                List<SaveRecordTimer> ranquings;
                if (ranquingTotal.m_SavedRanquings != null)
                {
                     ranquings = ranquingTotal.m_SavedRanquings.ToList();
                }
                else
                {
                    ranquings = new();
                }
               
                ranquings.Add(data);
                ranquingTotal.m_SavedRanquings = ranquings.ToArray();
                string jsonData = JsonUtility.ToJson(ranquingTotal);
                File.WriteAllText(GameManager.Instance.RutaCompletaRanquing, jsonData);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError($"Error while trying to save {GameManager.Instance.RutaCompletaRanquing} with exception {e}");
            }
        }

        /* 
         public void SaveDataPreStes()
         {
             ISaveablePreSetsData dataPreSets = FindObjectOfType<GameManager>();
             SavePreSets data = new();
             data.PopulateDataPreSets(dataPreSets);
         }
        */
    }

}
