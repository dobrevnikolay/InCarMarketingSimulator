using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using Core;
using Microsoft.Win32;

namespace InCarGUI
{
    public class MainViewModel : BaseViewModel
    {
        #region Private members

        private string _pathToImageToShow;

        private Bitmap _screenPictureBitmap;

        private string _patternSearchResult;

        #endregion

        #region Commands

        public ICommand ChooseScreenCommand { get; set; }

        public IAsyncCommand FindPatternsCommand { get; set; }

        public ICommand ForceAdCreateCommand { get; set; }

        #endregion

        #region Public Properties

        public string PatternSearchResult
        {
            get => _patternSearchResult;
            set
            {
                _patternSearchResult = value;
                this.OnPropertyChanged();
            }
        }

        public string PathToImageToShow
        {
            get => _pathToImageToShow;
            set
            {
                _pathToImageToShow = value;
                this.OnPropertyChanged();
            }
        }

        public string PathToBackgroundImg { get; set; }

        public string ProgramLogo { get; set; }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            ChooseScreenCommand = new RelayCommand(ChooseScreenPicture);
            FindPatternsCommand = new AsyncRelayCommand(FindPatternsAsync);
            ForceAdCreateCommand = new RelayCommand(ForceAdCreation);
            PathToImageToShow = @"Images/Img.jpg";
            PathToBackgroundImg = @"Images/backGround.jpg";
            ProgramLogo = @"Images/logo.png";
        }

        private void ForceAdCreation()
        {
            throw new NotImplementedException();
        }

        private async Task FindPatternsAsync()
        {
            try
            {
                var patternMatcher = new OpenCvPatternMatcher();
                var mongodb = new MongoDB("InCarMarketing", "InCarMarketingScreen");
                ScreenAnalyzer sa = new ScreenAnalyzer(mongodb, patternMatcher);
                await sa.AnalyzeScreenAsync(_screenPictureBitmap);
                var results = ScreenAnalyzer.AnalyzeResults;
                PatternSearchResult = results.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine("No picture selected");
            }
        }

        private void ChooseScreenPicture()
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.DefaultExt = ".jpg";
            fileDialog.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|GIF Files (*.gif)|*.gif";

            var result = fileDialog.ShowDialog();

            if (true == result)
            {
                PathToImageToShow = fileDialog.FileName;
                _screenPictureBitmap = new Bitmap(fileDialog.FileName);
            }
        }

        #endregion
    }
}
