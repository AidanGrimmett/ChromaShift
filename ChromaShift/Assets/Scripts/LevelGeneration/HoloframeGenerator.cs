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

    [SerializeField] Material invisible;

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

        children = chosen.GetComponentsInChildren<Transform>();

        childIndex = Random.Range(0, children.Length);

        chosen = children[childIndex].gameObject;

        GameObject active = Instantiate(chosen, chosen.transform);
        GameObject inactive = Instantiate(active, chosen.transform);

        Destroy(GetComponent<BoxCollider>());
        Destroy(GetComponent<MeshRenderer>());

        active.AddComponent<SetColorFromTag>();
        active.name = "Active";
        active.transform.localPosition = Vector3.zero;
        active.transform.localScale = Vector3.one;

        inactive.name = "Inactive";
        inactive.transform.localPosition = Vector3.zero;
        active.transform.localScale = Vector3.one;
        inactive.AddComponent<Outline>();
        inactive.AddComponent<SetOutlineColor>();
        inactive.GetComponent<MeshRenderer>().enabled = false;
        Destroy(inactive.GetComponent<BoxCollider>());

        chosen.AddComponent<RandomTagGeneration>();
        chosen.AddComponent<ColoredObjectActivator>();
        Destroy(chosen.GetComponent<MeshRenderer>());
        Destroy(chosen.GetComponent<BoxCollider>());
    }
}
