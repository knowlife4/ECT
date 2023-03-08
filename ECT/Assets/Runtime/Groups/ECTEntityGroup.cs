using System.Collections.Generic;

namespace ECT
{
    public class ECTEntityGroup<TEntity> where TEntity : IEntity
    {
        public TEntity[] Entities { get; private set; }
        public List<TEntity> EntitiesList { get; private set; } = new();

        void UpdateArray () => Entities = EntitiesList.ToArray();

        public void Tag (TEntity entity)
        {
            if(EntitiesList.Contains(entity) || entity == null) return;
            EntitiesList.Add(entity);
            UpdateArray();
        }

        public void UnTag (TEntity entity)
        {
            if(!EntitiesList.Contains(entity)) return;
            EntitiesList.Remove(entity);
            UpdateArray();
        }
    }
}