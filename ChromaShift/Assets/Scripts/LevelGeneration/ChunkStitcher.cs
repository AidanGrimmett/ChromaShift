using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class ChunkStitcher : MonoBehaviour
{
    [SerializeField] private string directory = "Chunks";

    private static GameObject[] prefabArray;

    private static GameObject previousChunkPosition;

    private static List<GameObject> generatedChunks = new List<GameObject>();

    private void Start()
    {
        prefabArray = Resources.LoadAll<GameObject>(directory);

        //Generates the first two chunks
        GenerateChunk();
        GenerateChunk();
    }

    public void GenerateChunk()
    {
        //Chooses a random index in the list of chunks
        int index = Random.Range(0, prefabArray.Length);

        //Creates the new chunk
        GameObject generated = Instantiate(prefabArray[index], GameObject.Find("World").transform);

        Vector3 exit;

        if (previousChunkPosition != null)
        {
            exit = previousChunkPosition.transform.Find("Exit").position; //Determines the starting point of the next chunk
        }
        else
        {
            exit = Vector3.zero; //Starts from (0,0,0) if first chunk
        }

        Vector3 entry = generated.transform.Find("Entry").position;

        generated.transform.localPosition = exit - entry; //Calculates where the chunk should be

        generatedChunks.Add(generated); //Keeps a record of what chunks are currently active

        previousChunkPosition = generated; //Sets the last chunk position to be the newly generated chunk
    }

    public void DeleteChunk()
    {
        if (generatedChunks.Count > 3)
        {
            Destroy(generatedChunks[0]);

            generatedChunks.RemoveAt(0);
        }
        //Deletes chunks so we only have 3 generated at one time
    }
}
