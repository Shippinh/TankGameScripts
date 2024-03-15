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
        
        /*inputX = Input.GetAxisRaw("Horizontal");                                       //або 0 або 1
        inputZ = Input.GetAxisRaw("Vertical");*/

        input.x = Input.GetAxis("Horizontal");                                           //від 0 до 1
        input.z = Input.GetAxis("Vertical");

        //input.Normalize();                                                             //якщо ЛИШЕ необхідні абсолютні значення
        input = Vector3.ClampMagnitude(input, 1f);                                       //якщо необхідні усі значення

        Vector3 displacement = new Vector3(input.x, 0f, input.z);                        //зсув, використовується для симуляції руху
                                                                                         //displacement = input, position != input

        //transform.localPosition = new Vector3(input.x, 0.5f, input.z);                 //телепортація відповідно вводу, input = position

        Vector3 desiredVelocity = new Vector3(input.x, 0f, input.z) * maxSpeed;          //зміна максимальної швидкості відбувається у вводі Input
                                                                                         //зміна velocity також відбувається за певний час
                                                                                         //оскільки таких рух нагадує рух автомобілю,
                                                                                         //слід ввести поняття максимального пришвидшення

        float maxSpeedChange = maxAcceleration * Time.deltaTime;                         //визначаємо acceleration

        /*if (velocity.x < desiredVelocity.x)
        {
            velocity.x = Mathf.Min(velocity.x + maxSpeedChange, desiredVelocity.x);      //якщо velocity менше ніж задано в Input
        }
        else if (velocity.x > desiredVelocity.x)
        {
            velocity.x = Mathf.Max(velocity.x - maxSpeedChange, desiredVelocity.x);      //якщо velocity більше ніж задано в Input
        }*/

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);   //MoveTowards(звідки рухаємось, куди рухаємось, як швидко рухаємось)
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        //velocity += acceleration * Time.deltaTime;                                     //acceleration = vt

        displacement = velocity * Time.deltaTime;                                        //d = vi

        //transform.localPosition += displacement;                                       //рух у сторону вводу input.
                                                                                         //Без врухування velocity призводить до занадто швидкого руху,
                                                                                         //оскільки позиція змінюється з кожним кадром Update
                                                                                         //Щоб вирішити це вводиться velocity за час

        Vector3 newPosition = transform.localPosition + displacement;                    //Зберігаємо рух сфери перед її зміщенням
                                                                                         //робимо це для того, щоб при обмеженні можна було визначити
                                                                                         //чи буде входити сфера у куб при наступному зміщенні
        /*if (!allowedArea.Contains(new Vector2(newPosition.x, newPosition.z)))          //обмежуємо рух коли сфера усередині квадрату
        {
            //newPosition = transform.localPosition;                                     //дає поганий рух
            newPosition.x = Mathf.Clamp(newPosition.x, allowedArea.xMin, allowedArea.xMax); //враховуємо межі квадрату без velocity
            newPosition.z = Mathf.Clamp(newPosition.z, allowedArea.yMin, allowedArea.yMax);
        }*/

        if (newPosition.x < allowedArea.xMin)                                            //те саме, але замість обмеження усередині квадрату, обмежуємо
        {                                                                                //рух відповідно координат меж + враховуємо velocity
            newPosition.x = allowedArea.xMin;
            velocity.x = -velocity.x * bounciness;                                       //velocity = 0f - занулюємо прискорення у сторону
        }                                                                                //velocity = -velocity - відскок у протилежну до колізії сторону
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
