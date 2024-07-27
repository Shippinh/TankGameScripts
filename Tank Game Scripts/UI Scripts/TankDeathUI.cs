using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.PlasticSCM.Editor.WebApi;

public class TankDeathUI : MonoBehaviour
{
    public TankPlayerBehavior player;
    public TankEconomyController economyController;
    public UIDocument deathScreen;
    private string scenePath = "Assets/Scenes/";

    //rewrite this!!!
    void OnEnable()
    {
        //Button restartButton = deathScreen.rootVisualElement.Q<Button>("RestartButton");
        //restartButton.clicked += () => OnRestartButtonClick();
        //restartButton.RegisterCallback<MouseUpEvent>(OnRestartButtonClick);
        VisualElement restartButton = deathScreen.rootVisualElement.Q("RestartButton") as Button;
        restartButton.RegisterCallback<ClickEvent>(OnRestartButtonClick);

        Button returnButton = deathScreen.rootVisualElement.Q<Button>("ReturnButton");
        returnButton.clicked += () => SceneManager.LoadScene(scenePath + "MainMenuScene.unity");
    }

    void Start()
    {
        Button restartButton = deathScreen.rootVisualElement.Q<Button>("RestartButton");
        Button returnButton = deathScreen.rootVisualElement.Q<Button>("ReturnButton");

        restartButton.SetEnabled(false);
        returnButton.SetEnabled(false);
    }

    void Awake()
    {
        deathScreen = GetComponent<UIDocument>();
    }

    private void OnRestartButtonClick(ClickEvent evt)//REMINDER TO NEVER USE OPACITY AS TRANSITION, IT DOESN'T DISABLE THE UI THINGS AT ALL LOL
    {
        VisualElement screenBG = deathScreen.rootVisualElement.Q("DeathImg");
        VisualElement button = deathScreen.rootVisualElement.Q("RestartButton") as Button;
        Button returnButton = deathScreen.rootVisualElement.Q<Button>("ReturnButton");
        button.SetEnabled(false);
        returnButton.SetEnabled(false);

        Debug.Log("you did it, scene restarts");
        //DeathScreenEnabled(false);
        //Debug.Log("Restarting");
        string sceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(sceneName);

        screenBG.style.opacity = 0f;
        button.style.opacity = 0f;
        returnButton.style.opacity = 0f;
    }

    public IEnumerator ShowDeathScreen()
    {
        VisualElement screenBG = deathScreen.rootVisualElement.Q("DeathImg");
        Button restartButton = deathScreen.rootVisualElement.Q<Button>("RestartButton");
        Button returnButton = deathScreen.rootVisualElement.Q<Button>("ReturnButton");

        yield return new WaitForSeconds(1f);

        restartButton.SetEnabled(true);
        returnButton.SetEnabled(true);

        //DeathScreenEnabled(true);

        screenBG.style.opacity = 1f;
        restartButton.style.opacity = 1f;
        returnButton.style.opacity = 1f;

        //Debug.Log("DEATH SCREEN");
        yield return 0;
    }

    public void DeathScreenEnabled(bool isEnabled)
    {
        deathScreen.gameObject.SetActive(isEnabled);
        Debug.Log("DEATH SCREEN");
    }
}
