using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool isShooting, isSmashing, inVehicle, isMoving, isJumping, isFalling, isClimbing, isLedging, isCrouching, isSliding, isShaking;
    public GameObject bow, arrow, aim;
    public Transform body, player;
    protected int count;
    public bool isPlayer;
    protected float step;

    protected void UpdateCharacter()
    {
        step += Time.deltaTime;
        if (step >= 1)
        {
            step = 0;
        }
        Swim();
        Crouch();
        Fall();
        if (Mathf.Abs(GetComponent<Rigidbody>().angularVelocity.y) > .5f)
        {
            Vector3 oldVel = GetComponent<Rigidbody>().angularVelocity;
            GetComponent<Rigidbody>().angularVelocity = new Vector3(oldVel.x, oldVel.y / 2, oldVel.z);
        }
    }

    protected IEnumerator Shoot()
    {
        isShooting = true;
        bow.SetActive(true);
        arrow.SetActive(true);
        body.GetComponent<Animator>().SetBool("Shoot", true);
        yield return new WaitForSeconds(1.2f);
        if (count == 0)
        {
            GameObject shotArrow;
            if (isPlayer)
            {
                shotArrow = GameObject.Instantiate(arrow, aim.transform.position, aim.transform.rotation);
                shotArrow.transform.localScale = arrow.transform.localScale * 9 / 25;
            }
            else
            {
                shotArrow = GameObject.Instantiate(arrow, arrow.transform.position, arrow.transform.rotation);
                shotArrow.transform.localScale = arrow.transform.localScale * 9 / 25;
            }
            shotArrow.AddComponent<Rigidbody>();
            shotArrow.GetComponent<Rigidbody>().mass = .1f;
            shotArrow.AddComponent<Weapon>();
            if (isPlayer)
            {
                shotArrow.transform.tag = "Player";
                shotArrow.GetComponent<Weapon>().type = WeaponType.ARROW;
                shotArrow.GetComponent<Rigidbody>().AddForce(aim.transform.forward * 1000);
            }
            else
            {
                shotArrow.transform.tag = "Enemy";
                shotArrow.GetComponent<Weapon>().type = WeaponType.ARROW;
                shotArrow.GetComponent<Rigidbody>().AddForce((player.position - body.position) * 100);
            }
            shotArrow.AddComponent<BoxCollider>();
            count++;
        }
        arrow.SetActive(false);
        isShooting = false;
        yield return new WaitForSeconds(.3f);
        count = 0;
        bow.SetActive(false);
        body.GetComponent<Animator>().SetBool("Shoot", false);
    }
    
    public void SetInVehicle(bool value)
    {
        inVehicle = value;
    }

    public bool GetInVehicle()
    {
        return inVehicle;
    }

    protected void Swim()
    {
        if (transform.position.y < -1f)
        {
            GetComponent<Rigidbody>().AddForce(transform.up * 120 * (-1f - transform.position.y) / 4);
            Vector3 oldVel = GetComponent<Rigidbody>().velocity;
            GetComponent<Rigidbody>().velocity = new Vector3(oldVel.x, oldVel.y / (1 + ((5f + transform.position.y) / 20)), oldVel.z);
        }
        if (transform.position.y < -1f)
        {
            body.GetComponent<Animator>().SetBool("Swim", true);
        }
        else
        {
            body.GetComponent<Animator>().SetBool("Swim", false);
        }
    }

    protected void Crouch()
    {
        if (isCrouching)
        {
            //body.GetComponent<Animator>().SetFloat("Crouch", 1);
            body.GetComponent<Animator>().SetFloat("Crouch", Mathf.Lerp(body.GetComponent<Animator>().GetFloat("Crouch"), 1, step));
            body.GetComponentInChildren<BoxCollider>().center = new Vector3(0, 4, 0);
            body.GetComponentInChildren<BoxCollider>().size = new Vector3(1.7f, 8, 1.2f);
        }
        else
        {
            //body.GetComponent<Animator>().SetFloat("Crouch", 0);
            body.GetComponent<Animator>().SetFloat("Crouch", Mathf.Lerp(body.GetComponent<Animator>().GetFloat("Crouch"), 0, step));
            body.GetComponentInChildren<BoxCollider>().center = new Vector3(0, 5.3f, 0);
            body.GetComponentInChildren<BoxCollider>().size = new Vector3(1.7f, 10.6f, 1.2f);
        }
    }

    protected void Fall()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, .1f, 0), -transform.up, .4f))
        {
            if (!isJumping)
            {
                isFalling = false;
                body.GetComponent<Animator>().SetBool("Fall", false);
            }
        }
        else if (!isClimbing && !isLedging)
        {
            isFalling = true;
            body.GetComponent<Animator>().SetBool("Fall", true);
        }
    }
}
