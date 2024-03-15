using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCamera : MonoBehaviour
{
    public Rigidbody body;
    public Vector3 offset = new Vector3(0f, 8f, -2.5f);

    // Update is called once per frame
    void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(0f, 0f, body.position.z) + offset;
    }
}
