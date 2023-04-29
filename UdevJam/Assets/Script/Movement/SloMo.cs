using UnityEngine;

public class SloMo : MonoBehaviour
{
    public float slowDownFactor = 0.05f;
    public static bool isSlomo;
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            DoSlowMotion();
        }
        if(!Input.GetKey(KeyCode.F))
        {
            Time.timeScale = 1f;
            isSlomo = false;
        }
    }
    
    public void DoSlowMotion()
    {
        isSlomo = true;
        Time.timeScale = slowDownFactor;
    }
}
