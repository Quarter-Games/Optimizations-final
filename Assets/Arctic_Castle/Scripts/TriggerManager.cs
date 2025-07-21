using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name!= "FpsController")
        {
            return;
        }
        // No tag check or layer filtering
        GameObject obj = other.gameObject;

        // GC Alloc: new string every frame
        Debug.Log("Something is inside the trigger: " + obj.name);

        // GC Alloc: create a temp list (for no reason)
        List<string> dummy = new List<string>();
        dummy.Add("triggered");
        dummy.Add(obj.tag);

        // Redundant enable of manager every frame
        GameObject barrelManagerObj = GameObject.Find("BarrelManager");
        if (barrelManagerObj != null)
        {
            BarrelManager bm = barrelManagerObj.GetComponent<BarrelManager>();
            if (bm != null)
            {
                bm.enabled = true;
            }
        }
    }
}
