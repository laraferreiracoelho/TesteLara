using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TesteDois.Models;
using TesteDois.Repos;

namespace TesteDois.Controllers
{
    public class UsuarioController : ApiController
    {
        string awsAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
        string AWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
        string ServiceUrl = ConfigurationManager.AppSettings["ServiceUrl"];
        public async Task<IList<Usuario>> Get()
        {
            
            var repos = new DynamoDBHelper(awsAccessKey, AWSSecretKey, ServiceUrl);

            
            var response = await repos.GetRows<Usuario>(new List<ScanCondition>());
            
            return response;
          
        }
        [Route("~/api/Usuario/GetByEmail")]
        public async Task<Usuario> GetByEmail([FromUri] string email)
        {

            var repos = new DynamoDBHelper(awsAccessKey, AWSSecretKey, ServiceUrl);

            List<ScanCondition> conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition("email", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, email));
            var response = await repos.GetRows<Usuario>(conditions);

            return response.FirstOrDefault();

        }
        // GET api/values/5
        public Usuario Get(string id)
        {
            var repos = new DynamoDBHelper(awsAccessKey, AWSSecretKey, ServiceUrl);

            var response = repos.Load<Usuario>(id);

            return response;
        }



        // POST api/values
        public async Task Post([FromBody]Usuario usuario)
        {

            using (var repos = new DynamoDBHelper(awsAccessKey, AWSSecretKey, ServiceUrl))
            {
                await repos.Save<Usuario>(usuario);
            }
            

        }

        // PUT api/values/5
        public async Task Put(string id, [FromBody]Usuario usuario)
        {

            using (var repos = new DynamoDBHelper(awsAccessKey, AWSSecretKey, ServiceUrl))
            {
                var response = repos.Load<Usuario>(id);
                response.Nome = usuario.Nome!=null? usuario.Nome: response.Nome;
                response.email = usuario.email != null ? usuario.email : response.email;
                response.Nivel = usuario.Nivel != null ? usuario.Nivel : response.Nivel;
                response.DataNascimento = usuario.DataNascimento !=DateTime.MinValue? usuario.DataNascimento : response.DataNascimento;
                response.Documento = usuario.Documento != null ? usuario.Documento : response.Documento;
                await repos.Save<Usuario>(response);
            }


        }

        // DELETE api/values/5
        public async Task Delete(string id)
        {
            using (var repos = new DynamoDBHelper(awsAccessKey, AWSSecretKey, ServiceUrl))
            {
                var response = repos.Load<Usuario>(id);
                await repos.Delete<Usuario>(response);
            }
        }
    }
}

