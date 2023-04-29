using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public Transform head;
    void Update()
    {
        transform.position = head.position;
    }
}
