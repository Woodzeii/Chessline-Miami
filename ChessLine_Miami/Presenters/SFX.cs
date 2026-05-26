using System;
using NAudio.Wave;
using System.Windows.Forms;
using System.IO;
using System.Media;
using System.Windows.Media;
using ChessLine_Miami.UI;
using ChessLine_Miami.Presenters;
using ChessLine_Miami.Models;
using ChessLine_Miami.Logic;
using Microsoft.VisualBasic;
namespace ChessLine_Miami;

public class SFX
{

    private static MediaPlayer mediaPlayer;
    public static PlayerProgress PlayerProgress { get; set; }

    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;


    #region NAudio Methods
    public void PlayMp3(string filePath)
    {
        try
        {
            // Останавливаем прошлый трек, если он играл
            StopMp3(); 

            // Инициализируем устройства
            outputDevice = new WaveOutEvent();
            audioFile = new AudioFileReader(filePath);

            // Загружаем файл в плеер и запускаем
            outputDevice.Init(audioFile);
            outputDevice.Play();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка воспроизведения: {ex.Message}");
        }
    }

    
    public void StopMp3()
    {
        if (outputDevice != null)
        {
            outputDevice.Stop();
            outputDevice.Dispose();
            outputDevice = null;
        }
        if (audioFile != null)
        {
            audioFile.Dispose();
            audioFile = null;
        }
    }
   

    
    public void TogglePauseMp3()
    {
        if (outputDevice != null)
        {
            if (outputDevice.PlaybackState == PlaybackState.Playing)
                outputDevice.Pause();
            else if (outputDevice.PlaybackState == PlaybackState.Paused)
                outputDevice.Play();
        }
    }
     #endregion



    public static void UpdateVolume()
    {
        if (mediaPlayer != null)
        {
            mediaPlayer.Volume = Program.PlayerProgress.Volume;
        }
    }
    
    public static void PlayHydrogen()
    {
        PlayMusic("MoonHydrogenRemix.mp3");
    }
    public static void PlayNormal()
    {
        PlayMusic("normal.mp3");
    }
    public static void PlayHackers()
    {
        PlayMusic("Hackers.mp3");
    }
    public static void Play1st()
    {
        PlayMusic("1st.mp3");
    }
    public static void PlayLevelFinished()
    {
        PlayMusic("LevelFinished.mp3");
    }
    public static void PlayCyberpunk()
    {
        PlayMusic("Cyberpunk.mp3");
    }
    public static void PlayEpic()
    {
        PlayMusic("Epic.mp3");
    }
    public static void PlayNightCrawler()
    {
        PlayMusic("NightCrawler.mp3");
    }

    public static void PlayMenuMusic()
    {
        PlayMusic("main-menu.mp3");
    }

    public static void PlayActive()
    {
        PlayMusic("Active.wav");
    }

    private static void PlayMusic(string fileName)
    {
        var musicPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI", "Music", fileName);
        if (!File.Exists(musicPath))
            return;

        if (mediaPlayer == null)
            mediaPlayer = new MediaPlayer();
        else
        {
            mediaPlayer.Stop();
            mediaPlayer.Close();
        }

        mediaPlayer.Open(new Uri(musicPath));
        mediaPlayer.Volume = Program.PlayerProgress.Volume;
        mediaPlayer.MediaEnded -= OnMediaEnded;
        mediaPlayer.MediaEnded += OnMediaEnded;
        mediaPlayer.Play();
    }

    private static void OnMediaEnded(object sender, EventArgs e)
    {
        if (mediaPlayer != null)
        {
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Play();
        }
    }

}