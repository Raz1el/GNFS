using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Integer_arithmetic;

namespace GNFS.ECM
{
    public class PrimeManager
    {

        ulong _low,_sieveStep;
        int _currentIndex;
        ulong[] _primes;
        EratosthenesSieve _sieve;

        public ulong NextPrime()
        {
            if (_currentIndex==_primes.Length)
            {
                _low += _sieveStep;
                _primes = _sieve.GetPrimes(_low+1, _low + _sieveStep);
                _currentIndex = 0;

            }
            return _primes[_currentIndex++];
        }

        public void Init(ulong start)
        {
            _low = start;
            _sieveStep = 10000000;
            _sieve=new EratosthenesSieve();
            _primes = _sieve.GetPrimes(_low, _low + _sieveStep);
            _currentIndex = 0;
        }
    }
}
