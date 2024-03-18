using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TankPlayerBehavior : MonoBehaviour
{
    [SerializeField]
    TankMovement tankMovementReference;
    public TankMenuHandler deathMenuHandler;

    public uint playerHP = 3;
    public uint playerArmor = 3;

    public bool isUnkillable = false;

    public float towerPower = 10f;
    bool isDestroyed = false;
    // Start is called before the first frame update
    void Awake()
    {
        tankMovementReference = GetComponent<TankMovement>();
        deathMenuHandler.deathScreen.rootVisualElement.Q<Button>("RestartButton").SetEnabled(false);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

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

            isDestroyed = true;

            //audioSource.pitch = Random.Range(0.8f, 1.3f);
            //audioSource.Play();

            tower.AddForce(Vector3.up * towerPower, ForceMode.Impulse);
            tower.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * towerPower, ForceMode.Impulse);

            yield return new WaitForSeconds(1f);

            tower.gameObject.GetComponent<SphereCollider>().enabled = true;
            
            //deathMenuHandler.DeathScreenEnabled(true);
            StartCoroutine(deathMenuHandler.ShowDeathScreen());
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Mine")
        {
            playerHP = 0;
            playerArmor = 0;
        }

        if (col.collider.tag == "Enemy Tank")
        {
            if (col.gameObject.GetComponent<TankEnemyBehaviour>().isDestroyed == false)
            {
                playerHP = 0;
                playerArmor = 0;
            }
        }

        if (col.collider.tag == "Enemy Bullet")
        {
            if (playerArmor > 0)
                playerArmor--;
            else
                playerHP--;
            //Debug.Log("Hit");
        }

        if(playerHP == 0)
        {
            StartCoroutine(Destruction());
        }
    }
}
