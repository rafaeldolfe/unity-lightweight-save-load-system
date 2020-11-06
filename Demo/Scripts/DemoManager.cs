using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PlainSaveLoad
{
    public class DemoManager : MonoBehaviour
    {
        public List<GameObject> brawlers;
        public GlobalPersistentDataManager gdm;

        public List<GameObject> GetListOfEnemies()
        {
            brawlers = brawlers.Where(b => b != null).ToList();
            return brawlers;
        }

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
    }
}