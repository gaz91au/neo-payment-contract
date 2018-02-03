using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace neo_payments
{
    public class Program : SmartContract
    {
        public static object Main(string operation, params object[] args)
        {
            switch (operation)
            {
                case OperationType.BALANCE:
                    return Balance((byte[])args[0]);

                case OperationType.TRANSFER:
                    return Transfer((byte[])args[0], (byte[])args[1]);

                case OperationType.DELETE:
                    return Delete(((byte[])args[0]));

                default:
                    return false;
            }
        }
        private static byte[] Balance(byte[] asset)
        {
            return Storage.Get(Storage.CurrentContext, asset);
        }

        private static bool Transfer(byte[] asset, byte[] to)
        {
            if (!Runtime.CheckWitness(to)) return false;
            byte[] from = Storage.Get(Storage.CurrentContext, asset);
            if (from == null) return false;
            if (!Runtime.CheckWitness(from)) return false;
            Storage.Put(Storage.CurrentContext, asset, to);
            return true;
        }

        private static bool Delete(byte[] asset)
        {
            byte[] source = Storage.Get(Storage.CurrentContext, asset);
            if (source == null) return false;
            if (!Runtime.CheckWitness(source)) return false;
            Storage.Delete(Storage.CurrentContext, asset);
            return true;
        }
    }
}
