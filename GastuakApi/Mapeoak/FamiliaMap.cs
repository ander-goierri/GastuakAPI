using FluentNHibernate.Mapping;
using GastuakApi.Modeloak;

namespace GastuakApi.Mapeoak
{
    public class FamiliaMap : ClassMap<Familia>
    {
        public FamiliaMap()
        {
            Table("familiak");

            Id(x => x.Id).Column("id").GeneratedBy.Identity();

            Map(x => x.Izena).Column("izena");

            HasManyToMany(x => x.Erabiltzaileak)
                .Table("familia_erabiltzailea")
                .ParentKeyColumn("familia_id")
                .ChildKeyColumn("erabiltzailea_id")
                .Cascade.All()
                .Inverse() // garrantzitsua
                .Not.LazyLoad(); // Esto sería eager loading
                
        }
    }
}
