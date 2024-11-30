using System.Collections.Generic;
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

        public static string GetName(this ElementValue value)
        {
            return ElementNames[(int) value];
        }
        
        public static readonly List<string> ElementNames = new List<string>
        {
            "2",
            "4",
            "8",
            "16",
            "32",
            "64",
            "128",
            "256",
            "512",
            "1024",
            "2048",
            "4096",
        };

    }
}