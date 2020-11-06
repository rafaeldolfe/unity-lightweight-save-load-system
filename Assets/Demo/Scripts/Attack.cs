using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlainSaveLoad
{
    public class Attack : MonoBehaviour
    {
        public int damage;
        public int force;
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Brawler"))
            {
                Vector3 dir = (collision.gameObject.transform.position - transform.position).normalized;
                collision.gameObject.GetComponent<Health>().currentHealth -= damage;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
            }
        }
    }
}