using System;
using System.Collections.Generic;

namespace HexGridNamespace
{
    [Serializable]
    public class ElementColorData
    {
        public List<ValueToColorPair> ValueToColorPairs;
        
        public UnityEngine.Color GetColor(ElementValue value)
        {
            foreach (var pair in ValueToColorPairs)
            {
                if (pair.Value == value)
                {
                    return pair.Color;
                }
            }

            return UnityEngine.Color.white;
        }
    }
    
    [Serializable]
    public class ValueToColorPair
    {
        public ElementValue Value;
        public UnityEngine.Color Color;
    }
}