using System.Collections;
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
        GameOver,
        LevelUp
    }

    public GameState currentState, previousState;

    [Header("Screens")]
    public GameObject PauseScreen;
    public GameObject ResultsScreen;
    public GameObject LevelUpScreen;

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

    [Header("Stopwatch")]
    private float stopwatchTime;
    public float TimeLimit;
    public TMP_Text StopwatchDisplay;

    [Header("Damage Text Settings")]
    public Canvas DamageTextCanvas;

    public float TextFontSize = 20f;
    public TMP_FontAsset TextFont;
    public Camera ReferenceCamera;

    public TMP_Text TimeSurvivedDisplay;
    
    public bool IsGameOver = false;
    public bool IsChoosingUpgrade = false;

    public GameObject PlayerObject;

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
        LevelUpScreen.SetActive(false);
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopwatch();
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
            case GameState.LevelUp:
                if (!IsChoosingUpgrade)
                {
                    IsChoosingUpgrade = true;
                    Time.timeScale = 0f;
                    Debug.Log("Upgrades shown");
                    LevelUpScreen.SetActive(true);
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
        TimeSurvivedDisplay.text = StopwatchDisplay.text;
        ChangeState(GameState.GameOver);
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
    
    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        PlayerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        IsChoosingUpgrade = false;
        Time.timeScale = 1;
        LevelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
    
    void DisplayResults()
    {
        ResultsScreen.SetActive(true);
    }

    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;
        UpdateStopwatchDisplay();

        if (stopwatchTime >= TimeLimit)
        {
            PlayerObject.SendMessage("Kill");
        }
    }

    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        StopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public static void GenerateFloatingText(string text, Transform target, float duration = 1f, float speed = 1f)
    {
        if (!instance.DamageTextCanvas) return;
        if (!instance.ReferenceCamera) return;
        instance.StartCoroutine(instance.GenerateFloatingTextCoroutine(text, target, duration, speed));
    }
    
    IEnumerator GenerateFloatingTextCoroutine(string text, Transform target, float duration, float speed)
    {
        // Start generating the floating text.
        GameObject textObj = new GameObject("Damage Floating Text");
        RectTransform rect = textObj.AddComponent<RectTransform>();
        TextMeshProUGUI tmPro = textObj.AddComponent<TextMeshProUGUI>();
        tmPro.text = text;
        tmPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
        tmPro.verticalAlignment = VerticalAlignmentOptions.Middle;
        tmPro.fontSize = TextFontSize;
        if (TextFont) tmPro.font = TextFont;
        rect.position = ReferenceCamera.WorldToScreenPoint(target.position);

        // Makes sure this is destroyed after the duration finishes.
        Destroy(textObj, duration);

        // Parent the generated text object to the canvas.
        textObj.transform.SetParent(instance.DamageTextCanvas.transform);

        // Pan the text upwards and fade it away over time.
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0;
        float yOffset = 0;
        while(t < duration)
        {
            // Wait for a frame and update the time.
            yield return w;
            t += Time.deltaTime;

            // Fade the text to the right alpha value.
            tmPro.color = new Color(tmPro.color.r, tmPro.color.g, tmPro.color.b, 1 - t / duration);

            // Pan the text upwards.
            yOffset += speed * Time.deltaTime;
            rect.position = ReferenceCamera.WorldToScreenPoint(target.position + new Vector3(0,yOffset));
        }
    }

}
