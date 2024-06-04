using SaveLoadGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace multilanguaje
{
    public class MultiLanguageManager : MonoBehaviour
    {
        private const string languagesFile = "Languages.txt";
        private string rutaCompleta;
        private string rutaCompletaHastaCarpeta;
        [SerializeField] private Multilanguage.Idioma m_IdiomaActual;
        [SerializeField] private GameEvent cambioIdiomaEvent;
        [SerializeField] private ConsumablesDataBase m_ConsumablesList;
        [SerializeField] private EquipableDataBase m_EquipablesList;
        [SerializeField] private AbilityDataBase m_AbilitiesList;


        void Start()
        {
            rutaCompleta = Path.Combine(Application.streamingAssetsPath, "Language", languagesFile);
            getIdioma();
        }

        public void getIdioma()
        {
            string jsonDataLectura = File.ReadAllText(rutaCompleta);
            Multilanguage multilanguage = new Multilanguage();
            JsonUtility.FromJsonOverwrite(jsonDataLectura, multilanguage);

            for (int i = 0; i < multilanguage.m_Idiomas.Length - 1; i++)
            {
                if (multilanguage.m_Idiomas[i].id == GameManager.Instance.IdiomaJuego.ToString())
                {
                    m_IdiomaActual = multilanguage.m_Idiomas[i];
                }
            }

            for(int i = 0; i < m_IdiomaActual.consumiblesDatos.Length; i++)
            {
                Consumable consumableScriptable = m_ConsumablesList.GetItemByID(m_IdiomaActual.consumiblesDatos[i].id);
                consumableScriptable.itemName = m_IdiomaActual.consumiblesDatos[i].name;
                consumableScriptable.description = m_IdiomaActual.consumiblesDatos[i].descripcion;
            }

            for (int i = 0; i < m_IdiomaActual.equipablesDatos.Length; i++)
            {
                Equipable equipableScriptable = m_EquipablesList.GetItemByID(m_IdiomaActual.equipablesDatos[i].id);
                if(m_IdiomaActual.equipablesDatos[i].name != null)
                    equipableScriptable.itemName = m_IdiomaActual.equipablesDatos[i].name;
                if (m_IdiomaActual.equipablesDatos[i].descripcion != null)
                    equipableScriptable.description = m_IdiomaActual.equipablesDatos[i].descripcion;
            }
            for (int i = 0; i < m_IdiomaActual.habilidadesDatos.Length; i++)
            {
                Ability abilityScriptable = m_AbilitiesList.GetItemByID(m_IdiomaActual.habilidadesDatos[i].id);
                    abilityScriptable.abilityName = m_IdiomaActual.habilidadesDatos[i].name;
                    abilityScriptable.abilityDescription = m_IdiomaActual.habilidadesDatos[i].descripcion;
            }
            ChangeLanguage(GameManager.Instance.IdiomaJuego.ToString().ToLower());
            cambioIdiomaEvent.Raise();
        }

        public Multilanguage.PiccoloChanFrases GetPiccoloChadFrases()
        {
            Multilanguage.PiccoloChanFrases piccoloFrases = m_IdiomaActual.frasesPiccoloChan;

            return piccoloFrases;
        }

        public void ChangeLanguage(string localeCode)
        {
            StartCoroutine(SetLocale(localeCode));
        }

        private IEnumerator SetLocale(string localeCode)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales.Find(l => l.Identifier.Code == localeCode);

            if (locale != null)
            {
                yield return LocalizationSettings.InitializationOperation; 
                LocalizationSettings.SelectedLocale = locale;
            }
            else
            {
                Debug.LogWarning("Locale not found: " + localeCode);
            }
        }
    }
}