using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDestruction : MonoBehaviour
{
    public float lifeSpan = 2.0f;

    void Start(){
        Destroy(gameObject, lifeSpan);
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject){
            Destroy(this.gameObject);
        }
    }
}
