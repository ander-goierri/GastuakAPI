using FluentNHibernate.Mapping;
using GastuakApi.Modeloak;

namespace GastuakApi.Mapeoak
{
    /// <summary>
    /// <see cref="Familia"/> entitatearen eta datu-baseko "familiak" taularen arteko
    /// Fluent NHibernate mapaketa definitzen du.
    /// </summary>
    /// <remarks>
    /// Many-to-many erlazioa definitzen du <see cref="Erabiltzailea"/> entitatearekin
    /// "erabiltzailea_familia" erlazio-taularen bidez.
    /// 
    /// <c>Inverse()</c> erabiltzeak adierazten du erlazioaren jabetza beste aldean
    /// (ErabiltzaileaMap-en) dagoela.
    /// </remarks>
    public class FamiliaMap : ClassMap<Familia>
    {
        public FamiliaMap()
        {
            Table("familiak");

            Id(x => x.Id).Column("id").GeneratedBy.Identity();

            Map(x => x.Izena).Column("izena");

            HasManyToMany(x => x.Erabiltzaileak)
                .Table("erabiltzailea_familia")
                .ParentKeyColumn("familia_id")
                .ChildKeyColumn("erabiltzailea_id")
                .Inverse()
                .LazyLoad()
                .Cascade.All();
        }
    }
}
