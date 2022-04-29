using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lens : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] meshRenderer;
    [SerializeField] private TextMeshPro TextMeshPro;
    private Color color;
    public Color positifColor;
    public Color negatifColor;
    public int amount = 5;
    void Start()
    {
        color = amount < 0 ? negatifColor : positifColor;
        meshRenderer[0].material.color = color;
        meshRenderer[1].material.color = new Color(color.r, color.g, color.b, 0.5f);

        TextMeshPro.text = ((amount < 0) ? "" : "+") + amount;
    }
}
