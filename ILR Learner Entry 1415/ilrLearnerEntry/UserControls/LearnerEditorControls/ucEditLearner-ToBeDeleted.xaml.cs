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

//using ILR;

namespace ilrLearnerEntry.REMOVED.UserControls.LearnerEditorControls
{
    /// <summary>
    /// Interaction logic for ucEditor.xaml
    /// </summary>
    public partial class ucEditLearner : UserControl, INotifyPropertyChanged
    {
        #region Private Variables
        #endregion  

    
        #region Constructor
        public ucEditLearner()
        {
            InitializeComponent();
            this.DataContext = this;
        
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
