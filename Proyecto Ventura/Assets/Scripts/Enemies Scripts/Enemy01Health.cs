using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy01Health : MonoBehaviour
{
    [SerializeField] private int startingHealth = 20;
    [SerializeField] private float timeSinceLastHit = 0.5f;
    [SerializeField] private float dissapearSpeed = 2f;
    [SerializeField] private int currentHealth;

    private float timer = 0f;
    private Animator anim;
    private NavMeshAgent nav;
    private bool isAlive;
    private new Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;
    private bool dissapearEnemy = false;
    private BoxCollider weaponCollider;

    private new AudioSource audio;
    public AudioClip hurtAudio;
    public AudioClip deathAudio;

    private DropItems dropItems;

    private DBConnection dbConn;

    string otherName;
    public bool IsAlive { get => this.isAlive; }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        isAlive = true;
        currentHealth = startingHealth;
        weaponCollider = GetComponentInChildren<BoxCollider>();
        audio = GetComponent<AudioSource>();
        dropItems = GetComponent<DropItems>();

        dbConn = FindObjectOfType<DBConnection>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(dissapearEnemy)
        {
            transform.Translate(-Vector3.up * dissapearSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        otherName = other.name;
        if(timer >= timeSinceLastHit && !GameManager.instance.GameOver){
            if(other.tag == "PlayerWeapon")
            {
                TakeHit();
                timer = 0f;
            }
        }
    }

    void TakeHit()
    {
        if(currentHealth > 0)
        {
            anim.Play("EnemyHurt");
            audio.PlayOneShot(hurtAudio);
            currentHealth -= 10;
        }

        if(currentHealth <= 0)
        {
            EnviarBD();
            isAlive = false;
            KillEnemy();
        }
    }

    void KillEnemy()
    {
        capsuleCollider.enabled = false;
        nav.enabled = false;
        anim.SetTrigger("EnemyDie");
        audio.PlayOneShot(deathAudio);
        rigidbody.isKinematic = true;
        weaponCollider.enabled = false;



        StartCoroutine(removeEnemy());
        print("Drop Item");
        dropItems.Drop();
    }

    IEnumerator removeEnemy()
    {
        yield return new WaitForSeconds(2f);
        dissapearEnemy = true;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void EnviarBD(){
        var fecha = System.DateTime.Now;
        if(otherName == "SwordPFB(Clone)"){
            dbConn.InsertObjetoDestruido(1, this.name, "Sword", fecha.ToString());
        }else if (otherName == "PlayerArrow_PFB(Clone)"){
            dbConn.InsertObjetoDestruido(1, this.name, "Arrow", fecha.ToString());
        }
        
    }

}
