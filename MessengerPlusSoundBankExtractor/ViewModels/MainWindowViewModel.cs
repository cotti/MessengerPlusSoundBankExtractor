using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData.Binding;
using MessengerPlusSoundBankExtractor.Models;
using MessengerPlusSoundBankExtractor.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace MessengerPlusSoundBankExtractor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private AudioPlayer player = new AudioPlayer();
        private ObservableCollectionExtended<AudioFile> audioList = new();
        private string _OriginPath = "Z:\\bkup\\pacotee.plp";

        private ReadOnlyMemory<byte> activeFile;

        public string OriginPath
        {
            get { return _OriginPath; }
            set { this.RaiseAndSetIfChanged(ref _OriginPath, value); }
        }

        private string _TargetPath = "X:\\Users\\cotti\\Dev\\Nova pasta";

        public string TargetPath
        {
            get { return _TargetPath; }
            set { this.RaiseAndSetIfChanged(ref _TargetPath, value); }
        }

        public ReactiveCommand<Unit, Task> SelectOriginPathCommand { get; }
        public ReactiveCommand<Unit, Task> SelectTargetPathCommand { get; }
        public ReactiveCommand<Unit, Task> ConvertFileCommand { get; }
        public ReactiveCommand<AudioFile, Unit> PlayFileCommand { get; }
        public ObservableCollectionExtended<AudioFile> AudioList { get => audioList; set => audioList = value; }
        public ReactiveCommand<Unit, Task> SaveFilesCommand { get; }
        

        public MainWindowViewModel()
        {
            this.WhenAnyValue(o => o.OriginPath).Subscribe(o => this.RaisePropertyChanged(nameof(OriginPath)));
            this.WhenAnyValue(o => o.TargetPath).Subscribe(o => this.RaisePropertyChanged(nameof(TargetPath)));
            this.WhenAnyValue(o => o.AudioList).Subscribe(o => this.RaisePropertyChanged(nameof(AudioList)));
            SelectOriginPathCommand = ReactiveCommand.Create(SelectOriginPath);
            SelectTargetPathCommand = ReactiveCommand.Create(SelectTargetPath);
            ConvertFileCommand = ReactiveCommand.Create(ConvertFile);
            PlayFileCommand = ReactiveCommand.Create<AudioFile>(Play);
            SaveFilesCommand = ReactiveCommand.Create(SaveFiles);
            activeFile = null!;

            if(Design.IsDesignMode)
            {
                audioList.Add(new AudioFile("AAAAAAAAAAA", null));
                audioList.Add(new AudioFile("BBBBBBBBBBB", null));
            }
        }

        private void Play(AudioFile file)
        {
            file.DurationSeconds = 0;
            Task.Run(() => player.PlayAudio(file));
        }

        private async Task ConvertFile()
        {
            activeFile = null!;
            GC.Collect(1);
            activeFile = await File.ReadAllBytesAsync(OriginPath)!;
            var audioFileIndexes = FileConverter.FindPattern(activeFile.Span);
            AudioList.AddRange(FileConverter.GetFileList(audioFileIndexes, activeFile));

        }

        private async Task SaveFiles()
        {
            await FileConverter.SaveFiles(audioList.ToList(), TargetPath);
        }

        private async Task SelectTargetPath()
        {
            var folderDialog = new OpenFolderDialog()
            {
                Title = "Select a folder for the files",
                Directory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            };
            if (Avalonia.Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                TargetPath = await folderDialog.ShowAsync(desktop.MainWindow) ?? TargetPath;
        }

        private async Task SelectOriginPath()
        {
            var fileDialog = new OpenFileDialog()
            {
                AllowMultiple = false,
                Filters = new List<FileDialogFilter> { new FileDialogFilter { Name = "Messenger Plus Pack (.plp)", Extensions = new List<string> { "plp" } } },
                Title = "Select a Messenger Plus Pack file",
                Directory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            };
            if (Avalonia.Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                OriginPath = (await fileDialog.ShowAsync(desktop.MainWindow))!.FirstOrDefault()!;
        }
    }
}
