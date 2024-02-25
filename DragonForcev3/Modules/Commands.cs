using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;

namespace DragonForcev3.Modules
{
    public  class Commands :  ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping() {
            await ReplyAsync("Pong");
        }

        //comando de banir
        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "Você não tem permissão ``TOMA-LHE BAN``!")]
        public async Task BanMember(IGuildUser user = null, [Remainder] string reason = null) {
            
            if (user == null) await ReplyAsync("Por favor, especifique o usuário!");

            if (reason == null) reason = "Não especificado";

            await Context.Guild.AddBanAsync(user, 1, reason);

            var EmbedBuilder = new EmbedBuilder()
                .WithDescription($":white_check_mark: {user.Mention} foi banidoz\n por {reason}")
                .WithFooter(footer => {
                    footer
                    .WithText("Usuário banido log")
                    .WithIconUrl("https://i.imgur.com.6Bi17B3.png");
                });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }
    }
}
