using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace ArnoldVinkMessageBox
{
    public partial class AVMessageBox : Window
    {
        //Message Box Variables
        private static bool vMessageBoxPopupCancelled = false;
        private static Int32 vMessageBoxPopupResult = 0;
        private static AVMessageBox _AVMessageBox;

        //Initialize messagebox
        public AVMessageBox() { InitializeComponent(); }

        //Show and close Messagebox Popup
        public static async Task<Int32> Popup(string Question, string Description, string Answer1, string Answer2, string Answer3, string Answer4)
        {
            try
            {
                _AVMessageBox = new AVMessageBox();

                //Set messagebox question content
                _AVMessageBox.grid_MessageBox_Text.Text = Question;
                if (!string.IsNullOrWhiteSpace(Description))
                {
                    _AVMessageBox.grid_MessageBox_Description.Text = Description;
                    _AVMessageBox.grid_MessageBox_Description.Visibility = Visibility.Visible;
                }
                else
                {
                    _AVMessageBox.grid_MessageBox_Description.Text = "";
                    _AVMessageBox.grid_MessageBox_Description.Visibility = Visibility.Collapsed;
                }
                if (!string.IsNullOrWhiteSpace(Answer1))
                {
                    _AVMessageBox.grid_MessageBox_Btn1.Content = Answer1;
                    _AVMessageBox.grid_MessageBox_Btn1.Visibility = Visibility.Visible;
                }
                else
                {
                    _AVMessageBox.grid_MessageBox_Btn1.Content = "";
                    _AVMessageBox.grid_MessageBox_Btn1.Visibility = Visibility.Collapsed;
                }
                if (!string.IsNullOrWhiteSpace(Answer2))
                {
                    _AVMessageBox.grid_MessageBox_Btn2.Content = Answer2;
                    _AVMessageBox.grid_MessageBox_Btn2.Visibility = Visibility.Visible;
                }
                else
                {
                    _AVMessageBox.grid_MessageBox_Btn2.Content = "";
                    _AVMessageBox.grid_MessageBox_Btn2.Visibility = Visibility.Collapsed;
                }
                if (!string.IsNullOrWhiteSpace(Answer3))
                {
                    _AVMessageBox.grid_MessageBox_Btn3.Content = Answer3;
                    _AVMessageBox.grid_MessageBox_Btn3.Visibility = Visibility.Visible;
                }
                else
                {
                    _AVMessageBox.grid_MessageBox_Btn3.Content = "";
                    _AVMessageBox.grid_MessageBox_Btn3.Visibility = Visibility.Collapsed;
                }
                if (!string.IsNullOrWhiteSpace(Answer4))
                {
                    _AVMessageBox.grid_MessageBox_Btn4.Content = Answer4;
                    _AVMessageBox.grid_MessageBox_Btn4.Visibility = Visibility.Visible;
                }
                else
                {
                    _AVMessageBox.grid_MessageBox_Btn4.Content = "";
                    _AVMessageBox.grid_MessageBox_Btn4.Visibility = Visibility.Collapsed;
                }

                //Reset messagebox variables
                vMessageBoxPopupResult = 0;
                vMessageBoxPopupCancelled = false;

                //Show the messagebox popup
                _AVMessageBox.Show();

                //Wait for user messagebox input
                while (vMessageBoxPopupResult == 0 && !vMessageBoxPopupCancelled) { await Task.Delay(100); }
                if (vMessageBoxPopupCancelled) { return 0; }

                //Hide the messagebox popup
                _AVMessageBox.Hide();
                _AVMessageBox = null;
            }
            catch { }
            return vMessageBoxPopupResult;
        }

        //Set MessageBox Popup Result
        void grid_MessageBox_Btn1_Click(object sender, RoutedEventArgs e) { vMessageBoxPopupResult = 1; }
        void grid_MessageBox_Btn2_Click(object sender, RoutedEventArgs e) { vMessageBoxPopupResult = 2; }
        void grid_MessageBox_Btn3_Click(object sender, RoutedEventArgs e) { vMessageBoxPopupResult = 3; }
        void grid_MessageBox_Btn4_Click(object sender, RoutedEventArgs e) { vMessageBoxPopupResult = 4; }

        //Handle window closing event
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                if (vMessageBoxPopupResult == 0 && !vMessageBoxPopupCancelled) { e.Cancel = true; }
                Debug.WriteLine("Closing the messagebox window.");
            }
            catch { }
        }
    }
}