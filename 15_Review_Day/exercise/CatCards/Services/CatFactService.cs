using CatCards.Models;
using RestSharp;
using System;

namespace CatCards.Services
{
    public class CatFactService : ICatFactService
    {

        private readonly string API_PULL = @"https://cat-data.netlify.app/api/facts/random";

        private readonly RestClient client = new RestClient();

        private CatFact catFact;


        public CatFact GetFact()
        {
            RestRequest request = new RestRequest(API_PULL);
            IRestResponse<CatFact> response = client.Get<CatFact>(request);
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

                    catFact = response.Data;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occurred - getting cat fact " + ex.Message);
                }

                return catFact;
            }
        }
    }
}