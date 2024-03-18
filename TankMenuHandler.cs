using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.PlasticSCM.Editor.WebApi;

public class TankMenuHandler : MonoBehaviour
{
    //public TankPlayerBehavior playerScript;
    //public TankWorldController worldController;
    public UIDocument deathScreen;
    public UIDocument mainMenu;

    private void OnEnable()
    {
        //Button restartButton = deathScreen.rootVisualElement.Q<Button>("RestartButton");
        //restartButton.clicked += () => OnRestartButtonClick();
        //restartButton.RegisterCallback<MouseUpEvent>(OnRestartButtonClick);
    }

    private void Awake()
    {
        VisualElement restartButton = deathScreen.rootVisualElement.Q("RestartButton") as Button;
        restartButton.RegisterCallback<ClickEvent>(OnRestartButtonClick);

        //VisualElement restartButton = deathScreen.rootVisualElement.Q("RestartButton") as Button;
        //restartButton.RegisterCallback<ClickEvent>(OnRestartButtonClick);

        /*VisualElement playButton = mainMenu.rootVisualElement.Q("PlayButton") as Button;
        playButton.RegisterCallback<ClickEvent>(OnPlayButtonClick);

        VisualElement shopButton = mainMenu.rootVisualElement.Q("ShopButton") as Button;
        shopButton.RegisterCallback<ClickEvent>(OnShopButtonClick);

        VisualElement settingsButton = mainMenu.rootVisualElement.Q("SettingsButton") as Button;
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClick);*/
    }

    private void OnRestartButtonClick(ClickEvent evt)//REMINDER TO NEVER USE OPACITY AS TRANSITION, IT DOESN'T DISABLE THE UI THINGS AT ALL LOL
    {
        VisualElement screenBG = deathScreen.rootVisualElement.Q("DeathImg");
        VisualElement button = deathScreen.rootVisualElement.Q("RestartButton") as Button;
        button.SetEnabled(false);

        Debug.Log("you did it, scene restarts");
        //DeathScreenEnabled(false);
        //Debug.Log("Restarting");
        string sceneName = SceneManager.GetActiveScene().name;

        GameObject tankWorldController = new GameObject();
        TankEconomyController economyController;
        int totalCoinCount;
        int currentCoinCount;

        List<GameObject> rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects().ToList();
        foreach(GameObject gameObject in rootGameObjects)
        {
            //Debug.Log(gameObject.name);
            if(gameObject.name == "Tank world")
            {
                tankWorldController = gameObject;
            }
        }

        economyController = tankWorldController.GetComponentInChildren<TankEconomyController>();
        //Debug.Log(economyController.gameObject.name);

        totalCoinCount = economyController.TotalCoinCount;
        currentCoinCount = economyController.CurrentCoinCount;
        //Debug.Log("Pre reload: Total coin count = " + economyController.TotalCoinCount + ";\nCurrent coin count = " + economyController.CurrentCoinCount);

        SceneManager.LoadScene(sceneName);
        
        rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects().ToList();
        foreach(GameObject gameObject in rootGameObjects)
        {
            //Debug.Log(gameObject.name);
            if(gameObject.name == "Tank world")
            {
                economyController = gameObject.GetComponentInChildren<TankEconomyController>();
                economyController.TotalCoinCount = currentCoinCount + totalCoinCount;
                economyController.CurrentCoinCount = 0;
            }
        }
        Debug.Log("Post reload: Total coin count = " + economyController.TotalCoinCount + ";\nCurrent coin count = " + economyController.CurrentCoinCount);

        screenBG.style.opacity = 0f;
        button.style.opacity = 0f;
    }

    /*private void OnPlayButtonClick(ClickEvent evt)
    {

    }

    private void OnShopButtonClick(ClickEvent evt)
    {

    }

    private void OnSettingsButtonClick(ClickEvent evt)
    {

    }*/

    public IEnumerator ShowDeathScreen()
    {
        VisualElement screenBG = deathScreen.rootVisualElement.Q("DeathImg");
        VisualElement button = deathScreen.rootVisualElement.Q("RestartButton") as Button;

        yield return new WaitForSeconds(1f);

        button.SetEnabled(true);

        //DeathScreenEnabled(true);

        screenBG.style.opacity = 1f;
        button.style.opacity = 1f;

        //Debug.Log("DEATH SCREEN");
        yield return 0;
    }

    public void DeathScreenEnabled(bool isEnabled)
    {
        deathScreen.gameObject.SetActive(isEnabled);
        Debug.Log("DEATH SCREEN");
    }
}
