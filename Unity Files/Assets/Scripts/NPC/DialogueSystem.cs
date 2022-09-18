using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueSystem : MonoBehaviour
{
    [System.Serializable]
    public class DialogueValues
    {
        // all stuff for the dialogue works
        public Sprite imgNPC;
        public string nameChar;
        public string[] dialogue;
        public bool shouldShowSomething;
        public bool shouldHideSomething;
        public bool lastDialogue;
        public GameObject[] ObjsToShow;
        public GameObject[] ObjsToHide;
        public GameObject[] hideImmediately;
        public GameObject[] showImmediately;
        public AudioClip talkAudio;        
    }

    // the all UI dialogue to show/hide
    [Header("UI Dialogue:")]
    public string NPCInteractionPhrase = "";
    public GameObject UIDialogue;
    public GameObject tinyBoxGameobject;     

    // sine movement for UI
    [Space(5)]
    [Header("Sine movement data:")]
    public float frequency = 5;
    public float magnitude = 5;
    public float offset = 0;
    private Vector3 startPos;

    // talk button
    [Space(5)]
    [Header("Proceed button:")]
    public Button nextButton;

    // the texts of each dialogue
    [Space(5)]
    [Header("All dialogues class:")]
    public DialogueValues[] dialogues;

    // text component
    [Space(5)]
    [Header("All dialogues class and name text:")]
    public TMPro.TextMeshProUGUI dialText;
    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI tinyBoxText;

    // npc image and gameobject ingame
    [Space(5)]
    [Header("NPC Expression / GameObject:")]
    public Image NPCUIImg;
    public Transform NPCGameSprite;

    [Space(5)]
    [Header("Offset camera values:")]
    public Vector3 offsetCam;

    // identify which dialogue class should be selected
    [Space(5)]
    [Header("Dialogue class position:")]
    public int dialoguePos = 0;

    // play the first audio interaction
    [Space(5)]
    [Header("First audio to play when interact with the character:")]
    public AudioClip firstTalkAudio;

    // trigger specified methods of other scripts here
    [Space(5)]
    [Header("Custom events to trigger:")]
    public UnityEvent eventsTrigger;

    [Space(5)]
    [Header("All sprites to highlight when interact:")]
    public SpriteRenderer[] spritesHighlight;

    [Space(5)]
    [Header("Set NPC style here:")]
    public SpriteRenderer eyeSprite;
    public Sprite hairStyle;
    public Sprite shirtStyle;
    public Sprite armLeftStyle;
    public Sprite armRightStyle;
    public Sprite leftLegStyle;
    public Sprite rightLegStyle;
    public Color skinColor;
    public Color eyeColor;

    [Space(5)]
    [Header("Can interact:")]
    public bool canPlayerInteract = false;

    // the actual text marker
    private int actualDialogue = 0;

    // to material value weight start change values
    private bool startSineMaterialValue = false;

    // get the audiosource component inside gameobject
    private AudioSource audioSrc;

    // receives the main camera to zoom in on NPC 
    private CameraController cameraMainGame;

    // receives the player gameobject to call pause method
    public MainCharacter playerOBJ;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        cameraMainGame = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();

        // start the first dialogue when active
        nameText.text = dialogues[dialoguePos].nameChar;
        dialText.text = dialogues[dialoguePos].dialogue[actualDialogue];
        NPCUIImg.sprite = dialogues[dialoguePos].imgNPC;

        // get initial position of tiny box to animate him
        startPos = tinyBoxGameobject.transform.position;
        tinyBoxText.text = NPCInteractionPhrase;

        // set the NPC styles using spritesHighlights array order                       
        spritesHighlight[8].sprite = hairStyle;

        // change colors of eye and skin
        eyeSprite.color = eyeColor;
        spritesHighlight[1].color = skinColor;
        spritesHighlight[3].color = skinColor;
        spritesHighlight[5].color = skinColor;

        // apply shirt sprite if exists something
        if(shirtStyle != null)
        {
            spritesHighlight[0].sprite = shirtStyle;
        }

        // apply sprite of arms when set something, else only change skin color
        if(armLeftStyle == null && armRightStyle == null)
        {
            spritesHighlight[2].color = skinColor;
            spritesHighlight[4].color = skinColor;
        }
        else
        {
            spritesHighlight[2].sprite = armLeftStyle;
            spritesHighlight[4].sprite = armRightStyle;
        }

        // apply sprite of legs when set something, else only change skin color
        if (leftLegStyle == null && rightLegStyle == null)
        {
            spritesHighlight[6].color = skinColor;
            spritesHighlight[7].color = skinColor;
        }
        else
        {
            spritesHighlight[6].sprite = leftLegStyle;
            spritesHighlight[7].sprite = rightLegStyle;
        }
    }

    private void Update()
    {
        // animate the tiny box
        TinyTalkBoxAnimation();

        // if player stay close of a npc and press the button, the interaction starts
        if (canPlayerInteract)
        {
            if (Input.GetKeyDown("x") && !playerOBJ.isPaused)
            {   
                // call all necessary in game methods and hide tiny box
                eventsTrigger.Invoke();                
                tinyBoxGameobject.SetActive(false);

                // call zoom in camera method
                cameraMainGame.targetNPC = NPCGameSprite;
                cameraMainGame.offset = offsetCam;
                cameraMainGame.startZoom = true;

                // set interaction to false
                canPlayerInteract = false;

                // disable outlines
                foreach (SpriteRenderer mats in spritesHighlight)
                {
                    mats.material.SetFloat("_OutlineEnabled", 0);
                }
            }            
        }
    }

    private void FixedUpdate()
    {
        // start sine outline material
        if (startSineMaterialValue)
        {
            LerpMaterialValue();
        }
    }

    // method assigned to the nextButton
    public void NextDialogue()
    { 
        // while the talks don't finish, proceed to the next
        if (actualDialogue < dialogues[dialoguePos].dialogue.Length - 1)
        {
            nextButton.gameObject.SetActive(true);
            actualDialogue++;            
            dialText.text = dialogues[dialoguePos].dialogue[actualDialogue];                       
        }
        else 
        {
            // on the final dialog these itens will be revealed
            if (dialogues[dialoguePos].shouldShowSomething)
            {
                foreach(GameObject obs in dialogues[dialoguePos].ObjsToShow)
                {
                    obs.SetActive(true);
                }
            }
            // on the final dialog these itens will be hide
            if (dialogues[dialoguePos].shouldHideSomething)
            {
                foreach (GameObject obs in dialogues[dialoguePos].ObjsToHide)
                {
                    obs.SetActive(false);
                }
            }

            // if the player is in last dialogue, the character can move again
            if (dialogues[dialoguePos].lastDialogue)
            {
                playerOBJ.PausedGame(false);
                cameraMainGame.startZoom = false;
                canPlayerInteract = true;
            }

            actualDialogue = 0;
            nextButton.gameObject.SetActive(false);            
        }
    }

    public void ChangeDialogue(int dialPos)
    {
        dialoguePos = dialPos;
        nextButton.gameObject.SetActive(true);

        // set the new text, image and audio to play in the next talk
        nameText.text = dialogues[dialoguePos].nameChar;
        dialText.text = dialogues[dialoguePos].dialogue[actualDialogue];
        NPCUIImg.sprite = dialogues[dialoguePos].imgNPC;

        PlayAudioDialogue();
        HideItemImmediately();
    }

    public void ShowUIDialogue()
    {
        // start a new dialog here
        UIDialogue.SetActive(true);
        ChangeDialogue(0);
        HideItemImmediately();
    }

    public void HideItemImmediately()
    {
        // don't wait to the dialog ends, hide all the objects of the class
        foreach (GameObject obs in dialogues[dialoguePos].hideImmediately)
        {
            obs.SetActive(false);
        }
    }

    public void ShowItemImmediately()
    {
        // don't wait to the dialog ends, show all the objects of the class
        foreach (GameObject obs in dialogues[dialoguePos].showImmediately)
        {
            obs.SetActive(true);
        }
    }

    public void PlayAudioDialogue()
    {
        if(dialogues[dialoguePos].talkAudio != null && actualDialogue == 0)
        {
            audioSrc.clip = dialogues[dialoguePos].talkAudio;
            audioSrc.Play(0);
        }        
    }

    public void PlayFirstAudioDialogue()
    {
        audioSrc.clip = firstTalkAudio;
        audioSrc.Play(0);
    }

    public void TinyTalkBoxAnimation()
    {
        tinyBoxGameobject.transform.position = startPos + transform.up * Mathf.Sin(Time.time * frequency + offset) * magnitude;
    }

    public void LerpMaterialValue()
    {
        // change the weight param of each outline material with ping pong style
        float lerp = Mathf.PingPong(Time.time, 0.5f) / 0.5f;

        foreach (SpriteRenderer mats in spritesHighlight)
        {
            mats.material.SetFloat("_Weight", Mathf.Lerp(0, 1, lerp));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // collision with the player
        if (collision.gameObject.tag == "Player")
        {
            canPlayerInteract = true;
            tinyBoxGameobject.SetActive(true);            
            playerOBJ = collision.gameObject.GetComponent<MainCharacter>();

            foreach (SpriteRenderer mats in spritesHighlight)
            {
                // enable outline mode
                mats.material.SetFloat("_OutlineEnabled", 1);
            }

            startSineMaterialValue = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // collision with the player
        if (collision.gameObject.tag == "Player")
        {
            canPlayerInteract = false;
            tinyBoxGameobject.SetActive(false);
            
            foreach (SpriteRenderer mats in spritesHighlight)
            {
                // disable outline mode
                mats.material.SetFloat("_OutlineEnabled", 0);
            }

            startSineMaterialValue = false;
        }
    }
}
