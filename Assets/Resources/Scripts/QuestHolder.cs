using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    public GameObject currentQuestGrid;
    public GameObject choiceQuestPanel;
    public GameObject currentQuestPanel;
    public bool clicked = false;
    private MenuScript menu;
    private MainBehaviour main;

    public void Awake()
    {   
        menu = FindObjectOfType<MenuScript>();
        main = FindObjectOfType<MainBehaviour>();
        currentQuestGrid = menu.currentQuestGrid;
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
            Debug.Log( "Accepted quest: " + questTitle + ", finish: "+questFinish );
            if (newQuest != null)
            {
                GameObject GO = Instantiate( newQuest );
                GO.transform.SetParent( currentQuestGrid.transform );
                GO.transform.localScale = new Vector3( 1, 1, 1 );
                GO.GetComponent<QuestHolder>().UpdateValues(questID, questTitle, questDesc, questFinish, thriftinessPoints,fitnessPoints,happinessPoints,healthPoints,reputationPoints,wisdomPoints,pawprintPoints);
                GO.GetComponent<QuestHolder>().UpdateInfo();
                Debug.Log( "Created quest: " + GO.GetComponent<QuestHolder>().questTitle);      
                //clicked = true;
                menu.state = MenuState.NoMenu;
            }
        }
    }
    public void DenyQuest()
    {
        Debug.Log("Denied quest: " + questTitle);
        menu.state = MenuState.NoMenu;
    }

    public void CompleteQuest()
    {
        Debug.Log("Completed quest: " + questTitle);
        Debug.Log("questFinish: "+questFinish);
        menu.Popup(questTitle,questFinish);
        main.AddScore(thriftinessPoints,fitnessPoints,happinessPoints,healthPoints,reputationPoints,wisdomPoints,pawprintPoints);
        Destroy(gameObject);
    }

    public void ForfeitQuest()
    {
        Debug.Log("Forfeited quest: " + questTitle);
        main.AddScore( -thriftinessPoints, -fitnessPoints, -happinessPoints, -healthPoints, -reputationPoints, -wisdomPoints, -pawprintPoints);
        menu.state = MenuState.NoMenu;
        Destroy( gameObject );
    }
}
