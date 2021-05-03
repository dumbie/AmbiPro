using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Input;

namespace AmbiPro.Settings
{
    partial class FormSettings
    {
        private void sp_DecreaseBlockSize_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Debug.WriteLine("Decreasing the block sizes..." + sp_Block1.Width);
                if (sp_Block1.Width >= 50)
                {
                    sp_Block1.Width = sp_Block1.Width - 10;
                    sp_Block1.Height = sp_Block1.Height - 10;
                    sp_Block2.Width = sp_Block2.Width - 10;
                    sp_Block2.Height = sp_Block2.Height - 10;
                    sp_Block3.Width = sp_Block3.Width - 10;
                    sp_Block3.Height = sp_Block3.Height - 10;
                    sp_Block4.Width = sp_Block4.Width - 10;
                    sp_Block4.Height = sp_Block4.Height - 10;
                    sp_Block5.Width = sp_Block5.Width - 10;
                    sp_Block5.Height = sp_Block5.Height - 10;
                    sp_Block6.Width = sp_Block6.Width - 10;
                    sp_Block6.Height = sp_Block6.Height - 10;
                    sp_Block7.Width = sp_Block7.Width - 10;
                    sp_Block7.Height = sp_Block7.Height - 10;
                    sp_Block8.Width = sp_Block8.Width - 10;
                    sp_Block8.Height = sp_Block8.Height - 10;
                }
            }
            catch { }
        }

        private void sp_IncreaseBlockSize_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Debug.WriteLine("Increasing the block sizes..." + sp_Block1.Width);
                if (sp_Block1.Width < (Screen.PrimaryScreen.Bounds.Height / 4))
                {
                    sp_Block1.Width = sp_Block1.Width + 10;
                    sp_Block1.Height = sp_Block1.Height + 10;
                    sp_Block2.Width = sp_Block2.Width + 10;
                    sp_Block2.Height = sp_Block2.Height + 10;
                    sp_Block3.Width = sp_Block3.Width + 10;
                    sp_Block3.Height = sp_Block3.Height + 10;
                    sp_Block4.Width = sp_Block4.Width + 10;
                    sp_Block4.Height = sp_Block4.Height + 10;
                    sp_Block5.Width = sp_Block5.Width + 10;
                    sp_Block5.Height = sp_Block5.Height + 10;
                    sp_Block6.Width = sp_Block6.Width + 10;
                    sp_Block6.Height = sp_Block6.Height + 10;
                    sp_Block7.Width = sp_Block7.Width + 10;
                    sp_Block7.Height = sp_Block7.Height + 10;
                    sp_Block8.Width = sp_Block8.Width + 10;
                    sp_Block8.Height = sp_Block8.Height + 10;
                }
            }
            catch { }
        }
    }
}