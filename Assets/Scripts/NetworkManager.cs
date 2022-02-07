using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    [SerializeField] Button join;
    [SerializeField] GameObject joinMenu;
    [SerializeField] GameObject roomMenu;
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text players;
    [SerializeField] GameObject roomListingPrefab;
    [SerializeField] Transform roomListingContent;
    [SerializeField] Button start;

    void Awake()
    {
        if (instance != null && instance != this)
            gameObject.SetActive(false);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("START");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master " + join);
        FindJoinComponents();
        join.interactable = true;
        PhotonNetwork.JoinLobby();
    }
    public void FindJoinComponents()
    {
        join = GameObject.Find("Button - Create").GetComponent<Button>();
        joinMenu = GameObject.Find("Join/Create");
        roomListingContent = GameObject.Find("Content").transform;
    }
    public void CreateRoom(string room)
    {
        PhotonNetwork.CreateRoom(room);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.CurrentRoom.IsVisible = true;
    }
    public void JoinRoom(string room)
    {
        PhotonNetwork.JoinRoom(room);
    }

    public void ChangeScene(string scene)
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void TryToJoin(string room, string username)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.LocalPlayer.NickName = username;
            PhotonNetwork.JoinOrCreateRoom(room, null, null, null);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name + ", Size: " + PhotonNetwork.CurrentRoom.PlayerCount);
        FindJoinComponents();
        if(joinMenu!=null)
            joinMenu.SetActive(false);
        FindRoomComponents();

        UpdatePlayerList();

        roomName.text = PhotonNetwork.CurrentRoom.Name;

        if (PhotonNetwork.IsMasterClient)
            start.interactable = true;
    }
    void FindRoomComponents()
    {
        roomMenu = GameObject.Find("Room");
        foreach (Transform child in roomMenu.transform)
        {
            child.gameObject.SetActive(true);
        }
        roomName = GameObject.Find("Text - Room Name").GetComponent<TMP_Text>();
        players = GameObject.Find("Text - Players").GetComponent<TMP_Text>();
        start = GameObject.Find("Button - Start").GetComponent<Button>();
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        players.text += newPlayer.NickName + "\n";
    }
    public override void OnRoomListUpdate(List<Photon.Realtime.RoomInfo> roomList)
    {
        Debug.Log(roomList.Count);
        foreach (Transform t in roomListingContent.GetComponentInChildren<Transform>())
            GameObject.Destroy(t.gameObject);
        foreach (Photon.Realtime.RoomInfo room in roomList)
        {
            if (room.PlayerCount > 0)
            {
                Instantiate(roomListingPrefab, roomListingContent).GetComponent<RoomListingController>().Setup(room.Name, room.PlayerCount);
            }
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdatePlayerList();
        if (PhotonNetwork.IsMasterClient)
            start.interactable = true;
    }

    public void UpdatePlayerList()
    {
        string s = "Player:\n";
        foreach (var player in PhotonNetwork.PlayerList)
            s += player.NickName + "\n";
        players.text = s;
    }

}
