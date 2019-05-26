using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace movement
{
    public class EntityManager : MonoBehaviour
    {
        public GameObject leader;
        public List<GameObject> army;
        public TypeSoldier type;
        private Formation form;
        private GameObject realLeader;

        public enum TypeSoldier
        {
            archer,
            crossbow,
            swordsman,
            horse
        }

        // Start is called before the first frame update
        void Start()
        {

            switch (type)
            {
                case TypeSoldier.swordsman:
                    form = new InfataryFormation(gameObject);
                    break;
                case TypeSoldier.horse:
                    form = new CavalryFormation(gameObject);
                    break;
                case TypeSoldier.crossbow:
                    form = new CrossbowFormation(gameObject);
                    break;
                case TypeSoldier.archer:
                    form = new ArcherFormation(gameObject);
                    break;
            }
            realLeader = leader.GetComponentInChildren<NavMeshAgent>().gameObject;
            Enemies leaderEnemies = leader.GetComponentInChildren<Enemies>();
           
            foreach (GameObject go in army)
            {
                if (leaderEnemies.attacking)
                {
                    go.GetComponentInChildren<Enemies>().following = false;
                }
                else
                {
                    go.GetComponentInChildren<Enemies>().following = true;
                }
                go.GetComponentInChildren<Enemies>().leader = leader;
            }
            Reset();
           
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.GetChildCount() == 0)
            {
                Destroy(gameObject);
            }

            army.RemoveAll(item => item.GetComponentInChildren<Dying>().state != Dying.State.Alive);
            if (leader != null)
            {
                if (leader.GetComponentInChildren<Dying>().state != Dying.State.Alive && army.Count > 0)
                {
                    leader = army[0];
                    leader.GetComponentInChildren<Enemies>().following = false;
                    army.RemoveAt(0);
                }
                if (leader.GetComponentInChildren<Dying>().state == Dying.State.Alive)
                {
                    realLeader = leader.GetComponentInChildren<NavMeshAgent>().gameObject;
                }


                Enemies leaderEnemies = leader.GetComponentInChildren<Enemies>();
                foreach (GameObject go in army)
                {
                    if (leaderEnemies.attacking)
                    {
                        go.GetComponentInChildren<Enemies>().following = false;
                    }
                    else
                    {
                        go.GetComponentInChildren<Enemies>().following = true;
                    }
                    go.GetComponentInChildren<Enemies>().leader = leader;
                }
                form.doFormation(army, realLeader);
            }
            
           
        }

        private void Reset()
        {
            form.doFormation(army, leader);

            Vector3 leaderPos = leader.GetComponentInChildren<NavMeshAgent>().transform.position;

            form.doFormation(army, realLeader);
            foreach (GameObject guard in army)  
            {
                Vector3 offset = guard.GetComponentInChildren<Enemies>().offset;
                
                guard.transform.position = new Vector3(leaderPos.x - realLeader.transform.forward.x * (0.3f + offset.x) + realLeader.transform.right.x * (offset.y), 0f, leaderPos.z - realLeader.transform.forward.z * (0.3f + offset.x) + realLeader.transform.right.z * (offset.y));

            }
        }
    }
}
