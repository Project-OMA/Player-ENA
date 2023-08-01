using MicelioUnity;
using MicelioEntity = MicelioUnity.Entity;

namespace ENA.Provenance
{
    public interface IEntity: IEntityFactory
    {
        string ID {get;}
        string Name {get;}
    }

    public static class IEntityExtensions
    {
        public static MicelioEntity ToEntity(this IEntity self)
        {
            string entityID = self.ID;
            if (string.IsNullOrEmpty(entityID))
                entityID = MicelioEntity.GenerateEntityID();

            return new MicelioEntity(entityID, self.Name);
        }
    }
}