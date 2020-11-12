using Newtonsoft.Json;
using System.Collections.Generic;

/// <summary>
/// Author: Huang Chaoshan, Lim Pei Yan
/// The Dtos aims to convert the JsonProperty to C# object
/// </summary>
namespace BricksMeatballs.Dtos
{
    /// <summary>
    /// The PmiResidenceTransactionDto defines the get set method for the JSON Property fetched from the api call
    /// </summary>
    public class PmiResidenceTransactionDto
    {
        [JsonProperty("contractDate")]
        public string ContractDate { get; set; }

        [JsonProperty("area")]
        public string Area { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("propertyType")]
        public string PropertyType { get; set; }

        [JsonProperty("typeOfArea")]
        public string TypeOfArea { get; set; }

        [JsonProperty("tenure")]
        public string Tenure { get; set; }

        [JsonProperty("floorRange")]
        public string FloorRange { get; set; }

        [JsonProperty("typeOfSale")]
        public string TypeOfSale { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("noOfUnits")]
        public string NoOfUnits { get; set; }
    }

    /// <summary>
    /// The PmiResidenceResultDto defines the get set method for the JSON Property fetched from the api call
    /// </summary>
    public class PmiResidenceResultDto
    {
        [JsonProperty("project")]
        public string Project { get; set; }

        [JsonProperty("marketSegment")]
        public string MarketSegment { get; set; }

        [JsonProperty("transaction")]
        public List<PmiResidenceTransactionDto> Transaction { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("y")]
        public string Y { get; set; }

        [JsonProperty("x")]
        public string X { get; set; }
    }

    /// <summary>
    /// The PmiResidenceTransactionResponseDto defines the get set method for the JSON Property fetched from the api call
    /// </summary>
    public class PmiResidenceTransactionResponseDto
    {
        [JsonProperty("Result")]
        public List<PmiResidenceResultDto> PmiResidenceResults { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }
    }



}
