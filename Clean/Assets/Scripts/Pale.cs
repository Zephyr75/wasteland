using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pale : Module
{
    // Start is called before the first frame update
    void Start()
    {
        SetFirst(KeyCode.Space);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Character>().GetInVehicle())
        {
            if (transform.parent.GetComponent<Propeller>().up || transform.parent.GetComponent<Propeller>().down ||
                    transform.parent.GetComponent<Propeller>().left || transform.parent.GetComponent<Propeller>().right ||
                        transform.parent.GetComponent<Propeller>().space)
            {
                transform.Rotate(Vector3.up * 200);
            }
        }
    }
}
