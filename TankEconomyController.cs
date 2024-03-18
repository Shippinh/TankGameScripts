using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class TankEconomyController : MonoBehaviour
{
    [SerializeField]
    static private int totalCoinCount = 0; //use static to make global variables that persist between scripts
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

    public void IncreaseCurrentCoinCount()
    {
        CurrentCoinCount++;
    }
}
