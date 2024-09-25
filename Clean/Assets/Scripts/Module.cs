using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    public bool isSpring, isColliding;
    public GameObject machine;
    public Transform player;

    
    private KeyCode firstInput = KeyCode.Z, secondInput = KeyCode.S, thirdInput = KeyCode.Q, fourthInput = KeyCode.D, fifthInput = KeyCode.Space;

    public void ModuleUpdate()
    {
        if (transform.position.y < 0f)
        {
            GetComponent<Rigidbody>().AddForce(transform.up * 200 * - transform.position.y / 5);
            Vector3 oldVel = GetComponent<Rigidbody>().velocity;
            GetComponent<Rigidbody>().velocity = new Vector3(oldVel.x, oldVel.y / (1 + ((5 + transform.position.y) / 25)), oldVel.z);
        }
    }

    private void Update()
    {
        ModuleUpdate();
    }

    public void Link(GameObject otherObject)
    {
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = otherObject.GetComponent<Rigidbody>();
        joint.breakForce = 3000;
    }

    public void SetFirst(KeyCode input)
    {
        firstInput = input;
    }

    public void SetSecond(KeyCode input)
    {
        secondInput = input;
    }

    public void SetThird(KeyCode input)
    {
        thirdInput = input;
    }

    public void SetFourth(KeyCode input)
    {
        fourthInput = input;
    }

    public void SetFifth(KeyCode input)
    {
        fifthInput = input;
    }

    public KeyCode GetFirst()
    {
        return firstInput;
    }

    public KeyCode GetSecond()
    {
        return secondInput;
    }

    public KeyCode GetThird()
    {
        return thirdInput;
    }

    public KeyCode GetFourth()
    {
        return fourthInput;
    }

    public KeyCode GetFifth()
    {
        return fifthInput;
    }

    private void OnTriggerEnter(Collider other)
    {
        isColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isColliding = false;
    }
}
