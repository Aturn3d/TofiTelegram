using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States;
using Bot.Services.States.Base;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot;
using User = Bot.Model.User;

namespace Bot.Services
{
    public interface ITelegramBotService
    {
        Task HandleUpdate(Update update);
        //for Debug Purpose

    }


    public class TelegramBotService: ITelegramBotService
    {
        private IUserService userService;
        private State _state;
        private Update _update;

        internal User User { get; private set; }
        internal ITelegramBotClient Bot { get; private set; }
        
       
        public TelegramBotService(IUserService userService, IBotFactory botFactory)
        {
            this.userService = userService;
            Bot = botFactory.GetTelegramBot();
        }
        
        public async Task HandleUpdate(Update update)
        {
            try {
                this._update = update;
                User = GetUser(GetUserIdFromUpdate());
                _state = StateFactory.GetState((StatesTypes)User.ChatState);
                await _state.HandleUpdate(this, _update);
            }
            finally {
                User.ChatState = (int)_state.StateTypesId;
                userService.CreateOrUpdateUser(User);
            }
        }

        internal void SetState(State state)
        {
            this._state = state;
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
            return userService.GetByTelegramUserId(userId) ?? new User()
            {
                ChatId = GetChatId(),
                UserId = userId
            };
        }
    }
}
