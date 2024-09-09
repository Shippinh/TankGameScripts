using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class TankEconomyController : MonoBehaviour
{
    [SerializeField]
    static private int totalCoinCount = 0; //use static to make global variables that store stuff between script activations
    public int TotalCoinCount
    {
        get
        {
            return totalCoinCount;
        }
        set
        {
            totalCoinCount = value;
        }
    }
    [SerializeField]
    static private int currentCoinCount = 0;
    public int CurrentCoinCount
    {
        get
        {
            return currentCoinCount;
        }
        set
        {
            currentCoinCount = value;
        }
    }

    [SerializeField]
    static private int thrallUpgradeLevel = 0;
    public int ThrallUpgradeLevel
    {
        get
        {
            return thrallUpgradeLevel;
        }
        set
        {
            thrallUpgradeLevel = value;
        }
    }

    [SerializeField]
    static private int ramUpgradeLevel = 0;
    public int RamUpgradeLevel
    {
        get
        {
            return ramUpgradeLevel;
        }
        set
        {
            ramUpgradeLevel = value;
        }
    }

    public void IncreaseCurrentCoinCount()
    {
        CurrentCoinCount++;
    }

    public void ResetCurrentCoinCount()
    {
        CurrentCoinCount = 0;
    }

    public static int GetCurrentCointCount()
    {
        return currentCoinCount;
    }
}
