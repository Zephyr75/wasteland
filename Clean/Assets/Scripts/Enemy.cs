using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public NavMeshAgent agent;
    public Transform enemy;
    private bool counting, inView, inRange, tempFollow, follow;
    private float timer = -1;
    private RaycastHit hit;
    public GameObject bush;
    public Transform playerHead, enemyHead;
    public LayerMask maskAvoid, maskRange;
    public int range, detection;

    // Start is called before the first frame update
    void Start()
    {
        Physics.Raycast(transform.position, -transform.up, out hit, 10);
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(transform.position + new Vector3(0, 3.5f, 0), (playerHead.position - enemyHead.position) * 1.1f, out hit, detection, maskAvoid);
        inRange = Physics.Raycast(transform.position + new Vector3(0, 3.5f, 0), (playerHead.position - enemyHead.position) * 1.1f, detection, maskRange);
        
        Debug.DrawRay(transform.position + new Vector3(0, 3.5f, 0), (playerHead.position - enemyHead.position) * 1.1f);

        if (hit.collider != null)
        {
            inView = hit.collider.gameObject.name == "SkeletonPlayer" && inRange;
        }

        bush.SetActive(!follow);

        if (!counting)
        {
            if (!follow && inView)
            {
                timer = 2;
                counting = true;
                tempFollow = true;
            }
            if (follow && !inView)
            {
                timer = 8;
                counting = true;
                tempFollow = false;
            }
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (!follow && !inView)
            {
                tempFollow = false;
            }
            if (follow && inView)
            {
                timer = -1;
                counting = false;
            }
        }
        if (timer <= 0 && timer > -1)
        {
            follow = tempFollow;
            counting = false;
            timer = -1;
        }

        if (!isShooting && follow)
        {
            if (Vector3.Distance(transform.position, player.position) < range)
            {
                agent.speed = 0;
                enemy.GetComponent<Animator>().SetFloat("Y", 0);
                StartCoroutine(Shoot());
            }
            else
            {
                agent.speed = 10;
                enemy.GetComponent<Animator>().SetFloat("Y", 1);
            }
        }

        if (!follow)
        {
            enemy.GetComponent<Animator>().SetFloat("Y", 0);
            agent.speed = 0;
        }
        
        agent.SetDestination(player.position);

    }

    public void Die(int type)
    {
        switch (type)
        {
            case 0: StartCoroutine(Stun()); break;
            default: StartCoroutine(Shot()); break;
        }
    }

    IEnumerator Shot()
    {
        enemy.GetComponent<Animator>().enabled = false;
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            if (col.transform.tag != "Enemy")
            {
                col.enabled = !col.enabled;
            }
        }
        /*foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.enabled = !rb.enabled;
        }*/
        yield return new WaitForSeconds(5f);
        Destroy(enemy.parent.gameObject);
    }

    IEnumerator Stun()
    {
        enemy.GetComponent<Animator>().SetBool("Stun", true);
        yield return new WaitForSeconds(1.8f);
        Destroy(enemy.parent.gameObject);
    }
}
