using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    bool isPaused = false;
    public GameObject pausePanel;

    private void Start()
    {
        pausePanel.SetActive(false);
        GetComponent<Canvas>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Shooting.Instance.getShootingTime())
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }
}
