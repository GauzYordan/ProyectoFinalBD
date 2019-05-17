using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeItem : MonoBehaviour
{
    private GameObject player;
    private LifeManager lifeManager;

    private CharacterMovement charMov;

    private SpriteRenderer spriteRenderer;

    public GameObject pickUpEffect;

    private PowerItemExplode powerItemExplode;
    private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
        lifeManager = FindObjectOfType<LifeManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        powerItemExplode = GetComponent<PowerItemExplode>();
        boxCollider = GetComponent<BoxCollider>();
        charMov = player.GetComponent<CharacterMovement>();
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject == player){
            PickLife();
            print("Life Collected");
        }
    }

    public void PickLife(){
        lifeManager.GiveLife();
        powerItemExplode.Pickup();
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        charMov.ObjetoTriggerPickup(this.name);
        Destroy(gameObject);
    }
}
