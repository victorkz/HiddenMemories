using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    //nome do objeto
    public string ItemName;
    //icone
    public Sprite Icon = null;
    //Item que podemos combinar, pode ser nulo
    public Item combineItem = null;
    //objeto in game que podemos combinar
    public GameObject combineGameObject = null;
    //se ouver item de combinar ele se torna:
    public Item becomeItem = null;

    public virtual void Use()
    {
        //usa o item algo ocorre

        Debug.Log("Habilita " + name);

    }

}
