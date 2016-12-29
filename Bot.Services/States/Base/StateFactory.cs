namespace Bot.Services.States.Base
{
    internal static class StateFactory
    {
        public static State GetState(StatesTypes stateTypesId)
        {
            switch (stateTypesId) {
                case StatesTypes.InitialState:
                   return new InitialState();
                case StatesTypes.Exchange:
                    return new ExchangeState();
                default:
                    return DefaulState;
            }
        }

        public static State DefaulState => new InitialState();
    }

    internal enum StatesTypes
    {
        InitialState = 0,
        Exchange
    }
}
