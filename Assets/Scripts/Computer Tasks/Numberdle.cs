using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Numberdle : MonoBehaviour
{
    int[] answer = new int[5];
    int[] guess = new int[5] { 0, 0, 0, 0, 0};
    [SerializeField] List<GameObject> input_squares = new List<GameObject>();
    private int newNumber;

    // Start is called before the first frame update
    void Start()
    {
        generateAnswer();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void IncreaseGuessAt(int i)
    {
        guess[i] = (guess[i] + 1) % 10;
        PlayAudioHandler.Instance.playRandomAudio("ComputerClick", 5);
        input_squares[i].GetComponentInChildren<TMP_Text>().text = guess[i].ToString();
    }
    public void DecreaseGuessAt(int i)
    {
        guess[i] = ((guess[i] - 1) % 10 + 10) % 10;
        PlayAudioHandler.Instance.playRandomAudio("ComputerClick", 5);
        input_squares[i].GetComponentInChildren<TMP_Text>().text = guess[i].ToString();
    }

    public void guessSubmitted()
    {
        PlayAudioHandler.Instance.playRandomAudio("ComputerClick", 5);

        int correctSpotsFound = 0;
        //show correct spots
        for(int i = 0; i < answer.Length; i++)
        {
            Image square = input_squares[i].GetComponentInChildren<Image>();
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
            //0-9
            answer[i] = Random.Range(0, 10);
        }
    }
}
