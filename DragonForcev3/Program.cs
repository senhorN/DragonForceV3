using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace DragonForcev3
{
    internal class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();
        
        #region metodos privados
        private DiscordSocketClient _client;
        private CommandService _commands;
        private  IServiceProvider _services;
        #endregion


        public async Task RunBotAsync() { 
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string token = "MTIxMTEwNTU2NTIJNr4vvdoBL93NIND345Y7nd9oOKQLKEDcsj0KOc1rP8MfPr4vvdoBLQMc9K3egsa4Y";

            _client.Log += _client_Log;

            await ResgisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await Task.Delay(-1);

        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);

            return Task.CompletedTask;
        }

        public async Task ResgisterCommandsAsync() {

            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async  Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            
            if (message.Author.IsBot) return;

            int argsPos = 0;
            if (message.HasStringPrefix("!", ref argsPos)) {
                
                var result = await _commands.ExecuteAsync(context, argsPos, _services);

                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);

                if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}
