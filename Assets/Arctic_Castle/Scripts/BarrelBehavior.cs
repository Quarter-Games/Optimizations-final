using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BarrelBehavior : MonoBehaviour
{
    void Update()
    {
        // GC Spike #1: LINQ every frame
        var randomList = new List<int> { 1, 2, 3, 4, 5 };
        var even = randomList.Where(x => x % 2 == 0).ToList(); // alloc + closure

        // GC Spike #2: String formatting (heap alloc)
        string debugText = "Barrel Pos: " + transform.position.ToString(); // boxing + ToString()
        Debug.Log(debugText); // more GC if logging in Editor

        // GC Spike #3: List allocation every frame
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < 5; i++)
        {
            points.Add(UnityEngine.Random.insideUnitSphere);
        }

        // GC Spike #4: Random scale and raycast still here
        transform.localScale = Vector3.one * UnityEngine.Random.Range(0.2f, 0.3f);

        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }

        // GC Spike #5: Instantiate object mid-update
        GameObject clone = Instantiate(gameObject); // massive spike
        Destroy(clone, 0.1f); // Also triggers GC
    }
}
