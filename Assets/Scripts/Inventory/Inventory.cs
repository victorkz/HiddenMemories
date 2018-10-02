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
    public int space = 4;
    //Lista publica de items
    public List<Item> items = new List<Item>();

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
}

