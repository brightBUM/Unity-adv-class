using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] float speed = 3.0f;
    [SerializeField] Transform holdTransform;
    Rigidbody rb;

    Transform inRangeObject;
    Transform holdingObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        rb.MovePosition(rb.position+move*Time.deltaTime*speed);

        //interaction
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(inRangeObject!=null) //holding
            {
                //pickUp
                if(holdingObject!=null)
                {
                    //swap
                    var tempPos = inRangeObject.position;
                    var tempObject = holdingObject;

                    inRangeObject.parent = this.transform;
                    inRangeObject.localPosition = holdTransform.localPosition;
                    holdingObject = inRangeObject;

                    inRangeObject = tempObject;
                    inRangeObject.parent = null;
                    inRangeObject.position = tempPos;


                    return;
                }
                else
                {
                    //attach
                    inRangeObject.parent = this.transform;
                    inRangeObject.localPosition = holdTransform.localPosition;
                    holdingObject = inRangeObject;
                    inRangeObject = null;
                    return;
                }
            }
            else
            {
                //dropped
                if (holdingObject != null)
                {
                    holdingObject.parent = null; //freeing
                    holdingObject = null;
                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
            inRangeObject = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
            inRangeObject = null;
    }
}
