using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    BATTE, ARROW
}

public class Weapon : MonoBehaviour
{
    public WeaponType type;
    public GameObject impact, player;
    private int typeNbr, count;
    private GameObject newImpact;

    private void Update()
    {
        switch (type)
        {
            case WeaponType.BATTE: typeNbr = 0;
                transform.localPosition = new Vector3(-.24f, .44f, .07f);
                transform.localEulerAngles = new Vector3(-19, -111.8f, 94.4f);
                break;
            default: typeNbr = 1;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Enemy")
        {
            StartCoroutine(Impact(collision));
            Debug.Log("got rekt");
            Application.Quit();
        }
        if (collision.gameObject.tag == "Enemy" && gameObject.tag == "Player")
        {
            StartCoroutine(Impact(collision));
            collision.gameObject.GetComponentInParent<Enemy>().Die(typeNbr);
        }
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Enemy")
        {
            StartCoroutine(Impact(collision));
        }

    }

    IEnumerator Impact(Collision col)
    {
        if (count == 0)
        {
            newImpact = GameObject.Instantiate(impact, col.GetContact(0).point, Quaternion.Inverse(player.transform.rotation));
            newImpact.SetActive(true);
            count += 1;
        }
        yield return new WaitForSeconds(1);
        Destroy(newImpact);
        count = 0;
    }
}
