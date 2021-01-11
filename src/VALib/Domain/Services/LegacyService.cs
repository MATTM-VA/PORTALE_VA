using VALib.Configuration;
using VALib.Domain.Repositories.Contenuti;

namespace VALib.Domain.Services
{
    public static class LegacyService
    {
        public static int GetOggettoVasID(int id)
        {
            return id + Settings.SumOggettoID;
        }

        public static int GetDocumentoVasID(int id)
        {
            return id + Settings.SumDocumentoID;
        }

        public static int GetUltimoOggettoProceduraID(int id)
        {
            return OggettoRepository.Instance.RecuperaOggettoProceduraIDCorrente(id) ?? 0;
        }
    }
}
