using FluentNHibernate.Mapping;

namespace GastuakApi.Mapeoak
{
    public class ProduktuaMap: ClassMap<Modeloak.Produktua>
    {
        public ProduktuaMap()
        {
            Table("produktuak");

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Izena)
                .Not.Nullable()
                .Length(100);

            Map(x => x.Prezioa)
                .Not.Nullable();
        }

    }
}
