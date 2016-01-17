using ColossalFramework;
using ColossalFramework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QuickBuildozer
{
    public class TryDeleteBuildingEnumerator : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object current;

        private BulldozeToolExtender _bulldozeTool;
        private ushort _buildingId;

        private bool _nextResult;

        public TryDeleteBuildingEnumerator(BulldozeTool bulldozeTool, ushort buildingId)
        {
            _bulldozeTool = new BulldozeToolExtender(bulldozeTool);
            _buildingId = buildingId;
            _nextResult = true;
        }

        object IEnumerator<object>.Current
        {
            get { return this.current; }
        }

        object IEnumerator.Current
        {
            get { return this.current; }
        }

        public bool MoveNext()
        {
            if (_nextResult)
            {
                _nextResult = false;
                current = 0;
                DoDelete();

                return true;
            }
            return false;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        private void DoDelete()
        {
            if (RequireConfirmation())
                DeleteWithConfirmation();
            else
                _bulldozeTool.DeleteBuildingImpl(_buildingId);
        }

        private bool RequireConfirmation()
        {
            var info = Singleton<BuildingManager>.instance.m_buildings.m_buffer[_buildingId].Info;
            if (info.m_placementStyle == ItemClass.Placement.Automatic) return false;

            if (Event.current == null) return false;

            bool confirmRequired = false;
            if (ConfigurationAccessor.Instance.UseAlt && !Event.current.alt)
                confirmRequired = true;
            if (ConfigurationAccessor.Instance.UseCtrl && !Event.current.control)
                confirmRequired = true;
            if (ConfigurationAccessor.Instance.UseShift && !Event.current.shift)
                confirmRequired = true;

            return confirmRequired;
        }

        private void DeleteWithConfirmation()
        {
            ConfirmPanel.ShowModal(LocaleID.CONFIRM_BUILDINGDELETE, delegate (UIComponent comp, int ret)
            {
                if (ret == 1)
                    _bulldozeTool.DeleteBuilding(_buildingId);
            });
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _nextResult = false;
                if (_bulldozeTool != null)
                    _bulldozeTool = null;
            }
        }
    }
}
