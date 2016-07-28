using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;


public class MainBehaviour : MonoBehaviour
{
    public AudioSource pointAudio;
    public AudioClip pointUp;
    public AudioClip pointDown;

    public Image meekaThriftiness;
    public Image meekaFitness;
    public Image meekaHappiness;
    public Image meekaHealth;
    public Image meekaReputation;
    public Image meekaWisdom;
    public Image pawprintFrontImage;
    public Image pawprintBackImage;
    public Image pawprintHillImage;


    public Sprite fitnessPlus;         
    public Sprite fitness0;         
    public Sprite fitnessMinus;     
    
    [Space(5)]

    public Sprite happinessPlus;
    public Sprite happiness;
    public Sprite happinessMinus;

    [Space(5)]

    public Sprite healthPlus;
    public Sprite healthMinus;
    public Sprite healthMinus1;
    public Sprite healthMinus2;
    public Sprite healthMinus3;

    [Space(5)]

    public Sprite hillPlus;
    public Sprite hill;
    public Sprite hillMinus;

    [Space(5)]

    public Sprite pawprintBackPlus;
    public Sprite pawprintBack;
    public Sprite pawprintBackMinus;

    [Space(5)]

    public Sprite pawprintFrontPlus;
    public Sprite pawprintFront;
    public Sprite pawprintFrontMinus;

    [Space(5)]

    public Sprite reputationMinus;
    public Sprite reputationPlus1;
    public Sprite reputationPlus2;
    public Sprite reputationPlus3;

    [Space(5)]

    public Sprite thriftinessPlus;
    public Sprite thriftiness;
    public Sprite thriftinessMinus;

    [Space(5)]

    public Sprite wisdomPlus1;
    public Sprite wisdomPlus2;
    public Sprite wisdomPlus3;
    public Sprite wisdomPlus4;
    public Sprite wisdomPlus5;
    public Sprite wisdom;
    public Sprite wisdomMinus1;
    public Sprite wisdomMinus2;
    public Sprite wisdomMinus3;

    [Space(5)]
    public bool thriftMode;
    public Sprite thriftSweat;
    public Sprite FitnessAura;

    [Space(5)]
    public int thriftinessScore = 0;
    public int fitnessScore = 0;
    public int happinessScore = 0;
    public int healthScore = 0;
    public int reputationScore = 0;
    public int wisdomScore = 0;
    public int pawprintScore = 0;

    private int wisdomPlusCounter = 0;
    private int wisdomMinusCounter = 0;
    private bool wisdomPlusCoroutineHasStarted = false;
    private bool wisdomMinusCoroutineHasStarted = false;

    private int healthMinusCounter = 0;
    private bool healthMinusCoroutineHasStarted = false;

    public UICircle thriftCircle;
    public UICircle fitnessCircle;
    public UICircle happinessCircle;
    public UICircle healthCircle;
    public UICircle reputationCircle;
    public UICircle wisdomCircle;

    public void Start()
    {
        updateScoreValues( );
        /*
          Click menu button
          Display 2 options:
            - New quest
                - when clicked shows a new screen with a random quest from xml with accept and deny buttons
                    - Then close menu.

            - Current quests
                - When clicked shows a new screen with the quests that the player accepted, with "Complete" and "Forfeit" buttons.
                    - When "Complete" is clicked the points defined from the quest xml are added to the main score value of that attribute.








        */
    }

    public void AddScore(int thrift, int fit, int happ, int health, int rep, int wis, int paw)
    {
        thriftinessScore += thrift;
        fitnessScore += fit;
        happinessScore += happ;
        healthScore += health;
        reputationScore += rep;
        wisdomScore += wis;
        pawprintScore += paw;
        
        updateScoreValues( );


        thriftCircle.fillPercent = thriftinessScore * 10;
        thriftCircle.SetAllDirty( );
        fitnessCircle.fillPercent = fitnessScore * 10;
        fitnessCircle.SetAllDirty( );
        happinessCircle.fillPercent = happinessScore * 10;
        happinessCircle.SetAllDirty( );
        healthCircle.fillPercent = healthScore * 10;
        healthCircle.SetAllDirty( );
        reputationCircle.fillPercent = reputationScore * 10;
        reputationCircle.SetAllDirty( );
        wisdomCircle.fillPercent = wisdomScore * 10;
        wisdomCircle.SetAllDirty( );
        Canvas.ForceUpdateCanvases( );
    }

    public void increaseScore()
    {
        updateScoreValues( );
        thriftCircle.fillPercent = thriftinessScore;
        thriftCircle.SetAllDirty();
        fitnessCircle.fillPercent = fitnessScore;
        fitnessCircle.SetAllDirty( );
        happinessCircle.fillPercent = happinessScore;
        happinessCircle.SetAllDirty( );
        healthCircle.fillPercent = healthScore;
        healthCircle.SetAllDirty( );
        reputationCircle.fillPercent = reputationScore;
        reputationCircle.SetAllDirty( );
        wisdomCircle.fillPercent = wisdomScore;
        wisdomCircle.SetAllDirty( );

        Canvas.ForceUpdateCanvases( );
    }

    public void Update()
    {
        

        if(Input.GetKeyDown(KeyCode.I))
        {
            
        }
        if(Input.GetKeyDown(KeyCode.U))
        {
            thriftinessScore--;
            fitnessScore--;
            happinessScore--;
            healthScore--;
            reputationScore--;
            wisdomScore--;
            pawprintScore--;
            updateScoreValues();
            
        }



        if(!wisdomPlusCoroutineHasStarted && wisdomScore > 2)
        {
            Debug.Log("Cycling wisdom icons.");
            //StartCoroutine( "cycleWisdom" );
            InvokeRepeating("cycleWisdomPlus",2f,2f);
            wisdomPlusCoroutineHasStarted = true;
        }
        else if(wisdomPlusCoroutineHasStarted && wisdomScore <= 2)
        {
            wisdomPlusCoroutineHasStarted = false;
            CancelInvoke("cycleWisdomPlus");
        }
        if(!wisdomMinusCoroutineHasStarted && wisdomScore < 0)
        {
            Debug.Log("Cycling wisdom icons.");
            //StartCoroutine( "cycleWisdom" );
            InvokeRepeating("cycleWisdomMinus",2f,2f);
            wisdomMinusCoroutineHasStarted = true;
        }
        else if(wisdomMinusCoroutineHasStarted && wisdomScore > 0)
        {
            wisdomMinusCoroutineHasStarted = false;
            CancelInvoke("cycleWisdomMinus");
        }


        if(!healthMinusCoroutineHasStarted && healthScore < 0)
        {
            Debug.Log("Cycling wisdom icons.");
            //StartCoroutine( "cycleWisdom" );
            InvokeRepeating("cycleHealthMinus",2f,2f);
            healthMinusCoroutineHasStarted = true;
        }
        else if(healthMinusCoroutineHasStarted && healthScore > 0)
        {
            healthMinusCoroutineHasStarted = false;
            CancelInvoke("cycleHealthMinus");
        }

    }

    public void updateScoreValues()
    {   
        ///THRIFTINESS
        if(thriftinessScore <0)
        {
            pointAudio.PlayOneShot(pointDown);
            meekaThriftiness.sprite = thriftinessMinus;
        }
        else if(thriftinessScore>0 && thriftinessScore <=3)
        {
            meekaThriftiness.sprite = thriftiness;
        }
        else if(thriftinessScore>3)
        {
            pointAudio.PlayOneShot( pointUp );
            meekaThriftiness.sprite = thriftinessPlus;
        }
        ///FITNESS
        if (fitnessScore < 0)
        {
            pointAudio.PlayOneShot( pointDown );
            meekaFitness.sprite = fitnessMinus;
        }
        else if (fitnessScore > 0 && fitnessScore <= 3)
        {
            meekaFitness.sprite = fitness0;
        }
        else if (fitnessScore > 3)
        {
            pointAudio.PlayOneShot( pointUp );
            meekaFitness.sprite = fitnessPlus;
        }
        ///HAPPINESS
        if (happinessScore < 0)
        {
            pointAudio.PlayOneShot( pointDown );
            meekaHappiness.sprite = happinessMinus;
        }
        else if (happinessScore > 0 && happinessScore <= 3)
        {
            meekaHappiness.sprite = happiness;
        }
        else if (happinessScore > 3)
        {
            pointAudio.PlayOneShot( pointUp );
            meekaHappiness.sprite = happinessPlus;
        }

        ///HEALTH
        if (healthScore < 0)
        {
            pointAudio.PlayOneShot( pointDown );
            meekaHealth.gameObject.SetActive( true );
            meekaHealth.sprite = healthMinus;
        }
        else if (healthScore >= 0 && healthScore <= 3)
        {
            meekaHealth.gameObject.SetActive(false);
        }
        else if (healthScore > 3)
        {
            pointAudio.PlayOneShot( pointUp );
            meekaHealth.gameObject.SetActive( true );
            meekaHealth.sprite = healthPlus;
        }

        ///REPUTATION
        if (reputationScore < 0)
        {
            pointAudio.PlayOneShot( pointDown );
            meekaReputation.gameObject.SetActive( true );
            meekaReputation.sprite = reputationMinus;
        }
        else if (reputationScore == 0)
        {
            meekaReputation.gameObject.SetActive(false);
        }
        else if (reputationScore == 1)
        {
            pointAudio.PlayOneShot( pointUp );
            meekaReputation.gameObject.SetActive( true );
            meekaReputation.sprite = reputationPlus1;
        }
        else if (reputationScore == 2)
        {
            pointAudio.PlayOneShot( pointUp );
            meekaReputation.gameObject.SetActive( true );
            meekaReputation.sprite = reputationPlus2;
        }
        else if (reputationScore == 3)
        {
            pointAudio.PlayOneShot( pointUp );
            meekaReputation.gameObject.SetActive( true );
            meekaReputation.sprite = reputationPlus3;
        }
        ///WISDOM
        if (wisdomScore < 0)
        {
            pointAudio.PlayOneShot( pointDown );
            meekaWisdom.gameObject.SetActive( true );
            meekaWisdom.sprite = wisdomMinus3;
        }
        else if (wisdomScore == 0)
        {
            meekaWisdom.gameObject.SetActive(false);
            meekaWisdom.sprite = wisdom;
        }
        else if (wisdomScore > 2)
        {
            pointAudio.PlayOneShot( pointUp );
            meekaWisdom.gameObject.SetActive( true );
        }
        ///PAWPRINT
        if (pawprintScore < 0)
        {
            pointAudio.PlayOneShot( pointDown );
            pawprintBackImage.sprite = pawprintBackMinus;
            pawprintFrontImage.sprite = pawprintFrontMinus;
            pawprintHillImage.sprite = hillMinus;
        }
        else if (pawprintScore == 0)
        {
            pawprintBackImage.sprite = pawprintBack;
            pawprintFrontImage.sprite = pawprintFront;
            pawprintHillImage.sprite = hill;
        }
        else if (pawprintScore > 2)
        {
            pointAudio.PlayOneShot( pointUp );
            pawprintBackImage.sprite = pawprintBackPlus;
            pawprintFrontImage.sprite = pawprintFrontPlus;
            pawprintHillImage.sprite = hillPlus;
        }
    }

    void cycleWisdomPlus()
    {
        
        if (wisdomPlusCounter == 0)
        {
            meekaWisdom.sprite = wisdomPlus1;
            //Debug.Log("1");
        }
        if (wisdomPlusCounter == 1)
        {
            meekaWisdom.sprite = wisdomPlus2;
            //Debug.Log( "2" );
        }
        if (wisdomPlusCounter == 2)
        {
            meekaWisdom.sprite = wisdomPlus3;
            //Debug.Log( "3" );
        }
        if (wisdomPlusCounter == 3)
        {
            meekaWisdom.sprite = wisdomPlus4;
            //Debug.Log( "4" );
        }
        if (wisdomPlusCounter == 4)
        {
            meekaWisdom.sprite = wisdomPlus5;
            //Debug.Log( "5" );
            wisdomPlusCounter = 0; 
        }
        wisdomPlusCounter++;
    }
    void cycleWisdomMinus()
    { 
        if (wisdomMinusCounter == 1)
        {
            meekaWisdom.sprite = wisdomMinus1;
            //Debug.Log("1");
        }
        if (wisdomMinusCounter == 2)
        {
            meekaWisdom.sprite = wisdomMinus2;
            //Debug.Log( "2" );
        }
        if (wisdomMinusCounter == 3)
        {
            meekaWisdom.sprite = wisdomMinus3;
            //Debug.Log( "3" );
            wisdomMinusCounter = 0;
        }
        wisdomMinusCounter++;
    }
    void cycleHealthMinus()
    { 
        if (healthMinusCounter == 1)
        {
            meekaHealth.sprite = healthMinus;
            //Debug.Log("1");
        }
        if (healthMinusCounter == 2)
        {
            meekaHealth.sprite = healthMinus1;
            //Debug.Log( "2" );
        }
        if (healthMinusCounter == 3)
        {
            meekaHealth.sprite = healthMinus2;
            //Debug.Log( "3" );
        }
        if (healthMinusCounter == 4)
        {
            meekaHealth.sprite = healthMinus3;
            //Debug.Log( "4" );
            healthMinusCounter = 0;
        }
        healthMinusCounter++;
    }

    
}
