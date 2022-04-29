using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGameStudio;

public class CharacterContactController : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameManager.instance.GetActiveController().playerController;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tags.FAILPLANE))
        {
            GameManager.instance.GetActiveController().IsFail();
        }

        if (other.CompareTag(Constants.Tags.OBSTACLE))
        {
            GameManager.instance.GetParticleController()
                .InstantiateDead(transform.root);
            GameManager.instance.GetActiveController().IsFail();
            GameManager.instance.GetActiveController().cameraController
                .ShakeCamera(Constants.CamerasType.FollowCam, .6f);
        }

        if (other.CompareTag(Constants.Tags.PARQUETPATH) ||
            other.CompareTag(Constants.Tags.PATHEND))
        {
            if (!playerController.isFinish)
            {
                other.GetComponent<MeshRenderer>().enabled = false;
                //StartCoroutine(WaitForDestroyParquet(other));
                GameManager.instance.GetParticleController()
                    .InstantiatePaequetBroke(other.ClosestPoint(transform.position));
            }
        }

        if (other.CompareTag(Constants.Tags.LENS))
        {
            int amount = other.GetComponent<Lens>().amount;

            GameManager.instance.GetParticleController()
                .InstantiateLens(other.transform.position, amount);

            if (amount > 0)
                GameManager.instance.GetPlayerStack()
                    .AddToStack(amount);
            else
                GameManager.instance.GetPlayerStack()
                    .RemoveFromStack(amount);

            Destroy(other.gameObject);
        }

        if (other.CompareTag(Constants.Tags.FINISHPROVISION))
        {
            GameManager.instance.GetPathPlot().isCanDraw = false;
        }

        if (other.CompareTag(Constants.Tags.FINISH))
        {
            GameManager.instance.GetActiveController().OnFinalCollision();
            GameManager.instance.GetPathPlot().ManuelPathCreator();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constants.Tags.FINISH))
        {
            if (GameManager.instance.GetPlayerStack().GetStackCount <= 0)
            {
                playerController.isCanMove = true;
                playerController.ManuelChangingEnd(5);

                other.GetComponent<Collider>().enabled = false;

                GameManager.instance.GetActiveController().cameraController.
                    SetPriorityTo(Constants.CamerasType.ActionCam);
            }
            else
            {
                playerController.ManuelControlOfspeed(13.5f, 30);
            }
        }
    }

    private IEnumerator WaitForDestroyParquet(Collider other)
    {
        yield return new WaitForSeconds(.1f);
        other.transform.parent.gameObject.SetActive(false);
    }
}
