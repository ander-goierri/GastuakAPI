using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GastuakApi.Mapeoak;
using GastuakApi.Modeloak;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace GastuakApi
{
    /// <summary>
    /// NHibernate-ren <see cref="ISessionFactory"/> sortu eta eskuratzeko laguntza klasea.
    /// </summary>
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        /// <summary>
        /// Aplikazio osoan erabiliko den <see cref="ISessionFactory"/> instantzia.
        /// </summary>
        public static ISessionFactory SessionFactory =>
            _sessionFactory ??= CreateSessionFactory();

        /// <summary>
        /// NHibernate konfigurazioa eraiki eta <see cref="ISessionFactory"/> sortzen du.
        /// </summary>
        /// <returns>Konfiguratutako <see cref="ISessionFactory"/>.</returns>
        private static ISessionFactory CreateSessionFactory()
        {
            var config = Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                    .ConnectionString("Server=localhost;Port=3310;Database=gastuak;Uid=root;Pwd=;"))
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<FamiliaMap>();
                })
                .ExposeConfiguration(cfg =>
                {
                    cfg.SetProperty("current_session_context_class", "async_local");
                })
                .BuildConfiguration();

            //dbBirSortu(config);
            //dbEguneratu(config);

            return config.BuildSessionFactory();
        }

        /// <summary>
        /// Datu-basearen eskema eguneratzen du (taulak/kolumnak sinkronizatu), datuak galdu gabe.
        /// </summary>
        /// <param name="config">NHibernate konfigurazioa.</param>
        public static void dbEguneratu(NHibernate.Cfg.Configuration config)
        {
            SchemaUpdate schemaUpdate = new SchemaUpdate(config);
            schemaUpdate.Execute(false, true);
        }

        /// <summary>
        /// Datu-basearen eskema berriz sortzen du (DROP + CREATE) eta hasierako datuak txertatzen ditu.
        /// </summary>
        /// <param name="config">NHibernate konfigurazioa.</param>
        /// <remarks>
        /// Kontuz: honek taulak ezabatu eta berriz sortzen ditu, beraz datuak galdu daitezke.
        /// </remarks>
        public static void dbBirSortu(NHibernate.Cfg.Configuration config)
        {
            var schemaExport = new SchemaExport(config);
            schemaExport.Drop(true, true);    // Ezabatu
            schemaExport.Create(true, true);  // Sortu
            datuakSortu(config);
        }

        /// <summary>
        /// Hasierako datuak txertatzen ditu.
        /// </summary>
        /// <param name="config">NHibernate konfigurazioa.</param>
        public static void datuakSortu(NHibernate.Cfg.Configuration config)
        {
            using (var session = config.BuildSessionFactory().OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Save(new Familia { Izena = "Tolosa" });
                session.Save(new Familia { Izena = "Sebastian" });
                session.Save(new Familia { Izena = "Toledo" });

                transaction.Commit();
            }
        }
    }
}
