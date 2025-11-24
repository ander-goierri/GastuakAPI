namespace GastuakApi.Modeloak
{
    public class Erabiltzailea
    {
        public virtual int Id { get; set; }
        public virtual string Izena { get; set; }

        public virtual float SarreraRekurrentea { get; set; }

        public virtual int SoldataNagusia_id { get; set; }

        public virtual IList<Familia> Familiak { get; set; }

        public Erabiltzailea()
        {
            Familiak = new List<Familia>();
        }
    }
}
