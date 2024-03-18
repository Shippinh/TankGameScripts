using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMineBehaviour : MonoBehaviour
{
    [SerializeField]
    public GameObject charge;

    public float destroyTime = 20f;
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(charge, destroyTime + 19f);
        Destroy(gameObject, destroyTime + 20f);
    }
}
