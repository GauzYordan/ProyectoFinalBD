using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : MonoBehaviour
{

    private GameObject player;
    private CharacterMovement characterMovement;

    private PowerItemExplode powerItemExplode;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
        characterMovement = player.GetComponent<CharacterMovement>();
        powerItemExplode = GetComponent<PowerItemExplode>();
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject ==  player){
            powerItemExplode.Pickup();
            characterMovement.AmmoPickup(this.name);
            Destroy(gameObject);
        }
    }
}

