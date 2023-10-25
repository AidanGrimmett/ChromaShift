using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    Start,
    Play,
    End,
    Leaderboard,
    Options,
    Controls
}

public class MenuManager : MonoBehaviour
{
    public GameState gameState;

    [SerializeField] private GameObject start;
    [SerializeField] private GameObject play;
    [SerializeField] private GameObject end;
    [SerializeField] private GameObject leaderboard;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject controls;

    [SerializeField] private Leaderboard leaderboardValues;

    private bool submitted;

    private GameObject player;
    private PlayerHealthBarScript playerHealthScript;
    private GameObject world;
    [SerializeField]private GameObject cam;

    [SerializeField] GameObject worldPrefab;

    private RigidbodyConstraints unFrozenPlayer;
    private RigidbodyConstraints unFrozenWorld;

    public void Start()
    {
        gameState = GameState.Start;

        Debug.Log(leaderboard.name);

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
                leaderboard.SetActive(false);
                options.SetActive(false);
                controls.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameState.Play:
                start.SetActive(false);
                play.SetActive(true);
                end.SetActive(false);
                leaderboard.SetActive(false);
                options.SetActive(false);
                controls.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case GameState.End:
                //leaderboardValues.AddHighscoreEntry(Score.score, Time.time,)
                start.SetActive(false);
                play.SetActive(false);
                end.SetActive(true);
                leaderboard.SetActive(false);
                options.SetActive(false);
                controls.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameState.Leaderboard:
                leaderboardValues.ClearScores();
                leaderboardValues.LoadScores();
                start.SetActive(false);
                play.SetActive(false);
                end.SetActive(false);
                leaderboard.SetActive(true);
                options.SetActive(false);
                controls.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameState.Options:
                start.SetActive(false);
                play.SetActive(false);
                end.SetActive(false);
                leaderboard.SetActive(false);
                options.SetActive(true);
                controls.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameState.Controls:
                start.SetActive(false);
                play.SetActive(false);
                end.SetActive(false);
                leaderboard.SetActive(false);
                options.SetActive(false);
                controls.SetActive(true);
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

    public void LeaderboardMenu()
    {
        gameState = GameState.Leaderboard;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        world.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        world.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void OptionsMenu()
    {
        gameState = GameState.Options;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        world.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        world.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void ControlsMenu()
    {
        gameState = GameState.Controls;
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
        player.transform.rotation = Quaternion.identity;
    }

    public void GameOver()
    {
        gameState = GameState.End;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        world.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        world.GetComponent<Rigidbody>().velocity = Vector3.zero;

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Reload()
    {
        ReloadWorld();
        player.transform.position = new Vector3(-9.5f, 15, 0);
    }

    private void ReloadWorld()
    {
        ChunkStitcher.EmptyGeneratedChunks();
        world.transform.position = Vector3.zero;
        GameObject entrance = world.GetComponent<ChunkStitcher>().spawnTube;
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
        world.AddComponent<ChunkStitcher>().spawnTube = entrance;
    }
}