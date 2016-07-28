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
    Popup,
}

public class MenuScript : MonoBehaviour
{

    public new AudioSource audio;
    public Button thisButton;
    public MenuState state;
    public GameObject questChoiceHolder;
    public GameObject questCurrentHolder;
    public GameObject questsOnHoldHolder;
    public GameObject currentQuestGrid;
    public GameObject holdQuestGrid;

    public GameObject choiceMenu;
    public GameObject popUp;
    public Text popupTitle;
    public Text popupText;

    public string localPopupTitle;
    public string localPopupText;
    public QUEST.QuestReader questReader;

    private bool questChoiceHolderActive    = false;
    private bool questCurrentHolderActive   = false;
    private bool choiceMenuActive           = false;
    private bool holdActive                 = false;
    private bool popupActive                = false;

    public int holdSlots = 3;


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
                thisButton.onClick.AddListener( ( ) => OpenChoiceMenu( ));
                updateAllMenus( );
                break;
            
            case MenuState.Choice:
                choiceMenuActive = true;
                questCurrentHolderActive = false;
                questChoiceHolderActive = false;
                popupActive = false;
                holdActive = false;
                thisButton.onClick.AddListener( ( ) => NoMenu( ) );
                updateAllMenus( );
                break;
            
            case MenuState.NewQuest:
                choiceMenuActive = false;
                questCurrentHolderActive = false;
                questChoiceHolderActive = true;
                popupActive = false;
                holdActive                 = false;
                thisButton.onClick.AddListener( ( ) => NoMenu( ) );
                updateAllMenus( );
                break;
            
            case MenuState.CurrentQuest:
                choiceMenuActive = false;
                questCurrentHolderActive = true;
                questChoiceHolderActive = false;
                popupActive = false;
                holdActive = false;
                thisButton.onClick.AddListener( ( ) => NoMenu( ) );
                updateAllMenus( );
                break;
            case MenuState.Hold:
                choiceMenuActive = false;
                questCurrentHolderActive = false;
                questChoiceHolderActive = false;
                popupActive = false;
                holdActive = true;
                thisButton.onClick.AddListener( ( ) => NoMenu( ) );
                updateAllMenus( );
                break;
            case MenuState.Popup:
                choiceMenuActive = false;
                questCurrentHolderActive = false;
                questChoiceHolderActive = false;
                popupActive = true;
                holdActive = false;
                thisButton.onClick.AddListener( ( ) => NoMenu( ) );
                updateAllMenus( );
                popupTitle.text = localPopupTitle;
                popupText.text = localPopupText;
                break;
            
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
        audio.Play( );
        state = MenuState.CurrentQuest; 
    }
    public void OpenQuestsOnHold()
    {
        audio.Play( );
        state = MenuState.Hold; 
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
        questCurrentHolder.SetActive( questCurrentHolderActive );
        questChoiceHolder.SetActive( questChoiceHolderActive );
        popUp.SetActive( popupActive );
        questsOnHoldHolder.SetActive( holdActive );
    }

    public void printMessage()
    {
        Debug.Log("test test");
    }
}
