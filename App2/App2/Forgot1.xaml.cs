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
	public partial class Forgot1 : ContentPage
	{
		public Forgot1 ()
		{
			InitializeComponent ();
		}

        private async void OnRestoreClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Forgot2());
        }
    }
}