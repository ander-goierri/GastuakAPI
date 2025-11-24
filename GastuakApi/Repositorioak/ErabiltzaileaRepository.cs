using GastuakApi.Modeloak;
using NHibernate;


namespace GastuakApi.Repositorioak
{
    public class ErabiltzaileaRepostory
    {
        private readonly ISessionFactory _sessionFactory;

        public ErabiltzaileaRepostory(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

    }
}

