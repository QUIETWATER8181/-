using Modding;
using System.Reflection;

namespace HollowAll
{
    /// <summary>
    /// HollowAll - 可装备所有护符
    /// 原理：游戏通过 PlayerData.GetInt("charmCost_N") 读取每个护符的占位，
    /// 我们拦截该读取，对所有 charmCost_* 字段返回 0，即所有护符不占护符槽，
    /// 从而可以在有限的护符槽内装备任意数量的护符。
    /// </summary>
    public class HollowAll : Mod, ITogglableMod
    {
        /// <summary>当前已加载的模组实例。</summary>
        public static HollowAll Instance;

        /// <summary>获取程序集版本号。</summary>
        public override string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>模组初始化：注册读取钩子。</summary>
        public override void Initialize()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
            this.Log("HollowAll 已启用：所有护符占位已设置为 0，可装备任意护符");
            ModHooks.GetPlayerIntHook += GetInt;
        }

        /// <summary>卸载模组：移除钩子，恢复原版护符占位。</summary>
        public void Unload()
        {
            ModHooks.GetPlayerIntHook -= GetInt;
            Instance = null;
            this.Log("HollowAll 已停用：护符占位已恢复原版");
        }

        /// <summary>
        /// 拦截玩家整数数据的读取。当读取到护符占位字段（charmCost_N）时，
        /// 一律返回 0；其余字段保持原值不变。
        /// </summary>
        private int GetInt(string name, int orig)
        {
            if (!string.IsNullOrEmpty(name) && name.StartsWith("charmCost_"))
            {
                return 0;
            }

            return orig;
        }
    }
}