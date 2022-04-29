using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Colors
{
    [Header("Platform")]
    public Material platform;

    [Header("Stack")]
    public Color stackColor;
    public Color stackFusingColor;
    public bool autoStackColor;

    [Header("Collectible")]
    public Material collectible;

    [Header("Road")]
    public Gradient roadDrawColor;
}

public class ColorController : MonoBehaviour
{
    [SerializeField] private Colors[] colors;
    [HideInInspector] public Colors currentColor;

    private void Awake()
    {
        currentColor = colors[Random.Range(0, colors.Length)];

        if (currentColor.autoStackColor)
        {
            currentColor.stackColor = currentColor.collectible.color;
            currentColor.stackFusingColor = currentColor.collectible.color;
        }
    }
}
