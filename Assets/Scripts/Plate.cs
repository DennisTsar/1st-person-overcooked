using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Plate : MonoBehaviour
{
    public GameObject final1;
    public List<string> dish1;
    public GameObject final2;
    public List<string> dish2;
    public List<GameObject> stuff;
    public bool done = false;
    private List<string> dish;
    private GameObject final;
    void Start()
    {
        
    }

    public virtual bool Add(GameObject g)
    {
        if (!done && g.name.StartsWith(dish1[stuff.Count]))
        {
            dish = dish1;
            final = final1;
        }
        else if (!done && g.name.StartsWith(dish2[stuff.Count]))
        {
            dish = dish2;
            final = final2;
        }
        if (!done && dish!=null && (g.name.StartsWith(dish[stuff.Count])))
        {
            if(stuff.Count == dish.Count-1)
            {
                foreach (GameObject go in stuff)
                    Destroy(go);
                Destroy(g);
                final = Instantiate(final, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity, transform);
                final.transform.localScale = new Vector3(5f, 5f, 5f);
                done = true;
            }
            else
            { 
                stuff.Add(g);
                g.transform.parent = transform;
                if (g.name.StartsWith("Toast"))
                    g.transform.rotation = Quaternion.Euler(90, 0, 0);
                if (stuff.Count == 1)
                    g.transform.position = transform.position + new Vector3(0, transform.localScale.y, 0);
                else
                {
                    GameObject go = stuff[stuff.Count - 2];
                    g.transform.position = go.transform.position + new Vector3(0, go.GetComponent<Renderer>().bounds.size.y, 0);
                }

            }
            return true;
        }
        return false;
    }
}
