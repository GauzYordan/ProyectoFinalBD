using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerItem : MonoBehaviour
{

    private GameObject player;
    private PlayerHealth playerHealth;
    private CharacterMovement charMov;

    private new ParticleSystem particleSystem;

    private MeshRenderer meshRenderer;
    private ParticleSystem brainParticles;

    private PowerItemExplode powerItemExplode;
    private SphereCollider sphereCollider;

    public GameObject pickupEffect;

    void Pickup(){
        Instantiate(pickupEffect, transform.position, transform.rotation);
    }

    // Start is called before the first frame update
    void Start(){
        player = GameManager.instance.Player;
        playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.enabled = true;

        charMov = player.GetComponent<CharacterMovement>();

        particleSystem = player.GetComponent<ParticleSystem>();
        var emission = particleSystem.emission;
        emission.enabled = false;

        meshRenderer = GetComponentInChildren<MeshRenderer>();
        brainParticles = GetComponent<ParticleSystem>();    

        powerItemExplode = GetComponent<PowerItemExplode>();
        sphereCollider = GetComponent<SphereCollider>();    
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject == player){
            playerHealth.InvincibilityItem();
            charMov.ObjetoTriggerPickup(this.name);
            StartCoroutine(InvincibleRoutine());
            meshRenderer.enabled = false;
        }
    }

    public IEnumerator InvincibleRoutine(){
        print("pick invincible");

        powerItemExplode.Pickup();

        var emission = particleSystem.emission;
        emission.enabled = true;

        playerHealth.enabled = false;

        var brainEmission = brainParticles.emission;
        brainEmission.enabled = false;

        sphereCollider.enabled = false;

        yield return new WaitForSeconds(10f);
        print("no more invincible");
        emission.enabled = false;
        playerHealth.enabled = true;
        Destroy(gameObject);        
    }
}
