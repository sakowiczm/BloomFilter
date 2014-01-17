using System;
using System.Collections;
using System.Collections.Generic;

namespace BloomFilterTests
{
    public class BloomFilter
    {
        private readonly int _bits;
        private readonly IEnumerable<Func<string, int>> _hashes;
        private readonly BitArray _filter; 

        public BloomFilter(int bits, IList<Func<string, int>> hashes)
        {
            if(hashes == null) 
                throw new ArgumentNullException("hashes");

            if(hashes.Count == 0)
                throw new ArgumentException("Collection cannot be empty!", "hashes");

            _bits = bits;
            _hashes = hashes;
            _filter = new BitArray(bits);
        }

        public void Add(string data)
        {
            foreach (var @function in _hashes)
            {
                int hash = Math.Abs(@function(data));

                _filter.Set(hash % _bits, true);
            }
        }

        public bool Lookup(string data)
        {
            foreach(var @function in _hashes)
            {
                int hash = Math.Abs(@function(data));

                if (_filter[hash % _bits] == false)
                    return false;

            }

            return true;
        }

        public static double FalsePositiveProbability(int bitSize, int setSize, int numberOfHashes)
        {
            if (setSize == 0)
                throw new ArgumentOutOfRangeException("bitSize");

            return Math.Pow((1 - Math.Exp(-numberOfHashes * setSize / (double)bitSize)), numberOfHashes);
        }

        public static int OptimalNumberofHashes(int bitSize, int setSize)
        {
            if(setSize == 0)
                throw new ArgumentOutOfRangeException("setSize");

            return (int)Math.Ceiling((bitSize / setSize) * Math.Log(2.0));
        }
    }
}