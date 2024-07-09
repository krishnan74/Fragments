using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;
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

    public void Drop(){
        objectRigidBody.useGravity = true;
        objectGrabPointTransform = null;
    }

    public void Grab( Transform objectGrabPointTransform){
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidBody.useGravity = false;
    }
}
