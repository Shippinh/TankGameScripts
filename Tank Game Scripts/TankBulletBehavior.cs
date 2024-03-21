using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBulletBehavior : MonoBehaviour
{
    public float speed;
    public float destroyTime = 20f;
    public float ineffectiveTime = 10f;
    public float moveTime = 1f;
    public bool useConstantFlying = false;

    [SerializeField]
    Rigidbody bullet;

    // Start is called before the first frame update
    void Awake()
    {
        if(gameObject.tag == "Player Bullet")
        {
            Physics.IgnoreLayerCollision(12, 13);
        }
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

    private void OnCollisionExit(Collision col)
    {
        if (this.tag == "Enemy Bullet")
            if (col.collider.tag == "Player")
            {
                bullet.gameObject.tag = "Non Effective Bullet";
                bullet.useGravity = false;
            }
    }

    private void AddForceToBullet()//physics based shot, used when we need to apply force once
    {
        Vector3 bulletDirection = transform.worldToLocalMatrix.inverse * Vector3.forward * speed;
        bullet.AddForce(bulletDirection, ForceMode.Impulse);
        StartCoroutine(MakeBulletIneffective());
        Destroy(bullet.gameObject, destroyTime);
    }

    private IEnumerator MakeBulletIneffective()
    {
        yield return new WaitForSeconds(ineffectiveTime);
        bullet.gameObject.tag = "Non Effective Bullet";
    }
}
