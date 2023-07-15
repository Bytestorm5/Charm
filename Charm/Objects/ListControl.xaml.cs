﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Arithmic;
using Internal.Fbx;
using Tiger;
using Tiger.Schema;

namespace Charm.Objects;

/// <summary>
/// The current implementation of Package is limited so you cannot have nested views below a Package.
/// For future, would be better to split the tag items up so we can cache them based on parents.
/// </summary>
public partial class ListControl : UserControl
{
    private readonly BaseListViewModel _viewModel;

    public ListControl()
    {
        InitializeComponent();
        _viewModel = new BaseListViewModel();
    }

    public void LoadView<TViewModel>(OnListItemClicked onListItemClicked) where TViewModel : IViewModel
    {
        _viewModel.LoadView<TViewModel>(onListItemClicked);
        DataContext = _viewModel;
    }

    public void LoadDataView<TViewModel>()
    {
        // (DataContext as BaseListViewModel).LoadDataView<TViewModel>();
    }
}