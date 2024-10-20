using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag=="Player"){
            GetComponent<MeshRenderer>().material.color=Color.green;
            gameObject.tag="ladder";

        }
        
    }
    //  void Update() {
    //     OnCollisionEnter(o);
    // }

}

