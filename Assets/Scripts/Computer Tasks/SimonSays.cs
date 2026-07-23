using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimonSays : MonoBehaviour
{
    public GameObject beginScreen;
    public GameObject flashScreen;
    public GameObject questionScreen;
    public float flashOnScreenDuration = 0.8f;

    public Color red;
    public Color green;
    public Color blue;
    public Color yellow;
    private Color defaultColor;

    private Color[] colorAppearanceOrder = new Color[4];
    private int currentColor = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        beginScreen.SetActive(true);
        flashScreen.SetActive(false);
        questionScreen.SetActive(false);
        defaultColor = flashScreen.GetComponent<Image>().color;

        red.a = 1;
        green.a = 1;
        blue.a = 1;
        yellow.a = 1;

        defaultColor.a = 0;
    }

    public void beginTask()
    {
        beginScreen.SetActive(false);
        StartCoroutine(flashSequence());
    }

    IEnumerator flashSequence()
    {
        flashScreen.SetActive(true);
        TextMeshProUGUI countdown = flashScreen.GetComponentInChildren<TextMeshProUGUI>();
        for (int i = 3; i > 0; i--)
        {
            countdown.text = i.ToString();
            PlayAudioHandler.Instance.playAudioSource("beep");
            yield return new WaitForSeconds(1f);
        }

        countdown.text = "";
        Image flash = flashScreen.GetComponent<Image>();

        for(int i = 0; i < 4; i++)
        {
            int a = Random.Range(0, 4);
            if(a == 0)
            {
                flash.color = red;
                PlayAudioHandler.Instance.playAudioSource("red");
                colorAppearanceOrder[i] = red;
            }
            else if(a == 1)
            {
                flash.color = blue;
                PlayAudioHandler.Instance.playAudioSource("blue");
                colorAppearanceOrder[i] = blue;
            }
            else if (a == 2)
            {
                flash.color = yellow;
                PlayAudioHandler.Instance.playAudioSource("yellow");
                colorAppearanceOrder[i] = yellow;
            }
            else if (a == 3)
            {
                flash.color = green;
                PlayAudioHandler.Instance.playAudioSource("green");
                colorAppearanceOrder[i] = green;
            }
            yield return new WaitForSeconds(flashOnScreenDuration);
        }

        flash.color = defaultColor;
        questionScreen.SetActive(true);
    }

    public void readColorOrderInput(string input)
    {
        Color toCompare = new Color();
        if(input == "red")
        {
            toCompare = red;
        }
        else if(input == "green")
        {
            toCompare = green;
        }
        else if (input == "yellow")
        {
            toCompare = yellow;
        }
        else if (input == "blue")
        {
            toCompare = blue;
        }

        if (colorAppearanceOrder[currentColor] == toCompare)
        {
            currentColor++;
            PlayAudioHandler.Instance.playAudioSource(input);
            if (currentColor == 4)
            {
                GameManager.Instance.taskDone();
            }
        }
        else
        {
            PlayAudioHandler.Instance.playAudioSource("incorrectBeep");
            currentColor = 0;
            Start();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
