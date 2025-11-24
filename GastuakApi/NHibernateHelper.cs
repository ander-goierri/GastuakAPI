using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GastuakApi.Mapeoak;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace GastuakApi
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        public static ISessionFactory SessionFactory =>
            _sessionFactory ??= CreateSessionFactory();

        private static ISessionFactory CreateSessionFactory()
        {
            var config = Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                    .ConnectionString("Server=localhost;Port=3310;Database=gastuak;Uid=root;Pwd=;"))
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<ErabiltzaileaMap>();
                    m.FluentMappings.AddFromAssemblyOf<FamiliaMap>();
                })
                .BuildConfiguration();

            // CREA / ACTUALIZA LAS TABLAS AUTOMÁTICAMENTE
            new SchemaUpdate(config).Execute(false, true);

            // Si quieres recrear todo:
            // new SchemaExport(config).Create(false, true);

            return config.BuildSessionFactory();
        }
    }
}
