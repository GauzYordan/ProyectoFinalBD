using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System;

public class CharacterMovement : MonoBehaviour
{
    public float maxSpeed = 6.0f;
    public bool facingRight = true;
    public float moveDirection;

    public float jumpSpeed = 60.0f;
    public bool grounded = false;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    new Rigidbody rigidbody;
    private Animator anim;

    //Spawn of sword
    public float swordSpeed = 600.0f;
    public Transform swordSpawn;
    public Rigidbody[] ammo;

    Rigidbody clone;

    private new AudioSource audio;
    public AudioClip swordAudio;
    public AudioClip playerJump;

    public WeaponSwitching weaponSwitching;
    public int weapon = 0;

    private DBConnection dbConnection;

    public int swordAmmo;
    public int crossBowAmmo;

    float timer = 0f;

    void Awake()
    {
        groundCheck = GameObject.Find("GroundCheck").transform;
        swordSpawn = GameObject.Find("WeaponSpawn").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();


        weaponSwitching = GetComponentInChildren<WeaponSwitching>();

        dbConnection = FindObjectOfType<DBConnection>();
        swordAmmo = dbConnection.IniAmmo("Sword");
        crossBowAmmo = dbConnection.IniAmmo("Arrow");
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = Input.GetAxis("Horizontal");
        

        if(grounded && Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("isJumping");
            rigidbody.AddForce(new Vector2(0, jumpSpeed));
            audio.PlayOneShot(playerJump);
        }

        if(Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

        
        if(timer >= 5f){
            PosicionActual();
            timer = 0;
        }

        timer+= Time.deltaTime;
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        rigidbody.velocity = new Vector2(moveDirection * maxSpeed, rigidbody.velocity.y);

        if(moveDirection > 0.0f && !facingRight)
        {
            Flip();
        } else if (moveDirection < 0.0f && facingRight)
        {
            Flip();
        }
        anim.SetFloat("Speed", Mathf.Abs(moveDirection));
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World);
    }

    void Attack()
    {
        weapon = weaponSwitching.SelectedWeapon;
        
        if(weapon == 0 && swordAmmo > 0){
            anim.SetTrigger("attacking");
            dbConnection.RestaMunicionesActuales(1, "Sword");
            swordAmmo = dbConnection.IniAmmo("Sword");

            dbConnection.SumaMunicionesUsadas(1, "Sword");
        }else if(weapon == 1 && crossBowAmmo > 0){
            anim.SetTrigger("attacking");
            dbConnection.RestaMunicionesActuales(1, "Arrow");
            crossBowAmmo = dbConnection.IniAmmo("Arrow");

            dbConnection.SumaMunicionesUsadas(1, "Arrow");

        }
    }

    public void CallFireProyectile()
    {
        

        clone = Instantiate(ammo[weapon], swordSpawn.position, swordSpawn.rotation) as Rigidbody;
        clone.AddForce(swordSpawn.transform.right * swordSpeed);
        audio.PlayOneShot(swordAudio);

    }

    public void AmmoPickup(string ammoPicked){
        int min = 10;
        int max = 100;

        if(ammoPicked == "AmmoArrow"){
            dbConnection.AmmoPickup(1, "Arrow", UnityEngine.Random.Range(min, max), ammoPicked, this.transform.position.x, this.transform.position.y);
            crossBowAmmo = dbConnection.IniAmmo("Arrow");
        }else if(ammoPicked == "AmmoSword"){
            dbConnection.AmmoPickup(1, "Sword", UnityEngine.Random.Range(min, max), ammoPicked, this.transform.position.x, this.transform.position.y);
            swordAmmo = dbConnection.IniAmmo("Sword");
        }
    }

    public void ObjetoTriggerPickup(string objectPicked){
        dbConnection.InsertObjetoTrigger(1, objectPicked, this.transform.position.x, this.transform.position.y);
    }

    public void PosicionActual(){
        var fecha = System.DateTime.Now;

        dbConnection.PosicionPlayer(1, fecha.ToString(), this.transform.position.x, this.transform.position.y);
    }
}