// 数值及增幅类型
public enum NumericType
{
	None = 0,
	#region Excel配置

	hp = 1,  //当前生命
	max_hp = 2,  //生命上限
	max_hp_rate = 3,  //生命上限%
	f_max_hp = 4,  //最终生命上限

	atk = 5,  //攻击力
	atk_rate = 6,  //攻击力%
	f_atk = 7,  //最终攻击力
	atk_param = 8,//攻击参数

	def = 10,  //防御力
	def_rate = 11,  //防御力%
	f_def = 12,  //最终防御力

	acc_rate = 15,  //命中率%
	dod_rate = 16,  //闪避%
	acc = 17,//命中
	dod = 18,//闪避

	cri_rate = 20,  //暴击%
	cri_res_rate = 21,  //暴击抵抗%
	cri_dam_rate = 22,  //暴击伤害%
	cri = 23,//暴击
	cri_res = 24,//暴击抵抗

	move_speed = 25,  //移动速度
	move_speed_rate = 26,  //移动速度%
	f_move_speed = 27,  //最终移动速度

	heal = 30,  //治疗强度
	heal_rate = 31,  //治疗强度%
	f_heal = 32,  //最终治疗强度

	dam1_rate = 35,  //伤害提升1%
	dam2_rate = 36,  //伤害提升2%
	dam3_rate = 37,  //伤害提升3%

	dam1_res_rate = 40,  //伤害减少1%
	dam2_res_rate = 41,  //伤害减少2%
	dam3_res_rate = 42,  //伤害减少3%

	extra_dam1_rate = 45,  //受到额外伤害1%
	extra_dam2_rate = 46,  //受到额外伤害2%
	extra_dam3_rate = 47,  //受到额外伤害3%

	heal_effect1_rate = 50,  //治疗效果1%
	heal_effect2_rate = 51,  //治疗效果2%
	be_heal_effect1_rate = 52,  //受治疗效果1%
	be_heal_effect2_rate = 53,  //受治疗效果2%

	gun_dam1_rate = 60,  //枪械伤害1%
	fire_dam1_rate = 61,  //火伤害1%
	ice_dam1_rate = 62,  //冰伤害1%
	flash_dam1_rate = 63,  //电伤害1%
	dark_dam1_rate = 64,  //暗伤害1%
	light_dam1_rate = 65,  //光伤害1%
	phy_dam1_rate = 66,  //物理伤害1%

	gun_dam2_rate = 70,  //枪械伤害2%
	fire_dam2_rate = 71,  //火伤害2%
	ice_dam2_rate = 72,  //冰伤害2%
	flash_dam2_rate = 73,  //电伤害2%
	dark_dam2_rate = 74,  //暗伤害2%
	light_dam2_rate = 75,  //光伤害2%
	phy_dam2_rate = 76,  //物理伤害2%

	gun_atk = 80,  //枪械攻击力
	fire_atk = 81,  //火攻击力
	ice_atk = 82,  //冰攻击力
	flash_atk = 83,  //电攻击力
	dark_atk = 84,  //暗攻击力
	light_atk = 85,  //光攻击力
	phy_atk = 86,  //物理攻击力

	gun_dam_res_rate = 90,  //枪械伤害抵抗%
	fire_dam_res_rate = 91,  //火伤害抵抗%
	ice_dam_res_rate = 92,  //冰伤害抵抗%
	flash_dam_res_rate = 93,  //电伤害抵抗%
	dark_dam_res_rate = 94,  //暗伤害抵抗%
	light_dam_res_rate = 95,  //光伤害抵抗%
	phy_dam_res_rate = 96,  //物理伤害抵抗%

	beat_back_res_rate = 100,  //击退抵抗%
	tow_res_rate = 101,  //牵引抵抗%


	hp_dam_rate = 110,  //对血量高于X的怪物造成伤害%
	lv_dam_rate = 111,  //局内前X级攻击造成伤害%
	boss_dam_rate = 112,  //对精英和首领造成伤害%
	close_range_dam_rate = 113,  //对近距离的目标造成伤害%
	debuff_dam_rate = 114,  //对负面状态目标造成伤害%
	blast_dam_rate = 115,  //爆炸伤害%
						   //blast_range_rate = 116,  //爆炸范围%

	skill_cd1_rate = 130,  //技能冷却1%
	skill_cd2_rate = 131,  //技能冷却2%

	debuff_time_rate = 135,  //添加负面状态持续时间%

	burning_res_rate = 140,  //燃烧抵抗%

	frostbite_res_rate = 141,  //冻伤抵抗%"
	curse_res_rate = 142,  //诅咒抵抗%
	electric_res_rate = 143,  //感电抵抗%
	stun_res_rate = 144,  //眩晕抵抗%
	freeze_res_rate = 145,  //冻结抵抗%
	chaos_res_rate = 146,  //混乱抵抗%
	retard_res_rate = 147,  //减速抵抗%
	blind_res_rate = 148,  //失明抵抗%
	disability_res_rate = 149,  //残废抵抗%
	fragile_res_rate = 150,  //易伤抵抗%

	fatal_burning_dam_res_rate = 155,  //致命燃烧伤害抵抗%

	anger = 160,  //
	f_anger_max = 161,  //
	hp_recover = 162,  //每5秒生命回复

	f_gun_atk = 180,//最终枪械攻击力
	f_fire_atk = 181,//最终火攻击力
	f_ice_atk = 182,//最终冰攻击力
	f_flash_atk = 183,//最终电攻击力
	f_dark_atk = 184,//最终暗攻击力
	f_light_atk = 185,//最终光攻击力
	f_phy_atk = 186,//最终物理攻击力

	#endregion
}
