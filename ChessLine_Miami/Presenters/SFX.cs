using System;
using NAudio.Wave;
using System.Windows.Forms;
namespace ChessLine_Miami.Presenters;
public class SFX
{
    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;

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
}