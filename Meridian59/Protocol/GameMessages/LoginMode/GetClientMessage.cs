﻿/*
 Copyright (c) 2012-2013 Clint Banzhaf
 This file is part of "Meridian59 .NET".

 "Meridian59 .NET" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59 .NET" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59 .NET".
 If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Text;
using Meridian59.Common.Constants;
using Meridian59.Protocol.Enums;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// Sent from the server to the client in response to a not accepted 
    /// major/minor client version received in previous 'LoginMessage'.
    /// This signals a client update.
    /// </summary>
    [Serializable]
    public class GetClientMessage : LoginModeMessage
    {
        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + TypeSizes.SHORT + UpdateURL.Length + TypeSizes.SHORT + FileName.Length;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(UpdateURL.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(UpdateURL), 0, Buffer, cursor, UpdateURL.Length);
            cursor += UpdateURL.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(FileName.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(FileName), 0, Buffer, cursor, FileName.Length);
            cursor += FileName.Length;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);
         
            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            UpdateURL = Encoding.Default.GetString(Buffer, cursor, len);
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            FileName = Encoding.Default.GetString(Buffer, cursor, len);
            cursor += len;

            return cursor - StartIndex;
        }
        #endregion
        
        public string UpdateURL { get; set; }
        public string FileName { get; set; }

        public GetClientMessage(string UpdateURL, string FileName) 
            : base(MessageTypeLoginMode.GetClient)
        {
            this.UpdateURL = UpdateURL;
            this.FileName = FileName;                    
        }

        public GetClientMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }       
    }
}
