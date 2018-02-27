using UnityEngine;

namespace Fizzle
{
    public static class PatchCableColor
    {
        static float alpha = 0.5f;
        static Vector3[] colorSet = new[] {
            new Vector3(230, 25, 75),
            new Vector3(60, 180, 75),
            new Vector3(255, 225, 25),
            new Vector3(0, 130, 200),
            new Vector3 (245, 130, 48),
            new Vector3 (145, 30, 180),
            new Vector3  (70, 240, 240),
            new Vector3  (240, 50, 230),
            new Vector3 (210, 245, 60),
            new Vector3  (250, 190, 190),
            new Vector3  (0, 128, 128),
            new Vector3 (230, 190, 255),
            new Vector3   (170, 110, 40),
            new Vector3  (255, 250, 200),
            new Vector3(128, 0, 0),
            new Vector3 (170, 255, 195),
            new Vector3  (128, 128, 0),
            new Vector3   (255, 215, 180),
            new Vector3   (0, 0, 128),
            new Vector3   (128, 128, 128)
        };

        public static Color GetColor(int id)
        {
            var c = colorSet[Mathf.Abs(id) % (colorSet.Length - 1)];
            return new Color(c.x, c.y, c.z, alpha);
        }
    }
}