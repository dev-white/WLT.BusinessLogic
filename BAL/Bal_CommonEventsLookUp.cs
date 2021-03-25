using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLT.BusinessLogic.BAL
{
  public  class Bal_CommonEventsLookUp
    {
        public string GetCommonEventName(int EventId, long ImeiNumber, int DriverID, string CurrentEventName)
        {
            string eventName;

            switch (EventId)
            {

                case 1:
                    eventName = "overspeed - device speed limit";
                    break;

                case 2:
                    eventName = "harsh braking";
                    break;

                case 3:
                    eventName = "harsh acceleration";
                    break;

                case 4:
                    eventName = "excessive idle";
                    break;

                case 5:
                    eventName = "tow";
                    break;

                case 6:
                    eventName = "overspeed off";
                    break;

                case 7:
                    eventName = "ign on";
                    break;

                case 8:
                    eventName = "ign off";
                    break;

                case 9:
                    eventName = "engine on";
                    break;

                case 10:
                    eventName = "engine off";
                    break;

                case 11:
                    eventName = "excessive idle off";
                    break;

                case 12:
                    eventName = "motion on";
                    break;

                case 13:
                    eventName = "motion off";
                    break;

                case 14:
                    eventName = "harsh cornering";
                    break;
                case 15:
                    eventName = "harsh cornering off";
                    break;

                case 16:
                    eventName = "tow off";
                    break;

                case 17:
                    eventName = "unit powering on";
                    break;

                case 18:
                    eventName = "unit powering off (int batt only)";
                    break;

                case 19:
                    eventName = "GSM Jamming Detected";
                    break;

                case 20:
                    eventName = "impact event";
                    break;

                case 21:
                    eventName = "engine over revving";
                    break;

                case 22:
                    eventName = "main power low";
                    break;

                case 23:
                    eventName = "main power is normal";
                    break;

                case 24:
                    eventName = "main power supply connected";
                    //eventName = eventName + " (" + GetLastKnownBattery(ImeiNumber) + "%)"; 
                    break;

                case 25:
                    eventName = "main power supply disconnected (int batt only)";
                    break;

                case 26:
                    eventName = "internal backup battery started charging";
                    eventName = eventName + " (" + GetLastKnownBattery(ImeiNumber) + "%)";
                    break;

                case 27:
                    eventName = "internal backup battery stopped charging";
                    break;

                case 28:
                    eventName = "internal backup battery low";
                    eventName = eventName + " (" + GetLastKnownBattery(ImeiNumber) + "%)";
                    break;

                case 29:
                    eventName = "internal backup battery connected";
                    //eventName = eventName + " (" + GetLastKnownBattery(ImeiNumber) + "%)"; 
                    break;

                case 30:
                    eventName = "internal backup battery removed";
                    eventName = eventName + " (last battery level " + GetLastKnownBattery(ImeiNumber) + "%)";
                    break;

                case 31:
                    eventName = "internal backup battery normal";
                    break;

                case 32:
                    eventName = "GSM Jamming Stopped";
                    break;

                case 33:
                    eventName = "authorised driver tagged";
                    if (DriverID > 0)
                    {
                        // use the ID to lookup drivername in driver table
                        eventName = eventName + ": " + GetDriverName(DriverID);
                    }
                    break;

                case 34:
                    eventName = "unauthorised driver tagged";
                    if (DriverID > 0)
                    {
                        // use the ID to lookup drivername in driver table
                        eventName = eventName + ": " + GetDriverName(DriverID);
                    }
                    break;

                case 35:
                    eventName = "immobilizer on";
                    break;

                case 36:
                    eventName = "immobilizer off";
                    break;

                case 37:
                    eventName = "Engine over revving stopped";
                    break;

                case 38:
                    eventName = "harsh braking off";
                    break;

                case 39:
                    eventName = "harsh acceleration off";
                    break;

                case 40:
                    eventName = "unit is overheating";
                    break;

                case 41:
                    eventName = "Tamper - unit case/shell opened";
                    break;

                case 42:
                    eventName = "Fingerprint Log in";
                    break;

                case 43:
                    eventName = "Fingerprint Log out";
                    break;

                case 44:
                    eventName = "Engine over heating";
                    break;

                case 45:
                    eventName = "Engine over heating stopped";
                    break;

                case 46:
                    eventName = "Fingerprint Log in";
                    break;

                case 47:
                    eventName = "UNUSED";
                    break;

                case 48:
                    eventName = "UNUSED";
                    break;

                case 49:
                    eventName = "UNUSED";
                    break;

                case 50:
                    eventName = "UNUSED";
                    break;

                case 51:
                    eventName = "UNUSED";
                    break;

                case 52:
                    eventName = "UNUSED";
                    break;

                case 53:
                    eventName = "UNUSED";
                    break;

                case 54:
                    eventName = "GPS Antenna Error";
                    break;

                case 55:
                    eventName = "GPS Antenna Error Cleared";
                    break;

                case 56:
                    eventName = "UNUSED";
                    break;

                case 57:
                    eventName = "UNUSED";
                    break;

                case 58:
                    eventName = "UNUSED";
                    break;

                //case 59:
                //                    eventName = "voltage is outside predefined ranged";
                //                    break;

                //case 60:
                //                    eventName = "voltage is within predefined ranged";
                //                    break;

                //case 61:
                //                    eventName = "Main power supply connected";
                //                    break;

                //case 62:
                //                    eventName = "Main power supply disconnected (int batt devices only)";
                //                    break;

                //case 63:
                //    eventName = "internal backup battery started charging";
                //    break;

                //case 64:
                //    eventName = "internal backup battery stopped charging";
                //    break;

                //case 65:
                //    eventName = "internal backup battery low";
                //    break;

                //case 66:
                //    eventName = "internal backup battery connected";
                //    break;

                //case 67:
                //    eventName = "internal backup battery removed";
                //    break;

                //case 68:
                //    eventName = "Tamper - unit case/shell opened";
                //    break;

                case 69:
                    eventName = "UNUSED";
                    break;

                case 70:
                    eventName = "UNUSED";
                    break;

                case 71:
                    eventName = "UNUSED";
                    break;

                case 72:
                    eventName = "UNUSED";
                    break;

                //case 73:
                //                    eventName = "Power Supply Voltage";
                //                    break;

                //case 74:
                //                    eventName = "Battery Voltage";
                //                    break;

                //case 75:
                //                    eventName = "Battery Current";
                //                    break;

                case 76:
                    eventName = "UNUSED";
                    break;

                case 77:
                    eventName = "UNUSED";
                    break;

                case 78:
                    eventName = "UNUSED";
                    break;
                case 79:
                    eventName = "UNUSED";
                    break;

                case 80:
                    eventName = "UNUSED";
                    break;

                case 81:
                    eventName = "UNUSED";
                    break;

                case 82:
                    eventName = "UNUSED";
                    break;

                case 83:
                    eventName = "UNUSED";
                    break;

                case 84:
                    eventName = "UNUSED";
                    break;

                case 85:
                    eventName = "UNUSED";
                    break;

                case 86:
                    eventName = "UNUSED";
                    break;

                case 87:
                    eventName = "UNUSED";
                    break;

                //case 88:
                //                    eventName = "UNUSED";
                //                    break;
                //case 89:
                //                    eventName = "UNUSED";
                //                    break;
                //case 90:
                //                    eventName = "UNUSED";
                //                    break;
                //case 91:
                //                    eventName = "UNUSED";
                //                    break;
                //case 92:
                //                    eventName = "UNUSED";
                //                    break;
                //case 93:
                //                    eventName = "UNUSED";
                //                    break;
                //case 94:
                //                    eventName = "UNUSED";
                //                    break;
                //case 95:
                //                    eventName = "UNUSED";
                //                    break;
                //case 96:
                //                    eventName = "UNUSED";
                //                    break;
                //case 97:
                //                    eventName = "UNUSED";
                //                    break;
                //case 98:
                //                    eventName = "UNUSED";
                //                    break;
                //case 99:
                //                    eventName = "UNUSED";
                //                    break;
                //case 100:
                //                    eventName = "UNUSED";
                //                    break;

                //case 101:
                //                    eventName = "UNUSED";
                //                    break;

                //case 102:
                //                    eventName = "UNUSED";
                //                    break;

                //case 103:
                //                    eventName = "UNUSED";
                //                    break;

                //case 104:
                //                    eventName = "UNUSED";
                //                    break;

                //case 105:
                //                    eventName = "UNUSED";
                //                    break;

                //case 106:
                //                    eventName = "UNUSED";
                //                    break;

                //case 107:
                //                    eventName = "UNUSED";
                //                    break;

                case 108:
                    eventName = "UNUSED";
                    break;

                case 109:
                    eventName = "UNUSED";
                    break;

                case 110:
                    eventName = "UNUSED";
                    break;

                case 111:
                    eventName = "UNUSED";
                    break;

                case 112:
                    eventName = "UNUSED";
                    break;

                case 113:
                    eventName = "UNUSED";
                    break;

                case 114:
                    eventName = "UNUSED";
                    break;

                case 115:
                    eventName = "UNUSED";
                    break;

                case 116:
                    eventName = "UNUSED";
                    break;

                case 117:
                    eventName = "UNUSED";
                    break;

                case 118:
                    eventName = "UNUSED";
                    break;

                case 119:
                    eventName = "UNUSED";
                    break;

                case 120:
                    eventName = "Digital Input 0 On";
                    break;

                case 121:
                    eventName = "Digital Input 0 Off";
                    break;

                case 122:
                    eventName = "GPRS reconnected";
                    break;

                case 123:
                    eventName = "UNUSED";
                    break;

                case 124:
                    eventName = "Position";
                    break;

                case 125:
                    eventName = "Event Not Configured in DB";
                    break;

                case 126:
                    eventName = "Ignition off and stationary";
                    break;

                case 127:
                    eventName = "";
                    break;

                case 128:
                    eventName = "ignition is on but the car is motionless";
                    break;

                case 129:
                    eventName = "Ignition is on and the vehicle is moving";
                    break;

                case 130:
                    eventName = "device is motionless, no ignition signal detected";
                    break;

                case 131:
                    eventName = "Motion Status Changed";
                    break;

                //132 empty

                case 133:
                    eventName = "Ignition off, vehicle moved not yet a tow";
                    break;

                case 134:
                    eventName = CheckIfCustomIOName("Digital Input 1 On", 134, ImeiNumber);
                    break;

                case 135:
                    eventName = CheckIfCustomIOName("Digital Input 1 Off", 135, ImeiNumber);
                    break;

                case 136:
                    eventName = CheckIfCustomIOName("Digital Input 2 On", 136, ImeiNumber);
                    break;

                case 137:
                    eventName = CheckIfCustomIOName("Digital Input 2 Off", 137, ImeiNumber);
                    break;

                case 138:
                    eventName = CheckIfCustomIOName("Digital Input 3 On", 138, ImeiNumber);
                    break;

                case 139:
                    eventName = CheckIfCustomIOName("Digital Input 3 Off", 139, ImeiNumber);
                    break;

                case 140:
                    eventName = CheckIfCustomIOName("Digital Input 4 On", 140, ImeiNumber);
                    break;

                case 141:
                    eventName = CheckIfCustomIOName("Digital Input 4 Off", 141, ImeiNumber);
                    break;

                case 142:
                    eventName = CheckIfCustomIOName("Digital Input 5 On", 142, ImeiNumber);
                    break;

                case 143:
                    eventName = CheckIfCustomIOName("Digital Input 5 Off", 143, ImeiNumber);
                    break;

                case 144:
                    eventName = CheckIfCustomIOName("Digital Input 6 On", 144, ImeiNumber);
                    break;

                case 145:
                    eventName = CheckIfCustomIOName("Digital Input 6 Off", 145, ImeiNumber);
                    break;

                case 146:
                    //eventName = "Digital Output 1 On";
                    eventName = CheckIfCustomIOName("Digital Output 1 On", 146, ImeiNumber);
                    break;

                case 147:
                    //eventName = "Digital Output 1 Off";
                    eventName = CheckIfCustomIOName("Digital Output 1 Off", 147, ImeiNumber);
                    break;

                case 148:
                    //eventName = "Digital Output 2 On";
                    eventName = CheckIfCustomIOName("Digital Output 2 On", 148, ImeiNumber);
                    break;

                case 149:
                    //eventName = "Digital Output 2 Off";
                    eventName = CheckIfCustomIOName("Digital Output 2 Off", 149, ImeiNumber);
                    break;

                case 150:
                    eventName = "OBD Report";
                    break;

                case 151:
                    eventName = "Fuel Value With Position Update";
                    break;

                case 152:
                    eventName = "Temperature Value With Position Update";
                    break;

                case 153:
                    eventName = "Panic button pressed";
                    break;

                case 154:
                    eventName = "Ignition status changed";
                    break;

                //case 155:
                //    eventName = "";
                //    break;
                //case 156:
                //    eventName = "";
                //    break;

                case 168:
                    eventName = "Power Cut Alarm";
                    break;
                    
                case 200:
                    eventName = "Obd unit plugged into vehcile";
                    break;

                case 201:
                    eventName = "Obd unit unplugged from vehcile";
                    break;

                //Entered Location
                case 800:
                    eventName = "entered location";
                    break;

                //Exited Location
                case 801:
                    eventName = "exited location";
                    break;

                //Entered Nogo
                case 802:
                    eventName = "entered no go zone";
                    break;

                //Exited nogo
                case 803:
                    eventName = "exited no go zone";
                    break;

                //Entered Keep in
                case 804:
                    eventName = "entered keep in zone";
                    break;

                //Exited Keep in
                case 805:
                    eventName = "exited keep in zone";
                    break;

                case 806:
                    eventName = "entered route";
                    break;

                case 807:
                    eventName = "overspeed in no-go";
                    break;

                case 808:
                    eventName = "overspeed in location";
                    break;

                //overspeed- roadspeed
                case 809:
                    eventName = "overspeed - within Geozone";
                    break;

                //overspeed- roadspeed
                case 810:
                    eventName = "overspeed - road speed";
                    break;

                //exit route
                case 811:
                    eventName = "exited route";
                    break;

                //sudden Decrease
                case 812:
                    eventName = CurrentEventName;
                    break;

                //sudden increase
                case 813:
                    eventName = CurrentEventName;
                    break;

                ////sudden Decrease AN2  - this should be depreciated for 813 globally
                //case 814:
                //    eventName = "na";
                //    break;

                ////sudden increase AN2 - this should be depreciated for 813 globally
                //case 815:
                //    eventName = "na";
                //    break;

                //Out of range
                case 816:
                    eventName = CurrentEventName;
                    break;
                //Out of range
                case 817:
                    eventName = CurrentEventName;
                    break;

                //Over Road Speed Limit
                case 820:
                    eventName = CurrentEventName;
                    break;

                //Over Road Speed Limit
                case 821:
                    eventName = CurrentEventName;
                    break;

                //Over Road Speed Limit
                case 822:
                    eventName = CurrentEventName;
                    break;

                //Over Road Speed Limit
                case 823:
                    eventName = CurrentEventName;
                    break;
                //Over Road Speed Limit
                case 824:
                    eventName = CurrentEventName;
                    break;
                //Over Road Speed Limit
                case 825:
                    eventName = CurrentEventName;
                    break;

                case 826:
                    eventName = "Entered proximity zone";
                    break;

                case 827:
                    eventName = "Exited proximity zone";
                    break;

                case 828:
                    eventName = "Speed Sensor Connected";
                    break;


                case 829:
                    eventName = "Speed Sensor Disconnected";
                    break;

                default:
                    eventName = "Event Name Not Found";
                    break;
            }

            return eventName;
        }

        public string GetLastKnownBattery(long ImeiNumber)
        {
            return "";

        }

        public string GetDriverName(int DriverID)
        {
            return "";
        }


        public string CheckIfCustomIOName(string io, int ioID, long ImeiNumber)
        {
            return "";
        }
    }
}
