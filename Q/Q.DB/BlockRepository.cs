using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Q.Data.Models.Struct;

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

            //Store block data
            for (int i = 0; i < block.Data.Count; i++)
            {
                BlockData blockData = block.Data[i];
                
                //Users
                if (blockData is BlockDataRegistration)
                {
                    BlockDataRegistration registrationData = (BlockDataRegistration)blockData;
                    UserRepository.Add(registrationData, i, block.Hash);
                }
                //Transactions
                else if (blockData is BlockDataTransaction)
                {
                    BlockDataTransaction transactionData = (BlockDataTransaction)blockData;
                    TransactionRepository.Add(transactionData, i, block.Hash);
                }
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
            UserRepository.Clear();
            TransactionRepository.Clear();

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
                    Difficulty = block.Difficulty,
                    Data = GetBlockData(block)
                };
            }
            return null;
        }

        private static List<BlockData> GetBlockData(DBO.Block block)
        {
            using (Context db = new Context())
            {
                var users = from user in db.Users where user.BlockHash == block.Hash orderby user.DataIndex select user;
                return users.Select(user => new BlockDataRegistration()
                {
                    PublicKey = user.PublicKey,
                    Alias = user.Alias,
                    Timestamp = user.Timestamp
                }).ToList<BlockData>();
            }
        }
    }
}
