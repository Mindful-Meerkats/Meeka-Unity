using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System;
using LitJson;

namespace QUEST
{
    public class QuestReader : MonoBehaviour
    {

        public string jsonFile;
        public JsonData questData;

        public GameObject loadingImage;

        public Text textField;
        public GameObject grid;
        public QuestHolder[] holder;
        public QuestDirectory directory;
        public QuestDirectoryJSON directoryJSON;
        public int current = 0;
        private TextReader reader;
        

        public Color test;

        string path;

        public Image DEBUG;
        public Text DEBUGTEXT;

        public class QuestDirectory
        {
            [XmlElement( "Quest" )]
            public List<Quest> questList = new List<Quest>( );
        }
        public class QuestDirectoryJSON
        {
            public List<Quest> questList = new List<Quest>();
        }

        public class Quest
        {
            public int questID { get; set; }
            public string questTitle { get; set; }
            public string questDesc { get; set; }

            public string questFinish { get; set; }

            public int thriftinessPoints { get; set; }
            public int fitnessPoints { get; set; }
            public int happinessPoints { get; set; }
            public int healthPoints { get; set; }
            public int reputationPoints { get; set; }
            public int wisdomPoints { get; set; }
            public int pawprintPoints { get; set; }

        }

        public void Start ( )
        {
            holder = grid.GetComponentsInChildren<QuestHolder>( );
            Deserialize( );
            if(!loadingImage.activeSelf)loadingImage.SetActive(true);

        }
        /*static public void Serialize ( Quest details )
        {
            XmlSerializer serializer = new XmlSerializer( typeof( Quest ) );
            using (TextWriter writer = new StreamWriter( @"Assets/QuestTest.xml" ))
            {
                serializer.Serialize( writer, details );
            }
        }  */

        public void Update ( )
        {
            if (Input.GetKeyDown( KeyCode.Space ))
            {
                PickNextQuestInDirectory( );
            }
            if (Input.GetKeyDown( KeyCode.R ))
            {
                PickRandomQuest( );
            }
        }


        public void Deserialize ( )
        {

            StartCoroutine( "DeserializeJSON" );
            //StartCoroutine( "DeserializeXML" );

        }

        IEnumerator DeserializeXML ( )
        {
            string filePath = "https://dl.dropboxusercontent.com/u/107094743/QuestTest.xml";
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            WWW www = new WWW( filePath );
            XmlSerializer deserializer = new XmlSerializer( typeof( QuestDirectory ) );

            yield return www;
            XmlDocument result = new XmlDocument( );

            if (DEBUGTEXT != null) DEBUGTEXT.text = result.InnerXml;
            //TextReader reader = new StreamReader( filePath );
            XmlReader xml = XmlReader.Create( filePath );
            xml.Read( );
            Debug.Log( "What:" + www.text );
            object obj = deserializer.Deserialize( xml );
            Debug.Log(obj);
            QuestDirectory XmlData = (QuestDirectory)obj;
            directory = XmlData;
            PickRandomQuest( );
            if (DEBUG != null) DEBUG.color = Color.green;
            //reader.Close( );
        }


        //Deserialize JSON from a public dropbox folder.
        IEnumerator DeserializeJSON ( )
        {
            string filePath = "https://dl.dropboxusercontent.com/u/107094743/QuestJSON.json";
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            WWW www = new WWW( filePath );

            yield return www;
            loadingImage.SetActive( false );
            jsonFile = www.text;

            //Store a local QuestDirectoryJSON
            QuestDirectoryJSON newDirectoryJSON = JsonMapper.ToObject<QuestDirectoryJSON>( jsonFile );
            JsonData data = JsonMapper.ToObject( jsonFile );
            
            

            for(int ii = 0; ii < data[0].Count; ii++)
            {
                //Debug.Log( "Data from JSON file: " + data[0][ii] );
                Quest newQuest = new Quest();
                newQuest.questID = Int32.Parse( (string)data[0][ii]["questID"]);
                newQuest.questTitle = (string)data[0][ii]["questTitle"];
                newQuest.questDesc = (string)data[0][ii]["questDesc"];
                newQuest.questFinish = (string)data[0][ii]["questFinish"];
                newQuest.thriftinessPoints = Int32.Parse( (string)data[0][ii]["thriftinessPoints"]);
                newQuest.fitnessPoints = Int32.Parse( (string)data[0][ii]["fitnessPoints"]);
                newQuest.happinessPoints = Int32.Parse( (string)data[0][ii]["happinessPoints"]);
                newQuest.healthPoints = Int32.Parse( (string)data[0][ii]["healthPoints"]);
                newQuest.reputationPoints = Int32.Parse( (string)data[0][ii]["reputationPoints"]);
                newQuest.wisdomPoints = Int32.Parse( (string)data[0][ii]["wisdomPoints"]);
                newQuest.pawprintPoints = Int32.Parse( (string)data[0][ii]["pawprintPoints"]);

                newDirectoryJSON.questList.Add(newQuest);

            }
            directoryJSON = newDirectoryJSON;
            PickRandomQuest( );
        }




        public bool MyRemoteCertificateValidationCallback ( System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors )
        {
            bool isOk = true;
            // If there are errors in the certificate chain, look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                        chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan( 0, 1, 0 );
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                        bool chainIsValid = chain.Build( (X509Certificate2)certificate );
                        if (!chainIsValid)
                        {
                            isOk = false;
                        }
                    }
                }
            }
            return isOk;
        }

        public void PickRandomQuest ()
        {
            //Debug.Log("Picking random quest. "+"Json quest directory available?: " + directoryJSON);

            foreach (QuestHolder qh in holder)
            {
                int random = 0;
                //if (directory != null)
                //    random = Mathf.RoundToInt( UnityEngine.Random.Range( 0, directory.questList.Count ) );
                if (directoryJSON != null)
                {
                    random = Mathf.RoundToInt( UnityEngine.Random.Range( 0, directoryJSON.questList.Count ) );
                    Debug.Log("Current random quest: "+ directoryJSON.questList[random].questTitle);
                }
                current = random;
                UpdateTextValue( qh, current );
            }
        }
        public void PickNextQuestInDirectory ( )
        {

            foreach (QuestHolder qh in holder)
            {
                if (directory != null)
                {
                    if (current < directory.questList.Count - 1)
                    {
                        current++;
                    }
                    else
                    {
                        current = 0;
                    }
                    UpdateTextValue( qh, current );
                }
                if (directoryJSON != null)
                {
                    if (current < directoryJSON.questList.Count - 1)
                    {
                        current++;
                    }
                    else
                    {
                        current = 0;
                    }
                    UpdateTextValue( qh, current );
                }

            }

        }

        private void UpdateTextValue ( QuestHolder holderNr, int nr )
        {
            if (directory != null)
            {
                holderNr.UpdateValues( directory.questList[nr].questID,
                                      directory.questList[nr].questTitle,
                                      directory.questList[nr].questDesc,
                                      directory.questList[nr].questFinish,
                                      directory.questList[nr].thriftinessPoints,
                                      directory.questList[nr].fitnessPoints,
                                      directory.questList[nr].happinessPoints,
                                      directory.questList[nr].healthPoints,
                                      directory.questList[nr].reputationPoints,
                                      directory.questList[nr].wisdomPoints,
                                      directory.questList[nr].pawprintPoints );
                holderNr.UpdateInfo( );
            }
            if (directoryJSON != null)
            {
                holderNr.UpdateValues( directoryJSON.questList[nr].questID,
                                      directoryJSON.questList[nr].questTitle,
                                      directoryJSON.questList[nr].questDesc,
                                      directoryJSON.questList[nr].questFinish,
                                      directoryJSON.questList[nr].thriftinessPoints,
                                      directoryJSON.questList[nr].fitnessPoints,
                                      directoryJSON.questList[nr].happinessPoints,
                                      directoryJSON.questList[nr].healthPoints,
                                      directoryJSON.questList[nr].reputationPoints,
                                      directoryJSON.questList[nr].wisdomPoints,
                                      directoryJSON.questList[nr].pawprintPoints );
                holderNr.UpdateInfo( );
            }
        }
    }
}
