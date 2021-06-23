using Swiper.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Swiper.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwiperControl : ContentView
    {
        //Created class instance variables, declared atop for readablitiy
        //Underscore prefixed variables "_Variable" are considered "temp" variables
        private readonly double _initialRotation;
        private static readonly Random _random = new Random();
        private double _screenWidth = -1;
        private const double DeadZone = 0.4d;
        private const double DecisionThreshold = 0.4d;

        public SwiperControl()
        {
            //Starts the component
            InitializeComponent();

            //Sets up the pan gesture and sets some initial rotation values
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            this.GestureRecognizers.Add(panGesture);

            _initialRotation = _random.Next(-10, 10);
            photo.RotateTo(_initialRotation, 100, Easing.SinInOut);

            //Initializes an instance of the picture class
            var picture = new Picture();
            //Sets the description text of the swipper control to the discription of the picture class
            descriptionLabel.Text = picture.Description;
            //Sets the image source to the picture instance of this picture class using the UriImageSource
            //"built-in" to retrieve the image from the the web
            image.Source = new UriImageSource() { Uri = picture.Uri };
            
            //Sets the picture loading label to be visible and passes text to display
            loadingLabel.SetBinding(IsVisibleProperty, "IsLoading");
            //Sets the loadingLabel to the downloaded image retrieved from the web
            loadingLabel.BindingContext = image;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (Application.Current.MainPage == null)
            {
                return;
            }
            
            //fieled used to store the width as soon as its resolved by the OnSizeAllocated method
            _screenWidth = Application.Current.MainPage.Width;
        }

        //Method takes a value to set a min boundary and a maximum boundary
        private static double Clamp(double value, double min, double max)
        {
            //Ternary expression to return the edge values
            return (value < min) ? min : (value > max) ? max : value;
        }
        
        //Calculating the the "pan zones" that determine the state of the image
        private void CalculatePanState(double panX)
        {
            var halfScreenWidth = _screenWidth / 2;
            var deadZoneEnd = DeadZone * halfScreenWidth;

            if (Math.Abs(panX) < deadZoneEnd)
            {
                return;
            }

            var passedDeadzone = panX < 0 ? panX + deadZoneEnd : panX - deadZoneEnd;
            var decsionZoneEnd = DecisionThreshold * halfScreenWidth;
            var opacity = passedDeadzone / decsionZoneEnd;

            opacity = Clamp(opacity, -1, 1);

            likeStackLayout.Opacity = opacity;
            denyStackLayout.Opacity = -opacity;
        }

        //Standard way of handling events in C# environment
        //Controls the panning of the photo with switch statement
        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    PanStarted();
                    break;

                case GestureStatus.Running:
                    PanRunning(e);
                    break;

                case GestureStatus.Completed:
                    PanCompleted();
                    break;
            }
        }

        //Scales the photo upon pan(dragging) the image 110% of current value
        private void PanStarted()
        {
            photo.ScaleTo(1.1, 100);
        }

        //Sizes the picture during the pan event
        //One for the X-axis and one for the Y-axis
        private void PanRunning(PanUpdatedEventArgs e)
        {
            photo.TranslationX = e.TotalX;
            photo.TranslationY = e.TotalY;
            photo.Rotation = _initialRotation + (photo.TranslationX / 25);

            CalculatePanState(e.TotalX);
        }

        //Restores the image once the pan is complete and image is released
        private void PanCompleted()
        {
            //Moves image back its original location in 250ms using easing function for effect
            photo.TranslateTo(0, 0, 250, Easing.SpringOut);
            photo.RotateTo(_initialRotation, 250, Easing.SpringOut);
            photo.ScaleTo(1, 250);
        }

        //Function that determines if an image has been panned far enough to exit
        private bool CheckForExitCriteria()
        {
            var halfScreenWidth = _screenWidth / 2;
            var decisionBreakpoint = DeadZone * halfScreenWidth;
            return (Math.Abs(photo.TranslationX) > decisionBreakpoint);
        }

    }
}