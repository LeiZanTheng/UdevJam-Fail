using UnityEngine;

public class GRABBING : MonoBehaviour
{
    public Transform Cam;
    public LayerMask Layer;
    public Rigidbody rb;
    public float GrabbingDistance = 3f;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        if (rb)
        {
            Vector3 vector = (Cam.position + Cam.forward * 2) - rb.transform.position;
            Vector3 torque = new Vector3(-rb.transform.position.x, -rb.transform.position.y, -rb.transform.position.z) * Mathf.Deg2Rad;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.AddForce(vector * 500f);
            //use this to spin objects
            //rb.AddTorque(torque * 100f);

            if(vector.magnitude > GrabbingDistance)
            {
                Release();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            RaycastHit Hitinfo;
            if (Physics.Raycast(Cam.position, Cam.forward,out Hitinfo, GrabbingDistance, Layer))
            {
                rb = Hitinfo.collider.GetComponent<Rigidbody>();
            }
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            Release();
        }
    }
    void Release()
    {
        rb = null; 
    }
}
