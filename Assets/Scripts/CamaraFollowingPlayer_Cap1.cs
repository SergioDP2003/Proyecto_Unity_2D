using UnityEngine;

public class CameraFollowingPlayer_Cap1 : MonoBehaviour
{
    public Transform target;         // El jugador
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target == null) return;

        offset = new Vector3(8.5f, 3f, -10f);

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = new Vector3(
            smoothedPosition.x,
            smoothedPosition.y,
            transform.position.z   // MUY IMPORTANTE mantener la Z de la cámara
        );
    }
}

