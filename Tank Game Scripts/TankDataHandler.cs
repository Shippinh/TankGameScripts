using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;

public static class TankDataHandler
{
    const float defaultThrallDuration = 3f;
    const float defaultRamDuration = 4f;
    public static void SaveAllData(int totalCoinCount, int currentCoinCount, int thrallUpgradeLevel, int ramUpgradeLevel, int armorUpgradeLevel)
    {
        string content = "";

        content += "\nCoins:" + (totalCoinCount + currentCoinCount).ToString();

        content += "\nThrallLevel:" + thrallUpgradeLevel.ToString();
        content += "\nRamLevel:" + ramUpgradeLevel.ToString();
        content += "\nArmorLevel:" + armorUpgradeLevel.ToString();

        content += "\nThrallDuration:" + CalculateDurationThrall(thrallUpgradeLevel).ToString();
        content += "\nRamDuration:" + CalculateDurationRam(ramUpgradeLevel).ToString();

        Debug.Log("Saving");
        UpdateTextFile(content).Wait();
        Debug.Log("Save complete");
    }

    public static void SaveAllData()
    {
        string content = "";

        content += "\nCoins:" + 0.ToString();

        content += "\nThrallLevel:" + 1.ToString();
        content += "\nRamLevel:" + 1.ToString();
        content += "\nArmorLevel:" + 3.ToString();

        content += "\nThrallDuration:" + defaultThrallDuration.ToString();
        content += "\nRamDuration:" + defaultRamDuration.ToString();

        Debug.Log("Saving");
        UpdateTextFile(content).Wait();
        Debug.Log("Save complete");
    }

    /// <summary>
    /// Checks if the game has generated a save file
    /// </summary>
    /// <returns>True if file is present, false if file doesn't exist</returns>
    public static bool CheckForData()
    {
        string path = Application.dataPath + "/Save Data.txt";
        Dictionary<string, float> content = new Dictionary<string, float>();
        try
        {
            if(new FileInfo(path).Length == 0)
            {
            }
        }
        catch
        {
            return false;
        }
        return true;
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
    private static float CalculateDurationRam(int upgradeLevel)
    {
        float output = defaultRamDuration - 0.4f;
        //float e = (float)System.Math.E;
        for(int i = 0; i < upgradeLevel; i++)
        {
            /*output += (float)System.Math.Round(3 * System.Math.Pow(e / 3, i), 2);
            Debug.Log(output);*/
            if(i < 20)
            {
                output += 0.4f;
            }
            else if(i < 40 && i >= 20)
            {
                output += 0.2f;
            }
            else
            {
                output += 0.05f;
            }
        }
        return output;
    }

    private static float CalculateDurationThrall(int upgradeLevel)
    {
        float output = defaultThrallDuration - 0.3f;
        //float e = (float)System.Math.E;
        for(int i = 0; i < upgradeLevel; i++)
        {
            /*output += (float)System.Math.Round(2 * System.Math.Pow(e / 2, i), 2);
            Debug.Log(output);*/
            if(i < 15)
            {
                output += 0.3f;
            }
            else if(i < 30  && i >= 15)
            {
                output += 0.15f;
            }
            else
            {
                output += 0.05f;
            }
        }
        return output;
    }
}
