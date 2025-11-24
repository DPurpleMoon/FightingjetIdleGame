using UnityEngine;

namespace jetfighter.rotation
{
public class playerrotation : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        LookAt(mousePos);
    }

    protected void LookAt(Vector3 target)
    {
        float lookAngle = AngleBetweenTwoPoints(target, transform.position);

        transform.eulerAngles = new Vector3(0, 0, lookAngle - 90f);
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
}