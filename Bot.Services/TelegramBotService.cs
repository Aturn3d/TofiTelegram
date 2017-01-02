using System;
using System.Threading.Tasks;
using Bot.Services.Common;
using Bot.Services.States.Base;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = Bot.Model.User;

namespace Bot.Services
{
    public interface ITelegramBotService
    {
        Task HandleUpdate(Update update);
        //for Debug Purpose
    }


    public class TelegramBotService : ITelegramBotService
    {
        private readonly IUserService userService;
        private State _state;
        private Update _update;

        internal User User { get; private set; }
        internal ITelegramBotClient Bot { get; }


        public TelegramBotService(IUserService userService, IBotFactory botFactory)
        {
            this.userService = userService;
            Bot = botFactory.GetTelegramBot();
        }

        public async Task HandleUpdate(Update update)
        {
            _update = update;
            var userId = GetUserIdFromUpdate();
            var cachedUser = UsersCache.GetOrAdd(userId);
            if (cachedUser.IsProcessing) {
                await Bot.SendTextMessageAsync(userId, "Ваш предыдущий запрос обрабатывается, ожидайте");
            }
            else {
                try {
                    cachedUser.IsProcessing = true;
                    User = GetUser(userId);
                    await Bot.SendChatActionAsync(User.ChatId, ChatAction.Typing);
                    _state = State.GetState((StatesTypes) User.ChatState, this, update);
                    await _state.HandleUpdate();
                }
                finally {
                    User.ChatState = (int) _state.StateTypesId;
                    userService.CreateOrUpdateUser(User);
                    cachedUser.IsProcessing = false;
                }
            }
        }

        internal void SetState(State state)
        {
            _state = state;
            state.PrepareState();
        }

        private long GetChatId()
        {
            return _update.Message.Chat.Id;
        }

        private int GetUserIdFromUpdate()
        {
            switch (_update.Type) {
                case UpdateType.MessageUpdate:
                    return _update.Message.From.Id;
                case UpdateType.InlineQueryUpdate:
                    return _update.InlineQuery.From.Id;
                case UpdateType.ChosenInlineResultUpdate:
                    return _update.ChosenInlineResult.From.Id;
                case UpdateType.CallbackQueryUpdate:
                    return _update.CallbackQuery.From.Id;
                case UpdateType.EditedMessage:
                    return _update.EditedMessage.From.Id;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private User GetUser(int userId)
        {
            return userService.GetByTelegramUserId(userId) ?? new User
            {
                ChatId = GetChatId(),
                UserId = userId
            };
        }
    }
}
