using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace PlainSaveLoad
{
    public class RigidbodyData
    {
        public Vector3 velocity;
        public Vector3 angularVelocity;

        public RigidbodyData(Vector3 velocity, Vector3 angularVelocity)
        {
            this.velocity = velocity;
            this.angularVelocity = angularVelocity;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("velocity: ");
            sb.Append(velocity);
            sb.Append("angularVelocity: ");
            sb.Append(angularVelocity);

            return sb.ToString();
        }
    }
}