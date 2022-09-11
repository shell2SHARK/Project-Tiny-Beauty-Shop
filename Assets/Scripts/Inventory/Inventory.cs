using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Place here the character's Equip Item script:")]
    public EquipItens itensCharacterScript;
    [Header("Place here the character's Scriptable Object:")]
    public GameItens gameItensData;
    [Space(5)]
    [Header("Place here the slot icon prefab:")]
    public GameObject slotItemIcon;
    [Space(5)]
    [Header("Identify the slot parent gameobject:")]
    public GameObject slotParent;

    private void Start()
    {
        // draw the first slots of the item "hat" to the inventory
        CallHatsToSlot();
    }

    public void CallHatsToSlot()
    {
        // first - the function deletes the others itens slot
        DeleteOldSlots();

        // for each item we have on scriptable object, create the slots
        for (int i = 0; i < gameItensData.hats.Length; i++)
        {
            GameObject slotIcon = Instantiate(slotItemIcon);
            SetSlotValues slotScript = slotIcon.GetComponent<SetSlotValues>();
            Button slotBTN = slotIcon.GetComponent<Button>();

            // parent the prefab slot to the correct Canvas parent
            slotIcon.transform.SetParent(slotParent.transform, false);

            // add the equip item's script of the character to the slot
            slotScript.equipScript = itensCharacterScript;

            // give to the slot your id (image, item price and the item name)
            slotScript.imgComponent.sprite = gameItensData.hats[i].imgSpriteFRONT;
            slotScript.textComponent.text = "$" + gameItensData.hats[i].value.ToString();
            slotScript.nameItem = gameItensData.hats[i].nameItem;

            // assign a custom event to the slot button
            // if the player click him, a specified function inside the slot script is called to set this item
            slotBTN.onClick.AddListener(delegate { slotScript.SetHatToCharacter(); });
        }
    }

    public void CallShirtsToSlot()
    {
        DeleteOldSlots();

        for (int i = 0; i < gameItensData.body.Length; i++)
        {
            GameObject slotIcon = Instantiate(slotItemIcon);
            SetSlotValues slotScript = slotIcon.GetComponent<SetSlotValues>();
            Button slotBTN = slotIcon.GetComponent<Button>();

            slotIcon.transform.SetParent(slotParent.transform, false);

            slotScript.equipScript = itensCharacterScript;

            slotScript.imgComponent.sprite = gameItensData.body[i].imgSpriteFRONT;
            slotScript.textComponent.text = "$" + gameItensData.body[i].value.ToString();
            slotScript.nameItem = gameItensData.body[i].nameItem;

            slotBTN.onClick.AddListener(delegate { slotScript.SetShirtToCharacter(); });
        }
    }

    public void CallLegsToSlot()
    {
        DeleteOldSlots();

        for (int i = 0; i < gameItensData.legs.Length; i++)
        {
            GameObject slotIcon = Instantiate(slotItemIcon);
            SetSlotValues slotScript = slotIcon.GetComponent<SetSlotValues>();
            Button slotBTN = slotIcon.GetComponent<Button>();

            slotIcon.transform.SetParent(slotParent.transform, false);

            slotScript.equipScript = itensCharacterScript;

            slotScript.imgComponent.sprite = gameItensData.legs[i].imgSpriteFRONT;
            slotScript.textComponent.text = "$" + gameItensData.legs[i].value.ToString();
            slotScript.nameItem = gameItensData.legs[i].nameItem;

            slotBTN.onClick.AddListener(delegate { slotScript.SetLegToCharacter(); });
        }
    }

    public void CallArmsToSlot()
    {
        DeleteOldSlots();

        for (int i = 0; i < gameItensData.arms.Length; i++)
        {
            GameObject slotIcon = Instantiate(slotItemIcon);
            SetSlotValues slotScript = slotIcon.GetComponent<SetSlotValues>();
            Button slotBTN = slotIcon.GetComponent<Button>();

            slotIcon.transform.SetParent(slotParent.transform, false);

            slotScript.equipScript = itensCharacterScript;

            slotScript.imgComponent.sprite = gameItensData.arms[i].imgSpriteFRONT;
            slotScript.textComponent.text = "$" + gameItensData.arms[i].value.ToString();
            slotScript.nameItem = gameItensData.arms[i].nameItem;

            slotBTN.onClick.AddListener(delegate { slotScript.SetArmToCharacter(); });
        }
    }

    private void DeleteOldSlots()
    {
        for (int i = 0; i < slotParent.transform.childCount; i++)
        {
            Destroy(slotParent.transform.GetChild(i).gameObject);
        }
    }
}
