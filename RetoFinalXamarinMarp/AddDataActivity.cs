using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;

namespace RetoFinalXamarinMarp
{
    [Activity(Label = "Registrar información")]
    public class MedirFelicidadActivity : Activity
    {
        public static MobileServiceClient MobileService = new MobileServiceClient("https://finalmarp.azurewebsites.net");
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            //SetContentView(Resource.Layout.AddData);

            
            //TodoItem item = new TodoItem { Text = "Awesome item" };
            //await MobileService.GetTable<TodoItem>().InsertAsync(item);
        }
    }
}