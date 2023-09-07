using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

public class HoloframeGenerator : MonoBehaviour
{
    [SerializeField] private string directory = "HoloframeParts";
    [SerializeField] private Vector3[] coOrdinates;

    private GameObject[] prefabArray;

    private List<GameObject> gameObjects = new List<GameObject>();

    private void Start()
    {
        prefabArray = Resources.LoadAll<GameObject>(directory);

        GenerateSegments();

        ColorObject();
    }

    private void GenerateSegments()
    {
        foreach (Vector2 obj in coOrdinates)
        {
            GameObject child = Instantiate(prefabArray[Random.Range(0, prefabArray.Length)], transform);
            child.transform.localPosition = obj;
            gameObjects.Add(child);
        }
    }

    private void ColorObject()
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        int childIndex = Random.Range(0, children.Length);

        GameObject chosen = children[childIndex].gameObject;
        chosen.AddComponent<SetColorFromTag>();
    }
}
