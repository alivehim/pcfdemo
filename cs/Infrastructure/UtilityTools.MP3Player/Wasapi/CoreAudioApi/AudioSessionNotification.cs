using System.Runtime.InteropServices;
using UtilityTools.MP3Player.Wasapi.CoreAudioApi.Interfaces;

namespace UtilityTools.MP3Player.Wasapi.CoreAudioApi
{
    internal class AudioSessionNotification : IAudioSessionNotification
    {
        private AudioSessionManager parent;

        internal AudioSessionNotification(AudioSessionManager parent)
        {
            this.parent = parent;
        }

        [PreserveSig]
        public int OnSessionCreated(IAudioSessionControl newSession)
        {
            parent.FireSessionCreated(newSession);
            return 0;
        }
    }
}
