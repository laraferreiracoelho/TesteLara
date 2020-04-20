using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesteDois.Utils;

namespace TesteDois.Models
{

    [DynamoDBTable("UsuariosDB")]
    public class Usuario
    {
        public Usuario()
        {
        }

        [DynamoDBHashKey] //Partition key
        public string id
        {
            get; set;
        }
        [DynamoDBProperty]
        public string email
        {
            get; set;
        }
        [DynamoDBProperty]
        public string Nome
        {
            get; set;
        }

        [DynamoDBProperty(typeof(DateTimeUtcConverter))]
        public DateTime DataNascimento
        {
            get; set;
        }

        [DynamoDBProperty]
        public string Documento
        {
            get; set;
        }

        [DynamoDBProperty]
        public string Nivel
        {
            get; set;
        }
        [DynamoDBProperty]
        public string Senha
        {
            get; set;
        }
    }
}