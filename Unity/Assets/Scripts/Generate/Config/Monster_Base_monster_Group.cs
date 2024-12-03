using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Monster_Base_monster_GroupCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Monster_Base_monster_Group");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Monster_Base_monster_Group> dict = new Dictionary<int, Monster_Base_monster_Group>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Monster_Base_monster_Group> list = new List<Monster_Base_monster_Group>();
		
        
        public void Merge(object o)
        {
            Monster_Base_monster_GroupCategory s = o as Monster_Base_monster_GroupCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Monster_Base_monster_Group config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Monster_Base_monster_Group Get(int id)
        {
            this.dict.TryGetValue(id, out Monster_Base_monster_Group item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Monster_Base_monster_Group)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Monster_Base_monster_Group)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Monster_Base_monster_Group> GetAll()
        {
            return this.dict;
        }

        public Monster_Base_monster_Group GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            var enumerator = this.dict.Values.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current; 
        }
    }


    [ProtoContract]
	public partial class Monster_Base_monster_Group: ProtoObject, IConfig
	{
		/// <summary>怪物组id</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>怪物类型5</summary>
		[ProtoMember(10)]
		public int[] monster_Group_monsterTypes { get; set; }
		/// <summary>怪物Id5</summary>
		[ProtoMember(11)]
		public int[] monster_Group_monsterIds { get; set; }

	}
}
