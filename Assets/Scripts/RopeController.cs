using UnityEngine;

public class RopeController : MonoBehaviour
{
    public bool active = false;
    public Vector3 start, end, direction;
    public float length;

    void FixedUpdate()
    {
        direction = start - end;

        length = Vector3.Distance(end, start);

        if (!active)
        {
            transform.localScale = new Vector3(0,0,0);
            return;
        }

        transform.localScale = new Vector3(0.1f, length / 2, 0.1f);

        transform.position = (end - start) / 2.0f + start;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);
    }
}
