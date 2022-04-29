using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashCharactersRandom : MonoBehaviour
{
    private InverseKinematic[] characters;
    private GameObject characterSelected;


    private void Start()
    {
        characters = GetComponentsInChildren<InverseKinematic>();

        int count = characters.Length;
        for (int i = 0; i < count; i++)
        {
            characters[i].gameObject.SetActive(false);
        }
        characterSelected = characters[Random.Range(0, count)].gameObject;
        characterSelected.SetActive(true);
    }
}

