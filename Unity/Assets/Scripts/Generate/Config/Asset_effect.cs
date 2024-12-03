using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Asset_effectCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Asset_effect");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Asset_effect> dict = new Dictionary<int, Asset_effect>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Asset_effect> list = new List<Asset_effect>();
		
        
        public void Merge(object o)
        {
            Asset_effectCategory s = o as Asset_effectCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Asset_effect config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Asset_effect Get(int id)
        {
            this.dict.TryGetValue(id, out Asset_effect item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Asset_effect)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Asset_effect)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Asset_effect> GetAll()
        {
            return this.dict;
        }

        public Asset_effect GetOne()
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
	public partial class Asset_effect: ProtoObject, IConfig
	{
		/// <summary>特效ID</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>动画类型</summary>
		[ProtoMember(2)]
		public int type { get; set; }
		/// <summary>资源路径</summary>
		[ProtoMember(3)]
		public string asset { get; set; }
		/// <summary>资源名</summary>
		[ProtoMember(4)]
		public string name { get; set; }
		/// <summary>绑定朝向</summary>
		[ProtoMember(5)]
		public int bind_rotation { get; set; }
		/// <summary>朝向类型</summary>
		[ProtoMember(6)]
		public int rotation_type { get; set; }
		/// <summary>旋转偏移</summary>
		[ProtoMember(7)]
		public int rotate_angle { get; set; }
		/// <summary>绑定位置</summary>
		[ProtoMember(8)]
		public int bind_position { get; set; }
		/// <summary>位置偏移</summary>
		[ProtoMember(9)]
		public int[] offset { get; set; }
		/// <summary>缩放</summary>
		[ProtoMember(10)]
		public int scale { get; set; }
		/// <summary>特效播放速度系数</summary>
		[ProtoMember(11)]
		public int play_speed { get; set; }
		/// <summary>备注</summary>
		[ProtoMember(12)]
		public string 备注 { get; set; }

	}
}
