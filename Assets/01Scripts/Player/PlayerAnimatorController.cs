using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [HideInInspector] public Animator characterAnimatorController;
    private Animator skateAnimatorController;
    private int characterTemp;

    private void Start()
    {
        //characterAnimatorController = GetComponentInChildren<Animator>();
        skateAnimatorController = GetComponent<Animator>();
    }

    public void SetCharacterAnimation(int value)
    {
        if (characterTemp == value)
            return;
        characterAnimatorController.SetInteger(Constants.Animations.CHARACTER_IDLE_ACTION, value);
        characterTemp = value;
    }

    public void SetSkateAnimator(int value)
    {
        skateAnimatorController.SetInteger(Constants.Animations.SKATE_ACTION, value);
    }
}
