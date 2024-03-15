using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public GameObject[] toDisable;
    public GameObject[] toActivate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (GameObject obj in toDisable)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in toActivate)
        {
            obj.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
