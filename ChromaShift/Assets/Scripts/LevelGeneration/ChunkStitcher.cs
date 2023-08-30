using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ChunkStitcher : MonoBehaviour
{
    [SerializeField] private string directory = "Assets/Prefabs/Chunks";

    private static GameObject[] prefabArray;

    private static GameObject previousChunkPosition;

    private static List<GameObject> generatedChunks = new List<GameObject>();

    private void Start()
    {

        string[] prefabPaths = Directory.GetFiles(directory, "*.prefab");

        prefabArray = prefabPaths.Select(path => AssetDatabase.LoadAssetAtPath<GameObject>(path)).ToArray();

        //Generates the first chunk
        GenerateChunk();

        //Generates the next chunk
        GenerateChunk();
    }

    public void GenerateChunk()
    {
        int index = Random.Range(0, prefabArray.Length);

        GameObject generated = Instantiate(prefabArray[index], GameObject.Find("World").transform);

        Vector3 exit;

        if (previousChunkPosition != null)
        {
            exit = previousChunkPosition.transform.Find("Exit").position;
        }
        else
        {
            exit = Vector3.zero;
        }

        Vector3 entry = generated.transform.Find("Entry").position;

        Debug.Log(exit);
        Debug.Log(entry);

        generated.transform.localPosition = exit - entry;

        generatedChunks.Add(generated);

        previousChunkPosition = generated;
    }

    public void DeleteChunk()
    {
        Debug.Log(generatedChunks.Count);
        if (generatedChunks.Count > 3)
        {
            Destroy(generatedChunks[0]);

            generatedChunks.RemoveAt(0);
        }
    }
}
