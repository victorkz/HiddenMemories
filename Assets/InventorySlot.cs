using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;

    public Button ItemButton;
    public Button removeButton;
    public Button useButton;
    public Button combineButton;

    //Cor itemButton
    ColorBlock ItemButtonColor;

    public Item item;

    private void Awake()
    {
        // ItemButtonColor = GetComponent<ColorBlock>();
        ItemButtonColor.normalColor = new Color(255, 0, 0, 255);
        ItemButtonColor.colorMultiplier = 5;
    }

    //Adiciona item e habilita ele no menu
    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.Icon;
        icon.enabled = true;

        removeButton.interactable = true;
        useButton.interactable = true;
        combineButton.interactable = true;
    }

    //Ñ funciona
    public void CombineItem(Item item1, Item item2)
    {
        if (item1.ItemName == item2.combineItem.name || item2.ItemName == item1.combineItem.name)
        {
            item = item1.becomeItem;
        }
    }

    //Ñ funciona
    void CombineObject(Item newItem, GameObject gameObject)
    {
        //Verifica nome do item
        //verifica se o game object é igual ao do item
        //executa
    }


    public void ClearShot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;

        removeButton.interactable = false;
        useButton.interactable = false;
        combineButton.interactable = false;
    }

    //Remove item e desabilita ele no menu
    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }

    //Coloca Item para se combinado
    public void OnCombineButton()
    {
        Inventory.instance.CombineItems(item);
        //ItemButton.colors = ItemButtonColor;
    }

    public void OnUseItem()
    {
        if (item != null)
        {
            //item.Use();
            Inventory.instance.UseItem(item);

        }
    }
}
