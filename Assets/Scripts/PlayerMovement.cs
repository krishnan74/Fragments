using UnityEngine;

public class PlayerMovement : MonoBehaviour{
 
    #region "Variables"
    private Rigidbody Rigid;
    public float MouseSensitivity;

    public float MoveSpeed;
    public Camera PlayerCamera;
    public float JumpForce;

    public float pickUpDistance = 5;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;

    private ObjectGrabbable objectGrabbable;
    #endregion
   
    void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Rigid = GetComponent<Rigidbody>();

    }


    void Update ()
    {
        Rigid.MoveRotation(Rigid.rotation * Quaternion.Euler(new Vector3(0, Input.GetAxis("Mouse X") * MouseSensitivity, 0)));
        Rigid.MovePosition(transform.position + (transform.forward * Input.GetAxis("Vertical") * MoveSpeed) + (transform.right * Input.GetAxis("Horizontal") * MoveSpeed));
        if (Input.GetKeyDown("space")){
            Rigid.AddForce(transform.up * JumpForce);
        }

        float verticalRotation = -Input.GetAxis("Mouse Y") * MouseSensitivity;
        Vector3 currentRotation = PlayerCamera.transform.localRotation.eulerAngles;
        float newRotationX = currentRotation.x + verticalRotation;
        
        PlayerCamera.transform.localRotation = Quaternion.Euler(newRotationX, currentRotation.y, currentRotation.z);
        PickUpObject();
        
    }

    void PickUpObject(){
        if(Input.GetKeyDown(KeyCode.E)){

            if(objectGrabbable == null){

                if(Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out RaycastHit hit, pickUpDistance)){
                    if(hit.transform.TryGetComponent(out objectGrabbable)){
                        objectGrabbable.Grab(objectGrabPointTransform);
                    }
                }
            }

            else{
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
            
        }
    }
}
 