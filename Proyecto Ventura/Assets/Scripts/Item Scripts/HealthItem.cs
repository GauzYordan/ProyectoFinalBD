using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{ 
    string nameItem;

    private GameObject player;
    private PlayerHealth playerHealth;

    private CharacterMovement characterMovement;

    private PowerItemExplode powerItemExplode;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
        playerHealth = player.GetComponent<PlayerHealth>();
        characterMovement = player.GetComponent<CharacterMovement>();
        powerItemExplode = GetComponent<PowerItemExplode>();
        nameItem = this.name;
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject ==  player){
            powerItemExplode.Pickup();
            playerHealth.PowerUpHealth();
            characterMovement.ObjetoTriggerPickup(this.name);
            Destroy(gameObject);
        }
    }
}
