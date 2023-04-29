using UnityEngine;

public class WallRun : MonoBehaviour
{
    public bool IsWallrun = false;
    public LayerMask ignorePlayerandObject;
    public RaycastHit SlopeHit;
    public float SlopeAngle = 60f;
    public Transform orientation;
    public float walldistance = 0.6f;
    public float MinimumJumpheight = 1.5f;
    public float OnWallRunForce = 100f;
    public Rigidbody rb;
    bool wallleft = false;
    bool wallright = false;
    public float wallrungravity;
    public float wallrunjumpforce;
    public float WallRunHorizontalJumpforce;

    //camera effect
    public Camera Cam;
    public float CamTilt = 10f;
    public float TiltTime = 15f;
    public float tilt { get; private set; }
    RaycastHit leftwallhit;
    RaycastHit rightwallhit;
    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, MinimumJumpheight);
    }
    void Checkwall()
    {
        wallleft = Physics.Raycast(transform.position, -orientation.right, out leftwallhit, walldistance, ~ignorePlayerandObject);
        wallright = Physics.Raycast(transform.position, orientation.right, out rightwallhit, walldistance, ~ignorePlayerandObject);
    }
    private void Update() 
    {
        Checkwall();
        if (CanWallRun())
        {
            if (wallleft)
            {
                DoWallRun();
            }
            if (wallright)
            {
                DoWallRun();
            }
            if (!wallleft && !wallright)
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
        if (OnSlope())
        {
            if (rb.velocity.y > 0 && !Input.GetButton("Jump") && !Input.GetKeyDown(KeyCode.LeftShift))
            {
                rb.AddForce(Vector3.down * 4650f * Time.deltaTime, ForceMode.Force);
            }
        }
    }
    void DoWallRun()
    {
        IsWallrun = true;
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallrungravity /** Time.deltaTime * 100f*/, ForceMode.Force);
        rb.AddForce(orientation.forward * OnWallRunForce * Time.deltaTime, ForceMode.Force);
        if (wallleft)
        {
            tilt = Mathf.Lerp(tilt, -CamTilt, TiltTime * Time.deltaTime);
        }
        if (wallright)
        {
            tilt = Mathf.Lerp(tilt, CamTilt, TiltTime * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallleft)
            {
                Vector3 walljumpdir = transform.up;
                Vector3 WalljumpDirHorizontal = leftwallhit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(walljumpdir * wallrunjumpforce * 100, ForceMode.Force);
                rb.AddForce(WalljumpDirHorizontal * WallRunHorizontalJumpforce * 100, ForceMode.Force);
            }
            if (wallright)
            {
                Vector3 walljumpdir = transform.up;
                Vector3 WalljumpDirHorizontal = rightwallhit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(walljumpdir * wallrunjumpforce * 100, ForceMode.Force);
                rb.AddForce(WalljumpDirHorizontal * WallRunHorizontalJumpforce * 100, ForceMode.Force);
            }
        }
    }
    //because the use gravity in the wallrun is enable all time so I add the slope handle in the wallrun script to manage easily
    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out SlopeHit, 1f + 0.3f) && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            float Angle = Vector3.Angle(Vector3.up, SlopeHit.normal);
            return Angle < SlopeAngle && Angle != 0;
        }
        return false;
    }
    void StopWallRun()
    {
        rb.useGravity = !OnSlope();
        tilt = Mathf.Lerp(tilt, 0, TiltTime * Time.deltaTime);
        IsWallrun = false;
    }
}