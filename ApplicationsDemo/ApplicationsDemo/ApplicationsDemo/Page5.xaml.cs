using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ApplicationsDemo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Page5 : ContentPage
	{
		public Page5 ()
		{
		    //Remove back button, it will be handled in out button
		    NavigationPage.SetHasBackButton(this, false);
		    //Removes the bar at the top
		    NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent ();
		}

	    private void Button_OnClicked(object sender, EventArgs e)
	    {
	        App.Current.MainPage.Navigation.PopAsync();
	    }
	}
}