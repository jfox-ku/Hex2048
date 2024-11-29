using UnityEngine;

namespace HexGridNamespace
{
    public static class CoordinateExtensions
    {
        public static Axial Up(this Axial axial)
        {
            return axial + new Axial(0,-1);
        }
        
        public static Axial Down(this Axial axial)
        {
            return axial + new Axial(0, 1);
        }
        
        public static Axial UpRight(this Axial axial)
        {
            return axial + new Axial(1,-1);
        }
        
        public static Axial UpLeft(this Axial axial)
        {
            return axial + new Axial(-1,0);
        }
        
        public static Axial DownRight(this Axial axial)
        {
            return axial + new Axial(1,0);
        }
        
        public static Axial DownLeft(this Axial axial)
        {
            return axial + new Axial(-1,1);
        }
        
        public static Axial CubeToAxial(this CubeCoor cube)
        {
            return new Axial
            {
                Q = cube.Q,
                R = cube.R
            };
        }

        public static Axial Normalize(this Axial c) //todo check accuracy
        {
            // Compute the magnitude (length of the vector)
            float magnitude = Mathf.Sqrt(c.Q * c.Q + c.R * c.R);

            // Avoid division by zero
            if (magnitude == 0)
                return Axial.Zero;

            // Scale down the vector
            int normalizedQ = Mathf.RoundToInt(c.Q / magnitude);
            int normalizedR = Mathf.RoundToInt(c.R / magnitude);

            return new Axial(normalizedQ, normalizedR);
        }

        
        public static Axial ToAxialDirection(this UnityEngine.Vector2 direction)
        {
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            
            if (angle <= 60) return Axial.UpRight;   
            if (angle is > 60 and < 120) return Axial.Up;        
            if (angle is >= 120 and < 180) return Axial.UpLeft;   
            if (angle is >= 180 and < 240) return Axial.DownLeft;  
            if (angle is >= 240 and < 300) return Axial.Down;    
            if (angle >= 300) return Axial.DownRight; 
            
            return Axial.Zero;
        }
        
        public static Vector2 SnapTo60Degrees(this Vector2 direction)
        {
            // Ensure the vector is normalized
            direction.Normalize();

            // Calculate the angle in degrees (0° at right, counter-clockwise is positive)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Normalize the angle to [0, 360)
            if (angle < 0) angle += 360;

            // Snap the angle to the nearest multiple of 60
            float snappedAngle = Mathf.Round((angle - 30) / 60) * 60 + 30;

            // Convert the snapped angle back to a Vector2
            float radians = snappedAngle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        
        public static CubeCoor AxialToCube(this Axial axial)
        {
            return new CubeCoor
            {
                Q = axial.Q,
                R = axial.R,
                S = -axial.Q - axial.R
            };
        }
    }
}