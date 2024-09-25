using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{

    private float mouseSensiX = 1, mouseSensi = 1, vertRotation, horizRotation, step, distance;
    public Transform focus, aim, player, cam;
    private bool inFront;
    private RaycastHit hitFront;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        Debug.DrawRay(focus.position, - cam.forward * 13, Color.red);
        inFront = Physics.Raycast(focus.position, - cam.forward, out hitFront, 13) ? hitFront.transform.name != "Boy" && hitFront.distance < distance : false;

        //Debug.Log(hitFront.distance + " " + distance);

        if (inFront && distance > 0)
        {
            distance -= hitFront.distance == 0 || (Mathf.Abs(distance - hitFront.distance) > 1 && distance > 1) ? Time.deltaTime * 10 : Time.deltaTime * 10 * Mathf.Abs(distance - hitFront.distance);
        }

        step += Time.deltaTime;
        if (step >= 1)
        {
            step = 0;
        }
        horizRotation += Input.GetAxis("Mouse X") * mouseSensi;
        vertRotation += Input.GetAxis("Mouse Y") * mouseSensi;
        if (transform.GetComponent<Character>().isShooting)
        {
            vertRotation = Mathf.Clamp(vertRotation, -60, 30);
        }
        else
        {
            vertRotation = Mathf.Clamp(vertRotation, -60, 15);
        }
        focus.localEulerAngles = Vector3.left * vertRotation + Vector3.up * horizRotation;
        if ((transform.GetComponent<Character>().isMoving || transform.GetComponent<Character>().isShooting) && !transform.GetComponent<Character>().GetInVehicle())
        {
            player.localEulerAngles = new Vector3(0, focus.localEulerAngles.y, 0);
        }
        cam.localEulerAngles = focus.localEulerAngles;
        if (!transform.GetComponent<Character>().isShaking)
        {
            if (transform.GetComponent<Character>().isShooting)
            {
                distance = Mathf.Lerp(distance, 5, step);
                aim.gameObject.SetActive(true);
                cam.position = aim.position - aim.forward * distance;
            }
            else
            {
                if (!inFront && distance < 10)
                {
                    distance += hitFront.distance == 0 || (Mathf.Abs(distance - hitFront.distance) > 1 && distance > 1) ? Time.deltaTime * 10 : Time.deltaTime * 10 * Mathf.Abs(distance - hitFront.distance);
                }
                aim.gameObject.SetActive(false);
                cam.position = focus.position - focus.forward * distance;
            }
        }
        if (cam.position.y < 1)
        {
            cam.position = new Vector3(cam.position.x, 1, cam.position.z);
        }
    }
}
