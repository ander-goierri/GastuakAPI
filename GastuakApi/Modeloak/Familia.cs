using System.Text.Json.Serialization;

namespace GastuakApi.Modeloak
{
    /// <summary>
    /// Familia bat adierazten duen entitatea.
    /// </summary>
    /// <remarks>
    /// NHibernate-k mapatutako klasea da. Propietateak <c>virtual</c> dira
    /// lazy loading eta proxy mekanismoak ahalbidetzeko.
    /// </remarks>
    public class Familia
    {
        public virtual int Id { get; set; }

        public virtual string Izena { get; set; }

        /// <summary>
        /// Familiari lotutako erabiltzaileen zerrenda (many-to-many erlazioa).
        /// </summary>
        public virtual IList<Erabiltzailea> Erabiltzaileak { get; set; } = new List<Erabiltzailea>();

        /// <summary>
        /// NHibernate-rentzat beharrezkoa den eraikitzaile hutsa.
        /// </summary>
        public Familia()
        {
        }

        /// <summary>
        /// Familia berri bat sortzeko eraikitzailea.
        /// </summary>
        /// <param name="izena">Familiaren izena.</param>
        public Familia(string izena)
        {
            this.Izena = izena;
        }
    }
}
