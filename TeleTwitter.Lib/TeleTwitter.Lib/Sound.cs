using System;
using System.Runtime.InteropServices;

namespace TeleTwitter.Lib
{
    public class Sound
    {
        #region Private Members

        private const int SND_ASYNC = 1;
        private const int SND_FILENAME = 0x20000;
        private const int SND_PURGE = 0x40;

        #endregion

        #region Methods
        
        public static void Beep()
        {
            Console.Beep(0x3e80, 1);
        }

        public static void Play(string file)
        {
            try
            {
                PlaySound(file, 0, SND_FILENAME | SND_ASYNC);
            }
            catch (Exception)
            {
            }
        }

        [DllImport("WinMM.dll")]
        private static extern bool PlaySound(string fname, int Mod, int flag);
        
        public static void Stop()
        {
            try
            {
                PlaySound(null, 0, SND_PURGE);
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}

