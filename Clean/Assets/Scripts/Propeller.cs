using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : Module
{
    public Direction direction;
    public bool up, down, left, right, space;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if (player.GetComponent<Character>().GetInVehicle())
        {

            //Vector3 oldVel = GetComponent<Rigidbody>().angularVelocity;
            //GetComponent<Rigidbody>().angularVelocity = new Vector3(oldVel.x/1000, oldVel.y, oldVel.z/1000);

            up = Input.GetKey(GetFirst());
            down = Input.GetKey(GetSecond());
            left = Input.GetKey(GetThird());
            right = Input.GetKey(GetFourth());
            space = Input.GetKey(GetFifth());

            Debug.DrawRay(transform.position + new Vector3(0, 0, .5f), transform.right, Color.green);
            if (up)
            {
                GetComponent<Rigidbody>().AddForce(transform.forward * 100);
            }
            if (down)
            {
                GetComponent<Rigidbody>().AddForce(-transform.forward * 100);
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
            if (space)
            {
                GetComponent<Rigidbody>().AddForce(transform.up * 300);
            }
        }
    }

    private void RollForward()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 50);
    }

    private void RollBackward()
    {
        GetComponent<Rigidbody>().AddForce(-transform.forward * 50);
    }
}
