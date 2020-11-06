using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlainSaveLoad
{
    public class IdentificationManager : MonoBehaviour
    {
        public Dictionary<int, GameObject> idToGameObjects = new Dictionary<int, GameObject>();
    }
}