using UnityEngine;
using System.Collections;
using System;

namespace MalbersAnimations.HAP
{
    /// <summary>
    /// Used for Linking the Reins to the hand of the Rider
    /// </summary>
    [RequireComponent(typeof(Mountable))]
    public class IKReins : MonoBehaviour
    {
        public Transform ReinLeftHand, ReinRightHand;
        protected Vector3 LocalStride_L, LocalStride_R;
        protected Transform riderHand_L, riderHand_R;
        protected Mountable Montura;

        protected bool freeRightHand = true;
        protected bool freeLeftHand = true;

        private void Awake()
        {
            Montura = GetComponent<Mountable>();
        }

        void Start()
        {
            if (ReinLeftHand && ReinRightHand)
            {
                LocalStride_L = ReinLeftHand.localPosition;             //Set the Reins Local Values Values
                LocalStride_R = ReinRightHand.localPosition;            //Set the Reins Local Values Values
            }
            else
            {
                Debug.LogWarning("Some of the Reins has not been set on the inspector. Please fill the values");
            }
        }

        private void OnEnable()
        {
            Montura.OnMounted.AddListener(OnRiderMounted);
            Montura.OnDismounted.AddListener(OnRiderDismounted);
        }



        private void OnDisable()
        {
            Montura.OnMounted.RemoveListener(OnRiderMounted);
            Montura.OnDismounted.RemoveListener(OnRiderDismounted);
        }

        /// <summary>
        /// Checking if the Right hand is free
        /// </summary>
        public void RightHand_is_Free(bool value)
        {
            freeRightHand = value;
            if (!value && ReinRightHand)
            {
                ReinRightHand.localPosition = LocalStride_R;
            }
        }

        /// <summary>
        /// Checking if the Left hand is free
        /// </summary>
        public void LeftHand_is_Free(bool value)
        {
            freeLeftHand = value;
            if (!value && ReinLeftHand)
            {
                ReinLeftHand.localPosition = LocalStride_L;
            }
        }

        void OnRiderMounted()
        {
            Animator RiderAnim = Montura.ActiveRider.Anim;  //Get the Rider Animator
            riderHand_L = RiderAnim.GetBoneTransform(HumanBodyBones.LeftHand);
            riderHand_R = RiderAnim.GetBoneTransform(HumanBodyBones.RightHand);
        }

        private void OnRiderDismounted()
        {
            riderHand_L = null;
            riderHand_R = null;
        }


        void LateUpdate()
        {
            if (!ReinLeftHand || !ReinRightHand) return; //There's no Reins Reference


            if (Montura.ActiveRider && Montura.ActiveRider.IsRiding)
            {
                if (freeLeftHand)
                {
                    ReinLeftHand.position = Vector3.Lerp(riderHand_L.position, riderHand_L.GetChild(1).position, 0.5f);     //Put it in the middle o the left hand
                }
                else
                {
                    if (freeRightHand)
                        ReinLeftHand.position = Vector3.Lerp(riderHand_R.position, riderHand_R.GetChild(1).position, 0.5f); //if the right hand is holding a weapon put the right rein to the Right hand
                }

                if (freeRightHand)
                {
                    ReinRightHand.position = Vector3.Lerp(riderHand_R.position, riderHand_R.GetChild(1).position, 0.5f); //Put it in the middle o the RIGHT hand
                }
                else
                {
                    if (freeLeftHand)
                        ReinRightHand.position = Vector3.Lerp(riderHand_L.position, riderHand_L.GetChild(1).position, 0.5f); //if the right hand is holding a weapon put the right rein to the Left hand
                }
            }
            else
            {
                ReinLeftHand.localPosition = LocalStride_L;
                ReinRightHand.localPosition = LocalStride_R;
            }
        }
    }
}
