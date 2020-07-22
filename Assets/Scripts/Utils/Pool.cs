using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class Pool
    {
        public Transform transform;
        public bool inUse;

        public Pool(Transform transform)
        {
            this.transform = transform;
        }

        public void Use()
        {
            inUse = true;
        }

        public void Dispose()
        {
            inUse = false;
        }
    }
}

