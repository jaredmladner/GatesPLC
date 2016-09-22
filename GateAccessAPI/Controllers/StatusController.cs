using GateAccessAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;

namespace GateAccessAPI.Controllers
{
    [RoutePrefix("status")]
    public class StatusController : ApiController
    {

        // GET: api/status
        [Route("")]
        public IEnumerable<string> Get()
        {
            List<string> registeries = Driver.ModbusTcpMasterReadInputs(Convert.ToUInt16(GateConstants.addressMap[GateConstants.ACP]), Convert.ToUInt16(1));

            return registeries.ToArray();

        }
        // GET: api/status/{register}/{numReg}
        [Route("{register:int}/{numReg:int}")]
        public IEnumerable<string> GetByAdress(int register, int numReg)
        {
            List<string> registeries = Driver.ModbusTcpMasterReadInputs(Convert.ToUInt16(register), Convert.ToUInt16(numReg));
            return registeries.ToArray();
        }

        // GET: api/status/acp
        [Route("acp")]
        public IEnumerable<Status> GetACPStatus()
        {
            List<string> registeries = Driver.ModbusTcpMasterReadInputs(Convert.ToUInt16(GateConstants.addressMap["ACP"]), Convert.ToUInt16(2));
            string acpEqModeBinarVal = getBinaryVal(registeries[0]);
            string acpModeBinaryVal = getBinaryVal(registeries[1]);
            // get chars 2,3,4,5 from the right to determine status: 0 0 0 0 0 0 0 0 0 0 '0 1 0 0' 0 1
            string acpEqModeBinary = acpEqModeBinarVal.Substring(10, 4);
            string acpEqModeStatusVal = Convert.ToInt32(acpEqModeBinary, 2).ToString();
            string acpEqMode = acpEqModeBinarVal.Substring(15, 1);
            string acpModeBinary = acpModeBinaryVal.Substring(10, 4);
            string acpModeStatusVal = Convert.ToInt32(acpModeBinary, 2).ToString();
            var data = new List<Status>
                          {
                             new Status { name = "ACP", code = acpEqMode,  status = GateConstants.statusCodes[GateConstants.ACP][acpEqMode] },
                             new Status { name = "ACP Equipment Mode", code = acpEqModeStatusVal,  status = GateConstants.statusCodes[GateConstants.ACPEquipmentMode][acpEqModeStatusVal] },
                             new Status { name = "ACP Mode", code = acpModeStatusVal,  status = GateConstants.statusCodes[GateConstants.ACPMode][acpModeStatusVal] }
                          };
            return data;

        }

        private string getBinaryVal(string value)
        {
            // get decimal value as string
            string decimalStr = value;
            string decimalVal = decimalStr.Substring(decimalStr.IndexOf("=") + 1);
            // get the binary value
            string shortBinaryVal = Convert.ToString(Convert.ToInt32(decimalVal, 10), 2);
            // pad to be 16 char long for parsing
            return shortBinaryVal.PadLeft(16, '0');
        }

        // GET: api/status/lane
        [Route("lane")]
        public String GetLaneStatus()
        {

            List<string> registeries = Driver.ModbusTcpMasterReadInputs(Convert.ToUInt16(110), Convert.ToUInt16(1));

            var data = new
            {
                Address = 110,
                Registers = registeries
            };
            string json = JsonConvert.SerializeObject(data);
            return json;

        }

        // GET: api/status/lane/1
        [Route("lane/{id:int}")]
        public IEnumerable<Status> GetLaneStatusById(int id)
        {
            List <string> laneReg = Driver.ModbusTcpMasterReadInputs(Convert.ToUInt16(GateConstants.addressMap["Lane" + id]), Convert.ToUInt16(GateConstants.addressMap["LaneOffSet" + id]));
            List <string> vehicleCountReg = Driver.ModbusTcpMasterReadInputs(Convert.ToUInt16(GateConstants.addressMap["VehicleCountLane" + id]), Convert.ToUInt16(1));
            string laneEqModeBinaryVal = getBinaryVal(laneReg[0]);
            string laneVTGBinaryVal = null;
            string laneVTBBinaryVal = null;

            // lanes 1 & 2 will have all 5: eqMode, drop arm, reject drop arm, VTG, and VTB
            if (id == 1 || id == 2)
            {
                laneVTGBinaryVal = getBinaryVal(laneReg[3]);
                laneVTBBinaryVal = getBinaryVal(laneReg[4]);
            }
            // lanes 3 & 4 will only have 3: eqMode, VTG, and VTB
            else
            {
                laneVTGBinaryVal = getBinaryVal(laneReg[1]);
                laneVTBBinaryVal = getBinaryVal(laneReg[2]);
            }
            string vehicleCountBinaryVal = vehicleCountReg[0].Substring(vehicleCountReg[0].IndexOf("=") + 1);

            // get chars 2,3,4,5 from the right to determine status: 0 0 0 0 0 0 0 0 0 0 '0 1 0 0' 0 1
            // get lane eq mode
            string laneEqModeBinary = laneEqModeBinaryVal.Substring(10, 4);
            string laneEqModeStatusVal = Convert.ToInt32(laneEqModeBinary, 2).ToString();
            // get lane vtg
            string laneVTGBinary = laneVTGBinaryVal.Substring(9, 3);
            string laneVTGBinaryStatusVal = Convert.ToInt32(laneVTGBinary, 2).ToString();
            // get lane vtb
            string laneVTBBinary = laneVTBBinaryVal.Substring(9, 3);
            string laneVTBBinaryStatusVal = Convert.ToInt32(laneVTBBinary, 2).ToString();

            var data = new List<Status>
                          {
                             new Status { name = "Lane " + id + ": Equipment Mode", code = laneEqModeStatusVal,  status = GateConstants.statusCodes[GateConstants.LaneEqMode][laneEqModeStatusVal] },
                             new Status { name = "Lane " + id + ": VTG", code = laneVTGBinaryStatusVal, status = GateConstants.statusCodes[GateConstants.LaneVTG][laneVTGBinaryStatusVal] },
                             new Status { name = "Lane " + id + ": VTB", code = laneVTBBinaryStatusVal, status = GateConstants.statusCodes[GateConstants.LaneVTB][laneVTBBinaryStatusVal] }
                          };
            if (id == 1 || id == 2 )
            {
                string laneDropArmBinaryVal = getBinaryVal(laneReg[1]);
                string laneRejectDropArmBinaryVal = getBinaryVal(laneReg[2]);

                // get lane drop arm 
                string laneDropArmBinary = laneDropArmBinaryVal.Substring(9, 3);
                string laneDropArmStatusVal = Convert.ToInt32(laneDropArmBinary, 2).ToString();
                // get lane reject drop arm
                string laneRejectDropArmBinary = laneRejectDropArmBinaryVal.Substring(9, 3);
                string laneRejectDropArmStatusVal = Convert.ToInt32(laneRejectDropArmBinary, 2).ToString();
                data.Add(new Status { name = "Lane " + id + ": Reject Drop Arm", code = laneRejectDropArmStatusVal, status = GateConstants.statusCodes[GateConstants.LaneRejectDropArm][laneRejectDropArmStatusVal] });
                data.Add(new Status { name = "Lane " + id + ": Drop Arm", code = laneDropArmStatusVal, status = GateConstants.statusCodes[GateConstants.LaneDropArm][laneDropArmStatusVal] });
            }

            data.Add(new Status { name = "Lane " + id + ": VehicleCount", status = vehicleCountBinaryVal });
           return data;

        }

        // POST: api/Status
        [Route("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Status/5
        [Route("")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Status/5
        [Route("")]
        public void Delete(int id)
        {
        }
    }
}
