using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject overlayBackground;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject pauseOverPanel;
    [SerializeField] GameObject eaten;
    [SerializeField] GameObject timeup;
    [SerializeField] Transform player;

    [SerializeField] TextMeshProUGUI realTimeScore;

    [SerializeField] TextMeshProUGUI bestResults;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI size;
    [SerializeField] Slider volume;

    public int currentScore;
    public static GameUIManager guiInstance;
    AudioManager am;

    private void Awake()
    {
        am = AudioManager.amInstance;
        am.PlayBGM("gameBGM");

        guiInstance = this;
        Time.timeScale = 1;
        currentScore = 0;
        UpdateScore();
    }

    public void UpdateScore(int amt = 0)
    {
        currentScore = Mathf.Abs(currentScore + amt);
        realTimeScore.text = $"{currentScore.ToString("n0")}";
    }

    #region UI Functions
    public void PauseGame()
    {
        am.PlaySF("tap");
        Time.timeScale = 0;
        overlayBackground.SetActive(true);
        pauseOverPanel.SetActive(true);
    }

    public void UnpauseGame()
    {
        am.PlaySF("tap");
        Time.timeScale = 1;
        overlayBackground.SetActive(false);
        pauseOverPanel.SetActive(false);
    }

    public void GameOver(bool isEaten)
    {
        Time.timeScale = 0;
        am.StopBGM("gameBGM");


        overlayBackground.SetActive(true);
        gameOverPanel.SetActive(true);
        if (isEaten)
        {
            am.PlaySF("eaten");
            eaten.SetActive(true);
            timeup.SetActive(false);
        }
        else
        {
            am.PlaySF("alarm");
            eaten.SetActive(false);
            timeup.SetActive(true);
        }
        score.text = $"{currentScore.ToString("n0")}";
        size.text = $"{player.localScale.x.ToString("F2")} KG";
        float playerSize = Mathf.Round(player.localScale.x * 100f) / 100f;
        int currentHighscore = 0;
        float currentBestSize = 0;

        if (PlayerPrefs.HasKey("highscore"))
        {
            if (PlayerPrefs.GetInt("highscore") < currentScore)
            {
                PlayerPrefs.SetInt("highscore", currentScore);
                currentHighscore = currentScore;
            }
            else
            {
                currentHighscore = PlayerPrefs.GetInt("highscore");
            }
        }
        else
        {
            currentHighscore = currentScore;
            PlayerPrefs.SetFloat("highscore", currentHighscore);
        }
        if (PlayerPrefs.HasKey("bestsize"))
        {
            if (PlayerPrefs.GetFloat("bestsize") < playerSize)
            {
                PlayerPrefs.SetFloat("bestsize", playerSize);
                currentBestSize = playerSize;
            }
            else
            {
                currentBestSize = PlayerPrefs.GetFloat("bestsize");
            }
        }
        else
        {
            currentBestSize = playerSize;
            PlayerPrefs.SetFloat("bestsize", currentBestSize);
        }

        //Change best score and size
        bestResults.text = $"Best:\n{currentBestSize} KG\n{currentHighscore.ToString("n0")}";
    }

    public void ReturnToMainMenu()
    {
        am.StopBGM("gameBGM");
        am.PlaySF("tap");
        PlayerPrefs.SetInt("returned", 1);
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void Retry()
    {
        am.PlaySF("tap");
        SceneManager.LoadSceneAsync("StageScene");
    }

    public void SliderChange()
    {
        am.PlaySF("slider");
        float newValue = volume.value;
        AudioListener.volume = newValue;
        PlayerPrefs.SetFloat("volume", newValue);
    }
    #endregion
}
