using JisalimuOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nest;

namespace JisalimuOnline.Controllers
{
    [RoutePrefix("api/v1/hospitals")]
    public class HospitalsController : ApiController
    {
        
        //The method below gets hospitals via a spatial search from Elasticsearch
        [Route("{latitude}/{longitude}/{distance}")]
        [HttpGet]
        public IHttpActionResult GetHospitals(double latitude, double longitude, double distance)
        {
           var  node = new Uri("http://jisalimu.cloudapp.net:9200/", UriKind.Absolute);

         var settings = new ConnectionSettings(
            node,
            defaultIndex: "jisalimu"
        );

        var client = new ElasticClient(settings);

             //THis is a function to get hospitals and index it into the user tags
            double LATITUDE = latitude, LONGITUDE = longitude, DISTANCE = distance;

            var result = client.Search<Hospital>(s => s
                .Filter(f => f
                  .GeoDistance(
                    n => n.geolocation,
                    d => d.Distance(DISTANCE, GeoUnit.Kilometers).Location(LATITUDE, LONGITUDE)
                  )
                )
              );
           
            //Return the hospitals in a HTTP OK-200 object
            return Ok(result.Documents);
            //Create a hospital request object and index it on ElasticSearch
        }

        [Route("{latitude}/{longitude}/{distance}/{userid}")]
        [HttpGet]
        public IHttpActionResult GetHospitalsUser(double latitude, double longitude, double distance, string userid)
        {
            var node = new Uri("http://jisalimu.cloudapp.net:9200/", UriKind.Absolute);

            var settings = new ConnectionSettings(
               node,
               defaultIndex: "jisalimu"
           );

            var client = new ElasticClient(settings);

            //THis is a function to get hospitals and index it into the user tags
            double LATITUDE = latitude, LONGITUDE = longitude, DISTANCE = distance;

            var result = client.Search<Hospital>(s => s
                .Filter(f => f
                  .GeoDistance(
                    n => n.geolocation,
                    d => d.Distance(DISTANCE, GeoUnit.Kilometers).Location(LATITUDE, LONGITUDE)
                  )
                )
              );

            //Return the hospitals in a HTTP OK-200 object
            return Ok(result.Documents);
            //Create a hospital request object and index it on ElasticSearch
        }
    }
}
