using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    [System.Serializable]
    public class Oscilloscope
    {
        public float duration;
        public float waveformStartTime;
        public float waveformLength;
        public float verticalScale = 1;
        public float verticalBias = 1;

        public void Update(Rect rect, Color color, System.Func<float, float> Evaluate)
        {
            waveformStartTime = EditorGUILayout.Slider("Start", waveformStartTime, 0, duration);
            waveformLength = EditorGUILayout.Slider("Period", waveformLength, 0, duration);
            waveformStartTime = Mathf.Min(duration - waveformLength, waveformStartTime);
            Rect r = AudioCurveRendering.BeginCurveFrame(rect);
            AudioCurveRendering.DrawCurve(r, (t) => Evaluate(waveformStartTime + (t * waveformLength)), color);
            AudioCurveRendering.EndCurveFrame();
        }
    }
}