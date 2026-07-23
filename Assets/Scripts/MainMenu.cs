using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource themeBeginning;
    public AudioSource themeLoop;
    public GameObject fadeIn;
    public GameObject mainPanel;
    public GameObject instructions;
    public float fadeInDuration;
    private bool hasBegunLooping = false;
    void OnEnable()
    {
        Debug.Log("OnEnable triggered");
        fadeIn.SetActive(true);
        instructions.SetActive(false);
        Time.timeScale = 1f; //in case we came from the pause menu
        StartCoroutine(begin());
    }

    IEnumerator begin()
    {
        yield return new WaitForSeconds(0.85f);
        Image fadeInImage = fadeIn.GetComponent<Image>();
        float timer = 0f;

        while(timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeInDuration);
            fadeInImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        fadeInImage.color = new Color(0f, 0f, 0f, 0f);
        fadeIn.SetActive(false);
    }

    public void playButtonPressed()
    {
        SceneManager.LoadScene("Main Game Scene");
    }

    public void instructionsOn()
    {
        instructions.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void instructionsOff()
    {
        instructions.SetActive(false);
        mainPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!themeBeginning.isPlaying && !hasBegunLooping)
        {
            themeLoop.Play();
            hasBegunLooping = true;
        }
    }
}
