using GastuakApi.Modeloak;

namespace GastuakApi.Testak
{
    using System;
    using Xunit;

    public class CarteraTests
    {
        [Fact]
        public void Eraikitzailea_Saldorik_sartu_gabe_Defektuz_ZeroDa()
        {
            var cartera = new Cartera();
            Assert.Equal(0, cartera.Saldo);
        }

        [Fact]
        public void Eraikitzaileak_TipoDesegokia_JasotzenDuenean_Saldoa_ZeroDa()
        {
            String num = "-10";
            int numInt = int.Parse(num);
            var cartera = new Cartera(numInt); // int da baina baliogabea
            Assert.Equal(0, cartera.Saldo);
        }

        [Fact]
        public void Eraikitzaileak_SaldoNegatiboa_JasotzenDuenean_Saldoa_ZeroDa()
        {
            var cartera = new Cartera(-50);
            Assert.Equal(0, cartera.Saldo);
        }

        [Fact]
        public void Eraikitzaileak_SaldoPositiboa_JasotzenDuenean_OndoEsleitzenDa()
        {
            var cartera = new Cartera(100);
            Assert.Equal(100, cartera.Saldo);
        }

        [Fact]
        public void Dirua_Sartzean_Saldoa_GehitzenDa()
        {
            var cartera = new Cartera(100);

            cartera.Ingresar(50);

            Assert.Equal(150, cartera.Saldo);
        }

        [Fact]
        public void Dirua_Sartzean_Ingresua_Negatiboa_Bada_Errorea_emango_du()
        {
            var cartera = new Cartera(100);
            
            Assert.Throws<Exception>(() => cartera.Gastar(-50));

        }

        [Fact]
        public void Dirua_Gastatzean_Saldoa_negatiboa_jarrita_Errorea_emango_du()
        {
            var cartera = new Cartera(100);

            Assert.Throws<Exception>(() => cartera.Ingresar(-50));

        }

        [Fact]
        public void Dirua_Gastatzean_Saldoa_GutxitzenDa()
        {
            var cartera = new Cartera(100);

            cartera.Gastar(40);

            Assert.Equal(60, cartera.Saldo);
        }

        [Fact]
        public void Saldoa_Baino_Gehiago_Gastatzean_Salbuespena_JaustenDa()
        {
            var cartera = new Cartera(50);

            Assert.Throws<SaldoInsuficienteException>(() => cartera.Gastar(100));
        }
    }

}
