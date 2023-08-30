using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    

    void Awake()
    {

        // Declare and initialize variables
        int numOfMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;


        // Singleton pattern
        if (numOfMusicPlayers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }

}
