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

        private MenuScript menu;

        public static List< Quest > availableQuests = new List<Quest>( );

        public static List< Quest > acceptedQuests  = new List<Quest>( );
        
        public static List< Quest > blockedQuests   = new List<Quest>( );

        public static List< Quest > holdQuests      = new List<Quest>( );


        public string jsonFile;
        public string jsonFilePath;
        public JsonData questData;

        public GameObject loadingImage;

        public Text textField;
        public GameObject grid;
        public QuestHolder[] holder;
        public QuestDirectory directory;

        public int current = 0;        

        public Color test;
        public Image DEBUG;
        public Text DEBUGTEXT;

        public class QuestDirectory
        {
            [XmlElement( "Quest" )]
            public List<Quest> questList = new List<Quest>( );
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
            menu = FindObjectOfType<MenuScript>( );
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
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            WWW www = new WWW( jsonFilePath );

            yield return www;
            loadingImage.SetActive( false );
            jsonFile = www.text;

            JsonData data = JsonMapper.ToObject( jsonFile );
            
            

            for(int ii = 0; ii < data[0].Count; ii++)
            {
                //Debug.Log( "Data from JSON file: " + data[0][ii] );
                Quest newQuest = new Quest();
                newQuest.questID =              int.Parse( (string)data[0][ii]["questID"]);
                newQuest.questTitle =           (string)data[0][ii]["questTitle"];
                newQuest.questDesc =            (string)data[0][ii]["questDesc"];
                newQuest.questFinish =          (string)data[0][ii]["questFinish"];
                newQuest.thriftinessPoints =    int.Parse( (string)data[0][ii]["thriftinessPoints"]);
                newQuest.fitnessPoints =        int.Parse( (string)data[0][ii]["fitnessPoints"]);
                newQuest.happinessPoints =      int.Parse( (string)data[0][ii]["happinessPoints"]);
                newQuest.healthPoints =         int.Parse( (string)data[0][ii]["healthPoints"]);
                newQuest.reputationPoints =     int.Parse( (string)data[0][ii]["reputationPoints"]);
                newQuest.wisdomPoints =         int.Parse( (string)data[0][ii]["wisdomPoints"]);
                newQuest.pawprintPoints =       int.Parse( (string)data[0][ii]["pawprintPoints"]);

                availableQuests.Add( newQuest );

            }

            //foreach (Quest quest in availableQuests)
            //{
            //    Debug.Log( quest.questID );
            //}
            //PickRandomQuest( );
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

        private string debug = "";

        public void PickRandomQuest ()
        {
            //Debug.Log("Picking random quest. "+"Json quest directory available?: " + directoryJSON);

            foreach (QuestHolder qh in holder)
            {
                int random = 0;
                if ( availableQuests.Count >= 1 )
                {
                    random = Mathf.RoundToInt( UnityEngine.Random.Range( 1, availableQuests.Count ) );
                    Debug.Log("RANDOM: "+random + ", Quest count: "+availableQuests.Count);

                    //Debug.Log("QuestID:"+availableQuests[random].questID);
                    if (availableQuests.Count > 1)
                    {
                        UpdateTextValue( qh, random, availableQuests[random] );
                        current = random;
                        debug+=availableQuests.Count;
                        Debug.Log(debug);
                    }
                    else
                    {
                        UpdateTextValue( qh, 1, availableQuests[1] );
                    }
                    Debug.Log( "Current random quest: " + availableQuests[current].questTitle );
                } 
                else
                {
                    menu.Popup("All out!","No more quests available!");
                }   
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
                    UpdateTextValue( qh, current, availableQuests[current] );
                }
                if (current < availableQuests.Count - 1)
                {
                    current++;
                }
                else
                {
                    current = 0;
                }
                UpdateTextValue( qh, current, availableQuests[current] );

            }

        }

        private void UpdateTextValue ( QuestHolder holderNr, int nr, Quest selectedQuest )
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
                holderNr.thisQuest = selectedQuest;
                holderNr.UpdateInfo( );
            }
            if (availableQuests.Count != 0 && holderNr != null)
            {
                holderNr.UpdateValues( availableQuests[nr].questID,
                                      availableQuests[nr].questTitle,
                                      availableQuests[nr].questDesc,
                                      availableQuests[nr].questFinish,
                                      availableQuests[nr].thriftinessPoints,
                                      availableQuests[nr].fitnessPoints,
                                      availableQuests[nr].happinessPoints,
                                      availableQuests[nr].healthPoints,
                                      availableQuests[nr].reputationPoints,
                                      availableQuests[nr].wisdomPoints,
                                      availableQuests[nr].pawprintPoints );
                holderNr.thisQuest = selectedQuest;
                holderNr.UpdateInfo( );
            }
        }
    }
}
