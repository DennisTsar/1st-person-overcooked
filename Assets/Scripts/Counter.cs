using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Counter : Interactable
{
    public GameObject ob;

    public override void OnInteract(GameObject carry, int playerID)
    {
        if (ob != null && carry == null)
        {
            photonView.RPC("PickupItem", RpcTarget.All, playerID);
        }
        else if (ob == null && carry != null)
        {
            photonView.RPC("PlaceItem", RpcTarget.All, carry.GetPhotonView().ViewID, playerID);
        }
        else if (ob != null && ob.name.StartsWith("Plate3") && carry != null)
        {
            photonView.RPC("PlaceOnPlate", RpcTarget.All, carry.GetPhotonView().ViewID, playerID);
        }
    }

    [PunRPC]
    void PickupItem(int playerID)
    {
        GameObject player = PhotonView.Find(playerID).gameObject;
        player.GetComponent<PlayerController>().SetCarry(ob);
        ob = null;
    }

    [PunRPC]
    void PlaceItem(int carryID, int playerID)
    {
        GameObject player = PhotonView.Find(playerID).gameObject;
        GameObject carry = PhotonView.Find(carryID).gameObject;
        player.GetComponent<PlayerController>().SetCarry(null);
        ob = carry;
        ob.transform.position = transform.position + Vector3.up * 2.15f;
        ob.transform.rotation = Quaternion.identity;
        if (ob.name.StartsWith("Toast"))
            ob.transform.rotation = Quaternion.Euler(90, 0, 0);
        ob.transform.SetParent(transform);
    }

    [PunRPC]
    void PlaceOnPlate(int carryID, int playerID)
    {
        GameObject player = PhotonView.Find(playerID).gameObject;
        GameObject carry = PhotonView.Find(carryID).gameObject;
        if (ob.GetComponent<Plate>().Add(carry))
        {
            //carry.transform.position = transform.position + Vector3.up * 2.15f;
            //ob.transform.rotation = Quaternion.identity;
            //carry.transform.parent = ob.transform;
            player.GetComponent<PlayerController>().SetCarry(null);
        }
    }
}
