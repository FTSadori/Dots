using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Media;

namespace Dots_1._0v
{
    public enum SoundEnable
    {
        mouseEnterSound,
        niceClick,
        weaponsPull,
        selectSound,
    }
    public enum MusicEnable
    {
        menuSongQuiet,
        menuSong,
    }

    public static class StaticSoundPlayer
    {
        private static string GetPathForSound(SoundEnable sound)
            => @"Sounds\" + sound.ToString() + ".wav";
        private static string GetPathForMusic(MusicEnable music)
            => @"Music\" + music.ToString() + ".mp3";

        public static bool SoundOn = true;

        public static MediaPlayer backgroundMusic = new MediaPlayer();
        public static bool isBackPlaying = false;

        public static MediaPlayer soundPlayer = new MediaPlayer();

        public static void PlaySound(SoundEnable sound)
        {
            if (!SoundOn) return;
            soundPlayer.Open(new Uri(GetPathForSound(sound), UriKind.Relative));
            soundPlayer.Play();
        }

        public static void StartBackgroundMusic(MusicEnable music)
        {
            if (!SoundOn) return;
            backgroundMusic.Open(new Uri(GetPathForMusic(music), UriKind.Relative));
            backgroundMusic.Position = TimeSpan.Zero;
            backgroundMusic.Play();
            isBackPlaying = true;
            backgroundMusic.MediaEnded += Media_Ended;
        }

        public static void Media_Ended(object sender, EventArgs e)
        {
            MediaPlayer player = sender as MediaPlayer;
            player.Position = TimeSpan.Zero;
            player.Play();
        }

        public static void BackgroundMusicStop()
        {
            backgroundMusic.Stop();
        }
    }
}
