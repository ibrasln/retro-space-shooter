using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        instance = this;
    }

    public IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            elapsed += Time.deltaTime;
            transform.position = new Vector3(x, y, -10f);
            yield return 0;
        }
        transform.position = originalPosition;
    }

    public void ZoomInCamera(float fov, Vector2 pos, float speed)
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, speed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, new Vector3(pos.x, pos.y, transform.position.z), speed * Time.deltaTime);
    }

    public void ZoomOutCamera(float speed)
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 80, speed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, transform.position.z), speed * Time.deltaTime);
    }

}
