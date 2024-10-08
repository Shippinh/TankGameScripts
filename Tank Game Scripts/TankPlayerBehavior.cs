using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TankPlayerBehavior : MonoBehaviour
{
    [SerializeField]
    TankMovement tankMovementReference;
    public TankDeathUI deathUI;
    public TankEconomyController economyController;

    public int playerHP = 3;
    public int playerArmor = 3;

    public bool isUnkillable = false;

    public float towerPower = 10f;

    public List<GameObject> upgrades;
    private int hitCount = 0;
    public List<AudioSource> audioSources;
    public Dictionary<string, float> data;
    public bool isDestroyed = false;
    
    //rewrite this!!!!!
    public Dictionary<string, bool> upgradeDict = new Dictionary<string, bool>();
    void Awake()
    {
        data = TankDataHandler.LoadAllData();
        data.Remove(data.First().Key);

        audioSources = GetComponents<AudioSource>().ToList();

        foreach(GameObject upg in upgrades)
        {
            upgradeDict.Add(upg.name, false); //upgrade name : is active?
        }

        playerArmor = (int)data["ArmorLevel"];

        tankMovementReference = GetComponent<TankMovement>();
        deathUI.deathScreen.rootVisualElement.Q<Button>("RestartButton").SetEnabled(false);//on restart causes null reference
    }

    private IEnumerator Destruction()
    {
        if(!isUnkillable)
        {
            tankMovementReference.enabled = false;
            tankMovementReference.EnableBodyConstraints();

            //Debug.Log("Hit");

            Rigidbody tower = tankMovementReference.tower;
            tower.transform.parent = null;
            tower.isKinematic = false;
            tower.mass = Random.Range(0.4f, 0.7f);

            tower.AddForce(Vector3.up * towerPower, ForceMode.Impulse);
            tower.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * towerPower, ForceMode.Impulse);

            yield return new WaitForSeconds(1f);

            tower.gameObject.GetComponent<SphereCollider>().enabled = true;
            
            //skipping saving if nothing changed
            if((int)data["Coins"] + economyController.CurrentCoinCount != (int)data["Coins"])
            {
                TankDataHandler.SaveAllData((int)data["Coins"], economyController.CurrentCoinCount, (int)data["ThrallLevel"], (int)data["RamLevel"], (int)data["ArmorLevel"]);
                economyController.ResetCurrentCoinCount();
            }

            //deathMenuHandler.DeathScreenEnabled(true);
            StartCoroutine(deathUI.ShowDeathScreen());
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Mine")
        {
            //Debug.Log("Hit Mine");
            if (!upgradeDict["Thrall"])
            {
                playerHP = 0;
                playerArmor = 0;
            }
            else
            {
                Destroy(col.gameObject);
            }
        }

        if (col.collider.tag == "Enemy Tank")
        {
            //Debug.Log("Hit Enemy Tank");
            TankEnemyBehaviour enemyTank = col.gameObject.GetComponent<TankEnemyBehaviour>();
            if (!enemyTank.isDestroyed)
            {
                if (!upgradeDict["Ram"])
                {
                    playerHP = 0;
                    playerArmor = 0;
                }
                else
                {
                    StartCoroutine(enemyTank.Destruction());
                    playerArmor++;
                }
            }
        }
        
        if (col.collider.tag == "Enemy Bullet")
        {
            int audioSourceIndex = Random.Range(0,2);
            hitCount++;

            if (playerArmor > 0)
            {
                playerArmor--;
            }
            else
            {
                playerHP--;
            }

            StartCoroutine(PlayBulletHitSound(audioSources[audioSourceIndex], col));
            Debug.Log("Player got hit " + hitCount + " times");
        }

        if(playerHP == 0)
        {
            if(!isDestroyed)
            {
                StartCoroutine(Destruction());
                isDestroyed = true;
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {

        if (col.tag == "Upgrade Bubble")
        {
            Destroy(col.gameObject);
            foreach(string upg in upgradeDict.Keys.ToList())
            {
                upgradeDict[upg] = false;
            }
            foreach(GameObject obj in upgrades)
            {
                obj.SetActive(false);
            }
            int rand = Random.Range(0, upgradeDict.Count);
            GameObject currentUpgrade = upgrades[rand];
            
            switch(currentUpgrade.name)
            {
                case "Ram":
                    StartCoroutine(ActivateCurrentUpgrade(currentUpgrade, data["RamDuration"]));
                    break;
                case "Thrall":
                    playerArmor += 2;
                    StartCoroutine(ActivateCurrentUpgrade(currentUpgrade, data["ThrallDuration"]));
                    break;
            }
        }
    }

    private IEnumerator PlayBulletHitSound(AudioSource source, Collision col)
    {
        source.pitch = Random.Range(0.8f, 1.3f);
        source.PlayOneShot(source.clip);
        yield return new WaitUntil(() => source.time >= source.clip.length);
    }

    private IEnumerator ActivateCurrentUpgrade(GameObject currentUpg, float duration)
    {
        currentUpg.SetActive(true);
        upgradeDict[currentUpg.name] = true;
        Debug.Log(currentUpg.name + " has been activated");
        yield return new WaitForSeconds(duration);
        Debug.Log(currentUpg.name + " has been deactivated");
        currentUpg.SetActive(false);
        upgradeDict[currentUpg.name] = false;
    }

    public int GetCoinCount()
    {
        return (int)data["Coins"];
    }
}
