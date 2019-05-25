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
        public GameObject CreateObject;

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
                    form = new InfataryFormation();
                    break;
            }

            foreach (GameObject go in army)
            {
               // go.GetComponent<AgentFollow>().agent.GetComponent<NavMeshAgent>().enabled = true;
            }

            Reset();
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (form.ofensive)
                {
                    form.ofensive = false;
                }
                else
                {
                    form.ofensive = true;
                }
            }


            foreach (GameObject go in army)
            {
                //go.GetComponent<AgentFollow>().agent.GetComponent<NavMeshAgent>().enabled = true;
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                while (army.Count < 5)
                {
                    GameObject go = Instantiate(CreateObject);


                    army.Add(go);
                }
                Reset();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {

                var n = Random.Range(0, army.Count);
                Destroy(army[n]);
                army.RemoveAt(n);
            }

            form.doFormation(army, leader);
        }

        private void Reset()
        {
            form.doFormation(army, leader);

            Vector3 leaderPos = leader.transform.position;


            foreach (GameObject guard in army)
            {
                //Vector3 offset = guard.GetComponent<AgentFollow>().offset;
                //guard.transform.position = new Vector3(leaderPos.x - leader.transform.forward.x * (0.3f + offset.x) + leader.transform.right.x * (offset.y), 5.809459f, leaderPos.z - leader.transform.forward.z * (0.3f + offset.x) + leader.transform.right.z * (offset.y));

            }
        }
    }
}
