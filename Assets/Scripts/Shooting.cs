using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public bool shootingTime = false;
    public Camera mainCamera;
    private Vector3 startingPOS;
    private Quaternion startingRotation;
    public GameObject monster;
    public GameObject computer;
    public GameObject computerScreen;
    public Light muzzleFlash;
    public Vector3 shootPOS = new Vector3(22.9f, 0.4f, 56.7f);
    public int bullets = 3;
    private int totalBullets;

    private AudioSource[] audios;

    public Texture2D cursorTexture;
    public Vector2 cursorHotSpot;

    public static Shooting Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public bool getShootingTime()
    {
        return shootingTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPOS = transform.position;
        startingRotation = transform.rotation;
        muzzleFlash.enabled = false;
        totalBullets = bullets;

        audios = GetComponents<AudioSource>();
        string audiosFound = "Shotgun audios loaded:\n";
        foreach(AudioSource audio in audios)
        {
            audiosFound += audio.clip.name + "\n";
        }
        Debug.Log(audiosFound);
    }

    private AudioSource findAudioClip(string name)
    {
        foreach(AudioSource source in audios)
        {
            if(source.clip.name == name) return source;
        }
        return null;
    }

    private void playAudioSource(string name)
    {
        AudioSource a = findAudioClip(name);
        if (a != null)
        {
            a.Play();
            return;
        }
        Debug.Log("Failure finding audio " + name);
    }

    public void OnMouseDown()
    {
        if (shootingTime == false && ComputerClick.Instance.getState() == false) 
        { 
            shootingTime = true;
            StartCoroutine(ShootingMode()); 
        }
    }

    IEnumerator ShootingMode()
    {
        playAudioSource("shotgun cock");

        ShootingUIHandler.Instance.shootingUI(true);
        ShootingUIHandler.Instance.updateBullets(bullets, totalBullets);
        transform.rotation = Quaternion.identity; //reset rotation
        transform.position = shootPOS;

        yield return null;
        while (shootingTime)
        {
            if (Input.GetMouseButtonDown(0) && bullets > 0)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == monster)
                    {
                        Debug.Log("Monster hit!");
                        MonsterController.Instance.shot(0.05f);
                    }
                    else if(hit.collider.gameObject == this.gameObject || hit.collider.gameObject == computer || hit.collider.gameObject == computerScreen)
                    {
                        playAudioSource("shotgun put down");
                        goto endCoroutine;
                    }
                }
                transform.LookAt(hit.point);
                muzzleFlash.enabled = true;
                playAudioSource("shotgun shoot");
                yield return new WaitForSeconds(0.05f);
                muzzleFlash.enabled = false;
                shootingTime = false;
                changeBullets(-1);
            }
            else if (Input.GetMouseButtonDown(0) && bullets == 0) playAudioSource("shotgun no ammo");

            yield return null;
        }
        
        endCoroutine:
        shootingTime = false;
        ShootingUIHandler.Instance.shootingUI(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (shootingTime == true) playAudioSource("shotgun put down"); //if statement ensures it isnt played back-to-back between frames
            StartCoroutine(setShootingTimeFalse());
        }

        if(shootingTime == false)
        {
            transform.position = startingPOS;
            transform.rotation = startingRotation;
        }

        handleCursor();
    }

    private void handleCursor()
    {
        if(shootingTime == true)
        {
            Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
    IEnumerator setShootingTimeFalse()
    {
        yield return null;
        shootingTime = false;
    }

    public void changeBullets(int a)
    {
        bullets += a;
        if(bullets > totalBullets)
        {
            totalBullets = bullets;
        }
    }
}
