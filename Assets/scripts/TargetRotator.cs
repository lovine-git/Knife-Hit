using System.Collections;
using UnityEngine;

public class TargetRotator : MonoBehaviour
{
    [System.Serializable]
    public class RotationElement
    {
        public float rotationSpeed;
        public float rotationDuration;
    }

    [SerializeField]
    private RotationElement[] rotationPattern;

    private WheelJoint2D wheelJoint;
    private JointMotor2D motor;
    private int rotationIndex = 0; 
    private float currentSpeed;

    private void Awake()
    {
        wheelJoint = GetComponent<WheelJoint2D>();
        motor = new JointMotor2D();
        StartCoroutine("PlayRotationPattern");
    }



   
    void Update()
    {
       
        transform.Rotate(0, 0, currentSpeed * Time.deltaTime);
    }




    public void NextLevel()
    {
       
        GameObject[] allSpears = GameObject.FindGameObjectsWithTag("Spear");
        foreach (GameObject spear in allSpears)
        {
            Destroy(spear);
        }
        GameObject[] allApples = GameObject.FindGameObjectsWithTag("Apple");
        foreach (GameObject apple in allSpears)
        {
            Destroy(apple);
        }

        
        rotationIndex += 2;
        if (rotationIndex >= rotationPattern.Length) rotationIndex = 0;


        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }




        StopCoroutine("PlayRotationPattern");
        StartCoroutine("PlayRotationPattern");

        Debug.Log("Log cleared of " + allSpears.Length + " spears!");
    }




    private IEnumerator PlayRotationPattern()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

          
            if (GameController.Instance != null && !GameController.Instance.IsGameActive)
            {
                
                motor.motorSpeed = 0;
                wheelJoint.motor = motor;

               
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.angularVelocity = 0;
                    
                    rb.bodyType = RigidbodyType2D.Static;
                }

                yield break; 
            }

        
    

  
    SetMotor(rotationPattern[rotationIndex + 1].rotationSpeed);
            yield return new WaitForSecondsRealtime(rotationPattern[rotationIndex + 1].rotationDuration);
        }
    }

    private void SetMotor(float speed)
    {
        motor.motorSpeed = speed;
        motor.maxMotorTorque = 10000;
        wheelJoint.motor = motor;
    }
}