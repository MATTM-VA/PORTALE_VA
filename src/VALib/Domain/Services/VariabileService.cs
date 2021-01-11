using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Repositories.UI;
using VALib.Domain.Entities.UI;

namespace VALib.Domain.Services
{
    public static class VariabileService
    {
        public static string GetValore(string chiave)
        {
            string result = chiave;

            Variabile variabile = null;

            variabile = VariabileRepository.Instance.RecuperaVariabile(chiave);

            if (variabile != null)
                result = variabile.Valore;
            else
                throw new KeyNotFoundException(chiave);

            return result;
        }
    }
}
