using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using JisalimuOnline.Models;
using Nest;
using JisalimuOnline.ViewModels;

namespace JisalimuOnline.Controllers
{
     [RoutePrefix("api/v1/users")]
    public class UsersController : ApiController
    {
         private JisalimuContext db = new JisalimuContext();

        [Route("")]
        [HttpGet]
        // GET: api/Users
        public IQueryable<User> GetUser()
        {
            Uri node = new Uri("http://jisalimu.cloudapp.net:9200/", UriKind.Absolute);

            ConnectionSettings settings = new ConnectionSettings(
              node,
              defaultIndex: "jisalimu"
          );

            ElasticClient client = new ElasticClient(settings);

            var result = client.Search<User>(q =>
                q.From(0)
                .Size(200)
                .MatchAll());
            
            
            return result.Documents.AsQueryable();
        }

         [Route("trigger")]
         [HttpPost]
        public IHttpActionResult ProcessTrigger(Trigger trigger)
        {
            PerformTrigger(trigger);
            return Ok("Trigger successfully sent");
        }

         public void PerformTrigger(Trigger myTrigger)
         {
             Uri node = new Uri("http://jisalimu.cloudapp.net:9200/", UriKind.Absolute);

             ConnectionSettings settings = new ConnectionSettings(
               node,
               defaultIndex: "jisalimu"
           );

             ElasticClient client = new ElasticClient(settings);

             client.Index<Trigger>(myTrigger);
         }

        [Route("{id}")]
        [HttpGet]
        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string id)
        {
            Uri node = new Uri("http://jisalimu.cloudapp.net:9200/", UriKind.Absolute);

            ConnectionSettings settings = new ConnectionSettings(
              node,
              defaultIndex: "jisalimu"
          );

            ElasticClient client = new ElasticClient(settings);

            string QUERY = "";
            var results = client.Search<User>(q => q.QueryRaw(QUERY));

            //////////////////////////////
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            var user_results = results.Documents;

            return Ok(user);
        }

        [Route("login")]
        [HttpPost]
         public IHttpActionResult Login(LoginViewModel viewModel )
        {

            Uri node = new Uri("http://jisalimu.cloudapp.net:9200/", UriKind.Absolute);

            ConnectionSettings settings = new ConnectionSettings(
              node,
              defaultIndex: "jisalimu"
          );

            ElasticClient client = new ElasticClient(settings);

            var res = client.Search<User>(q => q.Query( t => t.Term("email",viewModel.email) && t.Term("password",viewModel.password)));
            //var user = db.User.FirstOrDefault(u => u.email == viewModel.email && u.password == viewModel.password);
            if (res.Documents != null)
	        {
                
                return Ok(res.Documents.FirstOrDefault());
	        }
            else
            {
                return BadRequest("The user does not exist");
            }
          
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(string id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.userid)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


         [Route("register")]
         [HttpGet]
         public IHttpActionResult RegisterSample()
            {
                RegisterViewModel viewmodel = new RegisterViewModel();
                return Ok(viewmodel);
            }
        // POST: api/Users
        [Route("register")]
        [HttpPost]
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(RegisterViewModel viewModel)
        {
             Uri node = new Uri("http://jisalimu.cloudapp.net:9200/", UriKind.Absolute);

          ConnectionSettings settings = new ConnectionSettings(
            node,
            defaultIndex: "jisalimu"
        );

         ElasticClient client = new ElasticClient(settings);

            JisalimuOnline.Models.User newuser = new JisalimuOnline.Models.User();
            newuser.userid = Guid.NewGuid().ToString();
            newuser.firstname = viewModel.firstname;
            newuser.lastname = viewModel.lastname;
            newuser.password = viewModel.password;
            newuser.phonenumber = viewModel.phonenumber;
            newuser.county = viewModel.county;
            newuser.gender = viewModel.gender;
            newuser.date_of_birth = viewModel.date_of_birth;
            newuser.register_date = DateTime.Today.ToString();
            newuser.first_contact = viewModel.first_contact;
            newuser.second_contact = viewModel.second_contact;
            newuser.third_contact = viewModel.third_contact;

            //Try to index the user into the database
            client.Index<User>(newuser);
            client.Refresh();

            //db.User.Add(newuser);


            return Ok(newuser);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(string id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.User.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string id)
        {
            return db.User.Count(e => e.userid == id) > 0;
        }
    }
}