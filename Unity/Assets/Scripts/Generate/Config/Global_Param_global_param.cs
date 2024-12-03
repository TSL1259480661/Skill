using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Global_Param_global_paramCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Global_Param_global_param");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Global_Param_global_param> dict = new Dictionary<int, Global_Param_global_param>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Global_Param_global_param> list = new List<Global_Param_global_param>();
		
        
        public void Merge(object o)
        {
            Global_Param_global_paramCategory s = o as Global_Param_global_paramCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Global_Param_global_param config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Global_Param_global_param Get(int id)
        {
            this.dict.TryGetValue(id, out Global_Param_global_param item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Global_Param_global_param)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Global_Param_global_param)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Global_Param_global_param> GetAll()
        {
            return this.dict;
        }

        public Global_Param_global_param GetOne()
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
	public partial class Global_Param_global_param: ProtoObject, IConfig
	{
		/// <summary>主键ID</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>键值</summary>
		[ProtoMember(2)]
		public int value { get; set; }

	}
}
