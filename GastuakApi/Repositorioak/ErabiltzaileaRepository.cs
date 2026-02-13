using GastuakApi.Modeloak;
using NHibernate;

namespace GastuakApi.Repositorioak
{
    /// <summary>
    /// <see cref="Erabiltzailea"/> entitatearen datu-baseko CRUD eragiketak egiteko repositorioa.
    /// </summary>
    public class ErabiltzaileaRepository
    {
        private readonly NHibernate.ISession _session;

        /// <summary>
        /// Repositorioa sortzen du eta uneko NHibernate saioa eskuratzen du.
        /// </summary>
        /// <param name="sessionFactory">NHibernate saioak sortzeko fabrikatzailea.</param>
        public ErabiltzaileaRepository(ISessionFactory sessionFactory)
        {
            _session = sessionFactory.GetCurrentSession();
        }

        /// <summary>
        /// Erabiltzaile berri bat gordetzen du.
        /// </summary>
        /// <param name="erabiltzailea">Gorde beharreko erabiltzailea.</param>
        public void Add(Erabiltzailea erabiltzailea)
        {
            using var tx = _session.BeginTransaction();

            _session.Save(erabiltzailea);

            tx.Commit();
        }

        /// <summary>
        /// ID baten arabera erabiltzaile bat lortzen du.
        /// </summary>
        /// <param name="id">Bilatu nahi den erabiltzailearen identifikatzailea.</param>
        /// <param name="eager">Une honetan ez da erabiltzen (etorkizuneko eager loading-erako pentsatua).</param>
        /// <returns>Erabiltzailea aurkitzen bada, objektua; bestela <c>null</c>.</returns>
        public Erabiltzailea? Get(int id, bool eager = false)
        {
            var query = _session.Query<Erabiltzailea>()
                .Where(x => x.Id == id);

            var erabiltzailea = query.SingleOrDefault();
            return erabiltzailea;
        }

        /// <summary>
        /// ID zerrenda baten arabera erabiltzaileak lortzen ditu.
        /// </summary>
        /// <param name="ids">Bilatu beharreko erabiltzaileen identifikatzaileen zerrenda.</param>
        /// <returns>Bat datozen erabiltzaileen zerrenda.</returns>
        public List<Erabiltzailea> GetByIds(List<int> ids)
        {
            return _session.Query<Erabiltzailea>()
                .Where(x => ids.Contains(x.Id))
                .ToList();
        }

        /// <summary>
        /// Erabiltzaile guztiak lortzen ditu.
        /// </summary>
        /// <returns>Erabiltzaile guztien zerrenda.</returns>
        public IList<Erabiltzailea> GetAll()
        {
            return _session.Query<Erabiltzailea>().ToList();
        }

        /// <summary>
        /// Erabiltzaile baten datuak eguneratzen ditu.
        /// </summary>
        /// <param name="erabiltzailea">Eguneratu beharreko erabiltzailea.</param>
        public void Update(Erabiltzailea erabiltzailea)
        {
            using var tx = _session.BeginTransaction();

            _session.Update(erabiltzailea);

            tx.Commit();
        }

        /// <summary>
        /// Erabiltzaile bat ezabatzen du.
        /// </summary>
        /// <param name="erabiltzailea">Ezabatu beharreko erabiltzailea.</param>
        public void Delete(Erabiltzailea erabiltzailea)
        {
            using var tx = _session.BeginTransaction();

            _session.Delete(erabiltzailea);

            tx.Commit();
        }
    }
}
