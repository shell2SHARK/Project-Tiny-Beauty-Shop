using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameController : MonoBehaviour
{
    // start unfade the screen with the post process effect
    [Header("Place the post process camera here:")]
    public PostProcessVolume volumePost;
    public float speedFocal = 1;
    public bool startFocalDepth = false;

    // get the effect atributte
    private DepthOfField depthValues;

    private void Start()
    {
        // identify the specific atributte value we want inside the post processing component
        volumePost.profile.TryGetSettings(out depthValues);
    }

    private void Update()
    {
        // if true, start unfade
        if (startFocalDepth)
        {
            StartGame();
        }
        
    }

    public void ExitGame()
    {
        // exit the game
        Application.Quit();
    }

    public void StartGame()
    {
        // while the focus value is more than zero, execute the method
        startFocalDepth = true;
        depthValues.focalLength.value -= speedFocal;

        if(depthValues.focalLength.value <= 0)
        {
            startFocalDepth = false;
        }
    }
}
