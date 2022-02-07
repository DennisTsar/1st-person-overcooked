using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Controls orders

public class OrdersController : MonoBehaviourPunCallbacks
{
    // Array of all possible recipes
    public GameObject[] recipes;

    // List of current orders to fulfill
    List<GameObject> orders;

    int MAX_ORDERS = 4;
    float NEW_ORDER_FREQUENCY = 5f;
    public int score = 0;


    void Start()
    {
        orders = new List<GameObject>();

        InvokeRepeating("NewOrder", 0, NEW_ORDER_FREQUENCY);
    }

    void NewOrder()
    {
        
        int recipeIdx = Random.Range(0, recipes.Length);
        photonView.RPC("NewOrder", RpcTarget.All, recipeIdx);
    }

    [PunRPC]
    void NewOrder(int recipeIdx)
    {
        if (orders.Count < MAX_ORDERS)
        {
            // Create new random order
            GameObject order = Instantiate(recipes[Random.Range(0, recipes.Length)]);
            order.transform.SetParent(this.transform);
            order.transform.localPosition = new Vector3(0, 0, 0);
            foreach (GameObject o in orders)
            {
                o.transform.localPosition += new Vector3(300, 0, 0);
            }
            orders.Add(order);
        }
    }

    public void DeleteOrder(string order)
    {
        photonView.RPC("DeleteOrderRPC", RpcTarget.All, order);
    }

    void DeleteOrderRPC(string order)
    {
        // Find and delete order by tag
        for (int i = orders.Count - 1; i >= 0; i--)
        {
            if (orders[i].name.StartsWith(order))
            {
                Destroy(orders[i]);
                orders.RemoveAt(i);
                //Add score
                score += 100;
                GameObject.Find("Canvas").transform.Find("Score").GetComponent<Text>().text = "Score: " + score;
                break;
            }
        }
        // Rearrange remaining orders
        for (int i = orders.Count - 1; i >= 0; i--)
        {
            orders[i].transform.localPosition = new Vector3(300 * i, 0, 0);
        }
    }
}
