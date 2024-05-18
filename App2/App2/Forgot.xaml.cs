using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Forgot : ContentPage
	{
		public Forgot ()
		{
			InitializeComponent ();
		}

        private async void OnRestoreClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Forgot1());
        }
    }
}