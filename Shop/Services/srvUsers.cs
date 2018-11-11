using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.Models;

namespace Shop.Services
{
    public class srvUsers
    {
        public Usuario logIn(Usuario oUser)
        {
            try
            {
                using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
                {
                    return bd.Usuario.Where(x => x.nombreUsuario == oUser.nombreUsuario.ToUpper() && x.contraseña == oUser.contraseña).First();
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}