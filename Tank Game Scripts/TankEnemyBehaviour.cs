using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityEngine
{
    public class TankEnemyBehaviour : MonoBehaviour
    {
        [SerializeField]
        public Rigidbody tower;

        [SerializeField]
        public Rigidbody bullet;

        [SerializeField]
        public Rigidbody body;

        [SerializeField]
        public GameObject playerTankBody;

        [SerializeField]
        public Collider towerCollider;

        [SerializeField]
        public GameObject towerCannon;

        [SerializeField]
        AudioSource audioSource;

        [SerializeField]
        public GameObject obj;

        public bool isDestroyed = false;
        public bool canAim = true;
        public bool canDrive = false;
        public bool drivesLeft = true;

        public float towerPower = 10f;
        public float destroyTime = 20f;
        public float shootTime = 2f;
        public float driveDistance = 3f;
        private void Awake()
        {
            Physics.IgnoreCollision(tower.GetComponent<Collider>(), GetComponent<BoxCollider>(), true);
            playerTankBody = GameObject.FindGameObjectWithTag("Player");
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(Shoot(shootTime));
            Destroy(obj, destroyTime + 20f);
            Destroy(tower.gameObject, destroyTime + 20f);
            RandomiseProperties();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (canAim)
                Aim();
            if (canDrive)
                Drive();
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.collider.tag == "Player Bullet" && !isDestroyed)
            {
                StartCoroutine(Destruction());
            }
            if (col.collider.tag == "Wall" && !isDestroyed)
            {
                drivesLeft = !drivesLeft;
            }
        }

        private IEnumerator Destruction()
        {
            //Debug.Log("Hit");
            tower.transform.parent = null;
            tower.isKinematic = false;
            tower.constraints = RigidbodyConstraints.None;
            body.constraints = RigidbodyConstraints.None;

            tower.mass = Random.Range(0.4f, 0.7f);
            isDestroyed = true;

            audioSource.pitch = Random.Range(0.8f, 1.3f);
            audioSource.Play();

            tower.AddForce(Vector3.up * towerPower, ForceMode.Impulse);
            tower.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * towerPower, ForceMode.Impulse);

            yield return new WaitForSeconds(1f);

            Physics.IgnoreCollision(tower.GetComponent<Collider>(), GetComponent<BoxCollider>(), false);
            Destroy(obj, destroyTime);
            Destroy(tower.gameObject, destroyTime);
        }

        private void Aim()
        {
            if (!isDestroyed)
            {
                float distanceToPlane = Vector3.Dot(this.transform.up, playerTankBody.transform.position - tower.position);
                Vector3 planePoint = playerTankBody.transform.position - this.transform.up * distanceToPlane;
                tower.transform.LookAt(planePoint, this.transform.up);
            }
        }

        private IEnumerator Shoot(float bTime)
        {
            while (1 < 2)//lol
            {
                if (!isDestroyed)
                {
                    Instantiate(bullet, towerCannon.transform.position, towerCannon.transform.rotation);
                    yield return new WaitForSeconds(bTime);
                }
                else
                    break;
            }
        }

        private void RandomiseProperties()
        {
            //Aiming
            if (Random.Range(0, 2) == 1)//50/50
            {
                canAim = true;
            }
            else
            {
                canAim = false;
            }

            //Driving
            if (Random.Range(0, 2) == 1)// 50/50
            {
                canDrive = true;
                tower.isKinematic = true;
                tower.constraints = RigidbodyConstraints.None;
                body.constraints = RigidbodyConstraints.None;
                body.constraints = RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                canDrive = false;
            }
            if (!canAim)
                obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f); //rotate the tower towards player on spawn if can't aim
        }

        public void CollisionDetected(TankCheckTowerCollision childScript)
        {
            StartCoroutine(Destruction());
        }

        private void Drive()
        {
            if (!isDestroyed)
            {
                if (drivesLeft)
                {
                    body.MovePosition(body.position + Vector3.left * driveDistance * Time.fixedDeltaTime);
                }
                else
                {
                    body.MovePosition(body.position + Vector3.right * driveDistance * Time.fixedDeltaTime);
                }
            }
        }
    }
}
