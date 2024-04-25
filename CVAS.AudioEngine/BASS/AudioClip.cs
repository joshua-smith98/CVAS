using Un4seen.Bass;

namespace CVAS.AudioEngine.BASS
{
    /// <summary>
    /// BASS implementation of <see cref="IAudioClip"/>.
    /// </summary>
    internal abstract class AudioClip : IAudioClip
    {
        internal int GetStreamHandle()
        {
            var ret = GetStreamHandleImpl();
            Task.Run(() => FreeUponStop(ret));
            return ret;
        }
        
        protected abstract int GetStreamHandleImpl();

        private static void FreeUponStop(int streamHandle)
        {
            while(Bass.BASS_ChannelIsActive(streamHandle) is not BASSActive.BASS_ACTIVE_PLAYING)
                Thread.Sleep(100);
            while(Bass.BASS_ChannelIsActive(streamHandle) is BASSActive.BASS_ACTIVE_PLAYING)
                Thread.Sleep(100);
            
            Bass.BASS_ChannelFree(streamHandle);
        }
    }
}
