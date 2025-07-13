using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Happy_Plops
{
    /// <summary>
    /// Interaktionslogik für About.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public string GameName = "";
        public AboutWindow(string gameName)
        {
            InitializeComponent();
            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "???";
            this.Title = "Über " + gameName;
            GameTitle.Text = gameName;
            VersionText.Text = $"Version: {version}";
            AutorText.Text = "von Alan Shor";

        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
