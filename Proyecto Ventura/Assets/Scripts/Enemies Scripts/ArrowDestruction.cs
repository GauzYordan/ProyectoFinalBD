using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDestruction : MonoBehaviour
{
    public float lifeSpan = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeSpan);
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject){
            Destroy(this.gameObject);
        }
    }
}
