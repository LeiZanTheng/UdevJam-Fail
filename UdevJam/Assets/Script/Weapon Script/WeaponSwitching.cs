using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    int weaponIndex = 0;
    public float TimeBtwSwitchching = 0.3f;
    bool allowSwitch;
    private void Start() {
        allowSwitch = true;
    }
    private void Update() {
        if(allowSwitch)
        {
            int PreviousIndex = weaponIndex;
            if(Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if(weaponIndex >= transform.childCount - 1)
                {
                    weaponIndex = 0;
                }
                else
                {
                    weaponIndex++;
                }
                allowSwitch = false;
                Invoke("ResetSwitch", TimeBtwSwitchching);
            }
            if(Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if(weaponIndex <= 0)
                {
                    weaponIndex = transform.childCount - 1;
                }
                else
                {
                    weaponIndex--;
                }
                allowSwitch = false;
                Invoke("ResetSwitch", TimeBtwSwitchching);
            }
            if(PreviousIndex != weaponIndex)
            {
                SelectWeapon();
            }
        }
    }
    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if(i == weaponIndex)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
    void ResetSwitch()
    {
        allowSwitch = true;
    }
}
