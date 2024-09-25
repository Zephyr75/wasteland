using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject explosion, player;

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Explode(collision));
    }

    IEnumerator Explode(Collision col)
    {
        if (col.gameObject == player)
        {
            Application.Quit();
        }
        else
        {
            col.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 10000, 0));
        }
        explosion.SetActive(true);
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(2f);
        explosion.SetActive(false);
        GetComponent<SphereCollider>().enabled = false;
    }
}
