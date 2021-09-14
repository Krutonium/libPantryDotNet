using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace libPantryDotNet
{
    public class Pantry
    {
        private static WebClient client = new WebClient();
        /// <summary>
        /// Gets a users pantry, including statistics about it.
        /// </summary>
        /// <param name="pantryId">Your pantry ID</param>
        /// <returns>PantryInfo</returns>
        public PantryInfo GetPantry(string pantryId)
        {
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            client.Encoding = Encoding.UTF8;
            var pantryInfo = JsonSerializer.Deserialize<PantryInfo>
                (client.DownloadString("https://getpantry.cloud/apiv1/pantry/" + pantryId));
            return pantryInfo;
        }
        /// <summary>
        /// Gets the JSON from a basket by name. It's up to you to deserialize it, or use the dynamic object.
        /// </summary>
        /// <param name="basketId">the name of the basket to get the json from</param>
        /// <param name="pantryId">the pantry to access to get the basket</param>
        /// <returns>string</returns>
        public string GetBasket(string basketName, string pantryId)
        {
            return client.DownloadString
                ("https://getpantry.cloud/apiv1/pantry/" + pantryId + "/basket/" + basketName);
        }

        /// <summary>
        /// Given a basket name, this will update the existing contents and
        /// return the contents of the newly updated basket.
        /// This operation performs a deep merge and will overwrite the values of any existing keys,
        /// or append values to nested objects or arrays.
        /// </summary>
        /// <param name="name">name of the basket</param>
        /// <param name="contents">JSON to be uploaded</param>
        /// <param name="pantryId">the pantry that contains the basket</param>
        /// <returns>Result rom the service</returns>
        public string UpdateBasket(string name, string contents, string pantryId)
        {
            //https://getpantry.cloud/apiv1/pantry/YOUR_PANTRY_ID/basket/YOUR_BASKET_NAME
            //"https://getpantry.cloud/apiv1/pantry/" + pantryId + "/basket/" + name
            HttpClient httpClient = new HttpClient();
            var response = client.UploadString
                ("https://getpantry.cloud/apiv1/pantry/" + pantryId + "/basket/" + name, 
                    WebRequestMethods.Http.Put,  
                    contents);

            return response;
        }
        /// <summary>
        /// Create a new basket with some default content
        /// </summary>
        /// <param name="name">The name of your new basket</param>
        /// <param name="contents">The JSON to upload</param>
        /// <param name="pantryId">The pantry you should be accessing</param>
        /// <returns>Result from the service</returns>
        public string CreateBasket(string name, string contents, string pantryId)
        {
            //https://getpantry.cloud/apiv1/pantry/YOUR_PANTRY_ID/basket/YOUR_BASKET_NAME
            //https://getpantry.cloud/apiv1/pantry/" + pantryId + "/basket/" + name
            var response =
                client.UploadString
                ("https://getpantry.cloud/apiv1/pantry/" + pantryId + "/basket/" + name,
                    WebRequestMethods.Http.Post,contents);
            return response;
        }
        /// <summary>
        /// Deletes a Basket. WARNING: THIS IS IRREVERSABLE - USE WITH CAUTION.
        /// </summary>
        /// <param name="name">Name of the basket to delete</param>
        /// <param name="pantryId">Pantry to delete it from</param>
        /// <param name="AreYouSure">This cannot be undone.</param>
        /// <returns>Result from the service</returns>
        public string DeleteBasket(string name, string pantryId, bool AreYouSure)
        {
            //https://getpantry.cloud/apiv1/pantry/YOUR_PANTRY_ID/basket/YOUR_BASKET_NAME
            //https://getpantry.cloud/apiv1/pantry/" + pantryId + "/basket/" + name
            HttpClient httpClient = new HttpClient();
            var response = httpClient.DeleteAsync("https://getpantry.cloud/apiv1/pantry/" + pantryId + "/basket/" + name).Result;
            return response.Content.ReadAsStringAsync().Result;
            //WebClient doesn't seem to support Delete.
        }

        public class PantryInfo
        {
            public string name { get; set; }
            public string description { get; set; }
            public List<object> errors { get; set; }
            public bool notifications { get; set; }
            public int percentFull { get; set; }
            public List<string> baskets { get; set; }
        }
    }
}