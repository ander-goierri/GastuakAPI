using GastuakApi.Modeloak;
using NHibernate;

namespace GastuakApi.Repositorioak
{ 
    public class FamiliaRepository
    {
        private readonly ISessionFactory _sessionFactory;

        public FamiliaRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Add(Familia familia)
        {
            using var session = _sessionFactory.OpenSession();
            using var tx = session.BeginTransaction();

            session.Save(familia);

            tx.Commit();
        }

        public Familia? Get(int id, bool eager = false)
        {
            using var session = _sessionFactory.OpenSession();
            Familia f = session.Get<Familia>(id);
            /*
            if (eager) {
                NHibernateUtil.Initialize(f.Erabiltzaileak);
            }
            */
            return f;
        }

        public IList<Familia> GetAll()
        {
            using var session = _sessionFactory.OpenSession();
            return session.Query<Familia>().ToList();
        }

        public void Update(Familia familia)
        {
            using var session = _sessionFactory.OpenSession();
            using var tx = session.BeginTransaction();

            session.Update(familia);

            tx.Commit();
        }

        public void Delete(Familia familia)
        {
            using var session = _sessionFactory.OpenSession();
            using var tx = session.BeginTransaction();

            session.Delete(familia);

            tx.Commit();
        }

    }

}
