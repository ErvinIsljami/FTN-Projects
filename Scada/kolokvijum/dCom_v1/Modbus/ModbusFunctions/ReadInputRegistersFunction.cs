using Common;
using Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Modbus.ModbusFunctions
{
    /// <summary>
    /// Class containing logic for parsing and packing modbus read input registers functions/requests.
    /// </summary>
    public class ReadInputRegistersFunction : ModbusFunction
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadInputRegistersFunction"/> class.
        /// </summary>
        /// <param name="commandParameters">The modbus command parameters.</param>
        public ReadInputRegistersFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
		{
			CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusReadCommandParameters));
		}

		/// <inheritdoc />
		public override byte[] PackRequest()
		{
            byte[] pack = new byte[12];
            ModbusReadCommandParameters param = CommandParameters as ModbusReadCommandParameters;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)param.TransactionId)), 0, pack, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)param.ProtocolId)), 0, pack, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)param.Length)), 0, pack, 4, 2);
            pack[6] = param.UnitId;
            pack[7] = param.FunctionCode;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)param.StartAddress)), 0, pack, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)param.Quantity)), 0, pack, 10, 2);


            return pack;
        }

		/// <inheritdoc />
		public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
		{
            Dictionary<Tuple<PointType, ushort>, ushort> dict = new Dictionary<Tuple<PointType, ushort>, ushort>();
            if (response[7] == CommandParameters.FunctionCode + 0x80)
            {
                HandeException(response[8]);
            }
            else
            {
                ushort value;
                ushort adresa = ((ModbusReadCommandParameters)CommandParameters).StartAddress;
                for (int i = 0; i < response[8]; i = i + 2)
                {
                    value = BitConverter.ToUInt16(response, (i + 9));
                    value = (ushort)IPAddress.NetworkToHostOrder((short)value);
                    dict.Add(new Tuple<PointType, ushort>(PointType.ANALOG_INPUT, adresa), value);
                    adresa++;
                }

            }





            return dict;
        }
	}
}