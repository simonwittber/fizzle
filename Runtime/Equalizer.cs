using UnityEngine;
using System.Collections;

namespace Fizzle
{
    [System.Serializable]
    public class Equalizer : IHasGUID
    {
        const int sampleRate = 44100;

        public JackSignal input = new JackSignal();
        public JackIn lg = new JackIn();
        public JackIn mg = new JackIn();
        public JackIn hg = new JackIn();
        public JackIn lowFreq = new JackIn();
        public JackIn highFreq = new JackIn();
        public JackOut output = new JackOut();

        public int ID { get { return output.id; } set { output.id = value; } }

        float lf;
        float f1p0;
        float f1p1;
        float f1p2;
        float f1p3;

        float hf;
        float f2p0;
        float f2p1;
        float f2p2;
        float f2p3;

        float sdm1;
        float sdm2;
        float sdm3;

        public Equalizer()
        {
            lg.Value = 1.0f;
            mg.Value = 1.0f;
            hg.Value = 1.0f;
            lowFreq.Value = 120;
            highFreq.Value = 880;
        }

        public Equalizer(int lowF, int highF)
        {
            lg.Value = 1.0f;
            mg.Value = 1.0f;
            hg.Value = 1.0f;
            lowFreq.Value = lowF;
            highFreq.Value = highF;
        }

        public float Update()
        {
            if (input.connectedId == 0)
            {
                output.Value = 0;
                return 0;
            }
            lf = 2 * Mathf.Sin(Mathf.PI * ((float)lowFreq / (float)sampleRate));
            hf = 2 * Mathf.Sin(Mathf.PI * ((float)highFreq / (float)sampleRate));

            var sample = input.Value;
            float l, m, h;

            f1p0 += (lf * (sample - f1p0)) + Mathf.Epsilon;
            f1p1 += (lf * (f1p0 - f1p1));
            f1p2 += (lf * (f1p1 - f1p2));
            f1p3 += (lf * (f1p2 - f1p3));

            l = f1p3;

            f2p0 += (hf * (sample - f2p0)) + Mathf.Epsilon;
            f2p1 += (hf * (f2p0 - f2p1));
            f2p2 += (hf * (f2p1 - f2p2));
            f2p3 += (hf * (f2p2 - f2p3));

            h = sdm3 - f2p3;

            m = sdm3 - (h + l);

            l *= lg;
            m *= mg;
            h *= hg;

            sdm3 = sdm2;
            sdm2 = sdm1;
            sdm1 = sample;

            var eqSmp = (l + m + h);
            output.Value = eqSmp;
            return eqSmp;
        }

    }

}