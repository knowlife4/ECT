using System.Collections.Generic;

namespace ECT
{
    public class ECTEntityGroup<MyEntity> where MyEntity : IEntity
    {

        public MyEntity[] Entities { get; private set; }
        public List<MyEntity> EntitiesList { get; private set; } = new();

        void UpdateArray () => Entities = EntitiesList.ToArray();

        public void Tag (MyEntity entity)
        {
            if(EntitiesList.Contains(entity) || entity == null) return;
            EntitiesList.Add(entity);
            UpdateArray();
        }

        public void Untag (MyEntity entity)
        {
            if(!EntitiesList.Contains(entity)) return;
            EntitiesList.Remove(entity);
            UpdateArray();
        }
    }
}