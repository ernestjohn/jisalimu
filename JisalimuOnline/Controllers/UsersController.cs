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
using JisalimuLibrary.Models;
using JisalimuOnline.Models;
using JisalimuLibrary.ViewModels;
using Nest;

namespace JisalimuOnline.Controllers
{
     [RoutePrefix("api/v1/users")]
    public class UsersController : ApiController
    {
        private JisalimuContext db = new JisalimuContext();
        Uri node = new Uri("http://quintelelastic.cloudapp.net");
        ConnectionSettings settings = new ConnectionSettings(node);
        public ElasticClient client = new ElasticClient(settings);

        [Route("")]
        [HttpGet]
        // GET: api/Users
        public IQueryable<User> GetUser()
        {
            //client.Search<User>(q => q.Query(c => c.Term(t => t.)))

            return db.User;
        }

        [Route("{id}")]
        [HttpGet]
        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Route("login")]
        [HttpPost]
         public IHttpActionResult Login(LoginViewModel viewModel )
        {
            var user = db.User.FirstOrDefault(u => u.email == viewModel.email && u.password == viewModel.password);
            if (user != null)
	        {
                return Ok(user);
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

        // POST: api/Users
        [Route("register")]
        [HttpPost]
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(RegisterViewModel viewModel)
        {
            JisalimuLibrary.Models.User newuser = new JisalimuLibrary.Models.User();
            newuser.userid = Guid.NewGuid().ToString();
            newuser.firstname = viewModel.firstname;
            newuser.lastname = viewModel.lastname;
            newuser.password = viewModel.password;
            newuser.phonenumber = viewModel.phonenumber;
            newuser.county = viewModel.county;
            newuser.gender = viewModel.gender;
            newuser.date_of_birth = viewModel.date_of_birth;
            newuser.register_date = DateTime.Today.ToString();

            //Try to index the user into the database
            client.Index<User>(newuser);
            client.Refresh();

            db.User.Add(newuser);


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserExists(newuser.userid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

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