using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectibleZone : MonoBehaviour
{

    [SerializeField] private int collectCount = 1;

    private PlayerStack playerStack;
    void Start()
    {
        playerStack = CGameStudio.GameManager.instance.GetPlayerStack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tags.COLLECTIBLE))
        {
            other.GetComponent<Collider>().enabled = false;

            if (playerStack.stackList.Count > 0)
                other.transform.SetParent(playerStack.stackList[0].parent);
            else
                other.transform.SetParent(transform);

            Vector3 pos = playerStack.stackList.Count == 0 ?
                Vector3.zero : playerStack.stackList[playerStack.stackList.Count - 1].localPosition;
            other.transform.DOLocalMove(pos, .35f).OnComplete(() =>
            {
                Destroy(other.gameObject);
                playerStack.AddToStack(collectCount);
            });
            if ((playerStack.stackList.Count >= 1))
                CGameStudio.GameManager.instance.GetParticleController()
                    .InstantiateCollectParticle(playerStack.stackList[playerStack.stackList.Count - 1]);
        }
    }
}
