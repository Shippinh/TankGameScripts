using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBulletBehavior : MonoBehaviour
{
    public float speed;
    public float destroyTime = 20f;
    public float moveTime = 1f;
    public bool useConstantFlying = false;

    [SerializeField]
    Rigidbody bullet;

    // Start is called before the first frame update
    void Awake()
    {
        bullet = GetComponent<Rigidbody>();
        if (useConstantFlying)
        {
            StartCoroutine(MoveBulletOverTime(moveTime));
        }
        else
            AddForceToBullet();
        Destroy(bullet.gameObject, destroyTime);
    }

    /*// Update is called once per frame
    void FixedUpdate()
    {
        Destroy(bullet.gameObject, destroyTime);
    }*/

    private IEnumerator MoveBulletOverTime(float moveT)//for constant flying
    {
        float t = 0f;
        while (t < moveT)
        {
            Vector3 bulletDirection = transform.worldToLocalMatrix.inverse * Vector3.forward * speed * Time.fixedDeltaTime;
            bullet.MovePosition(bullet.transform.position + bulletDirection);
            t += Time.deltaTime;
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (this.tag == "Enemy Bullet")
            if (col.collider.tag == "Player")
            {
                bullet.useGravity = false;
            }
    }

    private void AddForceToBullet()//physics based shot, used when we need to apply force once
    {
        Vector3 bulletDirection = transform.worldToLocalMatrix.inverse * Vector3.forward * speed;
        bullet.AddForce(bulletDirection, ForceMode.Impulse);
        Destroy(bullet.gameObject, destroyTime);
    }
}
