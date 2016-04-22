using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

using Microsoft.ProjectOxford.Emotion; 
using Microsoft.ProjectOxford.Emotion.Contract;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EmotionCaptureApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FaceRecognizerPage : Page
    {
        string pictureName = string.Empty;
        private readonly IFaceServiceClient faceServiceClient = new FaceServiceClient(ApplicationConstants.FACE_API_SUBSCRIPTION_KEY); // This is for face recognition
        private readonly EmotionServiceClient emotionServiceClient = new EmotionServiceClient(ApplicationConstants.EMOTIONS_API_SUBSCRIPTION_KEY);


        public FaceRecognizerPage()
        {
            this.InitializeComponent();
            if(string.IsNullOrEmpty(ApplicationConstants.EMOTIONS_API_SUBSCRIPTION_KEY))
            {
                SolidColorBrush brush = new SolidColorBrush(Windows.UI.Colors.Red);
                Status.Foreground = brush;
                Status.Text = "You have not set the subscription key to access Cognitive Services Emotions API";
        }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null && (!string.IsNullOrEmpty(e.Parameter.ToString())))
            {
                pictureName = e.Parameter.ToString();
                LoadPicture();
            }
        }

        /// <summary>
        /// This is called after the Camera capture is done and the picture is to be displayed
        /// 
        /// </summary>
        private async void LoadPicture()
        {
            try

            {
                // Open the picture file captured from the camera
                if (string.IsNullOrEmpty(pictureName))
                    return;
                Windows.Storage.StorageFile file = await KnownFolders.PicturesLibrary.GetFileAsync(pictureName);

                if (file != null)
                {
                    // Open a stream for the selected file.
                    // The 'using' block ensures the stream is disposed
                    // after the image is loaded.
                    using (Windows.Storage.Streams.IRandomAccessStream fileStream =
                        await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {
                        // Set the image source to the selected bitmap.
                        Windows.UI.Xaml.Media.Imaging.BitmapImage bitmapImage =
                            new Windows.UI.Xaml.Media.Imaging.BitmapImage();

                        bitmapImage.SetSource(fileStream);
                        FacePhoto.Source = bitmapImage;

                    }
                }
            }
            catch(Exception ex)
            {
                SolidColorBrush brush = new SolidColorBrush(Windows.UI.Colors.Red);
                Status.Foreground = brush;
                Status.Text = "Error Loading picture : " + ex.Message;
            }
        }

        /// <summary>
        /// If the user needs to choose an existing picture from the Library and use that for face recognition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {

            Windows.Storage.Pickers.FileOpenPicker openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.SuggestedStartLocation =  Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            openPicker.ViewMode =  Windows.Storage.Pickers.PickerViewMode.Thumbnail;

            // Filter to include a sample subset of file types.
            openPicker.FileTypeFilter.Clear();
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".jpg");

            // Open the file picker.
            Windows.Storage.StorageFile file =
                await openPicker.PickSingleFileAsync();

            if(file!=null)
                pictureName = file.Name;

            // 'file' is null if user cancels the file picker.
            if (file != null)
            {
                // Open a stream for the selected file.
                // The 'using' block ensures the stream is disposed
                // after the image is loaded.
                using (Windows.Storage.Streams.IRandomAccessStream fileStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap.
                    Windows.UI.Xaml.Media.Imaging.BitmapImage bitmapImage =
                        new Windows.UI.Xaml.Media.Imaging.BitmapImage();

                    bitmapImage.SetSource(fileStream);
                    FacePhoto.Source = bitmapImage;
                }
            }
        }

        private async Task GetFeedbackOnMenu()
        {
            Menufeedback.Text = string.Empty;
            EmotionValue.Text = string.Empty; 
            Emotion[] emotionResult = await GetEmotions();
            if (emotionResult == null || emotionResult.Length==0)
            {
                SolidColorBrush brush = new SolidColorBrush(Windows.UI.Colors.Red);
                Menufeedback.Foreground = brush;
                Menufeedback.Text = "No data returned from Emotions API. Ensure picture quality is good";
                return;
            }
            string verdict = string.Empty;
            try
            {
                string result = RecognizedEmotion(emotionResult[0].Scores, out verdict);
                EmotionValue.Text = "[" + result + "]";
                if ("positive".Equals(verdict))
                {
                    SolidColorBrush brush = new SolidColorBrush(Windows.UI.Colors.Green);
                    Menufeedback.Foreground = brush;
                }
                else
                {
                    SolidColorBrush brush = new SolidColorBrush(Windows.UI.Colors.Red);
                    Menufeedback.Foreground = brush;
                }
                Menufeedback.Text = "Verdict on today's Menu is : " + verdict;
            }
            catch(Exception ex)
            {
                SolidColorBrush brush = new SolidColorBrush(Windows.UI.Colors.Red);
                Status.Foreground = brush;
                Status.Text = "Error reading Emotion : " + ex.Message;
            }
        }

        private async void GetEmotionButton_Click(object sender, RoutedEventArgs e)
        {
            await GetFeedbackOnMenu();
        }

      
        private async Task<Emotion[]> GetEmotions()
        {

            try
            {
                Emotion[] emotionResult;

                Windows.Storage.StorageFile file = await KnownFolders.PicturesLibrary.GetFileAsync(pictureName);

                if (file != null)
                {
                    // Open a stream for the selected file.
                    // The 'using' block ensures the stream is disposed
                    // after the image is loaded.
                    using (Stream fileStream =
                        await file.OpenStreamForReadAsync())
                    {
                        emotionResult = await emotionServiceClient.RecognizeAsync(fileStream);
                        return emotionResult;
                    }
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                SolidColorBrush brush = new SolidColorBrush(Windows.UI.Colors.Red);
                Status.Foreground = brush;
                Status.Text = "Error reading Emotion : " + ex.Message;
                return null;
            }
        }


        private string RecognizedEmotion(Scores scores, out string verdict)
        {
            string result = string.Empty;
            verdict = "positive";
            float[] allscores = new float[8];
            allscores[0] = scores.Anger;
            allscores[1] = scores.Contempt;
            allscores[2] = scores.Disgust;
            allscores[3] = scores.Fear;
            allscores[4] = scores.Happiness;
            allscores[5] = scores.Neutral;
            allscores[6] = scores.Sadness;
            allscores[7] = scores.Surprise;
            float finalresult = allscores.Max();
            List<float> allScoresList = new List<float>();
            allScoresList.Add(scores.Anger);
            allScoresList.Add(scores.Contempt);
            allScoresList.Add(scores.Disgust);
            allScoresList.Add(scores.Fear);
            allScoresList.Add(scores.Happiness);
            allScoresList.Add(scores.Neutral);
            allScoresList.Add(scores.Sadness);
            allScoresList.Add(scores.Surprise);

            int indexresult = allScoresList.IndexOf(finalresult);
            if (indexresult == 0)
            {
                result = "Anger";
                verdict = "negative";
            }
            else if (indexresult == 1)
            {
                result = "Contempt";
                verdict = "negative";

            }
            else if (indexresult == 2)
            {
                result = "Disgust";
                verdict = "negative";

            }
            else if (indexresult == 3)
            {
                result = "Fear";
                verdict = "negative";

            }
            else if (indexresult == 4)
            {
                result = "Happiness";
            }
            else if (indexresult == 5)
            {
                result = "Neutral";
            }
            else if (indexresult == 6)
            {
                result = "Sadness";
                verdict = "negative";

            }
            else if (indexresult == 7)
            {
                result = "Surprise";
            }
            else
            {
                result = "None detected";
                verdict = "negative";
            }
            return result;
        }

        /// <summary>
        /// This recognizes the face in the picture. The integration with the Azure Cognitive Services Face API is done, but the results
        /// are not integrated into the UI yet...
        /// </summary>
        public async void RecognizeFaces()
        {
            // The bounded rectangle coordinates are returned post the identification of the faces in the picture
            // by the Face API in Cognitive Services
            FaceRectangle[] faceRects = await UploadAndDetectFaces(pictureName);
            #region  WPF code does not work here
            //if (faceRects.Length > 0)
            //{
            //DrawingVisual visual = new DrawingVisual();
            //DrawingContext drawingContext = visual.RenderOpen();
            //drawingContext.DrawImage(bitmapSource,
            //new Rect(0, 0, bitmapSource.Width, bitmapSource.Height));
            //double dpi = bitmapSource.DpiX;
            //double resizeFactor = 96 / dpi;

            //foreach (var faceRect in faceRects)
            //{
            //drawingContext.DrawRectangle(
            //Brushes.Transparent,
            //new Pen(Brushes.Red, 2),
            //new Rect(
            //faceRect.Left * resizeFactor,
            //faceRect.Top * resizeFactor,
            //faceRect.Width * resizeFactor,
            //faceRect.Height * resizeFactor
            //)
            //);
            //}

            //drawingContext.Close();
            //RenderTargetBitmap faceWithRectBitmap = new RenderTargetBitmap(
            //(int)(bitmapSource.PixelWidth * resizeFactor),
            //(int)(bitmapSource.PixelHeight * resizeFactor),
            //96,
            //96,
            //PixelFormats.Pbgra32);

            //faceWithRectBitmap.Render(visual);
            //FacePhoto.Source = faceWithRectBitmap;
            //}
            #endregion
        }

        private async Task<FaceRectangle[]> UploadAndDetectFaces(string imageFilePath)
        {
            try
            {
                Windows.Storage.StorageFile file = await KnownFolders.PicturesLibrary.GetFileAsync(imageFilePath);

                if (file != null)
                {
                    // Open a stream for the selected file.
                    // The 'using' block ensures the stream is disposed
                    // after the image is loaded.
                    using (Stream fileStream =
                        await file.OpenStreamForReadAsync())
                    {
                        var faces = await faceServiceClient.DetectAsync(fileStream);
                        var faceRects = faces.Select(face => face.FaceRectangle);
                        return faceRects.ToArray();
                    }
                }
                else
                    return new FaceRectangle[0];
            }
            catch (Exception ex)
            {
                SolidColorBrush brush = new SolidColorBrush(Windows.UI.Colors.Red);
                Status.Foreground = brush;
                Status.Text = "Error Loading picture : " + ex.Message;
                return null;
            }
        }

        private void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EmotionCapturePage));
        }

        private async void SendTelemetryButton_Click(object sender, RoutedEventArgs e)
        {
            Status.Text = string.Empty;
            await IotHubClient.InitializeConnections();
            IotHubClient.SendWeightDataToIotHub();
            if(string.IsNullOrEmpty(IotHubClient.ErrorString))
            {
                SolidColorBrush brush = new SolidColorBrush(Windows.UI.Colors.Green);
                Status.Foreground = brush;
                Status.Text = "sending food consumption data continuously....";
            }
            else
            {
                SolidColorBrush brush = new SolidColorBrush(Windows.UI.Colors.Red);
                Status.Foreground = brush;
                Status.Text = "Error sending telemetry data : " + IotHubClient.ErrorString;
            }
        }

        private async void SendEmotionButton_Click(object sender, RoutedEventArgs e)
        {
            Status.Text = string.Empty;
            await IotHubClient.InitializeConnections();

            if (string.IsNullOrEmpty(this.EmotionValue.Text))
            {
                SolidColorBrush brush = new SolidColorBrush(Windows.UI.Colors.Red);
                Status.Foreground = brush;
                Status.Text = "No Emotion data to send";
                return;
            }
            await IotHubClient.SendFootfallAndEmotionData(this.EmotionValue.Text);

            if (string.IsNullOrEmpty(IotHubClient.ErrorString))
            {
                SolidColorBrush brush = new SolidColorBrush(Windows.UI.Colors.Green);
                Status.Foreground = brush;
                Status.Text = "sent";
            }
            else
            {
                SolidColorBrush brush = new SolidColorBrush(Windows.UI.Colors.Red);
                Status.Foreground = brush;
                Status.Text = "Error sending emotion data : " + IotHubClient.ErrorString;
            }
        }

        private void StopTelemetryButton_Click(object sender, RoutedEventArgs e)
        {
            IotHubClient.StopWeightDataToIotHub();
            SolidColorBrush brush = new SolidColorBrush(Windows.UI.Colors.Black);
            Status.Foreground = brush;
            Status.Text = "stopped...";
        }
    }
}
