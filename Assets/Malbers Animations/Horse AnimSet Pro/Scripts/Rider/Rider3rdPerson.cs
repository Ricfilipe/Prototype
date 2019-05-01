using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using MalbersAnimations.Events;
using MalbersAnimations.Utilities;
using System;

namespace MalbersAnimations.HAP
{
    public class Rider3rdPerson : Rider //,IAnimatorBehaviour
    {
        #region IK VARIABLES    
        protected float L_IKFootWeight = 0f;        //IK Weight for Left Foot
        protected float R_IKFootWeight = 0f;        //IK Weight for Right Foot
        #endregion

        public UnityEvent OnStartMounting = new UnityEvent();
        public UnityEvent OnEndMounting = new UnityEvent();
        public UnityEvent OnStartDismounting = new UnityEvent();
        public UnityEvent OnEndDismounting = new UnityEvent();
        public UnityEvent OnAlreadyMounted = new UnityEvent();

        public int MountLayerIndex = -1;                    //Mount Layer Hash

       

        protected AnimatorUpdateMode Initial_UpdateMode;    //On wich Animator Update Mode was the Rider before mounting

        public override Mountable Montura
        {
            get { return montura; }
            set
            {
                montura = value;
                MountLayerIndex = value != null ? Anim.GetLayerIndex(Montura.MountLayer) : -1;   //Gets the layer mask of the Montura that you just found
            }
        }


        private void Reset()
        {
            DismountInput.GetPressed = InputButton.LongPress;
#if UNITY_EDITOR
            MEvent Dismountkey = MalbersTools.GetInstance<MEvent>("Dismount Key");
            MEvent MountDismountUI = MalbersTools.GetInstance<MEvent>("Mount Dismount UI");

            if (Dismountkey)
            {
                UnityEditor.Events.UnityEventTools.AddPersistentListener(DismountInput.OnLongPress, Dismountkey.Invoke);
                UnityEditor.Events.UnityEventTools.AddPersistentListener(DismountInput.OnPressedNormalized, Dismountkey.Invoke);
                UnityEditor.Events.UnityEventTools.AddIntPersistentListener(DismountInput.OnInputDown, Dismountkey.Invoke, 0);
                UnityEditor.Events.UnityEventTools.AddPersistentListener(DismountInput.OnInputUp, Dismountkey.Invoke);
            }

            if (MountDismountUI)
            {
                OnStartMounting = new UnityEvent();
                UnityEditor.Events.UnityEventTools.AddBoolPersistentListener(OnStartMounting, MountDismountUI.Invoke, false);
            }

            var malbersinput = GetComponent<MalbersInput>();
            if (malbersinput)
            {
                UnityEditor.Events.UnityEventTools.AddBoolPersistentListener(OnStartMounting, malbersinput.EnableMovement, false);
                UnityEditor.Events.UnityEventTools.AddBoolPersistentListener(OnEndDismounting, malbersinput.EnableMovement, true);
            }
#endif
        }
        /// Set all the References
        void Awake()
        {
            _transform = transform;
            if (!Anim) Anim = GetComponentInChildren<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponents<Collider>();

            MountLayerIndex = -1;                                       //Resets the Mounting Layer;
        }

        void Start()
        {
            inputSystem = DefaultInput.GetInputSystem(PlayerID);        //Set the Correct Input System

            MountInput.InputSystem =
            DismountInput.InputSystem = 
            CallAnimalInput.InputSystem =  inputSystem;

            IsOnHorse = Mounted = false;                                    //initialize in false
            if (Anim) Initial_UpdateMode = Anim.updateMode;           //Gets the Update Mode of the Animator to restore later when dismounted.

            if (StartMounted) AlreadyMounted();                             //Set All if Started Mounted is Active
        }

        /// <summary>
        ///Set all the correct atributes and variables to Start Mounted on the next frame
        /// </summary>
        public void AlreadyMounted()
        {
            StartCoroutine(AlreadyMountedC());
        }

        IEnumerator AlreadyMountedC()
        {
            yield return null;      //Wait for the next frame

            if (AnimalStored != null && StartMounted)
            {
                Montura = AnimalStored;
                Montura.ActiveRider = this;

                if (MountTrigger == null) Montura.transform.GetComponentInChildren<MountTriggers>(); //Save the first Mount trigger you found

                Start_Mounting();
                End_Mounting();

                if (Anim) Anim.Play(Montura.MountIdle, MountLayerIndex);               //Play Mount Idle Animation Directly
              
                toogleCall = true;                                                     //Turn Off the Call

                Montura.Mounted = Mounted = true;                                      //Send to the animalMount that mounted is active

                Anim.SetBool(Hash.Mount, mounted);                                     //Update Mount Parameter In the Animator
            }

            Montura.Animal.OnSyncAnimator.AddListener(SyncAnimator);                   //Add the Sync Method to sync all parameters
            OnAlreadyMounted.Invoke();
        }

        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        ///This is call at the Beginning of the Mount Animations
        /// </summary>
        public override void Start_Mounting()
        {
            if (Anim)
            {
                Anim.SetLayerWeight(MountLayerIndex, 1);                                //Enable Mount Layer set the weight to 1
                Anim.SetBool(Hash.Mount, Mounted);                                      //Update the Mount Parameter on the Animator

                Vector3 HipPos = transform.position;
                HipPos.y = Anim.GetBoneTransform(HumanBodyBones.Hips).position.y;
                transform.position = HipPos;                                            //It will change the Transform Y position to the Hip Position
            }

            if (!MountTrigger)
                MountTrigger = Montura.GetComponentInChildren<MountTriggers>();         //If null add the first mount trigger found

            if (DisableComponents)
            {
                ToggleComponents(false);                                                //Disable all Monobehaviours breaking the Riding System
            }

            base.Start_Mounting();
            OnStartMounting.Invoke();                                                   //Invoke UnityEvent for  Start Mounting

            if (!Anim) End_Dismounting();                                               //If is there no Animator  execute the End_Dismounting part
        }

        /// <summary>
        /// Execute the last code after the Rider mounts the Animal
        /// </summary>
        public override void End_Mounting()
        {
            base.End_Mounting();

            if (Anim) Anim.updateMode = AnimatorUpdateMode.Normal;                      //Needed to make IK Stuffs Manually like Bow, Pistol etc...

            OnEndMounting.Invoke();
        }

        public override void Start_Dismounting()
        {
            base.Start_Dismounting();

            if (Anim) Anim.updateMode = Initial_UpdateMode;                      //Restore Update mode to its original

            OnStartDismounting.Invoke();
        }
        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        ///This is call at the end of the Dismounting Animations States on the animator
        /// </summary>
        public override void End_Dismounting()
        {
            if (Anim && MountLayerIndex != -1) Anim.SetLayerWeight(MountLayerIndex, 0);                       //Reset the Layer Weight to 0 when end dismounting
            base.End_Dismounting();

            if (DisableComponents) ToggleComponents(true);                          //Enable all Monobehaviours breaking the Mount System
            OnEndDismounting.Invoke();                                              //Invoke UnityEvent when is off Animal
        }

        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Syncronize the Horse/Rider animations if Rider loose sync with the animal on the locomotion state
        /// </summary>
        protected virtual void Animators_ReSync()
        {
            if (!Anim) return;
            if (!Montura.syncAnimators) return;                                                                     //Don't Sync animators when that parameter is disabled

            if (Montura.Animal.AnimState == AnimTag.Locomotion)                                                     //Search for syncron the locomotion state on the animal
            {
                 RiderNormalizedTime = Anim.GetCurrentAnimatorStateInfo(MountLayerIndex).normalizedTime;            //Get the normalized time from the Rider
                 HorseNormalizedTime = Montura.Animal.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;           //Get the normalized time from the Horse

                syncronize = true;

                if (Mathf.Abs(RiderNormalizedTime - HorseNormalizedTime) > 0.1f && Time.time - LastSyncTime > 1f)   //Checking if the animal and the rider are unsync by 0.2
                {
                    Anim.CrossFade(AnimTag.Locomotion, 0.2f, MountLayerIndex, HorseNormalizedTime);                 //Normalized with blend
                    LastSyncTime = Time.time;
                }
            }
            else
            {
                syncronize = false;
                RiderNormalizedTime = HorseNormalizedTime = 0;
            }
        }

        //Used this for Sync Animators
        private float RiderNormalizedTime;
        private float HorseNormalizedTime;
        private float LastSyncTime;
        private bool syncronize;

        void Update()
        {
            SetMounting();

            //───────────────────────────────────────────────────────────────────────────────────────────────────────
            if (IsRiding && Montura)                                          //Run Stuff While Mounted
            {
                WhileIsMounted();
            }



            if ((LinkUpdate & UpdateType.Update) == UpdateType.Update)
            {
                LinkRider();
            }
        }

        private void LateUpdate()
        {
            if ((LinkUpdate & UpdateType.LateUpdate) == UpdateType.LateUpdate)
            {
                LinkRider();
            }
        }

        private void FixedUpdate()
        {
            if ((LinkUpdate & UpdateType.FixedUpdate) == UpdateType.FixedUpdate)
            {
                LinkRider();
            }
        }

        /// <summary>
        /// Set if the Rider can Mount
        /// </summary>
        public virtual void SetMounting()
        {
            if (CanMount)                                               //if are near an animal and we are not already on an animal
            {
                if (MountInput.GetInput) MountAnimal();                 //Run mounting Animations
            }
            else if (CanDismount)                                       //if we are already mounted and the animal is not moving (Mounted && IsOnHorse && Montura.CanDismount)
            {
                if (DismountInput.GetInput) DismountAnimal();           //Run Dismounting Animations
            }
            else if (CanCallAnimal)                                     //if there is no horse near, call the animal in the slot
            {
               if (CallAnimalInput.GetInput)  CallAnimal();
            }
        }

        /// <summary>
        /// Check without Input if the Rider can Mount Dismount or Call an Animal
        /// </summary>
        public virtual void CheckMountDismount()
        {
            if (CanMount)                       //if are near an animal and we are not already on an animal
            {
                MountAnimal();                  //Run mounting Animations
            }
            else if (CanDismount)               //if we are already mounted and the animal is not moving (Mounted && IsOnHorse && Montura.CanDismount)
            {
                DismountAnimal();               //Run Dismounting Animations
            }
            else if (CanCallAnimal)             //if there is no horse near, call the animal in the slot
            {
                CallAnimal();
            }
        }

       


        protected virtual void WhileIsMounted()
        {
            Animators_ReSync();                                         //Check the syncronization and fix it if is offset***
        }

        protected virtual void SyncAnimator()
        {
            Animal animal = Montura.Animal;
            float speed = animal.Speed;

            if (animal.Fly) speed = Mathf.Clamp(speed * 2, 0, 2); //Just set when is flying to Locomotion Trot on the Rider
         

            Anim.SetFloat(animal.hash_Vertical, speed);
            Anim.SetFloat(animal.hash_Horizontal, Montura.Animal.Direction);
            Anim.SetFloat(animal.hash_Slope, Montura.Animal.Slope);
            Anim.SetBool(animal.hash_Stand, Montura.Animal.Stand);

            Anim.SetBool(animal.hash_Jump, !Montura.Animal.Fly ? Montura.Animal.Jump : false); //if Fly is active don't access the fly

            Anim.SetBool(animal.hash_Attack1, Montura.Animal.Attack1);
            Anim.SetBool(animal.hash_Shift, Montura.Animal.Attack2 ? false : Montura.Animal.Shift);

            Anim.SetBool(animal.hash_Damaged, Montura.Animal.Damaged);

            Anim.SetBool(animal.hash_Stunned, Montura.Animal.Stun);
            Anim.SetBool(animal.hash_Action, Montura.Animal.Action);

            Anim.SetInteger(animal.hash_IDAction, Montura.Animal.ActionID);
            Anim.SetInteger(animal.hash_IDInt, Montura.Animal.IDInt);             
            Anim.SetFloat(animal.hash_IDFloat, Montura.Animal.IDFloat);

            if (Montura.Animal.canSwim) Anim.SetBool(animal.hash_Swim, Montura.Animal.Swim);
        }

        public virtual void MountAnimal()
        {
            if (!CanMount) return;
            if (Montura == null) return;

            Montura.Animal.OnSyncAnimator.AddListener(SyncAnimator);         //Add the Sync Method to sync all parameters

            if (Anim)
            {
                Anim.SetLayerWeight(MountLayerIndex, 1);                     //Enable the Mounting layer  
                Anim.SetBool(Hash.Mount, Mounted);                           //Update Mount Parameter on the Animator
            }

            if (!Montura.InstantMount)                                                    //If is instant Mount play it      
            {
                if (Anim) Anim.Play(MountTrigger.MountAnimation, MountLayerIndex);      //Play the Mounting Animations
            }
            else
            {
                Start_Mounting();
                End_Mounting();
                if (Anim) Anim.Play(Montura.MountIdle, MountLayerIndex);                //Ingore the Mounting Animations
            }
        }

        public virtual void DismountAnimal()
        {
            if (!CanDismount) return;

            Montura.Mounted = Mounted = false;                                  //Unmount the Animal

            Montura.Animal.OnSyncAnimator.RemoveListener(SyncAnimator);        //Add the Sync Method to sync all parameters

            if (Anim)
            {
                Anim.SetBool(Hash.Mount, Mounted);                          //Update Mount Parameter In the Animator
                Anim.SetInteger(Hash.MountSide,MountTrigger.DismountID);    //Update MountSide Parameter In the Animator
            }

            if (Montura.InstantMount)                                       //Use for Instant mount
            {
                Anim.Play(Hash.Null, MountLayerIndex);

                Anim.SetInteger(Hash.MountSide,0);                          //Update MountSide Parameter In the Animator
                Start_Dismounting();
                End_Dismounting();

                _transform.position = MountTrigger.transform.position + (MountTrigger.transform.forward * -0.2f);   //Move the rider directly to the last mounting Trigger
            }
        }

        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// IK Feet Adjustment while mounting
        /// </summary>
        void OnAnimatorIK()
        {
            if (Anim == null) return;           //If there's no animator skip
            if (Montura != null)
            {
                if (Montura.FootLeftIK == null || Montura.FootRightIK == null 
                 || Montura.KneeLeftIK == null || Montura.KneeRightIK == null) return;  //if is there missing an IK point do nothing

                //linking the weights to the animator
                if (Mounted || IsOnHorse)
                {
                    L_IKFootWeight = 1f;
                    R_IKFootWeight = 1f;

                    int CurrentMountedHash = Anim.GetCurrentAnimatorStateInfo(MountLayerIndex).tagHash;

                    if (CurrentMountedHash == Hash.Tag_Mounting || CurrentMountedHash == Hash.Tag_Unmounting)
                    {
                        L_IKFootWeight = Anim.GetFloat(Hash.IKLeftFoot);
                        R_IKFootWeight = Anim.GetFloat(Hash.IKRightFoot);
                    }

                    //setting the weight
                    Anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, L_IKFootWeight);
                    Anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, R_IKFootWeight);

                    Anim.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, L_IKFootWeight);
                    Anim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, R_IKFootWeight);

                    //Knees
                    Anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, L_IKFootWeight);
                    Anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, R_IKFootWeight);

                    //Set the IK Positions
                    Anim.SetIKPosition(AvatarIKGoal.LeftFoot, Montura.FootLeftIK.position);
                    Anim.SetIKPosition(AvatarIKGoal.RightFoot, Montura.FootRightIK.position);

                    //Knees
                    Anim.SetIKHintPosition(AvatarIKHint.LeftKnee, Montura.KneeLeftIK.position);    //Position
                    Anim.SetIKHintPosition(AvatarIKHint.RightKnee, Montura.KneeRightIK.position);  //Position

                    Anim.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, L_IKFootWeight);   //Weight
                    Anim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, R_IKFootWeight);  //Weight

                    //setting the IK Rotations of the Feet
                    Anim.SetIKRotation(AvatarIKGoal.LeftFoot, Montura.FootLeftIK.rotation);
                    Anim.SetIKRotation(AvatarIKGoal.RightFoot, Montura.FootRightIK.rotation);
                }
                else
                {
                    Anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0f);
                    Anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);

                    Anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0f);
                    Anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0f);
                }
            }
        }

        ///Inspector Entries
        [SerializeField] public bool Editor_Advanced;

        
        public bool debug;

        void OnDrawGizmos()
        {
            if (!debug) return;
            if (!Anim) return;
            if (syncronize)
            {
                Transform head = Anim.GetBoneTransform(HumanBodyBones.Head);

                if ((int)RiderNormalizedTime % 2 == 0)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                    Gizmos.DrawSphere((head.position - transform.root.right * 0.2f), 0.05f);

                if ((int)HorseNormalizedTime % 2 == 0)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                    Gizmos.DrawSphere((head.position + transform.root.right * 0.2f), 0.05f);

#if UNITY_EDITOR
                UnityEditor.Handles.color = Color.white;
                UnityEditor.Handles.Label(head.position + transform.up * 0.5f, "Sync Status");
#endif

            }


        }
        
        #region MountDismount

        //TransformAnimation Fix;
        //float ScaleFactor;
        //private Transform LeftFoot;
        //private Transform RightFoot;
        //private Vector3 BottomPosition;
        //private Vector3 HipPosition;
        //private Vector3 LastRelativeRiderPosition;

        //public void OnStateEnter(int ID, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    if (ID == Hash.Tag_Mounting)
        //    {

        //    }
        //    else if (ID == Hash.Tag_Unmounting)
        //    {
        //        Enter_Dismount();
        //    }
        //}

       

        //public void OnStateExit(int ID, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    if (ID == Hash.Tag_Mounting)
        //    {

        //    }
        //    else if (ID == Hash.Tag_Unmounting)
        //    {
        //        End_Dismounting();
        //    }
        //}

        //public void OnStateMove(int ID, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    if (ID == Hash.Tag_Mounting)
        //    {

        //    }
        //    else if (ID == Hash.Tag_Unmounting)
        //    {
        //        StateMove_Dismount(stateInfo, layerIndex);
        //    }
        //}

        //private void Enter_Dismount()
        //{
        //    Anim.SetInteger(Hash.MountSide, 0);                 //remove the side of the mounted **IMPORTANT*** otherwise it will keep trying to dismount

        //    ScaleFactor = Montura.Animal.ScaleFactor;                                     //Get the scale Factor from the Montura

        //    LeftFoot = Anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        //    RightFoot = Anim.GetBoneTransform(HumanBodyBones.RightFoot);

        //    BottomPosition = Montura.MountPoint.InverseTransformPoint((LeftFoot.position + RightFoot.position) / 2); //Convert it to Local Space
        //    HipPosition = Montura.MountPoint.InverseTransformPoint(Anim.rootPosition);         //Convert it to Local Space

        //    Fix = MountTrigger.Adjustment;            //Store the Fix

        //    Start_Dismounting();

        //    _transform.position = Montura.MountPoint.position;
        //    _transform.rotation = Montura.MountPoint.rotation;

        //    LastRelativeRiderPosition = Montura.MountPoint.InverseTransformPoint(transform.position);
        //}

        //private void StateMove_Dismount(AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    var transition = Anim.GetAnimatorTransitionInfo(layerIndex);

        //    float transitionTime = transition.normalizedTime;
        //    float time = Time.deltaTime;


        //    transform.rotation = Anim.rootRotation;
        //    transform.position = Montura.MountPoint.TransformPoint(LastRelativeRiderPosition); //Parent Position without Parenting

        //    //Smoothly move the center of mass to the desired position in the first Transition
        //    if (Anim.IsInTransition(layerIndex) && stateInfo.normalizedTime < 0.5f)
        //    {

        //        //BottomPosition = MountPoint.InverseTransformPoint((LeftFoot.position + RightFoot.position) / 2); //Convert it to Local Space
        //        //HipPosition = MountPoint.InverseTransformPoint(animator.rootPosition);                          //Convert it to Local Space

        //        var newPos = Montura.MountPoint.position;
        //        newPos.y = Montura.MountPoint.TransformPoint(Vector3.Lerp(HipPosition, BottomPosition, transitionTime)).y;

        //        transform.position = newPos;


        //        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation, transitionTime);
        //    }
        //    else
        //    {
        //        transform.position += (Anim.velocity * Time.deltaTime * ScaleFactor * (Fix ? Fix.delay : 1));
        //    }

        //    //Stop the Mountura from walking forward when Dismounting
        //    if (Montura)
        //    {
        //        Montura.Animal.MovementAxis = Montura.Animal.MovementAxis * (1 - stateInfo.normalizedTime);

        //        //Don't go under the floor
        //        if (MountTrigger)
        //        {
        //            if (transform.position.y < MountTrigger.transform.position.y)
        //            {
        //                transform.position = new Vector3(transform.position.x, MountTrigger.transform.position.y, transform.position.z);
        //            }
        //        }

        //        if (stateInfo.normalizedTime > 0.8f)
        //        {
        //            _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation, transitionTime);
        //            _transform.position = Vector3.Lerp(_transform.position, new Vector3(transform.position.x, MountTrigger.transform.position.y, transform.position.z), time * 5f);
        //        }
        //    }

        //    Anim.rootPosition = transform.position;

        //    LastRelativeRiderPosition = Montura.MountPoint.InverseTransformPoint(transform.position);
        //}




        //public void OnStateUpdate(int ID, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    //if (ID == Hash.Tag_Mounting)
        //    //{

        //    //}
        //    //else if (ID == Hash.Tag_Unmounting)
        //    //{

        //    //}
        //}
        #endregion
    }
}