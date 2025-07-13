using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Happy_Plops
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string GameTitle = "Happy Plops";

        private GameData Data { get; set; } = new GameData();
        public MainWindow()
        {
            InitializeComponent();
            this.Title = GameTitle;
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = Ask_Before_Lose_Data("Spielstand aufgeben?", "Möchtest du wirklich neu starten? Ohne zu Speichern geht der momentane Spielstand verloren.");
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Neues Dokument erstellen");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Data.NewPlop("Alan",5);
            Data.NewPlop("Max", 1);
            Data.NewGroup();
            Data.Plops[1].Parents.Add(Data.Plops[0]);
            Data.Groups[0].Add(Data.Plops[0]);

            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = System.IO.Path.Combine(exePath, "SaveFiles");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = System.IO.Path.Combine(folderPath, "SaveFile01.json");

            Data.hasEdited = false;
            string json = JsonConvert.SerializeObject(Data, Data.Settings());
            //string json = JsonSerializer.Serialize(Data, options);
            File.WriteAllText(filePath, json);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {

            //Data.NewPlop("Alan", 5);
            //Data.NewPlop("Laura", 3);
            //Data.NewPlop("Max", 1);
            //Data.NewGroup();
            //Data.Groups[0].Add(Data.Plops[0]);
            MessageBox.Show("Vor dem Laden:\n"+Data.ToString());
            MessageBoxResult result = Ask_Before_Lose_Data("Spielstand aufgeben?", "Möchtest du wirklich neu starten? Ohne zu Speichern geht der momentane Spielstand verloren.");
            if (result == MessageBoxResult.No)
            {
                return;
            }

            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = System.IO.Path.Combine(exePath, "SaveFiles");
            string filePath = System.IO.Path.Combine(folderPath, "SaveFile01.json");

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Speicherdatei nicht gefunden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                //GameData loadedData = JsonSerializer.Deserialize<GameData>(json, options);
                GameData loadedData = JsonConvert.DeserializeObject<GameData>(json, Data.Settings());

                if (loadedData != null)
                {
                    Data = loadedData;
                    MessageBox.Show("Daten erfolgreich geladen.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Fehler beim Laden der Daten.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Datei:\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (Data.Plops[0] == Data.Groups[0].Members[0])
            {
                MessageBox.Show("Data.Plops[0] == Data.Groups[0].Members[0]");
            }
            else
            {
                MessageBox.Show("Data.Plops[0] IS NICHT Data.Groups[0].Members[0] :(");
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = Ask_Before_Lose_Data("Wirklich beenden?", "Willst du das Spiel wirklich beenden?");
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow(GameTitle)
            {
                Owner = this,
                GameName = GameTitle
            };
            about.ShowDialog();
        }

        private string ChooseFormByCount(int count, string singular, string plural)
        {
            if (count == 1)
            {
                return singular;
            }
            else
            {
                return plural;
            }
        }

        private MessageBoxResult Ask_Before_Lose_Data(string caption, string text)
        {
            if (!Data.hasEdited)
            {
                return MessageBoxResult.Yes;
            }
            int minutesSinceSaved = Data.MinutesPassedSinceLastSave();
            if (minutesSinceSaved >= 0)
            {
                string minutenForm = ChooseFormByCount(minutesSinceSaved, "Minute", "Minuten");
                return MessageBox.Show(
                        $"{text}\n\nZuletzt Gespeichert: {Data._lastTimeSaved.ToShortTimeString()} Uhr\n(Vor {minutesSinceSaved} {minutenForm})",
                        caption,
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                        );
            }
            return MessageBoxResult.Yes;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Data.NewPlop(Data.Plops.Count.ToString());
        }
    }
}
