using CatCards.Models;
using RestSharp;
using System;

namespace CatCards.Services
{
    public class CatPicService : ICatPicService
    {

        private readonly string API_PULL = @"https://cat-data.netlify.app/api/pictures/random";

        private readonly RestClient client = new RestClient();

        private CatPic catPic;



        public CatPic GetPic()
        {
            RestRequest request = new RestRequest(API_PULL);
            IRestResponse<CatPic> response = client.Get<CatPic>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            else
            {
                try
                {

                    
                    catPic = response.Data;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occurred - getting cat fact" + ex.Message);
                }

                return catPic;
            }
        }
    }
}
