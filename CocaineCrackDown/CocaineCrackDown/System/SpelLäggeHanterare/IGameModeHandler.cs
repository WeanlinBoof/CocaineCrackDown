namespace CocaineCrackDown.System.SpelLäggeHanterare {

    public interface IGameModeHandler {

        void Setup(GameSettings settings);

        bool WeHaveAWinner();
    }
}