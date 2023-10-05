using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState
{
    Start,
    Play,
    End
}

public class MenuManager : MonoBehaviour
{
    public GameState gameState;

    [SerializeField] private GameObject start;
    [SerializeField] private GameObject play;
    [SerializeField] private GameObject end;

    private GameObject player;
    private PlayerHealthBarScript playerHealthScript;
    private GameObject world;

    [SerializeField] GameObject worldPrefab;

    private RigidbodyConstraints unFrozenPlayer;
    private RigidbodyConstraints unFrozenWorld;

    public void Start()
    {
        gameState = GameState.Start;

        player = GameObject.Find("Player");
        playerHealthScript = player.GetComponent<PlayerHealthBarScript>();
        world = GameObject.Find("World");

        unFrozenPlayer = player.GetComponent<Rigidbody>().constraints;
        unFrozenWorld = world.GetComponent<Rigidbody>().constraints;

        MainMenu();
    }

    public void Update()
    {
        switch (gameState)
        {
            case GameState.Start:
                start.SetActive(true);
                play.SetActive(false);
                end.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameState.Play:
                start.SetActive(false);
                play.SetActive(true);
                end.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case GameState.End:
                start.SetActive(false);
                play.SetActive(false);
                end.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }

    public void MainMenu()
    {
        gameState = GameState.Start;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        world.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        world.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void StartGame()
    {
        gameState = GameState.Play;
        playerHealthScript.SetHealth(float.MaxValue);
        player.GetComponent<Rigidbody>().constraints = unFrozenPlayer;
        world.GetComponent<Rigidbody>().constraints = unFrozenWorld;
    }

    public void GameOver()
    {
        gameState = GameState.End;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        world.GetComponent<Rigidbody>().velocity = Vector3.zero;

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Reload()
    {
        ReloadWorld();
        player.transform.position = new Vector3(0, 1, 0);
        StartGame();
    }

    private void ReloadWorld()
    {
        ChunkStitcher.EmptyGeneratedChunks();
        world.transform.position = Vector3.zero;
        Destroy(world.GetComponent<ChunkStitcher>());
        List<GameObject> toDelete = new List<GameObject>();
        foreach (Transform child in world.GetComponentsInChildren<Transform>())
        {
            if (child.parent == world.transform)
            {
                toDelete.Add(child.gameObject);
            }
        }
        foreach (GameObject obj in toDelete)
        {
            Destroy(obj);
        }
        world.AddComponent<ChunkStitcher>();
    }
}