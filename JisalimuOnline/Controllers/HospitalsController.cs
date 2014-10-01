using JisalimuOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nest;
using JisalimuLibrary.Models;

namespace JisalimuOnline.Controllers
{
    [RoutePrefix("api/v1/hospitals")]
    public class HospitalsController : ApiController
    {
        private JisalimuContext db = new JisalimuContext();

        Uri node = new Uri("http://quintelelastic.cloudapp.net:9200/", UriKind.Absolute);

        ConnectionSettings settings = new ConnectionSettings(
            node,
            defaultIndex: "my-application"
        );

        ElasticClient client = new ElasticClient(settings);

        //The method below gets hospitals via a spatial search from Elasticsearch
        [Route("{latitude}/{longitude}/{distance}")]
        [HttpGet]
        public IHttpActionResult GetHospitals(double latitude, double longitude, double distance)
        {
            double LATITUDE = -1, LONGITUDE = 36;

            //Raw JSON query into elasticsearch
            string JSON_QUERY = "{\"filtered\" : {\"query\" : {\"match_all\" : {}},\"filter\" : {\"geo_distance\" : {\"distance\" : \"200km\",\"Hospital.geolocation.\" : {\"latitude\" : "+LATITUDE +",\"longitude\" : "+ LONGITUDE+"}}}}}";
            //Create a hospital request object and index it on ElasticSearch

           var result =  client.Search<Hospital>(q => q.QueryRaw(JSON_QUERY));
           
            //client.Search<Hospital>(r => 
            //    r.From(0)
            //    .Size(20)
            //    .Query( o => 
            //    o.Term(t => t. )));

            //Return the hospitals in a HTTP OK-200 object
           return Ok(result.Documents);
        }

        [Route("{latitude}/{longitude}/{distance}/{userid}")]
        [HttpGet]
        public IHttpActionResult GetHospitalsUser(double latitude, double longitude, double distance, string userid)
        {
            //THis is a function to get hospitals and index it into the user tags
            double LATITUDE = -1, LONGITUDE = 36;

            //Raw JSON query into elasticsearch
            string JSON_QUERY = "{\"filtered\" : {\"query\" : {\"match_all\" : {}},\"filter\" : {\"geo_distance\" : {\"distance\" : \"200km\",\"Hospital.geolocation.\" : {\"latitude\" : " + LATITUDE + ",\"longitude\" : " + LONGITUDE + "}}}}}";
            //Create a hospital request object and index it on ElasticSearch

            var result = client.Search<Hospital>(q => q.QueryRaw(JSON_QUERY));
           

            //client.Search<Hospital>(r => 
            //    r.From(0)
            //    .Size(20)
            //    .Query( o => 
            //    o.Term(t => t. )));

            //Return the hospitals in a HTTP OK-200 object
            return Ok(result.Documents);
            //Create a hospital request object and index it on ElasticSearch
        }
    }
}
