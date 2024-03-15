using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphereCatlike : MonoBehaviour
{
    [SerializeField]
    Vector3 input;

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;

    Vector3 velocity;

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10f;                     //responsiveness of the sphere

    [SerializeField]
    Rect allowedArea = new Rect(-5f, -5f, 10f, 10f); //limiting the movement

    [SerializeField, Range(0f, 1f)]
    float bounciness = 0.5f;                         //limiting bounciness when collide inside rectangle

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        /*inputX = Input.GetAxisRaw("Horizontal");                                       //��� 0 ��� 1
        inputZ = Input.GetAxisRaw("Vertical");*/

        input.x = Input.GetAxis("Horizontal");                                           //�� 0 �� 1
        input.z = Input.GetAxis("Vertical");

        //input.Normalize();                                                             //���� ���� �������� �������� ��������
        input = Vector3.ClampMagnitude(input, 1f);                                       //���� �������� �� ��������

        Vector3 displacement = new Vector3(input.x, 0f, input.z);                        //����, ��������������� ��� ��������� ����
                                                                                         //displacement = input, position != input

        //transform.localPosition = new Vector3(input.x, 0.5f, input.z);                 //������������ �������� �����, input = position

        Vector3 desiredVelocity = new Vector3(input.x, 0f, input.z) * maxSpeed;          //���� ����������� �������� ���������� � ���� Input
                                                                                         //���� velocity ����� ���������� �� ������ ���
                                                                                         //������� ����� ��� ������ ��� ���������,
                                                                                         //��� ������ ������� ������������� ������������

        float maxSpeedChange = maxAcceleration * Time.deltaTime;                         //��������� acceleration

        /*if (velocity.x < desiredVelocity.x)
        {
            velocity.x = Mathf.Min(velocity.x + maxSpeedChange, desiredVelocity.x);      //���� velocity ����� �� ������ � Input
        }
        else if (velocity.x > desiredVelocity.x)
        {
            velocity.x = Mathf.Max(velocity.x - maxSpeedChange, desiredVelocity.x);      //���� velocity ����� �� ������ � Input
        }*/

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);   //MoveTowards(����� ��������, ���� ��������, �� ������ ��������)
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        //velocity += acceleration * Time.deltaTime;                                     //acceleration = vt

        displacement = velocity * Time.deltaTime;                                        //d = vi

        //transform.localPosition += displacement;                                       //��� � ������� ����� input.
                                                                                         //��� ���������� velocity ���������� �� ������� �������� ����,
                                                                                         //������� ������� ��������� � ������ ������ Update
                                                                                         //��� ������� �� ��������� velocity �� ���

        Vector3 newPosition = transform.localPosition + displacement;                    //�������� ��� ����� ����� �� ��������
                                                                                         //������ �� ��� ����, ��� ��� �������� ����� ���� ���������
                                                                                         //�� ���� ������� ����� � ��� ��� ���������� ������
        /*if (!allowedArea.Contains(new Vector2(newPosition.x, newPosition.z)))          //�������� ��� ���� ����� �������� ��������
        {
            //newPosition = transform.localPosition;                                     //�� ������� ���
            newPosition.x = Mathf.Clamp(newPosition.x, allowedArea.xMin, allowedArea.xMax); //��������� ��� �������� ��� velocity
            newPosition.z = Mathf.Clamp(newPosition.z, allowedArea.yMin, allowedArea.yMax);
        }*/

        if (newPosition.x < allowedArea.xMin)                                            //�� ����, ��� ������ ��������� �������� ��������, ��������
        {                                                                                //��� �������� ��������� ��� + ��������� velocity
            newPosition.x = allowedArea.xMin;
            velocity.x = -velocity.x * bounciness;                                       //velocity = 0f - ��������� ����������� � �������
        }                                                                                //velocity = -velocity - ������ � ���������� �� ���糿 �������
        else if (newPosition.x > allowedArea.xMax)
        {
            newPosition.x = allowedArea.xMax;
            velocity.x = -velocity.x * bounciness;
        }
        if (newPosition.z < allowedArea.yMin)
        {
            newPosition.z = allowedArea.yMin;
            velocity.z = -velocity.z * bounciness;
        }
        else if (newPosition.z > allowedArea.yMax)
        {
            newPosition.z = allowedArea.yMax;
            velocity.z = -velocity.z * bounciness;
        }

        transform.localPosition = newPosition;
    }
}
