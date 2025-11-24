using FluentNHibernate.Mapping;
using GastuakApi.Modeloak;

namespace GastuakApi.Mapeoak
{
    public class ErabiltzaileaMap : ClassMap<Erabiltzailea>
    {
        public ErabiltzaileaMap()
        {
            Table("erabiltzailea");

            Id(x => x.Id).Column("id").GeneratedBy.Identity();

            Map(x => x.Izena).Column("izena");

            Map(x => x.SarreraRekurrentea).Column("sarrera_rekurrentea");

            Map(x => x.SoldataNagusia_id).Column("soldata_nagusia_id");

            HasManyToMany(x => x.Familiak)
                .Table("familia_erabiltzailea")
                .ParentKeyColumn("erabiltzailea_id")
                .ChildKeyColumn("familia_id")
                .Cascade.All()
                .Not.LazyLoad();

        }
    }
}
