using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHand : MonoBehaviour
{
    #region Campo
    private Animator anim;

    //Pega mão esquerda
    public Transform leftHandTarget;
    public float leftHandWeight;
    public float leftHandRotWeight;

    //Pega cotovelo esquerdo
    public Transform leftElbowTarget;
    public float leftEbowWeight;

    //pega a mão direita
    public Transform rightHandTarget;
    public float rightHandWeight;
    public float rightHandRotWeight;

    //Pega cotovelo direto
    public Transform rightElbowTarget;
    public float rightEbowWeight;

    public bool isIKActive;

    public float distance;

    public float verticalOffset;

    private RaycastHit leftHit;
    private RaycastHit rightHit;

    public LayerMask layer;
    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        CheckNearWall();
    }

    private void CheckNearWall()
    {
        if (isIKActive)
        {
            if (Physics.Raycast(transform.position + Vector3.up * verticalOffset, transform.right, out rightHit, distance, layer.value))
            {

                rightHandTarget.position = rightHit.point - transform.right * 0.04f;
                rightHandWeight = Mathf.Lerp(rightHandWeight, 1, Time.fixedDeltaTime * 5);
                rightHandRotWeight = Mathf.Lerp(rightHandRotWeight, 1, Time.fixedDeltaTime * 5);
                rightHandTarget.rotation = Quaternion.LookRotation(transform.up + transform.forward, rightHit.normal);
                rightEbowWeight = Mathf.Lerp(rightEbowWeight, 1, Time.fixedDeltaTime * 5);
                rightElbowTarget.position = transform.position + transform.right * 0.5f + transform.up + transform.forward * -0.5f;
                //rightElbowTarget.position = transform.position + new Vector3(0.5f, 1, -0.5f);
            }
            else
            {
                rightHandWeight = Mathf.Lerp(rightHandWeight, 0, Time.fixedDeltaTime * 5);
                rightHandRotWeight = Mathf.Lerp(rightHandRotWeight, 0, Time.fixedDeltaTime * 5);
                rightEbowWeight = Mathf.Lerp(rightEbowWeight, 0, Time.fixedDeltaTime * 5);
            }

            if (Physics.Raycast(transform.position + Vector3.up * verticalOffset, -transform.right, out leftHit, distance, layer.value))
            {
                leftHandTarget.position = leftHit.point + transform.right * 0.04f;
                leftHandWeight = Mathf.Lerp(leftHandWeight, 1, Time.fixedDeltaTime * 5);
                leftHandRotWeight = Mathf.Lerp(leftHandRotWeight, 1, Time.fixedDeltaTime * 5);
                leftHandTarget.rotation = Quaternion.LookRotation(transform.up + transform.forward, leftHit.normal);
                leftEbowWeight = Mathf.Lerp(rightEbowWeight, 1, Time.fixedDeltaTime * 5);
                leftElbowTarget.position = transform.position + transform.right * -0.5f + transform.up + transform.forward * -0.5f;
            }
            else
            {
                leftHandWeight = Mathf.Lerp(leftHandWeight, 0, Time.fixedDeltaTime * 5);
                leftHandRotWeight = Mathf.Lerp(leftHandRotWeight, 0, Time.fixedDeltaTime * 5);
                leftEbowWeight = Mathf.Lerp(rightEbowWeight, 0, Time.fixedDeltaTime * 5);
            }
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isIKActive)
        {
            //Esquerdo
            //posição e peso
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);

            //cotovelo posição
            anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, leftEbowWeight);
            anim.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowTarget.position);

            //rotação e peso
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandRotWeight);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);


            //Direto
            //posição e peso
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
            anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);

            //cotovelo posição
            anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightEbowWeight);
            anim.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowTarget.position);

            //rotação e peso
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandRotWeight);
            anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.up * verticalOffset, transform.position + Vector3.up * verticalOffset + transform.right * distance);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * verticalOffset, transform.position + Vector3.up * verticalOffset + -transform.right * distance);
    }
}
