using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class ForceTeste : MonoBehaviour
{

    Rigidbody rdb;
    public ControlePersonagemTeste player;
    public bool pushing;

    BoxCollider caixa;

    Vector3 moveInput;
    Vector3 moveVelocity;

    void Start()
    {
        rdb = GetComponent<Rigidbody>();
        caixa = GetComponent<BoxCollider>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (pushing)
        {
            player.transform.parent = this.transform;
            //caixa.isTrigger = true;

        }

        /*
        moveInput = new Vector3(horizontal, 0, vertical);
        moveInput.Normalize();

        rdb.AddForce(player.transform.forward * 1);*/
        //rdb.AddTorque(personagem.transform.forward * 5000f);
        /*
            else
                personagem.transform.parent = null;
    }

    public bool Interact()
{
    return true;
}

public bool NoInteract()
{
    return false;
}
public void CharacterInteract(ControlePersonagemTeste Personagem)
{
    player = Personagem.GetComponent<ControlePersonagemTeste>();
    // player.transform.position = Personagem.transform.position;

    player.characterState = ControlePersonagemTeste.CharacterState.pushing;
    SetPosition();

    Interact();
    pushing = true;
}

public void NoCharacterInteract(ControlePersonagemTeste Personagem)
{
    player = Personagem.GetComponent<ControlePersonagemTeste>();
    // player.transform.position = Personagem.transform.position;

    player.characterState = ControlePersonagemTeste.CharacterState.idle;

    pushing = false;
}


void SetPosition()
{
    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    //player.transform.position = new Vector3(this.transform.position.x + 50000f, player.transform.position.y, this.transform.position.z);

    print(player.transform.position);


}



private void OnCollisionEnter(Collision hit)
{
    CharacterPushing(hit);

}

bool CharacterPushing(Collision character)
{

    if (character.collider.tag == "Player")
    {
        personagem = character.collider.GetComponent<ControlePersonagem>();

        if (personagem.characterState == ControlePersonagem.CharacterState.pushing)
            print("FORCE");
        return pushing = true;
    }
    return pushing = false;

}


private void OnControllerColliderHit(ControllerColliderHit hit)
{
    CharacterPushing2(hit);
}

void CharacterPushing2(ControllerColliderHit character)
{

    if (character.collider.tag == "Player")
    {
        personagem = character.collider.GetComponent<ControlePersonagem>();

        if (personagem.characterState == ControlePersonagem.CharacterState.pushing)
            return pushing = true;
    }
    return pushing = false;

    print("FORCE2");
}*/



