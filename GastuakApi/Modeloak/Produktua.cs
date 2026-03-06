namespace GastuakApi.Modeloak
{
    public class Produktua
    {
        public virtual int? Id { get; protected set; }
        public virtual string? Izena { get; set; }
        public virtual decimal? Prezioa { get; set; }

        public Produktua(int id, string izena, decimal prezioa)
        {
            this.Id = id;
            this.Izena = izena;
            this.Prezioa = prezioa;
        }

    }
}
