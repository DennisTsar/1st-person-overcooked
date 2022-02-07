using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Interactable : MonoBehaviourPunCallbacks
{
    public GameObject player;
    [System.NonSerialized] public Camera cam;
    void Start()
    {
        cam = Camera.main;
    }
    public virtual void OnInteract(GameObject carry, int playerID)
    {

    }
}
