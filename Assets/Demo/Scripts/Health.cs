using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlainSaveLoad
{
    public class Health : MonoBehaviour
    {
        public int maxHealth;
        public int currentHealth;

        private void Update()
        {
            if (currentHealth < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}