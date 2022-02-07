using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TimedTask : Interactable
{
    public GameObject timerPrefab;
    public GameObject arrowPrefab;
    public FoodType type;
    private bool active;
    private GameObject item;
    private GameObject arrowInstance;


    public override void OnInteract(GameObject carry, int playerID)
    {
        if (item == null && carry != null && carry.GetComponent<Food>().type == type)
        {
            GameObject timer = PhotonNetwork.Instantiate(timerPrefab.name, transform.position + Vector3.up * .25f, transform.rotation);
            timer.GetComponent<ProgressBar>().parentName = gameObject.name;
            photonView.RPC("PlaceItem", RpcTarget.All, carry.GetPhotonView().ViewID, playerID);
        }
        else if (!active && item != null && carry == null)
        {
            photonView.RPC("PickupItem", RpcTarget.All, playerID);
        }
    }

    [PunRPC]
    void PlaceItem(int carryID, int playerID)
    {
        GameObject carry = PhotonView.Find(carryID).gameObject;
        GameObject player = PhotonView.Find(playerID).gameObject;
        player.GetComponent<PlayerController>().SetCarry(null);
        item = carry;
        item.transform.position = transform.position + transform.up * 0.25f;
        item.transform.parent = transform;
        active = true;
    }

    [PunRPC]
    void PickupItem(int playerID)
    {
        GameObject player = PhotonView.Find(playerID).gameObject;
        player.GetComponent<PlayerController>().SetCarry(item);
        PhotonNetwork.Destroy(arrowInstance);
        item = null;
    }
    [PunRPC]
    public void OnEnd()
    {
        GameObject dying = item;
        item = PhotonNetwork.Instantiate(dying.GetComponent<Food>().afterPrepared.name, transform.position + Vector3.up * .25f, transform.rotation);
        PhotonNetwork.Destroy(dying);
        arrowInstance = PhotonNetwork.Instantiate(arrowPrefab.name, transform.position + Vector3.up * .25f, transform.rotation);
        active = false;
    }
}