using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateContactController : MonoBehaviour
{
    ParticleController particleController;

    private void Start()
    {
        particleController = CGameStudio.GameManager.instance.GetParticleController();
    }

    private void FixedUpdate()
    {
        if (Physics.CapsuleCast(transform.position + Vector3.forward*.8f,
            transform.position - Vector3.forward * .8f, .2f, transform.forward, out RaycastHit hit,
            0.15f, 1 << LayerMask.NameToLayer(Constants.Layers.PARQUET)))
        {
            particleController.InstantiateSparkleParticle(hit.point + Vector3.up * .1f);
        }
        if (Physics.CapsuleCast(transform.position + Vector3.forward * .8f,
        transform.position - Vector3.forward * .8f, .2f, transform.forward, out RaycastHit hit1,
        0.15f, 1 << LayerMask.NameToLayer(Constants.Layers.PLATFORM)))
        {
            particleController.InstantiateSparkleParticle(hit1.point + Vector3.up * .1f);
        }

    }
}
