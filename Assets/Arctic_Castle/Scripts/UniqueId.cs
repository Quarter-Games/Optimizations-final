using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

[ExecuteAlways]
public class UniqueId : MonoBehaviour
{
    [SerializeField, HideInInspector] private string id;

    public string Id
    {
        get
        {
            if (string.IsNullOrEmpty(id))
            {
                Generate();
            }
            return id;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        if (string.IsNullOrEmpty(id))
        {
            Generate();
            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(this);
            }
        }
    }
#endif

    private void Generate()
    {
        id = Guid.NewGuid().ToString("N"); // 32 chars, stable
    }
}

