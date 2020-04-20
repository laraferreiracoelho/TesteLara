using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webTeste02.Models
{
    public class User
    {
            public User()
            {
                id = System.Guid.NewGuid().ToString();
            }

            

        public string id
        {
            get; set;
        }
        public string email
            {
                get; set;
            }
           
            public string Nome
            {
                get; set;
            }

          
            public DateTime DataNascimento
            {
                get; set;
            }

     
            public string Documento
            {
                get; set;
            }

      
            public string Nivel
            {
                get; set;
            }
         
            public string Senha
            {
                get; set;
            }
        }
}
