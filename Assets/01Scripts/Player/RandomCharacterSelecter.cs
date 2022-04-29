using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCharacterSelecter : MonoBehaviour
{
    private PlayerController playerController;
    private InverseKinematic[] characters;
    private GameObject characterSelected;

    [SerializeField] private int selectedCharacterIndex = -1;

    private void Awake()
    {
        characters = FindObjectsOfType<InverseKinematic>();

        int count = characters.Length;
        for (int i = 0; i < count; i++)
        {
            characters[i].gameObject.SetActive(false);
        }

        if (selectedCharacterIndex == -1)
        {
            characterSelected = characters[Random.Range(0, count)].gameObject;
        }
        else
        {
            characterSelected = characters[selectedCharacterIndex].gameObject;
        }

        characterSelected.SetActive(true);

        playerController = CGameStudio.GameManager.instance.GetActiveController().playerController;
        playerController.playerAnimatorController = GetComponent<PlayerAnimatorController>();

        playerController.playerAnimatorController.characterAnimatorController = characterSelected.GetComponent<Animator>();
        playerController.stackFollowPoint = characterSelected.GetComponentInChildren<StackFollowPoint>().transform;
        playerController.characterSelected = characterSelected;
    }
}
