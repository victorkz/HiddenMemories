using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK : MonoBehaviour
{
    Animator anim;


    public float Offset;
    public Transform pointIKRight;
    public Transform pointIKLeft;

    Vector3 footRightPosition;
    Vector3 footLeftPosition;

    Quaternion footRightRotation;
    Quaternion footLeftRotation;

    public bool UseIK;
    bool canUseIk;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (canUseIk)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);

            anim.SetIKPosition(AvatarIKGoal.RightFoot, footRightPosition + Vector3.up * Offset);
            anim.SetIKPosition(AvatarIKGoal.LeftFoot, footLeftPosition + Vector3.up * Offset);

            anim.SetIKRotation(AvatarIKGoal.RightFoot, footRightRotation);
            anim.SetIKRotation(AvatarIKGoal.LeftFoot, footLeftRotation);
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);

            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
        }
    }

    RaycastHit hit;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (UseIK && anim.GetFloat("SpeedPercent") == 0)
            canUseIk = true;
        else
            canUseIk = false;

        print(canUseIk);

        if (canUseIk)
        {
            if (Physics.Raycast(pointIKRight.position, Vector3.down, out hit))
            {
                footRightPosition = hit.point;
                footRightRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            }

            if (Physics.Raycast(pointIKLeft.position, Vector3.down, out hit))
            {
                footLeftPosition = hit.point;
                footLeftRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            }
        }
    }
}
