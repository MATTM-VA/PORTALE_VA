using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VAPortale.Filters;
using VAPortale.Models;
using VALib.Domain.Entities.UI;
using VALib.Domain.Repositories.UI;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Repositories.Contenuti;
using VALib.Helpers;
using VAPortale.Code;
using ElogToolkit;
using VALib.Domain.Services;
using VAPortale.Models.Support;
using Newtonsoft.Json;
using VALib.Domain.Report;

namespace VAPortale.Controllers
{
    [LanguageAttribute]
    public class ProvvedimentiRegionaliController : Controller
    {
        //
        // GET: /ProvvedimentiRegionali/

        public ActionResult ProvvedimentiElenco(ProcedureProvvedimentiElencoModel model, Int32 proceduraID = 0, Int32 TipologiaID = 0)
        {
            VoceMenu voce = VoceMenuRepository.Instance.RecuperaVoceMenu("ProvvedimentiElenco");

            model.VoceMenu = voce;
            model.TipoProvvedimento = null;
            
            DateTime? dataDa = null;
            DateTime? dataA = null;

            dataDa = Parse.ToDateTimeOrNull(model.DataDa, "dd-MM-yyyy");
            dataA = Parse.ToDateTimeOrNull(model.DataA, "dd-MM-yyyy");

            int totale = 0;

            model.ProcedureSelectList = ModelUtils.CreaProceduraSelectList(MacroTipoOggettoEnum.Aia, true);
            model.CategorieInstallazioneSelectList = ModelUtils.CreaCategoriaSelectList(true);

            model.Risorse = ProvvedimentoRepository.Instance.RecuperaProvvedimentiRegionali(CultureHelper.GetCurrentCultureShortName(),
                            model.Testo, dataDa, dataA, 
                            proceduraID,
                            TipologiaID,
                            model.IndiceInizio,
                            model.IndiceInizio + model.DimensionePagina,
                            out totale);

            model.TotaleRisultati = totale;
            
            return View("ProvvedimentiElenco", model); 

        }

    }
}
