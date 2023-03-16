﻿using MP3Sharp;
using OpenTK.Audio.OpenAL;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessengerPlusSoundBankExtractor.Services
{
    class AudioContext : IDisposable
    {
        private ALDevice device;
        private ALContext context;

        public AudioContext()
        {
            device = ALC.OpenDevice(null);
            context = ALC.CreateContext(device, (int[])null);
            ALC.MakeContextCurrent(context);
        }

        public static ALFormat GetSoundFormat(SoundFormat format) => format switch
        {
            SoundFormat.Pcm16BitMono => ALFormat.Mono16,
            SoundFormat.Pcm16BitStereo => ALFormat.Stereo16,
            _ => throw new Exception()
        };


        public void Dispose()
        {
            ALC.DestroyContext(context);
            ALC.CloseDevice(device);
        }
    }

    public class AudioPlayer : IDisposable
    {
        private List<string?> devices;
        private AudioContext context;
        private bool disposedValue;

        public AudioPlayer()
        {
            devices = ALC.GetString(AlcGetStringList.DeviceSpecifier);
            context = new AudioContext();
        }

        public async Task PlayAudio(ReadOnlyMemory<byte> file)
        {
            byte[] pcmData = null;
            using var mp3Stream = new MP3Stream(new MemoryStream(file.ToArray()));
            using var pcmStream = new MemoryStream();
            var buffer = new byte[4096];
            int bytesReturned = 1;
            int totalBytesRead = 0;
            try
            {
                while (bytesReturned > 0)
                {
                    bytesReturned = mp3Stream.Read(buffer, 0, buffer.Length);
                    totalBytesRead += bytesReturned;
                    await pcmStream.WriteAsync(buffer, 0, bytesReturned);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            pcmData = pcmStream.ToArray();
            buffer = null;

            int bufferId = AL.GenBuffer();

            AL.BufferData<byte>(bufferId, AudioContext.GetSoundFormat(SoundFormat.Pcm16BitMono), new ReadOnlySpan<byte>(pcmData), mp3Stream.Frequency * 2);

            int sourceId = AL.GenSource();
            AL.Source(sourceId, ALSourcei.Buffer, bufferId);

            AL.SourcePlay(sourceId);

            SpinWait.SpinUntil(() => AL.GetSourceState(sourceId) == ALSourceState.Stopped);

            AL.DeleteSource(sourceId);
            AL.DeleteBuffer(bufferId);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
