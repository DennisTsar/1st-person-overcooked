using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Game countdown timer

public class Timer : MonoBehaviourPunCallbacks
{

    public float time;
    public bool gameOn;


    void Update()
    {
        if (gameOn)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (time > 0)
                {
                    time -= Time.deltaTime;
                    float m = Mathf.FloorToInt(time / 60);
                    float s = Mathf.FloorToInt(time % 60);
                    photonView.RPC("UpdateTime", RpcTarget.All, m, s);
                }
                else
                {
                    gameOn = false;
                    time = 0;
                    photonView.RPC("GameOver", RpcTarget.All);
                }
            }
        }
    }

    public void StartOver()
    {
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    void UpdateTime(float m, float s)
    {
        GetComponent<Text>().text = string.Format("{0:00}:{1:00}", m, s);
    }

    [PunRPC]
    void GameOver()
    {
        GetComponent<Text>().text = "00:00";
        GameObject.Find("Canvas").transform.Find("GameOver").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("GameOver").Find("GameOverText").GetComponent<Text>().text = "GAME OVER\nScore: " + GameObject.Find("Canvas").transform.Find("Orders").GetComponent<OrdersController>().score;
    }
}
