using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopScript : MonoBehaviour
{
    [Header("Place here the slot icon prefab:")]
    public GameObject slotItemIcon;
    [Space(5)]
    [Header("Identify the slot parent gameobject:")]
    public GameObject slotParent;
    [Space(5)]
    [Header("Place here the character's Scriptable Object:")]
    public GameItens gameItensData;
    [Space(5)]
    [Header("Inventory Script Character:")]
    public Inventory inventoryScript;
    // here the index of each dialogue talk was inserted
    // the controller objects visibility is controlled by him too
    [Space(5)]
    [Header("Index of all dialogues on dialogue UI:")]
    public int idxOfBuyQuestion = 0;
    public int idxOfSellQuestion = 0;
    public int idxOfFinishBuyQuestion = 0;
    public int idxOfFinishSellQuestion = 0;
    public int idxOfDontHaveMoneyQuestion = 0;
    // all the necessary UI itens to show info to the player
    [Space(5)]
    [Header("Confirm Button UI:")]
    public DialogueSystem dialSystem;
    public TMPro.TextMeshProUGUI textConfirm;
    public Button yes;
    public Image shopBG;
    public Image shopButtonBG;
    [Space(5)]
    [Header("Buy/Sell state:")]
    public bool isBuying = true;

    // the inventory opening button receives the function BuyItem by default as event trigger
    public void BuyItem()
    {
        isBuying = true;
        shopBG.color = Color.red;
        shopButtonBG.color = Color.red;
        ShowHatItens(); // first itens to show on the shop
    }

    public void SellItem()
    {
        isBuying = false;
        shopBG.color = Color.blue;
        shopButtonBG.color = Color.blue;
        ShowHatItens(); //first itens to show on the shop
    }

    // each function here is connected inside a button on navigator shop buttons
    public void ShowHatItens()
    {
        if (isBuying)
        {
            // if buy -> call the set function using the actual class item to setup the values
            SetItensToBuy(inventoryScript.myItens.hatName, "hats");
        }
        else
        {
            // if not -> the sell function is call using the actual class item to setup the values
            SetItensToSell(inventoryScript.myItens.hatName, "hats");
        }           
    }

    public void ShowShirtItens()
    {       
        if (isBuying)
        {
            SetItensToBuy(inventoryScript.myItens.shirtName, "body");
        }
        else
        {
            SetItensToSell(inventoryScript.myItens.shirtName, "body");
        }
    }

    public void ShowLegItens()
    {
        if (isBuying)
        {
            SetItensToBuy(inventoryScript.myItens.legName, "legs");
        }
        else
        {
            SetItensToSell(inventoryScript.myItens.legName, "legs");
        }
    }

    public void ShowArmItens()
    {
        if (isBuying)
        {
            SetItensToBuy(inventoryScript.myItens.armName, "arms");            
        }
        else
        {
            SetItensToSell(inventoryScript.myItens.armName, "arms");
        }
    }
    //----

    public void SetItensToBuy(string[] itemToSort, string ScriptableItens)
    {        
        // first - the function deletes the others itens slot
        DeleteOldSlots();

        // search the actual itens inside the Scriptable Object
        GameItens.itemSpecs[] type = (GameItens.itemSpecs[])gameItensData.GetType().GetField(ScriptableItens).GetValue(gameItensData);

        // create a temporary array with the itens the player have
        string[] itemName = SortItensToBuy(type, itemToSort);
        print("here chama sort " + itemToSort[0] + itemToSort[1] + itemToSort[2] + itemToSort[3]);
        print("here chama " + itemName[0] + itemName[1] + itemName[2] + itemName[3]);

        for (int i = 0; i < type.Length; i++)
        {
            // if you don't have, let's add on the slot
            if (itemName[i] != null)
            {
                GameObject slotIcon = Instantiate(slotItemIcon);
                SetSlotValues slotScript = slotIcon.GetComponent<SetSlotValues>();
                Button slotBTN = slotIcon.GetComponent<Button>();

                // parent the prefab slot to the correct Canvas parent
                slotIcon.transform.SetParent(slotParent.transform, false);

                // give to the slot your id (image, item price and the item name)
                float priceItem = type[i].value;
                slotScript.imgComponent.sprite = type[i].imgSpriteFRONT;
                slotScript.textComponent.text = "$" + priceItem.ToString();
                slotScript.nameItem = type[i].nameItem;

                // assign a custom event to the slot button
                // if the player click him, a specified function inside the slot script is called to set this item
                switch (ScriptableItens)
                {
                    case "hats":
                        slotBTN.onClick.AddListener(delegate { dialSystem.ChangeDialogue(idxOfBuyQuestion); });
                        slotBTN.onClick.AddListener(delegate { dialSystem.ShowItemImmediately(); });
                        slotBTN.onClick.AddListener(delegate { ConfirmBuyItem(slotScript.nameItem, itemName, "hats", priceItem); });                                                                       
                        break;
                    case "body":
                        slotBTN.onClick.AddListener(delegate { dialSystem.ChangeDialogue(idxOfBuyQuestion); });
                        slotBTN.onClick.AddListener(delegate { dialSystem.ShowItemImmediately(); });
                        slotBTN.onClick.AddListener(delegate { ConfirmBuyItem(slotScript.nameItem, itemName, "body", priceItem); });
                        break;
                    case "legs":
                        slotBTN.onClick.AddListener(delegate { dialSystem.ChangeDialogue(idxOfBuyQuestion); });
                        slotBTN.onClick.AddListener(delegate { dialSystem.ShowItemImmediately(); });
                        slotBTN.onClick.AddListener(delegate { ConfirmBuyItem(slotScript.nameItem, itemName, "legs", priceItem); });
                        break;
                    case "arms":
                        slotBTN.onClick.AddListener(delegate { dialSystem.ChangeDialogue(idxOfBuyQuestion); });
                        slotBTN.onClick.AddListener(delegate { dialSystem.ShowItemImmediately(); });
                        slotBTN.onClick.AddListener(delegate { ConfirmBuyItem(slotScript.nameItem, itemName, "arms", priceItem); });
                        break;
                }               
            }
        }
    }

    public void SetItensToSell(string[] itemToSort, string ScriptableItens)
    {
        // first - the function deletes the others itens slot
        DeleteOldSlots();

        // search the actual itens inside the Scriptable Object
        GameItens.itemSpecs[] type = (GameItens.itemSpecs[])gameItensData.GetType().GetField(ScriptableItens).GetValue(gameItensData);

        // create a temporary array with the itens the player have
        string[] itemName = SortItensToSell(type);

        for (int i = 0; i < type.Length; i++)
        {
            int idxItem = System.Array.IndexOf(itemName, itemToSort[i]);

            // if you don't have, let's add on the slot
            if (idxItem > -1)
            {
                GameObject slotIcon = Instantiate(slotItemIcon);
                SetSlotValues slotScript = slotIcon.GetComponent<SetSlotValues>();
                Button slotBTN = slotIcon.GetComponent<Button>();

                // parent the prefab slot to the correct Canvas parent
                slotIcon.transform.SetParent(slotParent.transform, false);

                // give to the slot your id (image, item price and the item name)
                float priceItem = type[i].value;
                slotScript.imgComponent.sprite = type[idxItem].imgSpriteFRONT;
                slotScript.textComponent.text = "$" + type[idxItem].value.ToString();
                slotScript.nameItem = type[idxItem].nameItem;

                // assign a custom event to the slot button
                // if the player click him, a specified function inside the slot script is called to set this item
                switch (ScriptableItens)
                {
                    case "hats":
                        slotBTN.onClick.AddListener(delegate { dialSystem.ChangeDialogue(idxOfSellQuestion); });
                        slotBTN.onClick.AddListener(delegate { dialSystem.ShowItemImmediately(); });
                        slotBTN.onClick.AddListener(delegate { ConfirmSellItem(slotScript.nameItem, itemName, "hats", priceItem); });
                        //slotBTN.onClick.AddListener(delegate { DropItem(slotScript.nameItem, itemName, "hats", priceItem); });
                        break;
                    case "body":
                        slotBTN.onClick.AddListener(delegate { dialSystem.ChangeDialogue(idxOfSellQuestion); });
                        slotBTN.onClick.AddListener(delegate { dialSystem.ShowItemImmediately(); });
                        slotBTN.onClick.AddListener(delegate { ConfirmSellItem(slotScript.nameItem, itemName, "body", priceItem); });
                        break;
                    case "legs":
                        slotBTN.onClick.AddListener(delegate { dialSystem.ChangeDialogue(idxOfSellQuestion); });
                        slotBTN.onClick.AddListener(delegate { dialSystem.ShowItemImmediately(); });
                        slotBTN.onClick.AddListener(delegate { ConfirmSellItem(slotScript.nameItem, itemName, "legs", priceItem); });
                        break;
                    case "arms":
                        slotBTN.onClick.AddListener(delegate { dialSystem.ChangeDialogue(idxOfSellQuestion); });
                        slotBTN.onClick.AddListener(delegate { dialSystem.ShowItemImmediately(); });
                        slotBTN.onClick.AddListener(delegate { ConfirmSellItem(slotScript.nameItem, itemName, "arms", priceItem); });
                        break;
                }
            }
        }
    }

    private string[] SortItensToBuy(GameItens.itemSpecs[] scriptableOBJ, string[] actualItem)
    {
        // sort a array based with the itens the player actually have
        string[] itensToShop = new string[scriptableOBJ.Length];

        for (int i = 0; i < scriptableOBJ.Length; i++)
        {
            itensToShop[i] = scriptableOBJ[i].nameItem;
        }

        for (int i = 0; i < scriptableOBJ.Length; i++)
        {
            int idxItem = System.Array.IndexOf(itensToShop, actualItem[i]);

            if (idxItem > -1)
            {
                itensToShop[idxItem] = null;
            }
        }

        return itensToShop;
    }

    public string[] SortItensToSell(GameItens.itemSpecs[] scriptable)
    {
        // create a temporary array with the item received on param based on Scriptable Object
        string[] itensInSO = new string[scriptable.Length];

        for (int i = 0; i < scriptable.Length; i++)
        {
            itensInSO[i] = scriptable[i].nameItem;
        }

        return itensInSO;
    }

    private void DeleteOldSlots()
    {
        // get the parent slot child and destroy him
        for (int i = 0; i < slotParent.transform.childCount; i++)
        {
            Destroy(slotParent.transform.GetChild(i).gameObject);
        }
    }

    public void GetItem(string itemName,string[] allItens, string itemType,float price)
    {
        // search and give the item from the inventory
        int idx = System.Array.IndexOf(allItens, itemName);

        switch (itemType)
        {
            case "hats":
                inventoryScript.myItens.hatName[idx] = itemName;
                inventoryScript.CallHatsToSlot();
                ShowHatItens();
                break;
            case "body":
                inventoryScript.myItens.shirtName[idx] = itemName;
                inventoryScript.CallShirtsToSlot();
                ShowShirtItens();
                break;
            case "legs":
                inventoryScript.myItens.legName[idx] = itemName;
                inventoryScript.CallLegsToSlot();
                ShowLegItens();
                break;
            case "arms":
                inventoryScript.myItens.armName[idx] = itemName;
                inventoryScript.CallArmsToSlot();
                ShowArmItens();                
                break;
        }

        // money changes here
        inventoryScript.itensCharacterScript.characterBody.money -= price;
        inventoryScript.itensCharacterScript.characterBody.UpdateMoneyText();
    }

    public void DropItem(string itemName, string[] allItens, string itemType, float price)
    {
        // search and remove item from inventory
        int idx = System.Array.IndexOf(allItens, itemName);

        switch (itemType)
        {
            // clear the item slot founded and update the character
            case "hats":
                inventoryScript.myItens.hatName[idx] = "";
                inventoryScript.CallHatsToSlot();
                inventoryScript.itensCharacterScript.characterBody.SetHat("");
                ShowHatItens();
                break;
            case "body":
                inventoryScript.myItens.shirtName[idx] = "";
                inventoryScript.CallShirtsToSlot();
                inventoryScript.itensCharacterScript.characterBody.SetShirt("");
                ShowShirtItens();
                break;
            case "legs":
                inventoryScript.myItens.legName[idx] = "";
                inventoryScript.CallLegsToSlot();
                inventoryScript.itensCharacterScript.characterBody.SetLeg("");
                ShowLegItens();
                break;
            case "arms":
                inventoryScript.myItens.armName[idx] = "";
                inventoryScript.CallArmsToSlot();
                inventoryScript.itensCharacterScript.characterBody.SetArm("");
                ShowArmItens();
                break;
        }

        // money changes here
        inventoryScript.itensCharacterScript.characterBody.money += price;
        inventoryScript.itensCharacterScript.characterBody.UpdateMoneyText();
    }

    public void ConfirmBuyItem(string itemName, string[] allItens, string itemType, float price)
    {
        float moneyChar = inventoryScript.itensCharacterScript.characterBody.money;               
        textConfirm.text = "- Confirm the buy -\n" + itemName + "\nPrice - $" + price;       

        // remove the old listerners off the yes button to assign news to him
        yes.onClick.RemoveAllListeners();
   
        if (moneyChar >= price)
        {
            // show the correct dialog after click the button
            yes.onClick.AddListener(delegate { dialSystem.ChangeDialogue(idxOfFinishBuyQuestion); });

            // call the method who give to the player the selected item
            yes.onClick.AddListener(delegate { GetItem(itemName, allItens, itemType, price); });

            // hide the selected elements when the button pressed
            yes.onClick.AddListener(delegate { dialSystem.HideItemImmediately(); });

            // show the selected elements when the button pressed
            yes.onClick.AddListener(delegate { dialSystem.ShowItemImmediately(); });
        }
        else
        {
            // change the shopkeeper dialog and hide selected objects
            yes.onClick.AddListener(delegate { dialSystem.ChangeDialogue(idxOfDontHaveMoneyQuestion); });
            yes.onClick.AddListener(delegate { dialSystem.HideItemImmediately(); });
        }
    }

    public void ConfirmSellItem(string itemName, string[] allItens, string itemType, float price)
    {
        textConfirm.text = "- Confirm the sell -\n" + itemName + "\nPrice - $" + price;

        // // remove the old listerners off the yes button to assign news to him
        yes.onClick.RemoveAllListeners();

        // show the correct dialog after click the button
        yes.onClick.AddListener(delegate { dialSystem.ChangeDialogue(idxOfFinishSellQuestion); });

        // call the method who draw the player's item
        yes.onClick.AddListener(delegate { DropItem(itemName, allItens, itemType, price); });

        // hide the selected elements when the button pressed
        yes.onClick.AddListener(delegate { dialSystem.HideItemImmediately(); });

        // show the selected elements when the button pressed
        yes.onClick.AddListener(delegate { dialSystem.ShowItemImmediately(); });
    }
}
