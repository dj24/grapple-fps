using UnityEngine;

public class RopeGenerator : MonoBehaviour
{
    int links = 30;

    public GameObject child;
    float linkLength;


    void Start()
    {
        float distance = Vector3.Distance(GameManager.Player.transform.position, transform.position);
        linkLength = distance / links / 2f;

        HingeJoint hinge;
        GameObject newChild = Instantiate(child, transform.position, Quaternion.identity, transform);
        newChild.transform.localScale = new Vector3(0.2f, linkLength, 0.2f);

        //TODO: refactor to add first link in loop, starting at 0
        for (int i = 1; i < links; i++)
        {
            Vector3 newPos = newChild.transform.position;
            Vector3 adjustedPos = new Vector3(newPos.x, newPos.y - (linkLength * 2), newPos.z);
            newChild = Instantiate(newChild, adjustedPos, Quaternion.identity, transform);
            hinge = newChild.GetComponent<HingeJoint>();
            hinge.connectedBody = transform.GetChild(i - 1).gameObject.GetComponent<Rigidbody>();
        }

        transform.rotation = Quaternion.FromToRotation(Vector3.down, GameManager.Player.transform.position - transform.position);

        hinge = GameManager.Player.gameObject.AddComponent<HingeJoint>();
        hinge.connectedBody = transform.GetChild(14).gameObject.GetComponent<Rigidbody>();

        for (int i = 1; i < links; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

    }

    private void Update()
    {
        linkLength -= 0.1f * Time.deltaTime;
        GameObject currentChild;

        for (int i = 0; i < links; i++)
        {
            currentChild = transform.GetChild(i).gameObject;
            Vector3 currentPos = currentChild.transform.position;
            currentChild.transform.localScale = new Vector3(0.2f, linkLength, 0.2f);
            //currentChild.transform.position = new Vector3(currentPos.x, currentPos.y - (linkLength * 2), currentPos.z);
        }
    }
}
