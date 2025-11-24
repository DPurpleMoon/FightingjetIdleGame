using UnityEngine;

namespace jetfighter.rotation
{
public class rotation : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition) ;
        Vector2 direction = (mousePos - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg +90;

        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}
}
