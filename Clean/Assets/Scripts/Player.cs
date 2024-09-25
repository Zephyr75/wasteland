using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private int moveSpeed = 15;
    private float shake;
    public Transform cam;
    private bool up, down, left, right, shift, ctrl, space, e, a;
    public LayerMask maskClimb, maskLedge;
    public GameObject batte;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    

    // Update is called once per frame
    void Update()
    {
        UpdateCharacter();
        moveSpeed = isClimbing ? 3 : (isLedging ? 2 : (isCrouching ? 5 : 15));
        if (isShaking)
        {
            ShakeCamera();
        }
        
        if (!inVehicle)
        {
            up = Input.GetKey(KeyCode.Z);
            down = Input.GetKey(KeyCode.S);
            right = Input.GetKey(KeyCode.D);
            left = Input.GetKey(KeyCode.Q);
            shift = Input.GetKey(KeyCode.LeftShift);
            ctrl = Input.GetKey(KeyCode.LeftControl);
            //shift = Input.GetKey(KeyCode.V);
            //ctrl = Input.GetKey(KeyCode.C);
            space = Input.GetKey(KeyCode.Space);
            e = Input.GetKey(KeyCode.E);
            a = Input.GetKeyDown(KeyCode.A);
        }
        
        isCrouching = ctrl && !isShooting && !isSmashing;
        isMoving = (up || down || right || left) && !isShooting && !isSmashing;

        isClimbing = Physics.Raycast(transform.position, body.forward, 1, maskClimb);
        isLedging = Physics.Raycast(transform.position, body.forward, 1, maskLedge);

        if (!isShooting && !isSmashing)
        {
            if (up)
            {
                if (isClimbing || isLedging)
                {
                    GetComponentInParent<Rigidbody>().useGravity = false;
                    transform.position += transform.up * Time.deltaTime * moveSpeed;
                    if (isClimbing)
                    {
                        body.GetComponent<Animator>().SetBool("Climb", true);
                    }
                    else
                    {
                        body.GetComponent<Animator>().SetBool("Ledge", true);
                    }
                }
                else
                {
                    GetComponentInParent<Rigidbody>().useGravity = true;
                    body.GetComponent<Animator>().SetBool("Climb", false);
                    body.GetComponent<Animator>().SetBool("Ledge", false);
                    transform.position += body.forward * Time.deltaTime * moveSpeed;
                    body.GetComponent<Animator>().SetFloat("Y", Mathf.Lerp(body.GetComponent<Animator>().GetFloat("Y"), 1, step));
                }
            }
            else
            {
                GetComponentInParent<Rigidbody>().useGravity = true;
                body.GetComponent<Animator>().SetBool("Climb", false);
                body.GetComponent<Animator>().SetBool("Ledge", false);
            }

            if (down)
            {
                transform.position -= body.forward * Time.deltaTime * moveSpeed;
                body.GetComponent<Animator>().SetFloat("Y", Mathf.Lerp(body.GetComponent<Animator>().GetFloat("Y"), -1, step));
            }
            if (right)
            {
                transform.position += body.right * Time.deltaTime * moveSpeed;
                body.GetComponent<Animator>().SetFloat("X", Mathf.Lerp(body.GetComponent<Animator>().GetFloat("X"), 1, step));
            }
            if (left)
            {
                transform.position -= body.right * Time.deltaTime * moveSpeed;
                body.GetComponent<Animator>().SetFloat("X", Mathf.Lerp(body.GetComponent<Animator>().GetFloat("X"), -1, step));
            }

            if (!right && !left)
            {
                body.GetComponent<Animator>().SetFloat("X", Mathf.Lerp(body.GetComponent<Animator>().GetFloat("X"), 0, step));
            }
            if (!up && !down)
            {
                body.GetComponent<Animator>().SetFloat("Y", Mathf.Lerp(body.GetComponent<Animator>().GetFloat("Y"), 0, step));
            }
        }
        

        if (!isClimbing && !isLedging && !isJumping && !isFalling)
        {
            if (!isSliding)
            {
                if (shift && !isShooting && !isSmashing)
                {
                    transform.GetComponent<Rigidbody>().AddForce(body.forward * 1000);
                    StartCoroutine(Slide());
                }
                if (!isSmashing && !isShooting)
                {
                    if (e && count == 0)
                    {
                        StartCoroutine(Shoot());
                        StartCoroutine(Shake(1.2f, .2f));
                    }
                    if (a)
                    {
                        StartCoroutine(Attack());
                        StartCoroutine(Shake(.6f, .5f));
                    }
                }
            }
            if (space)
            {
                StartCoroutine(Jump());
            }
        }
        
        Debug.DrawRay(transform.position + new Vector3(0, 0, 0), body.forward, Color.red);
        Debug.DrawRay(transform.position + new Vector3(0, .1f, 0), -transform.up * 3f, Color.red);
    }

    private void ShakeCamera()
    {
        if (shake > 0f)
        {
            cam.localPosition = cam.localPosition + Random.insideUnitSphere * shake;
            shake -= 2 * Time.deltaTime;
        }
        else
        {
            isShaking = false;
        }
    }

    IEnumerator Slide()
    {
        isSliding = true;
        body.GetComponent<Animator>().SetBool("Slide", true);
        yield return new WaitForSeconds(1.5f);
        body.GetComponent<Animator>().SetBool("Slide", false);
        isSliding = false;
    }

    IEnumerator Attack()
    {
        batte.GetComponent<MeshRenderer>().enabled = true;
        batte.GetComponent<BoxCollider>().enabled = true;
        body.GetComponent<Animator>().SetBool("Attack", true);
        yield return new WaitForSeconds(1.2f);
        body.GetComponent<Animator>().SetBool("Attack", false);
        batte.GetComponent<MeshRenderer>().enabled = false;
        batte.GetComponent<BoxCollider>().enabled = false;
    }

    IEnumerator Jump()
    {
        isJumping = true;
        body.GetComponent<Animator>().SetBool("Jump", true);
        yield return new WaitForSeconds(.25f);
        transform.GetComponent<Rigidbody>().AddForce(transform.up * 400);
        body.GetComponent<Animator>().SetBool("Jump", false);
        yield return new WaitForSeconds(.25f);
        isJumping = false;
    }

    IEnumerator Shake(float time, float intensity)
    {
        yield return new WaitForSeconds(time);
        isShaking = true;
        shake = intensity;
    }

}
