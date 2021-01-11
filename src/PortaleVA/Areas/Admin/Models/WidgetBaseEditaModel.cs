using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VALib.Domain.Entities.UI;

namespace VAPortale.Areas.Admin.Models
{
    public class WidgetBaseEditaModel
    {
        public int ID { get; set; }

        public Widget Widget { get; set; }

    }
}