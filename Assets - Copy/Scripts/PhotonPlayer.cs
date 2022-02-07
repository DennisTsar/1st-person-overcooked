using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayer : MonoBehaviourPunCallbacks
{
    private float xRot = 0.0f;
    private float yRot = 0.0f;
    public Camera cam;
    private RaycastHit hit;
    private Animator animator;
    private bool carry = false;
    private string thing;

    void Start()
    {
        if (photonView.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
            // cam = Camera.main;
            cam.gameObject.SetActive(true);
            animator = GetComponent<Animator>();
        }  else
        {
            cam.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Movement();
        }
    }

    void Movement()
    {
        float mouseX = Input.GetAxis("Mouse X") * 1.0f;
        float mouseY = Input.GetAxis("Mouse Y") * 1.0f;

        xRot += mouseX;
        yRot += mouseY;
        // yRot = Mathf.Clamp(yRot, -90, 90);
        cam.transform.eulerAngles = new Vector3(-yRot, xRot, 0.0f);
        transform.eulerAngles = new Vector3(0, xRot, 0.0f);

        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("walkb", true);
            animator.SetBool("walk", false);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("walk", true);
            animator.SetBool("walkb", false);
        }
        else
        {
            animator.SetBool("walk", false);
            animator.SetBool("walkb", false);
        }

        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * Time.deltaTime * 4);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * Time.deltaTime * 4);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * Time.deltaTime * 4);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * Time.deltaTime * 4);
        //Debug.DrawRay(transform.position + new Vector3(0, 0.3f, 0), transform.forward*3f, Color.red);
        /*if (Physics.Raycast(transform.position - new Vector3(0, 0.5f, 0), transform.forward*5f, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            Debug.Log("Hit!");
        }*/
        if (Input.GetMouseButtonDown(0))
        {
            //animator.SetTrigger("pickup");
            //Debug.DrawRay(transform.position - new Vector3(0, 1.1f, 0), transform.forward * 3f, Color.red);
            if (Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.forward * 3f, out hit, 3f))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.name.Equals("Cylinder"))
                {
                    carry = true;
                    thing = "ham";
                }
                //Debug.Log("Hit!");
            }
        }
    }
}
