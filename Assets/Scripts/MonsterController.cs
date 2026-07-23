using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MonsterController : MonoBehaviour
{
    public float speed = 3f; // movement speed

    //bounds for where it changes surfaces
    public float floorY = 0f;
    public float ceilingY = 4.1f;
    public float leftWallX = 17.3f;
    public float rightWallX = 31f;

    //X and Y levels for when it is on that surface
    public float onFloorY = 0f;
    public float onCeilingY = 5.1f;
    public float onLeftWallX = 16.4f;
    public float onRightWallX = 32.1f;

    public float targetZ = 36f; //how far away in the z its targe location is

    public float distanceToSwitchSurface = 3f; //how far ahead it aims to go onto another surface
    private Vector3 startingPOS;
    public Animator animator;

    public Vector3 currentTarget;
    public string desiredSurface;
    private float floatTolerance = 0.1f;

    public float gameOverZ = 62f;

    //these are for rotating based on surface
    private Quaternion LEFT_WALL_ROTATION = Quaternion.Euler(0f, 180f, 90f);
    private Quaternion RIGHT_WALL_ROTATION = Quaternion.Euler(0f, 180f, -90f);
    private Quaternion CEILING_ROTATION = Quaternion.Euler(0f, 180f, 180f);
    private Quaternion FLOOR_ROTATION = Quaternion.Euler(0f, 180f, 0f);

    private AudioSource footsteps;

    public static MonsterController Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        footsteps = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTarget = pickTarget();
        startingPOS = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverCheck())
        {
            GameOver.gameOverReason = "Monster";
            SceneManager.LoadScene("GameOver");
        }

        handleAnimations();

        moveIfLightsOn();
        
    }

    private void moveIfLightsOn()
    {
        if (ComputerClick.Instance.getState())
        {
            footsteps.UnPause();

            if (desiredSurface == "floor")
            {
                if (isOnSurface(desiredSurface))
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
                }
                else if (isOnSurface("right"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, changeSurface(desiredSurface), speed * Time.deltaTime);
                    transitionPositionCheck("right", desiredSurface);
                }
                else if (isOnSurface("left"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, changeSurface(desiredSurface), speed * Time.deltaTime);
                    transitionPositionCheck("left", desiredSurface);
                }
                else if (isOnSurface("ceiling"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, changeSurface("left"), speed * Time.deltaTime);
                    transitionPositionCheck("ceiling", "left");
                }
            }
            else if (desiredSurface == "ceiling")
            {
                if (isOnSurface(desiredSurface))
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
                }
                else if (isOnSurface("right"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, changeSurface(desiredSurface), speed * Time.deltaTime);
                    transitionPositionCheck("right", desiredSurface);
                }
                else if (isOnSurface("left"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, changeSurface(desiredSurface), speed * Time.deltaTime);
                    transitionPositionCheck("left", desiredSurface);
                }
                else if (isOnSurface("floor"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, changeSurface("right"), speed * Time.deltaTime);
                    transitionPositionCheck("floor", "right");
                }
            }
            else if (desiredSurface == "right")
            {
                if (isOnSurface(desiredSurface))
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
                }
                else if (isOnSurface("floor"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, changeSurface(desiredSurface), speed * Time.deltaTime);
                    transitionPositionCheck("floor", desiredSurface);
                }
                else if (isOnSurface("ceiling"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, changeSurface(desiredSurface), speed * Time.deltaTime);
                    transitionPositionCheck("ceiling", desiredSurface);
                }
                else if (isOnSurface("left"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, changeSurface("floor"), speed * Time.deltaTime);
                    transitionPositionCheck("left", "floor");
                }
            }
            else if (desiredSurface == "left")
            {
                if (isOnSurface(desiredSurface))
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
                }
                else if (isOnSurface("floor"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, changeSurface(desiredSurface), speed * Time.deltaTime);
                    transitionPositionCheck("floor", desiredSurface);
                }
                else if (isOnSurface("ceiling"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, changeSurface(desiredSurface), speed * Time.deltaTime);
                    transitionPositionCheck("ceiling", desiredSurface);
                }
                else if (isOnSurface("right"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, changeSurface("floor"), speed * Time.deltaTime);
                    transitionPositionCheck("right", "floor");
                }
            }
        }
        //lights off
        else
        {
            footsteps.Pause();
        }
    }

    private void transitionPositionCheck(string fromSurface, string toSurface)
    {
        float desiredX = 0f;
        float desiredY = 0f;
        if (fromSurface == "floor")
        {
            desiredY = floorY;
        }
        else if (fromSurface == "ceiling")
        {
            desiredY = ceilingY;
        }
        else if (fromSurface == "right")
        {
            desiredX = rightWallX;
        }
        else if (fromSurface == "left")
        {
            desiredX = leftWallX;
        }

        if (toSurface == "floor")
        {
            if (Mathf.Abs(transform.position.y - floorY) <= floatTolerance)
            {
                transform.position = new Vector3(desiredX, onFloorY, transform.position.z);
            }
        }
        else if (toSurface == "ceiling")
        {
            if (Mathf.Abs(transform.position.y - ceilingY) <= floatTolerance)
            {
                transform.position = new Vector3(desiredX, onCeilingY, transform.position.z);
            }
        }
        else if (toSurface == "right")
        {
            if (Mathf.Abs(transform.position.x - rightWallX) <= floatTolerance)
            {
                transform.position = new Vector3(onRightWallX, desiredY, transform.position.z);
            }
        }
        else if (toSurface == "left")
        {
            if (Mathf.Abs(transform.position.x - leftWallX) <= floatTolerance)
            {
                transform.position = new Vector3(onLeftWallX, desiredY, transform.position.z);
            }
        }
    }
    private void handleAnimations()
    {
        if (ComputerClick.Instance.getState())
        {
            animator.speed = 1;
        }
        else
        {
            animator.speed = 0;
        }

        if (isOnSurface("floor"))
        {
            transform.rotation = FLOOR_ROTATION;
        }
        if (isOnSurface("ceiling"))
        {
            transform.rotation = CEILING_ROTATION;
        }
        if (isOnSurface("right"))
        {
            transform.rotation = RIGHT_WALL_ROTATION;
        }
        if (isOnSurface("left"))
        {
            transform.rotation = LEFT_WALL_ROTATION;
        }
    }
    public bool gameOverCheck()
    {
        return transform.position.z <= gameOverZ;
    }
    private bool isOnSurface(string surface)
    {
        if (surface == "floor")
        {
            return Mathf.Abs(transform.position.y - onFloorY) <= floatTolerance;
        }
        else if (surface == "ceiling")
        {
            return Mathf.Abs(transform.position.y - onCeilingY) <= floatTolerance;
        }
        else if (surface == "right")
        {
            return Mathf.Abs(transform.position.x - onRightWallX) <= floatTolerance;
        }
        else
        {
            return Mathf.Abs(transform.position.x - onLeftWallX) <= floatTolerance;
        }
    }

    private Vector3 changeSurface(string surface)
    {
        if(surface == "floor")
        {
            return new Vector3(transform.position.x, floorY, transform.position.z - distanceToSwitchSurface);
        }
        else if(surface == "ceiling")
        {
            return new Vector3(transform.position.x, ceilingY, transform.position.z - distanceToSwitchSurface);
        }
        else if (surface == "right")
        {
            return new Vector3(rightWallX, transform.position.y, transform.position.z - distanceToSwitchSurface);
        }
        else
        {
            return new Vector3(leftWallX, transform.position.y, transform.position.z - distanceToSwitchSurface);
        }
    }
    public Vector3 pickTarget()
    {
        int rng = Random.Range(1, 5);
        float rightWallFairSpotOnFloor = 26.2f;
        float floorLevelFairSpotOnRightWall = 2f;

        //option1: floor
        if (rng == 1)
        {
            float desiredX = Random.Range(leftWallX, rightWallFairSpotOnFloor);
            desiredSurface = "floor";
            return new Vector3(desiredX, onFloorY, transform.position.z - targetZ);
        }
        //option2: left wall
        else if (rng == 2)
        {
            float desiredY = Random.Range(floorY, ceilingY);
            desiredSurface = "left";
            return new Vector3(onLeftWallX, desiredY, transform.position.z - targetZ);
        }
        //option3: right wall
        else if (rng == 3)
        {
            float desiredY = Random.Range(floorLevelFairSpotOnRightWall, ceilingY);
            desiredSurface = "right";
            return new Vector3(onRightWallX, desiredY, transform.position.z - targetZ);
        }
        //option4: ceiling
        else
        {
            float desiredX = Random.Range(leftWallX, rightWallX);
            desiredSurface = "ceiling";
            return new Vector3(desiredX, onCeilingY, transform.position.z - targetZ);
        }
    }

    public void shot(float muzzleFlashSeconds)
    {
        StartCoroutine(shotSequence(muzzleFlashSeconds));
    }

    IEnumerator shotSequence(float wait)
    {
        animator.speed = 3f;
        PlayAudioHandler.Instance.playRandomAudio("monsterHit", 3);
        yield return new WaitForSeconds(wait);
        transform.position = new Vector3(transform.position.x, transform.position.y, 95f);
        currentTarget = pickTarget();
    }
}
