using UnityEngine;

public class RigidbodyCleaner : MonoBehaviour
{
    [SerializeField]
    private new Rigidbody rigidbody;

    public void Clean()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}