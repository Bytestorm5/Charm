﻿using System.Collections.Generic;
using Arithmic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace Charm.Views;

public partial class ProgressView : UserControl
{
    private Queue<string> _progressStages;
    private int TotalStageCount;
    private bool bLogProgress = true;
    private bool bUseFullBar = false;

    public ProgressView()
    {
        InitializeComponent();
        Hide();
    }

    public void Hide()
    {
        IsVisible = false;
    }

    public void Show()
    {
        // Grid.Background = new SolidColorBrush(new Color {A = 0, B = 0, G = 0, R = 0});
        IsVisible = true;
    }

    private void UpdateProgress()
    {
        ProgressBar.Value = GetProgressPercentage();
        ProgressText.Text = GetCurrentStageName();
    }

    public void SetProgressStages(List<string> progressStages, bool bLogProgress = true, bool bUseFullBar = false)
    {
        this.bLogProgress = bLogProgress;
        this.bUseFullBar = bUseFullBar;
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            TotalStageCount = progressStages.Count;
            _progressStages = new Queue<string>();
            foreach (var progressStage in progressStages)
            {
                _progressStages.Enqueue(progressStage);
            }

            UpdateProgress();
            Show();
        });
    }

    public void CompleteStage()
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            string removed = _progressStages.Dequeue();
            if (bLogProgress)
                Log.Info($"Completed loading stage: {removed}");
            UpdateProgress();
            if (_progressStages.Count == 0)
            {
                Hide();
            }
        });
    }

    public string GetCurrentStageName()
    {
        if (_progressStages.Count > 0)
        {
            var stage = _progressStages.Peek();
            if (bLogProgress)
                Log.Info($"Starting loading stage: {stage}");
            return stage;
        }
        return "Loading";
    }

    public int GetProgressPercentage()
    {
        // We want to artificially make it more meaningful, so we pad by 15% on each side
        if (bUseFullBar)
            return 100 - 100 * _progressStages.Count / TotalStageCount;
        else
            return 95 - 90 * _progressStages.Count / TotalStageCount;
    }
}

