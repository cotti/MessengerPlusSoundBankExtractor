using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MessengerPlusSoundBankExtractor.Controls
{
    public class AudioPlayer : TemplatedControl
    {
        public static readonly StyledProperty<bool> IsPlayingProperty =
    AvaloniaProperty.Register<AudioPlayer, bool>(nameof(IsPlaying), false, false, Avalonia.Data.BindingMode.TwoWay);

        public bool IsPlaying
        {
            get { return GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
        }

        public static readonly StyledProperty<string> CurrentDurationProperty =
AvaloniaProperty.Register<AudioPlayer, string>(nameof(CurrentDuration), "0:55", false, Avalonia.Data.BindingMode.TwoWay, (x) => x != "0" ? true : false);

        public string CurrentDuration
        {
            get { return GetValue(CurrentDurationProperty); }
            set { SetValue(CurrentDurationProperty, value); }
        }

        public static readonly StyledProperty<string> TotalDurationProperty =
AvaloniaProperty.Register<AudioPlayer, string>(nameof(TotalDuration), "1:07", false, Avalonia.Data.BindingMode.TwoWay, (x) => x != "0" ? true : false);

        public string TotalDuration
        {
            get { return GetValue(TotalDurationProperty); }
            set { SetValue(TotalDurationProperty, value); }
        }
        public static readonly StyledProperty<double> ProgressProperty =
AvaloniaProperty.Register<AudioPlayer, double>(nameof(Progress), 0, false, Avalonia.Data.BindingMode.TwoWay, (x) => x >= 0 ? true : false);

        public double Progress
        {
            get { return GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }
        public static readonly StyledProperty<ICommand?> CommandProperty =
            AvaloniaProperty.Register<Button, ICommand?>(nameof(Command));
        public static readonly StyledProperty<object?> CommandParameterProperty =
    AvaloniaProperty.Register<Button, object?>(nameof(CommandParameter));

        public ICommand? Command
        {
            get => GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        public object? CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
    }
}
