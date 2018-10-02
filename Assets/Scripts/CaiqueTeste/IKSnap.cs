using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSnap : MonoBehaviour
{
    public bool useIK;

    public Vector3 leftHandTarget;
    public Vector3 rightHandTarget;

    public Vector3 leftHandOffset;
    public Vector3 rightHandOffset;

    public Quaternion leftHandRot;
    public Quaternion rightHandRot;

    public Vector3 leftFootPos;
    public Vector3 rightFootPos;

    public Vector3 leftHandOriginalPos;
    public Vector3 rightHandOriginalPos;

    public Vector3 leftFootOffset;
    public Vector3 rightFootOffset;

    public Quaternion leftFootRot;
    public Quaternion rightFootRot;

    public Quaternion leftFootRotOffset;
    public Quaternion rightFootRotOffset;

    public bool leftHandIK;
    public bool rightHandIK;

    public bool leftFootIK;
    public bool rightFootIK;

    Animator anim;

    RaycastHit LHit;
    RaycastHit RHit;

    RaycastHit LFHit;
    RaycastHit RFHit;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //LeftHandIKTest
        if (Physics.Raycast(transform.position + new Vector3(0.0f, 2.0f, 0.5f), -transform.up + new Vector3(-0.5f, 0.0f, 0.0f), out LHit, 1f))
        {
            leftHandIK = true;
            leftHandTarget = LHit.point - leftHandOffset;
            leftHandTarget.x = leftHandOriginalPos.x;
            //leftHandTarget.z = leftFootPos.z - leftHandOffset.z;
            leftHandRot = Quaternion.FromToRotation(Vector3.forward, LHit.normal);
        }
        else
            leftHandIK = false;

        //RightHandIKTest
        if (Physics.Raycast(transform.position + new Vector3(0.0f, 2.0f, 0.5f), -transform.up + new Vector3(0.5f, 0.0f, 0.0f), out RHit, 1f))
        {
            rightHandIK = true;
            rightHandTarget = RHit.point - rightHandOffset;
            rightHandRot = Quaternion.FromToRotation(Vector3.forward, RHit.normal);
        }
        else
            rightHandIK = false;

        if (Physics.Raycast(transform.position + new Vector3(-0.3f, 0.50f, 0.0f), transform.forward, out LFHit, 1f))
        {
            leftFootIK = true;
            leftFootPos = LFHit.point - leftFootOffset;
            leftFootRot = Quaternion.FromToRotation(Vector3.up, LFHit.normal) * leftFootRotOffset;
        }
        else leftFootIK = false;

        if (Physics.Raycast(transform.position + new Vector3(0.3f, 0.50f, 0.0f), transform.forward, out RFHit, 1f))
        {
            rightFootIK = true;
            rightFootPos = RFHit.point - rightFootOffset;
            rightFootRot = Quaternion.FromToRotation(Vector3.up, RFHit.normal) * rightFootRotOffset;


        }
        else rightFootIK = false;


    }

    private void Update()
    {
        //Mãos
        Debug.DrawRay(transform.position + new Vector3(0.0f, 2.0f, 0.5f), -transform.up + new Vector3(-0.5f, 0.0f, 0.0f), Color.yellow);
        Debug.DrawRay(transform.position + new Vector3(0.0f, 2.0f, 0.5f), -transform.up + new Vector3(0.5f, 0.0f, 0.0f), Color.grey);

        //Pés
        //Left
        Debug.DrawRay(transform.position + new Vector3(-0.3f, 0.50f, 0.0f), transform.forward, Color.red);

        //Right
        Debug.DrawRay(transform.position + new Vector3(0.3f, 0.50f, 0.0f), transform.forward, Color.red);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (useIK)
        {
            leftHandOriginalPos = anim.GetIKPosition(AvatarIKGoal.LeftHand);
            rightHandOriginalPos = anim.GetIKPosition(AvatarIKGoal.RightHand);

            if (leftHandIK)
            {
                //Mão Esquerda
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget);

                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRot);
            }

            if (rightHandIK)
            {
                //Mão Direita
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget);

                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
                anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandRot);
            }

            if (leftFootIK)
            {
                anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);

                anim.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRot);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            }

            if (rightFootIK)
            {
                anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);
                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);

                anim.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
            }

        }
    }
}

