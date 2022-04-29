using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGameStudio;

public class ColorTaker : MonoBehaviour
{
    [SerializeField]
    private enum Object
    {
        Platform, Collectible, ParquetBrokeParticle
    }
    [SerializeField] private Object objectType;

    void Start()
    {
        switch (objectType)
        {
            case Object.Platform:
                GetComponent<MeshRenderer>().material = GameManager.instance.GetActiveController().GetColorController().currentColor.platform;
                break;

            case Object.Collectible:
                GetComponent<MeshRenderer>().material = GameManager.instance.GetActiveController().GetColorController().currentColor.collectible;
            
                // transform.GetChild(0).GetComponent<TrailRenderer>().colorGradient.SetKeys(
                //             new GradientColorKey[] {
                //             new GradientColorKey(GameManager.instance.GetActiveController().GetColorController().currentColor.collectible.color, 1.0f),
                //             new GradientColorKey(Color.white, 1.0f) },
                //             new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
                //         );
            
                break;

            case Object.ParquetBrokeParticle:
                var main = GetComponent<ParticleSystem>().main;
                main.startColor = GameManager.instance.GetActiveController().GetColorController().currentColor.platform.color;
                break;
        }
    }
}