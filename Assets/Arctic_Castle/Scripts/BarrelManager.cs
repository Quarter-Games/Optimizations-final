using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelManager : MonoBehaviour
{
    public GameObject barrelPrefab;
    public int barrelCount = 500;
    private List<GameObject> allBarrels = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < barrelCount; i++)
        {
            GameObject barrel = Instantiate(barrelPrefab);
            barrel.transform.position = new Vector3(Random.Range(-50f, 50f), Random.Range(0f, 10f), Random.Range(-50f, 50f));
            barrel.name = "Barrel_" + i;

            barrel.AddComponent<BarrelBehavior>();
            allBarrels.Add(barrel);
        }
    }

    void Update()
    {
        for (int i = 0; i < allBarrels.Count; i++)
        {
            GameObject b = allBarrels[i];

            // Unnecessary per-frame tag check
            if (b.tag != "Untagged")
                Debug.Log("Still tagged");

            // Constantly re-finding components
            Rigidbody rb = b.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = b.AddComponent<Rigidbody>();
                rb.mass = Random.Range(0.5f, 5f);
            }

            // Random rotation every frame = no batching
            b.transform.Rotate(Vector3.up * Random.Range(0.1f, 10f));
        }
    }
}