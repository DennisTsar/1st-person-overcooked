using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField room;
    public TMP_InputField username;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void JoinOrCreate()
    {
        if (room.text != "" && username.text != "")
            NetworkManager.instance.TryToJoin(room.text,username.text);
    }
    public void LeaveRoom()
    {

    }

    public void StartGame()
    {
        NetworkManager.instance.ChangeScene("");
    }
}
