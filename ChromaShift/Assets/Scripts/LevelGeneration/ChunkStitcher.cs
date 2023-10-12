using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkStitcher : MonoBehaviour
{
    //The folder in Resources that contains the chunk prefabs.
    [SerializeField] private string directory = "Chunks";

    //Array that is loaded up with the chunks.
    private static GameObject[] prefabArray;

    //Keeps track of the last loaded chunk. Useful for keeping track of the end point.
    private static GameObject previousChunk;

    //Allows the current generated chunks to be interacted with such as resetting of the position.
    private static List<GameObject> generatedChunks = new List<GameObject>();

    private void Start()
    {
        prefabArray = Resources.LoadAll<GameObject>(directory);

        //Generates the first 3 chunks of the game.
        GenerateChunk(Vector3.zero);
        GenerateChunk();
    }

    public void GenerateChunk()
    {
        //Chooses a random index in the list of chunks
        int index = Random.Range(0, prefabArray.Length);

        //Creates the new chunk
        GameObject generated = Instantiate(prefabArray[index], GameObject.Find("World").transform);

        Vector3 entry = generated.transform.Find("Entry").position;

        Vector3 exit = previousChunk.transform.Find("Exit").position;

        generated.transform.localPosition = exit - entry; //Calculates where the chunk should be

        generatedChunks.Add(generated); //Keeps a record of what chunks are currently active

        previousChunk = generated; //Sets the last chunk position to be the newly generated chunk
    }

    private void GenerateChunk(Vector3 exit)
    {
        //This is for the special case where we don't have a previous generated chunk. Will only be run once.
        int index = Random.Range(0, prefabArray.Length);

        GameObject generated = Instantiate(prefabArray[index], GameObject.Find("World").transform);

        Vector3 entry = generated.transform.Find("Entry").position;

        generated.transform.localPosition = Vector3.zero - entry; //Calculates where the chunk should be

        generatedChunks.Add(generated); //Keeps a record of what chunks are currently active

        previousChunk = generated; //Sets the last chunk position to be the newly generated chunk
    }

    public void DeleteChunk()
    {
        if (generatedChunks.Count > 3)
        {
            //Deletes the oldest chunk.
            Destroy(generatedChunks[0]);

            generatedChunks.RemoveAt(0);
        }
    }

    //To stop the world object heading towards negtive infinity.
    public void ResetChunks()
    {
        Transform wld = GameObject.Find("World").transform;
        Transform plr = GameObject.Find("Player").transform;

        float xOffset = wld.position.x;
        float yOffset = generatedChunks[1].transform.localPosition.y;

        foreach (GameObject chunk in generatedChunks)
        {
            chunk.transform.localPosition = new Vector3(chunk.transform.localPosition.x + xOffset, chunk.transform.localPosition.y - yOffset, chunk.transform.localPosition.z);
        }

        plr.transform.position = new Vector3(plr.position.x, plr.position.y - yOffset, plr.transform.position.z);
        wld.transform.position = new Vector3(0, wld.transform.position.y, wld.transform.position.z);
    }

    //Useful for grabbing the oldest generated chunk. That is the one the player is currently in.
    public GameObject GetCurrentChunk()
    {
        if (generatedChunks.Count != 0)
        {
            return generatedChunks[0];
        }
        else
        {
            return null;
        }
    }

    //Useful for restarting the game.
    public static void EmptyGeneratedChunks()
    {
        generatedChunks.Clear();
    }
}
