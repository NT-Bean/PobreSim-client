using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Crafts
{
    [Serializable]
    public class CraftDatasContainer
    {
        public List<CraftData> craftDatas;
        public CraftDatasContainer(List<CraftData> craftDatas) { this.craftDatas = craftDatas; }
    }
}