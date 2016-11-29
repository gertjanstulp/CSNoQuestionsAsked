using ColossalFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NoQuestionsAsked
{
    public class BulldozeToolExtender
    {
        private readonly BulldozeTool _bulldozeTool;

        public BulldozeToolExtender()
        {
            _bulldozeTool = GameObject.FindObjectOfType<BulldozeTool>();
        }

        public void DeleteBuildingImpl(ushort building)
        {
            // This method is copied from the Assembly-CSharp.dll assembly as provided by CO and then modified to allow for mass-bulldozing all 
            // assets. By default you can only delete one 'confirmable' asset per mouse click, so you cannot bulldoze multiple assets by just 
            // dragging the mouse accross assets (which is possible for roads, trees, RCI buildings, etc). A check was in place for this in this 
            // method on the placementStyle of the building, which had to be Automatic for mass-bulldozing to be possible. This check has been 
            // removed, so mass-bulldozing is possible now on all building types. Note: Just like with roads, trees and RCI assets, when you 
            // start mass-bulldozing a specific item you can only mass-bulldoze other items that are of the same type (you cannot bulldoze roads 
            // and trees in one sweep). This behavior is still applied here, so for example, if you start mass-bulldozing a park asset you can 
            // only mass-bulldoze other park assets and not unique-building assets in one sweep (this is done by setting the m_bulldozingService 
            // field).
            try
            {
                ModLogger.Debug("Starting DeleteBuildingImpl for building {0}", building);

                if (Singleton<BuildingManager>.instance.m_buildings.m_buffer[(int)building].m_flags != Building.Flags.None)
                {
                    BuildingManager instance = Singleton<BuildingManager>.instance;
                    BuildingInfo info = instance.m_buildings.m_buffer[(int)building].Info;
                    if (info.m_buildingAI.CheckBulldozing(building, ref instance.m_buildings.m_buffer[(int)building]) != ToolBase.ToolErrors.None)
                    {
                        return;
                    }
                    SetFieldValue<BulldozeTool.Mode>("m_bulldozingMode", BulldozeTool.Mode.Building);
                    SetFieldValue<ItemClass.Service>("m_bulldozingService", info.m_class.m_service);
                    SetFieldValue<ItemClass.Layer>("m_bulldozingLayers", info.m_class.m_layer);
                    SetFieldValue<float>("m_deleteTimer", 0.1f);
                    int buildingRefundAmount = this.GetBuildingRefundAmount(building);
                    if (buildingRefundAmount != 0)
                    {
                        Singleton<EconomyManager>.instance.AddResource(EconomyManager.Resource.RefundAmount, buildingRefundAmount, info.m_class);
                    }
                    Vector3 position = instance.m_buildings.m_buffer[(int)building].m_position;
                    float angle = instance.m_buildings.m_buffer[(int)building].m_angle;
                    int width = instance.m_buildings.m_buffer[(int)building].Width;
                    int length = instance.m_buildings.m_buffer[(int)building].Length;
                    bool collapsed = (instance.m_buildings.m_buffer[(int)building].m_flags & Building.Flags.Collapsed) != Building.Flags.None;
                    instance.ReleaseBuilding(building);
                    int publicServiceIndex = ItemClass.GetPublicServiceIndex(info.m_class.m_service);
                    if (publicServiceIndex != -1)
                    {
                        Singleton<CoverageManager>.instance.CoverageUpdated(info.m_class.m_service, info.m_class.m_subService, info.m_class.m_level);
                    }
                    BuildingTool.DispatchPlacementEffect(info, building, position, angle, width, length, true, collapsed);
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
            finally
            {
                ModLogger.Debug("Finishing DeleteBuildingImpl for building {0}", building);
            }
        }

        public void DeleteBuilding(ushort buildingId)
        {
            ModLogger.Debug("Starting DeleteBuilding for building {0}", buildingId);
            
            // Reflection wrapper for invoking the private DeleteBuilding method of the default CO bulldoze tool and returning its' result
            MethodInfo method = typeof(BulldozeTool).GetMethod("DeleteBuilding", BindingFlags.NonPublic | BindingFlags.Instance);
            Singleton<SimulationManager>.instance.AddAction((IEnumerator)method.Invoke(_bulldozeTool, new object[] { buildingId }));

            ModLogger.Debug("Finishing DeleteBuilding for building {0}", buildingId);
        }

        private int GetBuildingRefundAmount(ushort building)
        {
            // Reflection wrapper for invoking the private GetBuildingRefundAmount method of the default CO bulldoze tool and returning its' result
            MethodInfo method = typeof(BulldozeTool).GetMethod("GetBuildingRefundAmount", BindingFlags.NonPublic | BindingFlags.Instance);
            return (int)method.Invoke(_bulldozeTool, new object[] { building });
        }

        private T GetFieldValue<T>(string fieldName)
        {
            // Reflection wrapper for getting the value of a private field of the default CO bulldoze tool
            return (T)GetField(fieldName).GetValue(_bulldozeTool);
        }

        private void SetFieldValue<T>(string fieldName, T propertyValue)
        {
            // Reflection wrapper for setting the value of a private field of the default CO bulldoze tool
            GetField(fieldName).SetValue(_bulldozeTool, propertyValue);
        }

        private FieldInfo GetField(string propertyName)
        {
            return typeof(BulldozeTool).GetField(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}
