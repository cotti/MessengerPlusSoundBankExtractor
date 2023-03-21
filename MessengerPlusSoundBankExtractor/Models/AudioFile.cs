using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerPlusSoundBankExtractor.Models
{
    public class AudioFile : ReactiveObject
    {
        private string? name;

        private ReadOnlyMemory<byte> file;
        public string Name { get => name!; set => name = value; }

        public ReadOnlyMemory<byte> File { get => file; set => file = value; }

        public string CurrentDuration => TimeSpan.FromSeconds(durationSeconds).ToString("mm\\:ss");
        public string TotalDuration => TimeSpan.FromSeconds(totalSeconds).ToString("mm\\:ss");

        private double durationSeconds = 0;
        private double totalSeconds = 1;

        public double DurationSeconds
        {
            get { return durationSeconds; }
            set { this.RaiseAndSetIfChanged(ref durationSeconds, value); }
        }

        public double TotalSeconds
        {
            get { return totalSeconds; }
            set { this.RaiseAndSetIfChanged(ref totalSeconds, value); }
        }

        private bool _isPlaying;

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { this.RaiseAndSetIfChanged(ref _isPlaying, value); }
        }

        public double Progress
        {
            get { return durationSeconds / totalSeconds * 100; }
        }


        public AudioFile(string name, ReadOnlyMemory<byte> file)
        {
            this.name = name;
            this.file = file;
        }

        public AudioFile() 
        {
            this.WhenAnyValue(o => o.IsPlaying).Subscribe(o => this.RaisePropertyChanged(nameof(IsPlaying)));
            this.WhenAnyValue(o => o.DurationSeconds).Subscribe(o => { this.RaisePropertyChanged(nameof(DurationSeconds)); this.RaisePropertyChanged(nameof(CurrentDuration)); this.RaisePropertyChanged(nameof(Progress)); });
            this.WhenAnyValue(o => o.TotalSeconds).Subscribe(o => { this.RaisePropertyChanged(nameof(TotalSeconds)); this.RaisePropertyChanged(nameof(TotalDuration)); });
        }
    }
}
