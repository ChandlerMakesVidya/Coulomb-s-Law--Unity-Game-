using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.EventSystems;

/*
 * Author: Chandler Hummingbird
 * Date Created: Sep 12, 2020
 * Date Modified Sep 12, 2020
 * Description: The Game Manager manages basic game resources such as score, health, and
 * outputs to UI elements in the game scene.
 */

public class GameManager : MonoBehaviour
{

    #region GM Singleton
    public static GameManager gm;
    private void Awake()
    {
        if (gm != null)
        {
            Debug.LogWarning("More than one instance of game manager exists!");
        }
        else
        {
            gm = this;
            DontDestroyOnLoad(gm);
        }
    }
    #endregion

    #region Variables
    public static float timer;
    [Tooltip("The levels of the game, in order.")]
    public GameObject[] levels;
    public int currentLevel;
    [Space]
    public TMP_Text timerValueDisplay;
    public TMP_Text levelNameDisplay;
    [Space]
    public AudioSource BGM;
    public AudioClip gameOverSFX;
    public AudioClip beatLevelSFX;
    public bool BGM_Over;

    public enum GameState
    {
        MainMenu, Playing, Death, GameOver, BeatLevel
    }

    [Space]
    public static GameState gameState = GameState.MainMenu;
    public bool gameOver;
    public GameObject charge;
    [Space]
    public Canvas menuCanvas;
    public Canvas HUDCanvas;
    public Canvas endScreenCanvas;
    public Canvas footerCanvas;
    [Space]

    public string endMessageWinText = "You win! Congratulations!";
    public string endMessageLoseText = "Sorry! You lose!";
    public TMP_Text endMessageDisplay;
    [Space]
    public string gameOverText = "Game Over!";
    public TMP_Text gameOverDisplay;
    [Space]
    public string gameTitleText = "Coulomb's Law";
    public TMP_Text gameTitleDisplay;
    [Space]
    public string gameCreditsText = "Credits: \n Chandler Hummingbird, University of Tulsa";
    public TMP_Text gameCreditsDisplay;
    [Space]
    public string gameCopyrightText = "Copyright " + currentYear;
    public TMP_Text gameCopyrightDisplay;
    [Space]

    /*public string firstLevelName;
    public string nextLevelName;
    public string levelToLoadName;
    public string currentLevelName;
    public string restartLevelToLoadName;*/

    private GameObject currentLevelGameObject;
    private float currentTime;
    private string FormatTime(float time)
    {
        int intTime = (int)time;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = timer * 1000;
        fraction = (fraction % 1000);
        string timeText = String.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        return timeText;
    }

    private bool gameStarted;
    private static bool replay;

    private static string currentYear = System.DateTime.Now.ToString("yyyy");
    #endregion

    private void Reset()
    {
        gameState = GameState.Playing;
        gameOver = false;
        //playerDead = false;
        endMessageWinText = "You win! Congratulations!";
        endMessageLoseText = "Sorry! You lose!";
        gameOverText = "Game Over!";
        gameTitleText = "Game Title";
        gameCreditsText = "Credits: \n Chandler Hummingbird, University of Tulsa";
        gameCopyrightText = "Copyright " + currentYear;
    }

    public void MainMenu()
    {
        /*gameTitleText = "Game Title";
        gameCreditsText = "Credits: \n Chandler Hummingbird, University of Tulsa";
        gameCopyrightText = "Copyright " + currentYear;*/
        Destroy(currentLevelGameObject);
        HideMenu();
        gameState = GameState.MainMenu;
        timer = 0.0f;
        gameTitleDisplay.text = gameTitleText;
        gameCreditsDisplay.text = gameCreditsText;
        gameCopyrightDisplay.text = gameCopyrightText;
        menuCanvas.gameObject.SetActive(true);
        footerCanvas.gameObject.SetActive(true);
    }

    public void HideMenu()
    {
        menuCanvas.gameObject.SetActive(false);
        HUDCanvas.gameObject.SetActive(false);
        endScreenCanvas.gameObject.SetActive(false);
        footerCanvas.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void PlayGame()
    {
        //currentLevel = 0;

        HideMenu();
        HUDCanvas.gameObject.SetActive(true);
        gameStarted = true;
        timer = 0.0f;
        charge = GameObject.FindGameObjectWithTag("Charge");
        gameState = GameState.Playing;
        //playerDead = false;
        //SceneManager.LoadScene(levelToLoadName, LoadSceneMode.Additive);
        //currentLevelName = levelToLoadName;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ResetLevel()
    {
        //playerDead = false;
        //SceneManager.UnloadSceneAsync(currentLevelName);
        Destroy(currentLevelGameObject);
        currentLevelGameObject = Instantiate(levels[currentLevel]);
        PlayGame();
    }

    public void LoadNextLevel()
    {
        if (currentLevel == levels.Length - 1)
        {
            currentLevel = 0;
        }
        else
        {
            currentLevel++;
        }

        LoadLevel(currentLevel);
    }

    public void LoadLevel(int index)
    {
        currentLevel = index;
        BGM_Over = false;
        Destroy(currentLevelGameObject);
        currentLevelGameObject = Instantiate(levels[index]);
        levelNameDisplay.text = currentLevelGameObject.GetComponent<Level>().levelName;
        PlayGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Scene currentLevel = SceneManager.GetActiveScene();
        //restartLevelToLoadName = currentLevel.name;

        HideMenu();
        //levelToLoadName = firstLevelName;
        //PlayGame();
        MainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameState != GameState.MainMenu)
        {
            MainMenu();
        }

        if (Input.GetKeyDown(KeyCode.End))
        {
            gameState = GameState.GameOver;
        }

        switch (gameState)
        {
            case (GameState.Playing):
                /*if(canBeatLevel & score >= scoreToBeatLevel)
                {
                    gameState = GameState.BeatLevel;
                }*/

                timer += Time.deltaTime;
                timerValueDisplay.text = FormatTime(timer);
                break;
            case GameState.Death:
                if(BGM != null || !BGM_Over)
                {
                    BGM.volume -= Time.deltaTime;
                    if (BGM.volume <= 0.0f)
                    {
                        BGM_Over = true;
                    }
                }
                else
                {
                    if(gameOverSFX != null)
                    {
                        AudioSource.PlayClipAtPoint(gameOverSFX, transform.position);
                    }
                    endMessageDisplay.text = endMessageLoseText;
                    gameState = GameState.GameOver;
                }
                break;
            case GameState.BeatLevel:
                if (BGM != null && !BGM_Over)
                {
                    BGM.volume -= Time.deltaTime;
                    if (BGM.volume <= 0.0f)
                    {
                        BGM_Over = true;
                    }
                }
                else
                {
                    if(beatLevelSFX != null)
                    {
                        AudioSource.PlayClipAtPoint(beatLevelSFX, transform.position);
                    }
                    /*if(nextLevelName != "")
                    {
                        StartNextLevel();
                    }
                    else
                    {
                        endMessageDisplay.text = endMessageWinText;
                        gameState = GameState.GameOver;
                    }*/

                    endMessageDisplay.text = endMessageWinText;
                    gameState = GameState.GameOver;
                }
                break;
            case GameState.GameOver:
                //player.SetActive(false);
                //HideMenu();
                if(endScreenCanvas != null)
                {
                    endScreenCanvas.gameObject.SetActive(true);
                }
                break;
        }
    }
}
