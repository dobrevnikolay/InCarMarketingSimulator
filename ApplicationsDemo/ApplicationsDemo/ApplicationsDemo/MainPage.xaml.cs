using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ApplicationsDemo
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{

		    //Remove back button, it will be handled in out button
		    NavigationPage.SetHasBackButton(this, false);
		    //Removes the bar at the top
		    NavigationPage.SetHasNavigationBar(this, false);


            InitializeComponent();
		}

	    private void Button_OnClicked(object sender, EventArgs e)
	    {
	        App.Current.MainPage.Navigation.PushAsync(new Page2());
	    }
	}
}
