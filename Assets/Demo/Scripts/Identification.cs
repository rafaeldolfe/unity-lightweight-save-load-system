using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlainSaveLoad
{
    /// <summary>
    /// This is a very basic version of an ID system for GameObjects. 
    /// Manually setting IDs like this is not desirable. For the demo, it is sufficient.
    /// </summary>
    public class Identification : MonoBehaviour
    {
        private IdentificationManager idm;
        public int id;

        private void Start()
        {
            idm = FindObjectOfType(typeof(IdentificationManager)) as IdentificationManager;

            idm.idToGameObjects[id] = gameObject;
        }
    }
}