namespace GastuakApi.Modeloak
{
    public class Familia
    {
        public virtual int Id { get; set; }
        public virtual string Izena { get; set; }

        public Familia()
        {
        }

        public Familia(string izena) { 
            this.Izena = izena;
        }
    }
}
