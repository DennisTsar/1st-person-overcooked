using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PhotonGameManager : MonoBehaviour, IOnEventCallback
{
    public GameObject playerPrefab;
    Vector3[] spawnPoints;

    const byte SpawnPlayerEventCode = 1;

    public void Start()
    {
        spawnPoints = new Vector3[]{
            new Vector3(0, 5, -1),
            new Vector3(3, 5, -1),
            new Vector3(6, 5, -1),
            new Vector3(9, 5, -1)
        };
        AddPlayer();
    }

    public void AddPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player p = PhotonNetwork.PlayerList[i];
                Vector3 spawnLoc = spawnPoints[i];
                object[] data = new object[] { spawnLoc, Quaternion.identity };
                RaiseEventOptions options = new RaiseEventOptions() { TargetActors = new int[] { p.ActorNumber } };
                PhotonNetwork.RaiseEvent(SpawnPlayerEventCode, data, options, SendOptions.SendReliable);
            }
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == SpawnPlayerEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;

            Vector3 spawnLoc = (Vector3)data[0];
            Quaternion spawnRot = (Quaternion)data[1];

            Debug.Log("SPAWN: " + spawnLoc);
            PhotonNetwork.Instantiate(this.playerPrefab.name, spawnLoc, spawnRot, 0);
        }
    }
}
