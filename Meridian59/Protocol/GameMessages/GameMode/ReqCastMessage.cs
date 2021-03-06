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
using Meridian59.Common.Constants;
using Meridian59.Protocol.Enums;
using Meridian59.Data.Models;

namespace Meridian59.Protocol.GameMessages
{
    [Serializable]
    public class ReqCastMessage : GameModeMessage
    {       
        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                int len = base.ByteLength + TypeSizes.INT + TypeSizes.SHORT;

                foreach(ObjectID ID in Targets)
                    len += ID.ByteLength;

                return len;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, StartIndex);

            Array.Copy(BitConverter.GetBytes(SpellID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Targets.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            foreach (ObjectID ID in Targets)
                cursor += ID.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);

            SpellID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Targets = new ObjectID[len];
            for (int i = 0; i < len; i++)
            {
                Targets[i] = new ObjectID(Buffer, cursor);
                cursor += Targets[i].ByteLength;
            }

            return cursor - StartIndex;
        }
        #endregion
        
        public uint SpellID { get; set; }
        public ObjectID[] Targets { get; set; }
        
        public ReqCastMessage(uint SpellID, ObjectID[] Targets) 
            : base(MessageTypeGameMode.ReqCast)
        {
            this.SpellID = SpellID;
            this.Targets = Targets;  
        }

        public ReqCastMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }        
    }
}
