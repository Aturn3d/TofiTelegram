using System;

namespace Bot.DAL.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        BotEntities Context { get; }
    }
}
