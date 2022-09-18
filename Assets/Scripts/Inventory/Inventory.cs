using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public class MyActualItens
    {
        public string[] hatName;
        public string[] shirtName;
        public string[] armName;
        public string[] legName;
    }

    public MyActualItens myItens;

    [Header("Place here the character's Equip Item script:")]
    public EquipItens itensCharacterScript;

    [Space(5)]
    [Header("Place here the character's Scriptable Object:")]
    public GameItens gameItensData;

    [Space(5)]
    [Header("Place here the slot icon prefab:")]
    public GameObject slotItemIcon;

    [Space(5)]
    [Header("Identify the slot parent gameobject:")]
    public GameObject slotParent;

    [Space(5)]
    [Header("Choose the zoom camera script:")]
    public CameraController mainCam;
    public Vector3 camOffset;

    // each method is called by the navigation buttons of inventory
    public void CallHatsToSlot()
    {
        SetItemOnSlot(myItens.hatName, "hats");        
    }

    public void CallShirtsToSlot()
    {
        SetItemOnSlot(myItens.shirtName, "body");
    }

    public void CallLegsToSlot()
    {
        SetItemOnSlot(myItens.legName, "legs");
    }

    public void CallArmsToSlot()
    {
        SetItemOnSlot(myItens.armName, "arms");
    }
    //----
    public void SetItemOnSlot(string[] inventItens, string ScriptableItens)
    {
        // first - the function deletes the others itens slot
        DeleteOldSlots();

        // search the actual itens inside the Scriptable Object
        GameItens.itemSpecs[] type = (GameItens.itemSpecs[])gameItensData.GetType().GetField(ScriptableItens).GetValue(gameItensData);

        // create a temporary array with the scriptable itens found
        string[] itemName = GetItemOnScriptable(type);

        for (int i = 0; i < type.Length; i++)
        {
            // compare if you have the same item inside on temporary array
            int idxItem = System.Array.IndexOf(itemName,inventItens[i]);

            // if you have, let's add on the slot
            if (idxItem > -1)
            {
                GameObject slotIcon = Instantiate(slotItemIcon);
                SetSlotValues slotScript = slotIcon.GetComponent<SetSlotValues>();
                Button slotBTN = slotIcon.GetComponent<Button>();

                // parent the prefab slot to the correct Canvas parent
                slotIcon.transform.SetParent(slotParent.transform, false);

                // add the equip item's script of the character to the slot
                slotScript.equipScript = itensCharacterScript;
                
                // give to the slot your id (image, item price and the item name)
                slotScript.imgComponent.sprite = type[idxItem].imgSpriteFRONT;
                slotScript.textComponent.text = "$" + type[idxItem].value.ToString();
                slotScript.nameItem = type[idxItem].nameItem;

                // assign a custom event to the slot button
                // if the player click him, a specified function inside the slot script is called to set this item
                switch (ScriptableItens)
                {
                    case "hats":
                        slotBTN.onClick.AddListener(delegate { slotScript.SetHatToCharacter(); });
                        break;
                    case "body":
                        slotBTN.onClick.AddListener(delegate { slotScript.SetShirtToCharacter(); });
                        break;
                    case "legs":
                        slotBTN.onClick.AddListener(delegate { slotScript.SetLegToCharacter(); });
                        break;
                    case "arms":
                        slotBTN.onClick.AddListener(delegate { slotScript.SetArmToCharacter(); });
                        break;
                }
            }
        }
    }

    public string[] GetItemOnScriptable(GameItens.itemSpecs[] scriptable)
    {
        // create a temporary array with the item received on param based on Scriptable Object
        string[] itensInSO = new string[scriptable.Length];

        for (int i = 0; i < scriptable.Length; i++)
        {
            itensInSO[i] = scriptable[i].nameItem;
        }

        return itensInSO;
    }

    public void ZoomInPlayerCamera()
    {
        mainCam.offset = camOffset;
        mainCam.targetNPC = itensCharacterScript.transform;
        mainCam.startZoom = true;
    }

    public void ZoomOutPlayerCamera()
    {
        mainCam.startZoom = false;
    }

    private void DeleteOldSlots()
    {
        for (int i = 0; i < slotParent.transform.childCount; i++)
        {
            Destroy(slotParent.transform.GetChild(i).gameObject);
        }
    }
}
