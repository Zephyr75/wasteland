using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    RIGHT, LEFT
}

public class Wheel : Module
{
    public Direction direction;
    private bool inContact;
    private int actual;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        inContact = actual > 0 || transform.position.y < .5f;

        if (player.GetComponent<Character>().GetInVehicle() && inContact)
        {
            ModuleUpdate();
            //Controls for driving
            bool up = Input.GetKey(GetFirst());
            bool down = Input.GetKey(GetSecond());
            bool left = Input.GetKey(GetThird());
            bool right = Input.GetKey(GetFourth());

            //Debug.Log(GetThirdInput().ToString());

            if (up)
            {
                RollForward();
            }
            if (down)
            {
                RollBackward();
            }
            if (right)
            {
                if (direction == Direction.RIGHT)
                {
                    RollBackward();
                }
                if (direction == Direction.LEFT)
                {
                    RollForward();
                }
            }
            if (left)
            {
                if (direction == Direction.LEFT)
                {
                    RollBackward();
                }
                if (direction == Direction.RIGHT)
                {
                    RollForward();
                }
            }
        }
    }

    private void RollForward()
    {
        GetComponent<Rigidbody>().AddForce(transform.parent.forward * 100);
        GetComponent<Rigidbody>().AddTorque(transform.right * 100);
    }

    private void RollBackward()
    {
        //GetComponent<Rigidbody>().AddForceAtPosition(-transform.parent.forward * 10 * speed, transform.position + new Vector3(0, .5f, 0));
        GetComponent<Rigidbody>().AddForce(-transform.parent.forward * 100);
        GetComponent<Rigidbody>().AddTorque(-transform.right * 100);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Machine" && collision.gameObject.tag != "Child")
        {
            actual += 1;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != "Machine" && collision.gameObject.tag != "Child")
        {
            actual -= 1;
        }
    }
}
