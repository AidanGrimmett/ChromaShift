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
        int index = Random.Range(0, gameObjects.Count);

        Transform[] children = gameObjects[index].GetComponentsInChildren<Transform>();

        List<Transform> validChildren = new List<Transform>();

        foreach (Transform child in children)
        {
            if (child != gameObjects[index].transform)
            {
                validChildren.Add(child);
            }
        }

        index = Random.Range(0, validChildren.Count);

        GameObject chosen = validChildren[index].gameObject;

        chosen.AddComponent<RandomTagGeneration>();
        chosen.AddComponent<ColoredObjectActivator>();

        SpriteRenderer rend = chosen.GetComponent<SpriteRenderer>();

        GameObject active = new GameObject("Active");
        active.transform.parent = chosen.transform;
        active.transform.position = chosen.transform.position;
        active.transform.localScale = chosen.transform.localScale;
        active.transform.rotation = chosen.transform.rotation;
        SpriteRenderer activeRend = active.AddComponent<SpriteRenderer>();
        activeRend.sprite = rend.sprite;
        active.AddComponent<SetColorFromTag>();
        active.AddComponent<BoxCollider>();

        GameObject inactive = new GameObject("Inactive");
        inactive.transform.parent = chosen.transform;
        inactive.transform.position = chosen.transform.position;
        inactive.transform.localScale = chosen.transform.localScale;
        inactive.transform.rotation = chosen.transform.rotation;
        SpriteRenderer inactiveRend = inactive.AddComponent<SpriteRenderer>();
        inactiveRend.sprite = rend.sprite;
        inactiveRend.color = new Color(0, 0, 0, 0);

        Destroy(chosen.GetComponent<SpriteRenderer>());
        Destroy(chosen.GetComponent<BoxCollider>());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(coOrdinates[1].x * 2f, coOrdinates[1].x, 0.1f));
    }
}
