namespace GastuakApi.Modeloak
{
    public class Familia
    {
        public virtual int Id { get; set; }
        public virtual string Izena { get; set; }

        public virtual IList<Erabiltzailea> Erabiltzaileak { get; set; }

        public Familia()
        {
            Erabiltzaileak = new List<Erabiltzailea>();
        }

        public Familia(string izena, IList<Erabiltzailea> erabiltzailea) { 
            this.Izena = izena;
            this.Erabiltzaileak = erabiltzailea;
        }
    }
}
