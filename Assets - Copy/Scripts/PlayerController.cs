using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    private float xRot = 0.0f;
    private float yRot = 0.0f;
    public Camera cam;
    private RaycastHit hit;
    private Animator animator;
    private GameObject carry;
    //public GameObject toast;
    private bool gameOn = true;

    void Start()
    {
        if (photonView.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
            cam.gameObject.SetActive(true);
            animator = GetComponent<Animator>();
        }  else
        {
            cam.enabled = false;
            cam.transform.GetComponent<AudioListener>().enabled = false;
        }
    }
    void Update()
    {
        if (gameOn&&photonView.IsMine)
        {
            if (!GameObject.Find("Canvas").transform.Find("Time").GetComponent<Timer>().gameOn)
            {
                gameOn = false;
                if (photonView.IsMine && Cursor.lockState.Equals(CursorLockMode.Locked))
                    Cursor.lockState = CursorLockMode.None;
            }
            Movement();
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            FixedMovement();
        }
    }

    void FixedMovement()
    {
        float vert = Input.GetAxis("Vertical");
        float horiz = Input.GetAxis("Horizontal");

        animator.SetBool("walk", vert > 0 || (vert == 0 && horiz != 0));
        animator.SetBool("walkb", vert < 0);

        if (!(vert == 0 && horiz == 0))
            GetComponent<Rigidbody>().velocity = (transform.forward * vert + transform.right * horiz) * 4;
        //GetComponent<Rigidbody>().angularVelocity = new Vector3(0, xRot, 0);
    }

    void Movement()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        xRot += mouseX;
        yRot += mouseY;
        yRot = Mathf.Clamp(yRot, -90, 90);
        /*if (Input.GetKey(KeyCode.LeftBracket))
        {
            mouseX = -1;
        }
        else if (Input.GetKey(KeyCode.RightBracket))
        {
            mouseX = 1;
        }
        else
            mouseX = 0;
        if (Input.GetKey(KeyCode.Equals))
        {
            mouseY = 1;
        }
        else if (Input.GetKey(KeyCode.Minus))
        {
            mouseY = -1;
        }
        else
        {
            mouseY = 0;
        }*/
        cam.transform.eulerAngles += new Vector3(-mouseY, 0, 0.0f);
        transform.eulerAngles += new Vector3(0, mouseX, 0.0f);

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            GameObject.Find("Canvas").transform.Find("Orders").GetComponent<OrdersController>().DeleteOrder("BurgerRecipe");
        }
        if (Input.GetMouseButtonDown(0))
        {
            //animator.SetTrigger("pickup");
            //Debug.DrawRay(transform.position - new Vector3(0, 1.1f, 0), transform.forward * 3f, Color.red);
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3f))
            {
                GameObject g = hit.collider.gameObject;
                Interactable i = hit.transform.GetComponent<Interactable>();
                if (i != null)
                    i.OnInteract(carry, photonView.ViewID);
                Debug.Log("Name: " + g.name);
            }
        }
    }

    public void SetCarry(GameObject item)
    {
        carry = item;
        if (carry != null)
        {
            carry.transform.SetParent(cam.transform);
            carry.transform.localPosition = new Vector3(0, 0, 0.8f);
            //carry.transform.localPosition = new Vector3(0.15f, 0.7f, 0.6f);
        }
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject;
    }
}
