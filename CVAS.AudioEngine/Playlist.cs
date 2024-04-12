﻿using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace CVAS.AudioEngineNS
{
    public class Playlist(params AudioClip[] audioClips) : AudioClip
    {
        public AudioClip[] AudioClips => audioClips;

        internal override int GetStreamHandle()
        {
            // Create mixer
            int mixerHandle = BassMix.BASS_Mixer_StreamCreate(
                44100,
                1,
                BASSFlag.BASS_MIXER_END |
                BASSFlag.BASS_MIXER_QUEUE |
                BASSFlag.BASS_STREAM_DECODE
                );
             
            if (mixerHandle == 0)
            {
                // Handle errors
                var bassError = Bass.BASS_ErrorGetCode();
                switch (bassError)
                {
                    case BASSError.BASS_ERROR_INIT:
                        throw new AudioEngineException("Attempted to create a playlist before BASS was initialised!");
                    case BASSError.BASS_ERROR_MEM:
                        throw new AudioEngineException("BASS ran out of memory while creating playlist.");
                    default:
                        throw new AudioEngineException($"An unknown error occurred while creating a playlist. BASS error code: {bassError}");
                }
            }

            // Add audioClips to mixer
            foreach (AudioClip audioClip in AudioClips)
                BassMix.BASS_Mixer_StreamAddChannel(mixerHandle, audioClip.GetStreamHandle(), BASSFlag.BASS_MIXER_CHAN_DOWNMIX);

            // Run autofree and return handle
            Task.Run(() => AudioEngine.FreeUponStop(mixerHandle));
            return mixerHandle;
        }
    }
}
