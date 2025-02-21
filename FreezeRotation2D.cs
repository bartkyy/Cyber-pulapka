using UnityEngine;

public class FreezeRotation2D : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Blokada rotacji na osi Z
    }
}
