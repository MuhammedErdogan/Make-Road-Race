using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGameStudio;

public class SetGradientColor : MonoBehaviour
{
    [SerializeField] private Gradient mainGradient;
    public Gradient levelGradient;
    [SerializeField] public Gradient FinishGradient;

    private void Start()
    {
        levelGradient = GameManager.instance.GetActiveController().GetColorController().currentColor.roadDrawColor;
        mainGradient = levelGradient;
    }

    public Color GetColor(float c)
    {
        c = Mathf.Clamp01(c);
        return mainGradient.Evaluate(c);
    }

    public void SetFinishGradient()
    {
        mainGradient = FinishGradient;
    }
}
