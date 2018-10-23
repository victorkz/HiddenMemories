using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found");
            return;
        }

        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallBack;

    public int space;

    //Lista publica de items
    public List<Item> items = new List<Item>();

    public Item combineItem1;
    public Item combineItem2;

    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            print("Não há espaço suficiente");
            return false;
        }

        items.Add(item);

        //Sempre que algo mudar no inventario o delegate é chamado
        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();

        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();
    }

    public void CombineItems(Item item)
    {
        if (combineItem1 == null)
            combineItem1 = item;
        else if (combineItem2 == null)
            combineItem2 = item;

        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();

    }

    //Teste
    public virtual void UseItem(Item item)
    {
        //usa o item algo ocorre

        Debug.Log("Habilita " + item.name);

    }

    public virtual void CombinaItems()
    {

    }

}

