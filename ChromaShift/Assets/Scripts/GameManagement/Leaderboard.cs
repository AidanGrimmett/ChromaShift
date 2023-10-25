using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTemplate;

    private List<Transform> highscoreTransforms;

    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //AddHighscoreEntry(21, );

        for (int i = 0; i < highscores.highscoresList.Count; i++)
        {
            for (int j = 0; j < highscores.highscoresList.Count; j++)
            {
                if (highscores.highscoresList[j].score < highscores.highscoresList[i].score)
                {
                    HighscoreEntry temp = highscores.highscoresList[i];
                    highscores.highscoresList[i] = highscores.highscoresList[j];
                    highscores.highscoresList[j] = temp;
                }
                if (highscores.highscoresList[j].score == highscores.highscoresList[i].score)
                {
                    if (highscores.highscoresList[j].score < highscores.highscoresList[i].score)
                    {
                        HighscoreEntry temp = highscores.highscoresList[i];
                        highscores.highscoresList[i] = highscores.highscoresList[j];
                        highscores.highscoresList[j] = temp;
                    }
                }
            }
        }

        highscoreTransforms = new List<Transform>();

        foreach(HighscoreEntry entry in highscores.highscoresList )
        {
            CreateHigscoreEntry(entry, entryContainer, highscoreTransforms);
        }
    }

    private void CreateHigscoreEntry(HighscoreEntry highScore, Transform container, List<Transform> transformList)
    {
        float templateHeight = 125f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector3(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        Transform[] children = entryTransform.GetComponentsInChildren<Transform>(); 
        
        string name = highScore.name;
        int score = highScore.score;
        float time = highScore.time;

        children[1].GetComponent<Text>().text = name;
        children[2].GetComponent<Text>().text = score.ToString();
        children[3].GetComponent<Text>().text = time.ToString();

        transformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(int score, float time, string name)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name, time = time };

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        
        if (highscores.highscoresList[4].score < score || (highscores.highscoresList[5].score == score && highscores.highscoresList[4].time < time ))
        {
            highscores.highscoresList.Add(highscoreEntry);
        }

        if (highscores.highscoresList.Count > 5)
        {
            for (int h = highscores.highscoresList.Count; h > 5; h--)
            {
                highscores.highscoresList.RemoveAt(5);
            }
        }

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoresList;
    }

    [System.Serializable] private class HighscoreEntry
    {
        public string name;
        public int score;
        public float time;
    }
}
