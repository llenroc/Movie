using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Event.Bus.Entities
{
    /// <summary>
    /// This type of event is used to notify just before creation of an Entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [Serializable]
    public class EntityCreatingEventData<TEntity> : EntityChangingEventData<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entity">The entity which is being created</param>
        public EntityCreatingEventData(TEntity entity)
            : base(entity)
        {

        }
    }
}
