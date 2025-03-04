﻿using UnityEngine;
using UnityEngine.Events;
using MalbersAnimations.Events;

namespace MalbersAnimations.HAP
{
    [System.Serializable]
    public class MountEvent : UnityEvent<Mountable>
    { }

    [System.Serializable]
    public class RiderEvent : UnityEvent<Rider>
    { }

    public abstract class Rider : MonoBehaviour, IAnimatorListener
    {
      
        public enum UpdateType                                      // The available methods of updating are:
        {
            Update =1 ,
            FixedUpdate = 2,                                            // Update in FixedUpdate (for tracking rigidbodies).
            LateUpdate = 4,                                             // Update in LateUpdate. (for tracking objects that are moved in Update)
        }

        #region Variables
        protected bool mounted = false;             //This is used to activate the Mounting/Dismounting True(Start Mounting), False(Start Dismounting)
        protected bool isOnHorse = false;           //This is true (Finish Mounting) False(Finish Dismounting). IMPORTANT this works with *Mounted*
        protected bool isInMountTrigger;            //If the animal can be mounted, if it doesn't have another rider or the animal isn't death

        protected Mountable montura;                //The animal/Carriage to mount

        protected bool toogleCall;                  //To call the animal or stop the call
        protected MalbersInput animalControls;      //Controls of the animal
        protected MountTriggers mountTrigger;       //Transform of the Selected Side to mount
        protected MonoBehaviour[] allComponents;    //Reference for all MonoBehaviour to enable/disable when/MountingDismounting  

        #region Rider Components
        public Animator Anim;                       //Reference for the Animator 
        protected Transform _transform;             //Reference for this transform
        protected Rigidbody _rigidbody;             //Reference for this rigidbody
        protected Collider[] _collider;             //Reference for all the colliders on this gameObject
        protected CapsuleCollider mountedCollider;  //For creating a collider when is mounted for Hit Porpouse
        #endregion

        public bool StartMounted;                    //True if we want to start mounted an animal

        /// <summary>Parent to mount Point </summary>
        [Tooltip("Parent the Rider to the Mount Point")]
        public bool Parent = false;

        [Utilities.Flag()]
        public UpdateType LinkUpdate = UpdateType.Update | UpdateType.FixedUpdate;

        /// <summary>This animal is the one that you can call or StartMount </summary>
        public Mountable AnimalStored;
        public InputRow MountInput = new InputRow("Mount", KeyCode.F, InputButton.Down);
        public InputRow DismountInput = new InputRow("Mount", KeyCode.F, InputButton.LongPress); 
        public InputRow CallAnimalInput = new InputRow("Call", KeyCode.F, InputButton.Down);

        internal IInputSystem inputSystem;

        public bool CreateColliderMounted;
        public float Col_radius = 0.25f;
        public bool Col_Trigger = true;
        public float Col_height = 0.8f;
        public float Col_Center = 0.48f;

        public bool DisableComponents;
        public MonoBehaviour[] DisableList;
        public AudioClip CallAnimalA;
        public AudioClip StopAnimalA;
        public AudioSource RiderAudio;

        public string PlayerID = "Player0";

        #endregion

        #region UnityEvents

        public GameObjectEvent OnFindMount = new GameObjectEvent();
        public BoolEvent OnCanMount =  new BoolEvent();
        public BoolEvent OnCanDismount = new BoolEvent();
        public BoolEvent OnCallMount = new BoolEvent();

        #endregion

        #region Properties
        protected MonoBehaviour[] AllComponents
        {
            get
            {
                if (allComponents == null)
                {
                    allComponents = GetComponents<MonoBehaviour>();
                }
                return allComponents;
            }
        }

        /// <summary>
        /// Mount that represent the Horse, Dragon or Carriage common properties
        /// </summary>
        public virtual Mountable Montura
        {
            get { return montura; }
            set { montura = value;}
        }

        /// <summary>
        /// If Null means that we are NOT Near to an Animal
        /// </summary>
        public MountTriggers MountTrigger
        {
            get { return mountTrigger; }
            set { mountTrigger = value; }
        }


        /// <summary>
        /// Check if can mount an Animal
        /// </summary>
        public bool CanMount
        {
            get { return MountTrigger && !Mounted && !IsOnHorse; }  //We Have a mount trigger... the rider is not mounted or on the horse
        }

        /// <summary>
        /// Check if we can dismount the Animal
        /// </summary>
        public virtual bool CanDismount
        {
            get { return IsRiding; }
        }


        public virtual bool CanCallAnimal
        {
            get { return !MountTrigger && !Mounted && !IsOnHorse; }
        }


        /// <summary>
        /// True if the Rider Started Mounting
        /// False if the Rider Started Dismounting
        /// </summary>
        public bool Mounted
        {
            get { return mounted; }
            set { mounted = value; }
        }

        /// <summary>
        /// Returns if the Rider is on the horse and Mount animations are finished
        /// </summary>
        public bool IsRiding
        {
            get { return IsOnHorse && mounted; }
        }


        /// <summary>
        /// This is true (Finish Mounting) False (Finish Dismounting)
        /// </summary>
        public bool IsOnHorse
        {
            get { return isOnHorse; }
            protected set { isOnHorse = value; }
        }

        public virtual MalbersInput AnimalControl
        {
            get
            {
                if (animalControls == null)
                {
                    if (Montura.Animal)
                    {
                        animalControls = Montura.Animal.GetComponent<MalbersInput>();
                    }
                }
                return animalControls;
            }
            set { animalControls = value; }
        }

        protected Camera cam;                // Main Camera for the game
        public Camera MainCamera             // Get the Main Camera
        {
            get
            {
                if (Camera.main != null) cam = Camera.main;
                else cam = null;

                return cam;
            }
        }
        #endregion

      // protected Vector3 DeltaMountPointPos;

      

        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// CallBack at the Start of the Mount Animations
        /// </summary>
        public virtual void Start_Mounting()
        {
            Montura.ActiveRider = this;
            Montura.Mounted = Mounted = true;               //Sync Mounted Values in Animal and Rider

            if (_rigidbody)                                 //Deactivate stuffs for the Rider's Rigid Body
            {
                _rigidbody.useGravity = false;
                _rigidbody.isKinematic = true;
                DefaultConstraints = _rigidbody.constraints;
                _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
               // _rigidbody.constraints = RigidbodyConstraints.None;

            }

            ToogleColliders(false);                         //Deactivate All Colliders

            toogleCall = true;                              //Set the Call to Stop Animal
            CallAnimal(false);                              //If is there an animal following us stop him

            if (Montura.Animal)
            {
                AnimalStored = Montura.Animal.GetComponent<Mountable>(); //Store the last animal you mounted
            }

            if (Parent) transform.parent = Montura.MountPoint;
        }

        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// CallBack at the End of the Mount Animations
        /// </summary>
        public virtual void End_Mounting()
        {
            IsOnHorse = true;
            Montura.Mounted = Mounted = true;                                          //Sync Mounted Values in Animal and Rider again Double Check

            transform.localPosition = Vector3.zero;                                    //Reset Position
            transform.localRotation = Quaternion.identity;                             //Reset Rotation

            Montura.EnableControls(true);                                              //Enable Animal Controls

            if (CreateColliderMounted) MountingCollider(true);

            //Montura.ActiveRider = this;
        }

        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// CallBack at the Start of the Dismount Animations
        /// </summary>
        public virtual void Start_Dismounting()
        {
            MountingCollider(false);                                                 //Remove the Mount Collider
                                                                                     //Invoke UnityEvent when is off Animal
            if (CreateColliderMounted) MountingCollider(false);                      //Remove MountCollider

            transform.parent = null;

            Montura.ActiveRider = null;
            Montura.ActiveRiderCombat = null;
            Montura.Mounted = mounted = false;                                      //Disable Mounted on everyone

            if (!Anim) End_Dismounting();
        }

        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// CallBack at the End of the Dismount Animations
        /// </summary>
        public virtual void End_Dismounting()
        {
            IsOnHorse = false;                                                              //Is no longer on the Animal
           if (Montura) Montura.EnableControls(false);                                                  //Disable Montura Controls

            Montura = null;                                                                 //Reset the Montura

            toogleCall = false;                                                             //Reset the Call Animal

           // isInMountTrigger = false;                                                       //Reset the CanMount

            if (_rigidbody)                                                                 //Reactivate stuffs for the Rider's Rigid Body
            {
                _rigidbody.useGravity = true;
                _rigidbody.isKinematic = false;
                _rigidbody.constraints = DefaultConstraints;
            }

            if (Anim) Anim.speed = 1;                                                       //Reset AnimatorSpeed

            _transform.rotation = Quaternion.FromToRotation(_transform.up, -Physics.gravity) * _transform.rotation;  //Reset the Up Vector;

            ToogleColliders(true);                                                          //Enabled colliders
        }


        public virtual void LinkRider()
        {
            if (IsRiding)
            {
                //_rigidbody.MovePosition(Montura.MountPoint.position);
                //_rigidbody.MoveRotation(Montura.MountPoint.rotation);
                transform.position = Montura.MountPoint.position;
                transform.rotation = Montura.MountPoint.rotation;
            }
        }


        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Create a collider from hip to chest to check hits  when is on the horse 
        /// </summary>
        public virtual void MountingCollider(bool create)
        {
            if (create)
            {
                mountedCollider = gameObject.AddComponent<CapsuleCollider>();
                mountedCollider.center = new Vector3(0, Col_Center);
                mountedCollider.radius = Col_radius;
                mountedCollider.height = Col_height;
                mountedCollider.isTrigger = Col_Trigger;
            }
            else
            {
                Destroy(mountedCollider);
            }
        }

        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// To change the animal Stored
        /// </summary>
        public virtual void SetAnimalStored(Mountable MAnimal)
        {
            AnimalStored = MAnimal;
        }


        public virtual void KillMountura()
        {
            if (Montura)
            {
                Montura.Animal.Death = true;
            }
        }

        /// <summary>
        /// If the Animal has a IMountAI component it can be called
        /// </summary>
        public virtual void CallAnimal(bool playWistle = true)
        {
            if (!CanCallAnimal) return;

            if (AnimalStored)                                                               //Call the animal Stored
            {
                IMountAI AIAnimalM = AnimalStored.GetComponent<IMountAI>();
                if (AIAnimalM != null)
                {
                    toogleCall = !toogleCall;
                    AIAnimalM.CallAnimal(_transform, toogleCall);

                    if (CallAnimalA && StopAnimalA && playWistle)
                        RiderAudio.PlayOneShot(toogleCall ? CallAnimalA : StopAnimalA);
                }
            }
        }

        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Toogle Colliders in this game Object
        /// </summary>
        protected virtual void ToogleColliders(bool active)
        {
            if (_collider.Length > 0)
                foreach (var item in _collider)
                { item.enabled = active; }
        }


        ///──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Toogle the MonoBehaviour Components Attached to this game Objects but the Riders Scripts
        /// </summary>
        protected virtual void ToggleComponents(bool enabled)
        {
            if (DisableList.Length == 0)
            {
                foreach (var component in AllComponents)
                {
                    if (component is Rider || component is RiderCombat)
                    {
                        continue;
                    }
                    component.enabled = enabled;
                }
            }
            else
            {
                foreach (var component in DisableList)
                {
                    if (component != null) component.enabled = enabled;
                }
            }
        }

        /// <summary>
        /// Used for listening Message behaviour from the Animator
        /// </summary>
        public virtual void OnAnimatorBehaviourMessage(string message, object value)
        {
            this.InvokeWithParams(message, value);
        }

        //Editor Variables
        [HideInInspector] public bool Editor_RiderCallAnimal;
        [HideInInspector] public bool Editor_Events;
        [HideInInspector] public bool Editor_Inputs;
        private RigidbodyConstraints DefaultConstraints;
    }
}