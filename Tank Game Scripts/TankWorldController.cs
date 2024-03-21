using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityEngine
{

    public class TankWorldController : MonoBehaviour
    {
        public BoxCollider platformSpawnTrigger;
        public GameObject enemyTankPrefab;
        public GameObject minePrefab;
        public GameObject coinPacks;
        public GameObject platformPrefab;
        [SerializeField]

        public bool firstPlatform = false;

        public float destroyTime = 20f;
        private void Awake()
        {
            Destroy(this.gameObject, destroyTime);
        }

        /*private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player" || collision.gameObject.name == "Tank" || collision.gameObject.name == "Tank Body")
            {
                //Debug.Log("Hit!");
                GameObject nextPlatform = Instantiate(platformPrefab, this.transform.position + new Vector3(0f, 0f, 50f), Quaternion.identity);
                int i = Random.Range(0, 4);
                while (i < 4)
                {
                    //for some reason instantiates like ass and sometimes causes tank towers to be unproportional to the prefab
                    Instantiate(enemyTankPrefab, this.transform.position + new Vector3(Random.Range(-4f, 4f), 1.3f, Random.Range(10f, 80f)), Quaternion.identity);
                    i++;
                }
                i = Random.Range(0, 3);
                while (i < 3)
                {
                    Instantiate(minePrefab, this.transform.position + new Vector3(Random.Range(-5f, 5f), 0.54f, Random.Range(60f, 80f)), Quaternion.identity);
                    i++;
                }
                if(i < 4)
                {
                    int coinPackIndex = Random.Range(0, 4);
                    //Debug.Log(coinPacks.transform.GetChild(coinPackIndex).gameObject.name);
                    Instantiate(coinPacks.transform.GetChild(coinPackIndex).gameObject, this.transform.position + new Vector3(0f, 1.5f, Random.Range(60f, 80f)), Quaternion.identity);
                }
                nextPlatform.name = "New Platform";
            }
        }*/

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "Player" || collision.gameObject.name == "Tank" || collision.gameObject.name == "Tank Body")
            {
                //Debug.Log("Hit!");
                GameObject nextPlatform = Instantiate(platformPrefab, this.transform.position + new Vector3(0f, 0f, 50f), Quaternion.identity);
                int i = Random.Range(0, 4);
                while (i < 4)
                {
                    //for some reason instantiates like ass and sometimes causes tank towers to be unproportional to the prefab
                    Instantiate(enemyTankPrefab, this.transform.position + new Vector3(Random.Range(-4f, 4f), 1.3f, Random.Range(10f, 80f)), Quaternion.identity);
                    i++;
                }
                i = Random.Range(0, 3);
                while (i < 3)
                {
                    Instantiate(minePrefab, this.transform.position + new Vector3(Random.Range(-5f, 5f), 0.54f, Random.Range(60f, 80f)), Quaternion.identity);
                    i++;
                }
                if(i < 4)
                {
                    int coinPackIndex = Random.Range(0, 4);
                    //Debug.Log(coinPacks.transform.GetChild(coinPackIndex).gameObject.name);
                    Instantiate(coinPacks.transform.GetChild(coinPackIndex).gameObject, this.transform.position + new Vector3(0f, 1.5f, Random.Range(60f, 80f)), Quaternion.identity);
                }
                nextPlatform.name = "New Platform";
            }
        }
    }
}