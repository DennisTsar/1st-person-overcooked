using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Carriable : MonoBehaviour, IPunInstantiateMagicCallback
{
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // this.transform.localScale = new Vector3(2, 2, 2);
        ((GameObject)info.Sender.TagObject).GetComponent<PlayerController>().SetCarry(this.gameObject);
    }
}
