using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [Header("Buy/Sell state:")]
    public bool isBuying = true;

    void Start()
    {
        ShowHatItens();
    }

    private void Update()
    {
        if (Input.GetKeyDown("g"))
        {
           
        }
    }

    public void ShowHatItens()
    {
        if (isBuying)
        {
            // if buy call the set function using the actual class item to setup the values
            SetItensToBuy(inventoryScript.myItens.hatName, "hats");
        }
        else
        {
            // if not the sell function is call using the actual class item to setup the values
            SetItensToSell(inventoryScript.myItens.hatName, "hats");
        }           
    }

    // each function here is connected inside a button on navigator shop buttons
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
            SetItensToBuy(inventoryScript.myItens.shirtName, "legs");
        }
        else
        {
            SetItensToSell(inventoryScript.myItens.shirtName, "legs");
        }
    }

    public void ShowArmItens()
    {
        if (isBuying)
        {
            SetItensToBuy(inventoryScript.myItens.shirtName, "arms");
        }
        else
        {
            SetItensToSell(inventoryScript.myItens.shirtName, "arms");
        }
    }

    public void SetItensToBuy(string[] itemToSort, string ScriptableItens)
    {
        // first - the function deletes the others itens slot
        DeleteOldSlots();

        // search the actual itens inside the Scriptable Object
        GameItens.itemSpecs[] type = (GameItens.itemSpecs[])gameItensData.GetType().GetField(ScriptableItens).GetValue(gameItensData);

        // create a temporary array with the itens the player have
        string[] itemName = SortItensToBuy(type, itemToSort);

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
                slotScript.imgComponent.sprite = type[i].imgSpriteFRONT;
                slotScript.textComponent.text = "$" + type[i].value.ToString();
                slotScript.nameItem = type[i].nameItem;

                // assign a custom event to the slot button
                // if the player click him, a specified function inside the slot script is called to set this item
                switch (ScriptableItens)
                {
                    case "hats":
                        slotBTN.onClick.AddListener(delegate { GetItem(slotScript.nameItem, itemName, "hats"); });                       
                        break;
                    case "body":
                        slotBTN.onClick.AddListener(delegate { GetItem(slotScript.nameItem, itemName, "body"); });
                        break;
                    case "legs":
                        slotBTN.onClick.AddListener(delegate { GetItem(slotScript.nameItem, itemName, "legs"); });
                        break;
                    case "arms":
                        slotBTN.onClick.AddListener(delegate { GetItem(slotScript.nameItem, itemName, "arms"); });
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
                slotScript.imgComponent.sprite = type[idxItem].imgSpriteFRONT;
                slotScript.textComponent.text = "$" + type[idxItem].value.ToString();
                slotScript.nameItem = type[idxItem].nameItem;

                // assign a custom event to the slot button
                // if the player click him, a specified function inside the slot script is called to set this item
                switch (ScriptableItens)
                {
                    case "hats":
                        slotBTN.onClick.AddListener(delegate { DropItem(slotScript.nameItem, itemName, "hats"); });
                        break;
                    case "body":
                        slotBTN.onClick.AddListener(delegate { DropItem(slotScript.nameItem, itemName, "body"); });
                        break;
                    case "legs":
                        slotBTN.onClick.AddListener(delegate { DropItem(slotScript.nameItem, itemName, "legs"); });
                        break;
                    case "arms":
                        slotBTN.onClick.AddListener(delegate { DropItem(slotScript.nameItem, itemName, "arms"); });
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
        for (int i = 0; i < slotParent.transform.childCount; i++)
        {
            Destroy(slotParent.transform.GetChild(i).gameObject);
        }
    }

    public void BuyItem()
    {
        isBuying = true;        
    }

    public void SellItem()
    {
        isBuying = false;
    }

    public void GetItem(string itemName,string[] allItens, string itemType)
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

    }

    public void DropItem(string itemName, string[] allItens, string itemType)
    {
        // search and remove item from inventory
        int idx = System.Array.IndexOf(allItens, itemName);

        switch (itemType)
        {
            case "hats":
                inventoryScript.myItens.hatName[idx] = "";
                inventoryScript.CallHatsToSlot();
                ShowHatItens();
                break;
            case "body":
                inventoryScript.myItens.shirtName[idx] = "";
                inventoryScript.CallShirtsToSlot();
                ShowShirtItens();
                break;
            case "legs":
                inventoryScript.myItens.legName[idx] = "";
                inventoryScript.CallLegsToSlot();
                ShowLegItens();
                break;
            case "arms":
                inventoryScript.myItens.armName[idx] = "";
                inventoryScript.CallArmsToSlot();
                ShowArmItens();
                break;
        }
    }

}
