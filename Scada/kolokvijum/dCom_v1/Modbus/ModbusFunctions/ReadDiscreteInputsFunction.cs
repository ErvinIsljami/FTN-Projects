using Common;
using Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Modbus.ModbusFunctions
{
    /// <summary>
    /// Class containing logic for parsing and packing modbus read discrete inputs functions/requests.
    /// </summary>
    public class ReadDiscreteInputsFunction : ModbusFunction
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadDiscreteInputsFunction"/> class.
        /// </summary>
        /// <param name="commandParameters">The modbus command parameters.</param>
        public ReadDiscreteInputsFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
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
                int brojac = 0;
                byte maska = 1;
                ushort value;
                ushort adresa = ((ModbusReadCommandParameters)CommandParameters).StartAddress;
                for (int i = 0; i < response[8]; i++)
                {
                    byte temp = response[i + 9];
                    for (int j = 0; j < 8; j++)
                    {
                        value = (ushort)(temp & maska);
                        temp >>= 1;
                        dict.Add(new Tuple<PointType, ushort>(PointType.DIGITAL_INPUT, adresa), value);
                        brojac++;
                        adresa++;
                        if (brojac == ((ModbusReadCommandParameters)CommandParameters).Quantity)
                        {
                            break;
                        }

                    }

                }


            }

            return dict;
        }
	}
}