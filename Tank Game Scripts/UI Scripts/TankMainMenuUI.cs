using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;

public class TankMainMenuUI : MonoBehaviour
{
    private UIDocument mainMenu;
    private string scenePath = "Assets/Scenes/";

    void Awake()
    {
        mainMenu = GetComponent<UIDocument>();
    }

    void OnEnable()
    {
        VisualElement mainMenuRoot = mainMenu.rootVisualElement;

        Button playButton = mainMenuRoot.Q<Button>("PlayButton");
        Button shopButton = mainMenuRoot.Q<Button>("ShopButton");
        Button settingsButton = mainMenuRoot.Q<Button>("SettingsButton");

        playButton.clicked += () => SceneManager.LoadSceneAsync(scenePath + "TankWorld.unity");
    }
}
