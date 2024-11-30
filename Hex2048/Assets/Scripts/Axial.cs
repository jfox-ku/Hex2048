using System;
using System.Collections.Generic;

namespace HexGridNamespace
{
    [Serializable]
    public struct Axial
    {
        public int Q;
        public int R;
        public int S => -Q - R;
        
        public Axial(int q, int r)
        {
            Q = q;
            R = r;
        }

        public static Axial[] AxialDirectionVectors = new Axial[] { Up,Down,UpRight,UpLeft,DownRight,DownLeft };
        public static IEnumerable<Axial> Directions => AxialDirectionVectors;
        
        public static Axial Zero => new Axial(0, 0);
        public static Axial Up => new Axial(0, -1);
        public static Axial Down => new Axial(0, 1);
        public static Axial UpRight => new Axial(1, -1);
        public static Axial UpLeft => new Axial(-1, 0);
        public static Axial DownRight => new Axial(1, 0);
        public static Axial DownLeft => new Axial(-1, 1);
        
        
        public static Axial operator +(Axial a, Axial b)
        {
            return new Axial(a.Q + b.Q, a.R + b.R);
        }
        
        public static Axial operator -(Axial a, Axial b)
        {
            return new Axial(a.Q - b.Q, a.R - b.R);
        }
        
        public static Axial operator -(Axial a)
        {
            return new Axial(-a.Q, -a.R);
        }
        
        public static Axial operator *(Axial a, int i)
        {
            return new Axial(a.Q * i, a.R * i);
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Axial other)
            {
                return Q == other.Q && R == other.R;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Q, R);
        }

        public static bool operator ==(Axial a, Axial b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Axial a, Axial b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return $"({Q},{R})";
        }
    }   
    
}