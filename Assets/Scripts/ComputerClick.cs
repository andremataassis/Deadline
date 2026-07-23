using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerClick : MonoBehaviour
{
    public Material on, off;
    public Light computerLight; //light that lights the room
    public Light redLight;
    public Light greenLight;
    public Camera cam;
    public InputRelaySource relaySource;

    private bool state = false; //true if on, false if off

    public Vector3 endMarkerOn;
    public Vector3 endMarkerOff;
    public Vector3 rotationOnEuler;
    public Vector3 rotationOffEuler;
    public float fovOn = 60f;
    public float fovOff = 40f;
    public float cameraSpeed = 1.5f;
    public float rotationSpeed = 180f;
    public float fovSpeed = 30f;
    public float viewTime = 0.2f;
    public float cameraCoolDown = 0.2f;
    public GameObject button;
    public GameObject computerScreen;

    private bool moveCamera = false; //whether the camera is currently moving
    private Vector3 cameraTargetPos;
    private Quaternion cameraTargetRot;
    private float cameraTargetFOV;

    //the following object and method allow other scripts to call this isntance of this script,
    //allowing the other scripts to get any info they need, namely the "state" variable, which is whether
    //the room is illuminated or not
    public static ComputerClick Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        button.SetActive(false);
        computerScreen.SetActive(false);
    }

    //so that other scripts may retrieve the game state (light or no light)
    public bool getState() { return state; }

    public void OnMouseDown()
    {
        //only move the camera if it is not currently in movement
        if(moveCamera == false)
        {
            if (!state)//turning it on
            {
                cameraTargetPos = endMarkerOn;
                cameraTargetRot = Quaternion.Euler(rotationOnEuler);
                cameraTargetFOV = fovOn;
                moveCamera = true;
                StartCoroutine(TurnOn());
            }
        }
    }

    public void turnOffComputer()
    {
        if (moveCamera == false)
        {
            if (state) //turning it off
            {
                cameraTargetPos = endMarkerOff;
                cameraTargetRot = Quaternion.Euler(rotationOffEuler);
                cameraTargetFOV = fovOff;
                moveCamera = true;
                StartCoroutine(TurnOff());
            }
        }
    }

    IEnumerator TurnOn()
    {
        //turns lights on
        computerLight.enabled = true;
        redLight.enabled = false;
        greenLight.enabled = true;
        
        //updates state to on
        state = true;

        //all of the fancy audio
        PlayAudioHandler.Instance.stopAudioSource("computerPoweringDown");
        PlayAudioHandler.Instance.playAudioSource("computerStartUp");

        yield return new WaitForSeconds(viewTime);

        //some more fancy audio
        PlayAudioHandler.Instance.playAudioSource("computerAmbiance");

        //moves the camera
        while (Vector3.Distance(cam.transform.position, cameraTargetPos) > 0.01f ||
            Quaternion.Angle(cam.transform.rotation, cameraTargetRot) > 0.5f ||
            Mathf.Abs(cam.fieldOfView - cameraTargetFOV) > 0.1f)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, cameraTargetPos, cameraSpeed * Time.deltaTime);
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, cameraTargetRot, rotationSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, cameraTargetFOV, fovSpeed * Time.deltaTime);
            yield return null;
        }

        //camera move cooldown
        button.SetActive(true);
        yield return new WaitForSeconds(cameraCoolDown);

        //let OnMouseDown know that the camera is no longer moving
        moveCamera = false;

        yield break;
    }

    IEnumerator TurnOff()
    {
        button.SetActive(false);

        //play audio
        PlayAudioHandler.Instance.playAudioSource("computerPoweringDown");
        PlayAudioHandler.Instance.stopAudioSource("computerAmbiance");
        PlayAudioHandler.Instance.stopAudioSource("computerStartUp");

        //moves the camera
        while (Vector3.Distance(cam.transform.position, cameraTargetPos) > 0.01f ||
            Quaternion.Angle(cam.transform.rotation, cameraTargetRot) > 0.5f ||
            Mathf.Abs(cam.fieldOfView - cameraTargetFOV) > 0.1f)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, cameraTargetPos, cameraSpeed * Time.deltaTime);
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, cameraTargetRot, rotationSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, cameraTargetFOV, fovSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(viewTime);

        //turns lights off
        computerLight.enabled = false;
        redLight.enabled = true;
        greenLight.enabled = false;
        //updates state to off
        state = false;

        //play audio for screen turning off
        PlayAudioHandler.Instance.playAudioSource("screenShutOff");

        //camera move cooldown
        yield return new WaitForSeconds(cameraCoolDown);

        //let OnMouseDown know that the camera is no longer moving
        moveCamera = false;

        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        if (state)
        {
            computerScreen.SetActive(true);
        }
        else
        {
            computerScreen.SetActive(false);
        }

        if (moveCamera)
        {
            relaySource.enabled = false;
        }
        else
        {
            relaySource.enabled = true;
        }
    }
}
