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

    public uint playerHP = 3;
    public uint playerArmor = 3;

    public bool isUnkillable = false;

    public float towerPower = 10f;

    public List<GameObject> upgrades;
    private int hitCount = 0;
    public List<AudioSource> audioSources;

    public class DoubleBooleanPair
    {
        public double doublefloat { get; set; }
        public bool boolean { get; set; }
        public DoubleBooleanPair(double d, bool b)
        {
            doublefloat = d;
            boolean = b;
        }
    }
    
    
    public Dictionary<string, DoubleBooleanPair> upgradeDict = new Dictionary<string, DoubleBooleanPair>();
    //bool isDestroyed = false;
    // Start is called before the first frame update
    void Awake()
    {
        audioSources = GetComponents<AudioSource>().ToList();
        foreach(GameObject upg in upgrades)
        {
            upgradeDict.Add(upg.name, new DoubleBooleanPair(1d, false));
        }
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

            //isDestroyed = true;

            //audioSource.pitch = Random.Range(0.8f, 1.3f);
            //audioSource.Play();

            tower.AddForce(Vector3.up * towerPower, ForceMode.Impulse);
            tower.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * towerPower, ForceMode.Impulse);

            yield return new WaitForSeconds(1f);

            tower.gameObject.GetComponent<SphereCollider>().enabled = true;
            
            //deathMenuHandler.DeathScreenEnabled(true);
            StartCoroutine(deathUI.ShowDeathScreen());
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Mine")
        {
            //Debug.Log("Hit Mine");
            if (!upgradeDict["Thrall"].boolean)
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
                if (!upgradeDict["Ram"].boolean)
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
                StartCoroutine(WaitForSoundToFinish(audioSources[audioSourceIndex]));
            }
            else
            {
                playerHP--;
                StartCoroutine(WaitForSoundToFinish(audioSources[audioSourceIndex]));//better change this later
            }
            //Debug.Log("Player got hit " + hitCount + " times");
        }

        if(playerHP == 0)
        {
            StartCoroutine(Destruction());
        }
    }

    private void OnTriggerEnter(Collider col)
    {

        if (col.tag == "Upgrade Bubble")
        {
            Destroy(col.gameObject);
            foreach(string upg in upgradeDict.Keys.ToList())
            {
                upgradeDict[upg].boolean = false;
            }
            int rand = Random.Range(0, upgradeDict.Count);
            GameObject currentUpgrade = upgrades[rand];
            currentUpgrade.SetActive(true);
            upgradeDict[currentUpgrade.name].boolean = true;
        }
    }

    private IEnumerator WaitForSoundToFinish(AudioSource source)
    {
        source.pitch = Random.Range(0.8f, 1.3f);
        source.Play();
        yield return new WaitUntil(() => source.time >= source.clip.length);
    }
}
