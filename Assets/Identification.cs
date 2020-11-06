using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlainSaveLoad
{
    public class Identification : MonoBehaviour
    {
        private IdentificationManager idm;
        public int id;

        private void Start()
        {
            idm = FindObjectOfType(typeof(IdentificationManager)) as IdentificationManager;

            idm.idToGameObjects[id] = gameObject;

            Debug.Log($"This is my id: {id}, this is my name: {gameObject.name}");
        }
    }
}