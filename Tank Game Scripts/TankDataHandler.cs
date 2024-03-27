using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;

public static class TankDataHandler
{
    public static void SaveAllData(TankEconomyController economyController)
    {
        string content = "";

        content += "\nCoins:" + (economyController.TotalCoinCount + economyController.CurrentCoinCount).ToString();

        content += "\nThrallLevel:" + economyController.ThrallUpgradeLevel.ToString();
        content += "\nRamLevel:" + economyController.RamUpgradeLevel.ToString();

        content += "\nThrallDuration:" + CalculateDurationThrall(economyController.ThrallUpgradeLevel).ToString();
        content += "\nRamDuration:" + CalculateDurationRam(economyController.RamUpgradeLevel).ToString();

        Debug.Log("Saving");
        UpdateTextFile(content).Wait();
        Debug.Log("Save complete");
    }

    public static void SaveAllData(int totalCoinCount, int currentCoinCount, int thrallUpgradeLevel, int ramUpgradeLevel)
    {
        string content = "";

        content += "\nCoins:" + (totalCoinCount + currentCoinCount).ToString();

        content += "\nThrallLevel:" + thrallUpgradeLevel.ToString();
        content += "\nRamLevel:" + ramUpgradeLevel.ToString();

        content += "\nThrallDuration:" + CalculateDurationThrall(thrallUpgradeLevel).ToString();
        content += "\nRamDuration:" + CalculateDurationRam(ramUpgradeLevel).ToString();

        Debug.Log("Saving");
        UpdateTextFile(content).Wait();
        Debug.Log("Save complete");
    }

    /*public static void SaveAllData(Dictionary<string, float> data)
    {
        string content = "";

        foreach(var element in data.Keys)//this is retarded lol
        {

        }

        Debug.Log("Saving");
        UpdateTextFile(content).Wait();
        Debug.Log("Save complete");
    }*/

    public static void SaveAllData()
    {
        string content = "";

        content += "\nCoins:" + 0.ToString();

        content += "\nThrallLevel:" + 1.ToString();
        content += "\nRamLevel:" + 1.ToString();

        content += "\nThrallDuration:" + CalculateDurationThrall(1).ToString();
        content += "\nRamDuration:" + CalculateDurationRam(1).ToString();

        Debug.Log("Saving");
        UpdateTextFile(content).Wait();
        Debug.Log("Save complete");
    }

    public static Dictionary<string, float> LoadAllData()//this doesn't check if duration values are proper, might change it soon
    {
        string path = Application.dataPath + "/Save Data.txt";
        Dictionary<string, float> content = new Dictionary<string, float>();

        /*foreach(string str in File.ReadLines(path))
        {
            //Debug.Log(str);
            Debug.Log(str.Split(":").Last());
        }*/
        

        if(new FileInfo(path).Length != 0)
        {
            foreach(string str in File.ReadLines(path))
            {
                float value;
                string key;

                float.TryParse(str.Split(":").Last(), out value);//number
                key = str.Split(":").First();

                content.Add(key, value);
            }
            return content;
        }
        else
        {
            Debug.Log("The save file doesn't contain any strings");
            Debug.Log("Creating new save file...");
            SaveAllData();
            Debug.Log("New save file created!");

            foreach(string str in File.ReadLines(path))
            {
                float value;
                string key;

                float.TryParse(str.Split(":").Last(), out value);//number
                key = str.Split(":").First();

                content.Add(key, value);
            }
        }
        return content;
    }
    

    private static Task UpdateTextFile(string content) 
    {
        Debug.Log("Saving data to '" + Application.dataPath + "/Save Data.txt" + "'");
        //Path of the file
        string path = Application.dataPath + "/Save Data.txt";
        //Clear previous save data
        File.WriteAllText(path, "");
        //Write new save data
        File.WriteAllText(path, content);

        return Task.CompletedTask;
    }

    //preferably do this kind of calculation once in main menu
    //fuck around more, those function are very unpredictable
    private static float CalculateDurationRam(int upgradeLevel)
    {
        float output = 0;
        float e = (float)System.Math.E;
        for(int i = 0; i < upgradeLevel; i++)
        {
            output += (float)System.Math.Round(3 * System.Math.Pow(e / 3, i), 2);
            Debug.Log(output);
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
            Debug.Log(output);
        }
        return output;
    }
}
