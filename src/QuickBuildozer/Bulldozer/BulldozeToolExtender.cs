using ColossalFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace QuickBuildozer
{
    public class BulldozeToolExtender
    {
        private readonly BulldozeTool _bulldozeTool;

        public BulldozeToolExtender(BulldozeTool bulldozeTool)
        {
            _bulldozeTool = bulldozeTool;
        }

        public void DeleteBuildingImpl(ushort building)
        {
            try
            {
                if (Singleton<BuildingManager>.instance.m_buildings.m_buffer[(int)building].m_flags != Building.Flags.None)
                {
                    BuildingManager instance = Singleton<BuildingManager>.instance;
                    BuildingInfo info = instance.m_buildings.m_buffer[(int)building].Info;
                    if (info.m_buildingAI.CheckBulldozing(building, ref instance.m_buildings.m_buffer[(int)building]) == ToolBase.ToolErrors.None)
                    {
                        SetFieldValue<BulldozeTool.Mode>("m_bulldozingMode", BulldozeTool.Mode.Building);
                        SetFieldValue<ItemClass.Service>("m_bulldozingService", info.m_class.m_service);
                        SetFieldValue<float>("m_deleteTimer", 0.1f);

                        int buildingRefundAmount = this.GetBuildingRefundAmount(building);
                        if (buildingRefundAmount != 0)
                        {
                            Singleton<EconomyManager>.instance.AddResource(EconomyManager.Resource.RefundAmount, buildingRefundAmount, info.m_class);
                        }
                        Vector3 position = instance.m_buildings.m_buffer[(int)building].m_position;
                        float angle = instance.m_buildings.m_buffer[(int)building].m_angle;
                        int length = instance.m_buildings.m_buffer[(int)building].Length;
                        instance.ReleaseBuilding(building);
                        int publicServiceIndex = ItemClass.GetPublicServiceIndex(info.m_class.m_service);
                        if (publicServiceIndex != -1)
                        {
                            Singleton<CoverageManager>.instance.CoverageUpdated(info.m_class.m_service, info.m_class.m_subService, info.m_class.m_level);
                        }
                        BuildingTool.DispatchPlacementEffect(info, position, angle, length, true);
                    }
                }

                var hoverInstance = GetFieldValue<InstanceID>("m_hoverInstance");
                if (building == hoverInstance.Building)
                    hoverInstance.Building = 0;

                var lastInstance = GetFieldValue<InstanceID>("m_lastInstance");
                if (building == lastInstance.Building)
                    lastInstance.Building = 0;
            }
            catch (Exception ex)
            {

                ModLogger.Exception(ex);
            }
        }

        public void DeleteBuilding(ushort buildingId)
        {
            MethodInfo method = typeof(BulldozeTool).GetMethod("DeleteBuilding", BindingFlags.NonPublic | BindingFlags.Instance);
            Singleton<SimulationManager>.instance.AddAction((IEnumerator)method.Invoke(_bulldozeTool, new object[] { buildingId }));
        }

        private int GetBuildingRefundAmount(ushort building)
        {
            MethodInfo method = typeof(BulldozeTool).GetMethod("GetBuildingRefundAmount", BindingFlags.NonPublic | BindingFlags.Instance);
            return (int)method.Invoke(_bulldozeTool, new object[] { building });
        }

        private T GetFieldValue<T>(string fieldName)
        {
            return (T)GetField(fieldName).GetValue(_bulldozeTool);
        }

        private void SetFieldValue<T>(string fieldName, T propertyValue)
        {
            GetField(fieldName).SetValue(_bulldozeTool, propertyValue);
        }

        private FieldInfo GetField(string propertyName)
        {
            return typeof(BulldozeTool).GetField(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}
