using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class QuestHolder : MonoBehaviour
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

    public Text titleText;
    public Text descriptionText;
    public GameObject newQuest;
    public GameObject newQuestOnHold;
    public GameObject newBlockedQuest;
    public QUEST.QuestReader.Quest thisQuest;

    public GameObject currentQuestGrid;
    public GameObject choiceQuestPanel;
    public GameObject currentQuestPanel;
    public GameObject holdQuestPanel;
    public GameObject blockedQuestPanel;
    public bool clicked = false;
    private MenuScript menu;
    private MainBehaviour main;
    public DateTime startTime;
    public DateTime endTime;    

    private bool test = false;
    TimeSpan timeRemaining = new TimeSpan();

    public void Awake()
    {   
        menu = FindObjectOfType<MenuScript>();
        main = FindObjectOfType<MainBehaviour>();
        currentQuestGrid = menu.currentQuestGrid;
        holdQuestPanel = menu.holdQuestGrid;
        blockedQuestPanel = menu.blockedQuestGrid;
    }

    public void UpdateValues(int id, string title,string desc, string finish, int thriftiness, int fitness, int happiness, int health, int reputation, int wisdom, int pawprint)
    {
        questID             = id;
        questTitle          = title;
        questDesc           = desc;
        questFinish         = finish;
        thriftinessPoints   = thriftiness;
        fitnessPoints       = fitness;
        happinessPoints     = happiness;
        healthPoints        = health;
        reputationPoints    = reputation;
        wisdomPoints        = wisdom; 
        pawprintPoints      = pawprint; 
    }

    public void UpdateInfo()
    {
        titleText.text = questTitle;
        descriptionText.text = questDesc;
    }

    public void AcceptQuest()
    {
        if (!clicked)
        {
            //Debug.Log( "Accepted quest.");
            if (newQuest != null)
            {
                GameObject GO = Instantiate( newQuest );
                GO.transform.SetParent( currentQuestGrid.transform );
                GO.transform.localScale = new Vector3( 1, 1, 1 );
                GO.GetComponent<QuestHolder>().UpdateValues(questID, questTitle, questDesc, questFinish, thriftinessPoints,fitnessPoints,happinessPoints,healthPoints,reputationPoints,wisdomPoints,pawprintPoints);
                GO.GetComponent<QuestHolder>().UpdateInfo();
                //Debug.Log( "Created quest: " + GO.GetComponent<QuestHolder>().questTitle);      
                //clicked = true;
                menu.state = MenuState.NoMenu;
                if(transform.name.Contains("Hold")) 
                {
                    Destroy( gameObject );
                    QUEST.QuestReader.acceptedQuests.Add( thisQuest );
                    QUEST.QuestReader.holdQuests.Remove( thisQuest );
                }
                else
                {
                    QUEST.QuestReader.acceptedQuests.Add( thisQuest );
                    QUEST.QuestReader.availableQuests.Remove( thisQuest );
                }

                GO.GetComponent<QuestHolder>( ).startTime = DateTime.Now;
                Debug.Log( GO.GetComponent<QuestHolder>( ).startTime );
                GO.GetComponent<QuestHolder>( ).endTime = DateTime.Now.AddHours(24);
                //GO.GetComponent<QuestHolder>( ).endTime = DateTime.Now.AddSeconds(10);
                Debug.Log( GO.GetComponent<QuestHolder>( ).endTime );
                GO.GetComponent<QuestHolder>( ).test = true;
                //GO.GetComponent<QuestHolder>( ).timeRemaining = endTime-startTime;


            }
        }
    }

    public void Update()
    {
        if (test)
        {
            InvokeRepeating("updateTimer",0,2);
            test = false;
            //timeRemaining = endTime.Subtract( startTime );// - startTime;
            //Debug.Log( timeRemaining );
        }
    }

    public void updateTimer()
    {
        timeRemaining = endTime - DateTime.Now;
        //Debug.Log( "Time left until expiration: " + timeRemaining.Seconds );
        if (DateTime.Now > endTime)
        {
            Debug.Log("QUEST EXPIRED.");
            
            ForfeitQuest();
            CancelInvoke( );
            Destroy(gameObject);
        }
        
    }

    public void DenyQuest()
    {
        //Debug.Log("Denied quest.");
        menu.state = MenuState.NoMenu;
    }
    public void HoldQuest()
    {
        //Debug.Log( "Quest on hold.");
        if (holdQuestPanel.transform.childCount < main.holdSlots)
        {
            if (newQuestOnHold != null)
            {
                GameObject GO = Instantiate( newQuestOnHold );
                GO.transform.SetParent( holdQuestPanel.transform );
                GO.transform.localScale = new Vector3( 1, 1, 1 );
                GO.GetComponent<QuestHolder>( ).UpdateValues( questID, questTitle, questDesc, questFinish, thriftinessPoints, fitnessPoints, happinessPoints, healthPoints, reputationPoints, wisdomPoints, pawprintPoints );
                GO.GetComponent<QuestHolder>( ).UpdateInfo( );
                //clicked = true;

                QUEST.QuestReader.holdQuests.Add( thisQuest );
                QUEST.QuestReader.availableQuests.Remove( thisQuest );

                Debug.Log("Amount of quests on hold: "+QUEST.QuestReader.holdQuests.Count + ", Amount of quests still available: " + QUEST.QuestReader.availableQuests.Count );
                menu.state = MenuState.NoMenu;
            }
        }
        else
        {
           main.infoScreen.SetActive( true );
        }
    }
    public void BlockQuest()
    {
        if (newBlockedQuest != null)
        {
            Debug.Log("Added a quest to the blocked list.");
            GameObject GO = Instantiate( newBlockedQuest );
            GO.transform.SetParent( blockedQuestPanel.transform );
            GO.transform.localScale = new Vector3( 1, 1, 1 );
            GO.GetComponent<QuestHolder>( ).UpdateValues( questID, questTitle, questDesc, questFinish, thriftinessPoints, fitnessPoints, happinessPoints, healthPoints, reputationPoints, wisdomPoints, pawprintPoints );
            GO.GetComponent<QuestHolder>( ).UpdateInfo( );
            //clicked = true;

            QUEST.QuestReader.blockedQuests.Add( thisQuest );
            QUEST.QuestReader.availableQuests.Remove( thisQuest );
            menu.state = MenuState.NoMenu;
        }

    }
    public void UnblockQuest()
    {
        QUEST.QuestReader.availableQuests.Add( thisQuest );
        QUEST.QuestReader.blockedQuests.Remove( thisQuest );
        
        menu.state = MenuState.NoMenu;
        Destroy( gameObject );
    }

    public void CompleteQuest()
    {
        menu.Popup(questTitle,questFinish);
        main.AddScore(thriftinessPoints,fitnessPoints,happinessPoints,healthPoints,reputationPoints,wisdomPoints,pawprintPoints);
        
        QUEST.QuestReader.availableQuests.Add( thisQuest );
        QUEST.QuestReader.acceptedQuests.Remove( thisQuest );
        Debug.Log(QUEST.QuestReader.availableQuests.Count);

        Destroy(gameObject);
    }

    public void ForfeitQuest()
    {
        main.AddScore( -thriftinessPoints, -fitnessPoints, -happinessPoints, -healthPoints, -reputationPoints, -wisdomPoints, -pawprintPoints);
        menu.state = MenuState.NoMenu;
        QUEST.QuestReader.availableQuests.Add( thisQuest );
        QUEST.QuestReader.acceptedQuests.Remove( thisQuest );
        Destroy( gameObject );
    }
}
