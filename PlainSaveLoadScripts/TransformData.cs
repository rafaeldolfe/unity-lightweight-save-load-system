using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace PlainSaveLoad
{
    public class TransformData
    {
        public Vector3 position;

        public TransformData(Vector3 position)
        {
            this.position = position;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("position: ");
            sb.Append(position);

            return sb.ToString();
        }
    }
}