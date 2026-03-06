using GastuakApi.Modeloak;
using GastuakApi.Repositorioak;

namespace GastuakApi.Testak
{

    using MySqlX.XDevAPI;
    using NHibernate;
    using System.Collections.Generic;
    using Xunit;

    namespace GastuakApi.Tests
    {
        public class FamiliaMappingTests
        {

            [Fact]
            public void FamiliaEtaErabiltzaileak_RelazioaGordetzenDa()
            {
                /**
                var familiaRepo = new FamiliaRepository();
                var erabiltzaileRepo = new ErabiltzaileaRepository();

                var erabiltzailea = new Erabiltzailea
                {
                    Izena = "Jon",
                    Abizena = "Test"
                };

                erabiltzaileRepo.Add(erabiltzailea);

                var familia = new Familia
                {
                    Izena = "FamiliaRelazio",
                    Erabiltzaileak = new List<Erabiltzailea> { erabiltzailea }
                };

                familiaRepo.Add(familia);

                var lortutakoa = familiaRepo.Get(familia.Id);

                Assert.NotNull(lortutakoa);
                Assert.Single(lortutakoa.Erabiltzaileak);
                */
            }
        }
    }
}
