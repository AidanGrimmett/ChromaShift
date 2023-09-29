using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayBelowPlatforms : MonoBehaviour
{
    private ChunkStitcher chunks;

    private void Start()
    {
        chunks = GameObject.Find("World").GetComponent<ChunkStitcher>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, chunks.GetCurrentChunk().transform.Find("MinHeight").localPosition.z * 100, 0);
    }
}
