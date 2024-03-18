using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankCheckTowerCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if(transform != null)
        {
            if(transform.parent != null)
            {
                if(transform.parent.GetComponent<TankEnemyBehaviour>() != null)
                {
                    if(col.collider.tag == "Player Bullet" && transform.parent.GetComponent<TankEnemyBehaviour>().isDestroyed != true)
                    {
                        TankEnemyBehaviour thisTowersTank = transform.parent.GetComponent<TankEnemyBehaviour>();
                        float destroyTime = thisTowersTank.destroyTime; //+1f = yield wait in destruction function of enemy tank
                        thisTowersTank.CollisionDetected(this);
                        Destroy(this.gameObject, destroyTime);
                    }
                }
            }
        }
    }
}