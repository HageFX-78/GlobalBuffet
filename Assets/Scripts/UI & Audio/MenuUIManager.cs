using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuUIManager : MonoBehaviour
{
    [SerializeField] GameObject splashScreen;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsOverlay;
    [SerializeField] GameObject rulesOverlay;

    [SerializeField] Slider volSlider;

    AudioManager am;
    private void Awake()
    {
        Time.timeScale = 1;
        am = AudioManager.amInstance;
    }
    void Start()
    {
        am.PlayBGM("menuBGM");
        VolumeUpdate();
        CheckReturnFromGameScene();
    }

    public void SplashToMenu()
    {
        am.PlaySF("tap");
        splashScreen.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OnRules()
    {
        am.PlaySF("tap");
        rulesOverlay.SetActive(true);
    }

    public void OffRules()
    {
        am.PlaySF("tap");
        rulesOverlay.SetActive(false);
    }

    public void OnSettings()
    {
        am.PlaySF("tap");
        settingsOverlay.SetActive(true);
    }
    public void OffSettings()
    {
        am.PlaySF("tap");
        settingsOverlay.SetActive(false);
    }

    public void StartGame()
    {
        am.PlaySF("tap");
        am.StopBGM("menuBGM");
        SceneManager.LoadSceneAsync("StageScene");
    }

    public void QuitGame()
    {
        PlayerPrefs.SetInt("returned", 0);
        Application.Quit();
    }
    public void ResetVolume()
    {
        am.PlaySF("tap");
        PlayerPrefs.SetFloat("volume", 1.0f);
        volSlider.value = PlayerPrefs.GetFloat("volume");
        SliderChange();
    }
    public void ClearPlayerPrefs()
    {
        am.PlaySF("tap");
        PlayerPrefs.DeleteAll();
        VolumeUpdate();
        CheckReturnFromGameScene();
    }

    public void VolumeUpdate()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            volSlider.value = PlayerPrefs.GetFloat("volume");
            SliderChange();
        }
        else
        {
            PlayerPrefs.SetFloat("volume", 1.0f);
        }
    }

    public void CheckReturnFromGameScene()
    {
        if (PlayerPrefs.HasKey("returned"))
        {
            if (PlayerPrefs.GetInt("returned") == 1)
            {
                mainMenu.SetActive(true);
                splashScreen.SetActive(false);
                PlayerPrefs.SetInt("returned", 0);
            }
            else
            {
                mainMenu.SetActive(false);
                splashScreen.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetInt("returned", 0);
        }
    }
    public void SliderChange()
    {
        am.PlaySF("slider");
        float newValue = volSlider.value;
        AudioListener.volume = newValue;
        PlayerPrefs.SetFloat("volume", newValue);
    }
}
