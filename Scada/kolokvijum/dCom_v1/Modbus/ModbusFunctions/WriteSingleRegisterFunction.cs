using Common;
using Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Modbus.ModbusFunctions
{
    /// <summary>
    /// Class containing logic for parsing and packing modbus write single register functions/requests.
    /// </summary>
    public class WriteSingleRegisterFunction : ModbusFunction
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteSingleRegisterFunction"/> class.
        /// </summary>
        /// <param name="commandParameters">The modbus command parameters.</param>
        public WriteSingleRegisterFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
		{
			CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusWriteCommandParameters));
		}

        /// <inheritdoc />
        public override byte[] PackRequest()
        {
            byte[] pack = new byte[12];
            ModbusWriteCommandParameters param = CommandParameters as ModbusWriteCommandParameters;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)param.TransactionId)), 0, pack, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)param.ProtocolId)), 0, pack, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)param.Length)), 0, pack, 4, 2);
            pack[6] = param.UnitId;
            pack[7] = param.FunctionCode;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)param.OutputAddress)), 0, pack, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)param.Value)), 0, pack, 10, 2);


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
                ushort adresa = BitConverter.ToUInt16(response, 8);
                ushort value = BitConverter.ToUInt16(response, 10);
                adresa = (ushort)IPAddress.NetworkToHostOrder((short)adresa);
                value = (ushort)IPAddress.NetworkToHostOrder((short)value);
                dict.Add(new Tuple<PointType, ushort>(PointType.ANALOG_OUTPUT, adresa), value);
            }

            
            return dict;
        }
	}
}