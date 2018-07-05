// <copyright file="KeyWordWindow.xaml.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTool
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Interaction logic for KeyWordWindow
    /// </summary>
    public partial class KeyWordWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyWordWindow"/> class.
        /// </summary>
        public KeyWordWindow()
        {
            this.InitializeComponent();
            var keyWords = new List<Tuple<string, string>>();
            keyWords.Add(new Tuple<string, string>("WENN", "IF"));
            keyWords.Add(new Tuple<string, string>("DANN", "THEN"));
            keyWords.Add(new Tuple<string, string>("SONST", "ELSE"));
            keyWords.Add(new Tuple<string, string>("SONST WENN", "ELSEIF"));
            keyWords.Add(new Tuple<string, string>("ENDE WENN", "ENDIF"));
            keyWords.Add(new Tuple<string, string>("TUE", "DO"));
            keyWords.Add(new Tuple<string, string>("SOLANGE", "UNTIL"));
            keyWords.Add(new Tuple<string, string>("FORTSETZEN SCHRITT", "RESUME STEP"));
            keyWords.Add(new Tuple<string, string>("ABBRECHEN", "ABORT"));
            keyWords.Add(new Tuple<string, string>("SCHLIESST ANWENDUNGSFALL EIN", "INCLUDE USE CASE"));
            keyWords.Add(new Tuple<string, string>("ERWEITERT DURCH ANWENDUNGSFALL", "EXTENDED BY USE CASE"));
            keyWords.Add(new Tuple<string, string>("WÄHRENDDESSEN", "MEANWHILE"));
            keyWords.Add(new Tuple<string, string>("VALIDIERT, DASS", "VALIDATES THAT"));
            this.keyWordGrid.ItemsSource = keyWords;
        }
    }
}
