using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GateAccessAPI
{
    public class GateConstants
    {
        public const string ACP = "ACP";
        public const string ACPEquipmentMode = "ACPEquipmentMode";
        public const string ACPMode = "ACPMode";
        public const string LaneDropArm = "LaneDropArm";
        public const string LaneRejectDropArm = "LaneRejectDropArm";
        public const string LaneVTG = "LaneVTG";
        public const string LaneVTB = "LaneVTB";
        public const string LaneStatus = "LaneStatus";
        public const string LaneEqMode = "LaneEqMode";
        public const string Lane = "Lane";
        public const string VehicleCountOffset = "VehicleCountOffset";
        public static Dictionary<string, Dictionary<string, string>> statusCodes = new Dictionary<string, Dictionary<string, string>>()
        {
            {
                ACP,
                new Dictionary<string, string>
                {
                    {"0", "Unknown"},
                    {"1", "Auto"}
                }
            },
            {
                ACPEquipmentMode,
                new Dictionary<string, string>
                {
                    {"0", "Unknown"},
                    {"1", "Off Mode"},
                    {"2", "Local Device Mode"},
                    {"3", "Maintenance Interface"},
                    {"4", "PLC Auto Mode"}
                }
            },
            {
                ACPMode,
                new Dictionary<string, string>
                {
                    {"0", "Unknown"},
                    {"1", "Closed"},
                    {"2", "Manual"},
                    {"3", "Automated"},
                    {"4", "All Stop"}
                }
            },
            {
                LaneDropArm,
                new Dictionary<string, string>
                {
                    {"0", "Unknown"},
                    {"1", "Closing"},
                    {"2", "Closed"},
                    {"3", "Opening"},
                    {"4", "Opened"}
                }
            },
            {
                LaneRejectDropArm,
                new Dictionary<string, string>
                {
                    {"0", "Unknown"},
                    {"1", "Closing"},
                    {"2", "Closed"},
                    {"3", "Opening"},
                    {"4", "Opened"}
                }
            },
             {
                LaneVTG,
                new Dictionary<string, string>
                {
                    {"0", "Unknown"},
                    {"1", "Closing"},
                    {"2", "Closed"},
                    {"3", "Opening"},
                    {"4", "Opened"}
                }
            },
            {
                LaneVTB,
                new Dictionary<string, string>
                {
                    {"0", "Unknown"},
                    {"1", "Closing"},
                    {"2", "Closed"},
                    {"3", "Opening"},
                    {"4", "Opened"}
                }
            },
           {
                LaneStatus,
                new Dictionary<string, string>
                {
                    {"0", "Blocked"},
                    {"1", "Tailgating"},
                    {"2", "Gate Crashed"}
                }
            },
            {
                LaneEqMode,
                new Dictionary<string, string>
                {
                    {"0", "Unknown"},
                    {"1", "Off Mode"},
                    {"2", "Local Device Mode"},
                    {"3", "Maintenance Interface Mode"},
                    {"4", "PLC Auto Mode"}
                }
            }
        };
        public static Dictionary<string, int> addressMap = new Dictionary<string, int>()
        {
            { ACP, 10 },
            { "Lane1", 12 },
            { "Lane2", 17 },
            { "Lane3", 22 },
            { "Lane4", 25 },
            { "LaneOffSet1", 5 },
            { "LaneOffSet2", 5 },
            { "LaneOffSet3", 3 },
            { "LaneOffSet4", 3 },
            { "VehicleCountLane1", 28 },
            { "VehicleCountLane2", 29 },
            { "VehicleCountLane3", 30 },
            { "VehicleCountLane4", 31 }
        };
    }
}