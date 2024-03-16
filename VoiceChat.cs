using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Steamworks;
using System.Diagnostics;

namespace Steamvoice
{

    public class SteamVoiceChat
    {
        private WaveOutEvent waveOut;
        private BufferedWaveProvider bufferedWaveProvider;
        bool isBuffering = false;
        System.Timers.Timer updateTimer = new(100);

        private FadeInOutSampleProvider fadeInOutSampleProvider;

        public SteamVoiceChat()
        {
            waveOut = new WaveOutEvent();
            bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(22050, 16, 1));

            waveOut.Init(bufferedWaveProvider);
            waveOut.PlaybackStopped += (sender, e) => { isBuffering = false; };
            updateTimer.Elapsed += (sender, e) => Update();
        }

        public void Start()
        {
            updateTimer.Start();
        }

        public void StartRecording(CSteamID steamID)
        {
            SteamUser.StartVoiceRecording();
            SteamFriends.SetInGameVoiceSpeaking(steamID, true);
        }

        public void StopRecording(CSteamID steamID)
        {
            SteamUser.StopVoiceRecording();
            SteamFriends.SetInGameVoiceSpeaking(steamID, false);
        }

        private void Update()
        {
            uint nBytesAvailable = 0;
            EVoiceResult res = SteamUser.GetAvailableVoice(out nBytesAvailable);

            if (res == EVoiceResult.k_EVoiceResultOK && nBytesAvailable > 1024)
            {
                byte[] byteBuffer = new byte[1024];
                uint bufferSize;
                res = SteamUser.GetVoice(true, byteBuffer, 1024, out bufferSize);

                if (res == EVoiceResult.k_EVoiceResultOK && bufferSize > 0)
                {
                    ClientPlaySound(byteBuffer, bufferSize);
                }
            }
        }

        private void ClientPlaySound(byte[] byteBuffer, uint byteCount)
        {
            byte[] destBuffer = new byte[22050 * 2];
            uint bytesWritten;
            EVoiceResult voiceResult = SteamUser.DecompressVoice(byteBuffer, byteCount, destBuffer, (uint)destBuffer.Length, out bytesWritten, 22050);

            if (voiceResult == EVoiceResult.k_EVoiceResultOK && bytesWritten > 0)
            {
                PlaySound(destBuffer, (int)bytesWritten);
            }
        }

        private void PlaySound(byte[] audioData, int dataSize)
        {
            //fadeInOutSampleProvider = new FadeInOutSampleProvider(new SampleChannel(new RawSourceWaveStream(new MemoryStream(audioData, 0, dataSize), new WaveFormat(22050, 16, 1))), true);

            //fadeInOutSampleProvider.BeginFadeIn(200);
            //fadeInOutSampleProvider.BeginFadeOut(200);

            //byte[] sampleBuffer = new byte[dataSize];
            //fadeInOutSampleProvider.ToWaveProvider16().Read(sampleBuffer, 0, dataSize);
            int bytesPerSample = 2; // Assuming 16-bit samples
            int channels = 1;       // Mono

            for (int i = 0; i < 599; i += bytesPerSample * channels)
            {
                // Extract the sample value (assuming little-endian encoding)
                short sampleValue = BitConverter.ToInt16(audioData, i);

                // Scale the sample value
                sampleValue = (short)(sampleValue * 0.1);

                // Update the sample value in the byte array
                byte[] newSampleBytes = BitConverter.GetBytes(sampleValue);
                Buffer.BlockCopy(newSampleBytes, 0, audioData, i, bytesPerSample * channels);
            }

            bufferedWaveProvider.AddSamples(audioData, 0, dataSize);

            //bufferedWaveProvider.AddSamples(silence, 0, silence.Length);
            if (!isBuffering)
            {
                Trace.Write("e");
                isBuffering = true;
                waveOut.Play();
            }
        }
    }
}