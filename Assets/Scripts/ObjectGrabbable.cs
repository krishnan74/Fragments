using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;

    private bool isInPattaraiZone = false; 

    void Awake()
    {
        objectRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
        if(objectGrabPointTransform != null){
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, lerpSpeed * Time.deltaTime);
            objectRigidBody.MovePosition(newPosition);
            //objectRigidBody.MoveRotation(objectGrabPointTransform.rotation);
        }
    }

    private void OnTriggerEnter(
        Collider other)
    {
        if (other.gameObject.CompareTag("pattarai"))
        {
            Debug.Log("Object entered the trigger zone with tag 'pattarai'");

            isInPattaraiZone = true;
            
        }
    
    }

    private void OnTriggerExit(
        Collider other)
    {
        if (other.gameObject.CompareTag("pattarai"))
        {
            Debug.Log("Object exited the trigger zone with tag 'pattarai'");
            isInPattaraiZone = false;

           
        }
    }

    public void Drop(){
        objectGrabPointTransform = null;

        if( isInPattaraiZone == true){
            Debug.Log("Object dropped in the trigger zone with tag 'pattarai'");
            return; 
        }
        
        objectRigidBody.useGravity = true;
    }

    public void Grab( Transform objectGrabPointTransform){
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidBody.useGravity = false;
    }
}
