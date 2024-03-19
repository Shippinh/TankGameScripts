using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;

public class TankMainMenuUI : MonoBehaviour
{
    public UIDocument mainMenu;
    public UIDocument shopMenu;
    public UIDocument settingsMenu;
    public UIDocument backgroundImage;
    private string scenePath = "Assets/Scenes/";


    //THIS IS 
    void Awake()
    {
        GoToMainMenu();
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

        returnButton.clicked += () => GoToMainMenu();

        //SliderInt musicVolumeSliderInt;
        //SliderInt effectsVolumeSliderInt;
        #endregion
    }

    void GoToShop()
    {
        //DisableGameObjects();
        DisableDisplayStyles();
        //shopMenu.gameObject.SetActive(true);
        shopMenu.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    void GoToSettings()
    {
        //DisableGameObjects();
        DisableDisplayStyles();
        //settingsMenu.gameObject.SetActive(true);
        settingsMenu.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    void GoToMainMenu()
    {
        //DisableGameObjects();
        DisableDisplayStyles();
        //mainMenu.gameObject.SetActive(true);
        mainMenu.rootVisualElement.style.display = DisplayStyle.Flex;
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
