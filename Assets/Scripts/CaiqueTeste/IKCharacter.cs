using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKCharacter : MonoBehaviour
{
    Animator anim;
    ScriptPersonagem personagem;

    //mão esquerda
    public Transform leftHandTarget;
    public float leftHandIKWeight;
    public float leftHandRotWeight;

    //mão direita
    public Transform rightHandTarget;
    public float rightHandIKWeight;
    public float rightHandRotWeight;

    //cotovelo esquerdo
    public Transform leftElbowTarget;
    public float leftElbowIKWeight;

    //cotovelo direito
    public Transform rightElbowTarget;
    public float rightElbowIKWeight;

    public bool isIKActive;

    public LayerMask layer;

    float distance = 1f;

    RaycastHit rightHit;
    RaycastHit leftHit;

    public Vector3 rayPosicion;
    public float verticalOffset;
    public float horizontalOffset;
    public Vector3 targetVerticalOffset;
    //public Vector3 rightOffset;

    private void Start()
    {
        anim = GetComponent<Animator>();
        personagem = GetComponent<ScriptPersonagem>();
    }
    private void FixedUpdate()
    {

        CheckedNearWall();

    }

    private void Update()
    {/*
        if (personagem.box != null && personagem._characterState.ToString() == "push")
        {
            isIKActive = true;
        }
        else
            isIKActive = false;*/
    }

    void CheckedNearWall()
    {
        if (isIKActive)
        {
            //if (Physics.Raycast(leftHandTarget.position, leftHandTarget.forward, out leftHit, distance, layer.value))
            //if (Physics.Raycast(transform.position, transform.forward + targetVerticalOffset, out leftHit, distance, layer.value))
            if (Physics.Raycast(transform.position + Vector3.up * verticalOffset, transform.forward - transform.right, out leftHit, distance, layer.value))
            {
                leftHandTarget.position = leftHit.point - transform.forward * 0.02f;
                leftHandIKWeight = Mathf.Lerp(leftHandIKWeight, 1, Time.fixedDeltaTime * 3);
                leftHandRotWeight = Mathf.Lerp(leftHandIKWeight, 1, Time.fixedDeltaTime * 3);
                leftHandTarget.rotation = Quaternion.LookRotation(new Vector3(0, 30, 1), leftHit.normal);
                leftElbowIKWeight = Mathf.Lerp(leftElbowIKWeight, 1, Time.fixedDeltaTime * 3);
                rightElbowTarget.position = new Vector3(0.5f, 1, -0.5f);

                print(leftHit.collider.name);
            }
            else
            {
                leftHandIKWeight = Mathf.Lerp(leftHandIKWeight, 0, Time.fixedDeltaTime * 5);
                leftHandRotWeight = Mathf.Lerp(leftHandRotWeight, 0, Time.fixedDeltaTime * 5);
                leftElbowIKWeight = Mathf.Lerp(leftHandRotWeight, 0, Time.fixedDeltaTime * 5);
            }

            if (Physics.Raycast(transform.position + Vector3.up * verticalOffset, transform.forward + transform.right, out rightHit, distance, layer.value))
            {
                rightHandTarget.position = rightHit.point - transform.forward * 0.02f;
                rightHandIKWeight = Mathf.Lerp(rightHandIKWeight, 1, Time.fixedDeltaTime * 3);
                rightHandRotWeight = Mathf.Lerp(rightHandIKWeight, 1, Time.fixedDeltaTime * 3);
                rightHandTarget.rotation = Quaternion.LookRotation(new Vector3(0, 30, 1), rightHit.normal);
                rightElbowIKWeight = Mathf.Lerp(rightElbowIKWeight, 1, Time.fixedDeltaTime * 3); ;
                rightElbowTarget.position = new Vector3(0.5f, 1, -0.5f);
            }
            else
            {
                rightHandIKWeight = Mathf.Lerp(rightHandIKWeight, 0, Time.fixedDeltaTime * 5);
                rightHandRotWeight = Mathf.Lerp(rightHandRotWeight, 0, Time.fixedDeltaTime * 5);
                rightElbowIKWeight = Mathf.Lerp(rightHandRotWeight, 0, Time.fixedDeltaTime * 5);
            }

        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isIKActive)
        {
            //Braço esquerdo posição e peso
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);

            //rotação e peso
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandRotWeight);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);

            //cotovelo posição
            anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, leftElbowIKWeight);
            anim.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowTarget.position);

            //Braço direito posição e peso
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandIKWeight);
            anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);

            //rotação e peso
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandRotWeight);
            anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);

            //cotovelo posição
            anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightElbowIKWeight);
            anim.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowTarget.position);

        }
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position - Vector3.up * verticalOffset, transform.forward * distance);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + Vector3.up * verticalOffset, transform.forward * distance);
    }



}
