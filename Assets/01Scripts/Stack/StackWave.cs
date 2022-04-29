using CGameStudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackWave : MonoBehaviour
{
    public Transform target;
    public int ordre;

    private PlayerController playerController;
    private Vector3 playerPosTemp;
    private PlayerStack playerStack;
    private float playerVelocity;

    public bool isContacted = false;
    private void Start()
    {
        playerController = GameManager.instance.GetActiveController().playerController;
        playerStack = transform.parent.GetComponent<PlayerStack>();
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        FollowAxisY();
        FollowAxisX();
        FollowRotationY();
    }

    private void FollowAxisX()
    {
        Vector3 targetX = new Vector3(target.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetX, Time.deltaTime * 15);
    }

    private void FollowAxisY()
    {
        Vector3 playerPos = playerController.transform.position;
        playerVelocity = Mathf.Lerp(playerVelocity, Vector3.Distance(playerPosTemp, playerPos), Time.deltaTime * 0.1f);

        Vector3 targetWorldPos = new Vector3(transform.position.x, target.position.y + .1f, transform.position.z);
        Vector3 targetY = transform.parent.InverseTransformPoint(targetWorldPos);

        if (playerPos.y - playerPosTemp.y > 0.05f)
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetY, 10 * Time.deltaTime);
        else
            transform.localPosition = Vector3.Slerp(transform.localPosition,
                targetY,
                Time.deltaTime * 100 * (1 - Mathf.InverseLerp(ordre, 0, playerStack.GetStackCount)) * Mathf.Clamp(playerVelocity, 0.3f, 1));

        playerPosTemp = playerPos;
    }

    private void FollowRotationY()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            target.transform.eulerAngles.y,
            transform.rotation.eulerAngles.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tags.PLATFORM))
        {
            playerStack.RemoveFromStack(transform);
            isContacted = true;
        }
        if (other.CompareTag(Constants.Tags.OBSTACLE))
        {
            playerStack.RemoveFromStack(transform);
            isContacted = true;
        }
    }
}
