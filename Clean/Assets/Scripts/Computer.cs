using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Module
{
    private RaycastHit hit;
    private bool isPiloting;

    // Start is called before the first frame update
    void Start()
    {
        SetFirst(KeyCode.F);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, -transform.forward);
        if (Physics.Raycast(transform.position, -transform.forward, out hit, 3f))
        {
            if (hit.transform == player)
            {
                if (Input.GetKeyDown(GetFirst()))
                {
                    isPiloting = !isPiloting;
                }
            }
        }
        if (isPiloting)
        {
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            player.GetComponent<Rigidbody>().isKinematic = true;
            player.GetComponentInChildren<BoxCollider>().isTrigger = true;
            player.transform.position = transform.position - transform.forward - .5f * transform.up;
            player.transform.eulerAngles = transform.eulerAngles;
            player.GetComponentInChildren<Animator>().SetBool("Control", true);
        }
        else
        {
            player.transform.eulerAngles = Vector3.zero;
            player.GetComponentInChildren<Animator>().transform.eulerAngles = Vector3.zero;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
            player.GetComponent<Rigidbody>().isKinematic = false;
            player.GetComponentInChildren<BoxCollider>().isTrigger = false;
            player.GetComponentInChildren<Animator>().SetBool("Control", false);
        }
        //Debug.Log(isPiloting);

        player.GetComponent<Character>().SetInVehicle(isPiloting);
    }
}
