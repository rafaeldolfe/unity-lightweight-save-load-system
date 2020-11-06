using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlainSaveLoad
{
    public class Targeting : MonoBehaviour
    {
        private DemoManager dm;
        private IdentificationManager idm;
        private GameObject target;
        public int targetId = -1;
        public float timer;
        public float speed;

        private void Awake()
        {
            dm = FindObjectOfType(typeof(DemoManager)) as DemoManager;
            idm = FindObjectOfType(typeof(IdentificationManager)) as IdentificationManager;
        }

        private void Start()
        {
            if (idm.idToGameObjects.ContainsKey(targetId))
            {
                target = idm.idToGameObjects[targetId];
            }
        }
        private void Update()
        {
            if (target != null)
            {
                targetId = target.GetComponent<Identification>().id;
            }
            if (idm.idToGameObjects.ContainsKey(targetId) && idm.idToGameObjects[targetId] != null)
            {
                target = idm.idToGameObjects[targetId];
            }
            if (timer > 0)
            {
                if (target != null)
                {
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, Time.deltaTime * speed);
                }
                timer -= Time.deltaTime;
            }
            else
            {
                List<GameObject> enemies = dm.GetListOfEnemies();
                int randomindex = UnityEngine.Random.Range(0, enemies.Count);
                target = enemies[randomindex];
                targetId = target.GetComponent<Identification>().id;
                if (target != gameObject)
                {
                    timer = UnityEngine.Random.Range(0, 4);
                }
            }
        }
    }
}

