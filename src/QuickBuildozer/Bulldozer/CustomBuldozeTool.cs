using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace QuickBuildozer
{
    public class CustomBuldozeTool : DefaultTool
    {
        public IEnumerator TryDeleteBuilding(ushort building)
        {
            return new TryDeleteBuildingEnumerator( GameObject.FindObjectOfType<BulldozeTool>(), building);
        }
    }
}
