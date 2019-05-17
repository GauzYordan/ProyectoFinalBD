using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] private int startingHealth = 20;
    [SerializeField] private float timeSinceLastHit = 0.5f;
    [SerializeField] private int currentHealth;

    private float timer = 0f;
    private new Rigidbody rigidbody;
    private BoxCollider boxCollider;

    private DBConnection dbConn;

    string otherName;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        currentHealth = startingHealth;

        dbConn = FindObjectOfType<DBConnection>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
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
            currentHealth -= 10;
        }

        if(currentHealth <= 0)
        {
            EnviarBD();
            KillEnemy();
        }
    }

    void KillEnemy()
    {
        
        boxCollider.enabled = false;
        rigidbody.isKinematic = true;

        StartCoroutine(removeEnemy());
    }

    IEnumerator removeEnemy()
    {
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
