using BricksMeatballs.Dtos;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BricksMeatballs.Services
{
    /// <summary>
    /// Author: Huang Chaoshan, Lim Pei Yan
    /// The URA services is to make HTTP requests using  IHttpClientFactory to fetch the past transaction data from the URA API 
    /// and convert the result and stored into database.
    /// </summary>
    public class UraService
    {
        private readonly HttpClient _httpClient;
        public static string ConectionString = string.Empty;

        /// <summary>
        /// This method defines the api address, access key and token. Using all these information call the api. 
        /// </summary>
        /// <param name="httpClient"></param>
        public UraService(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://www.ura.gov.sg/uraDataService/invokeUraDS");
            httpClient.DefaultRequestHeaders.Add("AccessKey", "c28f758a-2fe4-401a-b7ad-715396a73048");
            httpClient.DefaultRequestHeaders.Add("Token", "");//"4-G6FjT32e4U4KNc1kWzDtH7eB3Za45zj72f2DH42ws59dPa08Ag-5k75xf92-4NTANkAVPJ7m4dfd7x4Y7c-a2sE8wdtvxRJwCK");
            _httpClient = httpClient;

        }

        /// <summary>
        /// This method is a asynchronous task that will run the batch and get the response from the URA API 
        /// and deserialize them to string
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        public async Task<PmiResidenceTransactionResponseDto> GetPMI_Resi_Transaction(int batch)
        {
            //  InsertData();
            var response = await _httpClient.GetAsync("?service=PMI_Resi_Transaction&batch=" + batch);
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            string Json = responseStream.ToString();
            try
            {
                PmiResidenceTransactionResponseDto objData = JsonConvert.DeserializeObject<PmiResidenceTransactionResponseDto>(responseStream);
                InsertRecord(objData);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return JsonConvert.DeserializeObject<PmiResidenceTransactionResponseDto>(responseStream);

        }

        /// <summary>
        /// This method will insert into MySQL after the transaction result is successfully fetched from the api call
        /// </summary>
        /// <param name="objData"></param>
        private void InsertRecord(PmiResidenceTransactionResponseDto objData)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(ConectionString);

                MySqlCommand cmd = null;
                string cmdString = "";
                conn.Open();
                foreach (PmiResidenceResultDto item in objData.PmiResidenceResults)
                {
                    try
                    {
                        foreach (PmiResidenceTransactionDto transactionDto in item.Transaction)
                        {
                            try
                            {
                                cmdString = "insert into pmiresidenceresult(street, x, project, y, area, floorrange, noofunits, contractdate, typeofsale, price, propertytype, district, typeofarea, tenure, marketsegment) values('" + item.Street + "','" + item.X + "','" + item.Project + "','" + item.Y + "','" + transactionDto.Area + "','" + transactionDto.FloorRange + "','" + transactionDto.NoOfUnits + "','" + transactionDto.ContractDate + "','" + transactionDto.TypeOfSale + "','" + transactionDto.Price + "','" + transactionDto.PropertyType + "','" + transactionDto.District + "','" + transactionDto.TypeOfArea + "','" + transactionDto.Tenure + "','" + item.MarketSegment + "')";
                                cmd = new MySqlCommand(cmdString, conn);
                                int result = cmd.ExecuteNonQuery();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("{0} Exception caught.", e);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Exception caught.", e);
                    }
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }
    }
}
