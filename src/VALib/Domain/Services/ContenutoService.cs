using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Repositories.Contenuti;
using VALib.Domain.Entities.UI;
using VALib.Domain.Repositories.UI;
using VALib.Configuration;

namespace VALib.Domain.Services
{
    public class ContenutoService
    {
        public ContenutoService()
        {

        }

        public Notizia CreaNotizia(CategoriaNotizia categoria, int immagineID, DateTime data, string titolo_IT, 
            string titolo_EN, string titoloBreve_IT, string titoloBreve_EN, string abstract_IT, 
            string abstract_EN, string testo_IT, string testo_EN)
        {
            Notizia notizia = null;

            if (string.IsNullOrWhiteSpace(titolo_IT))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "titolo_IT");

         
            if (categoria == null)
                throw new ArgumentNullException("L'argomento non può essere null, vuoto o contenere solo spazi.", "categoria");

            notizia = new Notizia();
            notizia.Pubblicata = false;
            notizia.Stato = StatoNotiziaEnum.Bozza;
            notizia.ID = 0;
            notizia.DataInserimento = DateTime.Now;
            notizia.DataUltimaModifica = notizia.DataInserimento;

            notizia.Data = data;
            notizia.Titolo_IT = titolo_IT;
            notizia.Titolo_EN = titolo_EN ?? "";
            notizia.TitoloBreve_IT = titoloBreve_IT ?? "";
            notizia.TitoloBreve_EN = titoloBreve_EN ?? "";
            notizia.Abstract_IT = abstract_IT ?? "";
            notizia.Abstract_EN = abstract_EN ?? "";
            notizia.Testo_IT = testo_IT ?? "";
            notizia.Testo_EN = testo_EN ?? "";
            notizia.Categoria = categoria;
            notizia.ImmagineID = immagineID;
         
            return notizia;
        }

        public int SalvaNotizia(Notizia notizia)
        {
            int id = 0;

            if (notizia == null)
                throw new ArgumentNullException("notizia");

            notizia.DataUltimaModifica = DateTime.Now;

            id = NotiziaRepository.Instance.SalvaNotizia(notizia);

            return id;
        }

        public List<string> NotiziaPubblicabile(Notizia notizia)
        {
            List<string> result = new List<string>(); ;

            if (string.IsNullOrWhiteSpace(notizia.Titolo_EN))
                result.Add("Titolo EN è obbligatorio");
            if (string.IsNullOrWhiteSpace(notizia.Titolo_IT))
                result.Add("Titolo IT è obbligatorio");
            if (string.IsNullOrWhiteSpace(notizia.TitoloBreve_EN))
                result.Add("Titolo breve EN è obbligatorio");
            if (string.IsNullOrWhiteSpace(notizia.TitoloBreve_IT))
                result.Add("Titolo breve IT è obbligatorio");
            if (string.IsNullOrWhiteSpace(notizia.Abstract_EN))
                result.Add("Abstract EN è obbligatorio");
            if (string.IsNullOrWhiteSpace(notizia.Abstract_IT))
                result.Add("Abstract IT è obbligatorio");
            if (string.IsNullOrWhiteSpace(notizia.Testo_EN))
                result.Add("Testo EN è obbligatorio");
            if (string.IsNullOrWhiteSpace(notizia.Testo_IT))
                result.Add("Testo IT è obbligatorio");

            return result;
        }

        public int DefaultImmagineID
        {
            get { return Settings.DefaultImmagineID; }
        }

        public OggettoCarosello CreaOggettoCarosello(int contenutoID, ContenutoOggettoCaroselloTipo tipoContenuto, DateTime data, string nome_IT, string nome_EN, string descrizione_IT, string descrizione_EN)
        {
            OggettoCarosello oggettoCarosello = null;

            if (string.IsNullOrWhiteSpace(nome_IT))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "nome_IT");

            if (string.IsNullOrWhiteSpace(nome_EN))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "nome_EN");

            if (string.IsNullOrWhiteSpace(descrizione_IT))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "descrizione_IT");

            if (string.IsNullOrWhiteSpace(descrizione_EN))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "descrizione_EN");

           
            oggettoCarosello = new OggettoCarosello();
            oggettoCarosello.Pubblicato = false;
            oggettoCarosello.ID = 0;
            oggettoCarosello.TipoContenuto = tipoContenuto;
            oggettoCarosello.DataInserimento = DateTime.Now;
            oggettoCarosello.DataUltimaModifica = oggettoCarosello.DataInserimento;

            oggettoCarosello.Data = data;
            oggettoCarosello.Nome_IT = nome_IT;
            oggettoCarosello.Nome_EN = nome_EN;
            oggettoCarosello.Descrizione_IT = descrizione_IT;
            oggettoCarosello.Descrizione_EN = descrizione_EN;
          
            oggettoCarosello.ContenutoID = contenutoID;

            return oggettoCarosello;
        }

        public int SalvaOggettoCarosello(OggettoCarosello oggettoCarosello)
        {
            int id = 0;

            if (oggettoCarosello == null)
                throw new ArgumentNullException("oggettoCarosello");

            oggettoCarosello.DataUltimaModifica = DateTime.Now;

            id = OggettoCaroselloRepository.Instance.SalvaOggettoCarosello(oggettoCarosello);

            return id;
        }

        public Immagine CreaImmagineMaster(string nome_IT, string nome_EN, string nomeFile)
        {
            Immagine immagine = null;

            if (string.IsNullOrWhiteSpace(nome_IT))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "nome_IT");

            if (string.IsNullOrWhiteSpace(nome_EN))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "nome_EN");

            if (string.IsNullOrWhiteSpace(nomeFile))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "nomeFile");

            immagine = new Immagine();
            immagine.FormatoImmagine = FormatoImmagineRepository.Instance.RecuperaFormatoImmagine((int)FormatoImmagineEnum.Master);
            immagine.DataInserimento = DateTime.Now;
            immagine.DataUltimaModifica = immagine.DataInserimento;
            immagine.ImmagineMasterID = 0;

            immagine.Nome_IT = nome_IT;
            immagine.Nome_EN = nome_EN;
            immagine.NomeFile = nomeFile;

            return immagine;
        }

        public Immagine CreaImmagine(Immagine immagineMaster, FormatoImmagine formato, string nomeFile)
        {
            Immagine immagine = null;

            if (immagineMaster == null)
                throw new ArgumentNullException("immagineMaster");

            if (formato == null)
                throw new ArgumentNullException("formato");

            immagine = new Immagine();
            immagine.ImmagineMasterID = immagineMaster.ID;
            immagine.FormatoImmagine = formato;
            immagine.DataInserimento = DateTime.Now;
            immagine.DataUltimaModifica = immagine.DataInserimento;

            immagine.Nome_IT = immagineMaster.Nome_IT;
            immagine.Nome_EN = immagineMaster.Nome_EN;
            immagine.NomeFile = nomeFile;

            return immagine;
        }

        public int SalvaImmagine(Immagine immagine)
        {
            int id = 0;

            if (immagine == null)
                throw new ArgumentNullException("immagine");

          
                immagine.DataUltimaModifica = DateTime.Now;

                id = ImmagineRepository.Instance.SalvaImmagine(immagine);
           

            return id;
        }

        public DocumentoPortale CreaDocumentoPortale(string nome_IT, string nome_EN)
        {
            DocumentoPortale documentoPortale = null;

            if (string.IsNullOrWhiteSpace(nome_IT))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "nome_IT");

            if (string.IsNullOrWhiteSpace(nome_EN))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "nome_EN");

            documentoPortale = new DocumentoPortale();
            documentoPortale.DataInserimento = DateTime.Now;
            documentoPortale.DataUltimaModifica = documentoPortale.DataInserimento;
            documentoPortale.NomeFileOriginale = "-";
            documentoPortale.TipoFile = null;
            documentoPortale.Dimensione = 0;

            documentoPortale.Nome_IT = nome_IT;
            documentoPortale.Nome_EN = nome_EN;

            return documentoPortale;
        }

        public int SalvaDocumentoPortale(DocumentoPortale documentoPortale)
        {
            int id = 0;

            if (documentoPortale == null)
                throw new ArgumentNullException("documentoPortale");

            documentoPortale.DataUltimaModifica = DateTime.Now;

            id = DocumentoPortaleRepository.Instance.SalvaDocumentoPortale(documentoPortale);

            return id;
        }

        public Widget CreaWidget(string nome, TipoWidget tipo)
        {
            Widget widget = null;

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "nome");

            if (tipo == TipoWidget.NonDefinito)
                throw new ArgumentException("Il tipo non può essere 'Non Definito'", "tipo");

            widget = new Widget();
            widget.DataInserimento = DateTime.Now;
            widget.DataUltimaModifica = widget.DataInserimento;
            widget.MostraTitolo = true;
            widget.Tipo = tipo;
            
            widget.Nome_IT = nome;

            return widget;
        }

        public int SalvaWidget(Widget widget)
        {
            int id = 0;

            if (widget == null)
                throw new ArgumentNullException("widget");

            widget.DataUltimaModifica = DateTime.Now;

            id = WidgetRepository.Instance.SalvaWidget(widget);

            return id;
        }

        public DatoAmbientaleHome CreaDatoAmbientaleHome(int immagineID, string titolo_IT, string titolo_EN, string link)
        {
            DatoAmbientaleHome datoAmbientale = null;

            if (string.IsNullOrWhiteSpace(titolo_IT))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "titolo_IT");

            if (string.IsNullOrWhiteSpace(titolo_EN))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "titolo_EN");

            datoAmbientale = new DatoAmbientaleHome();
            datoAmbientale.Pubblicato = false;
            datoAmbientale.ID = 0;
            datoAmbientale.DataInserimento = DateTime.Now;
            datoAmbientale.DataUltimaModifica = datoAmbientale.DataInserimento;

            datoAmbientale.Titolo_IT = titolo_IT;
            datoAmbientale.Titolo_EN = titolo_EN;
            datoAmbientale.Link = link;
            datoAmbientale.ImmagineID = immagineID;

            return datoAmbientale;
        }

        public int SalvaDatoAmbientaleHome(DatoAmbientaleHome datoAmbientaleHome)
        {
            int id = 0;

            if (datoAmbientaleHome == null)
                throw new ArgumentNullException("datoAmbientaleHome");

            datoAmbientaleHome.DataUltimaModifica = DateTime.Now;

            id = DatoAmbientaleHomeRepository.Instance.SalvaDatoAmbientaleHome(datoAmbientaleHome);

            return id;
        }

        public PaginaStatica CreaPaginaStatica(VoceMenu voceMenu, string testo_IT, string testo_EN)
        {
            PaginaStatica paginaStatica = null;

            if (string.IsNullOrWhiteSpace(testo_IT))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "testo_IT");

            if (string.IsNullOrWhiteSpace(testo_EN))
                throw new ArgumentException("L'argomento non può essere null, vuoto o contenere solo spazi.", "testo_EN");

            if (voceMenu == null)
                throw new ArgumentNullException("L'argomento non può essere null, vuoto o contenere solo spazi.", "voceMenu");

            paginaStatica = new PaginaStatica();
            paginaStatica.ID = 0;
            paginaStatica.DataInserimento = DateTime.Now;
            paginaStatica.DataUltimaModifica = paginaStatica.DataInserimento;

            paginaStatica.VoceMenu = voceMenu;
            paginaStatica.Testo_IT = testo_IT;
            paginaStatica.Testo_EN = testo_EN;

            return paginaStatica;
        }

        public int SalvaPaginaStatica(PaginaStatica paginaStatica)
        {
            int id = 0;

            if (paginaStatica == null)
                throw new ArgumentNullException("paginaStatica");

            paginaStatica.DataUltimaModifica = DateTime.Now;

            id = PaginaStaticaRepository.Instance.SalvaPaginaStatica(paginaStatica);

            return id;
        }

        public void SalvaVariabile(string chiave, string valore)
        {
            if (string.IsNullOrWhiteSpace(chiave))
                throw new ArgumentException("chiave null or whitespace");

            
            VariabileRepository.Instance.SalvaVariabile(chiave, valore);
        }

        public void EliminaCache()
        {
           
           WidgetRepository.Instance.EliminaCache();
        }

    }
}
