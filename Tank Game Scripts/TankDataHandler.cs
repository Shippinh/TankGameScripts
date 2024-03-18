using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TankDataHandler : MonoBehaviour
{
    private bool loadCoins = true;
    public bool LoadCoins
    {
        get
        {
            return loadCoins;
        }
        set
        {
            loadCoins = value;
        }
    }
    private bool loadUpgrades;
    public bool LoadUpgrades
    {
        get
        {
            return loadUpgrades;
        }
        set
        {
            loadUpgrades = value;
        }
    }
    private int savedCoinCount;

    // Start is called before the first frame update
    void Awake()
    {
        if(LoadCoins)
        {
            UpdateTextFile("Some Content");
        }
    }

    void UpdateTextFile(string content) 
    {
        //Path of the file
        string path = Application.dataPath + "/Save Data.txt";
        //Set file text
        File.WriteAllText(path, content);
    }
}
