namespace GastuakApi.Modeloak
{
    using System;

    public class SaldoInsuficienteException : Exception
    {
        public SaldoInsuficienteException(string message) : base(message)
        {
        }
    }

    public class Cartera
    {
        public int Saldo { get; private set; }

        public Cartera(int saldoInicial = 0)
        {
            if (saldoInicial > 0)
                Saldo = saldoInicial;
            else
                Saldo = 0;
        }

        public void Gastar(int cantidad)
        {

            if (cantidad < 0)
            {
                throw new Exception("Ingresatutako diru kantitatea ezin da negatiboa izan");
            }

            if (Saldo < cantidad)
            {
                throw new SaldoInsuficienteException(
                    $"No tienes dinero suficiente. Saldo actual: {Saldo}");
            }

            Saldo -= cantidad;
        }

        public void Ingresar(int cantidad)
        {
            if(cantidad < 0)
            {
                throw new Exception("Ingresatutako diru kantitatea ezin da negatiboa izan");
            }
            Saldo += cantidad;
        }
    }
}
