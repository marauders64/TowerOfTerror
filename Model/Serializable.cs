﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfTerror.Model
{
    interface ISerializable
    {
        List<string> Serialize();
        void Deserialize(List<Object> data);
    }

}
