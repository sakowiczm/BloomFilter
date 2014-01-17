using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HashLib;
using Xunit;

namespace BloomFilterTests
{
    public class BloomFilterTest
    {
        [Fact]
        public void Test1()
        {
            int m = 1000; // number of bits in the array

            IHash murmur2 = HashFactory.Hash32.CreateMurmur2();
            IHash jenkins = HashFactory.Hash32.CreateJenkins3();
            IHash fnv1a = HashFactory.Hash32.CreateFNV1a();

            var bf = new BloomFilter(m, new List<Func<string, int>>
                                        {
                                            //s => s.GetHashCode(),
                                            s => (int)murmur2.ComputeString(s).GetUInt(),
                                            s => (int)jenkins.ComputeString(s).GetUInt(),
                                            s => (int)fnv1a.ComputeString(s).GetUInt()
                                        });

            bf.Add("aaaaaaaa");
            bf.Add("bbbbbbbb");
            bf.Add("cccccccc");
            bf.Add("dddddddd");
            bf.Add("eeeeeeee");

            // false positives are accepted
            // false negatives are un-acceptable
            Assert.True(bf.Lookup("aaaaaaaa"));
            Assert.False(bf.Lookup("zzzzzzzz"));

            var falsePositiveProbability = BloomFilter.FalsePositiveProbability(1000, 5, 3);
            var optimalNumberofHashes = BloomFilter.OptimalNumberofHashes(1000, 5);

            Console.WriteLine(falsePositiveProbability); // should try to get this value to around 0.01
            Console.WriteLine(optimalNumberofHashes);
        }

    }
}
