﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ILR;
namespace ilrLearnerEntry.UserControls.LearnerEditorControls.LearnerControls
{
    /// <summary>
    /// Interaction logic for ucLLDDAndLearningSupport.xaml
    /// </summary>
    public partial class ucLLDDAndLearningSupport : UserControl, INotifyPropertyChanged
    {
        #region Private Variables
        private Learner _learner;
        private const Int32 _maxLRSItem = 4;
        private DataTable _dt;
        #endregion

        #region Constructor
        public ucLLDDAndLearningSupport()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        #endregion

        #region Public Properties

        public Learner CurrentItem
        {
            get { return _learner; }
            set
            {
                if (value != null)
                {
                    _learner = value;
                    this.DataContext = this;
                    lv_LSR.SelectionChanged -= lv_LSR_SelectionChanged;
                    OnPropertyChanged("LLDDTypeListDS");
                    OnPropertyChanged("LLDDTypeListLD");
                    ClearAllLRSSelected();
                    OnPropertyChanged("LSRList");
                    foreach (int? lrsCode in _learner.LSR)
                    {
                        SetLRSAsSelected(lrsCode.ToString());
                    }
                    OnPropertyChanged("LSRList");
                    OnPropertyChanged("CurrentItem");
                    OnPropertyChanged("ALSCost");

                    lv_LSR.SelectionChanged += lv_LSR_SelectionChanged;
                }
                else
                {
                    this.DataContext = null;
                    lv_LSR.SelectionChanged -= lv_LSR_SelectionChanged;
                    ClearAllLRSSelected();
                }
            }
        }

        public string ALSCost
        {
            get
            {
                if (CurrentItem == null || CurrentItem.ALSCost == null)
                { return string.Empty; }
                else
                { return CurrentItem.ALSCost.ToString(); }
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    CurrentItem.ALSCost = null;
                }
                else
                {
                    try
                    {
                        int number = Int32.Parse(value);
                        CurrentItem.ALSCost = number;
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show(String.Format("Value Entered is not a number : {0}", value)
                                                           , "Not a Number"
                                                           , MessageBoxButton.OK
                                                           , MessageBoxImage.Question);
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show(String.Format("Value to big : {0}", value)
                                                          , "Overflow"
                                                          , MessageBoxButton.OK
                                                          , MessageBoxImage.Question);
                    }
                }
                OnPropertyChanged("ALSCost");
            }
        }

        public DataTable LSRList
        {
            get { return _dt; }
            set
            {
                DataTable tmpDT = value;
                foreach (DataRow dr in tmpDT.Rows)
                {
                    if (String.IsNullOrWhiteSpace(dr["Code"].ToString()))
                    {
                        dr.Delete();
                        break;
                    }
                }

                _dt = tmpDT;
                _dt.Columns.Add(new DataColumn("IsSelected", typeof(bool)));
            }
        }
        public DataTable LLDDTypeListDS { set; get; }
        public DataTable LLDDTypeListLD { set; get; }
        public DataTable LLDDHealthProblemList { set; get; }
        #endregion

        #region Public Methods
        #endregion

        #region PRIVATE Methods
        private void lv_LSR_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (DataRowView x in e.AddedItems)
            {
                LearnerFAM tmp = _learner.LearnerFAMList.Where(f => f.LearnFAMType == "LSR" && f.LearnFAMCode == int.Parse(x[0].ToString())).FirstOrDefault();
                if (tmp == null)
                {
                    if (_learner.LSR.Count < _maxLRSItem)
                    {
                        _learner.AddFAM(LearnerFAM.MultiOccurrenceFAMs.LSR, int.Parse(x[0].ToString()));
                    }
                    else
                    {
                        MessageBox.Show(String.Format("   You may only select {0} items.", _maxLRSItem.ToString())
                                                               , "Max number of selectable items reached."
                                                               , MessageBoxButton.OK
                                                               , MessageBoxImage.Information
                                                               , MessageBoxResult.OK);

                        x["IsSelected"] = Convert.ToBoolean(false);
                        OnPropertyChanged("LSRList");
                    }
                }
            }
            foreach (DataRowView x in e.RemovedItems)
            {
                foreach (int fam in _learner.LSR)
                {
                    if (fam.ToString() == x[0].ToString())
                    {
                        _learner.RemoveFAM(LearnerFAM.MultiOccurrenceFAMs.LSR, int.Parse(x[0].ToString()));
                        break;
                    }
                }
            }
        }
        private void SetLRSAsSelected(string Filter)
        {
            if (_dt != null)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    if (dr["Code"].ToString() == Filter)
                    {
                        dr["IsSelected"] = Convert.ToBoolean(true);
                        break;
                    }
                }
            }
        }
        private void ClearAllLRSSelected()
        {
            if (_dt != null)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    dr["IsSelected"] = Convert.ToBoolean(false);
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        /// INotifyPropertyChanged requires a property called PropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires the event for the property when it changes.
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
#if DEBUG
            VerifyPropertyName(propertyName);
#endif
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                var msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                {
                    throw new Exception(msg);
                }
                else
                {
                    Debug.Fail(msg);
                }
            }
        }

        protected bool ThrowOnInvalidPropertyName { get; set; }

        #endregion



    }
}