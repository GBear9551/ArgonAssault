using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Player : MonoBehaviour
{


    [Header("Player Parameters")]
   
    [Tooltip("Can receive player input is a boolean value allowing this script to receive player input.")]
        [SerializeField] bool canReceivePlayerInput = true;
    
    [SerializeField] float movementControl = 1f;
    [SerializeField] float xRange = 3f;
    [SerializeField] float yRange = 3f;
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float positionRollFactor = -2f;
    [SerializeField] float controllerPitchFactor = -10f;
    [SerializeField] float controllerRollFactor = -10f;
    [SerializeField] float controllerYawFactor = 5f;


    [Tooltip("Reload scene time delay determines how long to wait after the player crashes, once the wait is complete, reload the level.")]
        [SerializeField] float loadDelay = 1f;

    // Cache
    [Header("Cached Data")]


    // ChatGPT and I working together on this disable animator bit.
    [Tooltip("Store the player rig so we can get the animator.")]
        [SerializeField] GameObject playerRig = null;

    [Tooltip("Cache the player rig here.")]
        [SerializeField] Rigidbody rb = null;


    [Tooltip("Grab the animator and disable it on crash.")]
        [SerializeField] Animator anima = null;


    [Tooltip("Explosion VFX played on crash.")]
        [SerializeField] GameObject explosionVFX;


    [Header("Lasers")]
    [SerializeField] GameObject[] lasers;

    float yThrow = 0f;
    float xThrow = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
        canReceivePlayerInput = true;
        playerRig = transform.parent.gameObject;
        rb = GetComponent<Rigidbody>();
        


    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();


        // Get 


        // Processing the player's destruction is handled in the OnCollisionEnter and CrashSequence functions.
            // Ordered functions : OnCollisionEnter() -> CrashSequence()


    }


    void ProcessInput()
    {

        if(canReceivePlayerInput == true)
        {
            ProcessTranslation();
            ProcessRotation();
            ProcessShooting();
        }

    }


    void OnCollisionEnter(Collision other)
    {

        // Declare and initialize variables

        // Run the crash sequence.
        // Function: CrashSequence()
            CrashSequence();
    }


    void CrashSequence()
    {
        Debug.Log("Crashing.");
        // Declare and initialize variables

        // Disable Input
        canReceivePlayerInput = false;

        // Play explosion effect
        Instantiate(explosionVFX, transform.position, Quaternion.identity);

        // Play crash SFX

        // Disable animator
        anima = playerRig.GetComponent<Animator>();
        anima.enabled = false;
        rb.useGravity = true;


        // Invoke reload level with a delay
        // Funciton: ReloadLevel()
            Invoke("ReloadLevel", loadDelay);


    }

    void ReloadLevel()
    {
        int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneBuildIndex);
    }



    void ActiveLasers()
    {

        foreach (GameObject laser in lasers)
        {
            
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = true;
        }


    }

    void DeactiveLasers()
    {

        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = false;
        }

    }

    void ProcessShooting()
    {
        if(Input.GetButton("Fire1"))
        {
            ActiveLasers();
        }
        else
        {
            DeactiveLasers();
        }

    }


    void ProcessRotation()
    {


        float pitchDueToPlayerController = yThrow * controllerPitchFactor;
        float pitchDueToPlayer_Y_Position = transform.localPosition.y * positionPitchFactor;
        float pitch = pitchDueToPlayer_Y_Position +  pitchDueToPlayerController;



        float rollDueToPlayerController = xThrow * controllerRollFactor;
        float rollDueToPlayer_X_Position = transform.localPosition.x * positionRollFactor;
        float roll = rollDueToPlayerController + rollDueToPlayer_X_Position;

        float yawDueToPlayerController = rollDueToPlayerController * -.15f;
        float yawDueToPlayer_X_Position = rollDueToPlayer_X_Position * -0.5f * controllerYawFactor;
        float yaw = yawDueToPlayerController + yawDueToPlayer_X_Position;
        
            
            
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffSet = (xThrow * movementControl * Time.deltaTime);
        float yOffSet = (yThrow * movementControl * Time.deltaTime);

        float new_x_position = transform.localPosition.x + xOffSet;
        new_x_position = Mathf.Clamp(new_x_position, -xRange, xRange);

        float new_y_position = transform.localPosition.y + yOffSet;
        new_y_position = Mathf.Clamp(new_y_position, -yRange, yRange);




        transform.localPosition = new Vector3(new_x_position, new_y_position, transform.localPosition.z);
    }


}
