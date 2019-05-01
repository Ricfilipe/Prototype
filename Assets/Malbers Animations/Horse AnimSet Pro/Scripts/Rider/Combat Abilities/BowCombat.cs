using System;
using MalbersAnimations.Weapons;
using UnityEngine;

namespace MalbersAnimations.HAP
{
    /// <summary>
    /// Ability that it will Manage the Bow Combat System while Riding
    /// </summary>
    [CreateAssetMenu(menuName = "Malbers Animations/HAP/Bow Combat")]
    public class BowCombat : RiderCombatAbility
    {
        bool isHolding;             //for checking if the Rider is Holding/Tensing the String
        float HoldTime;             //Time pass since the Rider started tensing the string

        private static Keyframe[] KeyFrames =
            { new Keyframe(0, 1), new Keyframe(1.25f, 1), new Keyframe(1.5f, 0), new Keyframe(2f, 0) };

        [Header("Right Handed Bow Offsets")]
        public Vector3 ChestRight = new Vector3(25, 0, 0);
        public Vector3 ShoulderRight = new Vector3(5, 0, 0);
        public Vector3 HandRight;

        [Header("Left Handed Bow Offsets")]
        public Vector3 ChestLeft = new Vector3(-25, 0, 0);
        public Vector3 ShoulderLeft = new Vector3(-5, 0, 0);
        public Vector3 HandLeft;


        [Space]
        [Tooltip("This Curve is for straightening the aiming Arm while is on the Aiming State")]
        public AnimationCurve AimWeight = new AnimationCurve(KeyFrames);

        protected bool KnotToHand;
        protected Quaternion Delta_Hand;
        private IBow Bow;

        public override bool TypeOfAbility(IMWeapon weapon)
        {
            return weapon is IBow;
        }

        public override WeaponType Type { get { return MalbersAnimations.WeaponType.Bow; } }


        public override WeaponType WeaponType()
        {
            return MalbersAnimations.WeaponType.Bow;
        }

        public override void StartAbility(RiderCombat ridercombat)
        {
            base.StartAbility(ridercombat);
            KnotToHand = false;
        }

        public override void ActivateAbility()
        {
            Bow = RC.Active_IMWeapon as IBow;   //Store the Bow
        }

        public override void UpdateAbility()
        {
            //Important !! Reset the bow state if is holding but is not aiming
            if (!RC.IsAiming && isHolding)
            {
                isHolding = false;
                HoldTime = 0;
                RC.Anim.SetFloat(Hash.IDFloat, 0);            //Reset Hold Animator Values
                Bow.BendBow(0);
            }

            BowKnotInHand(); //Update the BowKnot in the Hand if is not Firing
        }

        public override void PrimaryAttack()
        {
            BowAttack();       //Try Firing the Bow
        }

        public override void PrimaryAttackReleased()
        {
            ReleaseArrow();   //Try Releasing the Arrow
        }

        public override void LateUpdateAbility()
        {
            FixAimPoseBow();
        }

        /// <summary>
        /// Bow Attack Mode
        /// </summary>
        protected virtual void BowAttack()
        {
            if (RC.IsAiming && RC.WeaponAction != WeaponActions.Fire_Proyectile)                                            //Shoot arrows only when is aiming and If we are notalready firing any arrow
            {
                bool isInRange = RC.Active_IMWeapon.RightHand ? RC.HorizontalAngle < 0.5f : RC.HorizontalAngle > -0.5f; //Calculate the Imposible range to shoot

                if (!isInRange)
                {
                    isHolding = false;
                    HoldTime = 0;
                    return;
                }

                if (!isHolding)            //If Attack is pressed Start Bending for more Strength the Bow
                {
                    RC.SetAction(WeaponActions.Hold);
                    isHolding = true;
                    HoldTime = 0;
                }
                else             // //If Attack is pressed Continue Bending the Bow for more Strength the Bow
                {
                    HoldBow();
                }
            }
        }

        /// <summary>
        /// If Attack is Released Go to next Action and release the Proyectile
        /// </summary>
        private void ReleaseArrow()
        {
            if (RC.IsAiming && RC.WeaponAction != WeaponActions.Fire_Proyectile && isHolding)       //If we are not firing any arrow then try to Attack with the bow
            {
                var Knot = Bow.KNot;
                Knot.rotation = Quaternion.LookRotation(RC.AimDirection);                           //Aligns the Knot and Arrow to the AIM DIRECTION before Releasing the Arrow

                RC.SetAction(WeaponActions.Fire_Proyectile);              //Go to Action FireProyectile
                isHolding = false;
                HoldTime = 0;
                RC.Anim.SetFloat(Hash.IDFloat, 0);                        //Reset Hold Animator Values

                Bow.ReleaseArrow(RC.AimDirection);

                Bow.BendBow(0);

                RC.OnAttack.Invoke(RC.Active_IMWeapon);                 //Invoke the On Attack Event
            }
        }

        /// <summary>
        /// If Attack is pressed Continue Bending the Bow for more Strength the Bow
        /// </summary>
        private void HoldBow()
        {
            HoldTime += Time.deltaTime;

            if (HoldTime <= Bow.HoldTime + Time.deltaTime)
                Bow.BendBow(HoldTime / Bow.HoldTime);    //Bend the Bow

            RC.Anim.SetFloat(Hash.IDFloat, HoldTime / Bow.HoldTime);
        }

        /// <summary>
        /// This is Called by the Animator
        /// </summary>
        public virtual void EquipArrow()
        {
            Bow.EquipArrow();
        }

        /// <summary>
        /// Keeps the Camera from changing side while aiming
        /// </summary>
        public override bool ChangeAimCameraSide()
        {
            return false;
        }

        public override void ResetAbility()
        {
            KnotToHand = false;
            Bow = null;
        }

        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// This will rotate the bones of the character to match the AIM direction 
        /// </summary>
        protected virtual void FixAimPoseBow()
        {
            if (RC.IsAiming)
            {
                float Weight =
                    RC.Active_IMWeapon.RightHand ? AimWeight.Evaluate(1 + RC.HorizontalAngle) : AimWeight.Evaluate(1 - RC.HorizontalAngle); //The Weight evaluated on the AnimCurve

                Vector3 LookDirection = RC.Target ? RC.AimDirection : RC.AimDot ? Utilities.MalbersTools.DirectionFromCameraNoRayCast(RC.AimDot.position) : cam.forward;

                Quaternion LookRotation = Quaternion.LookRotation(LookDirection, RC.Target ? Vector3.up : cam.up);

                Vector3 ShoulderRotationAxis = RC.Target ? Vector3.Cross(Vector3.up, LookDirection).normalized : cam.right;

                RC.Chest.RotateAround(RC.Chest.position, ShoulderRotationAxis, (Vector3.Angle(Vector3.up, LookDirection) - 90) * Weight); //Nicely Done!!

                if (RC.Active_IMWeapon.RightHand)
                {
                    RC.Chest.rotation *= Quaternion.Euler(ChestRight);
                    RC.RightHand.rotation *= Quaternion.Euler(HandRight);
                    RC.RightShoulder.rotation = Quaternion.Lerp(RC.RightShoulder.rotation, LookRotation * Quaternion.Euler(ShoulderRight), Weight); // MakeDamage the boy always look to t
                }
                else
                {
                    RC.Chest.rotation *= Quaternion.Euler(ChestLeft);
                    RC.LeftHand.rotation *= Quaternion.Euler(HandLeft);
                    RC.LeftShoulder.rotation = Quaternion.Lerp(RC.LeftShoulder.rotation, LookRotation * Quaternion.Euler(ShoulderLeft), Weight); // MakeDamage the boy always look to t
                }
            }
        }

        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        ///Put the Bow Knot to the fingers Hand This is called for the Animator
        /// </summary>
        public virtual void BowKnotToHand(bool enabled)
        {
            KnotToHand = enabled;

            var bow = RC.Active_IMWeapon as IBow;

            if (!KnotToHand && bow != null)
            {
                bow.RestoreKnot();
            }
        }

        /// <summary>
        /// Updates the BowKnot position in the center of the hand if is active
        /// </summary>
        protected void BowKnotInHand()
        {
            if (KnotToHand)
            {
                var bow = RC.Active_IMWeapon as IBow;
                bow.KNot.position =
                    (RC.Anim.GetBoneTransform(RC.Active_IMWeapon.RightHand ? HumanBodyBones.LeftMiddleDistal : HumanBodyBones.RightMiddleDistal).position +
                    RC.Anim.GetBoneTransform(RC.Active_IMWeapon.RightHand ? HumanBodyBones.LeftThumbDistal : HumanBodyBones.RightThumbDistal).position) / 2;

                bow.KNot.position = bow.KNot.position;
            }
        }


        public override Transform AimRayOrigin()
        {
            return (RC.Active_IMWeapon as IBow).KNot;
        }
    }
}