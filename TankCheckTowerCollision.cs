using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCheckTowerCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "Player Bullet" && transform.parent.GetComponent<TankEnemyBehaviour>().isDestroyed != true)
        {
            transform.parent.GetComponent<TankEnemyBehaviour>().CollisionDetected(this);
        }
    }

}
