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

public class QuestReader : MonoBehaviour
{
    public Text textField;
    public GameObject grid;
    public QuestHolder[] holder;
    public QuestDirectory directory;
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

    public class Quest
    {
        public int questID { get; set; }
        public string questTitle { get; set; }
        public string questDesc { get; set; }

        public string questFinish { get; set;}

        public int thriftinessPoints { get; set; }
        public int fitnessPoints { get; set; }
        public int happinessPoints { get; set; }
        public int healthPoints { get; set; }
        public int reputationPoints { get; set; }
        public int wisdomPoints { get; set; }
        public int pawprintPoints { get; set; }
        
    }

    public void Start ()
    {
        holder = grid.GetComponentsInChildren<QuestHolder>();
        Deserialize( );
        
    }
    /*static public void Serialize ( Quest details )
    {
        XmlSerializer serializer = new XmlSerializer( typeof( Quest ) );
        using (TextWriter writer = new StreamWriter( @"Assets/QuestTest.xml" ))
        {
            serializer.Serialize( writer, details );
        }
    }  */

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PickNextQuestInDirectory();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            PickRandomQuest( );
        }
    }


    public void Deserialize()
    {

        StartCoroutine( "DeserializeXML" );

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
        xml.Read();
        Debug.Log("What:"+www.text);
        object obj = deserializer.Deserialize( xml );
        QuestDirectory XmlData = (QuestDirectory)obj;
        directory = XmlData;
        PickRandomQuest( );
        if(DEBUG != null) DEBUG.color = Color.green;
        //reader.Close( );
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

    public void PickRandomQuest()
    {  
        
        foreach (QuestHolder qh in holder)
        {
            int random = Mathf.RoundToInt( UnityEngine.Random.Range( 0, directory.questList.Count ) );
          
            current = random;
            UpdateTextValue( qh, current );
        }
    }
    public void PickNextQuestInDirectory ()
    { 
        
        foreach(QuestHolder qh in holder)
        {
            if (current < directory.questList.Count - 1)
            {
                current++;
            }
            else
            {
                current = 0;
            }
            UpdateTextValue( qh , current );
            
        }
        
    }

    private void UpdateTextValue(QuestHolder holderNr, int nr)
    {
        holderNr.UpdateValues(  directory.questList[nr].questID,
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
}
