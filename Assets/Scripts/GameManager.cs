using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public bool isLoaded;

    //Select GameManager's instance
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            if(instance != this && !instance.isLoaded)
            {
                Destroy(gameObject);
            }
        }
    }

    public void LoadLevel()
    {
        if (wonPanel != null)
            wonPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
        StartCoroutine(loadingLevel(Application.loadedLevelName));
        isLoaded = true;
    }

    public void LoadLevel(string levelName)
    {
        if (wonPanel != null)
            wonPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
        StartCoroutine(loadingLevel(levelName));
    }

    IEnumerator loadingLevel(string LevelName)
    {
        AsyncOperation async = Application.LoadLevelAsync(LevelName);

        while (!async.isDone)
        {
            Debug.Log(async.progress);
            yield return null;
        }
    }

    public GameObject wonPanel;
    public void LevelCompleted()
    {
        wonPanel = GameObject.Find("WonPanel");
        wonPanel.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    void FixedUpdate()
    {
        if (isLoaded && instance == this)
        {
            Destroy(gameObject);
        }
    }

}
