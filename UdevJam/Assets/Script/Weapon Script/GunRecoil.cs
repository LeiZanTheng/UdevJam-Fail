using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    Vector3 CurrentRotation;
    Vector3 TargetRotation;
    // Start is called before the first frame update
    [SerializeField] float RecoilX;
    [SerializeField] float RecoilY;
    [SerializeField] float RecoilZ;
    [SerializeField] float Snappiness;
    [SerializeField] float ReturnSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TargetRotation = Vector3.Lerp(TargetRotation,Vector3.zero, ReturnSpeed * Time.deltaTime);
        CurrentRotation = Vector3.Slerp(CurrentRotation, TargetRotation, Snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(CurrentRotation);
    }
    public void RecoilFire()
    {
        TargetRotation += new Vector3(RecoilX, Random.Range(RecoilY,-RecoilY), Random.Range(RecoilZ,-RecoilZ));
    }
}
