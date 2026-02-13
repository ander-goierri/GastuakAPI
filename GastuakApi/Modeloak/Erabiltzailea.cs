namespace GastuakApi.Modeloak
{
    /// <summary>
    /// Sistemako erabiltzailea adierazten duen entitatea.
    /// </summary>
    /// <remarks>
    /// NHibernate-k mapatutako klasea da. Propietateak <c>virtual</c> dira
    /// lazy loading eta proxy mekanismoak ahalbidetzeko.
    /// </remarks>
    public class Erabiltzailea
    {
        public virtual int Id { get; set; }

        public virtual string Izena { get; set; }

        public virtual string Abizena { get; set; }

        public virtual float SarreraRekurrentea { get; set; }

        public virtual int KontuNagusia_id { get; set; }

        /// <summary>
        /// Erabiltzailea lotuta dagoen familien zerrenda (many-to-many erlazioa).
        /// </summary>
        public virtual IList<Familia> Familiak { get; set; } = new List<Familia>();

        /// <summary>
        /// NHibernate-rentzat beharrezkoa den eraikitzaile hutsa.
        /// </summary>
        public Erabiltzailea()
        {
        }

        /// <summary>
        /// Erabiltzaile berri bat sortzeko eraikitzailea.
        /// </summary>
        /// <param name="izena">Erabiltzailearen izena.</param>
        /// <param name="abizena">Erabiltzailearen abizena.</param>
        /// <param name="sarrera">Erabiltzailearen sarrera errekurrentea.</param>
        public Erabiltzailea(string izena, string abizena, float sarrera)
        {
            this.Izena = izena;
            this.Abizena = abizena;
            this.SarreraRekurrentea = sarrera;
        }
    }
}
