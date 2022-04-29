
/**
 * Copyright (c) 2022-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using System.Collections;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem lensPositif, lensNegatif;
    [SerializeField] private ParticleSystem finish;
    [SerializeField] private ParticleSystem dead;
    [SerializeField] private ParticleSystem sparkle;
    [SerializeField] private ParticleSystem collect;
    [SerializeField] private ParticleSystem ParquetBroke;

    private ParticleSystem ParquetBrokeClone;
    private ParticleSystem collectClone;
    private ParticleSystem sparkleClone;
    private ParticleSystem deadClone;
    private ParticleSystem lensPositifClone, lensNegatifClone;

    private bool finishParticlePlayed = false;

    private void Start()
    {
        collectClone = Instantiate(collect);
        ParquetBrokeClone = Instantiate(ParquetBroke);
        sparkleClone = Instantiate(sparkle);
        deadClone = Instantiate(dead);
        lensPositifClone = Instantiate(lensPositif);
        lensNegatifClone = Instantiate(lensNegatif);
    }
    public void InstantiateCollectParticle(Vector3 pos)
    {
        collectClone.transform.position = pos;
        collectClone.Play();
    }

    public void InstantiateCollectParticle(Transform stack)
    {
        if (collectClone == null)
            collectClone = Instantiate(collect);

        collectClone.transform.SetParent(stack);
        collectClone.transform.localPosition = Vector3.zero;
        collectClone.Play();
    }

    public void InstantiateSparkleParticle(Vector3 pos)
    {
        sparkleClone.transform.position = pos;
        sparkleClone.Play();
    }

    public void InstantiatePaequetBroke(Vector3 pos)
    {
        ParquetBrokeClone.transform.position = pos;
        ParquetBrokeClone.Play();
    }
    public void InstantiateDead(Transform pos)
    {
        deadClone.transform.SetParent(pos);
        deadClone.transform.localPosition = Vector3.up * 1.5f;
        deadClone.Play();
    }

    public void InstantiateLens(Vector3 pos, int value)
    {
        ParticleSystem particleToPlay = value < 0 ? lensNegatifClone : lensPositifClone;
        particleToPlay.transform.position = pos;
        particleToPlay.Play();
    }

    public void InstantiateFinishParticle(Vector3 right, Vector3 left)
    {
        if (finishParticlePlayed)
            return;

        StartCoroutine(finishParticleSpawn(right, left));

        finishParticlePlayed = true;
    }

    private IEnumerator finishParticleSpawn(Vector3 right, Vector3 left)
    {
        yield return new WaitForSeconds(.65f);
        ParticleSystem rightClone = Instantiate(finish);
        ParticleSystem leftClone = Instantiate(finish);

        rightClone.transform.position = right;
        leftClone.transform.position = left;

        rightClone.Play();
        leftClone.Play();
    }
}
