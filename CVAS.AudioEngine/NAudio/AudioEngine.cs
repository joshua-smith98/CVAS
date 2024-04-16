﻿using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace CVAS.AudioEngine.NAudio
{
    /// <summary>
    /// Governs all audio mixing, rendering and playback. Non-constructable - call <see cref="InitImpl()"/> and use the static <see cref="AudioEngine.InstanceImpl"/> to access.
    /// </summary>
    internal class AudioEngine : IAudioEngine
    {
        public static IAudioEngine InstanceImpl
        {
            get
            {
                if (instance is null) throw new NullReferenceException();
                else return instance;
            }
        }
        private static AudioEngine? instance;

        public static bool IsInitialisedImpl => instance is not null;

        internal static readonly WaveFormat WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2); // TODO: Implement changing WaveFormat

        private WaveOutEvent waveOutEvent;

        private readonly MixingSampleProvider sampleProvider;

        private AudioEngine()
        {
            // Initialise properties
            sampleProvider = new MixingSampleProvider(WaveFormat);
            sampleProvider.ReadFully = true;
            waveOutEvent = new WaveOutEvent();
            waveOutEvent.Init(sampleProvider);
            waveOutEvent.Play();
        }

        /// <summary>
        /// Initialises <see cref="InstanceImpl"/>. Throws an exception if this is called more than once.
        /// </summary>
        /// <exception cref="AudioEngineException"></exception>
        public static void InitImpl()
        {
            // Check if already initialised
            if (instance is not null) throw new AudioEngineException("AudioEngine cannot be initialised twice!");
            instance = new AudioEngine();
        }

        /// <summary>
        /// Plays a single <see cref="AudioClip"/> once. For use when <see cref="InstanceImpl"/> hasn't been initialised.
        /// </summary>
        /// <param name="audioClip"></param>
        public static void PlayOnceImpl(IAudioClip iaudioClip)
        {
            var audioClip = (AudioClip)iaudioClip; // Assume this is the right type, since it shouldn't be different while on the same OS

            // Initialise new WaveOutEvent and play
            WaveOutEvent waveOutEvent = new();
            waveOutEvent.Init(audioClip.ToWaveProvider());
            waveOutEvent.Play();

            // Hang until playback is complete
            while (waveOutEvent.PlaybackState is PlaybackState.Playing);

            waveOutEvent.Dispose();
        }

        /// <summary>
        /// Returns true if the file at the given path is an audio file, false otherwise.
        /// Also returns false if the path is invalid or the file doesn't exist.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsAudioFileImpl(string path)
        {
            try
            {
                // Return true if we can load the audio file
                using (new AudioFileReader(path)) { } ;
                return true;
            }
            catch { return false; } // Otherwise return false
        }

        /// <summary>
        /// Plays the given <see cref="AudioClip"/>, with automatic resampling.
        /// </summary>
        /// <param name="audioClip"></param>
        public void Play(IAudioClip iaudioClip)
        {
            var audioClip = (AudioClip)iaudioClip; // Assume this is the right type, since it shouldn't be different while on the same OS

            MediaFoundationResampler resampler = new(audioClip.ToWaveProvider(), WaveFormat);
            sampleProvider.AddMixerInput(resampler);
        }

        /// <summary>
        /// Renders an <see cref="AudioClip"/> to a new file at the given path. Assumes the directory exists, and overwrites any existing file.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="path"></param>
        public static void RenderImpl(IAudioClip iaudioClip, string path)
        {
            var audioClip = (AudioClip)iaudioClip; // Assume this is the right type, since it shouldn't be different while on the same OS

            // Write wave file
            WaveFileWriter.CreateWaveFile16(path, audioClip.ToWaveProvider().ToSampleProvider());
        }

        /// <summary>
        /// Stops all audio playback.
        /// </summary>
        public void StopAll()
        {
            sampleProvider.RemoveAllMixerInputs();
        }

        public void Dispose()
        {
            waveOutEvent?.Stop();
            waveOutEvent?.Dispose();
            waveOutEvent = null!;
            GC.SuppressFinalize(this);
        }
    }
}