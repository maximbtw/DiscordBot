using DiscordExcel;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class BotCommand : BaseCommandModule
    {
        [Command("Roll")]
        [Description("Рандомит число")]
        public async Task Roll(CommandContext ctx,
            [Description("Минимальное значаение")] int minValue = 0,
            [Description("Максимальное значаение")] int maxValue = 101)
        {
            var number = CommandAsset.GetRandom(minValue, maxValue).ToString();

            var msg1 = await ctx.RespondAsync(number);
            for (int i = 0; i < 5; i++)
            {
                number = CommandAsset.GetRandom(minValue, maxValue).ToString();
                await msg1.ModifyAsync(number);
                System.Threading.Thread.Sleep(100);
            }
        }

        [Command("Roll")]
        [Description("Рандомный вариант")]
        public async Task Roll(CommandContext ctx,
            [Description("Варианты списка")] params string[] letters)
        {
            var option = letters[CommandAsset.GetRandom(0, letters.Length)];

            await ctx.Channel.SendMessageAsync("Я думаю.. Это: " + option).ConfigureAwait(false);
        }

        [Command("BigRoll")]
        [Description("Список пользователей в случайном порядке")]
        public async Task RollWithUsers(CommandContext ctx)
        {
            var users = ctx.Member.VoiceState?
                                  .Channel.Users
                                  .Select(x => x.DisplayName)
                                  .ToArray();
            if (users == null) return;
            CommandAsset.Shufle(users);

            string text = string.Empty;
            for (int i = 0; i < users.Length; i++)
                text += (i + 1).ToString() + ": " + users[i] + "\n";

            await ctx.Channel.SendMessageAsync(text).ConfigureAwait(false);
        }


        [Command("Avatar")]
        [Description("Возвращает аватар пользователя")]
        public async Task GiveAvatar(CommandContext ctx, 
            [Description("Ссылка на пользователя")] DiscordUser discordUser = null)
        {
            string url = discordUser == null ? ctx.User.AvatarUrl : discordUser.AvatarUrl;
            if (url == null) url = "У пользователя нет аватара";
            await ctx.Channel.SendMessageAsync(url).ConfigureAwait(false);
        }


        [Command("Tilibom")]
        [Description("Секретное сообщение")]
        public async Task MessageTiliBom(CommandContext ctx,
            [Description("Пользователь который получит сообщение")] DiscordMember discordMember = null)
        {
            if (discordMember == null) discordMember = ctx.Member;
            var message = discordMember.DisplayName+ ", тили-тили бом, закрой глаза скорее";
            await discordMember.SendMessageAsync(message).ConfigureAwait(false);
        }

        [Command("Emoji")]
        [Description("Случайный смайлик сервера")]
        public async Task GetRandomEmoji(CommandContext ctx)
        {;
            var emojis = ctx.Guild.Emojis.Select(x => x.Value).ToArray();

            var message = (emojis.Length == 0) 
                ? "Нет доступных смайликов" 
                : emojis[CommandAsset.GetRandom(0, emojis.Length)];

            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }

        [Command("Расп")]
        [Description("Расписание на ближайший день")]
        public async Task GetTimetableDay(CommandContext ctx,
            [Description("День: \"Вчера\" \"Сегодня\" \"Завтра\"")] string day = "сегодня",
            [Description("Группа")] string groupName = "бст1902")
        {
            var dayOfWeak = DayOfTheWeak.GetDayOfTheWeak(day);
            if (string.IsNullOrEmpty(dayOfWeak)) return;
            if (dayOfWeak.ToLower().Equals("выходной"))
            {
                await ctx.Channel.SendMessageAsync(dayOfWeak).ConfigureAwait(false);
                return;
            }

            var message = Timetable.GetMessageTimetable(groupName,dayOfWeak);
            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }

        [Command("ФуллРасп")]
        [Description("Полное расписание на неделю")]
        public async Task GetTimetable(CommandContext ctx,
            [Description("Группа")] string groupName = "бст1902")
        {
            var message = Timetable.GetMessageTimetable(groupName);
            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }

        [Command("Неделя")]
        [Description("Неделя с начала семестра")]
        public async Task GetWeak(CommandContext ctx)
        {
            var message = Weak.GetWeak();
            message += message.ToLower().Equals("чётная") ? " - нижняя" : " - верхняя";
            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }

        [Command("ДеньНедели")]
        [Description("День недели")]
        public async Task GetDay(CommandContext ctx, string day = "сегодня")
        {
            var dayOfWeak = DayOfTheWeak.GetDayOfTheWeak(day);
            if (string.IsNullOrEmpty(dayOfWeak)) dayOfWeak = "Не найденно";
            await ctx.Channel.SendMessageAsync(dayOfWeak).ConfigureAwait(false);
        }

        [Command("Анекдот")]
        [Description("Случайный анекдот")]
        public async Task GetRandomAnekdot(CommandContext ctx)
        {
            var number = CommandAsset.GetRandom(0, Anekdot.Anekdots.Count);
            await ctx.Channel.SendMessageAsync(Anekdot.Anekdots[number]).ConfigureAwait(false);
        }

        [Command("Возраст")]
        [Description("Возраст пользователя на сервере")]
        public async Task GetUserAgeOnServer(CommandContext ctx, DiscordMember discordMember)
        {  
            var days = Weak.GetAge(discordMember.JoinedAt.DateTime);

            var message = $"{discordMember.DisplayName} на сервере уже: {days} дней" +
                $" \nПрисоединился к нам: {discordMember.JoinedAt.DateTime}";
            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }
    }
}
