using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CoinLogic : MonoBehaviour
{
    public TankEconomyController economy;

    void Awake()
    {
        List<GameObject> rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects().ToList();
        foreach(GameObject gameObject in rootGameObjects)
        {
            //Debug.Log(gameObject.name);
            if(gameObject.name == "Tank world")
            {
                economy = gameObject.GetComponentInChildren<TankEconomyController>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            economy.IncreaseCurrentCoinCount();
            Destroy(this.gameObject);
        }
    }
}
