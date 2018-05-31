using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class Conversation
{
    public string Text;
    public Sprite PersonTalking;
    public ImageSide ImagePlacement;

    public enum ImageSide
    {
        Left,
        Right
    }
}

public class GameRules : MonoBehaviour
{

    public string LevelName;
    public Conversation[] OpeningConversation;
    public Conversation[] EndingConversation;
    public WayToWin WinCondition;
    public WayToLose LoseCondition;

    bool isInTransition = false;
    private bool IsPaused;

    [HideInInspector]
    public GameObject TargetToProtect;
    [HideInInspector]
    public GameObject TargetToDestroy;
    [HideInInspector]
    public float TimeToSurvive;
    [HideInInspector]
    public float TimeToWin;
    [HideInInspector]
    public GameObject ObjectToCollect;


    private GameObject _player;
    private GameObject _spawn;
    private List<GameObject> _enemies;
    private TankStats _playerStats;
    private EnemySpawn _currentRound;
    private Dictionary<string, string> _gameInfoText = new Dictionary<string, string>();
    private GameObject _conversationContainer;
    private GameObject _gameoverContainer;
    private Button _proceedButton;
    private bool _isOpeningConversationDone;
    private bool _isEndingConversationStarted;
    private List<Conversation> _openingConversationList = new List<Conversation>();
    private List<Conversation> _endingConversationList = new List<Conversation>();
    private Text _speechText;
    private Image _leftImage;
    private Image _rightImage;
    private Text _tooltip;
    private Text _gameOverHeader;
    private bool btnSet;

    public enum WayToWin
    {
        DestroyAll,
        DestroySpecific,
        Time,
        Collect,
        Round
    };

    public enum WayToLose
    {
        GetDestroyed,
        Time,
        FailToProtect
    };

    // Use this for initialization
    void Start()
    {
        InitiateUI();
        _player = GameObject.Find("Player");
        _playerStats = _player.GetComponent<TankStats>();
        
    }

    void Update()
    {
        Time.timeScale = 0;
        if (!_isEndingConversationStarted)
        {
            _conversationContainer.SetActive(!_isOpeningConversationDone);
            if (_openingConversationList.Count > 0)
            {
            StartConversation(_openingConversationList);
            }
            else
            {
            Time.timeScale = 1;
            _isOpeningConversationDone = true;
            _tooltip.text = "";
            }
        }
        else
        {
            _conversationContainer.SetActive(_isEndingConversationStarted);
            if (_endingConversationList.Count > 0)
            {
                _isEndingConversationStarted = true;
                StartConversation(_endingConversationList);
            }
            else
            {
                _conversationContainer.SetActive(false);
                _gameoverContainer.SetActive(true);
            }
        }

       
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_isOpeningConversationDone || _isEndingConversationStarted)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            HandleMenu();
        }
        _enemies = GetAllObjectsWithTagContainingString("Enemy");
        SetGameInfo();
        if (CheckIfWin(WinCondition))
        {
            Time.timeScale = 0;
            if (_endingConversationList.Count > 0)
            {
                _isEndingConversationStarted = true;
                _gameOverHeader.text = "YOU WIN";
            }
            if (!btnSet)
            {
                _proceedButton.transform.Find("Text").GetComponent<Text>().text = "NEXT";
                _proceedButton.onClick.AddListener(delegate {
                    GameObject.Find("ButtonHandler").GetComponent<GameoverButtonHandler>()
                    .LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    btnSet = true;
                });
            }
            
        }
        if (CheckIfLose(LoseCondition))
        {
            Time.timeScale = 0;
            _gameOverHeader.text = "YOU LOSE";
            _conversationContainer.SetActive(false);
            _gameoverContainer.SetActive(true);
            if (!btnSet)
            {
                _proceedButton.transform.Find("Text").GetComponent<Text>().text = "RETRY";
                _proceedButton.onClick.AddListener(delegate {
                    GameObject.Find("ButtonHandler").GetComponent<GameoverButtonHandler>()
                    .LoadScene(SceneManager.GetActiveScene().buildIndex);
                    btnSet = true;
                });
            }
        }


    }

    private void InitiateUI()
    {
        InitiateConversationUI();
        InitiateGameInfoUI();
        InitiateGameoverUI();
    }

    private void InitiateGameoverUI()
    {
        _gameoverContainer = GameObject.Find("Gameover Container");
        _gameOverHeader = _gameoverContainer.transform.Find("Header").GetComponent<Text>();
        _proceedButton = _gameoverContainer.transform.Find("Proceed Button").GetComponent<Button>();
        _gameoverContainer.SetActive(false);
    }

    private void InitiateGameInfoUI()
    {
        _gameInfoText["Initial"] = transform.Find("Canvas/GameInfo").GetComponent<Text>().text;
        Text levelName = transform.Find("Canvas/LevelName").GetComponent<Text>();
        _tooltip = transform.Find("Canvas/Tooltip").GetComponent<Text>();
        levelName.text = LevelName;
    }

    private void InitiateConversationUI()
    {
        _openingConversationList = OpeningConversation.ToList();
        _endingConversationList = EndingConversation.ToList();
        _conversationContainer = GameObject.Find("Speech Bubble Container");
        _speechText = _conversationContainer.transform.Find("Speech Bubble/Speech Text").GetComponent<Text>();
        _leftImage = _conversationContainer.transform.Find("Left Image").GetComponent<Image>();
        _rightImage = _conversationContainer.transform.Find("Right Image").GetComponent<Image>();
        _conversationContainer.SetActive(false);
    }


    private void StartConversation(List<Conversation> conversation)
    {
        Conversation currConversation = conversation[0];

        _speechText.text = currConversation.Text;

        if (currConversation.ImagePlacement.ToString().Equals("Left"))
        {
            _leftImage.color = Color.white;
            _leftImage.sprite = currConversation.PersonTalking;
            _rightImage.color = Color.clear;
        }
        else if (currConversation.ImagePlacement.ToString().Equals("Right"))
        {
            _rightImage.color = Color.white;
            _rightImage.sprite = currConversation.PersonTalking;
            _leftImage.color = Color.clear;
        }

        if (Input.GetMouseButtonDown(0))
        {
            conversation.Remove(currConversation);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            conversation.Clear();
        }
    }



    private void SetGameInfo()
    {
        Text text = transform.Find("Canvas/GameInfo").GetComponent<Text>();
        text.text = "";
        foreach (KeyValuePair<string, string> entry in _gameInfoText)
        {
            if(!entry.Key.Equals("Initial"))
            {
                text.text += entry.Key + ": " + entry.Value + "\n";
            }
        }
        text.text += "\n" + _gameInfoText["Initial"];
    }

    //Has the specified win condition been met
    private bool CheckIfWin(WayToWin winCondition)
    {
        switch (winCondition.ToString())
        {
            case "DestroyAll":
                return HasAllEnemiesBeenDestroyed();
            case "DestroySpecific":
                return HasTargetBeenDestroyed(TargetToDestroy);
            case "Time":
                return HasTimeRunOut(TimeToSurvive);
            case "Collect":
                return HasItemBeenCollected();
            case "Round":
                return currentRound();
            default:
                return false;
        }
    }


    //Has the specified lose condition been met
    private bool CheckIfLose(WayToLose loseCondition)
    {
        if(HaveIBeenDestroyed())
        {
            return true;
        }

        switch (loseCondition.ToString())
        {
            case "Time":
                return HasTimeRunOut(TimeToWin);
            case "FailToProtect":
                return HasTargetBeenDestroyed(TargetToProtect);
            default:
                return false;
        }
    }

    private bool HaveIBeenDestroyed()
    {
        _gameInfoText["Health"] = _playerStats.GetCurrHealth() + "/" + _playerStats.Maxhealth;
        return _playerStats.GetCurrHealth() <= 0;
    }
    
    private bool HasItemBeenCollected()
    {
        return ObjectToCollect == null;
    }

    private bool HasTimeRunOut(float time)
    {
        _gameInfoText["Timelimit"] = Math.Floor(time - Time.time) + "";
        return time<Time.time;
    }

    private bool HasTargetBeenDestroyed(GameObject target)
    {
        String healthString;
        if(target == null)
        {
            _gameInfoText["Target Health"] = "0";
            return true;
        }
        if(target.GetComponent<NPCStats>() != null)
        {
            healthString = target.GetComponent<NPCStats>().GetCurrHealth() + "/" + target.GetComponent<NPCStats>().Maxhealth;
        }
        else if (target.GetComponent<TankStats>() != null)
        {
            healthString = target.GetComponent<TankStats>().GetCurrHealth() + "/" + target.GetComponent<TankStats>().Maxhealth;
        }
        else
        {
            healthString = "Dead";
        }
        _gameInfoText["Target Health"] = healthString;
        return false;
    }

    private bool HasAllEnemiesBeenDestroyed()
    {
        _gameInfoText["Number of enemies"] = _enemies.Where(x => x!= null).Count() + "";
        return _enemies.All(x => x == null);
    }

    private bool currentRound()
    {
        _spawn = GameObject.Find("SpawnCenter");
        _currentRound = _spawn.GetComponent<EnemySpawn>();
        int round = _currentRound.GetCurrRound();
        _gameInfoText["Round "] = round + "";
        return round == 3;
    }

    private List<GameObject> GetAllObjectsWithTagContainingString(string tag)
    {
        GameObject[] gos = FindObjectsOfType<GameObject>();
        List<GameObject> gosWithTag = new List<GameObject>();
        foreach (GameObject go in gos)
        {
            if (go.tag.Contains(tag))
            {
                gosWithTag.Add(go);
            }
        }
        return gosWithTag;
    }

    public void HandleMenu()
    {
        if (!isInTransition)
        {
            //toogle input
            IsPaused = !IsPaused;

            //toogle menu
            SceneManager.LoadScene(0);

            isInTransition = true;

            StartCoroutine(WaitDisableTransition());
        }
    }

    IEnumerator WaitDisableTransition()
    {
        yield return new WaitForSeconds(0.35f);
        
        DisableTransition();
    }

    public void DisableTransition()
    {
        isInTransition = false;
    }

}


#if UNITY_EDITOR

//Custom editor
[CustomEditor(typeof(GameRules))]
public class GameRulesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        GameRules script = (GameRules)target;
        
        switch (script.WinCondition.ToString())
        {
            case "DestroyAll":

                break;
            case "DestroySpecific":
                    script.TargetToDestroy = EditorGUILayout.ObjectField("Target To Destroy", script.TargetToDestroy, typeof(GameObject), true) as GameObject;
                break;
            case "Time":
                script.TimeToSurvive = EditorGUILayout.FloatField("Time To Survive (sec)", script.TimeToSurvive);
                break;
            case "Collect":
                    script.ObjectToCollect = EditorGUILayout.ObjectField("Target To Collect", script.ObjectToCollect, typeof(GameObject), true) as GameObject;
                    break;

            default:

                break;
        }

        switch (script.LoseCondition.ToString())
        {
            case "GetDestroyed":

                break;
            case "Time":
                script.TimeToWin = EditorGUILayout.FloatField("Time Before Lose (sec)", script.TimeToWin);
                break;
            case "FailToProtect":
                script.TargetToProtect = EditorGUILayout.ObjectField("Target To Protect", script.TargetToProtect, typeof(GameObject), true) as GameObject;
                break;
            default:

                break;
        }
        if(GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
    }



}
#endif
