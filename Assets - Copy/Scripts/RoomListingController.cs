using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomListingController : MonoBehaviour
{
    [SerializeField] TMP_Text name;
    [SerializeField] TMP_Text count;
    public void Setup(string name, int count)
    {
        this.name.text = name;
        this.count.text = count+"";
    }
}
