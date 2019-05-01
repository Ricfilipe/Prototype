using UnityEngine;

using System.Collections;

namespace MalbersAnimations.HAP
{
    public class DismountBehavior : StateMachineBehaviour
    {
        Rider3rdPerson rider;
        /// <summary>
        /// Positions of the feet
        /// </summary>

        Vector3 HipPosition;        //Hip Last Postition
        Vector3 BottomPosition;     //Feets Last Postition

        Transform transform;
        Transform MountPoint;

        Vector3 LastRelativeRiderPosition;

        TransformAnimation Fix;

        float ScaleFactor;
        private Transform LeftFoot;
        private Transform RightFoot;

        IAnimatorBehaviour animatorListener;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //animatorListener = animator.GetComponent<IAnimatorBehaviour>();
            //if (animatorListener != null) animatorListener.OnStateEnter(Hash.Tag_Unmounting, stateInfo, layerIndex);

            #region OldWay
            animator.SetInteger(Hash.MountSide, 0);                 //remove the side of the mounted **IMPORTANT*** otherwise it will keep trying to dismount

            rider = animator.GetComponent<Rider3rdPerson>();
            ScaleFactor = rider.Montura.Animal.ScaleFactor;                                     //Get the scale Factor from the Montura

            LeftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            RightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);

            MountPoint = rider.Montura.MountPoint;

            BottomPosition = MountPoint.InverseTransformPoint((LeftFoot.position + RightFoot.position) / 2); //Convert it to Local Space
            HipPosition = MountPoint.InverseTransformPoint(animator.rootPosition);         //Convert it to Local Space

            Fix = rider.MountTrigger.Adjustment;            //Store the Fix

            transform = animator.transform;
            rider.Start_Dismounting();

            transform.position = rider.Montura.MountPoint.position;
            transform.rotation = rider.Montura.MountPoint.rotation;

            LastRelativeRiderPosition = MountPoint.InverseTransformPoint(transform.position);
            #endregion
        }


        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //if (animatorListener != null) animatorListener.OnStateExit(Hash.Tag_Unmounting, stateInfo, layerIndex);
            rider.End_Dismounting();
        }

        override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //if (animatorListener != null) animatorListener.OnStateMove(Hash.Tag_Unmounting, stateInfo, layerIndex);

            #region OldWay
            var transition = animator.GetAnimatorTransitionInfo(layerIndex);

            float transitionTime = transition.normalizedTime;
            float time = Time.deltaTime;


            transform.rotation = animator.rootRotation;
            transform.position = MountPoint.TransformPoint(LastRelativeRiderPosition); //Parent Position without Parenting

            //Smoothly move the center of mass to the desired position in the first Transition
            if (animator.IsInTransition(layerIndex) && stateInfo.normalizedTime < 0.5f)
            {

                //BottomPosition = MountPoint.InverseTransformPoint((LeftFoot.position + RightFoot.position) / 2); //Convert it to Local Space
                //HipPosition = MountPoint.InverseTransformPoint(animator.rootPosition);                          //Convert it to Local Space

                var newPos = MountPoint.position;
                newPos.y = MountPoint.TransformPoint(Vector3.Lerp(HipPosition, BottomPosition, transitionTime)).y;

                transform.position = newPos;


                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation, transitionTime);
            }
            else
            {
                transform.position += (animator.velocity * Time.deltaTime * ScaleFactor * (Fix ? Fix.delay : 1));
            }

            //Stop the Mountura from walking forward when Dismounting
            if (rider.Montura)
            {
                rider.Montura.Animal.MovementAxis = rider.Montura.Animal.MovementAxis * (1 - stateInfo.normalizedTime);

                //Don't go under the floor
                if (rider.MountTrigger)
                {
                    if (transform.position.y < rider.MountTrigger.transform.position.y)
                    {
                        transform.position = new Vector3(transform.position.x, rider.MountTrigger.transform.position.y, transform.position.z);
                    }
                }

                if (stateInfo.normalizedTime > 0.8f)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation, transitionTime);
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, rider.MountTrigger.transform.position.y, transform.position.z), time * 5f);
                }
            }

            animator.rootPosition = transform.position;

            LastRelativeRiderPosition = MountPoint.InverseTransformPoint(transform.position);
            #endregion
        }
    }
}