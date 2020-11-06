using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PlainSaveLoad
{
    [Serializable]
    public class PlainGameObject
    {
        [Serializable]
        public class ScriptProperties
        {
            public string type;
            public Dictionary<string, object> properties;
        }
        /// <summary>
        /// This exact prefab's name
        /// </summary>
        public string prefabName;
        /// <summary>
        /// The path to this prefab's location in the resource folder
        /// </summary>
        public string folder;
        public List<ScriptProperties> scripts;
        /// <summary>
        /// Transform component receives a special class to always save its data.
        /// </summary>
        public TransformData tData;
        /// <summary>
        /// Rigidbody component receives a special class to always save its data.
        /// </summary>
        public RigidbodyData rData;

        public T GetProperty<T>(Type type, string propertyName)
        {
            ScriptProperties props = scripts.Find(scr => Type.GetType(scr.type) == type);
            if (props == null)
            {
                return default;
            }
            object prop = props.properties[propertyName];

            if (prop.GetType() == typeof(JObject))
            {
                return ((JObject)props.properties[propertyName]).ToObject<T>();
            }
            else
            {
                //Debug.Log($"propp type: {propp.GetType()}");

                //Debug.Log($"T type: {typeof(T)}");
                return (T)Convert.ChangeType(prop, typeof(T));
            }
        }
        /// <summary>
        /// Instantiate this object's associated GameObject.
        /// Must provide the Instantiate function from a Unity object (e.g. MonoBehaviour)
        /// </summary>
        /// <param name="instantiateFunc"></param>
        /// <returns></returns>
        public GameObject InstantiateSelf(Func<GameObject, GameObject> instantiateFunc)
        {
            GameObject go = Resources.Load($"{folder}/{prefabName}") as GameObject;
            if (go == null)
            {
                throw new Exception($"Could not find in resources: {folder}/{prefabName}");
            }
            GameObject clone = instantiateFunc(go);
            
            // Here is where you add any specific support for certain components
            // In this case only Transform and Rigidbody have special implementations

            if (rData != null)
            {
                Rigidbody cloneRigidbody = clone.GetComponent<Rigidbody>();
                if (cloneRigidbody != null)
                {
                    cloneRigidbody.velocity = rData.velocity;
                    cloneRigidbody.angularVelocity = rData.angularVelocity;
                }
            }
            if (tData != null)
            {
                Transform cloneTransform = clone.GetComponent<Transform>();
                if (cloneTransform != null)
                {
                    cloneTransform.position = tData.position;
                }
            }

            // Here is where all monobehaviours are saved. Remember to have the proper
            // access modifiers, private, public etc. on the fields you want to have recovered

            List<MonoBehaviour> monos = clone.GetComponents<MonoBehaviour>().ToList();

            foreach (ScriptProperties props in scripts)
            {
                Type current = Type.GetType(props.type);
                foreach (MonoBehaviour mono in monos)
                {
                    if (current == mono.GetType())
                    {
                        string propsDictToJsonObject = JsonConvert.SerializeObject(props.properties);
                        JsonUtility.FromJsonOverwrite(propsDictToJsonObject, mono);
                    }
                }
            }

            return clone;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"prefabName: {prefabName}\n");
            sb.Append("scripts: \n");
            if (scripts.Count() > 0)
            {
                scripts.ForEach(script => sb.Append($"type: {script.type}\n properties.Count(): {script.properties.Count()}\n"));
            }
            return sb.ToString();
        }
    }
}