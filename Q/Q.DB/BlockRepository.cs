using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.DB
{
    public class BlockRepository
    {
        public static void Add(Data.Models.Block block)
        {
            using (Context db = new Context())
            {
                db.Add(new DBO.Block()
                {
                    Hash = block.Hash,
                    MerkleRoot = block.MerkleRoot,
                    PreviousBlockHash = block.PreviousBlockHash,
                    Timestamp = block.Timestamp,
                    Version = block.Version,
                    Nonce = block.Nonce,
                    Height = block.Height,
                    Difficulty = block.Difficulty
                });
                db.SaveChanges();
            }
        }

        public static Data.Models.Block GetBlock(int height)
        {
            using (Context db = new Context())
            {
                DBO.Block block = db.Blocks.First(block => block.Height == height);
                return ConvertBlock(block);
            }
        }

        public static Data.Models.Block GetBlock(string hash)
        {
            using (Context db = new Context())
            {
                DBO.Block block = db.Blocks.First(block => block.Hash == hash);
                return ConvertBlock(block);
            }
        }

        public static Data.Models.Block GetLastBlock()
        {
            using (Context db = new Context())
            {
                DBO.Block block = db.Blocks.OrderByDescending(block => block.Height).FirstOrDefault();
                return ConvertBlock(block);
            }
        }

        public static void Clear()
        {
            using (Context db = new Context())
            {
                db.Blocks.RemoveRange(from row in db.Blocks select row);
                db.SaveChanges();
            }
        }

        private static Data.Models.Block ConvertBlock(DBO.Block block)
        {
            if (block != null)
            {
                return new Data.Models.Block()
                {
                    PreviousBlockHash = block.PreviousBlockHash,
                    Timestamp = block.Timestamp,
                    Version = block.Version,
                    Nonce = block.Nonce,
                    Height = block.Height,
                    Difficulty = block.Difficulty
                };
            }
            return null;
        }
    }
}
