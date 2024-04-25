﻿using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.Misc;

namespace CVAS.AudioEngine.BASS
{
    /// <summary>
    /// BASS implementation of <see cref="IAudioEngine"/>.
    /// </summary>
    internal class AudioEngine : IAudioEngine<AudioClip>
    {
        public static IAudioEngine Instance
        {
            get
            {
                instance ??= new AudioEngine();
                return instance;
            }
        }
        private static AudioEngine? instance;
        private static bool IsInitialised => instance is not null;

        private int engineMixerHandle;

        private AudioEngine()
        {
            // Try to initialise BASS
            if (!Bass.BASS_Init(-1, 44100, 0, nint.Zero))
            {
                var bassError = Bass.BASS_ErrorGetCode();
                switch (bassError)
                {
                    case BASSError.BASS_ERROR_DX:
                        throw new AudioEngineException("Failed to initialise BASS audio engine: DirectX is not installed.");
                    case BASSError.BASS_ERROR_ALREADY:
                        throw new AudioEngineException("Tried to initialise BASS twice!");
                    case BASSError.BASS_ERROR_MEM:
                        throw new AudioEngineException("BASS ran out of memory while initialising.");
                    default:
                        throw new AudioEngineException($"Failed to initialise BASS audio engine because of an unknown error. BASS error code: {bassError}");
                }
            }

            // TODO: Assign Freq and Chans depending on the current device's Freq and Chans

            // Try to create mixer
            int mixerHandle = BassMix.BASS_Mixer_StreamCreate(
                44100,
                1,
                BASSFlag.BASS_MIXER_NONSTOP
                );

            if (mixerHandle == 0)
            {
                // Handle errors
                var bassError = Bass.BASS_ErrorGetCode();
                switch (bassError)
                {
                    case BASSError.BASS_ERROR_MEM:
                        throw new AudioEngineException("BASS ran out of memory while initialising global mixer.");
                    default:
                        throw new AudioEngineException($"An unknown error occurred while initialising the global mixer. BASS error code: {bassError}");
                }
            }

            engineMixerHandle = mixerHandle;
            Bass.BASS_ChannelPlay(engineMixerHandle, false);
        }

        /// <summary>
        /// Plays an <see cref="AudioClip"/> once. For use when <see cref="Instance"/> has not been initialised.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <exception cref="AudioEngineException"></exception>
        public static void PlayOnce(AudioClip audioClip)
        {
            // Check to see if we need to initialise BASS, and do so
            if (!IsInitialised)
            {
                // Initialise BASS
                if (!Bass.BASS_Init(-1, 44100, 0, nint.Zero))
                {
                    // Handle errors
                    var bassError = Bass.BASS_ErrorGetCode();
                    switch (bassError)
                    {
                        case BASSError.BASS_ERROR_DX:
                            throw new AudioEngineException("Failed to initialise BASS audio engine: DirectX is not installed.");
                        case BASSError.BASS_ERROR_ALREADY:
                            throw new AudioEngineException("Tried to initialise BASS twice!");
                        case BASSError.BASS_ERROR_MEM:
                            throw new AudioEngineException("BASS ran out of memory while initialising.");
                        default:
                            throw new AudioEngineException($"Failed to initialise BASS audio engine because of an unknown error. BASS error code: {bassError}");
                    }
                }
            }

            // Initialise temporary mixer
            int mixerHandle = BassMix.BASS_Mixer_StreamCreate(
                44100,
                1,
                BASSFlag.BASS_MIXER_END
                );

            if (mixerHandle == 0)
            {
                // Handle errors
                var bassError = Bass.BASS_ErrorGetCode();
                switch (bassError)
                {
                    case BASSError.BASS_ERROR_MEM:
                        throw new AudioEngineException("BASS ran out of memory while initialising play-once mixer.");
                    default:
                        throw new AudioEngineException($"An unknown error occurred while initialising play-once mixer. BASS error code: {bassError}");
                }
            }

            // Get audioClip stream and attach to mixer
            var audioClipHandle = audioClip.GetStreamHandle();
            BassMix.BASS_Mixer_StreamAddChannel(mixerHandle, audioClipHandle, BASSFlag.BASS_DEFAULT);

            // Play mixer
            Bass.BASS_ChannelPlay(mixerHandle, false);

            // Hang until stream stops
            while (Bass.BASS_ChannelIsActive(mixerHandle) is BASSActive.BASS_ACTIVE_PLAYING)
                Task.Delay(100); // Only check every 100ms

            // Free mixer
            Bass.BASS_ChannelFree(mixerHandle);

            // Free BASS if needed
            if (!IsInitialised) Bass.BASS_Free();
        }

        /// <summary>
        /// Returns true if the file at the given path is an audio file, false otherwise.
        /// Also returns false if the path is invalid or the file doesn't exist.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsAudioFile(string path)
        {
            // Check for bass init (we'll need it to load the file)
            var bassActive = Bass.BASS_IsStarted() != 0;
            if (!bassActive) Bass.BASS_Init(-1, 44100, 0, nint.Zero);

            // Attempt to load audio file from path
            int streamHandle = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_DEFAULT);
            if (streamHandle == 0) return false; // Return false if failed

            // If it is, first free the stream and BASS, then return
            Bass.BASS_StreamFree(streamHandle);
            if (!bassActive) Bass.BASS_Free();

            return true;
        }

        /// <summary>
        /// Plays the given <see cref="AudioClip"/>, with automatic resampling.
        /// </summary>
        /// <param name="audioClip"></param>
        public void Play(AudioClip audioClip)
        {
            BassMix.BASS_Mixer_StreamAddChannel(engineMixerHandle, audioClip.GetStreamHandle(), BASSFlag.BASS_DEFAULT);
        }

        /// <summary>
        /// Renders an <see cref="AudioClip"/> to a new file at the given path. Assumes the directory exists, and overwrites any existing file.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="path"></param>
        /// <exception cref="AudioEngineException"/>
        public static void Render(AudioClip audioClip, string path)
        {
            // Check to see if we need to initialise BASS and do so
            if (!IsInitialised)
            {
                // Initialise BASS
                if (!Bass.BASS_Init(-1, 44100, 0, nint.Zero))
                {
                    // Handle errors
                    var bassError = Bass.BASS_ErrorGetCode();
                    switch (bassError)
                    {
                        case BASSError.BASS_ERROR_DX:
                            throw new AudioEngineException("Failed to initialise BASS audio engine: DirectX is not installed.");
                        case BASSError.BASS_ERROR_ALREADY:
                            throw new AudioEngineException("Tried to initialise BASS twice!");
                        case BASSError.BASS_ERROR_MEM:
                            throw new AudioEngineException("BASS ran out of memory while initialising.");
                        default:
                            throw new AudioEngineException($"Failed to initialise BASS audio engine because of an unknown error. BASS error code: {bassError}");
                    }
                }
            }

            var audioClipStream = audioClip.GetStreamHandle();

            EncoderWAV encoderWAV = new EncoderWAV(audioClipStream)
            {
                InputFile = null,
                OutputFile = path,
            };
            encoderWAV.Start(null, nint.Zero, false);
            Utils.DecodeAllData(audioClipStream, false);
            while (Bass.BASS_ChannelIsActive(audioClipStream) is BASSActive.BASS_ACTIVE_PLAYING) { } // TODO make this async to avoid hanging GUI in future
            encoderWAV.Stop();
            encoderWAV.Dispose();

            // Free BASS if needed
            if (!IsInitialised) Bass.BASS_Free();
        }

        /// <summary>
        /// Stops all audio playback.
        /// </summary>
        public void StopAll()
        {
            // Stop global mixer
            Bass.BASS_ChannelStop(engineMixerHandle);

            // Remove all channels connected to mixer
            foreach (int channelHandle in BassMix.BASS_Mixer_StreamGetChannels(engineMixerHandle))
            {
                BassMix.BASS_Mixer_ChannelRemove(channelHandle);
            }

            // Restart global mixer
            Bass.BASS_ChannelPlay(engineMixerHandle, false);
        }

        public void Dispose()
        {
            Bass.BASS_ChannelStop(engineMixerHandle);
            Bass.BASS_ChannelFree(engineMixerHandle);
            Bass.BASS_Free();
            instance = null;
            GC.SuppressFinalize(this);
        }
    }
}