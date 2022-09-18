using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSlotValues : MonoBehaviour
{
    [Header("This variables change the slot img and price:")]
    public string nameItem;

    public Image imgComponent;
    public TMPro.TextMeshProUGUI textComponent;
    public EquipItens equipScript;

    // trigger the specified method when the slot with your correct item is clicked
    // the inventory script assign the event function to each slot when create him
    public void SetHatToCharacter()
    {
        equipScript.characterBody.SetHat(nameItem);
    }

    public void SetShirtToCharacter()
    {
        equipScript.characterBody.SetShirt(nameItem);
    }

    public void SetLegToCharacter()
    {
        equipScript.characterBody.SetLeg(nameItem);
    }

    public void SetArmToCharacter()
    {
        equipScript.characterBody.SetArm(nameItem);
    }
}
