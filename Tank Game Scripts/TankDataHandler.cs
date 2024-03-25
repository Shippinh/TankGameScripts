using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Unity.VisualScripting;

public static class TankDataHandler
{
    private static bool saveCoins = true;
    public static bool SaveCoins
    {
        get
        {
            return saveCoins;
        }
        set
        {
            saveCoins = value;
        }
    }
    
    private static bool saveUpgrades = true;
    public static bool SaveUpgrades
    {
        get
        {
            return saveUpgrades;
        }
        set
        {
            saveUpgrades = value;
        }
    }
    
    private static int savedCoinCount;
    public static int SavedCoinCount
    {
        set
        {
            savedCoinCount = value;
        }
        get
        {
            return savedCoinCount;
        }
    }

    private static float ramDuration;
    private static int ramUpgradeLevel;
    public static int RamUpgradeLevel
    {
        set
        {
            ramUpgradeLevel = value;
        }
        get
        {
            return ramUpgradeLevel;
        }
    }

    private static float thrallDuration;
    private static int thrallUpgradeLevel;
    public static int ThrallUpgradeLevel
    {
        set
        {
            thrallUpgradeLevel = value;
        }
        get
        {
            return thrallUpgradeLevel;
        }
    }

    public static void SaveAllData(bool doSaveCoins, bool doSaveUpgrades, TankEconomyController economyController)
    {
        SaveCoins = doSaveCoins;
        SaveUpgrades = doSaveUpgrades;

        string content = "";

        if(SaveCoins)
        {
            content += "\nCoins:" + (economyController.TotalCoinCount + economyController.CurrentCoinCount).ToString();
        }
        if(SaveUpgrades)
        {
            content += "\nThrallLevel:" + economyController.ThrallUpgradeLevel.ToString();
            content += "\nRamLevel:" + economyController.RamUpgradeLevel.ToString();
        }

        if(content == "")
        {
            Debug.Log("There is nothing to save");
        }
        else
        {
            UpdateTextFile(content);
        }
    }

    private static void UpdateTextFile(string content) 
    {
        //Path of the file
        string path = Application.dataPath + "/Save Data.txt";
        //Clear previous save data
        File.WriteAllText(path, "");
        //Write new save data
        File.WriteAllText(path, content);
    }

    //preferably do this kind of calculation once in main menu
    private static float CalculateDurationRam(int upgradeLevel)
    {
        float output = 0;
        float e = (float)System.Math.E;
        for(int i = 0; i < upgradeLevel; i++)
        {
            output += (float)System.Math.Round(3 * System.Math.Pow(e / 3, i), 2);
            //Debug.Log(output);
        }
        return output;
    }

    private static float CalculateDurationThrall(int upgradeLevel)
    {
        float output = 0;
        float e = (float)System.Math.E;
        for(int i = 0; i < upgradeLevel; i++)
        {
            output += (float)System.Math.Round(2 * System.Math.Pow(e / 2, i), 2);
            //Debug.Log(output);
        }
        return output;
    }
}
