using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemDropper : Interactable
{
    public GameObject item;
    public Vector3 scale;
    public float rotation;
    string itemName;

    public void Start()
    {
        itemName = item.name;
        item.transform.localScale = scale;
    }

    public override void OnInteract(GameObject carry, int playerID)
    {
        if (carry == null)
        {
            PhotonNetwork.Instantiate(itemName, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.Euler(0, 0, rotation) * Camera.main.transform.rotation);
        }
    }
 }
