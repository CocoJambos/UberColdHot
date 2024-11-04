using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.CompareTag(Tags.BasicBlock.ToString()))
        {
            hit.collider.GetComponent<BlockDisappearing>().TryToStartDisappearingOnPlayerTouch();
        }
    }
}
