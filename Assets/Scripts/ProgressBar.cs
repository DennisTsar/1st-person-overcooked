using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEditor;

public class ProgressBar : MonoBehaviourPunCallbacks
{
    private Camera cam;
    public float total;
    //public int parentId;
    public string parentName;
    public float t;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {

        transform.LookAt(cam.transform);
        t += Time.deltaTime;
        //Debug.Log(transform.Find("ProgressCanvas").Find("Bar").GetComponent<Image>());
        transform.Find("ProgressCanvas").Find("Bar").gameObject.GetComponent<Image>().fillAmount = t / total;
        if (t > total)
        {
            if (GameObject.Find(parentName) != null && GameObject.Find(parentName).GetComponent<TimedTask>() != null)
            {
                GameObject item = GameObject.Find(parentName).GetComponent<TimedTask>().item;
                GameObject i = PhotonNetwork.Instantiate(item.GetComponent<Food>().afterPrepared.name, transform.position + Vector3.up * .25f, transform.rotation);
                GameObject.Find(parentName).GetComponent<TimedTask>().photonView.RPC("OnEnd", RpcTarget.All, i.GetPhotonView().ViewID);
            }
            photonView.RPC("Destroy", RpcTarget.All);
        }
    }
    [PunRPC]
    void Destroy()
    {
        Destroy(gameObject);
    }
}