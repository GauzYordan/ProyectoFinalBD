using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] int startingHealth = 100;
    [SerializeField] float timeSinceLastHit = 2.0f;
    [SerializeField] Slider healthSlider;

    private CharacterMovement characterMovement;
    [SerializeField] private float timer = 0f;
    private Animator anim;
    [SerializeField] private int currentHealth;

    private new AudioSource audio;
    public AudioClip hurtAudio;
    public AudioClip deathAudio;
    public AudioClip pickItem;

    private new ParticleSystem particleSystem;

    public LevelManager levelManager;

    public bool isDead;

    public int CurrentHealth{
        get{return currentHealth;}
        set{
            if(value < 0){
                currentHealth = 0;
            }else{
                currentHealth = value;
            }
        }
    }

    public Slider HealthSlider { 
        get {return healthSlider;} 
    }

    void Awake(){
        Assert.IsNotNull(healthSlider);
    }

    // Start is called before the first frame update
    void Start(){
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
        characterMovement = GetComponent<CharacterMovement>();
        audio = GetComponent<AudioSource>();
        levelManager = FindObjectOfType<LevelManager>();
        isDead = false;

        particleSystem = GetComponent<ParticleSystem>();
        var emission = particleSystem.emission;
        emission.enabled = false;
    }

    public void PlayerKill(){
        if(currentHealth <= 0){
            characterMovement.enabled = false;
            levelManager.RespawnPlayer();
        }
    }

    // Update is called once per frame
    void Update(){
        timer += Time.deltaTime;
        PlayerKill();
    }

    void OnTriggerEnter(Collider other){
        if(timer >= timeSinceLastHit && !GameManager.instance.GameOver){
            if(other.tag == "Weapon"){
                takeHit();
                timer = 0;
            }
        }
    }

    void takeHit(){
        if(currentHealth > 0){
            GameManager.instance.PlayerHit(currentHealth);
            anim.Play("PlayerHurt");
            currentHealth -= 10;
            healthSlider.value = currentHealth;
            audio.PlayOneShot(hurtAudio);
        }

        if(currentHealth <= 0){
            GameManager.instance.PlayerHit(currentHealth);
            anim.SetTrigger("isDead");
            characterMovement.enabled = false;
            audio.PlayOneShot(deathAudio);
        }
    }

    public void PowerUpHealth(){
        if(currentHealth <= 80){
            CurrentHealth += 20;
        }else if(currentHealth < startingHealth){
            CurrentHealth = startingHealth;
        }
        healthSlider.value = currentHealth;
        audio.PlayOneShot(pickItem);
    }

    public void InvincibilityItem(){
        audio.PlayOneShot(pickItem);
    }

    public void JumpItem(){
        audio.PlayOneShot(pickItem);
    }

    public void KillBox(){
        CurrentHealth = 0;
        healthSlider.value = currentHealth;
    }
}
