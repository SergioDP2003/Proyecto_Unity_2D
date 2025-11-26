using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;         // El jugador
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target == null) return;

        offset = new Vector3(0f, 2f, -10f);

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = new Vector3(
            smoothedPosition.x,
            smoothedPosition.y,
            transform.position.z   // MUY IMPORTANTE mantener la Z de la cámara
        );
    }
}

