using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed;
    public enum Axisis{
        X,
        Y,
        Z
    }
    public Axisis axis; 

    // Update is called once per frame
    void Update()
    {
        switch(axis){
            case Axisis.X:
            transform.Rotate(transform.right * speed * Time.deltaTime); 
            break;
            case Axisis.Y:
            transform.Rotate(transform.up * speed * Time.deltaTime); 
            break;
            case Axisis.Z:
            transform.Rotate(transform.forward * speed * Time.deltaTime);
            break;
            default:
            Debug.Log("Axisis mistake");
            break; 
        }
       
    }
}
