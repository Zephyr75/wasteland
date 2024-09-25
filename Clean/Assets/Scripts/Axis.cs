using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Axis : Module
{
    // Start is called before the first frame update
    void Start()
    {
        SetFirst(KeyCode.A);
        SetSecond(KeyCode.E);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Character>().GetInVehicle())
        {
            ModuleUpdate();
            //Controls for driving
            bool left = Input.GetKey(GetFirst());
            bool right = Input.GetKey(GetSecond());

            //Debug.Log(GetThirdInput().ToString());
            
            if (right)
            {
                RollForward();
            }
            if (left)
            {
                RollBackward();
            }
        }
    }

    private void RollForward()
    {
        GetComponent<Rigidbody>().AddTorque(transform.up * 50);
    }

    private void RollBackward()
    {
        GetComponent<Rigidbody>().AddTorque(-transform.up * 50);
    }
}
