using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ClockScript : MonoBehaviour
{
    public float lengthOfNightInSeconds;
    private float timer;
    private float secondsInAMinute;
    private int minute;
    private int hour;
    private TextMeshProUGUI textDisplay;
    bool gameEnd = false;
    public float alarmBeepLength;

    // Start is called before the first frame update
    void Start()
    {
        hour = 12;
        minute = 0;
        secondsInAMinute = lengthOfNightInSeconds / 6f; //six hours in a night
        textDisplay = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        int hourBefore = hour;
        updateClockTime(timer);
        int hourAfter = hour;

        if(hourBefore != hourAfter)
        {
            PlayAudioHandler.Instance.playAudioSource("singleAlarmBeep"); //a beep to indicate a new hour
        }

        displayTime();
    }

    private void displayTime()
    {
        if(minute == 0)
        {
            string displayMinute = "00";
            textDisplay.text = hour.ToString() + ":" + displayMinute + "am";
        }
        else
        {
            textDisplay.text = hour.ToString() + ":" + minute.ToString() + "am";
        }
    }

    IEnumerator GameOverSequence()
    {
        PlayAudioHandler.Instance.playAudioSource("alarmBeeping");
        yield return new WaitForSeconds(alarmBeepLength);
        GameOver.gameOverReason = "Time";
        SceneManager.LoadScene("GameOver");
    }
    private void updateClockTime(float secondsSinceGameStart)
    {
        //sets the hour
        if ((timer / lengthOfNightInSeconds) <= 1.0f / 6.0f)
        {
            hour = 12;
        }
        else if ((timer / lengthOfNightInSeconds) <= 2.0f / 6.0f)
        {
            hour = 1;
        }
        else if ((timer / lengthOfNightInSeconds) <= 3.0f / 6.0f)
        {
            hour = 2;
        }
        else if ((timer / lengthOfNightInSeconds) <= 4.0f / 6.0f)
        {
            hour = 3;
        }
        else if ((timer / lengthOfNightInSeconds) <= 5.0f / 6.0f)
        {
            hour = 4;
        }
        else if ((timer / lengthOfNightInSeconds) <= 6.0f / 6.0f)
        {
            hour = 5;
        }
        else
        {
            hour = 6;
            minute = 0;
            if(gameEnd == false)
            {
                gameEnd = true;
                StartCoroutine(GameOverSequence());
            }
            return; //return here because minute will not be updated after 6am
        }

        //sets the minute
        float secondsIntoThisHour;
        if (hour == 12) 
        {
            secondsIntoThisHour = secondsSinceGameStart;
        }
        else
        {
            secondsIntoThisHour = secondsSinceGameStart - hour * secondsInAMinute;
        }
        if ((secondsIntoThisHour / secondsInAMinute) <= 1.0f / 6.0f)
        {
            minute = 0;
        }
        else if ((secondsIntoThisHour / secondsInAMinute) <= 2.0f / 6.0f)
        {
            minute = 10;
        }
        else if ((secondsIntoThisHour / secondsInAMinute) <= 3.0f / 6.0f)
        {
            minute = 20;
        }
        else if ((secondsIntoThisHour / secondsInAMinute) <= 4.0f / 6.0f)
        {
            minute = 30;
        }
        else if ((secondsIntoThisHour / secondsInAMinute) <= 5.0f / 6.0f)
        {
            minute = 40;
        }
        else if ((secondsIntoThisHour / secondsInAMinute) <= 6.0f / 6.0f)
        {
            minute = 50;
        }
    }
}
