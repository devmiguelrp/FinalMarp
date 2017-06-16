using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emotions
{
    public partial class ItemManager
    {
        static ItemManager defaultInstance = new ItemManager();
        MobileServiceClient client;
        IMobileServiceTable<TodoItem> todoTable;
        private ItemManager()
        {
            this.client = new
            MobileServiceClient(@"https://finalmarp.azurewebsites.net/");
            this.todoTable = client.GetTable<TodoItem>();
        }
        public static ItemManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }
        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }
        public async Task SaveTaskAsync(TodoItem item)
        {
            //if (item.Id == null)
            //{
                await todoTable.InsertAsync(item);
            //}
            //else
            //{
            //    await todoTable.UpdateAsync(item);
            //}
        }
    }
    public class TodoItem
    {
        private string _id;
        private string _nombre;
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }
        [JsonProperty(PropertyName = "nombre")]
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                _nombre = value;
            }
        }
        //[JsonProperty(PropertyName = "sexo")]
        //public bool Sexo { get; set; }
        [Version]
        public string Version { get; set; }
    }
}