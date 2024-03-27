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
        //nevermind, i give up
        /*data = TankDataHandler.LoadAllData(); //doing this in awake cause i have to load values for the shop initially
        data.Remove(data.First().Key); //*/

        //call app prefs here to load volume data
        GoToMainMenu();
        /*foreach(var element in data)
        {
            Debug.Log("Loading data: 'Key = " + element.Key + "; Value = " + element.Value + ";'");
        }*/
    }

    void Start()
    {
        //initializing values for Global Audio Mixer (the one that regulates volume of Audio Sources (that means i need to include mixer in all audio sources))
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

        playButton.clicked += () => SceneManager.LoadSceneAsync(scenePath + "TankWorld.unity");//redo this at some point
        shopButton.clicked += () => GoToShop();
        settingsButton.clicked += () => GoToSettings();
        #endregion

        #region Shop Menu
        Button returnFromShopButton = shopMenuRoot.Q<Button>("BackToMainMenuButton");
        Label coinsLabel = shopMenuRoot.Q<Label>("CoinsLabel");

        Label ramBonusLabel = shopMenuRoot.Q<Label>("RamBonusLabel");
        Label thrallBonusLabel = shopMenuRoot.Q<Label>("ThrallBonusLabel");
        Label armorBonusLabel = shopMenuRoot.Q<Label>("ArmorBonusLabel");

        Label ramCostLabel = shopMenuRoot.Q<Label>("RamCostLabel");
        Label thrallCostLabel = shopMenuRoot.Q<Label>("ThrallCostLabel");
        Label armorCostLabel = shopMenuRoot.Q<Label>("ArmorCostLabel");

        Button upgradeRamButton = shopMenuRoot.Q<Button>("UpgradeRamButton");
        Button upgradeThrallButton = shopMenuRoot.Q<Button>("UpgradeThrallButton");
        Button upgradeArmorButton = shopMenuRoot.Q<Button>("UpgradeArmorButton");
        
        returnFromShopButton.clicked += () => GoToMainMenuAndSave();

        upgradeRamButton.clicked += () => 
        {
            if(data["Coins"] >= 20)
            {
                data["RamLevel"]++;
                data["Coins"] -= 20;
                TankDataHandler.SaveAllData((int)data["Coins"], 0, (int)data["ThrallLevel"], (int)data["RamLevel"]);
                data = TankDataHandler.LoadAllData(); //super stupid
                data.Remove(data.First().Key); //i hate this so much

                coinsLabel.text = "Coins: " + data["Coins"];

                ramBonusLabel.text = "Level: " 
                + data["RamLevel"].ToString() 
                + "\nCurrent bonuses:\n+" 
                + data["RamDuration"].ToString() 
                + " to Duration\n+1 Armor per rammed tank";
            }
        };

        upgradeThrallButton.clicked += () => 
        {
            if(data["Coins"] >= 20)
            {
                data["ThrallLevel"]++;
                data["Coins"] -= 40;
                TankDataHandler.SaveAllData((int)data["Coins"], 0, (int)data["ThrallLevel"], (int)data["RamLevel"]);
                data = TankDataHandler.LoadAllData(); //super stupid
                data.Remove(data.First().Key); //even dumber

                coinsLabel.text = "Coins: " + data["Coins"];

                thrallBonusLabel.text = "Level: " 
                + data["ThrallLevel"].ToString() 
                + "\nCurrent bonuses:\n+" 
                + data["ThrallDuration"].ToString() + " to Duration";
            }
        };
        
        #endregion

        #region Settings Menu
        Button sendToPaymentButton = settingsMenuRoot.Q<Button>("SendToPaymentButton");
        Button returnFromSettingsButton = settingsMenuRoot.Q<Button>("BackToMainMenuButton");

        returnFromSettingsButton.clicked += () => GoToMainMenu();

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
        data = TankDataHandler.LoadAllData(); //doing this in awake cause i have to load values for the shop initially
        data.Remove(data.First().Key);

        var shopMenuRoot = shopMenu.rootVisualElement;
        DisableDisplayStyles();
        shopMenu.rootVisualElement.style.display = DisplayStyle.Flex;

        Label coinsLabelInst = shopMenuRoot.Q<Label>("CoinsLabel");
        coinsLabelInst.text = "Coins: " + data["Coins"].ToString();

        Label ramBonusLabelInst = shopMenuRoot.Q<Label>("RamBonusLabel");
        Label thrallBonusLabelInst = shopMenuRoot.Q<Label>("ThrallBonusLabel");
        //Label armorBonusLabelInst = shopMenuRoot.Q<Label>("ArmorBonusLabel");//implement later

        ramBonusLabelInst.text = "Level: " 
        + data["RamLevel"].ToString() 
        + "\nCurrent bonuses:\n+" 
        + data["RamDuration"].ToString() 
        + " to Duration\n+1 Armor per rammed tank";

        thrallBonusLabelInst.text = "Level: " 
        + data["ThrallLevel"].ToString() 
        + "\nCurrent bonuses:\n+" 
        + data["ThrallDuration"].ToString() + " to Duration";
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
        TankDataHandler.SaveAllData((int)data["Coins"], 0, (int)data["ThrallLevel"], (int)data["RamLevel"]);
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
