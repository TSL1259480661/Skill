using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Buff_Desc_buff_displayCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Buff_Desc_buff_display");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Buff_Desc_buff_display> dict = new Dictionary<int, Buff_Desc_buff_display>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Buff_Desc_buff_display> list = new List<Buff_Desc_buff_display>();
		
        
        public void Merge(object o)
        {
            Buff_Desc_buff_displayCategory s = o as Buff_Desc_buff_displayCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Buff_Desc_buff_display config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Buff_Desc_buff_display Get(int id)
        {
            this.dict.TryGetValue(id, out Buff_Desc_buff_display item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Buff_Desc_buff_display)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Buff_Desc_buff_display)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Buff_Desc_buff_display> GetAll()
        {
            return this.dict;
        }

        public Buff_Desc_buff_display GetOne()
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
	public partial class Buff_Desc_buff_display: ProtoObject, IConfig
	{
		/// <summary>buff特效id</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>buff特效优先级</summary>
		[ProtoMember(2)]
		public int buff_display_priority { get; set; }
		/// <summary>buff特效位置</summary>
		[ProtoMember(3)]
		public int buff_display_position { get; set; }

	}
}
