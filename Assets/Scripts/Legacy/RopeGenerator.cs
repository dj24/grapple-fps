using UnityEngine;

public class RopeGenerator : MonoBehaviour
{
    int links = 15;
    float linkLength;
    float grappleSpeed = 0.5f;
    Transform[] children;
    private Vector3[] _connectedAnchor;
    private Vector3[] _anchor;
    public GameObject child;
    public Vector3 anchor;

    void GenerateLinks()
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
            if(newChild.GetComponent<HingeJoint>() != null){
                hinge = newChild.GetComponent<HingeJoint>();
                hinge.connectedBody = transform.GetChild(i - 1).gameObject.GetComponent<Rigidbody>();
            }
        }
    }

     void StoreLinkPositions(){
         children = transform.GetComponentsInChildren<Transform>();
        _connectedAnchor = new Vector3[children.Length];
        _anchor = new Vector3[children.Length];
        for (int i = 1; i < children.Length; i++)
        {
            if (children[i].GetComponent<Joint>() != null)
            {
                _connectedAnchor[i] = children[i].GetComponent<Joint>().connectedAnchor;
                _anchor[i] = children[i].GetComponent<Joint>().anchor;
                children[i].GetComponent<Joint>().autoConfigureConnectedAnchor = false;
                children[i].GetComponent<Rigidbody>().isKinematic = false;
            }
        }
     }

     void RotateLinks(){
         // Rotates links to starting position
        transform.rotation = Quaternion.FromToRotation(Vector3.down, GameManager.Player.transform.position - transform.position);
     }

    void AddPlayerToChain(){
        HingeJoint hinge = GameManager.Player.gameObject.AddComponent<HingeJoint>();
        hinge.connectedBody = transform.GetChild(links - 1).gameObject.GetComponent<Rigidbody>();
    }

    void MoveFirstLink(){
        HingeJoint firstHinge = transform.GetChild(0).GetComponent<HingeJoint>();
        if(firstHinge != null){
            firstHinge.connectedAnchor = anchor;
        }
    }

     void Start()
    {
       GenerateLinks();
       RotateLinks();
       StoreLinkPositions(); 
       AddPlayerToChain();
    }

    private void Update() {
        if(linkLength < 0.05f){
            Destroy(gameObject);
            return;
        }

        linkLength -= grappleSpeed * Time.deltaTime;
        GameObject currentChild;

        for (int i = 0; i < children.Length - 1; i++)
        {
            //TODO: fix this to use transform[i] instead of GetChild
            currentChild = transform.GetChild(i).gameObject;
            if(currentChild.transform.localScale.y > 0){
                currentChild.transform.localScale = new Vector3(0.2f, linkLength, 0.2f);
                if (children[i].GetComponent<HingeJoint>() != null)
                {
                    children[i].GetComponent<HingeJoint>().connectedAnchor = _connectedAnchor[i];
                    children[i].GetComponent<HingeJoint>().anchor = _anchor[i];
                }
            }
        }

        MoveFirstLink();
        
    }
}
