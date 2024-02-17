﻿using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.AudioEngine
{
    public class Delay : IAudioClip
    {
        public WaveFormat WaveFormat => AudioPlayer.instance.WaveFormat;

        public int milliseconds { get; }

        public Delay(int milliseconds)
        {
            this.milliseconds = milliseconds;
        }

        public IWaveProvider toWaveProvider()
        {
            return new DelaySampleProvider(AudioPlayer.instance.WaveFormat, milliseconds).ToWaveProvider();
        }
    }
}
