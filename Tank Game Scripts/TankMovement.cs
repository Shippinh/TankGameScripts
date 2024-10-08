using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float speed = 0.25f;

    Quaternion desiredRotation, desiredTowerRotation;
    Vector3 towerRotationDirection;
    [SerializeField]
    Vector3 desiredVelocity;

    Vector2 playerInput;
    Ray mouseRay;
    Vector3 touchInput;
    float fireInput;
    float fireDelay = 2f; //how long to wait before allowing another shot
    float currentFireDelay = 0f;

    [SerializeField]
    public Rigidbody tower;

    [SerializeField]
    protected Rigidbody body;

    [SerializeField]
    public GameObject tankParent;

    [SerializeField]
    public GameObject bullet;

    [SerializeField]
    public GameObject turretCannon;

    bool hasFired = false;

    public AudioSource soundSource;
    public AudioClip shoot1;
    public AudioClip shoot2;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        //playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        fireInput = Input.GetAxis("Fire2");

        //this is fundamentally broken because i just had to use MovePosition instead of AddForce
        if(transform.position.x > 5)
            desiredVelocity = new Vector3(-1f, 0f, 1f).normalized * speed;
        else if(transform.position.x < -5)
            desiredVelocity = new Vector3(1f, 0f, 1f).normalized * speed;
        else
            desiredVelocity = new Vector3(playerInput.x, 0f, 1f).normalized * speed;

        if(Input.GetAxis("Fire1") != 0)
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

    void FixedUpdate()
    {
        desiredRotation = Quaternion.LookRotation(new Vector3(playerInput.x, 0f, 1f));
        desiredRotation = Quaternion.RotateTowards(body.rotation, desiredRotation, 360 * Time.fixedDeltaTime);

        body.MovePosition(body.position + desiredVelocity * speed * Time.fixedDeltaTime);
        body.MoveRotation(desiredRotation);

        Plane p = new Plane(Vector3.up, tower.position);
        Vector3 hitPoint = tower.position;
        if (p.Raycast(mouseRay, out float hitDist) && Input.GetAxis("Fire1") != 0)
        {
            hitPoint = mouseRay.GetPoint(hitDist);
            tower.transform.LookAt(hitPoint);
        }

        if (fireInput == 1)
        {
            if (!hasFired && currentFireDelay < 0)
            {
                currentFireDelay = fireDelay;
                soundSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                Instantiate(bullet, turretCannon.transform.position, turretCannon.transform.rotation); 
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    //AudioSource.PlayClipAtPoint(shoot1, transform.position);
                    soundSource.PlayOneShot(shoot1);
                }
                else
                {  
                   //AudioSource.PlayClipAtPoint(shoot2, transform.position);//soundSource.PlayOneShot(shoot2); 
                   soundSource.PlayOneShot(shoot1);
                }
                hasFired = true;
            }
        }
        else if(fireInput == 0)
            hasFired = false;
        
        currentFireDelay -= Time.deltaTime;
    }

    /*private void OnCollisionExit() //prevent sliding after collision, NOT, lol
    {
        SetBodyVelocityToZero();
    }*/

    public void SetBodyVelocityToZero()
    {
        body.velocity = Vector3.zero;
    }

    public void EnableBodyConstraints()
    {
        body.constraints = RigidbodyConstraints.FreezeAll;
    }
}
