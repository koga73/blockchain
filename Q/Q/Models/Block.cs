using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Q.Common;

namespace Q.Models
{
    public class Block
    {
        public static int MIN_DIFFICULTY = 4; //Number of leading zeros
        public static int MAX_DIFFICULTY = 16; //Number of leading zeros

        public DateTime Timestamp;
        public int Version;
        public string Nonce;
        public int Height;
        public int Difficulty = MIN_DIFFICULTY;
        public List<BlockDataBase> Data;

        public string Hash
        {
            get
            {
                return ComputeHash(Nonce);
            }
        }
        public string MerkelRoot
        {
            get
            {
                return ComputeMerkelRoot(this.Data);
            }
        }

        public Block()
        {
            Timestamp = DateTime.Now;
            Version = 1;
        }

        public string ComputeHash(string nonce)
        {
            string state = $"{Timestamp.Ticks}-{Version}-{nonce}-{Height}-{MerkelRoot}";
            return Utils.ComputeHash(Utils.ComputeHash(state)); //Double hash
        }

        public string ComputeMerkelRoot(List<BlockDataBase> data)
        {
            if (data == null)
            {
                return null;
            }
            int dataLen = data.Count;
            int halfLen = (int)Math.Floor(dataLen / 2.0);
            List<BlockDataBase> left = data.GetRange(0, halfLen);
            List<BlockDataBase> right = data.GetRange(halfLen, dataLen - halfLen);
            switch (dataLen)
            {
                case 1:
                    return Utils.ComputeHash(data[0].Hash + data[0].Hash);
                case 2:
                    return Utils.ComputeHash(data[0].Hash + data[1].Hash);
                default:
                    return ComputeMerkelRoot(left) + ComputeMerkelRoot(right);
            }
        }

        public void AddData(BlockDataBase data)
        {
            if (Data == null)
            {
                Data = new List<BlockDataBase>();
            }
            Data.Add(data);
        }

        override public string ToString()
        {
            string flatData = "";
            if (Data != null)
            {
                foreach (BlockDataBase blockData in Data)
                {
                    flatData += "\n" + blockData;
                }
            } else
            {
                flatData = "null";
            }
            flatData = $"[ {flatData}\n ]";
            return $"{{ Timestamp:{Timestamp.Ticks} \"Version\":{Version} \"Nonce\":{(Nonce != null ? Nonce : "null")} \"Hash\":\"{Hash}\" \"MerkelRoot\":\"{MerkelRoot}\" \"Height\":{Height} \"Data\":{flatData} }}";
        }
    }
}
