using BricksMeatballs.Dtos;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net;
using Newtonsoft.Json;

namespace BricksMeatballs.Services
{
    public class UraService
    {
        private readonly HttpClient _httpClient;
        public static string ConectionString = string.Empty;
        // private string ConString = "server=localhost;username=root;password=password;database=localhost;";
        // public MySqlConnection myConnection;

        public UraService(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://www.ura.gov.sg/uraDataService/invokeUraDS");
            httpClient.DefaultRequestHeaders.Add("AccessKey", "c28f758a-2fe4-401a-b7ad-715396a73048");
            httpClient.DefaultRequestHeaders.Add("Token", "0WkzNWa8teabcWZz3Bty7unwqyC30HJ8z75a@aF0Smz-mSGYP+My63-qp7NK+KaRe1Nt77CP7aJBJYE0r+Z40059y6Frvd4eazVT");

            _httpClient = httpClient;

        }

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
            catch (Exception)
            {

            }
            return JsonConvert.DeserializeObject<PmiResidenceTransactionResponseDto>(responseStream);

        }
        private void InsertRecord(PmiResidenceTransactionResponseDto objData)
        {
            try
            {

                //  MySqlConnection conn = new MySqlConnection("server=localhost;database=pmi;uid=root;pwd=mysql");
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
                                cmdString = "insert into pmiresidenceresult(street,x,project,y,area,floorrange,noofunits,contractdate,typeofsale,price,propertytype,district,typeofarea,tenure,marketsegment) values('" + item.Street + "','" + item.X + "','" + item.Project + "','" + item.Y + "','" + transactionDto.Area + "','" + transactionDto.FloorRange + "','" + transactionDto.NoOfUnits + "','" + transactionDto.ContractDate + "','" + transactionDto.TypeOfSale + "','" + transactionDto.Price + "','" + transactionDto.PropertyType + "','" + transactionDto.District + "','" + transactionDto.TypeOfArea + "','" + transactionDto.Tenure + "','" + item.MarketSegment + "')";
                                cmd = new MySqlCommand(cmdString, conn);
                                int result = cmd.ExecuteNonQuery();
                            }
                            catch (Exception E)
                            {
                            }
                        }
                    }
                    catch (Exception E)
                    {

                    }
                }
                conn.Close();
            }
            catch (Exception E)
            {

            }


        }
        //private void InsertData(PmiResidenceTransactionResponseDto objData)
        //{
        //    try
        //    {
        //        MySqlConnection conn = new MySqlConnection("server=localhost;database=pmi;uid=root;pwd=mysql");

        //        MySqlCommand cmd = null;
        //        string cmdString = "";
        //        conn.Open();


        //        cmdString = "insert into transaction values(" + ddd + ")";
        //        cmdString = "select * from transaction";
        //        cmd = new MySqlCommand(cmdString, conn);
        //        cmd.ExecuteNonQuery();

        //        conn.Close();

        //        //DataTable temp = new DataTable();
        //        //MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
        //        //adapter.Fill(temp);

        //    }
        //    catch (Exception E)
        //    {

        //    }


        //}

        //public void  MySQLDB()
        //{
        // MySql.Data.MySqlClient.MySqlConnection    myConnection = new MySql.Data.MySqlClient.MySqlConnection("");
        //    myConnection.Open();
        //}

        //public DataTable GetData(string _sqlCommand)
        //{
        //    MySqlCommand command = new MySqlCommand(_sqlCommand, myConnection);
        //    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
        //    DataTable dt = new DataTable();
        //    adapter.Fill(dt);
        //    return dt;
        //}

        //public void ExecuteQuery(string _sqlCommand)
        //{
        //    MySqlCommand command = new MySqlCommand("", myConnection);

        //}
    }

}
