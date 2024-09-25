using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public GameObject particle, main;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Impact());
        }
    }

    IEnumerator Impact()
    {
        main.SetActive(false);
        particle.SetActive(true);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
