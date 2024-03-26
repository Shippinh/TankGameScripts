using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Audio;

public class TankMainMenuUI : MonoBehaviour
{
    public UIDocument mainMenu;
    public UIDocument shopMenu;
    public UIDocument settingsMenu;
    public UIDocument backgroundImage;
    private string scenePath = "Assets/Scenes/";
    private Dictionary<string, float> data = new Dictionary<string, float>();

    void Awake()
    {
        data = TankDataHandler.LoadAllData(); //doing this in awake cause i have to load values for the shop initially
        //call app prefs here to load volume data
        GoToMainMenu();
        /*foreach(var element in data)
        {
            Debug.Log("Loading data: 'Key = " + element.Key + "; Value = " + element.Value + ";'");
        }*/
    }

    void Start()
    {
        //initializing values for Global Audio Mixer (the one that regulates volumes of Audio Sources (that means i need to include mixer in all audio sources))
        TankAppController.Instance.SetSFXVolume(PlayerPrefs.GetInt("SoundVolume", 0));
        TankAppController.Instance.SetMusicVolume(PlayerPrefs.GetInt("MusicVolume", 0));
    }

    void OnEnable()
    {
        VisualElement mainMenuRoot = mainMenu.rootVisualElement;
        VisualElement shopMenuRoot = shopMenu.rootVisualElement;
        VisualElement settingsMenuRoot = settingsMenu.rootVisualElement;
        VisualElement backgroundRoot = backgroundImage.rootVisualElement;

        #region Main Menu
        Button playButton = mainMenuRoot.Q<Button>("PlayButton");
        Button shopButton = mainMenuRoot.Q<Button>("ShopButton");
        Button settingsButton = mainMenuRoot.Q<Button>("SettingsButton");

        playButton.clicked += () => SceneManager.LoadSceneAsync(scenePath + "TankWorld.unity");
        shopButton.clicked += () => GoToShop();
        settingsButton.clicked += () => GoToSettings();
        #endregion

        #region Shop Menu
        #endregion

        #region Settings Menu
        Button sendToPaymentButton = settingsMenuRoot.Q<Button>("SendToPaymentButton");
        Button returnButton = settingsMenuRoot.Q<Button>("BackToMainMenuButton");

        returnButton.clicked += () => GoToMainMenuAndSave();

        SliderInt effectsVolumeSliderInt = settingsMenuRoot.Q<SliderInt>("SoundVolume");
        SliderInt musicVolumeSliderInt = settingsMenuRoot.Q<SliderInt>("MusicVolume");

        effectsVolumeSliderInt.value = PlayerPrefs.GetInt("SoundVolume", 0);
        musicVolumeSliderInt.value = PlayerPrefs.GetInt("MusicVolume", 0);
        
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
    }

    void GoToShop()
    {
        DisableDisplayStyles();
        shopMenu.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    void GoToSettings()
    {
        DisableDisplayStyles();
        settingsMenu.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    void GoToMainMenu()
    {
        DisableDisplayStyles();
        mainMenu.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    void GoToMainMenuAndSave()
    {
        DisableDisplayStyles();
        mainMenu.rootVisualElement.style.display = DisplayStyle.Flex;
        //TankDataHandler.SaveAllData(200, 0, 5, 6);
        TankDataHandler.SaveAllData(data);
    }

    void DisableDisplayStyles()
    {
        mainMenu.rootVisualElement.style.display = DisplayStyle.None;
        shopMenu.rootVisualElement.style.display = DisplayStyle.None;
        settingsMenu.rootVisualElement.style.display = DisplayStyle.None;
    }

    void DisableGameObjects()
    {
        mainMenu.gameObject.SetActive(false);
        shopMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
    }
}
