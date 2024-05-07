using SaveLoadGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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


        void Start()
        {
            rutaCompleta = Path.Combine(Application.dataPath, "Language", languagesFile);
            getIdioma();
        }

        public void getIdioma()
        {
            string jsonDataLectura = File.ReadAllText(rutaCompleta);
            Multilanguage multilanguage = new Multilanguage();
            JsonUtility.FromJsonOverwrite(jsonDataLectura, multilanguage);

            for (int i = 0; i < multilanguage.m_Idiomas.Length; i++)
            {
                if (multilanguage.m_Idiomas[i].id == GameManager.Instance.IdiomaJuego.ToString())
                {
                    m_IdiomaActual = multilanguage.m_Idiomas[i];
                }
            }
            Consumable consumable = m_ConsumablesList.GetItemByID("0");
            consumable.itemName = m_IdiomaActual.consumiblesDatos[0].name;
            cambioIdiomaEvent.Raise();

            //GameManager.Instance.IdiomaJuego;
            /*Multilanguage multilanguage = new Multilanguage();
            Multilanguage.Idioma lengua = new Multilanguage.Idioma();
             lengua.id = IdiomaEnum.ES.ToString();
             Multilanguage.PiccoloChanFrases piccoloFrases = new();
             string[] frases = new string[]
             {
                 "eo",
                 "oe"
             };

             piccoloFrases.frasesIniciales = frases;
             lengua.frasesPiccoloChan = piccoloFrases;
             Multilanguage.Idioma lengua2 = new Multilanguage.Idioma();
             lengua2.id = IdiomaEnum.EN.ToString();
             Multilanguage.PiccoloChanFrases piccoloPhrases = new();
             string[] phrases = new string[]
            {
                 "eiou",
                 "oue"
            };
             piccoloPhrases.frasesIniciales = phrases;
             lengua2.frasesPiccoloChan = piccoloPhrases;
             Multilanguage.Idioma[] lenguas = new Multilanguage.Idioma[2];
            lenguas[0] = lengua;
            lenguas[1] = lengua2;
            multilanguage.m_Idiomas = lenguas;
            string jsonData = JsonUtility.ToJson(multilanguage);
            File.WriteAllText(rutaCompleta, jsonData);*/
        }

        public Multilanguage.PiccoloChanFrases GetPiccoloChadFrases()
        {
            Multilanguage.PiccoloChanFrases piccoloFrases = m_IdiomaActual.frasesPiccoloChan;

            return piccoloFrases;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}