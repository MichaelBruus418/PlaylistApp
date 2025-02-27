using BusinessLogic.BLL;
using DTO.Model;
using PresentationWPF.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Track = DTO.Model.Track;

namespace PresentationWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // ***************************************************
        private String RequestUri = "http://localhost:53403/api/";
        // ***************************************************
        private HttpClient Client = new HttpClient();
        private Dictionary<string, string> ApiUrls = new Dictionary<string, string>();
        private TrackBLL TrackBLL = new TrackBLL();
        private Person person; // For Data Binding demonstration
        
        public MainWindow()
        {
            InitializeComponent();

            // Add event listeners programetically
            btnAddTrack.Click += btnAddTrack_Click;

            // Add WebAPI controllers to dictionary
            ApiUrls.Add("Track", this.RequestUri + "Track");
           
            Console.WriteLine("Denne virker ikke for MVC projekt.");
            Trace.WriteLine("Brug denne til MVC. Kræver System.Diagnostics;");
            UpdateTrackList();

            // --- Data Binding Demonstration ---
            person = new Person("Chuck Norris", 78, 80, 1000, true);
            personGrid.DataContext = person;

            changeBtn.Click += ChangeBtnClick;
        }

        private void BtnAddTrack_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        // --- Event Listeners ------------------------------------------------------------------------------------

        private void btnAddTrack_Click(object sender, RoutedEventArgs e)
        {
            Track track = new Track();
            String errorMsg = "";
            
            if (txtbxTrackTitle.Text.Trim().Length == 0)
            {
                errorMsg = "Please enter title.";
            }

            if (txtbxTrackLength.Text.Trim().Length > 0)
            {
                try
                {
                    track.Length = this.ParseLength(txtbxTrackTitle.Text.Trim());
                }
                catch (Exception)
                {
                    errorMsg = "Length is invlalid.\nFormat must be hh:mm:ss or mm:ss.";
                }
            }

            if (errorMsg.Length != 0)
            {
                MessageBox.Show(errorMsg, "Error");
            }
            else
            {
                track.Title = txtbxTrackTitle.Text.Trim();

                this.Client.DefaultRequestHeaders.Accept.Clear();
                StringContent content = new StringContent(JsonSerializer.Serialize(track), Encoding.UTF8, "application/json");
                HttpResponseMessage result = this.Client.PostAsync(this.ApiUrls["Track"], content).Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    // Clear Track title and length from textbox
                    txtbxTrackTitle.Clear();
                    txtbxTrackLength.Clear();
                }
                else
                {
                    var task = result.Content.ReadAsStringAsync();
                    String msg = JsonSerializer.Deserialize<String>(task.Result);
                    MessageBox.Show("Message from server: \n" + msg, "Error");
                }
                

                UpdateTrackList();
            }
        }

        private void btnDeleteTrack_Click(object sender, RoutedEventArgs e)
        {
            //Convert lvTracks.SelectedItems to a List of Tracks.
            List<DTO.Model.Track> selectedTracks = (new List<object>((IEnumerable<object>)lvTracks.SelectedItems)).ConvertAll(t => (DTO.Model.Track) t);
            
            // --- Use BLL to delete tracks ---
            //int rowsDeleted = TrackBLL.DeleteTracks(selectedTracks);
            //Trace.WriteLine("Rows deleted: " + rowsDeleted);

            // --- Use WebAPI to delete tracks ---
            List<int> selectedIds = selectedTracks.Select(x => (int)x.Id).ToList();
            foreach (int id in selectedIds)
            {
                this.Client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage result = this.Client.DeleteAsync(this.ApiUrls["Track"] + "/" + id).Result;
                if (result.StatusCode != HttpStatusCode.OK && result.StatusCode != HttpStatusCode.NotFound)
                {
                    var task = result.Content.ReadAsStringAsync();
                    String msg = JsonSerializer.Deserialize<String>(task.Result);
                    MessageBox.Show("Message from server: \n" + msg);
                }
            }

            UpdateTrackList();
        }

        private void lvTracks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Do stuff.
        }

        private void btnTestAPI_countAllTracks_Click(object sender, RoutedEventArgs e)
        {
            this.Client.DefaultRequestHeaders.Accept.Clear();
            Task<string> task = this.Client.GetStringAsync(this.RequestUri + "Track");
            var result = task.Result;

            List<DTO.Model.Track> tracks = JsonSerializer.Deserialize<List<DTO.Model.Track>>(result);
            MessageBox.Show("Num of tracks recieved via WebAPI: " + tracks.Count());
        }

       

        // ---------------------------------------------------------------------------------------------------------------

        private void UpdateTrackList()
        {
            this.Client.DefaultRequestHeaders.Accept.Clear();
            Task<string> task = this.Client.GetStringAsync(this.ApiUrls["Track"]);
            var result = task.Result;
            List<DTO.Model.Track> tracks = JsonSerializer.Deserialize<List<DTO.Model.Track>>(result);
            
            lvTracks.ItemsSource = tracks;
        }

        private TimeSpan ParseLength(String length)
        {
            if (length.Length != 0)
            {
                // --- Parse Length ---
                // Expected format is (string): hh:mm:ss|hh:mm|ss
                List<String> strValues = txtbxTrackLength.Text.Trim().Trim().Split(':').ToList();

                if (strValues.Count > 3)
                {
                    throw new ArgumentException("Length is not in a valid format");
                }

                // Pad with zeroes for hours and minutes
                while (strValues.Count < 3)
                {
                    strValues.Insert(0, "0");
                }

                try
                {
                     // Convert to list of ints
                    List<int> intValues = strValues.ConvertAll(v => int.Parse(v));
                    // Create TimeSpan object
                    return new TimeSpan(intValues[0], intValues[1], intValues[2]);

                }
                catch (Exception)
                {
                    throw new ArgumentException("Length is not in a valid format");
                }
            }

            throw new ArgumentException("Length is not in a valid format");
        }

        // --- Data Binding Demonstration ---
        private void ChangeBtnClick(object sender, RoutedEventArgs e)
        {
            this.person.Name = "Jet Li";
            this.person.Age = 99;
            this.person.Weight = 98;
            this.person.Score = 99999999;
            this.person.Accepted = false;
        }

        private void printBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.person.ToString());
        }

    }
}
