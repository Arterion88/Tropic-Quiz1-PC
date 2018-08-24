using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Globalization;
using System.Configuration;
using System.Threading;
using System.Resources;

namespace Quiz1_PC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<string> Languages = new List<string>();

        CultureInfo cultureInfo {
            set
            {
                Thread.CurrentThread.CurrentUICulture = value;
                Thread.CurrentThread.CurrentCulture = value;

                Properties.Resource.Culture = value;

                Properties.Settings.Default.Culture = value.TwoLetterISOLanguageName;
                Properties.Settings.Default.Save();
            }
        }

        public MainWindow()
        {
            if (!(Properties.Settings.Default.Culture is null))
                cultureInfo = new CultureInfo(Properties.Settings.Default.Culture);

            GetAvailableCultures().ToList().ForEach(x => Languages.Add(x.TwoLetterISOLanguageName));
            InitializeComponent();

            CmbLanguages_Refresh();
        }

        private void CmbLanguages_DropDownClosed(object sender, EventArgs e)
        {
            //MessageBox.Show(cmbLanguages.SelectedItem.ToString());
            cultureInfo = new CultureInfo(Languages[cmbLanguages.SelectedIndex]);

            Title = Properties.GlobalStrings.AppName;
            btnStart.Content = Properties.GlobalStrings.MainMenu_BtnStart;

            CmbLanguages_Refresh();
            
        }

        private void CmbLanguages_Refresh()
        {
            cmbLanguages.Items.Clear();

            foreach (string item in Properties.GlobalStrings.MainMenu_CmbLanguages.Split(',').ToList())
                cmbLanguages.Items.Add(item);

            cmbLanguages.SelectedIndex = Languages.IndexOf(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        }

        public static IEnumerable<CultureInfo> GetAvailableCultures()
        {
            List<CultureInfo> result = new List<CultureInfo>();

            ResourceManager rm = new ResourceManager(typeof(Properties.GlobalStrings));

            
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (CultureInfo culture in cultures)
            {
                try
                {
                    if (culture.Equals(CultureInfo.InvariantCulture)) continue; //do not use "==", won't work

                    ResourceSet rs = rm.GetResourceSet(culture, true, false);
                    if (rs != null)
                        result.Add(culture);
                }
                catch (CultureNotFoundException)
                {
                    //NOP
                }
            }
            return result;
        }



    }
}
