using System;

namespace Platform7
{
    public class MessageOptions
    {

        public string cityName;
        public string countryName;  
        public string CityName
        {

            get { return cityName; }

            set { cityName = value; }
        }

        public string CountryName
        {

            get { return countryName; }

            set { countryName = value; }


        }


        //public string CityName { get; set; } = "New York";
        //public string CountryName { get; set; } = "USA";

        //public string CityName { get; set; }
        //public string CountryName { get; set; }


    }



}
