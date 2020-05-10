using Newtonsoft.Json;
using RestSharp.Validation;
using System;
using System.Collections.Generic;
using System.IO;

namespace HotelMng
{
    public class Reservation
    {

        public long id { get; set; }

        [JsonRequired]
        public string guest { get; set; }

        [JsonRequired]
        public int noOfPersons { get; set; }

        [JsonRequired]
        public string arrivalDate { get; set; }

        [JsonRequired]
        public string departureDate { get; set; }

        [JsonRequired]
        public Enum roomType;

        [JsonRequired]
        public float roomFare { get; set; }

        [JsonRequired]
        public bool breakfastIncluded { get; set; }


       

    }
}
