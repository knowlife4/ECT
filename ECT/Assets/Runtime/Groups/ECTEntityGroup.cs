using System.Collections.Generic;

namespace ECT
{
    public class ECTEntityGroup<TEntity> where TEntity : IEntity
    {
        public TEntity[] Entities { get; private set; }

        List<TEntity> entitiesList = new();

        void UpdateArray() => Entities = entitiesList.ToArray();

        public void Tag(TEntity entity)
        {
            if (entity == null || entitiesList.Contains(entity)) return;
            entitiesList.Add(entity);
            UpdateArray();
        }

        public void UnTag(TEntity entity)
        {
            if (!entitiesList.Contains(entity)) return;
            entitiesList.Remove(entity);
            UpdateArray();
        }
    }
}