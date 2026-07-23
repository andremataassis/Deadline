using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Numberdle : MonoBehaviour
{
    int[] answer = new int[5];
    int[] guess = new int[5] { -1, -1, -1, -1, -1 };
    private int newNumber;
    private TMP_InputField[] boxes;

    // Start is called before the first frame update
    void Start()
    {
        generateAnswer();
        boxes = GetComponentsInChildren<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ReadStringInput(string s)
    {
        int number;
        bool isParsedSuccesfully = int.TryParse(s, out number);
        if (isParsedSuccesfully)
        {
            newNumber = number;
            PlayAudioHandler.Instance.playRandomAudio("ComputerClick", 5);
        }
        else
        {
            newNumber = -1;
        }
    }

    private void smartSelectNextBox(int i)
    {
        boxes[i].DeactivateInputField();
        if(!(i == 4))
        {
            boxes[i + 1].Select();
        }
    }

    public void ReadInputSource(int i)
    {
        guess[i] = newNumber;
        smartSelectNextBox(i);
    }

    public void guessSubmitted()
    {
        PlayAudioHandler.Instance.playRandomAudio("ComputerClick", 5);

        //check if guess is valid
        for (int i = 0; i < guess.Length; i++)
        {
            if (guess[i] == -1)
            {
                return;
            }
        }

        Debug.Log("guessSubmitted ran and passed validity");

        int correctSpotsFound = 0;
        //show correct spots
        for(int i = 0; i < answer.Length; i++)
        {
            Image square = transform.GetChild(i).GetComponentInChildren<Image>();
            if (guess[i] == answer[i])
            {
                square.color = Color.green;
                correctSpotsFound++;
            }
            else
            {
                square.color = Color.red;
            }
        }

        if(correctSpotsFound == 5)
        {
            GameManager.Instance.taskDone();
        }
    }

    private void generateAnswer()
    {
        for(int i = 0; i < answer.Length; i++)
        {
            answer[i] = Random.Range(0, 10);
        }
    }
}
