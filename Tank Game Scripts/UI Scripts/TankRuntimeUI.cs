using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;

public class TankRuntimeUI : MonoBehaviour
{
    public TankPlayerBehavior player;
    public TankEconomyController economyController;
    public UIDocument tankUI;
    public UIDocument pauseMenu;

    private Label coinRef;
    private Label armorRef;
    private Label healthRef;
    private string scenePath = "Assets/Scenes/";
    private int initialCoinCount;

    void OnEnable()
    {
        VisualElement tankUIRoot = tankUI.rootVisualElement;
        VisualElement pauseMenuRoot = pauseMenu.rootVisualElement;

        #region Pause Menu
        Button sendToPaymentButton = pauseMenuRoot.Q<Button>("SendToPaymentButton");
        Button returnFromSettingsButton = pauseMenuRoot.Q<Button>("ReturnToMainMenuButton");

        returnFromSettingsButton.clicked += () => 
        {
            TankDataHandler.SaveAllData((int)player.data["Coins"], 0, 
                                        (int)player.data["ThrallLevel"], (int)player.data["RamLevel"], 
                                        (int)player.data["ArmorLevel"]);
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync(scenePath + "MainMenuScene.unity");
        };

        SliderInt effectsVolumeSliderInt = pauseMenuRoot.Q<SliderInt>("SoundVolume");
        SliderInt musicVolumeSliderInt = pauseMenuRoot.Q<SliderInt>("MusicVolume");


        int musicVolume = TankAppController.Instance.GetMusicVolume();
        effectsVolumeSliderInt.value = PlayerPrefs.GetInt("SoundVolume", 0);
        musicVolumeSliderInt.value = PlayerPrefs.GetInt("MusicVolume", musicVolume);
        
        effectsVolumeSliderInt.RegisterValueChangedCallback(v =>
            {
                //var oldValue = v.previousValue;
                var newValue = v.newValue;
                PlayerPrefs.SetInt("SoundVolume", v.newValue);
                TankAppController.Instance.SetSFXVolume(v.newValue);
            });

        musicVolumeSliderInt.RegisterValueChangedCallback(v =>
            {
                //var oldValue = v.previousValue;
                var newValue = v.newValue;
                PlayerPrefs.SetInt("MusicVolume", v.newValue);
                TankAppController.Instance.SetMusicVolume(v.newValue);
                //Debug.Log("New music volume = " + TankAppController.Instance.MusicVolume.ToString());
            });
        #endregion

        #region Tank UI
        coinRef = tankUIRoot.Q<Label>("CoinsLabel");
        armorRef = tankUIRoot.Q<Label>("ArmorLabel");
        healthRef = tankUIRoot.Q<Label>("HealthLabel");

        Button enablePause = tankUIRoot.Q<Button>("PauseButton");

        enablePause.clicked += () => 
        {
            Time.timeScale = 0f;
            tankUIRoot.style.display = DisplayStyle.None;
            pauseMenuRoot.style.display = DisplayStyle.Flex;
        };

        #endregion

        #region Extra
        Button unpauseButton = pauseMenuRoot.Q<Button>("UnpauseButton");

        unpauseButton.clicked += () =>
        {
            Time.timeScale = 1f;
            tankUIRoot.style.display = DisplayStyle.Flex;
            pauseMenuRoot.style.display = DisplayStyle.None;
        };
        #endregion
    }

    void Start()
    {
        initialCoinCount = (int)player.data["Coins"];
        DisableSettingsMenu();
    }

    // Update is called once per frame
    void Update()
    {
        coinRef.text = (economyController.CurrentCoinCount + initialCoinCount).ToString();
        armorRef.text = player.playerArmor.ToString();
        healthRef.text = player.playerHP.ToString();
        if(player.isDestroyed)
        {
            tankUI.enabled = false;
        }
    }


    void DisableSettingsMenu()
    {
        pauseMenu.rootVisualElement.style.display = DisplayStyle.None;
    }
}
