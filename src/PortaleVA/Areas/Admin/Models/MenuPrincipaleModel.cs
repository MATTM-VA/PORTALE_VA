using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Support;
using VALib.Domain.Entities.Membership;

namespace VAPortale.Areas.Admin.Models
{
    public class MenuPrincipaleModel
    {
        public MenuPrincipaleModel()
        {
            ListaLink = new List<LinkItem>();
        }

        public List<LinkItem> ListaLink { get; set; }
    }
}