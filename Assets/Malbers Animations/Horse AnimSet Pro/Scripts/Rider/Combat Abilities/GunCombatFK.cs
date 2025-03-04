﻿using System;
using MalbersAnimations.Weapons;
using UnityEngine;

namespace MalbersAnimations.HAP
{
    [CreateAssetMenu(menuName = "Malbers Animations/HAP/Gun Combat FK")]
    public class GunCombatFK : GunCombat
    {

        public float AimHorizontalOffset = 20;                          //Adjusment for the Aim body Offet (to view better the hand)

        [Header("Right Offsets")]
        public Vector3 RightShoulderOffset = new Vector3(-90, 90, 0);
        public Vector3 RightHandOffset = new Vector3(-90, 90, 0);
        [Header("Right Offsets")]
        public Vector3 LeftShoulderOffset = new Vector3(90, 90, 0);
        public Vector3 LeftHandOffset = new Vector3(90, 90, 0);
        [Space]
        public Vector3 HeadOffset = new Vector3(0,-90,-90);
        [Range(0,1)]
        public float headLookWeight = 0.7f;

        protected Quaternion Delta_Rotation;


        public override void ActivateAbility()
        {
            base.ActivateAbility();
            EnableAimIKBehaviour(false);
        }

        public override void LateUpdateAbility()
        {
            FixAimPoseFK();
        }

        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// This will rotate the bones of the character to match the AIM direction 
        /// </summary>
        protected virtual void FixAimPoseFK()
        {
            if (RC.IsAiming)
            {
                // float Weight = RC.Active_MWeapon.RightHand ? FullLookAtAim.Evaluate(1 + RC.HorizontalAngle) :  FullLookAtAim.Evaluate(1 - RC.HorizontalAngle);

                if (RC.IsAiming && RC.WeaponAction != WeaponActions.ReloadRight && RC.WeaponAction != WeaponActions.ReloadLeft)
                {
                    Quaternion AimRotation;

                        AimRotation = Quaternion.LookRotation(RC.AimDirection, Vector3.up); //Get the Rotation ...
                  

                   Quaternion LookRotation = Quaternion.LookRotation(RC.AimDirection);
                    //AimRotation = Quaternion.LookRotation(RC.AimDirection, cam.up); //Get the Rotation ...


                    Vector3 ShoulderRotationAxis = Vector3.Cross(Vector3.up, RC.AimDirection).normalized; // cam.Right;

                    float angle = (Vector3.Angle(Vector3.up, RC.AimDirection) - 90);

                   
                    Debug.DrawRay(RC.Active_IMWeapon.RightHand?  RC.RightShoulder.position : RC.LeftShoulder.position, ShoulderRotationAxis, Color.green);

                    if (RC.Active_IMWeapon.RightHand)                                                                                                                //If the Weapon is RIGHT Handed  
                    {
                        RC.RightShoulder.RotateAround(RC.RightShoulder.position, ShoulderRotationAxis, angle);          //Rotate Up/Down the Right Shoulder to AIM Up/Down
                        RC.RightShoulder.rotation *= Quaternion.Euler(RightShoulderOffset);

                        if (!RC.Target)
                        {
                            RC.RightShoulder.RotateAround(RC.RightShoulder.position, Vector3.up, (RC.IsCamRightSide ? 0 : -AimHorizontalOffset));                   //Offset the RIGHT Arm for better view
                        }
                    }
                    else                                                                                                                                            //If the Weapon is LEFT Handed  
                    {
                        RC.LeftShoulder.RotateAround(RC.LeftShoulder.position, ShoulderRotationAxis, angle);                                                        //Rotate Up/Down the Left Shoulder to AIM Up/Down

                        RC.LeftShoulder.rotation *= Quaternion.Euler(LeftShoulderOffset);

                        if (!RC.Target)
                        {
                            RC.LeftShoulder.RotateAround(RC.LeftShoulder.position, Vector3.up, (RC.IsCamRightSide ? AimHorizontalOffset : 0));                      //Offset the LEFT Arm for better view  
                        }
                    }


                    RC.Head.rotation = Quaternion.Slerp(RC.Head.rotation, LookRotation * Quaternion.Euler(HeadOffset), headLookWeight);                             //Head Look Rotation

                    if (RC.WeaponAction != WeaponActions.Fire_Proyectile)                                                                                           //Activate the Hand AIM DIRECTION  when is not Firing or Reloading
                    {
                        if (RC.Active_IMWeapon.RightHand)
                        {
                            RC.RightHand.rotation = Delta_Rotation * Quaternion.Euler(RightHandOffset);
                        }
                        else
                        {
                            RC.LeftHand.rotation = Delta_Rotation * Quaternion.Euler(LeftHandOffset);
                        }

                        Delta_Rotation = Quaternion.Lerp(Delta_Rotation, AimRotation, Time.deltaTime * 20);                                                                 //Smoothly AIM the Hand
                    }
                }
            }
        }
    }
}