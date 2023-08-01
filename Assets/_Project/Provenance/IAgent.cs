using MicelioUnity;
using UnityEngine;

namespace ENA.Provenance
{
    public interface IAgent: IAgentFactory
    {
        string ID {get;}
        string Name {get;}
        string Type {get;}
    }

    public static class IAgentExtensions
    {
        public static Agent ToAgent(this IAgent self)
        {
            return new(self.ID, self.Name, self.Type);
        }
    }
}