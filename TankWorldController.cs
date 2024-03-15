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
        public GameObject moneyPrefab;
        public GameObject platformPrefab;

        public bool firstPlatform = false;

        public float destroyTime = 20f;

        private void Awake()
        {
            Destroy(this.gameObject, destroyTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "Player")
            {
                //Debug.Log("Hit!");
                GameObject nextPlatform = Instantiate(platformPrefab, this.transform.position + new Vector3(0f, 0f, 50f), Quaternion.identity);
                int i = Random.Range(0, 4);
                while (i < 4)
                {
                    Instantiate(enemyTankPrefab, this.transform.position + new Vector3(Random.Range(-5f, 5f), 1.3f, Random.Range(20f, 80f)), Quaternion.identity);
                    i++;
                }
                i = Random.Range(0, 3);
                while (i < 3)
                {
                    Instantiate(minePrefab, this.transform.position + new Vector3(Random.Range(-5f, 5f), 0.54f, Random.Range(60f, 80f)), Quaternion.identity);
                    i++;
                }
                nextPlatform.name = "New Platform";
            }
        }
    }
}