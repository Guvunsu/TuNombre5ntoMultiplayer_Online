using UnityEngine;

public class BindBonesToXR : MonoBehaviour
{

    [SerializeField] Transform xrHead;
    [SerializeField] Transform xrLeftHand;
    [SerializeField] Transform xrRightHand;

    [SerializeField] Transform titanHead;
    [SerializeField] Transform titanRightHand;
    [SerializeField] Transform titanLeftHand;
    [SerializeField] Transform titanHip;

    public void BindBones(Transform head, Transform leftHand, Transform rightHand)
    {
        this.xrHead = head;
        this.xrLeftHand = leftHand;
        this.xrRightHand = rightHand;
    }
    private void Update()
    {
        titanHip.position = xrHead.position;
        titanHead.rotation = xrHead.rotation;
        titanLeftHand.position = xrLeftHand.position;
        titanRightHand.position = xrRightHand.position;


    }
}
