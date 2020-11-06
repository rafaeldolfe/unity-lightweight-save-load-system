using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlainSaveLoad
{
    /// <summary>
    /// This is a very basic way of attaching an integer ID to a GameObject,
    /// but for this demo, it is sufficient.
    /// </summary>
    public class IdentificationManager : MonoBehaviour
    {
        public Dictionary<int, GameObject> idToGameObjects = new Dictionary<int, GameObject>();
    }
}