# HollowAll - 空洞骑士"可装备所有护符"模组

一个空洞骑士模组：启用后，**所有护符的占位（护符槽消耗）都将变为 0**，你可以在有限的护符槽内装备任意数量的护符。

## 实现原理

游戏通过 `PlayerData.GetInt("charmCost_N")`（N = 1~40）读取每个护符的占位。本模组拦截该读取（`ModHooks.GetPlayerIntHook`），对所有 `charmCost_*` 字段返回 `0`，从而所有护符都不占护符槽；其余玩家数据完全不受影响。

- 对游戏/存档零改动，纯运行时钩子
- 实现 `ITogglableMod`，可在模组菜单中随时停用，停用后立即恢复原版占位

## 构建

需要 .NET SDK（本机用 dotnet 10 构建验证通过）。csproj 默认引用 `.temp/hk-api12620/api/OutputFinal` 下的修补版游戏程序集（含 Modding API），若路径不同请修改 `HollowKnightRefs` 指向游戏 `hollow_knight_Data/Managed`。

```sh
dotnet build HollowAll.csproj -c Release
```

## 安装

将编译产物 `bin/Release/net472/HollowAll.dll` 放入游戏目录：

```
hollow_knight_Data/Managed/Mods/HollowAll/HollowAll.dll
```

启动游戏即可，`ModLog.txt` 会出现 `HollowAll 已启用` 日志。

## 兼容性

- 游戏版本：1.5.12620（配合 Modding API v78）
- 依赖 Modding API（Mod、ModHooks、ITogglableMod）