# Unity Lightweight Save Load System

A lightweight and easily modified JSON save load system for Unity which can save entire GameObjects, including its MonoBehaviours. It is done by converting between GameObjects and PlainGameObjects, the data version of GameObjects, saving the PlainGameObjects on disk. The system will require some coding by the user to operate.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Support](#support)
- [Contributing](#contributing)

## Installation

Make sure [Json.NET](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347) by Newtonsoft is working for you. This project is dependent on it.

Download the script files under `PlainSaveLoadScripts` files to your Unity project.

Alternatively, simply drag and drop the unitypackage `unity-lightweight-save-load-system.unitypackage` into your Unity window and import it. This will also include the demo, however. Make sure your resource folder is set up properly in relation to the demo, if you want to run the demo.

Woila! Now it works. You are ready to use the system.

## Usage

Included in the project is a demo of the system. The only files needed for your own use are the ones under the PlainSaveLoadScripts folder.

Here is an example of usage.

```csharp

        private void Update()
        {
            brawlers = brawlers.Where(b => b != null).ToList();
            // Save entire scene
            if (Input.GetKeyDown("k"))
            {
                gdm.SetGameData("Brawlers", brawlers.GetPlainClasses());
                gdm.Save(0);
            }
            // Load entire scene
            if (Input.GetKeyDown("l"))
            {
                gdm.LoadSave(0);
                List<PlainGameObject> plainBrawlers = gdm.GetGameData<List<PlainGameObject>>("Brawlers");
                // Remove all previous brawlers
                brawlers.ForEach(Destroy);
                // Instantiate the saved brawlers
                brawlers = plainBrawlers.InstantiateSelves(Instantiate);
            }
        }

```

The code above saves the scene's brawler GameObjects whenever the player hits "K". The player can then reload the saved GameObjects by hitting "L". As you can see, this code requires you to convert back and forth between GameObjects and PlainGameObjects. The database accepts any valid custom class or primitive, so you can save strings, ints, floats and classes.

You can also save and load from different files. The demo only uses the save file 0.

There is some additional support needed to properly save and load GameObjects.

```csharp
        ...
        
        private DemoManager dm;
        private IdentificationManager idm;
        // fields of type GameObject cannot be serialized. This is a limitation of Unity. 
        // GameObjects must always be instantiated in order to manifest in the game world.
        private GameObject target;
        // The solution in this particular case is to save an ID for the target GameObject.
        // This ID collaborates with the IdentificationManager to get the right target
        // GameObject upon reload.
        public int targetId = -1;
        public float timer;
        public float speed;

        ...
```

Component types unable to be directly serialized (Material, Mesh, etc.) require separate "Plain" implementations. Examples of these are included in the demo folder. MonoBehaviors don't need separate implementations, which is the main strength of this system. All properly flagged fields of MonoBehaviours are automatically serialized.

Try to keep the amount of Unity components in scripts to a minimum. They are the ones that require extra attention. All custom made classes are serialized without any issues.

```csharp

        ...

        /// <summary>
        /// Transform component receives a special class to always save its data.
        /// </summary>
        public TransformData tData;
        /// <summary>
        /// Rigidbody component receives a special class to always save its data.
        /// </summary>
        public RigidbodyData rData;
        
        ...
        
```
Above are fields dedicated to the Unity components Transform and Rigidbody.
```csharp
        
        public GameObject InstantiateSelf(Func<GameObject, GameObject> instantiateFunc)
        {
            
            ...
            
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
            
            ...
            
        }
        
        ...

```
Above we make sure to insert the position and velocity of the object when reloading it, using the information stored in the TransformData and RigidbodyData.

Rigidbodys and Transforms are currently automatically saved and loaded, just like with MonoBehaviours.

## Support

Please [open an issue](https://github.com/fraction/readme-boilerplate/issues/new) for support.

## Contributing

Contributions are welcome! I would love to see improvements on this system, as it has many areas that need could use refactoring. Open an issue or fork the repo and make a pull request.
