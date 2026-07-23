using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public GameObject monster;
    public GameObject UI;
    public float secondsOfJumpScare;
    public float animationSpeed;
    private Camera mainCamera;
    public AnimationCurve curve;
    public TextMeshProUGUI hint;

    public static string gameOverReason = "";

    // Start is called before the first frame update
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    void Start()
    {
        UI.SetActive(false);
        Animator monsterAnimations = monster.GetComponentInChildren<Animator>();
        monsterAnimations.speed = animationSpeed;
        hint.text = generateHintText();
        StartCoroutine(jumpScare());
    }

    private string generateHintText()
    {
        if(gameOverReason == "Monster")
        {
            int a = Random.Range(0, 3);
            if(a == 0)
            {
                return "Listen carefully for approaching footsteps while on the computer.";
            }
            else if(a == 1)
            {
                return "You are safe while the lights are off, and it is only then that you can fire your shotgun.";
            }
            else if(a == 2)
            {
                return "Missing a shot is fatal. Make sure you know where you're shooting.";
            }
        }
        else if(gameOverReason == "Time")
        {
            return "You ran out of time. Make sure to watch the clock and finish before the 6am deadline.";
        }

        return "Don't die.";
    }
    IEnumerator jumpScare()
    {
        StartCoroutine(cameraShake(secondsOfJumpScare));
        yield return new WaitForSeconds(secondsOfJumpScare);
        monster.SetActive(false);
        UI.SetActive(true);
    }

    IEnumerator cameraShake(float duration)
    {
        Vector3 startPosition = mainCamera.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            mainCamera.transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        mainCamera.transform.position = startPosition;
    }

    public void retry()
    {
        SceneManager.LoadScene("Main Game Scene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
