using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum MenuState
{
    NoMenu,
    Choice,
    NewQuest,
    CurrentQuest,
    Hold,
    Block,
    Popup,
}

public class MenuScript : MonoBehaviour
{

    public new AudioSource audio;
    public Button thisButton;
    public MenuState state;
    public GameObject questsChoiceHolder;
    public GameObject questsCurrentHolder;
    public GameObject questsOnHoldHolder;
    public GameObject questsBlockedHolder;
    public GameObject currentQuestGrid;
    public GameObject holdQuestGrid;
    public GameObject blockedQuestGrid;

    public GameObject choiceMenu;
    public GameObject popUp;
    public Text popupTitle;
    public Text popupText;

    public string localPopupTitle;
    public string localPopupText;
    public QUEST.QuestReader questReader;

    private bool questChoiceHolderActive    = false;
    private bool questCurrentHolderActive   = false;
    private bool questBlockedHolderActive   = false;
    private bool choiceMenuActive           = false;
    private bool holdActive                 = false;
    private bool blockActive                = false;
    private bool popupActive                = false;

    private MainBehaviour main;

    public void Start()
    {
        main = FindObjectOfType<MainBehaviour>( );
    }


    public void Update()
    {
        switch(state)
        {
            case MenuState.NoMenu:
                choiceMenuActive            = false;
                questCurrentHolderActive    = false;
                questChoiceHolderActive     = false;
                popupActive                 = false;
                holdActive                  = false;
                blockActive                 = false;
                thisButton.onClick.AddListener( ( ) => OpenChoiceMenu( ));
                updateAllMenus( );
                break;
            
            case MenuState.Choice:
                choiceMenuActive            = true;
                questCurrentHolderActive    = false;
                questChoiceHolderActive     = false;
                popupActive                 = false;
                holdActive                  = false;
                blockActive                 = false;
                thisButton.onClick.AddListener( ( ) => NoMenu( ) );
                updateAllMenus( );
                break;
            
            case MenuState.NewQuest:
                choiceMenuActive            = false;
                questCurrentHolderActive    = false;
                questChoiceHolderActive     = true;
                popupActive                 = false;
                holdActive                  = false;
                blockActive                 = false;
                thisButton.onClick.AddListener( ( ) => NoMenu( ) );
                updateAllMenus( );
                break;
            
            case MenuState.CurrentQuest:
                choiceMenuActive            = false;
                questCurrentHolderActive    = true;
                questChoiceHolderActive     = false;
                popupActive                 = false;
                holdActive                  = false;
                blockActive                 = false;
                thisButton.onClick.AddListener( ( ) => NoMenu( ) );
                updateAllMenus( );
                break;
            case MenuState.Hold:
                choiceMenuActive            = false;
                questCurrentHolderActive    = false;
                questChoiceHolderActive     = false;
                popupActive                 = false;
                holdActive                  = true;
                blockActive                 = false;
                thisButton.onClick.AddListener( ( ) => NoMenu( ) );
                updateAllMenus( );
                break;
            case MenuState.Block:
                choiceMenuActive            = false;
                questCurrentHolderActive    = false;
                questChoiceHolderActive     = false;
                popupActive                 = false;
                holdActive                  = false;
                blockActive                 = true;
                thisButton.onClick.AddListener( ( ) => NoMenu( ) );
                updateAllMenus( );
                break;
            case MenuState.Popup:
                choiceMenuActive            = false;
                questCurrentHolderActive    = false;
                questChoiceHolderActive     = false;
                popupActive                 = true;
                holdActive                  = false;
                blockActive                 = false;
                thisButton.onClick.AddListener( ( ) => NoMenu( ) );
                updateAllMenus( );
                popupTitle.text = localPopupTitle;
                popupText.text = localPopupText;
                break;
            
        }
    }

    public void incrementHoldSlots ( int value = 1)
    {
        if (main.scorePoints >= main.holdSlotValue)
        {
            main.holdSlots += value;
            main.scorePoints -= main.holdSlotValue;
            main.updateScorePoints( );
            Debug.Log( main.holdSlots );
        }
    }

    public void NoMenu()
    {
        audio.Play( );
        state = MenuState.NoMenu;   
    }

    public void Popup(string title,string txt)
    {
        audio.Play( );
        localPopupTitle = title; 
        localPopupText = txt;
        state = MenuState.Popup;
    }

    public void OpenChoiceMenu()
    {
        audio.Play( );
        state = MenuState.Choice;
    }

    public void OpenCurrentQuests()
    {
        if(currentQuestGrid.transform.childCount != 0)
        {
            audio.Play( );
            state = MenuState.CurrentQuest;
        } 
    }
    public void OpenQuestsOnHold()
    {
        if (holdQuestGrid.transform.childCount != 0)
        {
            audio.Play( );
            state = MenuState.Hold;
        }
    }
    public void OpenQuestsBlocked()
    {
        if (blockedQuestGrid.transform.childCount != 0)
        {
            audio.Play( );
            state = MenuState.Block;
        }
    }

    public void OpenQuestChoice()
    {
        audio.Play( );
        questReader.PickRandomQuest( );
        state = MenuState.NewQuest;  
    }

    private void updateAllMenus()
    {
        choiceMenu.SetActive( choiceMenuActive );
        questsCurrentHolder.SetActive( questCurrentHolderActive );
        questsBlockedHolder.SetActive( blockActive );
        questsChoiceHolder.SetActive( questChoiceHolderActive );
        popUp.SetActive( popupActive );
        questsOnHoldHolder.SetActive( holdActive );
    }

    public void printMessage()
    {
        Debug.Log("test test");
    }
}
