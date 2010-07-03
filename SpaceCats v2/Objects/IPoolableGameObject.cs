using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceCats_v2
{
    public interface IPoolableGameObject
    {
        void ReturnToPool();
    }
}
