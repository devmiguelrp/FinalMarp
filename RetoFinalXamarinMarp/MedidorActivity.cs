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
using Emotions;
using Plugin.Media.Abstractions;
using System.IO;

namespace RetoFinalXamarinMarp
{
    [Activity(Label = "MedidorActivity")]
    public class MedidorActivity : Activity
    {
        ItemManager manager;
        static System.IO.Stream streamCopy;
        TextView txtResultado;
        Button btnAnalizaFoto;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            manager = ItemManager.DefaultManager;
            SetContentView(Resource.Layout.Medidor);
            Button btnCamara = FindViewById<Button>(Resource.Id.btnCamara);
            btnAnalizaFoto = FindViewById<Button>(Resource.Id.btnAnalizaFoto);
            txtResultado = FindViewById<TextView>(Resource.Id.txtOutput);
            btnAnalizaFoto.Visibility = ViewStates.Invisible;
            btnCamara.Click += BtnCamara_Click;
            btnAnalizaFoto.Click += BtnAnalizaFoto_Click;
        }

        private async void BtnAnalizaFoto_Click(object sender, EventArgs e)
        {
            if (streamCopy != null)
            {
                btnAnalizaFoto.Visibility = ViewStates.Invisible;
                Toast.MakeText(this, "Analizando imagen utilizando Cognitive Services", ToastLength.Short).Show();
                Dictionary<string, float> emotions = null;
                try
                {
                    streamCopy.Seek(0, SeekOrigin.Begin);
                    emotions = await ServiceEmotions.GetEmotions(streamCopy);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Se ha presentado un error al conectar con los servicios", ToastLength.Short).Show();
                    return;
                }
                StringBuilder sb = new StringBuilder();
                if (emotions != null)
                {
                    sb.AppendLine();
                    
                    var max = emotions.Max(em => em.Value);
                    var estadoMax = emotions.Where(em => em.Value == max);
                    txtResultado.Text +="Todo indica que tu estado de ánimo es "+ estadoMax.First().Key + " con un " + Math.Round (estadoMax.First().Value*100,1) + "% de probabilidad.";
                    //registrar info en azure
                    TodoItem todoItem  = new TodoItem();
                    todoItem.Id = Android.Provider.Settings.Secure.GetString(ContentResolver,
                    Android.Provider.Settings.Secure.AndroidId);
                    todoItem.Nombre = "Miguel";
                   // await ItemManager.DefaultManager.SaveTaskAsync(todoItem);
                }
                else txtResultado.Text = "---No se detectó una cara---";
                //ResultadoEmociones += sb.ToString();
            }
            else txtResultado.Text = "---No has seleccionado una imagen---";
        }
        private async void BtnCamara_Click(object sender, EventArgs e)
        {
            MediaFile file = null;
            try
            {
                file = await ServiceImage.TakePicture("retofinal", true);
            }
            catch (Android.OS.OperationCanceledException)
            {
            }
            SetImageToControl(file);
            btnAnalizaFoto.Visibility = ViewStates.Visible;
        }
        private void SetImageToControl(MediaFile file)
        {
            if (file == null)
            {
                return;
            }
            ImageView imgImage = FindViewById<ImageView>(Resource.Id.imageViewFoto);
            imgImage.SetImageURI(Android.Net.Uri.Parse(file.Path));
            var stream = file.GetStream();
            streamCopy = new MemoryStream();
            stream.CopyTo(streamCopy);
            stream.Seek(0, SeekOrigin.Begin);
            file.Dispose();
        }
    }
}