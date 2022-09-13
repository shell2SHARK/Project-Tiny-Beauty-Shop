using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [System.Serializable]
    public class DialogueValues
    {
        // all stuff for the dialogue works
        public Sprite imgNPC;
        public string[] dialogue;
        public bool shouldShowSomething;
        public bool shouldHideSomething;
        public GameObject[] ObjsToShow;
        public GameObject[] ObjsToHide;
        public GameObject[] hideImmediately;
        public GameObject[] showImmediately;
    }

    // the all UI dialogue to show/hide
    [Header("UI Dialogue")]
    public GameObject UIDialogue;

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
    [Header("All dialogues class:")]
    public TMPro.TextMeshProUGUI dialText;

    // npc image
    [Space(5)]
    [Header("NPC Expression:")]
    public Image NPCImg;

    // identify which dialogue class should be selected
    [Space(5)]
    [Header("Dialogue class position:")]
    public int dialoguePos = 0;

    //the actual text marker
    private int actualDialogue = 0;

    private void Start()
    {
        dialText.text = dialogues[dialoguePos].dialogue[actualDialogue];
    }

    // method assigned to the nextButton
    public void NextDialogue()
    {       
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

            if (dialogues[dialoguePos].shouldHideSomething)
            {
                foreach (GameObject obs in dialogues[dialoguePos].ObjsToHide)
                {
                    obs.SetActive(false);
                }
            }

            actualDialogue = 0;
            nextButton.gameObject.SetActive(false);
        }
    }

    public void ChangeDialogue(int dialPos)
    {
        dialoguePos = dialPos;
        nextButton.gameObject.SetActive(true);
        dialText.text = dialogues[dialoguePos].dialogue[actualDialogue];
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

}