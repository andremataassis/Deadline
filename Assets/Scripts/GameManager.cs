using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] tasks; //stores how many tasks will be completed
    public int currentTaskNumber = 0;
    private GameObject currentTask;
    public float waitBeforeTaskDone = 0.5f;
    private bool coroutineRunning = false;
    public Button logOutButton;
    public Light mainLight;
    public GameObject logOutInstructions;

    public static GameManager Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        logOutButton.interactable = false;
        logOutInstructions.SetActive(false);
        startNewTask();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void gameWon()
    {
        logOutButton.interactable = true;
        logOutInstructions.SetActive(true);
    }
    //these two work with each other
    public void gameWonCalledByLogOutButton()
    {
        SceneManager.LoadScene("GameWin");
    }

    public void taskDone()
    {
        if(coroutineRunning == false) StartCoroutine(taskDoneAnimation());
    }

    IEnumerator taskDoneAnimation()
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(waitBeforeTaskDone);

        currentTaskNumber++;
        Destroy(currentTask);
        PlayAudioHandler.Instance.playAudioSource("computerCompleteSFX");

        if (currentTaskNumber == tasks.Length)
        {
            gameWon();
        }
        else
        {
            startNewTask();
        }
        coroutineRunning = false;
    }

        private void startNewTask()
    {
        GameObject task = Instantiate(tasks[currentTaskNumber]);
        task.transform.SetParent(transform);

        RectTransform rectTransform = task.GetComponent<RectTransform>();

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);  // Center anchor
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);  // Center anchor
        rectTransform.pivot = new Vector2(0.5f, 0.5f);      // Center pivot
        rectTransform.anchoredPosition = Vector2.zero;      // Center position

        currentTask = task;
    }

    public int getTotalTasks()
    {
        return tasks.Length;
    }
    
    public int getCurrentTask()
    {
        return currentTaskNumber;
    }
}
