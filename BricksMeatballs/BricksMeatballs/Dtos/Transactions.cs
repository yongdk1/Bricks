using Newtonsoft.Json;
using System.Collections.Generic;


namespace BricksMeatballs.Dtos
{

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

    public class PmiResidenceTransactionResponseDto
    {
        [JsonProperty("Result")]
        public List<PmiResidenceResultDto> PmiResidenceResults { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }
    }



}
