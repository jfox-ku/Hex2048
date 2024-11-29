using UnityEngine;

namespace HexGridNamespace
{
    public static class ElementValueExtensions
    {
        public static ElementValue GetRandom(int max)
        {
            return (ElementValue) Random.Range(0, max);
        }
        
        public static ElementValue GetNext(this ElementValue value)
        {
            return value + 1;
        }
    }
}