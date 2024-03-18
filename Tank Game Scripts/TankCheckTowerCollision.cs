using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCheckTowerCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        //this still needs more work, all those nullReferences are not good
        if(col.collider.tag == "Player Bullet" && transform.parent.GetComponent<TankEnemyBehaviour>().isDestroyed != true)
        {
            TankEnemyBehaviour thisTowersTank = transform.parent.GetComponent<TankEnemyBehaviour>();
            float destroyTime = thisTowersTank.destroyTime; //+1f = yield wait in destruction function of enemy tank
            thisTowersTank.CollisionDetected(this);
            Destroy(this.gameObject, destroyTime);
        }
    }
}