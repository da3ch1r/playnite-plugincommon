﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using CommonPluginsControls.Controls;
using CommonPlayniteShared.Common;
using CommonPluginsShared;
using LiveCharts;
using LiveCharts.Wpf;

namespace CommonPluginsControls.LiveChartsCommon
{
    /// <summary>
    /// Logique d'interaction pour CustomersTooltipForTime.xaml
    /// </summary>
    public partial class CustomerToolTipForTime : IChartTooltip
    {
        public TooltipSelectionMode? SelectionMode { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;


        #region Properties
        private TooltipData _data;
        public TooltipData Data
        {
            get
            {
                SharedConverter sharedConverter = new SharedConverter();
                DataTitle = sharedConverter.Convert(_data, null, null, CultureInfo.CurrentCulture).ToString();
                DataTitleInfo = string.Empty;
                if (ShowWeekPeriode && DateFirstPeriode != default(DateTime))
                {
                    int.TryParse(Regex.Replace(DataTitle, @"[^\d]", string.Empty), out int WeekNumber);
                    if (WeekNumber > 0)
                    {
                        DateTime First = Tools.YearWeekDayToDateTime(DateFirstPeriode.Year, DayOfWeek.Monday, WeekNumber);
                        DateTime Last = default(DateTime);

                        if ((WeekNumber + 1) > Tools.GetWeeksInYear(DateFirstPeriode.Year))
                        {
                            Last = Tools.YearWeekDayToDateTime(DateFirstPeriode.Year, DayOfWeek.Sunday, 1);
                        }
                        else
                        {
                            Last = Tools.YearWeekDayToDateTime(DateFirstPeriode.Year, DayOfWeek.Sunday, WeekNumber + 1);
                        }

                        DataTitleInfo = "[" + First.ToString(Constants.DateUiFormat) + " - " + Last.ToString(Constants.DateUiFormat) + "]";
                    }
                }
                OnPropertyChanged("DataTitle");
                OnPropertyChanged("DataTitleInfo");

                return _data;
            }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        public TextBlockWithIconMode Mode
        {
            get { return (TextBlockWithIconMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
            nameof(Mode),
            typeof(TextBlockWithIconMode),
            typeof(CustomerToolTipForTime),
            new FrameworkPropertyMetadata(TextBlockWithIconMode.IconTextFirstWithText));

        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        public static readonly DependencyProperty ShowIconProperty = DependencyProperty.Register(
            nameof(ShowIcon),
            typeof(bool),
            typeof(CustomerToolTipForTime),
            new FrameworkPropertyMetadata(false));

        public bool ShowTitle
        {
            get { return (bool)GetValue(ShowTitleProperty); }
            set { SetValue(ShowTitleProperty, value); }
        }

        public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(
            nameof(ShowTitle),
            typeof(bool),
            typeof(CustomerToolTipForTime),
            new FrameworkPropertyMetadata(false));

        public bool ShowWeekPeriode
        {
            get { return (bool)GetValue(ShowWeekPeriodeProperty); }
            set { SetValue(ShowWeekPeriodeProperty, value); }
        }

        public static readonly DependencyProperty ShowWeekPeriodeProperty = DependencyProperty.Register(
            nameof(ShowWeekPeriode),
            typeof(bool),
            typeof(CustomerToolTipForTime),
            new FrameworkPropertyMetadata(false));

        public DateTime DateFirstPeriode
        {
            get { return (DateTime)GetValue(DateFirstPeriodeProperty); }
            set { SetValue(DateFirstPeriodeProperty, value); }
        }

        public static readonly DependencyProperty DateFirstPeriodeProperty = DependencyProperty.Register(
            nameof(DateFirstPeriode),
            typeof(DateTime),
            typeof(CustomerToolTipForTime),
            new FrameworkPropertyMetadata(default(DateTime)));

        public string DataTitle { get; set; }
        public string DataTitleInfo { get; set; }
        #endregion


        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public CustomerToolTipForTime()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (ShowIcon)
            {
                if (Mode == TextBlockWithIconMode.IconTextFirstOnly || Mode == TextBlockWithIconMode.IconTextFirstWithText || Mode == TextBlockWithIconMode.IconTextOnly)
                {
                    Mode = TextBlockWithIconMode.IconTextFirstWithText;
                }
                else if (Mode == TextBlockWithIconMode.IconFirstOnly || Mode == TextBlockWithIconMode.IconFirstWithText || Mode == TextBlockWithIconMode.IconOnly)
                {
                    Mode = TextBlockWithIconMode.IconFirstWithText;
                }
            }
            else
            {
                Mode = TextBlockWithIconMode.TextOnly;
            }
        }
    }
}
