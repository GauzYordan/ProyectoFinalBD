using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpItem : MonoBehaviour
{
    private GameObject player;
    private CharacterMovement charMov;
    private PlayerHealth playerHp;

    private SphereCollider sphereCollider;
    private PowerItemExplode itemExplode;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
        charMov = player.GetComponent<CharacterMovement>();
        playerHp = player.GetComponent<PlayerHealth>();

        sphereCollider = GetComponent<SphereCollider>();
        itemExplode = GetComponent<PowerItemExplode>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject == player){
            playerHp.JumpItem();
            charMov.ObjetoTriggerPickup(this.name);
            StartCoroutine(InvincibleRoutine());
            sprite.enabled = false;
        }
    }

    public IEnumerator InvincibleRoutine(){
        print("pick mega jump");

        itemExplode.Pickup();
        sphereCollider.enabled = false;

        charMov.jumpSpeed = 1200f;

        yield return new WaitForSeconds(10f);
        print("no more mega  jump");
        charMov.jumpSpeed = 600f;
        Destroy(gameObject);     
    }
}
