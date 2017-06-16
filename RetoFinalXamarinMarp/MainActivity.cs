using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Runtime;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Android.Views;
using System;
using RetoFinalXamarinMarp.Services;

namespace RetoFinalXamarinMarp
{
    [Activity(Label = "Reto Final Xamarin", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private MobileServiceClient client;
        private MobileServiceUser user;
        //public static MobileServiceClient MobileService = new MobileServiceClient("https://finalmarp.azurewebsites.net");
        Button btnStartSession;
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            CurrentPlatform.Init();


            #region Registro de actividad
            //try
            //{
            //    ServiceHelper serviceHelper = new ServiceHelper();
            //    string AndroidId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            //    //btnRegistrar.Enabled = false;

            //    Toast.MakeText(this, "Enviando tu registro", ToastLength.Short).Show();
            //    await serviceHelper.InsertarEntidad("miguel.ramosp@outlook.com", "RetoFinal + https://github.com/devmiguelrp/RetoFinalXamarinMarp", AndroidId);
            //    Toast.MakeText(this, "Gracias por registrarte", ToastLength.Long).Show();

            //}
            //catch (Exception exc)
            //{
            //    Toast.MakeText(this, exc.Message, ToastLength.Long).Show();
            //}
            #endregion

            client = new MobileServiceClient("https://finalmarp.azurewebsites.net");

            btnStartSession = FindViewById<Button>(Resource.Id.btnIniciarSesion);
            btnStartSession.Click += BtnStartSession_Click;
            //btnStartSession.Visibility = Android.Views.ViewStates.Invisible;
            //iniciar plugin
            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;

            //OnRefreshItemsSelected();
            OnTryAuth();
        }

        private void BtnStartSession_Click(object sender, System.EventArgs e)
        {
            
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                //Toast.MakeText(this, "Conectado a Internet", ToastLength.Long).Show();
                var intent = new Intent(this, typeof(MedidorActivity));

                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Actualmente no estás conectado a Internet", ToastLength.Long).Show();
            }

            
        }

        private void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                btnStartSession.Visibility = Android.Views.ViewStates.Visible;
                Toast.MakeText(this, "Conectado a Internet", ToastLength.Long).Show();
            }
            else
            {
                btnStartSession.Visibility = Android.Views.ViewStates.Invisible;
                Toast.MakeText(this, "No hay una conexión disponible", ToastLength.Long).Show();
            }
        }


        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == 1 && resultCode == Result.Ok)
            {
                btnStartSession.Visibility = Android.Views.ViewStates.Invisible;
            }
        }

        //FB
        private async Task<bool> Authenticate()
        {
            var success = false;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                user = await client.LoginAsync(this, MobileServiceAuthenticationProvider.Facebook);
                CreateAndShowDialog(string.Format("Inicio exitoso - {0}", user.UserId), "Logged in!");
                //Toast.MakeText(this, string.Format("Inicio exitoso - {0}", user.UserId), ToastLength.Long).Show();
                success = true;
            }
            catch (Exception ex)
            {
                CreateAndShowDialog(ex, "Authentication failed");
            }
            return success;
        }
        [Java.Interop.Export()]
        public async void LoginUser(View view)
        {
            // Load data only after authentication succeeds.
            if (await Authenticate())
            {
                //Hide the button after authentication succeeds.
                FindViewById<Button>(Resource.Id.btnIniciarSesion).Visibility = ViewStates.Gone;
                //Toast.MakeText(this, string.Format("Inicio exitoso - {0}", user.UserId), ToastLength.Long).Show();
                // Load the data.
                //OnRefreshItemsSelected();
            }
        }
        private void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        private void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

        private async void OnRefreshItemsSelected()
        {
            // refresh view using local store.
            //await RefreshItemsFromTableAsync();
        }

        private async void OnTryAuth()
        {
            await Authenticate();
        }

    }
}

