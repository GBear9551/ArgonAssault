using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{



    [Header("Enemy Stats")]

    [Tooltip("Health Points")]
        [SerializeField] int healthPoints = 3;

    [Tooltip("Score to give when hit by the player.")]
        [SerializeField] int scorePerHit = 15;



    [Header("SFX and VFX")]

    [Tooltip("Referring to the sound manager such that the program can play the audioclip through an external audio source.")]
        [SerializeField] int toDo = 0;

    [Tooltip("Sound, played on kill. Single clip used via audioSource")]
         AudioSource soundPlayedOnDeath = null;

    [Tooltip("Visual Explosion, played on particle collision.")]
        [SerializeField] GameObject explosion = null;

    [Tooltip("Visual Hit effect, played on particle collision.")]
        [SerializeField] GameObject hitEffect = null;




    [Header("Messages sent to other game objects")]

    [Tooltip("Score to send on particle collision.")]
        [SerializeField] int scoreGivenOnHit = 15;


    [Header("Whenever a particle is spawned, make it's parent the spawn manager.")]
        [SerializeField] GameObject parent = null;

    [Header("Score board")]
        [SerializeField] ScoreBoard scoreBoard = null;


    // Start is called before the first frame update
    void Start()
    {
        soundPlayedOnDeath = GetComponent<AudioSource>();

        parent = GameObject.FindWithTag("SpawnManager");

        scoreBoard = FindObjectOfType<ScoreBoard>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnParticleCollision(GameObject Other)
    {

        // Spawn the hit particle
        createHitParticle(Other);

        // Increase Score
            AddScore(); 

        // The enemy has been hit! Cause some damage and check for destruction!
            causeDamage();

        

    }        

    void createHitParticle(GameObject Other)
    {
        // Declare and initialize variables
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        ParticleSystem particleSystem = Other.GetComponent<ParticleSystem>();
        
        if (particleSystem != null) 
        {

            // Get the number of collision events, store them in the collision event list.
            ParticlePhysicsExtensions.GetCollisionEvents(particleSystem, gameObject, collisionEvents);

            foreach (ParticleCollisionEvent collisionEvent in collisionEvents)
            {
                // 

                // create the VFX
                GameObject vfx = Instantiate(hitEffect, collisionEvent.intersection, Quaternion.identity);

                // Store the particle effect in the spawn manager.
                vfx.transform.parent = parent.transform;
            }

        }



    }





    void causeDamage()
    {

        // Declare and initialize variables

        // Decrease the enemies health points
          healthPoints--;

        // If the health points are zero or less, then explode!
        if (healthPoints < 0)
        {

            // Instantiate and play explosion vfx
            // Function: CreateExplosionVFX()
              CreateExplosionVFX();

            // Play sound sfx, may need to call on a sound manager due to the object being destroyed.
            // Function: PlayExplosionSoundClip()
              PlayExplosionSoundClip();

            // Fall out of the sky
              // Turn off the animator
              // Turn on UseGravity located in the rigidbody.


            // if the enemy's health point is less than 0, then destory this game object.
              Invoke("DestroyShip", 1f);
        }
    }

    void DestroyShip()
    {
        Destroy(gameObject);
    }

    void AddScore()
    {

        // Declare and initialize variables

        // Check cache for score board
        if (scoreBoard != null)
        {
            // Add score to the score board cached
              scoreBoard.IncreaseScore(scorePerHit);
        }
    }


    void createHitVFX()
    {
        
    }

    void CreateExplosionVFX()
    {
        // Declare and initialize variables

            float minStartSize = ((transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3f);
            float maxStartSize = minStartSize * 3f;


        // Create the explosion via instantiate.
            GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
       
        // Get the particle system from the instantied game object.
            ParticleSystem explosionParticleSystem = explosionInstance.GetComponent<ParticleSystem>();

        // Stop the particle system just in case the particle system was set to play on awake.
            explosionParticleSystem.Stop();

        // Every particle system actually has a parameter module that we can modify.
            var mainModule = explosionParticleSystem.main;


        // Change the start size based on the enemy spaceship scale.
            mainModule.startSize = Random.Range(minStartSize, maxStartSize);


        // Play the particle system
            explosionParticleSystem.Play();

        // Maintain a clean heirarchy upon instiantiate

        // parent the explosion to the spawn manager's transform
          explosionParticleSystem.transform.parent = parent.transform;
        


    }

    void PlayExplosionSoundClip()
    {
        // Declare and initialize variables

        // Play the explosion sound if the sound is not already playing
        if (this != null && soundPlayedOnDeath != null)
        {
            if (!soundPlayedOnDeath.isPlaying)
            {

                // Play sound
                soundPlayedOnDeath.Play();

            }
        }


    }

    // 



}
