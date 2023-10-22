using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver
    }

    public GameState currentState, previousState;

    [Header("Screens")]
    public GameObject PauseScreen;
    public GameObject ResultsScreen;

    [Header("Current Stat Displays")]
    public TMP_Text CurrentHealthDisplay;
    public TMP_Text CurrentRecoveryDisplay;
    public TMP_Text CurrentMoveSpeedDisplay;
    public TMP_Text CurrentMightDisplay;
    public TMP_Text CurrentProjectileSpeedDisplay;
    public TMP_Text CurrentMagnetDisplay;

    [Header("Results Screen Displays")]
    public Image ChosenCharacterImage;
    public TMP_Text ChosenCharacterName;
    public TMP_Text LevelReachedDisplay;
    public List<Image> ChosenWeaponsUI = new List<Image>(6);
    public List<Image> ChosenPassiveItemsUI = new List<Image>(6);

    public bool IsGameOver = false;
    

    private void Awake()
    {
        // Singleton and ready to mingleton
        if (instance == null) instance = this;
        else
        {
            Debug.LogError("ERROR: Extra " + this + " deleted");
            Destroy(gameObject);
        }
        
        DisableScreens();
    }

    void DisableScreens()
    {
        PauseScreen.SetActive(false);
        ResultsScreen.SetActive(false);
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                break;
            case GameState.Paused:
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                // We check this in order to prevent DisplayResults() from being executed multiple times, since
                // this is occuring in Update()
                if (!IsGameOver) 
                {
                    IsGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("Game is over");
                    DisplayResults();
                }
                break;
            default:
                Debug.LogError("ERROR: State does not exist");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            PauseScreen.SetActive(true);
            Debug.Log("Game is Paused");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState); // ensure that the game continues from the state it was paused from
            Time.timeScale = 1f;
            PauseScreen.SetActive(false);
            Debug.Log("Game is Resumed");
        }
    }

    public void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused) ResumeGame();
            else PauseGame();
        }
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        ResultsScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        ChosenCharacterImage.sprite = chosenCharacterData.Icon;
        ChosenCharacterName.text = chosenCharacterData.Name;
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        LevelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignResultingInventoryUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData)
    {
        if (chosenWeaponsData.Count != ChosenWeaponsUI.Count ||
            chosenPassiveItemsData.Count != ChosenPassiveItemsUI.Count)
        {
            Debug.LogError("ERROR: Chosen weapons and passive item data lists have different lengths");
            return;
        }

        for (int i = 0; i < ChosenWeaponsUI.Count; i++)
        {
            if (chosenWeaponsData[i].sprite)
            {
                ChosenWeaponsUI[i].enabled = true;
                ChosenWeaponsUI[i].sprite = chosenWeaponsData[i].sprite;
            }
            else
            {
                ChosenWeaponsUI[i].enabled = false;
            }
        }

        for (int i = 0; i < ChosenPassiveItemsUI.Count; i++)
        {
            if (chosenPassiveItemsData[i].sprite)
            {
                ChosenPassiveItemsUI[i].enabled = true;
                ChosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else
            {
                ChosenPassiveItemsUI[i].enabled = false;
            }
        }
    }
}
