using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : Module
{
    public GameObject bullet;
    private bool canShoot = true;
    private float verticalRotation;

    // Start is called before the first frame update
    void Start()
    {
        SetFirst(KeyCode.LeftShift);
        SetSecond(KeyCode.LeftControl);
        SetThird(KeyCode.C);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Character>().GetInVehicle())
        {
            if (Input.GetKeyDown(GetThird()) && canShoot)
            {
                StartCoroutine(Shoot());
            }

            bool up = Input.GetKey(GetFirst());
            bool down = Input.GetKey(GetSecond());

            if (up)
            {
                verticalRotation -= 1;
            }
            if (down)
            {
                verticalRotation += 1;
            }
            transform.localEulerAngles = Vector3.left * verticalRotation;
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;
        GameObject newBullet = GameObject.Instantiate(bullet, transform.position, transform.rotation);
        newBullet.transform.tag = "Player";
        Physics.IgnoreCollision(newBullet.GetComponent<Collider>(), GetComponent<Collider>(), true);
        newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 2500);
        transform.parent.GetComponent<Rigidbody>().AddForce(-transform.forward * 500);
        /*if (transform.parent.GetComponent<Rigidbody>() != null)
        {
            transform.parent.GetComponent<Rigidbody>().AddForce(-transform.forward * 2500);
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(-transform.forward * 2500);
        }*/
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }
}
