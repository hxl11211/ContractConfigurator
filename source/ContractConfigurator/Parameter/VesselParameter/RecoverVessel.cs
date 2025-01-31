﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP;
using Contracts;
using Contracts.Parameters;
using KSP.Localization;

namespace ContractConfigurator.Parameters
{
    /// <summary>
    /// Parameter for checking that a vessel is recovered.
    /// </summary>
    public class RecoverVessel : VesselParameter
    {
        protected Dictionary<Vessel, bool> recovered = new Dictionary<Vessel, bool>();
        
        public RecoverVessel()
            : this(null)
        {
        }

        public RecoverVessel(string title)
            : base(title)
        {
            this.title = title != null ? title : Localizer.GetStringByTag("#autoLOC_5050049");

            disableOnStateChange = true;
        }

        protected override void OnParameterSave(ConfigNode node)
        {
            base.OnParameterSave(node);
        }

        protected override void OnParameterLoad(ConfigNode node)
        {
            base.OnParameterLoad(node);
        }

        protected override void OnRegister()
        {
            base.OnRegister();
            GameEvents.onVesselRecovered.Add(OnVesselRecovered);
        }

        protected override void OnUnregister()
        {
            base.OnUnregister();
            GameEvents.onVesselRecovered.Remove(OnVesselRecovered);
        }

        private void OnVesselRecovered(ProtoVessel v, bool quick)
        {
            // Don't check if we're not ready to complete
            if (!ReadyToComplete())
            {
                return;
            }

            // Special handling for null vessel cases
            if (v.vesselRef == null)
            {
                VesselParameterGroup vpg = GetParameterGroupHost();
                if (vpg == null)
                {
                    SetState(ParameterState.Complete);
                }
                else
                {

                }
            }

            recovered[v.vesselRef] = true;

            CheckVessel(v.vesselRef);
        }

        /// <summary>
        /// Whether this vessel meets the parameter condition.
        /// </summary>
        /// <param name="vessel">The vessel to check</param>
        /// <returns>Whether the vessel meets the condition</returns>
        protected override bool VesselMeetsCondition(Vessel vessel)
        {
            LoggingUtil.LogVerbose(this, "Checking VesselMeetsCondition: {0}", vessel.id);

            return recovered.ContainsKey(vessel) && recovered[vessel];
        }
    }
}
