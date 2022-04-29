using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematic : MonoBehaviour
{
    [SerializeField] Transform leftFootSkoet = default;
    [SerializeField] Transform rightFootSoket = default;

    private Animator animator;
    private bool kinematicToRightFoot = true;
    private bool kinematicToLeftFoot = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSoket(Transform leftHand, Transform rightHand)
    {
        leftFootSkoet = leftHand;
        rightFootSoket = rightHand;
    }

    private void IKFoot()
    {
        if (leftFootSkoet == null || rightFootSoket == null)
            return;

        if (kinematicToRightFoot)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IK"));
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IK"));
            animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootSoket.position);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.Euler(rightFootSoket.rotation.eulerAngles));
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
        }

        if (kinematicToLeftFoot)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IK"));
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IK"));
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootSkoet.position);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.Euler(leftFootSkoet.rotation.eulerAngles));
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
        }
    }

    public void CancelRightFootKinematic()
    {
        kinematicToRightFoot = false;
        Invoke(nameof(StartRightFootKinematic), .7f);
    }

    public void CancelLeftFootKinematic()
    {
        kinematicToLeftFoot = false;
        Invoke(nameof(StartLeftFootKinematic), 1.1f);
    }

    public void StartRightFootKinematic()
    {
        kinematicToRightFoot = true;
    }

    public void StartLeftFootKinematic()
    {
        kinematicToLeftFoot = true;
    }


    private void OnAnimatorIK(int layerIndex)
    {
        IKFoot();
    }
}
