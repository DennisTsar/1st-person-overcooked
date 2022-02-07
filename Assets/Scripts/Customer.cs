using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Customer : Interactable
{

    public override void OnInteract(GameObject carry, int playerID)
    {
        if (carry != null && carry.GetComponent<Plate>() != null && carry.GetComponent<Plate>().done)
        {
            photonView.RPC("ReceiveDish", RpcTarget.All, carry.GetPhotonView().ViewID, playerID);
        }
    }
    [PunRPC]
    void ReceiveDish(int carryID, int playerID)
    {
        GameObject carry = PhotonView.Find(carryID).gameObject;
        GameObject player = PhotonView.Find(playerID).gameObject;
        foreach (Transform t in carry.transform)
        {
            if (t.name.StartsWith("Burger"))
                GameObject.Find("Canvas").transform.Find("Orders").GetComponent<OrdersController>().DeleteOrder("Burger Recipe");
            if (t.name.StartsWith("Sushi"))
                GameObject.Find("Canvas").transform.Find("Orders").GetComponent<OrdersController>().DeleteOrder("Sushi Recipe");
        }
        Destroy(carry);
        player.GetComponent<PlayerController>().SetCarry(null);
        transform.Find("Customer").GetComponent<Animator>().SetTrigger("Pickup");
    }
}
