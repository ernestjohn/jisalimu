using JisalimuLibrary.Models;
using JisalimuLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JisalimuLibrary
{
    public class JisalimuClient
    {
        //This is the official Jisalimu API abstraction for the Jisalimu Mobile, Windows
        private static JisalimuClient Instance;
        private HttpClient myClient = new HttpClient();

        public static JisalimuClient GetInstance()
        {
            if (Instance == null)
            {
                Instance = new JisalimuClient();
                return Instance;
            }
            else
            {
                return Instance;
            }
        }
        public User GetUser(string id)
        {
            return null;
        }
        public async Task<User> Login(string email, string password)
        {
            User user = new User();
            LoginViewModel viewModel = new LoginViewModel();
            viewModel.email = email; viewModel.password = password;

            var loginseria = JsonConvert.SerializeObject(viewModel);

            StringContent postdata = new StringContent(loginseria, Encoding.UTF8, "application/json");

            HttpResponseMessage result = await myClient.PostAsync("", postdata);

                try 
	            {	        
		            user = JsonConvert.DeserializeObject<User>(result.Content.ToString());
	            }
	            catch (Exception)
	            {
		
		            throw;
	            }
            return user;
        }

        public async Task<bool> RegisterUser(RegisterViewModel viewModel)
        {
      
            var registerseria = JsonConvert.SerializeObject(viewModel);

            StringContent postdata = new StringContent(registerseria, Encoding.UTF8, "application/json");

            HttpResponseMessage theresult = await myClient.PostAsync("", postdata);

                if (theresult.StatusCode == System.Net.HttpStatusCode.OK)
	            {
		            return true;
	            }
                else
	            {
                    return false;
	            }
           
        }
        public async Task<List<HospitalResult>> HospitalsAround(double latitude, double longitude, User user)
        {
            string response = await myClient.GetStringAsync("");
            
            var hosis = JsonConvert.DeserializeObject<List<HospitalResult>>(response);
            return hosis;

        }
        public void UpdateLocation(double latitude, double longitude, User user)
        {

        }
    }
}
