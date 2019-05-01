using UnityEngine;
using System.Collections;


namespace MalbersAnimations.HAP
{
    /// <summary>
    /// This Enable the mounting System
    /// </summary>
    public class MountTriggers : MonoBehaviour
    {
        /// <summary>
        /// The name of the Animation we need to play to Mount the Animal
        /// </summary>
        public string MountAnimation = "Mount";
        [Tooltip("the Transition ID value to dismount this kind of Montura.. (is Located on the Animator)")]
        public int DismountID = 1;
        public TransformAnimation Adjustment;
        Mountable Montura;
        Rider rider;

      

        // Use this for initialization
        void Awake()
        {
            Montura = GetComponentInParent<Mountable>(); //Get the Mountable in the parents
        }

        void OnTriggerEnter(Collider other)
        {
            GetAnimal(other);
        }
        

        private void GetAnimal(Collider other)
        {
            if (!Montura)
            {
                Debug.LogError("No Mountable Script Found... please add one");
                return;
            }
            if (!Montura.Mounted && Montura.CanBeMounted)                       //If there's no other Rider on the Animal or the the Animal isn't death
            {
                rider = other.GetComponentInChildren<Rider>();
                if (rider == null) rider = other.GetComponentInParent<Rider>();


                if (rider != null)
                {
                    if (rider.IsRiding) return;

                    rider.Montura = Montura;
                    rider.MountTrigger = this;                                 //Send the side transform to mount
                    rider.OnFindMount.Invoke(transform.root.gameObject);       //Invoke Found Animal
                    Montura.OnCanBeMounted.Invoke(true);
                    Montura.NearbyRider = true;
                }
            }
        }


        
        void OnTriggerExit(Collider other)
        {
            rider = other.GetComponentInChildren<Rider>();
            if (rider == null) rider = other.GetComponentInParent<Rider>();
         

            if (rider != null)
            {
                if (rider.IsRiding) return;                                         //You Cannot Mount if you are already mounted

                if (rider.MountTrigger == this && !Montura.Mounted)                 //When exiting if we are exiting From the Same Mount Trigger means that there's no mountrigger Nearby
                {
                    rider.MountTrigger = null;
                    if (rider.Montura)   rider.Montura.EnableControls(false);
                     
                    rider.Montura = null;
                    rider.OnFindMount.Invoke(null);
                    Montura.OnCanBeMounted.Invoke(false);
                    Montura.NearbyRider = false;
                }
                rider = null;
            }
        }
    }
}