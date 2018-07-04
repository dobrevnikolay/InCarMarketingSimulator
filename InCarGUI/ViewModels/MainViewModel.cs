using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using Core;
using Core.Models;
using Microsoft.Win32;

namespace InCarGUI
{
    public class MainViewModel : BaseViewModel
    {
        #region Private members

        private string _pathToImageToShow;

        private Bitmap _screenPictureBitmap;

        private string _patternSearchResult;

        private readonly IDataProvider _database;

        private readonly IPatternMatcher _patternMatcher;

        private IGeoInfoProvider _geoInfoProvider;

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

            _database = new MongoDataBase("InCarMarketing", "InCarMarketingScreen");
            _patternMatcher = new OpenCvPatternMatcher();
            _geoInfoProvider = new GoogleMapsInfoProvider();
            
        }

        private void ForceAdCreation()
        {
            //get from database and print
            var adcreator = new AdsCreator(_geoInfoProvider, _database);

            var result = adcreator.CreateAd("none");

            PatternSearchResult = result;
        }

        private async Task FindPatternsAsync()
        {
            try
            {
                
                ScreenAnalyzer sa = new ScreenAnalyzer(_database, _patternMatcher);
                await sa.AnalyzeScreenAsync(_screenPictureBitmap);
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
