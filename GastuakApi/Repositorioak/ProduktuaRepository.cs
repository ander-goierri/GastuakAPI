using GastuakApi.Modeloak;
using NHibernate;

namespace GastuakApi.Repositorioak
{
    public class ProduktuaRepository
    {
        private readonly NHibernate.ISession _session;

        public ProduktuaRepository(NHibernate.ISession session)
        {
            _session = session;
        }

        public ProduktuaRepository()
        {
        }

        /// <summary>
        ///  Obtiene una instancia de la entidad Produktua con el identificador especificado.
        /// </summary>
        /// <param name="id">El identificador único de la entidad Produktua que se va a recuperar.</param>
        /// <returns>La instancia de Produktua correspondiente al identificador especificado, o null si no se encuentra ninguna
        /// entidad con ese identificador.</returns>
        /// Virtual dauka erabiltzaileak metodo hau override egin dezan, adibidez, unit testing-eko mock-ak sortzeko.
        public virtual Produktua Get(int id)
        {
            return _session.Get<Produktua>(id);
        }

        public virtual void Save(Produktua product)
        {
            using var transaction = _session.BeginTransaction();
            _session.SaveOrUpdate(product);
            transaction.Commit();
        }
    }
}
