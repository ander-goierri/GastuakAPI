using GastuakApi.Modeloak;
using NHibernate;

namespace GastuakApi.Repositorioak
{
    /// <summary>
    /// <see cref="Familia"/> entitatearen datu-baseko CRUD eragiketak egiteko repositorioa.
    /// </summary>
    public class FamiliaRepository
    {
        private readonly NHibernate.ISession _session;

        /// <summary>
        /// Repositorioa sortzen du eta uneko NHibernate saioa eskuratzen du.
        /// </summary>
        /// <param name="sessionFactory">NHibernate saioak sortzeko fabrikatzailea.</param>
        public FamiliaRepository(ISessionFactory sessionFactory)
        {
            _session = sessionFactory.GetCurrentSession();
        }
        public FamiliaRepository()
        {
            _session = null;
        }

        /// <summary>
        /// Familia berri bat gordetzen du.
        /// </summary>
        /// <param name="familia">Gorde beharreko familia.</param>
        public void Add(Familia familia)
        {
            using var tx = _session.BeginTransaction();

            _session.Save(familia);

            tx.Commit();
        }

        /// <summary>
        /// ID baten arabera familia bat lortzen du.
        /// </summary>
        /// <param name="id">Bilatu nahi den familiaren identifikatzailea.</param>
        /// <param name="eager">Une honetan ez da erabiltzen (etorkizuneko eager loading-erako pentsatua).</param>
        /// <returns>Familia aurkitzen bada, objektua; bestela <c>null</c>.</returns>
        public Familia? Get(int id, bool eager = false)
        {
            var query = _session.Query<Familia>()
                .Where(x => x.Id == id);

            var familia = query.SingleOrDefault();
            return familia;
        }

        /// <summary>
        /// Familia guztiak lortzen ditu.
        /// </summary>
        /// <returns>Familia guztien zerrenda.</returns>
        public IList<Familia> GetAll()
        {
            return _session.Query<Familia>().ToList();
        }

        /// <summary>
        /// Familia baten datuak eguneratzen ditu.
        /// </summary>
        /// <param name="familia">Eguneratu beharreko familia.</param>
        public void Update(Familia familia)
        {
            using var tx = _session.BeginTransaction();

            _session.Update(familia);

            tx.Commit();
        }

        /// <summary>
        /// Familia bat ezabatzen du.
        /// </summary>
        /// <param name="familia">Ezabatu beharreko familia.</param>
        public void Delete(Familia familia)
        {
            using var tx = _session.BeginTransaction();

            _session.Delete(familia);

            tx.Commit();
        }
    }
}
